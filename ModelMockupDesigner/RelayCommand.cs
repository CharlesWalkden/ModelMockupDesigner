using System;
using System.Windows.Input;

namespace ModelMockupDesigner
{
    public class RelayCommand : ICommand
    {
        private Action mAction;
        private object navigate;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action action)
        {
            mAction = action;
        }

        public RelayCommand(object navigate)
        {
            this.navigate = navigate;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mAction?.Invoke();
        }
    }
}
