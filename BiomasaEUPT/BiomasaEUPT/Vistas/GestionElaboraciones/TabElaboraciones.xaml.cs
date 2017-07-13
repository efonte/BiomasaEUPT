using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

namespace BiomasaEUPT.Vistas.GestionElaboraciones
{
    /// <summary>
    /// Lógica de interacción para TabElaboraciones.xaml
    /// </summary>
    public partial class TabElaboraciones : UserControl
    {
        private BiomasaEUPTContext context;
        private CollectionViewSource ordenesElaboracionesViewSource;
        private CollectionViewSource productosTerminadosViewSource;

        private enum ModoPaginacion { Primera = 1, Siguiente = 2, Anterior = 3, Ultima = 4, PageCountChange = 5 };

        public TabElaboraciones()
        {
            InitializeComponent();
            DataContext = this;

            //ucTablaRecepciones.dgRecepciones.SelectionChanged += DgRecepciones_SelectionChanged;
            //ucTablaRecepciones.cbNumeroAlbaran.Checked += (s, e1) => { FiltrarTablaRecepciones(); };
            ucTablaElaboraciones.dgElaboraciones.SelectionChanged += DgElaboraciones_SelectionChanged;

            ucTablaProductosTerminados.bAnadirProductoTerminado.Click += BAnadirProductoTerminado_Click;

            Style rowStyle = new Style(typeof(DataGridRow), (Style)TryFindResource(typeof(DataGridRow)));
            rowStyle.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler(RowElaboraciones_DoubleClick)));
            ucTablaElaboraciones.dgElaboraciones.RowStyle = rowStyle;
            ucTablaElaboraciones.dgElaboraciones.SelectedIndex = -1;

