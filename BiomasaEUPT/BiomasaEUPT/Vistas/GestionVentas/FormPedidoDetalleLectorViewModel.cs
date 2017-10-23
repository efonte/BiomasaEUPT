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
    public class FormPedidoDetalleLectorViewModel : INotifyPropertyChanged
    {

        public TipoProductoEnvasado TipoProductoEnvasado { get; set; }
        public ProductoEnvasado ProductoEnvasado { get; set; }
        public ObservableCollection<TipoProductoEnvasado> TiposProductosEnvasadosDisponibles { get; set; }
        public ObservableCollection<PedidoDetalle> PedidosDetalles { get; set; }
        public ObservableCollection<ProductoEnvasado> ProductosEnvasados { get; set; }

        public int? Unidades { get; set; }
        public double? Volumen { get; set; }
        public string CantidadHint { get; set; }
        public double Cantidad { get; set; }
        public string CodigoHint { get; set; }
        public double Codigo { get; set; }

        public bool QuedaCantidadPorAlmacenar { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public FormPedidoDetalleLectorViewModel()
        {
            TiposProductosEnvasadosDisponibles = new ObservableCollection<TipoProductoEnvasado>();
            PedidosDetalles = new ObservableCollection<PedidoDetalle>();
            ProductosEnvasados = new ObservableCollection<ProductoEnvasado>();
        }
    }
}
