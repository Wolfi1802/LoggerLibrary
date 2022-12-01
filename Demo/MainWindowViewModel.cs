using LoggerLibrary;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Demo
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            this.ItemsSource = new ObservableCollection<LogModell>();

            GlobalLibraryValues.LibaryCaller += (callerType, message) =>
            {
                //Some weird Logs from Lib are here.
            };

            GlobalLibraryValues.LogsCaller += (log) =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    log.Message = log.Message.Replace("\n", string.Empty);
                    log.Stacktrace = log.Stacktrace.Replace("\n", string.Empty);
                    ItemsSource.Add(log);
                }));
            };
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
                Thread thread = new Thread(() => { base.TestLogs(); });
                thread.Start();
            }
            catch (Exception ex)
            {

            }
        });
    }
}
