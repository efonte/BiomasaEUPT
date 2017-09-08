using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiomasaEUPT.Vistas.GestionVentas
{
    /// <summary>
    /// Lógica de interacción para FormProductoEnvasado.xaml
    /// </summary>
    public partial class FormProductoEnvasado : UserControl
    {

        private CollectionViewSource productosEnvasadosViewSource;
        private CollectionViewSource pickingViewSource;
        private CollectionViewSource pedidoDetalleViewSource;
        private FormProductoEnvasadoViewModel viewModel;

        private BiomasaEUPTContext context;

        public FormProductoEnvasado(BiomasaEUPTContext context)
        {
            InitializeComponent();
            viewModel = new FormProductoEnvasadoViewModel();
            DataContext = viewModel;
            this.context = context;
        }

        public FormProductoEnvasado(BiomasaEUPTContext context, ProductoEnvasado productoEnvasado) : this(context)
        {
            gbTitulo.Header = "Editar Producto Envasado";

            /*if (productoEnvasado.TipoProductoTerminado.MedidoEnUnidades == true)
            {
                viewModel.Cantidad = productoEnvasado.Unidades.Value;
            }
            else
            {
                viewModel.Cantidad = productoEnvasado.Volumen.Value;
            }*/

            //viewModel.ProductosEnvasadosComposiciones = new ObservableCollection<ProductoEnvasadoComposicion>(context.ProductosEnvasadosComposiciones.Where(pec => pec.ProductoId == productoEnvasado.ProductoEnvasadoId).ToList());
            //viewModel.HistorialHuecosAlmacenajes = new ObservableCollection<HistorialHuecoAlmacenaje>(context.HistorialHuecosAlmacenajes.Where(hha => hha.ProductoTerminadoId == productoTerminado.ProductoTerminadoId).ToList());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            productosEnvasadosViewSource = ((CollectionViewSource)(FindResource("productosEnvasadosViewSource")));
            pickingViewSource = ((CollectionViewSource)(FindResource("pickingViewSource")));
            pedidoDetalleViewSource = ((CollectionViewSource)(FindResource("pedidoDetalleViewSource")));

            context.ProductosEnvasados.Load();
            context.Picking.Load();
            context.PedidosDetalles.Load();

            productosEnvasadosViewSource.Source = context.ProductosEnvasados.Local;
            pickingViewSource.Source = context.Picking.Local;
            pedidoDetalleViewSource.Source = context.PedidosDetalles.Local;

        }
    }
}
