using LoggerLibrary;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

namespace Demo
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            GlobalLibraryValues.LibaryCaller += (callerType, message) =>
            {
                //Debug.WriteLine($"Library Log : {callerType} - {message}");
            };

            GlobalLibraryValues.LogsCaller += (log) =>
            {

            };

            TestLogs();
        }
        private static void TestLogs()
        {
            for (int j = 0; j < 3; j++)
            {
                Thread thread = new Thread(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        //this.Log($"Thread NR{j} - Log NR{i} Hallo World", false);//TODO[TS]LOG
                    }
                });

                thread.Start();
                Thread.Sleep(1000);
            }
        }

        public ObservableCollection<LogModell> ItemsSource
        {
            get => base.GetProperty<ObservableCollection<LogModell>>(nameof(this.ItemsSource));
            set => base.SetProperty(nameof(this.ItemsSource), value);
        }

        public ICommand TestCommand => new RelayCommand(o =>
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        });

    }
}
