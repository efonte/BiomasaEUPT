using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    public class OpcionesViewModel : INotifyPropertyChanged
    {
        public ICommand AnadirComando { get; set; }
        public ICommand BorrarComando { get; set; }
        public ICommand ModificarComando { get; set; }
        public ICommand RefrescarComando { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public OpcionesViewModel()
        {

        }
    }
}
