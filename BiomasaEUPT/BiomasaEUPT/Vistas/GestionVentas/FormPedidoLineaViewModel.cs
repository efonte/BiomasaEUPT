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
    public class FormPedidoLineaViewModel : INotifyPropertyChanged
    {

        public TipoProductoEnvasado TipoProductoEnvasado { get; set; }

        public int? Unidades { get; set; }
        public double? Volumen { get; set; }
        public string CantidadHint { get; set; }
        public double Cantidad { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public FormPedidoLineaViewModel()
        {

        }
    }
}
