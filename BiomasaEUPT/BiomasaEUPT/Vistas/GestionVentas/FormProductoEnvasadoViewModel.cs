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
        public TipoProductoTerminado TipoProductoTerminado { get; set; }
        public ObservableCollection<ProductoEnvasadoComposicion> ProductosEnvasadosComposiciones { get; set; }
        public ObservableCollection<ProductoEnvasado> ProductosEnvasados { get; set; }
        public ObservableCollection<HuecoAlmacenaje> HuecosAlmacenajesDisponibles { get; set; }
        public ObservableCollection<HistorialHuecoAlmacenaje> HistorialHuecosAlmacenajes { get; set; }
        public ObservableCollection<HistorialHuecoAlmacenaje> HistorialHuecosAlmacenajesDisponibles { get; set; }
        public double? Volumen { get; set; }
        public int? Unidades { get; set; }
        public double? VolumenCliente { get; set; }
        public string CantidadHint { get; set; }
        public float Cantidad { get; set; }

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

        public bool QuedaCantidadPorAlmacenar { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public FormProductoEnvasadoViewModel() {
            ProductosEnvasadosComposiciones = new ObservableCollection<ProductoEnvasadoComposicion>();
            ProductosEnvasados= new ObservableCollection<ProductoEnvasado>();
            HuecosAlmacenajesDisponibles = new ObservableCollection<HuecoAlmacenaje>();
            HistorialHuecosAlmacenajes = new ObservableCollection<HistorialHuecoAlmacenaje>();
            HistorialHuecosAlmacenajesDisponibles = new ObservableCollection<HistorialHuecoAlmacenaje>();
        }
    }
}
