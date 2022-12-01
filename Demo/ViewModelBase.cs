using LoggerLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo
{
    public abstract class ViewModelBase : LoggerBase, INotifyPropertyChanged
    {

        public ViewModelBase() : base("DemoLog")
        {

        }

        public const string DEFAULT_STRING = "";
        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Dictionary<string, object> Storage { get; set; } = new Dictionary<string, object>();

        public T GetProperty<T>(string propertyName, T fallback = default)
        {
            T result = fallback;
            if (Storage.ContainsKey(propertyName))
                result = (T)Storage[propertyName];
            else
                SetProperty(propertyName, fallback);

            return result;
        }

        public void SetProperty<T>(string propertyName, T value)
        {

            if (!Storage.ContainsKey(propertyName))
                Storage.Add(propertyName, value);
            else
                Storage[propertyName] = value;

            OnPropertyChanged(propertyName);
        }

        internal void TestLogs()
        {
            for (int j = 0; j < 3; j++)
            {
                Thread thread = new Thread(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        this.Log($"Thread NR{j} - Log NR{i} Hallo World", false);
                    }
                });

                thread.Start();
                Thread.Sleep(1000);
            }
        }

        internal void Log(string log, bool enhancedLog)
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
