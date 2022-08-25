using System;
using System.Diagnostics;

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
            this.GlobalLogQueInstance.AddLogToLogQue(this.LoggerName, this.Formate(message, StackTreeMethod));
        }

        public void WriteException(string message, string StackTreeMethod, Exception exPara = null)
        {
            this.GlobalLogQueInstance.AddLogToLogQue(this.LoggerName, this.Formate(message, StackTreeMethod, exPara));
        }

        public void WriteEnhanced(string message, string StackTreeMethod, Exception exPara = null)
        {
            message += new StackTrace().GetFrames();
            this.GlobalLogQueInstance.AddLogToLogQue(this.LoggerName, this.Formate(message, StackTreeMethod, exPara));
        }

        public void WriteDebug(string message, string StackTreeMethod, Exception exPara = null)
        {
            this.GlobalLogQueInstance.AddLogToLogQue(this.LoggerName, this.Formate(message, StackTreeMethod, exPara));
        }

        private string Formate(string message, string StackTreeMethod, Exception exPara = null)
        {
            try
            {
                string formatedMessage = $"T\t{DateTime.Now}\r\nM\t{StackTreeMethod.Replace("\n", string.Empty)}\r\nI\t{message}\r\n";

                if (exPara != null)
                    formatedMessage += $"E\t{exPara}\r\n";

                return formatedMessage;
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.TriggerMessageCaller(ex);
                return null;
            }
        }
    }
}
