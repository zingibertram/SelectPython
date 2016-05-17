using System;
using System.Windows.Input;
using System.Runtime.Serialization;

namespace SelectPython.Core
{
    public class RelayCommand : ICommand
    {
        [NonSerialized]
        private Action<Object> execute;
        [NonSerialized]
        private Predicate<Object> canExecute;

        [field:NonSerialized]
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<Object> exec, Predicate<Object> canExec = null)
        {
            if (exec == null)
            {
                throw new NullReferenceException();
            }
            execute = exec;
            canExecute = canExec;
        }

        public bool CanExecute(Object parameter)
        {
            if (canExecute != null)
            {
                return canExecute(parameter);
            }
            return true;
        }

        public void Execute(Object parameter)
        {
            execute(parameter);
        }

        public void OnCanExecutedChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }
    }
}
