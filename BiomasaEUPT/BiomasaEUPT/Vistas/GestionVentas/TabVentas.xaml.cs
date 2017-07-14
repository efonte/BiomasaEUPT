using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
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
    /// Lógica de interacción para TabVentas.xaml
    /// </summary>
    public partial class TabVentas : UserControl
    {

        private BiomasaEUPTContext context;
        private CollectionViewSource productosEnvasadosViewSource;
        private CollectionViewSource pedidosViewSource;

        private enum ModoPaginacion { Primera = 1, Siguiente = 2, Anterior = 3, Ultima = 4, PageCountChange = 5 };

        public TabVentas()
        {
            InitializeComponent();
            DataContext = this;


            ucTablaPedidos.bAnadirPedido.Click += BAnadirPedido_Click;
            ucTablaProductosEnvasados.bAnadirProductoEnvasado.Click += BAnadirProductoEnvasado_Click;

            Style rowStyle = new Style(typeof(DataGridRow), (Style)TryFindResource(typeof(DataGridRow)));
            rowStyle.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler(RowPedidos_DoubleClick)));
            ucTablaPedidos.dgPedidos.RowStyle = rowStyle;
            ucTablaPedidos.dgPedidos.SelectedIndex = -1;

           // (ucTablaPedidos.ucPaginacion.DataContext as PaginacionViewSource).ParentUC = this;
           // (ucTablaPedidos.ucPaginacion.DataContext as PaginacionViewSource).CalcularItemsTotales();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = new BiomasaEUPTContext();
                productosEnvasadosViewSource = (CollectionViewSource)(ucTablaProductosEnvasados.FindResource("productosEnvasadosViewSource"));
                pedidosViewSource = (CollectionViewSource)(ucTablaPedidos.FindResource("pedidosViewSource"));


                context.ProductosEnvasados.Load();
                context.PedidosCabeceras.Load();

                
                productosEnvasadosViewSource.Source = context.ProductosEnvasados.Local;
                pedidosViewSource.Source = context.PedidosCabeceras.Local;

            }
        }

        private void DgPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pedido = (sender as DataGrid).SelectedItem as PedidoCabecera;
            if (pedido != null)
            {
                ucTablaProductosEnvasados.IsEnabled = true;
                productosEnvasadosViewSource.Source = context.ProductosEnvasados.Where(pe => pe.ProductoEnvasadoId == pedido.PedidoCabeceraId).ToList();
                
            }
            else
            {
                ucTablaPedidos.IsEnabled = false;
            }
        }

        private async void BAnadirPedido_Click(object sender, RoutedEventArgs e)
        {
            var formPedido = new FormPedido(context);

            if ((bool)await DialogHost.Show(formPedido, "RootDialog"))
            {
                context.PedidosCabeceras.Add(new PedidoCabecera()
                {
                    FechaPedido = new DateTime(formPedido.FechaPedido.Year, formPedido.FechaPedido.Month, formPedido.FechaPedido.Day, formPedido.FechaPedido.Hour, formPedido.FechaPedido.Minute, formPedido.FechaPedido.Second),
                    FechaFinalizacion = new DateTime(formPedido.FechaFinalizacion.Year, formPedido.FechaFinalizacion.Month, formPedido.FechaFinalizacion.Day, formPedido.FechaFinalizacion.Hour, formPedido.FechaFinalizacion.Minute, formPedido.FechaFinalizacion.Second),
                    ClienteId = (formPedido.cbClientes.SelectedItem as Cliente).ClienteId,
                    EstadoId = 1
                });
                context.SaveChanges();
            }
        }

        private async void BAnadirProductoEnvasado_Click(object sender, RoutedEventArgs e)
        {
            var formProductoEnvasado = new FormProductoEnvasado(context);

            if ((bool)await DialogHost.Show(formProductoEnvasado, "RootDialog"))
            {
                var productoEnvasado = new ProductoEnvasado()
                {
                    Nombre = formProductoEnvasado.tbNombre.Text,
                    //Volumen = formProductoEnvasado.Volumen,
                    Observaciones = formProductoEnvasado.tbObservaciones.Text,
                    FechaBaja = new DateTime(formProductoEnvasado.FechaBaja.Year, formProductoEnvasado.FechaBaja.Month, formProductoEnvasado.FechaBaja.Day, formProductoEnvasado.FechaBaja.Hour, formProductoEnvasado.FechaBaja.Minute, formProductoEnvasado.FechaBaja.Second),
                    TipoProductoTerminadoId = (formProductoEnvasado.cbTipoProductoTerminado.SelectedItem as TipoProductoTerminado).TipoProductoTerminadoId,
                    PickingId = (formProductoEnvasado.cbPicking.SelectedItem as Picking).PickingId
                };
                if (formProductoEnvasado.FechaBaja != null)
                    productoEnvasado.FechaBaja = new DateTime(formProductoEnvasado.FechaBaja.Year, formProductoEnvasado.FechaBaja.Month, formProductoEnvasado.FechaBaja.Day, formProductoEnvasado.HoraBaja.Hour, formProductoEnvasado.HoraBaja.Minute, formProductoEnvasado.HoraBaja.Second);

                context.ProductosEnvasados.Add(productoEnvasado);
                context.SaveChanges();
                //CollectionViewSource.GetDefaultView(ucTablaMateriasPrimas.dgMateriasPrimas.ItemsSource).Refresh();
                ucTablaProductosEnvasados.dgProductosEnvasados.Items.Refresh();
            }
        }

        public void FiltrarTablaPedidos()
        {
            pedidosViewSource.Filter += new FilterEventHandler(FiltroTablaPedidos);
        }

        private void FiltroTablaPedidos(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaPedidos.tbBuscar.Text.ToLower();
            var pedido = e.Item as PedidoCabecera;
            string fechaPedido = pedido.FechaPedido.ToString();
            string fechaFinalizacion = pedido.FechaFinalizacion.ToString();
            string estado = pedido.EstadoPedido.Nombre.ToLower();
            string cliente = pedido.Cliente.RazonSocial.ToLower();

            e.Accepted = (ucTablaPedidos.cbFechaPedido.IsChecked == true ? fechaPedido.Contains(textoBuscado) : false) ||
                         (ucTablaPedidos.cbFechaFinalizacion.IsChecked == true ? fechaFinalizacion.Contains(textoBuscado) : false) ||
                         (ucTablaPedidos.cbEstadoPedido.IsChecked == true ? estado.Contains(textoBuscado) : false) ||
                         (ucTablaPedidos.cbCliente.IsChecked == true ? cliente.Contains(textoBuscado) : false);

        }

        public void FiltrarTablaProductosEnvasados()
        {
            productosEnvasadosViewSource.Filter += new FilterEventHandler(FiltroTablaProductosEnvasados);
        }

        private void FiltroTablaProductosEnvasados(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaProductosEnvasados.tbBuscar.Text.ToLower();
            var productoEnvasado = e.Item as ProductoEnvasado;
            string nombre = productoEnvasado.Nombre.ToString();
            string tipo = productoEnvasado.TipoProductoTerminado.Nombre.ToLower();
            string grupo = productoEnvasado.TipoProductoTerminado.GrupoProductoTerminado.Nombre.ToLower();
            string volumen = productoEnvasado.Volumen.ToString();
            string codigo = productoEnvasado.Codigo.ToString();
            

            e.Accepted = (ucTablaProductosEnvasados.cbNombre.IsChecked == true ? nombre.Contains(textoBuscado) : false) ||
                         (ucTablaProductosEnvasados.cbTipo.IsChecked == true ? tipo.Contains(textoBuscado) : false) ||
                         (ucTablaProductosEnvasados.cbGrupo.IsChecked == true ? grupo.Contains(textoBuscado) : false) ||
                         (ucTablaProductosEnvasados.cbVolumen.IsChecked == true ? (volumen.Contains(textoBuscado)) : false) ||
                         (ucTablaProductosEnvasados.cbCodigo.IsChecked == true ? codigo.Contains(textoBuscado) : false);
        }

        #region BorrarPedido
        private ICommand _borrarPedidoComando;

        public ICommand BorrarPedidoComando
        {
            get
            {
                if (_borrarPedidoComando == null)
                {
                    _borrarPedidoComando = new RelayComando(
                        param => BorrarPedido(),
                        param => CanBorrarPedido()
                    );
                }
                return _borrarPedidoComando;
            }
        }

        private bool CanBorrarPedido()
        {
            if (ucTablaPedidos.dgPedidos.SelectedIndex != -1)
            {
                Recepcion pedidoSeleccionada = ucTablaPedidos.dgPedidos.SelectedItem as Recepcion;
                return pedidoSeleccionada != null;
            }
            return false;
        }

        private async void BorrarPedido()
        {
            string pregunta = ucTablaPedidos.dgPedidos.SelectedItems.Count == 1
                   ? "¿Está seguro de que desea borrar el pedido con fecha " + (ucTablaPedidos.dgPedidos.SelectedItem as PedidoCabecera).FechaPedido + "?"
                   : "¿Está seguro de que desea borrar los pedidos seleccionados?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {
                List<PedidoCabecera> pedidosABorrar = new List<PedidoCabecera>();
                var pedidosSeleccionados = ucTablaPedidos.dgPedidos.SelectedItems.Cast<PedidoCabecera>().ToList();
                foreach (var pedido in pedidosSeleccionados)
                {
                    if (!context.ProductosEnvasados.Any(pe => pe.ProductoEnvasadoId == pedido.PedidoCabeceraId))
                    {
                        pedidosABorrar.Add(pedido);
                    }
                }
                context.PedidosCabeceras.RemoveRange(pedidosABorrar);
                context.SaveChanges();
                if (pedidosSeleccionados.Count != pedidosABorrar.Count)
                {
                    string mensaje = ucTablaPedidos.dgPedidos.SelectedItems.Count == 1
                           ? "No se ha podido borrar el pedido seleccionado."
                           : "No se han podido borrar todas los pedidos seleccionados.";
                    mensaje += "\n\nAsegurese de no que no exista ningún producto terminado asociada a dicha pedido.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }

            }
        }
        #endregion

        #region BorrarProductoEnvasado
        private ICommand _borrarProductoEnvasadoComando;

        public ICommand BorrarProductoEnvasadoComando
        {
            get
            {
                if (_borrarProductoEnvasadoComando == null)
                {
                    _borrarProductoEnvasadoComando = new RelayComando(
                        param => BorrarProductoEnvasado(),
                        param => CanBorrarProductoEnvasado()
                    );
                }
                return _borrarProductoEnvasadoComando;
            }
        }

        private bool CanBorrarProductoEnvasado()
        {
            if (ucTablaProductosEnvasados.dgProductosEnvasados.SelectedIndex != -1)
            {
                PedidoCabecera pedidoSeleccionado = ucTablaProductosEnvasados.dgProductosEnvasados.SelectedItem as PedidoCabecera;
                return pedidoSeleccionado != null;
            }
            return false;
        }

        private async void BorrarProductoEnvasado()
        {
            string pregunta = ucTablaProductosEnvasados.dgProductosEnvasados.SelectedItems.Count == 1
                ? "¿Está seguro de que desea borrar el producto envasado " + (ucTablaProductosEnvasados.dgProductosEnvasados.SelectedItem as ProductoEnvasado).Codigo + "?"
                : "¿Está seguro de que desea borrar los productos envasados seleccionados?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {

                List<ProductoEnvasado> productoEnvasadoABorrar = new List<ProductoEnvasado>();
                var productosEnvasadosSeleccionados = ucTablaProductosEnvasados.dgProductosEnvasados.SelectedItems.Cast<ProductoEnvasado>().ToList();

                foreach (var productos in productosEnvasadosSeleccionados)
                {
                    /*if (!context.MateriasPrimas.Any(mp => mp.RecepcionId == recepcion.RecepcionId))
                    {

                    }*/
                }
                context.ProductosEnvasados.RemoveRange(ucTablaProductosEnvasados.dgProductosEnvasados.SelectedItems.Cast<ProductoEnvasado>().ToList());
                context.SaveChanges();
                ucTablaProductosEnvasados.dgProductosEnvasados.Items.Refresh();
                // CollectionViewSource.GetDefaultView(ucTablaMateriasPrimas.dgMateriasPrimas.ItemsSource).Refresh();
            }
        }
        #endregion

        private async void RowPedidos_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var fila = sender as DataGridRow;
            var pedidoSeleccionado = ucTablaPedidos.dgPedidos.SelectedItem as PedidoCabecera;
            var formPedido = new FormPedido(context, "Editar Pedido")
            {
                FechaPedido = pedidoSeleccionado.FechaPedido,
                FechaFinalizacion = pedidoSeleccionado.FechaFinalizacion.Value,
            };
            formPedido.cbEstados.Visibility = Visibility.Visible;
            formPedido.cbEstados.SelectedValue = pedidoSeleccionado.EstadoPedido.EstadoSalidaId;
            formPedido.cbClientes.SelectedValue = pedidoSeleccionado.Cliente.ClienteId;
            if ((bool)await DialogHost.Show(formPedido, "RootDialog"))
            {
                pedidoSeleccionado.FechaPedido = new DateTime(formPedido.FechaPedido.Year, formPedido.FechaPedido.Month, formPedido.FechaPedido.Day, formPedido.FechaPedido.Hour, formPedido.FechaPedido.Minute, formPedido.FechaPedido.Second);
                pedidoSeleccionado.FechaFinalizacion = new DateTime(formPedido.FechaFinalizacion.Year, formPedido.FechaFinalizacion.Month, formPedido.FechaFinalizacion.Day, formPedido.FechaFinalizacion.Hour, formPedido.FechaFinalizacion.Minute, formPedido.FechaFinalizacion.Second);
                pedidoSeleccionado.ClienteId = (formPedido.cbClientes.SelectedItem as Cliente).ClienteId;
                pedidoSeleccionado.EstadoId = (formPedido.cbEstados.SelectedItem as EstadoPedido).EstadoSalidaId;
                pedidosViewSource.View.Refresh();
                context.SaveChanges();
                /* using (var context = new BiomasaEUPTContext())
                 {
                     var recepcion = context.Recepciones.Single(tc => tc.NumeroAlbaran == albaranViejo);
                     recepcion.NumeroAlbaran = formTipo.Nombre;
                     tipoCliente.Descripcion = formTipo.Descripcion;
                     context.SaveChanges();
                 }*/
            }
        }
    }
}
