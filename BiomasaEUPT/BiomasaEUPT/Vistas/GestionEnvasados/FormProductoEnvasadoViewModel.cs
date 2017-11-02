using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Vistas.GestionEnvasados
{
    class FormProductoEnvasadoViewModel : INotifyPropertyChanged
    {

        public TipoProductoTerminado TipoProductoTerminado { get; set; }
        public TipoProductoEnvasado TipoProductoEnvasado { get; set; }
        public ObservableCollection<HistorialHuecoAlmacenaje> HistorialHuecosAlmacenajesDisponibles { get; set; }
        public ObservableCollection<HistorialHuecoAlmacenaje> HistorialHuecosAlmacenajes { get; set; }
        public ObservableCollection<ProductoEnvasadoComposicion> ProductosEnvasadosComposiciones { get; set; }
        public ObservableCollection<Picking> PickingDisponible { get; set; }
        public int? Unidades { get; set; }
        public double? Volumen { get; set; }
        public string CantidadHint { get; set; }
        
        public double Cantidad        { get; set; }
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

        public FormProductoEnvasadoViewModel()
        {
            HistorialHuecosAlmacenajesDisponibles = new ObservableCollection<HistorialHuecoAlmacenaje>();
            HistorialHuecosAlmacenajes = new ObservableCollection<HistorialHuecoAlmacenaje>();
            ProductosEnvasadosComposiciones = new ObservableCollection<ProductoEnvasadoComposicion>();
            PickingDisponible = new ObservableCollection<Picking>();
        }


    }
}
