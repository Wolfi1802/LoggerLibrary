using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Demo
{
    public class RelayCommand : ICommand
    {

        private Action<object> localAction;
        public RelayCommand(Action<object> action)
        {
            localAction = action;
        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            localAction(parameter);
        }

    }
}
