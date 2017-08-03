using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.GestionClientes;
using BiomasaEUPT.Vistas.GestionProveedores;
using BiomasaEUPT.Vistas.GestionUsuarios;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    public class FiltroViewModel<T> : INotifyPropertyChanged
    {
        // El tipo será asignado en cada una de las pestañas correspondientes.
        // Ejemplo: en TabUsuarios T será TipoUsuario
        public ObservableCollection<T> Items { get; set; }

        public ObservableCollection<T> ItemsSeleccionados { get; set; }

        public T ItemSeleccionado { get; set; }

        public ViewModelBase ViewModel { get; set; }

        public Action FiltrarItems { get; set; }

        public bool MostrarMenu { get; set; } = true;
        public string Titulo { get; set; } = "Filtro Tipo";

        public ICommand AnadirComando { get; set; }
        public ICommand BorrarComando { get; set; }
        public ICommand ModificarComando { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public FiltroViewModel()
        {
            if (typeof(T).Equals(typeof(TipoUsuario)))
                MostrarMenu = false;
        }

        public ICommand LBFiltro_SelectionChangedComando => new RelayCommandGenerico<IList<object>>(
            param =>
            {
                // Asigna el valor de ItemsSeleccionados ya que no se puede crear un Binding de SelectedItems desde el XAML
                ItemsSeleccionados = new ObservableCollection<T>(param.Cast<T>().ToList());

                FiltrarItems();
            });
    }
}
