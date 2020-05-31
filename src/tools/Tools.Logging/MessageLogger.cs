using CQRS.Tools.Utils.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Tools.Logging
{
    public class MessageLogger : MessageLoggerBase
    {
        public MessageLogger(HttpRequest request, string loggerName) : base(loggerName)
        {
            IpAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        public MessageLogger(HttpRequest request)
            : base() => IpAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString();

        #region "  Request  "

        public async Task LogRequestAsync(HttpRequest request)
        {
            RequestLine = $"{request.Method} {request.Path} {request.Headers["Authorization"]} { GetCustomHeader(request.Headers)}".Trim();

            if (!request.Headers.ContainsKey("CorrelationId"))
                request.Headers.Add("CorrelationId", new[] { CorrelationId.ToString() });

            Body = await new StreamReader(request.Body).ReadToEndAsync();
            Body = Body.TrimJson();

            base.LogRequestSync();
        }

        #endregion

        #region "  Response  "

        public async Task LogResponseAsync(HttpResponse response)
        {
            var request = response.HttpContext.Request;

            RequestLine = $"{request.Method} {request.Path}";

            StatusCode = response.StatusCode;

            Body = await HandleResponseMessageAsync(response);
            Body = Body.TrimJson();

            base.LogResponseSync();
        }

        #endregion

        #region "  Private  "

        public async Task<string> HandleResponseMessageAsync(HttpResponse response)
        {
            if (response.StatusCode >= 200 && response.StatusCode <= 299)
                return await new StreamReader(response.Body).ReadToEndAsync();

            var requestEx = response.HttpContext.Items["Exception"] as Exception;

            if (!requestEx.IsNull())
            {
                if (!response.HttpContext.Items["IsHandledError"].AsBool())
                    response.StatusCode = StatusCodes.Status500InternalServerError;

                return requestEx.TrimMessage();
            }

            var bodyString = await new StreamReader(response.Body).ReadToEndAsync();

            if (bodyString.IsNullOrWhiteSpace())
                return string.Empty;

            try
            {
                var error = JObject.Parse(bodyString);

                var exceptionMessage = error["ExceptionMessage"];
                var stackTrace = error["StackTrace"];

                if (!exceptionMessage.IsNull() && !stackTrace.IsNull())
                {
                    var message = new
                    {
                        message = exceptionMessage.ToString(),
                        traces = stackTrace.ToString()
                    };

                    return message.ToJsonString();
                }
                else
                    return error.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetCustomHeader(IHeaderDictionary header)
        {
            try
            {
                var startWith = header.Where(h => h.Key.StartsWith("X-"));

                var extraHeaders = string.Join(" ", startWith.Select(h => string.Format("{0}={1}", h.Key, string.Join(" ", h.Value).Trim()))); //get extras headers

                return extraHeaders.Trim();
            }
            catch
            {
                // Always return all zeroes for any failure (my calling code expects it)
            }

            return string.Empty;
        }


        #endregion
    }
}