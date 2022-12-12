using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LoggerLibrary
{
    internal sealed class LogQue
    {
        private static readonly Dictionary<string, List<LogModell>> logQue = new Dictionary<string, List<LogModell>>();
        private readonly object loggingQueLockObject = new object();
        private Thread logThread;
        private bool LogActive = false;
        private bool logThreadEnable = true;
        private static LogQue instance;

        internal static LogQue GetInstance()
        {
            if (instance == null)
                instance = new LogQue();

            return instance;
        }

        private LogQue()
        {
            try
            {
                if (GlobalLibraryValues.GetAutoCleanUpEnabled())
                    this.CleanLogDirectory();

                this.PrepareLogFilePath();
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseExceptionEvent(ex);
            }
        }

        ~LogQue()
        {
            this.logThreadEnable = false;
            this.LogActive = false;
        }

        internal void StartLogThread()
        {
            try
            {
                if (this.logThread == null)
                {
                    this.LogActive = true;
                    this.logThread = new Thread(LogWorker);
                    this.logThread.Name = $"{DateTime.Now}|{nameof(LogQue)}|{nameof(StartLogThread)}";
                    this.logThread.IsBackground = true;
                    this.logThread.Start();

                    GlobalLibraryValues.RaiseMessageAction($"{DateTime.Now}|{nameof(LogQue)}|{nameof(StartLogThread)} has started");
                }
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseExceptionEvent(ex);
            }
        }

        internal void AddLogToLogQue(string logfileName, LogModell model)
        {
            try
            {
                GlobalLibraryValues.LogsCaller?.Invoke(model);
                GlobalLibraryValues.LogList.Add(model);

                if (this.logThread != null)
                {
                    lock (this.loggingQueLockObject)
                    {
                        if (!logQue.ContainsKey(logfileName))
                            logQue.Add(logfileName, new List<LogModell>());

                        logQue[logfileName].Add(model);
                        GlobalLibraryValues.RaiseMessageAction($"Add Message to [{nameof(AddLogToLogQue)}] [{logfileName}] ");
                    }
                }
                else
                {
                    this.StartLogThread();
                    this.AddLogToLogQue(logfileName, model);
                }
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseMessageAction(ex.Message);
            }
        }

        internal bool CleanLogDirectory()
        {
            try
            {
                var filesToDelete = this.GetFilesToDelete();

                this.DeleteFiles(filesToDelete);

                return true;
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseExceptionEvent(ex);
                return false;
            }
        }

        private void PrepareLogFilePath()
        {
            try
            {
                if (!Directory.Exists(GlobalLibraryValues.GetPath()))
                {
                    Directory.CreateDirectory(GlobalLibraryValues.GetPath());
                }
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseMessageAction(ex.Message);
            }
        }

        private void WriteLogIntoQue(List<LogModell> errorQue, string logfileName)
        {
            try
            {
                if (errorQue.Count <= 0)
                    return;

                Helper.SaveText(errorQue[0], GlobalLibraryValues.GetPath(), this.GetLogName(logfileName));

                errorQue.RemoveAt(0);
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseMessageAction(ex.Message);
            }
        }

        private void LogWorker()
        {
            try
            {
                Thread.Sleep(1000);

                while (this.logThreadEnable)
                {
                    int remainingEntries = 0;

                    lock (this.loggingQueLockObject)
                    {
                        foreach (KeyValuePair<string, List<LogModell>> item in logQue)
                        {
                            List<LogModell> que = item.Value;
                            this.WriteLogIntoQue(que, item.Key);
                            remainingEntries += item.Value.Count;
                        }
                    }

                    if (remainingEntries == 0 && !this.LogActive)
                    {
                        GlobalLibraryValues.RaiseMessageAction($"{nameof(LogWorker)} ends with 0 remaining Logs");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseMessageAction(ex.Message);
            }
        }

        private string GetLogName(string logfileName)
        {
            try
            {
                return $"{logfileName}{GlobalLibraryValues.GetFileType()}";
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseMessageAction(ex.Message);
                return null;
            }
        }

        private string[] GetLogFiles()
        {
            try
            {
                return Directory.GetFiles(GlobalLibraryValues.GetPath());
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseMessageAction(ex.Message);
                return null;
            }
        }

        private bool DeleteFiles(List<FileInfo> filesToDelete)
        {
            int count = 0;

            try
            {
                if (filesToDelete.Count > 0)
                {
                    foreach (FileInfo fileToDelete in filesToDelete)
                    {
                        File.Delete(fileToDelete.FullName);
                        count++;
                    }
                }

                GlobalLibraryValues.RaiseMessageAction($"{filesToDelete.Count} Logs delete done");

                return true;
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseExceptionEvent(ex);
                return false;
            }
        }

        private List<FileInfo> GetFilesToDelete()
        {
            try
            {
                List<FileInfo> filesToDelete = new List<FileInfo>();

                string[] logFilesArray = this.GetLogFiles();

                foreach (string logFile in logFilesArray)
                {
                    FileInfo fileInfo = new FileInfo(logFile);

                    if ((DateTime.Now - fileInfo.LastWriteTime).TotalDays >= GlobalLibraryValues.GetAutoCleanUpDays())
                    {
                        filesToDelete.Add(fileInfo);
                    }
                }

                GlobalLibraryValues.RaiseMessageAction($"Found {filesToDelete.Count} Logs to delete");

                return filesToDelete;
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseExceptionEvent(ex);
                return null;
            }
        }
    }
}
