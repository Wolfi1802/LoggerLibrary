using System;
using System.Windows.Forms;

namespace LoggerLibrary
{
    public static class GlobalLibraryValues
    {
        /// <summary>
        /// Dieses Event kommuniziert nach außen wenn etwas im inneren passiert.
        /// </summary>
        public static Action<LogType, string> LibaryCaller { set; get; }

        /// <summary>
        /// Dieses Event kommuniziert nach außen wenn ein Log geschrieben wird.
        /// </summary>
        public static Action<LogModell> LogsCaller { set; get; }

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

        internal static void RaiseMessageAction(string message)
        {
            LibaryCaller?.Invoke(LogType.Log, $"{DateTime.Now} | {message}");
        }

        internal static void RaiseExceptionEvent(Exception message)
        {
            LibaryCaller?.Invoke(LogType.Exception, $"{DateTime.Now} | {message.Message}");
        }
    }
}
