using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace LoggerLibrary
{
    internal class MainClass : LoggerBase
    {
        private static MainClass instance = null;


        internal MainClass() : base(nameof(MainClass))
        {

        }

        public static MainClass GetInstance()
        {
            if (instance == null)
                instance = new MainClass();

            return instance;
        }


        private static void Main(string[] args)
        {
#if DEBUG
            GlobalLibraryValues.LibaryCaller += (callerType, message) =>
            {
                //Debug.WriteLine($"Library Log : {callerType} - {message}");
            };

            GlobalLibraryValues.LogsCaller += (log) =>
            {
                //Debug.WriteLine($"Receive Log : {log.LogType}");
            };
            TestLogs();

            Console.ReadLine();
#endif
        }

        private static void TestLogs()
        {
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
        }

        private void Log(string log, bool enhancedLog)
        {
            var StackTreeMethod = this.GetStacktreeMethodName();

            if (enhancedLog)
                base.WriteEnhanced(log, StackTreeMethod);
            else
                base.WriteLog(log, StackTreeMethod);
        }

        internal string GetStacktreeMethodName()
        {
            return Environment.StackTrace.Split("\r".ToCharArray())[3];//[TS]call out of mainclass => index 4
        }
    }
}
