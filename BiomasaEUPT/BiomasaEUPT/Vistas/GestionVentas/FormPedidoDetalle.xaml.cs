using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiomasaEUPT.Vistas.GestionVentas
{
    /// <summary>
    /// Lógica de interacción para FormPedidoDetalle.xaml
    /// </summary>
    public partial class FormPedidoDetalle : UserControl
    {     
        private FormPedidoDetalleViewModel viewModel;

        public FormPedidoDetalle(BiomasaEUPTContext context, PedidoLinea pedidoLinea)
        {
            InitializeComponent();
            viewModel = new FormPedidoDetalleViewModel() { PedidoLinea = pedidoLinea};
            DataContext = viewModel;
        }

        public FormPedidoDetalle(BiomasaEUPTContext context, PedidoLinea pedidoLinea, string _titulo) : this(context, pedidoLinea)
        {
            gbTitulo.Header = _titulo;

        }

        public FormPedidoDetalle(BiomasaEUPTContext context, PedidoLinea pedidoLinea, PedidoDetalle pedidoDetalle) : this(context, pedidoLinea)
        {
            gbTitulo.Header = "Editar Pedido Detalle Manual";

            if (pedidoDetalle.ProductoEnvasado.TipoProductoEnvasado.MedidoEnUnidades == true)
            {
                viewModel.Cantidad = pedidoDetalle.Unidades.Value;
            }
            else
            {
                viewModel.Cantidad = pedidoDetalle.Volumen.Value;
            }
        }

    
    }
}