            // Hacer doble clic en una fila del datagrid de materias primas hará que se ejecuta el evento RowMateriasPrimas_DoubleClick
            Style rowStyleProductosTerminados = new Style(typeof(DataGridRow), (Style)TryFindResource(typeof(DataGridRow)));
            rowStyleProductosTerminados.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler(RowProductosTerminados_DoubleClick)));
            ucTablaProductosTerminados.dgProductosTerminados.RowStyle = rowStyleProductosTerminados;

            (ucTablaElaboraciones.ucPaginacion.DataContext as PaginacionViewSource).ParentUC = this;
            (ucTablaElaboraciones.ucPaginacion.DataContext as PaginacionViewSource).CalcularItemsTotales();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = new BiomasaEUPTContext();
                ordenesElaboracionesViewSource = (CollectionViewSource)(ucTablaElaboraciones.FindResource("ordenesElaboracionesViewSource"));
                productosTerminadosViewSource = (CollectionViewSource)(ucTablaProductosTerminados.FindResource("productosTerminadosViewSource"));

                context.OrdenesElaboraciones.Load();
                context.ProductosTerminados.Load();

                ordenesElaboracionesViewSource.Source = context.OrdenesElaboraciones.Local;
                productosTerminadosViewSource.Source = context.ProductosTerminados.Local;

            }
        }

        private void DgElaboraciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ordenElaboracion = (sender as DataGrid).SelectedItem as OrdenElaboracion;
            if (ordenElaboracion != null)
            {
                ucTablaProductosTerminados.IsEnabled = true;
                productosTerminadosViewSource.Source = context.ProductosTerminados.Where(pt => pt.OrdenId == ordenElaboracion.OrdenElaboracionId).ToList();

            }
            else
            {
                ucTablaProductosTerminados.IsEnabled = false;
            }
        }

        private async void BAnadirElaboracion_Click(object sender, RoutedEventArgs e)
        {
            var formElaboracion = new FormElaboracion(context);

            if ((bool)await DialogHost.Show(formElaboracion, "RootDialog"))
            {
                context.OrdenesElaboraciones.Add(new OrdenElaboracion()
                {
                    EstadoElaboracionId = 1,
                    Descripcion = formElaboracion.Descripcion,
                    //EstadoId = (formRecepcion.cbEstadosRecepciones.SelectedItem as EstadoRecepcion).EstadoRecepcionId

                });
                context.SaveChanges();
            }
        }

        private async void BAnadirProductoTerminado_Click(object sender, RoutedEventArgs e)
        {
            var formProductoTerminado = new FormProductoTerminado(context);

            if ((bool)await DialogHost.Show(formProductoTerminado, "RootDialog"))
            {
                var formProductoTerminadoDataContext = formProductoTerminado.DataContext as FormProductoTerminadoViewModel;
                var productoTerminado = new ProductoTerminado()
                {
                    OrdenId = (ucTablaElaboraciones.dgElaboraciones.SelectedItem as OrdenElaboracion).OrdenElaboracionId,
                    TipoId = (formProductoTerminado.cbTiposMateriasPrimas.SelectedItem as TipoMateriaPrima).TipoMateriaPrimaId,
                    Volumen = formProductoTerminadoDataContext.Volumen,
                    Unidades = formProductoTerminadoDataContext.Unidades,
                    Observaciones = formProductoTerminadoDataContext.Observaciones


                };

                if (formProductoTerminadoDataContext.FechaBaja != null)
                {
                    productoTerminado.FechaBaja = new DateTime(
                        formProductoTerminadoDataContext.FechaBaja.Value.Year,
                        formProductoTerminadoDataContext.FechaBaja.Value.Month,
                        formProductoTerminadoDataContext.FechaBaja.Value.Day,
                        formProductoTerminadoDataContext.HoraBaja.Value.Hour,
                        formProductoTerminadoDataContext.HoraBaja.Value.Minute,
                        formProductoTerminadoDataContext.HoraBaja.Value.Second);
                }
                context.ProductosTerminados.Add(productoTerminado);
                var huecosProductosTerminados = new List<HistorialHuecoAlmacenaje>();
                foreach (var hpt in formProductoTerminadoDataContext.HistorialHuecosAlmacenajes)
                {
                    var haId = hpt.HuecoAlmacenaje.HuecoAlmacenajeId;
                    // Los huecos que no se ha añadido ninguna cantidad no se añaden
                    if (hpt.Unidades != 0 && hpt.Volumen != 0)
                    {
                        hpt.HuecoAlmacenaje = null;
                        hpt.HuecoAlmacenajeId = haId;
                        hpt.ProductoTerminado = productoTerminado;
                        huecosProductosTerminados.Add(hpt);
                    }
                }
                context.HistorialHuecosAlmacenajes.AddRange(huecosProductosTerminados);
                context.SaveChanges();

                CargarProductosTerminados();
            }
        }

        public void FiltrarTablaElaboraciones()
        {
            ordenesElaboracionesViewSource.Filter += new FilterEventHandler(FiltroTablaElaboraciones);
        }

        private void FiltroTablaElaboraciones(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaElaboraciones.tbBuscar.Text.ToLower();
            var elaboracion = e.Item as OrdenElaboracion;

            string fechaElaboracion = elaboracion.FechaElaboracion.ToString();
            string estado = elaboracion.EstadoElaboracion.Nombre.ToLower();


            e.Accepted = (ucTablaElaboraciones.cbFechaElaboracion.IsChecked == true ? fechaElaboracion.Contains(textoBuscado) : false) ||
                         (ucTablaElaboraciones.cbEstado.IsChecked == true ? estado.Contains(textoBuscado) : false);

        }

        public void FiltrarTablaProductosTerminados()
        {
            productosTerminadosViewSource.Filter += new FilterEventHandler(FiltroTablaProductosTerminados);
        }

        private void FiltroTablaProductosTerminados(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaProductosTerminados.tbBuscar.Text.ToLower();
            var productoTerminado = e.Item as ProductoTerminado;
            string tipo = productoTerminado.TipoProductoTerminado.Nombre.ToLower();
            string grupo = productoTerminado.TipoProductoTerminado.GrupoProductoTerminado.Nombre.ToLower();
            string volumen = productoTerminado.Volumen.ToString();
            string unidades = productoTerminado.Unidades.ToString();

            e.Accepted = (ucTablaProductosTerminados.cbTipo.IsChecked == true ? tipo.Contains(textoBuscado) : false) ||
                         (ucTablaProductosTerminados.cbGrupo.IsChecked == true ? grupo.Contains(textoBuscado) : false) ||
                         (ucTablaProductosTerminados.cbVolUni.IsChecked == true ? (volumen.Contains(textoBuscado) || unidades.Contains(textoBuscado)) : false);
        }

        #region BorrarElaboracion
        private ICommand _borrarElaboracionComando;

        public ICommand BorrarElaboracionComando
        {
            get
            {
                if (_borrarElaboracionComando == null)
                {
                    _borrarElaboracionComando = new RelayComando(
                        param => BorrarElaboracion(),
                        param => CanBorrarElaboracion()
                    );
                }
                return _borrarElaboracionComando;
            }
        }

        private bool CanBorrarElaboracion()
        {
            if (ucTablaElaboraciones.dgElaboraciones.SelectedIndex != -1)
            {
                OrdenElaboracion ElaboracionSeleccionada = ucTablaElaboraciones.dgElaboraciones.SelectedItem as OrdenElaboracion;
                return ElaboracionSeleccionada != null;
            }
            return false;
        }

        private async void BorrarElaboracion()
        {
            string pregunta = ucTablaElaboraciones.dgElaboraciones.SelectedItems.Count == 1
                   ? "¿Está seguro de que desea borrar la elaboración " + (ucTablaElaboraciones.dgElaboraciones.SelectedItem as OrdenElaboracion).OrdenElaboracionId + "?"
                   : "¿Está seguro de que desea borrar las elaboraciones seleccionadas?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {
                List<OrdenElaboracion> elaboracionesABorrar = new List<OrdenElaboracion>();
                var elaboracionesSeleccionadas = ucTablaElaboraciones.dgElaboraciones.SelectedItems.Cast<Recepcion>().ToList();
                foreach (var elaboracion in elaboracionesSeleccionadas)
                {
                    /*if (!context.OrdenesElaboraciones.Any(oe => oe.EstadoElaboracionId == elaboracion.EstadoId))
                    {
                        elaboracionesABorrar.Add(elaboracion);
                    }*/
                }
                context.OrdenesElaboraciones.RemoveRange(elaboracionesABorrar);
                context.SaveChanges();
                if (elaboracionesSeleccionadas.Count != elaboracionesABorrar.Count)
                {
                    string mensaje = ucTablaElaboraciones.dgElaboraciones.SelectedItems.Count == 1
                           ? "No se ha podido borrar la elaboración seleccionada."
                           : "No se han podido borrar todas las elaboración seleccionadas.";
                    mensaje += "\n\nAsegurese de no que no exista ningón producto terminado asociada a dicha elaboración.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }

            }
        }
        #endregion

        #region BorrarProductoTerminado
        private ICommand _borrarProductoTerminadoComando;

        public ICommand BorrarProductoTerminadoComando
        {
            get
            {
                if (_borrarProductoTerminadoComando == null)
                {
                    _borrarProductoTerminadoComando = new RelayComando(
                        param => BorrarProductoTerminado(),
                        param => CanBorrarProductoTerminado()
                    );
                }
                return _borrarProductoTerminadoComando;
            }
        }

        private bool CanBorrarProductoTerminado()
        {
            if (ucTablaProductosTerminados.dgProductosTerminados.SelectedIndex != -1)
            {
                ProductoTerminado productoTerminadoSeleccionado = ucTablaProductosTerminados.dgProductosTerminados.SelectedItem as ProductoTerminado;
                return productoTerminadoSeleccionado != null;
            }
            return false;
        }

        private async void BorrarProductoTerminado()
        {
            string pregunta = ucTablaProductosTerminados.dgProductosTerminados.SelectedItems.Count == 1
                ? "¿Está seguro de que desea borrar el producto terminado con código " + (ucTablaProductosTerminados.dgProductosTerminados.SelectedItem as ProductoTerminado).Codigo + "?"
                : "¿Está seguro de que desea borrar los productos terminados seleccionados?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {

                List<ProductoTerminado> productosTerminadosABorrar = new List<ProductoTerminado>();
                var productosTerminadosSeleccionados = ucTablaProductosTerminados.dgProductosTerminados.SelectedItems.Cast<ProductoTerminado>().ToList();

                foreach (var productosTerminados in productosTerminadosSeleccionados)
                {
                    if (!context.OrdenesElaboraciones.Any(oe => oe.OrdenElaboracionId == productosTerminados.OrdenId))
                    {

                    }
                }
                context.ProductosTerminados.RemoveRange(ucTablaProductosTerminados.dgProductosTerminados.SelectedItems.Cast<ProductoTerminado>().ToList());
                context.SaveChanges();
                ucTablaProductosTerminados.dgProductosTerminados.Items.Refresh();
                // CollectionViewSource.GetDefaultView(ucTablaMateriasPrimas.dgMateriasPrimas.ItemsSource).Refresh();
            }
        }
        #endregion

        private async void RowElaboraciones_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var fila = sender as DataGridRow;
            var elaboracionSeleccionada = ucTablaElaboraciones.dgElaboraciones.SelectedItem as OrdenElaboracion;
            var formElaboracion = new FormElaboracion(context, "Editar Elaboración")
            {
                Descripcion = elaboracionSeleccionada.Descripcion
            };
            formElaboracion.cbEstadoElaboracion.Visibility = Visibility.Visible;
            formElaboracion.cbEstadoElaboracion.SelectedValue = elaboracionSeleccionada.EstadoElaboracion.EstadoElaboracionId;
            if ((bool)await DialogHost.Show(formElaboracion, "RootDialog"))
            {
                elaboracionSeleccionada.EstadoElaboracionId = (formElaboracion.cbEstadoElaboracion.SelectedItem as EstadoElaboracion).EstadoElaboracionId;
                ordenesElaboracionesViewSource.View.Refresh();
                context.SaveChanges();

            }
        }

        private async void RowProductosTerminados_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var fila = sender as DataGridRow;
            var productoTerminadoSeleccionado = ucTablaProductosTerminados.dgProductosTerminados.SelectedItem as ProductoTerminado;
            var formProductoTerminado = new FormProductoTerminado(context, productoTerminadoSeleccionado);
            var formProductoTerminadoDataContext = formProductoTerminado.DataContext as FormProductoTerminadoViewModel;
            var historialHuecosAlmacenajesIniciales = formProductoTerminadoDataContext.HistorialHuecosAlmacenajes.ToList();
            if ((bool)await DialogHost.Show(formProductoTerminado, "RootDialog"))
            {
                productoTerminadoSeleccionado.OrdenId = productoTerminadoSeleccionado.OrdenElaboracion.OrdenElaboracionId;
                productoTerminadoSeleccionado.TipoId = productoTerminadoSeleccionado.TipoProductoTerminado.TipoProductoTerminadoId;
                productoTerminadoSeleccionado.Unidades = formProductoTerminadoDataContext.Unidades;
                productoTerminadoSeleccionado.Volumen = formProductoTerminadoDataContext.Volumen;
                productoTerminadoSeleccionado.Observaciones = formProductoTerminadoDataContext.Observaciones;
                if (formProductoTerminadoDataContext.FechaBaja != null)
                {
                    productoTerminadoSeleccionado.FechaBaja = new DateTime(
                        formProductoTerminadoDataContext.FechaBaja.Value.Year,
                        formProductoTerminadoDataContext.FechaBaja.Value.Month,
                        formProductoTerminadoDataContext.FechaBaja.Value.Day,
                        formProductoTerminadoDataContext.HoraBaja.Value.Hour,
                        formProductoTerminadoDataContext.HoraBaja.Value.Minute,
                        formProductoTerminadoDataContext.HoraBaja.Value.Second);
                }
                else
                {
                    productoTerminadoSeleccionado.FechaBaja = null;
                }
                if (!context.ProductosEnvasadosComposiciones.Any(pec => pec.ProductoId == productoTerminadoSeleccionado.ProductoTerminadoId))
                {
                    // Se borran todos los historiales huecos almacenajes antiguos y se añaden los nuevos
                    context.HistorialHuecosAlmacenajes.RemoveRange(historialHuecosAlmacenajesIniciales);
                    var huecosProductosTerminados = new List<HistorialHuecoAlmacenaje>();
                    foreach (var hha in formProductoTerminadoDataContext.HistorialHuecosAlmacenajes)
                    {
                        var haId = hha.HuecoAlmacenaje.HuecoAlmacenajeId;
                        // Los huecos que no se ha añadido ninguna cantidad no se añaden
                        if (hha.Unidades != 0 && hha.Volumen != 0)
                        {
                            hha.HuecoAlmacenaje = null;
                            hha.HuecoAlmacenajeId = haId;
                            hha.ProductoTerminado = productoTerminadoSeleccionado;
                            huecosProductosTerminados.Add(hha);
                        }
                    }
                    context.HistorialHuecosAlmacenajes.AddRange(huecosProductosTerminados);
                }

                context.SaveChanges();
                productosTerminadosViewSource.View.Refresh();

                CargarProductosTerminados();
            }
        }

        private void CargarProductosTerminados()
        {
            var elaboracion = ucTablaElaboraciones.dgElaboraciones.SelectedItem as OrdenElaboracion;
            if (elaboracion != null)
            {
                ucTablaProductosTerminados.bAnadirProductoTerminado.IsEnabled = elaboracion.EstadoElaboracionId == 1;
                productosTerminadosViewSource.Source = context.ProductosTerminados.Where(pt => pt.OrdenId == elaboracion.OrdenElaboracionId).ToList();
            }
            else
            {
                ucTablaProductosTerminados.bAnadirProductoTerminado.IsEnabled = false;
                productosTerminadosViewSource.Source = null;
            }
        }




    }
}
