using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProductManager.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _executeAction;
        private readonly Func<object, bool> _canExecuteAction;

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteCommand)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteCommand;
        }

        public void Execute(object parameter) => _executeAction(parameter);
        public bool CanExecute(object parameter) => _canExecuteAction?.Invoke(parameter) ?? true;
        public event EventHandler CanExecuteChanged;
        public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}