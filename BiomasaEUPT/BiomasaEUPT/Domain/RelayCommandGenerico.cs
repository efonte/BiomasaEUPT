using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BiomasaEUPT.Domain
{
    /// <summary>
    /// Un comando cuyo único propósito es delegar su funcionalidad a otro objeto invocando al delegado.
    /// El valor por defecto de retorno de la función CanExecute es 'true'.
    /// </summary>
    public class RelayCommandGenerico<T> : ICommand
    {
        #region Campos

        readonly Action<T> _execute = null;
        readonly Predicate<T> _canExecute = null;

        #endregion // Campos

        #region Constructores

        /// <summary>
        /// Crea un nuevo comando que siempre se puede ejecutar.
        /// </summary>
        /// <param name="execute">La lógica de ejecución.</param>
        public RelayCommandGenerico(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Crea un nuevo comando.
        /// </summary>
        /// <param name="execute">La lógica de ejecución.</param>
        /// <param name="canExecute">El estado de la lógica de ejecución.</param>
        public RelayCommandGenerico(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructores

        #region Miembros ICommand

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion // ICommand ICommand
    }
}
