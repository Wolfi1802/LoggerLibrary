using System;
using System.IO;

namespace LoggerLibrary
{
    internal sealed class Logger : IDisposable
    {
        private delegate void WriteMessage(string message);
        private static readonly Logger Instance = new Logger(GlobalLibraryValues.GetPath());
        private readonly object Locker = new object();
        private readonly StreamWriter Writer;
        private bool Disposed;

        private Logger(string fullFilePath)
        {
            this.Writer = new StreamWriter(fullFilePath, true);
        }

        ~Logger()
        {
            Dispose(false);
            GlobalLibraryValues.TriggerMessageCaller($"Kill {nameof(Logger)}");
        }

        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.TriggerMessageCaller(ex);
            }
        }

        internal static void Log(string message, string path = "")
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    path = GlobalLibraryValues.GetPath();

                WriteMessage action = Instance.MessageWriter;
                action.BeginInvoke(message, MessageWriteComplete, action);
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.TriggerMessageCaller(ex);
            }
        }

        private static void MessageWriteComplete(IAsyncResult iar)
        {
            try
            {
                ((WriteMessage)iar.AsyncState).EndInvoke(iar);
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.TriggerMessageCaller(ex);
            }
        }

        private void Dispose(bool disposing)
        {
            try
            {
                lock (Locker)
                {
                    if (Disposed)
                    {
                        return;
                    }

                    if (disposing)
                    {
                        if (Writer != null)
                        {
                            Writer.Dispose();
                        }
                    }

                    Disposed = true;
                }
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.TriggerMessageCaller(ex);
            }
        }

        private void MessageWriter(string message)
        {
            try
            {
                lock (Locker)
                {
                    if (!Disposed && (Writer != null))
                    {
                        Writer.WriteLine(message);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.TriggerMessageCaller(ex);
            }
        }
    }
}
