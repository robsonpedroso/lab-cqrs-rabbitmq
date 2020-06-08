using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageNotifier : INotifier
    {
        /// <summary>
        /// 
        /// </summary>
        public string CorrelationId { get; set; }
        private readonly ILog log;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public MessageNotifier(ILog log)
        {
            this.log = log;
            CorrelationId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public void Error(Exception exception)
        {
            Send(exception.Message, level: Level.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(object message, Exception exception)
        {
            Send(message, exception, Level.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Warn(object message)
        {
            Send(message, level: Level.Warn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(object message, Exception exception)
        {
            Send(message, exception, Level.Warn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            Send(message, level: Level.Info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(object message, Exception exception)
        {
            Send(message, exception, Level.Info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="level"></param>
        protected void Send(object message, Exception exception = null, Level level = null)
        {
            Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(CorrelationId))
                        CorrelationId = Guid.NewGuid().ToString();

                    ThreadContext.Properties["correlation_id"] = CorrelationId;

                    switch (level.Name)
                    {
                        case "ERROR":
                            log.Error(message, exception);
                            break;

                        case "WARN":
                            log.Warn(message, exception);
                            break;

                        case "INFO":
                        default:
                            log.Info(message, exception);
                            break;
                    }
                }
                catch
                {
                    //do not stop
                }
            });
        }
    }
}
