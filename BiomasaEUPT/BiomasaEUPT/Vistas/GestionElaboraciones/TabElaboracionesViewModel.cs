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

        // Checkbox Filtro Elaboraciones
        public bool FechaOrdenElaboracionSeleccionada { get; set; } = true;
        public bool EstadoElaboracionSeleccionado { get; set; } = false;

        private string _textoFiltroOrdenesElaboraciones;
        public string TextoFiltroOrdenesElaboraciones
        {
            get { return _textoFiltroOrdenesElaboraciones; }
            set
            {
                _textoFiltroOrdenesElaboraciones = value.ToLower();
                FiltrarOrdenesElaboraciones();
            }
        }

        // Checkbox Filtro ProductosTerminados
        public bool FechaBajaProductoTerminadoSeleccionado { get; set; } = false;
        public bool TipoProductoTerminadoSeleccionado { get; set; } = true;
        public bool GrupoProductoTerminadoSeleccionado { get; set; } = true;
        public bool TipoMateriaPrimaSeleccionado { get; set; } = true;
        public bool GrupoMateriaPrimaSeleccionado { get; set; } = true;
        public bool VolUniProductoTerminadoSeleccionado { get; set; } = false;

        private string _textoFiltroProductosTerminados;
        public string TextoFiltroProductosTerminados
        {
            get { return _textoFiltroProductosTerminados; }
            set
            {
                _textoFiltroProductosTerminados = value.ToLower();
                FiltrarProductosTerminados();
            }
        }

        private ICommand _anadirOrdenElaboracionComando;
        private ICommand _modificarOrdenElaboracionComando;
        private ICommand _borrarOrdenElaboracionComando;
        private ICommand _refrescarOrdenesElaboracionesComando;
        private ICommand _filtrarOrdenesElaboracionesComando;
        private ICommand _dgOrdenesElaboraciones_SelectionChangedComando;

        private ICommand _anadirProductoTerminadoComando;
        private ICommand _modificarProductoTerminadoComando;
        private ICommand _borrarProductoTerminadoComando;
        private ICommand _refrescarProductosTerminadosComando;
        private ICommand _filtrarProductosTerminadosComando;
        private ICommand _modificarObservacionesProductoTerminadoComando;

        private BiomasaEUPTContext context;
        public PaginacionViewModel PaginacionViewModel { get; set; }
        public MasOpcionesElaboracionesViewModel MasOpcionesElaboracionesViewModel { get; set; }

        public TabElaboracionesViewModel()
        {
            PaginacionViewModel = new PaginacionViewModel();
            MasOpcionesElaboracionesViewModel = new MasOpcionesElaboracionesViewModel();
        }

        public override void Inicializar()
        {
            context = new BiomasaEUPTContext();
            CargarOrdenesElaboraciones();
            PaginacionViewModel.GetItemsTotales = () => { return context.OrdenesElaboraciones.Count(); };
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
            (_dgOrdenesElaboraciones_SelectionChangedComando = new RelayCommandGenerico<IList<object>>(
                param => DGOrdenesElaboraciones_SelectionChanged(param)
            ));

        private void DGOrdenesElaboraciones_SelectionChanged(IList<object> ordenesElaboracionesSeleccionadas)
        {
            OrdenesElaboracionesSeleccionadas = ordenesElaboracionesSeleccionadas.Cast<OrdenElaboracion>().ToList();
            CargarProductosTerminados();
        }

        // Asigna el valor de ProductosTerminadosSeleccinodos ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGProductosTerminados_SelectionChangedComando => new RelayCommandGenerico<IList<object>>(param => ProductosTerminadosSeleccionados = param.Cast<ProductoTerminado>().ToList());

        #region Añadir Orden Elaboración
        public ICommand AnadirOrdenElaboracionComando => _anadirOrdenElaboracionComando ??
            (_anadirOrdenElaboracionComando = new RelayCommand(
                param => AnadirOrdenElaboracion()
            ));

        private async void AnadirOrdenElaboracion()
        {
            var formElaboracion = new FormElaboracion(context);

            if ((bool)await DialogHost.Show(formElaboracion, "RootDialog"))
            {
                context.OrdenesElaboraciones.Add(new OrdenElaboracion()
                {
                    Descripcion= formElaboracion.Descripcion,
                    EstadoElaboracionId=1
                });
                context.SaveChanges();
            }
        }
        #endregion

        
        #region Borrar Orden Elaboración
        public ICommand BorrarOrdenElaboracionComando => _borrarOrdenElaboracionComando ??
            (_borrarOrdenElaboracionComando = new RelayCommandGenerico<IList<object>>(
                param => BorrarOrdenElaboracion(),
                param => OrdenElaboracionSeleccionada != null
            ));

        private async void BorrarOrdenElaboracion()
        {
            string pregunta = OrdenesElaboracionesSeleccionadas.Count == 1
                   ? "¿Está seguro de que desea borrar la orden de elaboración " + OrdenElaboracionSeleccionada.EstadoElaboracionId + "?"
                   : "¿Está seguro de que desea borrar las órdenes de elaboración seleccionadas?";

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

        
        #region Modificar Orden Elaboración
        public ICommand ModificarOrdenElaboracíonComando => _modificarOrdenElaboracionComando ??
            (_modificarOrdenElaboracionComando = new RelayCommand(
                param => ModificarOrdenElaboracion(),
                param => OrdenElaboracionSeleccionada != null
             ));

        public async void ModificarOrdenElaboracion()
        {

            var formElaboracion = new FormElaboracion(context, "Editar Elaboración")
            {
                
                /*Descripcion= formElaboracion.Descripcion,
                NumeroAlbaran = RecepcionSeleccionada.NumeroAlbaran,
                FechaElaboracion = RecepcionSeleccionada.FechaRecepcion,
                Hora = RecepcionSeleccionada.FechaRecepcion*/
            };
            /*formElaboracion.cbEstadosElaboraciones.Visibility = Visibility.Visible;
            var albaranViejo = formRecepcion.NumeroAlbaran;
            formRecepcion.vAlbaranUnico.NombreActual = RecepcionSeleccionada.NumeroAlbaran;
            formRecepcion.cbEstadosRecepciones.SelectedValue = RecepcionSeleccionada.EstadoRecepcion.EstadoRecepcionId;
            formRecepcion.cbProveedores.SelectedValue = RecepcionSeleccionada.Proveedor.ProveedorId;*/
            if ((bool)await DialogHost.Show(formElaboracion, "RootDialog"))
            {
                /*RecepcionSeleccionada.NumeroAlbaran = formRecepcion.NumeroAlbaran;
                RecepcionSeleccionada.FechaRecepcion = new DateTime(formRecepcion.Fecha.Year, formRecepcion.Fecha.Month, formRecepcion.Fecha.Day, formRecepcion.Hora.Hour, formRecepcion.Hora.Minute, formRecepcion.Hora.Second);
                RecepcionSeleccionada.ProveedorId = (formRecepcion.cbProveedores.SelectedItem as Proveedor).ProveedorId;
                RecepcionSeleccionada.EstadoId = (formRecepcion.cbEstadosRecepciones.SelectedItem as EstadoRecepcion).EstadoRecepcionId;*/
                context.SaveChanges();
                OrdenesElaboracionesView.Refresh();
            }
        }
        #endregion

        
        #region Refrescar Ordenes Elaboración
        public ICommand RefrescarOrdenesElaboracionesComando => _refrescarOrdenesElaboracionesComando ??
            (_refrescarOrdenesElaboracionesComando = new RelayCommand(
                param => RefrescarOrdenesElaboraciones()
             ));

        public void RefrescarOrdenesElaboraciones()
        {
            PaginacionViewModel.Refrescar();
            OrdenElaboracionSeleccionada = null;
        }
        #endregion

       
        #region Filtro Orden Elaboracion
        public ICommand FiltrarOrdenesElaboracionesComando => _filtrarOrdenesElaboracionesComando ??
           (_filtrarOrdenesElaboracionesComando = new RelayCommand(
                param => FiltrarOrdenesElaboraciones()
           ));

        public void FiltrarOrdenesElaboraciones()
        {
            OrdenesElaboracionesView.Filter = FiltroOrdenesElaboraciones;
            OrdenesElaboracionesView.Refresh();
        }

        private bool FiltroOrdenesElaboraciones(object item)
        {
            var ordenElaboracion = item as OrdenElaboracion;

            string fechaElaboracion = ordenElaboracion.FechaElaboracion.ToString();
            string estado = ordenElaboracion.EstadoElaboracion.Nombre.ToLower();

            return (FechaOrdenElaboracionSeleccionada == true ? fechaElaboracion.Contains(TextoFiltroOrdenesElaboraciones) : false)
                || (EstadoElaboracionSeleccionado == true ? estado.Contains(TextoFiltroOrdenesElaboraciones) : false);
        }
        #endregion

       
        #region Añadir Producto Terminado
        public ICommand AnadirProductoTerminadoComando => _anadirProductoTerminadoComando ??
            (_anadirProductoTerminadoComando = new RelayCommand(
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

                    OrdenId = OrdenElaboracionSeleccionada.OrdenElaboracionId,
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
        #endregion

       
        #region Borrar Producto Terminado 
        public ICommand BorrarProductoTerminadoComando => _borrarProductoTerminadoComando ??
            (_borrarProductoTerminadoComando = new RelayCommandGenerico<IList<object>>(
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
                    if (!context.OrdenesElaboraciones.Any(oe => oe.OrdenElaboracionId == pt.OrdenId))
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
                           : "No se han podido borrar todos los productos terminados seleccionados.";
                    mensaje += "\n\nAsegurese de no que no exista ningún producto terminado elaborado con dicha materia prima.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
            }
        }
        #endregion

       
        #region Modificar Producto Terminado
        public ICommand ModificarProductoTerminadoComando => _modificarProductoTerminadoComando ??
            (_modificarProductoTerminadoComando = new RelayCommand(
                param => ModificarProductoTerminado(),
                param => ProductoTerminadoSeleccionado != null
             ));

        public async void ModificarProductoTerminado()
        {
            var formProductoTerminado = new FormProductoTerminado(context, ProductoTerminadoSeleccionado);
            var formProductoTerminadoDataContext = formProductoTerminado.DataContext as FormProductoTerminadoViewModel;
            var historialHuecosAlmacenajesIniciales = formProductoTerminadoDataContext.HistorialHuecosAlmacenajes.ToList();
            if ((bool)await DialogHost.Show(formProductoTerminado, "RootDialog"))
            {
                ProductoTerminadoSeleccionado.TipoId = formProductoTerminadoDataContext.TipoMateriaPrima.TipoMateriaPrimaId;
                ProductoTerminadoSeleccionado.Unidades = formProductoTerminadoDataContext.Unidades;
                ProductoTerminadoSeleccionado.Volumen = formProductoTerminadoDataContext.Volumen;
                ProductoTerminadoSeleccionado.Observaciones = formProductoTerminadoDataContext.Observaciones;
                if (formProductoTerminadoDataContext.FechaBaja != null)
                {
                    ProductoTerminadoSeleccionado.FechaBaja = new DateTime(
                        formProductoTerminadoDataContext.FechaBaja.Value.Year,
                        formProductoTerminadoDataContext.FechaBaja.Value.Month,
                        formProductoTerminadoDataContext.FechaBaja.Value.Day,
                        formProductoTerminadoDataContext.HoraBaja.Value.Hour,
                        formProductoTerminadoDataContext.HoraBaja.Value.Minute,
                        formProductoTerminadoDataContext.HoraBaja.Value.Second);
                }
                else
                {
                    ProductoTerminadoSeleccionado.FechaBaja = null;
                }
                if (!context.ProductosTerminadosComposiciones.Any(ptc => ptc.ProductoId == ProductoTerminadoSeleccionado.ProductoTerminadoId))
                {
                    // Se borran todos los historiales huecos almacenajes antiguos y se añaden los nuevos
                    context.HistorialHuecosAlmacenajes.RemoveRange(historialHuecosAlmacenajesIniciales);
                    var huecosProductosTerminados = new List<HistorialHuecoAlmacenaje>();
                    foreach (var hpt in formProductoTerminadoDataContext.HistorialHuecosAlmacenajes)
                    {
                        var haId = hpt.HuecoAlmacenaje.HuecoAlmacenajeId;
                        // Los huecos que no se ha añadido ninguna cantidad no se añaden
                        if (hpt.Unidades != 0 && hpt.Volumen != 0)
                        {
                            hpt.HuecoAlmacenaje = null;
                            hpt.HuecoAlmacenajeId = haId;
                            hpt.ProductoTerminado = ProductoTerminadoSeleccionado;
                            huecosProductosTerminados.Add(hpt);
                        }
                    }
                    context.HistorialHuecosAlmacenajes.AddRange(huecosProductosTerminados);
                }

                context.SaveChanges();
                ProductosTerminadosView.Refresh();
            }
        }
        #endregion

        
        #region Refrescar ProductosTerminados
        public ICommand RefrescarProductosTerminadosComando => _refrescarProductosTerminadosComando ??
            (_refrescarProductosTerminadosComando = new RelayCommand(
                param => CargarProductosTerminados(),
                param => OrdenElaboracionSeleccionada != null
             ));
        #endregion

        
        #region Modificar Observaciones Producto Terminado
        public ICommand ModificarObservacionesProductoTerminadoComando => _modificarObservacionesProductoTerminadoComando ??
            (_modificarObservacionesProductoTerminadoComando = new RelayCommand(
                param => ModificarObservacionesProductoTerminado(),
                param => ProductoTerminadoSeleccionado != null
             ));

        private void ModificarObservacionesProductoTerminado()
        {
            /*var materiaPrima = context.MateriasPrimas.Single(mp => mp.MateriaPrimaId == MateriaPrimaSeleccionada.MateriaPrimaId);
            materiaPrima.Observaciones = MateriaPrimaSeleccionada.Observaciones;*/
            context.SaveChanges();

            ObservacionesProductosTerminadosEnEdicion = false;
        }
        #endregion

        
        #region Filtro Productos Terminados
        public ICommand FiltrarProductosTerminadosComando => _filtrarProductosTerminadosComando ??
           (_filtrarProductosTerminadosComando = new RelayCommand(
                param => FiltrarProductosTerminados()
           ));

        public void FiltrarProductosTerminados()
        {
            ProductosTerminadosView.Filter = FiltroProductosTerminados;
            ProductosTerminadosView.Refresh();
        }

        private bool FiltroProductosTerminados(object item)
        {
            var productoTerminado = item as ProductoTerminado;
            string tipo = productoTerminado.TipoProductoTerminado.Nombre.ToLower();
            string grupo = productoTerminado.TipoProductoTerminado.GrupoProductoTerminado.Nombre.ToLower();
            string volumen = productoTerminado.Volumen.ToString();
            string unidades = productoTerminado.Unidades.ToString();
            string fechaBaja = productoTerminado.FechaBaja.ToString();

            return (FechaBajaProductoTerminadoSeleccionado == true ? fechaBaja.Contains(TextoFiltroProductosTerminados) : false)
                || (TipoProductoTerminadoSeleccionado == true ? tipo.Contains(TextoFiltroProductosTerminados) : false)
                || (GrupoProductoTerminadoSeleccionado == true ? grupo.Contains(TextoFiltroProductosTerminados) : false)
                || (VolUniProductoTerminadoSeleccionado == true ? (volumen.Contains(TextoFiltroProductosTerminados) || unidades.Contains(TextoFiltroProductosTerminados)) : false);

        }
        #endregion
    }
}
