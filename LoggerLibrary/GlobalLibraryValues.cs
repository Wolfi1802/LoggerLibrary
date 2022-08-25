using System;
using System.Windows.Forms;

namespace LoggerLibrary
{
    internal static class GlobalLibraryValues
    {
        private static string default_Path;
        private static bool AutoCleanUp;
        private static short AutoCleanUpDays;
        private static int LogSize;
        private static string fileType;

        static GlobalLibraryValues()
        {
            default_Path = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\";
            AutoCleanUp = false;
            AutoCleanUpDays = 7;//nhe Woche sollte als default ausreichen
            LogSize = 80000000; // 80 MB | [TS] muss so
            fileType = ".txt";
        }

        internal static string GetPath()
        {
            return default_Path;
        }

        internal static void SetPath(string path)
        {
            default_Path = path;
        }

        internal static bool GetAutoCleanUpEnabled()
        {
            return AutoCleanUp;
        }

        internal static void SetAutoCleanUp(bool value)
        {
            AutoCleanUp = value;
        }

        internal static short GetAutoCleanUpDays()
        {
            return AutoCleanUpDays;
        }

        internal static void SetAutoCleanUpDays(short value)
        {
            AutoCleanUpDays = value;
        }

        internal static int GetLogSize()
        {
            return LogSize;
        }

        internal static void SetLogSize(int value)
        {
            LogSize = value;
        }

        internal static string GetFileType()
        {
            return fileType;
        }

        internal static void SetFileType(string path)
        {
            fileType = path;
        }

        internal static void TriggerMessageCaller(string message)
        {
            MainClass.MessageCaller?.Invoke(MessageCallerEnum.Message, $"{DateTime.Now} | {message}");
        }

        internal static void TriggerMessageCaller(Exception message)
        {
            MainClass.MessageCaller?.Invoke(MessageCallerEnum.Exception, $"{DateTime.Now} | {message.Message}");
        }

        internal static void TriggerMessageCallerForLog(string message)
        {
            MainClass.MessageCaller?.Invoke(MessageCallerEnum.Log, $"{DateTime.Now} | {message}");
        }

    }
}
