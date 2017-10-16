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

        private CollectionViewSource pedidosViewSource;
        private CollectionViewSource pedidosDetallesViewSource;
        private CollectionViewSource gruposProductosTerminadosViewSource;

        private FormPedidoDetalleViewModel viewModel;

        private BiomasaEUPTContext context;


        public FormPedidoDetalle(BiomasaEUPTContext context)
        {
            InitializeComponent();
            InitializeComponent();
            viewModel = new FormPedidoDetalleViewModel();
            DataContext = viewModel;
            this.context = context;

        }

        public FormPedidoDetalle(BiomasaEUPTContext context, string _titulo) : this(context)
        {
            gbTitulo.Header = _titulo;

        }

        public FormPedidoDetalle(BiomasaEUPTContext context, PedidoDetalle pedidoDetalle)
        {
            gbTitulo.Header = "Editar Producto Envasado";

            if (pedidoDetalle.ProductoEnvasado.TipoProductoEnvasado.MedidoEnUnidades == true)
            {
                viewModel.Cantidad = pedidoDetalle.Unidades.Value;
            }
            else
            {
                viewModel.Cantidad = pedidoDetalle.Volumen.Value;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            pedidosViewSource = ((CollectionViewSource)(FindResource("pedidosViewSource")));
            pedidosDetallesViewSource = ((CollectionViewSource)(FindResource("pedidosDetallesViewSource")));
            gruposProductosTerminadosViewSource = ((CollectionViewSource)(FindResource("gruposProductosTerminadosViewSource")));

            context.PedidosCabeceras.Load();
            context.PedidosDetalles.Load();
            context.EstadosPedidos.Load();
            context.GruposProductosTerminados.Load();

            pedidosViewSource.Source = context.PedidosCabeceras.Local;
            pedidosDetallesViewSource.Source = context.PedidosDetalles.Local;
            pedidosDetallesViewSource.Source = context.PedidosDetalles.Local;
            gruposProductosTerminadosViewSource.Source = context.GruposProductosTerminados.Local;

        }


        private object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }

    }
}
