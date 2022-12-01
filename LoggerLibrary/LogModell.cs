using System;

namespace LoggerLibrary
{
    public class LogModell
    {
        public LogModell()
        {
        }

        public LogModell(string message, string stacktrace, DateTime? time, Exception ex, LogType logType)
        {
            if (!string.IsNullOrEmpty(message))
                this.Message = message;

            if (!string.IsNullOrEmpty(stacktrace))
                this.Stacktrace = stacktrace;

            if (time.HasValue)
                this.Time = time.Value;
            else
                this.Time = DateTime.Now;

            if (ex != null)
                this.Exception = ex;

            this.LogType = logType;
        }

        public Exception Exception { set; get; }

        public string Stacktrace { set; get; }

        public string Message { set; get; }

        public DateTime Time { set; get; }

        public LogType LogType { set; get; }
    }
}
