using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    public class ContadorViewModel<T> : INotifyPropertyChanged
    {
        // El tipo será asignado en cada una de las pestañas correspondientes.
        // Ejemplo: en TabUsuarios T será TipoUsuario
        public ObservableCollection<T> Tipos { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public ContadorViewModel()
        {

        }

    }
}
