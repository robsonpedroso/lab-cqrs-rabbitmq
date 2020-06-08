using Tools.Utils.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using System.Threading.Tasks;

namespace Tools.Logging
{
    public class MessageLoggingMiddleware
    {
        private RequestDelegate _next;

        private string _loggerName;

        public MessageLoggingMiddleware(RequestDelegate next, string loggerName)
        {
            _loggerName = loggerName;
            _next = next;
        }
        public MessageLoggingMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context)
        {
            var info = CreateInstance(context.Request, _loggerName);

            context.Request.EnableBuffering();
            await info.LogRequestAsync(context.Request);
            context.Request.Body.Position = 0;

            using (var buffer = new MemoryStream())
            {
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                await _next.Invoke(context);

                buffer.Seek(0, SeekOrigin.Begin);

                await info.LogResponseAsync(context.Response);

                buffer.Seek(0, SeekOrigin.Begin);
                await buffer.CopyToAsync(stream);
                context.Response.Body = stream;
            }
        }
        public MessageLogger CreateInstance(HttpRequest request, string loggerName)
        {
            if (!loggerName.IsNullOrWhiteSpace())
                return new MessageLogger(request, loggerName);
            else
                return new MessageLogger(request);
        }

    }
}