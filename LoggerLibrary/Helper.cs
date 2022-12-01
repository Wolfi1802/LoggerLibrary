using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace LoggerLibrary
{
    internal static class Helper
    {
        private static object lockObject = new object();

        internal static void SaveText(LogModell modell, string path, string fileName)
        {
            SaveText(FormateLog(modell), Path.Combine(path, fileName));
        }

        internal static void SaveText(string text, string path, string fileName)
        {
            SaveText(text, Path.Combine(path, fileName));
        }

        internal static void SaveText(string text, string path)
        {
            if (text == null)
                throw new ArgumentNullException($"{nameof(Helper)},{nameof(SaveText)}{nameof(text)} ist null");

            if (text == string.Empty)
                throw new ArgumentException($"{nameof(Helper)},{nameof(SaveText)}, Der Text darf kein leerer String sein", nameof(text));

            if (path == null)
                throw new ArgumentNullException($"{nameof(Helper)},{nameof(SaveText)},{nameof(path)} ist null");

            if (path == string.Empty)
                throw new ArgumentException($"{nameof(Helper)},{nameof(SaveText)}, Der Pfad darf kein leerer String sein", nameof(path));

            if (!ValidateParams(text, path))
                throw new Exception($"{nameof(Helper)},{nameof(SaveText)} in {nameof(ValidateParams)} ist eine Überprüfung Fehlerhaft!");

            GlobalLibraryValues.RaiseMessageAction($"Write text [{text.Length}] into {path}");

            lock (lockObject)
            {
                var path2 = new DirectoryInfo(Path.GetFullPath(path)).FullName;//Aus irgendeinem grund wird der pfad sonst umgewandelt mit falschen zeichen im pfad...

                File.AppendAllText(path2, text);
            }
        }

        internal static bool ValidateParams(string text, string path)
        {
            if (string.IsNullOrEmpty(text))
                throw new NullReferenceException($"{nameof(Helper)},{nameof(ValidateParams)},{nameof(text)} ist null oder leer");

            if (string.IsNullOrEmpty(path))
                throw new NullReferenceException($"{nameof(Helper)},{nameof(ValidateParams)},{nameof(path)} ist null oder leer");

            if (TryEnsurePathExists(path))
                return true;

            return false;
        }

        internal static bool TryEnsurePathExists(string path)
        {
            try
            {
                EnsurePathExists(path);
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseMessageAction($"{nameof(Helper)},{nameof(TryEnsurePathExists)},{ex}");
                return false;
            }
            return true;
        }

        internal static void EnsurePathExists(string path)
        {
            if (File.Exists(path))
                return;
            else
            {
                var directory = Path.GetDirectoryName(path);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    GlobalLibraryValues.RaiseMessageAction($"Create {directory}");
                }

                File.Create(path);
                GlobalLibraryValues.RaiseMessageAction($"Create {path}");
            }
        }

        /// <summary>
        /// Formatiert nach folgendem Muster:
        /// <para>Types: "\n[DateTime][string][String][Exception]"</para>
        /// <para>Names: "\n[Time][Message][Stacktrace][Exception]"</para>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string FormateLog(LogModell model)
        {
            try
            {
                string formatedMessage = $"\n[{model.Time}] [{model.Message}]";

                if(!string.IsNullOrEmpty(model.Stacktrace))
                    formatedMessage+= $" [{model.Stacktrace.Replace("\n", string.Empty)}]";

                if (model.Exception != null)
                    formatedMessage += $" [{model.Exception}]";

                return formatedMessage;
            }
            catch (Exception ex)
            {
                GlobalLibraryValues.RaiseExceptionEvent(ex);
                return null;
            }
        }
    }
}
