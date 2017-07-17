using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
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
using System.Windows.Data;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.GestionElaboraciones
{
    public class TabElaboracionesViewModel : ViewModelBase
    {

        public ObservableCollection<OrdenElaboracion> OrdenesElaboraciones { get; set; }
        public CollectionView OrdenesElaboracionesView { get; private set; }
        public IList<OrdenElaboracion> OrdenesElaboracionesSeleccionadas { get; set; }
        public OrdenElaboracion OrdenElaboracionSeleccionada { get; set; }

        public ObservableCollection<ProductoTerminado> ProductosTerminados { get; set; }
        public CollectionView ProductosTerminadosView { get; private set; }
        public IList<ProductoTerminado> ProductosTerminadosSeleccionados { get; set; }
        public ProductoTerminado ProductoTerminadoSeleccionado { get; set; }
        public bool ObservacionesProductosTerminadosEnEdicion { get; set; }

        private ICommand _anadirOrdenElaboracionComando;
        private ICommand _modificarOrdenElaboracionComando;
        private ICommand _borrarOrdenElaboracionComando;
        private ICommand _refrescarOrdenesElaboracionesComando;
        private ICommand _dgOrdenesElaboraciones_SelectionChangedComando;

        private ICommand _anadirProductoTerminadoComando;
        private ICommand _modificarProductoTerminadoComando;
        private ICommand _borrarProductoTerminadoComando;
        private ICommand _refrescarProductosTerminadosComando;
        private ICommand _modificarObservacionesProductoTerminadoComando;
        private BiomasaEUPTContext context;

        public PaginacionViewModel PaginacionViewModel { get; set; }

        public TabElaboracionesViewModel()
        {
            PaginacionViewModel = new PaginacionViewModel();
        }

        public override void Inicializar()
        {
            context = new BiomasaEUPTContext();
            CargarOrdenesElaboraciones();
            PaginacionViewModel.GetItemsTotales = () => { return context.Recepciones.Count(); };
            PaginacionViewModel.ActualizarContadores();
            PaginacionViewModel.CargarItems = CargarOrdenesElaboraciones;
        }

        public void CargarOrdenesElaboraciones(int cantidad = 10, int saltar = 0)
        {
            using (new CursorEspera())
            {
                OrdenesElaboraciones = new ObservableCollection<OrdenElaboracion>(
                    context.OrdenesElaboraciones
                    .Include(oe => oe.EstadoElaboracion).Include(oe => oe.ProductoTerminado)
                    .OrderBy(r => r.EstadoElaboracionId).Skip(saltar).Take(cantidad).ToList());
                OrdenesElaboracionesView = (CollectionView)CollectionViewSource.GetDefaultView(OrdenesElaboraciones);

                // Por defecto no está seleccionada ninguna fila del datagrid ordenesElaboraciones
                OrdenElaboracionSeleccionada = null;

            }
        }

        public void CargarProductosTerminados()
        {
            if (OrdenElaboracionSeleccionada != null)
            {
                using (new CursorEspera())
                {
                    ProductosTerminados = new ObservableCollection<ProductoTerminado>(
                          context.ProductosTerminados.
                          Where(pt => pt.OrdenId == OrdenElaboracionSeleccionada.OrdenElaboracionId)
                          .Include(pt => pt.HistorialHuecosAlmacenajes)
                          .Include(pt => pt.TipoProductoTerminado.GrupoProductoTerminado)
                          .ToList());
                    ProductosTerminadosView = (CollectionView)CollectionViewSource.GetDefaultView(ProductosTerminados);

                    ProductoTerminadoSeleccionado = null;
                }
            }
            else
            {
                // ucTablaProductosTerminados.bAnadirProductoTerminado.IsEnabled = false;
                ProductosTerminados = null;
            }
        }

        // Asigna el valor de OrdenesElaboracionesSeleccinodas ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGOrdenesElaboraciones_SelectionChangedComando => _dgOrdenesElaboraciones_SelectionChangedComando ??
            (_dgOrdenesElaboraciones_SelectionChangedComando = new RelayCommand2<IList<object>>(
                param => DGOrdenesElaboraciones_SelectionChanged(param)
            ));

        private void DGOrdenesElaboraciones_SelectionChanged(IList<object> ordenesElaboracionesSeleccionadas)
        {
            OrdenesElaboracionesSeleccionadas = ordenesElaboracionesSeleccionadas.Cast<OrdenElaboracion>().ToList();
            CargarProductosTerminados();
        }

        // Asigna el valor de ProductosTerminadosSeleccinodos ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGProductosTerminados_SelectionChangedComando => new RelayCommand2<IList<object>>(param => ProductosTerminadosSeleccionados = param.Cast<ProductoTerminado>().ToList());

        #region Añadir OrdenElaboracion
        public ICommand AnadirOrdenElaboracionComando => _anadirOrdenElaboracionComando ??
            (_anadirOrdenElaboracionComando = new RelayComando(
                param => AnadirOrdenElaboacion()
            ));

        private async void AnadirOrdenElaboacion()
        {
            var formOrdenElaboracion = new FormOrdenElaboracion(context);

            if ((bool)await DialogHost.Show(formOrdenElaboracion, "RootDialog"))
            {
                context.OrdenesElaboraciones.Add(new OrdenElaboracion()
                {
                    /*NumeroAlbaran = formRecepcion.NumeroAlbaran,
                    FechaRecepcion = new DateTime(formRecepcion.Fecha.Year, formRecepcion.Fecha.Month, formRecepcion.Fecha.Day, formRecepcion.Hora.Hour, formRecepcion.Hora.Minute, formRecepcion.Hora.Second),
                    ProveedorId = (formRecepcion.cbProveedores.SelectedItem as Proveedor).ProveedorId,
                    EstadoId = 1*/
                });
                context.SaveChanges();
            }
        }
        #endregion

        #region Borrar Recepción
        public ICommand BorrarrdenElaboracionComando => _borrarOrdenElaboracionComando ??
            (_borrarOrdenElaboracionComando = new RelayCommand2<IList<object>>(
                param => BorrarOrdenElaboracion(),
                param => OrdenElaboracionSeleccionada != null
            ));

        private async void BorrarOrdenElaboracion()
        {
            string pregunta = OrdenesElaboracionesSeleccionadas.Count == 1
                   ? "¿Está seguro de que desea borrar la orden de elaboración " + OrdenElaboracionSeleccionada.OrdenElaboracionId + "?"
                   : "¿Está seguro de que desea borrar las órdenes de elaboraciones seleccionadas?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {
                var ordenesElaboracionesABorrar = new List<OrdenElaboracion>();

                foreach (var ordenElaboracion in OrdenesElaboracionesSeleccionadas)
                {
                    if (!context.ProductosTerminados.Any(pt => pt.OrdenId == ordenElaboracion.OrdenElaboracionId))
                    {
                        ordenesElaboracionesABorrar.Add(ordenElaboracion);
                    }
                }
                context.OrdenesElaboraciones.RemoveRange(ordenesElaboracionesABorrar);
                context.SaveChanges();

                if (OrdenesElaboracionesSeleccionadas.Count != ordenesElaboracionesABorrar.Count)
                {
                    string mensaje = OrdenesElaboracionesSeleccionadas.Count == 1
                           ? "No se ha podido borrar la orden de elaboración seleccionada."
                           : "No se han podido borrar todas las órdenes de elaboraciones seleccionadas.";
                    mensaje += "\n\nAsegurese de no que no exista ningún producto terminado asociado a dicha orden de elaboración.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
                PaginacionViewModel.Refrescar();
            }
        }
        #endregion

        #region Añadir ProductoTerminado
        public ICommand AnadirMateriaPrimaComando => _anadirProductoTerminadoComando ??
            (_anadirProductoTerminadoComando = new RelayComando(
                param => AnadirProductoTerminado(),
                param => CanAnadirProductoTerminado()
            ));

        private bool CanAnadirProductoTerminado()
        {
            if (OrdenElaboracionSeleccionada != null)
            {
                return OrdenElaboracionSeleccionada.EstadoElaboracionId == 1; // Disponible
            }
            return false;
        }

        private async void AnadirProductoTerminado()
        {
            var formProductoTerminado = new FormProductoTerminado(context);

            if ((bool)await DialogHost.Show(formProductoTerminado, "RootDialog"))
            {
                var formProductoTerminadoDataContext = formProductoTerminado.DataContext as FormProductoTerminadoViewModel;
                var productoTerminado = new ProductoTerminado()
                {
                    /*RecepcionId = RecepcionSeleccionada.RecepcionId,
                    Observaciones = formMateriaPrimaDataContext.Observaciones,
                    //Codigo = formMateriaPrima.Codigo,
                    ProcedenciaId = (formMateriaPrima.cbProcedencias.SelectedItem as Procedencia).ProcedenciaId,
                    TipoId = (formMateriaPrima.cbTiposMateriasPrimas.SelectedItem as TipoMateriaPrima).TipoMateriaPrimaId,
                    Volumen = formMateriaPrimaDataContext.Volumen,
                    Unidades = formMateriaPrimaDataContext.Unidades*/
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
        #endregion

        #region Borrar ProductoTerminado   
        public ICommand BorrarProductoTerminadoComando => _borrarProductoTerminadoComando ??
            (_borrarProductoTerminadoComando = new RelayCommand2<IList<object>>(
                param => BorrarProductoTerminado(),
                param => ProductoTerminadoSeleccionado != null
            ));

        private async void BorrarProductoTerminado()
        {
            string pregunta = ProductosTerminadosSeleccionados.Count == 1
                ? "¿Está seguro de que desea borrar el producto terminado con código " + ProductoTerminadoSeleccionado.Codigo + "?"
                : "¿Está seguro de que desea borrar los productos terminados seleccionados?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {
                List<ProductoTerminado> productosTerminadosABorrar = new List<ProductoTerminado>();

                foreach (var pt in ProductosTerminadosSeleccionados)
                {
                    if (!context.ProductosTerminados.Any(ptc => ptc.ProductoTerminadoId == pt.ProductoTerminadoId))
                    {
                        productosTerminadosABorrar.Add(pt);
                    }
                }
                context.ProductosTerminados.RemoveRange(productosTerminadosABorrar);
                context.SaveChanges();
                CargarProductosTerminados();

                if (ProductosTerminadosSeleccionados.Count != productosTerminadosABorrar.Count)
                {
                    string mensaje = ProductosTerminadosSeleccionados.Count == 1
                           ? "No se ha podido borrar el producto terminado seleccionado."
                           : "No se han podido borrar todas los productos terminados seleccionados.";
                    mensaje += "\n\nAsegurese de no que no exista ningun producto terminado elaborado con dicha materia prima.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
            }
        }
        #endregion



    }
}
