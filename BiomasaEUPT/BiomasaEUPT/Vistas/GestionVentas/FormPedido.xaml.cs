using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
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

            //cbGruposProductosTerminados.SelectedValue = pedidoCabecera.TipoProductoTerminado.GrupoProductoTerminado.GrupoProductoTerminadoId;
            //cbTiposProductosTerminados.SelectedValue = pedidoCabecera.TipoProductoTerminado.TipoProductoTerminadoId;

            //viewModel.FechaBaja = pedido.FechaPedido;
            //viewModel.HoraBaja = pedido.FechaPedido;

            /*if (pedido.TipoProductoTerminado.MedidoEnUnidades == true)
            {
                viewModel.Cantidad = pedido.Unidades.Value;
            }
            else
            {
                viewModel.Cantidad = pedido.Volumen.Value;
            }*/
            //viewModel.ProductosEnvasadosComposiciones = new ObservableCollection<ProductoEnvasadoComposicion>(context.ProductosEnvasadosComposiciones.Where(pec => pec.ProductoId == pedido.PedidoCabeceraId).ToList());
            //viewModel.HistorialHuecosAlmacenajes = new ObservableCollection<HistorialHuecoAlmacenaje>(context.HistorialHuecosAlmacenajes.Where(hha => hha.ProductoTerminadoId == productoTerminado.ProductoTerminadoId).ToList());
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

        private void spProductosTerminadosComposiciones_Drop(object sender, DragEventArgs e)
        {
            /* var historialHuecoRecepcion = e.Data.GetData("HistorialHuecoRecepcion") as HistorialHuecoRecepcion;
             var productoTerminadoComposicion = new ProductoTerminadoComposicion() { HistorialHuecoRecepcion = historialHuecoRecepcion };
             viewModel.ProductosTerminadosComposiciones.Add(productoTerminadoComposicion);
             viewModel.HistorialHuecosRecepcionesDisponibles.Remove(historialHuecoRecepcion);*/
        }

        private void cProductoTerminadoComposicion_DeleteClick(object sender, RoutedEventArgs e)
        {
            /*var chip = sender as Chip;
            int historialHuecoRecepcionId = int.Parse(chip.CommandParameter.ToString());
            var productoTerminadoComposicion = viewModel.ProductosTerminadosComposiciones.Single(ptc => ptc.HistorialHuecoRecepcion.HistorialHuecoRecepcionId == historialHuecoRecepcionId);
            viewModel.ProductosTerminadosComposiciones.Remove(productoTerminadoComposicion);
            if (productoTerminadoComposicion.HistorialHuecoRecepcion.MateriaPrima.TipoId == (cbTiposMateriasPrimas.SelectedItem as TipoMateriaPrima).TipoMateriaPrimaId)
            {
                viewModel.HistorialHuecosRecepcionesDisponibles.Add(productoTerminadoComposicion.HistorialHuecoRecepcion);
            }*/
        }

        private void lbHuecosAlmacenajes_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*var parent = sender as ListBox;
            var huecoAlmacenaje = GetDataFromListBox(lbHuecosAlmacenajes, e.GetPosition(parent)) as HuecoAlmacenaje;
            if (huecoAlmacenaje != null)
            {
                DataObject dragData = new DataObject("HuecoAlmacenaje", huecoAlmacenaje);
                DragDrop.DoDragDrop(parent, dragData, DragDropEffects.Move);
            }*/
        }

        /*private object GetDataFromListBox(ListBox source, Point point)
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
        }*/
    }
}
