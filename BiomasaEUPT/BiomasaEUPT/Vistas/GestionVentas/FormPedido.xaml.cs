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
    /// Lógica de interacción para FormPedido.xaml
    /// </summary>
    public partial class FormPedido : UserControl
    {

        private CollectionViewSource pedidosViewSource;
        private CollectionViewSource estadosPedidosViewSource;
        private CollectionViewSource tiposProductosTerminadosViewSource;
        private CollectionViewSource gruposProductosTerminadosViewSource;
        private CollectionViewSource clientesViewSource;
        private FormProductoEnvasadoViewModel viewModel;

        public DateTime FechaPedido { get; set; }
        public DateTime HoraPedido { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public DateTime HoraFinalizacion { get; set; }
        private BiomasaEUPTContext context;

        public FormPedido(BiomasaEUPTContext context)
        {
            InitializeComponent();
            DataContext = this;
            this.context = context;
        }

        public FormPedido(BiomasaEUPTContext context, string _titulo) : this(context)
        {
            gbTitulo.Header = _titulo;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            pedidosViewSource = ((CollectionViewSource)(FindResource("pedidosViewSource")));
            estadosPedidosViewSource = ((CollectionViewSource)(FindResource("estadosPedidosViewSource")));
            clientesViewSource = ((CollectionViewSource)(FindResource("clientesViewSource")));

            context.PedidosCabeceras.Load();
            context.EstadosPedidos.Load();
            context.Clientes.Load();

            pedidosViewSource.Source = context.PedidosCabeceras.Local;
            estadosPedidosViewSource.Source = context.EstadosPedidos.Local;
            clientesViewSource.Source = context.Clientes.Local;

            dpFechaPedido.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
            dpFechaFinalizacion.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
        }

        private void spProductosEnvasadosComposiciones_Drop(object sender, DragEventArgs e)
        {
             var historialHuecoAlmacenaje= e.Data.GetData("HistorialHuecoAlmaceanje") as HistorialHuecoAlmacenaje;
             var productoEnvasadoComposicion = new ProductoEnvasadoComposicion() { HistorialHuecoAlmacenaje = historialHuecoAlmacenaje };
             viewModel.ProductosEnvasadosComposiciones.Add(productoEnvasadoComposicion);
             viewModel.HistorialHuecosAlmacenajesDisponibles.Remove(historialHuecoAlmacenaje);
        }

        private void cProductoEnvasadoComposicion_DeleteClick(object sender, RoutedEventArgs e)
        {
            /*var chip = sender as Chip;
            int historialHuecoAlmacenajeId = int.Parse(chip.CommandParameter.ToString());
            var productoEnvasadoComposicion = viewModel.ProductosEnvasadosComposiciones.Single(ptc => ptc.HistorialHuecoAlmacenaje.HistorialHuecoAlmacenajeId == historialHuecoAlmacenajeId);
            viewModel.ProductosEnvasadosComposiciones.Remove(productoEnvasadoComposicion);
            if (productoEnvasadoComposicion.HistorialHuecoAlmacenaje.ProductoTerminado.TipoId == (cbTiposProductosTerminados.SelectedItem as TipoProductoTerminado).TipoProductoTerminadoId)
            {
                viewModel.HistorialHuecosAlmacenajesDisponibles.Add(productoEnvasadoComposicion.HistorialHuecoAlmacenaje);
            }*/
        }


        
        private void AnadirProductoCantidad(object sender, RoutedEventArgs e)
        {
            wpProductosEnvasadosComposiciones.IsEnabled = true;
            var chip = sender as Chip;
            //int historialHuecoAlmacenajeId = int.Parse(chip.CommandParameter.ToString());
            //var productoEnvasadoComposicion = viewModel.ProductosEnvasadosComposiciones.Single(pec => pec.HistorialHuecoAlmacenaje.HistorialHuecoAlmacenajeId == historialHuecoAlmacenajeId);
            //viewModel.ProductosEnvasadosComposiciones.Remove(productoEnvasadoComposicion);
            int pedidoDetalleId = int.Parse(chip.CommandParameter.ToString());
            



        }
    }
}
