using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LoggerLibrary
{
    internal abstract class LoggerBase : ILogger
    {
        internal abstract string GetStacktreeMethodName();

        internal const int LOG_DAYS_TO_SAVE = 7;
        internal readonly string LoggerName;

        private LogQue GlobalLogQueInstance;

        internal LoggerBase(string LoggerName, string path = "")
        {
            this.GlobalLogQueInstance = LogQue.GetInstance();
            this.LoggerName = LoggerName;

            if (!string.IsNullOrEmpty(path))
                GlobalLibraryValues.SetPath(path);
        }


        public string GetFilePathName()
        {
            return $"{GlobalLibraryValues.GetPath()}{this.LoggerName}{GlobalLibraryValues.GetFileType()}";
        }

        public void WriteLog(string message, string StackTreeMethod)
        {
            this.GlobalLogQueInstance.AddLogToLogQue(this.LoggerName, new LogModell(message, StackTreeMethod, null, null));
        }

        public void WriteException(string message, string StackTreeMethod, Exception exPara = null)
        {
            this.GlobalLogQueInstance.AddLogToLogQue(this.LoggerName, new LogModell(message, StackTreeMethod, null, exPara));
        }

        public void WriteEnhanced(string message, string StackTreeMethod, Exception exPara = null)
        {
            message += new StackTrace().GetFrames();
            this.GlobalLogQueInstance.AddLogToLogQue(this.LoggerName, new LogModell(message, StackTreeMethod, null, exPara));
        }

        public void WriteDebug(string message, string StackTreeMethod, Exception exPara = null)
        {
            this.GlobalLogQueInstance.AddLogToLogQue(this.LoggerName, new LogModell(message, StackTreeMethod, null, exPara));
        }      
    }
}
