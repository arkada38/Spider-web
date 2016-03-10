using System;
using System.Windows.Input;

namespace Spider_web.Utils
{
    internal class CommandHandler : ICommand
    {
        public CommandHandler(Action<object> action)
        {
            ExecuteDelegate = action;
        }

        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }


        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return CanExecuteDelegate == null || CanExecuteDelegate(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteDelegate?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;

        #endregion
    }
}
