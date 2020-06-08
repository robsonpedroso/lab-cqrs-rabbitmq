using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Logging
{
    public interface INotifier
    {
        string CorrelationId { get; set; }
        void Info(object message);
        void Info(object message, Exception exception);
        void Warn(object message);
        void Warn(object message, Exception exception);
        void Error(Exception exception);
        void Error(object message, Exception exception);
    }
}