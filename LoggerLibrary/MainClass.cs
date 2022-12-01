using System;
using System.Collections.Generic;
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

            if (enhancedLog)
                base.WriteEnhanced(log, StackTreeMethod);
            else
                base.WriteLog(log, StackTreeMethod);

            GlobalLibraryValues.TriggerMessageCaller($"{log},{StackTreeMethod}");
        }

        private static void Main(string[] args)
        {
            GetInstance().PrePareLogging();

            for (int j = 0; j < 3; j++)
            {
                Thread thread = new Thread(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        GetInstance().Log($"Thread NR{j} - Log NR{i} Hallo World", false);
                    }
                });

                thread.Start();
                Thread.Sleep(1000);
            }


            Console.ReadLine();
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
