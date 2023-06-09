using System;
using System.Diagnostics;
using System.Windows.Input;

namespace FlavorFi.Common.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute) : this(execute, null) { }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines whether the command can execute on teh current command target.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute(parameter);
        }

        /// <summary>
        /// Performs the actions that are associated with the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Event raised if the command manager that centralizes the commanding operations 
        /// detects a change in the command source that might invalidate a command that has 
        /// been raised but no yet executed by the command binding.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }

    public class DelegateCommand<T> : ICommand
    {
                private readonly System.Predicate<T> _canExecute;
        private readonly System.Action<T> _execute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(System.Action<T> execute) : this(execute, null) { }

        public DelegateCommand(Action<T> execute, System.Predicate<T> canExecuted)
        {
            _execute = execute;
            _canExecute = canExecuted;
        }

        /// <summary>
        /// Performs the actions that are associated with the command.
        /// </summary>
        /// <param name="parameter"></param>
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute((T)parameter);
        }

        /// <summary>
        /// Performs the actions that are associated with the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

    }

}