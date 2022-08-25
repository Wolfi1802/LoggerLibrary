using System;

namespace LoggerLibrary
{
    public interface ILogger
    {
        string GetFilePathName();
        void WriteLog(string message, string methodName);
        void WriteException(string message, string methodName, Exception ex = null);
        void WriteEnhanced(string message, string methodName, Exception ex = null);
        void WriteDebug(string message, string methodName, Exception ex = null);
    }
}
