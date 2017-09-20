using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Vistas.GestionVentas
{
    class FormProductoEnvasadoViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<ProductoEnvasadoComposicion> ProductosEnvasadosComposiciones { get; set; }
        public ObservableCollection<ProductoEnvasado> ProductosEnvasados { get; set; }
        public float? Volumen { get; set; }

        private string _observaciones;
        public string Observaciones
        {
            get => _observaciones;
            set
            {
                // Si las observaciones es cadena vacía hay que asignarle el valor null
                _observaciones = value == "" ? null : value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public FormProductoEnvasadoViewModel() {
            ProductosEnvasadosComposiciones = new ObservableCollection<ProductoEnvasadoComposicion>();
            ProductosEnvasados= new ObservableCollection<ProductoEnvasado>();

        }
    }
}
