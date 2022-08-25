using System;
using System.IO;
using System.Text;

namespace LoggerLibrary
{
    internal  static class Helper
    {
        internal static void SaveText(string text, string path, string fileName, bool append = true)
        {
            SaveText(text, Path.Combine(path, fileName), append);
        }

        internal static void SaveText(string text, string path, bool append = true)
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

            StreamWriter writer;

            GlobalLibraryValues.TriggerMessageCaller($"Write text [{text.Length}] into {path}");
            GlobalLibraryValues.TriggerMessageCallerForLog(text);

            using (writer = new StreamWriter(path, append, Encoding.UTF8))
            {
                writer.WriteLine(text);
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
                GlobalLibraryValues.TriggerMessageCaller($"{nameof(Helper)},{nameof(TryEnsurePathExists)},{ex}");
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
                    GlobalLibraryValues.TriggerMessageCaller($"Create {directory}");
                }

                File.Create(path);
                GlobalLibraryValues.TriggerMessageCaller($"Create {path}");
            }
        }

    }
}
