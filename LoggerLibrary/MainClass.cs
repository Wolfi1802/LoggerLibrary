using System;
using System.Diagnostics;
using System.Threading;

namespace LoggerLibrary
{
    internal class MainClass : LoggerBase
    {
        private static MainClass instance = null;
        public static Action<MessageCallerEnum, string> MessageCaller { private set; get; }

        internal MainClass() : base(nameof(MainClass))
        {

        }

        public static MainClass GetInstance()
        {
            if (instance == null)
                instance = new MainClass();

            return instance;
        }

        public void Log(string log, bool enhancedLog)
        {
            var StackTreeMethod = this.GetStacktreeMethodName();
            base.WriteLog(log, StackTreeMethod);

            if (enhancedLog)
                base.WriteEnhanced(log, StackTreeMethod);

            GlobalLibraryValues.TriggerMessageCaller($"{log},{StackTreeMethod}");
        }

        private static void Main(string[] args)
        {
            GetInstance().PrePareLogging();
            for (int i = 0; i < 1000; i++)
            {
                GetInstance().Log("Hallo World", false);
            }

            GetInstance().Log("Hallo World", false);
            Thread.Sleep(5000);
        }

        internal override string GetStacktreeMethodName()
        {
            return Environment.StackTrace.Split("\r".ToCharArray())[3];//[TS]call out of mainclass => index 4
        }

        private void PrePareLogging()
        {
            MessageCaller += (callerType, message) =>
            {
                Debug.WriteLine($"{message}");
            };
        }
    }
}
