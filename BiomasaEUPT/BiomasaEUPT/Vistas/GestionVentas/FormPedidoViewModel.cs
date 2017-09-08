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
    public class FormPedidoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TipoProductoTerminado> TiposProductosTerminadosDisponibles { get; set; }
        public ObservableCollection<PedidoDetalle> PedidosDetalles { get; set; }
              public DateTime FechaPedido { get; set; }
        public DateTime HoraPedido { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public FormPedidoViewModel()
        {
            TiposProductosTerminadosDisponibles = new ObservableCollection<TipoProductoTerminado>();
            PedidosDetalles = new ObservableCollection<PedidoDetalle>();
        }

    }
}
