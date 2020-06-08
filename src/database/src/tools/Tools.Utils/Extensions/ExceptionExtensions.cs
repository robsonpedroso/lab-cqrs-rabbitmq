using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tools.Utils.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Trim message exception get erro line from StackTrace
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>        
        public static string TrimMessage(this Exception ex)
        {
            var st = new StackTrace(ex, true);
            var traces = new List<string>();

            for (int i = 0; i < st.FrameCount; i++)
            {
                var frame = st.GetFrame(i);

                if (!frame.GetFileName().IsNullOrWhiteSpace())
                    traces.Add("{0}:{1}:{2}".ToFormat(frame.GetFileName(), frame.GetFileLineNumber(), frame.GetFileColumnNumber()));
            }

            var parameters = string.Empty;

            if (ex.Data != null)
            {
                if (ex.Data.Contains("parameters"))
                    parameters = ex.Data["parameters"].ToJsonString();
            }

            var message = new
            {
                message = ex.GetBaseException().Message,
                traces = traces,
                parameters = parameters
            };

            return message.ToJsonString();
        }
    }
}
