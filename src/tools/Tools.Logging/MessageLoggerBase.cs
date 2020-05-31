using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;

namespace CQRS.Tools.Logging
{
    public abstract class MessageLoggerBase
    {
        #region "  Properties  "
        private static volatile object _lock = new object();
        private static volatile ILog _log;

        public string CorrelationId { get; protected set; }

        public string IpAddress { get; protected set; }

        public string RequestLine { get; protected set; }

        public string Body { get; protected set; }

        public int StatusCode { get; protected set; }
        public MessageLoggerBase() : this(null) { }

        public MessageLoggerBase(string loggerName)
        {
            CorrelationId = Guid.NewGuid().ToString();

            lock (_lock)
            {
                if (_log == null)
                {
                    if (string.IsNullOrWhiteSpace(loggerName))
                        _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                    else
                        _log = LogManager.GetLogger(Assembly.GetEntryAssembly(), loggerName);

                    XmlConfigurator.Configure(_log.Logger.Repository, new FileInfo("log4net.config"));
                }
            }
        }

        #endregion

        #region "  Request  "

        public virtual void LogRequestSync()
        {
            try
            {
                ThreadContext.Properties["correlation_id"] = CorrelationId;
                ThreadContext.Properties["client_ip"] = IpAddress;

                var _message = $"Request {RequestLine} {Body}";

                _log.Info(_message);
            }
            catch
            {
                //Not do anything
            }
        }

        #endregion

        #region "  Response  "

        public virtual void LogResponseSync()
        {
            try
            {
                ThreadContext.Properties["correlation_id"] = CorrelationId;
                ThreadContext.Properties["client_ip"] = IpAddress;

                var _message = $"Response {RequestLine} {Body}";

                switch (StatusCode)
                {
                    case 204: //NoContent
                    case 200: //OK
                        _log.Info(_message);
                        break;
                    case 401: //Unauthorized
                    case 403: //Forbidden
                    case 502: //BadGateway
                    case 400: //BadRequest
                        _log.Warn(_message);
                        break;
                    case 500: //InternalServerError
                        _log.Error(_message);
                        break;
                    default:
                        _log.Warn(_message);
                        break;
                }
            }
            catch
            {
                //Not do anything
            }
        }

        #endregion
    }
}
