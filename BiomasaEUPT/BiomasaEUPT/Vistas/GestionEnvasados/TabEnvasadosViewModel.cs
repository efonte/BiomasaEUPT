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

namespace BiomasaEUPT.Vistas.GestionEnvasados
{
    public class TabEnvasadosViewModel : ViewModelBase
    {

        public ObservableCollection<OrdenEnvasado> OrdenesEnvasados { get; set; }
        public CollectionView OrdenesEnvasadosView { get; private set; }
        public IList<OrdenEnvasado> OrdenesEnvasadosSeleccionadas { get; set; }
        public OrdenEnvasado OrdenEnvasadoSeleccionada { get; set; }

        public ObservableCollection<ProductoEnvasado> ProductosEnvasados { get; set; }
        public CollectionView ProductosEnvasadosView { get; private set; }
        public IList<ProductoEnvasado> ProductosEnvasadosSeleccionados { get; set; }
        public ProductoEnvasado ProductoEnvasadoSeleccionado { get; set; }
        public bool ObservacionesProductosEnvasadosEnEdicion { get; set; }

        public int IndiceMasOpciones { get; set; }

        // Checkbox Filtro Envasado
        public bool FechaOrdenEnvasadoSeleccionada { get; set; } = true;
        public bool EstadoOrdenEnvasadoSeleccionado { get; set; } = false;

        private string _textoFiltroOrdenesEnvasados;
        public string TextoFiltroOrdenesEnvasados
        {
            get { return _textoFiltroOrdenesEnvasados; }
            set
            {
                _textoFiltroOrdenesEnvasados = value.ToLower();
                FiltrarOrdenesEnvasados();
            }
        }

        // Checkbox Filtro Productos Envasados
        public bool TipoProductoEnvasadoSeleccionado { get; set; } = true;
        public bool GrupoProductoEnvasadoSeleccionado { get; set; } = true;
        public bool VolUniProductoEnvasadoSeleccionado { get; set; } = false;

        private string _textoFiltroProductosEnvasados;
        public string TextoFiltroProductosEnvasados
        {
            get { return _textoFiltroProductosEnvasados; }
            set
            {
                _textoFiltroProductosEnvasados = value.ToLower();
                FiltrarProductosEnvasados();
            }
        }

        private ICommand _anadirOrdenEnvasadoComando;
        private ICommand _modificarOrdenEnvasadoComando;
        private ICommand _borrarOrdenEnvasadoComando;
        private ICommand _refrescarOrdenesEnvasadosComando;
        private ICommand _filtrarOrdenesEnvasadosComando;
        private ICommand _dgOrdenesEnvasados_SelectionChangedComando;

        private ICommand _anadirProductoEnvasadoComando;
        private ICommand _modificarProductoEnvasadoComando;
        private ICommand _borrarProductoEnvasadoComando;
        private ICommand _refrescarProductosEnvasadoComando;
        private ICommand _filtrarProductosEnvasadosComando;
        private ICommand _modificarObservacionesProductoEnvasadoComando;

        private BiomasaEUPTContext context;
        public PaginacionViewModel PaginacionViewModel { get; set; }
        public MasOpcionesEnvasadosViewModel MasOpcionesEnvasadosViewModel { get; set; }


        public TabEnvasadosViewModel()
        {
            PaginacionViewModel = new PaginacionViewModel();
            MasOpcionesEnvasadosViewModel = new MasOpcionesEnvasadosViewModel();
        }

        public override void Inicializar()
        {
            using (new CursorEspera())
            {
                IndiceMasOpciones = 0;
                context = new BiomasaEUPTContext();
                CargarOrdenesEnvasados();
                PaginacionViewModel.GetItemsTotales = () => { return context.OrdenesEnvasados.Count(); };
                PaginacionViewModel.ActualizarContadores();
                PaginacionViewModel.CargarItems = CargarOrdenesEnvasados;
            }
        }

        public void CargarOrdenesEnvasados(int cantidad = 10, int saltar = 0)
        {
            using (new CursorEspera())
            {
                // Si los envasados disponibles son menores que la cantidad a coger,
                // se obtienen todas
                if (context.OrdenesEnvasados.Count() < cantidad)
                {
                    OrdenesEnvasados = new ObservableCollection<OrdenEnvasado>(context.OrdenesEnvasados.ToList());
                }
                else
                {
                    OrdenesEnvasados = new ObservableCollection<OrdenEnvasado>(
                    context.OrdenesEnvasados
                    .Include(oe => oe.EstadoEnvasado).Include(oe => oe.ProductoEnvasado)
                    .OrderBy(r => r.EstadoEnvasadoId).Skip(saltar).Take(cantidad).ToList());
                }
                OrdenesEnvasadosView = (CollectionView)CollectionViewSource.GetDefaultView(OrdenesEnvasados);

                // Por defecto no está seleccionada ninguna fila del datagrid ordenesEnvasado
                OrdenEnvasadoSeleccionada = null;

            }
        }

        public void CargarProductosEnvasados()
        {
            if (OrdenEnvasadoSeleccionada != null)
            {
                using (new CursorEspera())
                {
                    ProductosEnvasados = new ObservableCollection<ProductoEnvasado>(
                          context.ProductosEnvasados.
                          Where(pe => pe.OrdenId == OrdenEnvasadoSeleccionada.OrdenEnvasadoId)
                          .Include(pe => pe.Picking)
                          .Include(pe => pe.TipoProductoEnvasado.GrupoProductoEnvasado)
                          .ToList());
                    ProductosEnvasadosView = (CollectionView)CollectionViewSource.GetDefaultView(ProductosEnvasados);

                    ProductoEnvasadoSeleccionado = null;
                }
            }
            else
            {
                // ucTablaProductosEnvasado.bAnadirProductoEnvasado.IsEnabled = false;
                ProductosEnvasados = null;
            }
        }

        // Asigna el valor de OrdenesEnvasadosSeleccinodas ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGOrdenesEnvasados_SelectionChangedComando => _dgOrdenesEnvasados_SelectionChangedComando ??
            (_dgOrdenesEnvasados_SelectionChangedComando = new RelayCommandGenerico<IList<object>>(
                param => DGOrdenesEnvasados_SelectionChanged(param)
            ));

        private void DGOrdenesEnvasados_SelectionChanged(IList<object> ordenesEnvasadosSeleccionadas)
        {
            OrdenesEnvasadosSeleccionadas = ordenesEnvasadosSeleccionadas.Cast<OrdenEnvasado>().ToList();
            CargarProductosEnvasados();
        }

        // Asigna el valor de ProductosEnvasadosSeleccinodos ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGProductosEnvasados_SelectionChangedComando => new RelayCommandGenerico<IList<object>>(param => ProductosEnvasadosSeleccionados = param.Cast<ProductoEnvasado>().ToList());


        #region Añadir Orden Envasado
        public ICommand AnadirOrdenEnvasadoComando => _anadirOrdenEnvasadoComando ??
            (_anadirOrdenEnvasadoComando = new RelayCommand(
                param => AnadirOrdenEnvasado()
            ));

        private async void AnadirOrdenEnvasado()
        {
            var formEnvasado = new FormEnvasado(context);

            if ((bool)await DialogHost.Show(formEnvasado, "RootDialog"))
            {
                context.OrdenesEnvasados.Add(new OrdenEnvasado()
                {
                    Descripcion = formEnvasado.Descripcion,
                    EstadoEnvasadoId = 1
                });
                context.SaveChanges();
            }

            //CargarOrdenesEnvasados();
        }
        #endregion


        #region Borrar Orden Envasado
        public ICommand BorrarOrdenEnvasadoComando => _borrarOrdenEnvasadoComando ??
            (_borrarOrdenEnvasadoComando = new RelayCommandGenerico<IList<object>>(
                param => BorrarOrdenEnvasado(),
                param => OrdenEnvasadoSeleccionada != null
            ));

        private async void BorrarOrdenEnvasado()
        {
            string pregunta = OrdenesEnvasadosSeleccionadas.Count == 1
                   ? "¿Está seguro de que desea borrar la orden de elaboración " + OrdenEnvasadoSeleccionada.EstadoEnvasadoId + "?"
                   : "¿Está seguro de que desea borrar las órdenes de envasados seleccionadas?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {
                var ordenesEnvasadosABorrar = new List<OrdenEnvasado>();

                foreach (var ordenEnvasado in OrdenesEnvasadosSeleccionadas)
                {
                    if (!context.ProductosEnvasados.Any(pe => pe.OrdenId == ordenEnvasado.OrdenEnvasadoId))
                    {
                        ordenesEnvasadosABorrar.Add(ordenEnvasado);
                    }
                }
                context.OrdenesEnvasados.RemoveRange(OrdenesEnvasadosSeleccionadas);
                context.SaveChanges();

                if (OrdenesEnvasadosSeleccionadas.Count != OrdenesEnvasadosSeleccionadas.Count)
                {
                    string mensaje = OrdenesEnvasadosSeleccionadas.Count == 1
                           ? "No se ha podido borrar la orden de envasado seleccionada."
                           : "No se han podido borrar todas las órdenes de envasado seleccionadas.";
                    mensaje += "\n\nAsegurese de no que no exista ningún producto envasado asociado a dicha orden de envasado.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
                PaginacionViewModel.Refrescar();
            }
        }
        #endregion


        #region Modificar Orden Envasado
        public ICommand ModificarOrdenEnvasadoComando => _modificarOrdenEnvasadoComando ??
            (_modificarOrdenEnvasadoComando = new RelayCommand(
                param => ModificarOrdenEnvasado(),
                param => OrdenEnvasadoSeleccionada != null
             ));

        public async void ModificarOrdenEnvasado()
        {

            var formEnvasado = new FormEnvasado(context, "Editar Orden de Envasado")
            {

                Descripcion= OrdenEnvasadoSeleccionada.Descripcion
            };
            formEnvasado.cbEstadosEnvasados.Visibility = Visibility.Visible;
            formEnvasado.cbEstadosEnvasados.SelectedValue = OrdenEnvasadoSeleccionada.EstadoEnvasado.EstadoEnvasadoId;
            if ((bool)await DialogHost.Show(formEnvasado, "RootDialog"))
            {
                //RecepcionSeleccionada.FechaRecepcion = new DateTime(formRecepcion.Fecha.Year, formRecepcion.Fecha.Month, formRecepcion.Fecha.Day, formRecepcion.Hora.Hour, formRecepcion.Hora.Minute, formRecepcion.Hora.Second);
                OrdenEnvasadoSeleccionada.EstadoEnvasadoId = (formEnvasado.cbEstadosEnvasados.SelectedItem as EstadoEnvasado).EstadoEnvasadoId;
                context.SaveChanges();
                OrdenesEnvasadosView.Refresh();
            }
        }
        #endregion


        #region Refrescar Ordenes Envasados
        public ICommand RefrescarOrdenesEnvasadosComando => _refrescarOrdenesEnvasadosComando ??
            (_refrescarOrdenesEnvasadosComando = new RelayCommand(
                param => RefrescarOrdenesEnvasados()
             ));

        public void RefrescarOrdenesEnvasados()
        {
            PaginacionViewModel.Refrescar();
            OrdenEnvasadoSeleccionada = null;
        }
        #endregion


        #region Filtro Orden Envasado
        public ICommand FiltrarOrdenesEnvasadosComando => _filtrarOrdenesEnvasadosComando ??
           (_filtrarOrdenesEnvasadosComando = new RelayCommand(
                param => FiltrarOrdenesEnvasados()
           ));

        public void FiltrarOrdenesEnvasados()
        {
            OrdenesEnvasadosView.Filter = FiltroOrdenesEnvasados;
            OrdenesEnvasadosView.Refresh();
        }

        private bool FiltroOrdenesEnvasados(object item)
        {
            var ordenEnvasado = item as OrdenEnvasado;

            string fechaEnvasado = ordenEnvasado.FechaEnvasado.ToString();
            string estado = ordenEnvasado.EstadoEnvasado.Nombre.ToLower();

            return (FechaOrdenEnvasadoSeleccionada == true ? fechaEnvasado.Contains(TextoFiltroOrdenesEnvasados) : false)
                || (EstadoOrdenEnvasadoSeleccionado == true ? estado.Contains(TextoFiltroOrdenesEnvasados) : false);
        }
        #endregion

        #region Añadir Producto Envasado
        public ICommand AnadirProductoEnvasadoComando => _anadirProductoEnvasadoComando ??
            (_anadirProductoEnvasadoComando = new RelayCommand(
                param => AnadirProductoEnvasado(),
                param => CanAnadirProductoEnvasado()
            ));

        private bool CanAnadirProductoEnvasado()
        {
            if (OrdenEnvasadoSeleccionada != null)
            {
                return OrdenEnvasadoSeleccionada.EstadoEnvasadoId == 2; // Procesando
            }
            return false;
        }

        private async void AnadirProductoEnvasado()
        {
            var formProductoEnvasado = new FormProductoEnvasado(context);

            if ((bool)await DialogHost.Show(formProductoEnvasado, "RootDialog"))
            {
                var formProductoEnvasadoDataContext = formProductoEnvasado.DataContext as FormProductoEnvasadoViewModel;
                var productoEnvasado = new ProductoEnvasado()
                {
                    OrdenId = OrdenEnvasadoSeleccionada.OrdenEnvasadoId,
                    TipoProductoEnvasadoId = (formProductoEnvasado.cbTiposProductosEnvasados.SelectedItem as TipoProductoEnvasado).TipoProductoEnvasadoId,
                    Volumen = formProductoEnvasadoDataContext.Volumen,
                    Unidades = formProductoEnvasadoDataContext.Unidades,
                    PickingId = (formProductoEnvasado.cbPicking.SelectedItem as Picking).PickingId,
                    Observaciones = formProductoEnvasadoDataContext.Observaciones
                };

                
                context.ProductosEnvasados.Add(productoEnvasado);

                var productosEnvasadosComposiciones = new List<ProductoEnvasadoComposicion>();
                foreach (var pec in formProductoEnvasadoDataContext.ProductosEnvasadosComposiciones)
                {
                    var hhaId = pec.HistorialHuecoAlmacenaje.HistorialHuecoAlmacenajeId;
                    // Los huecos que no se ha añadido ninguna cantidad no se añaden
                    if (pec.Unidades != 0 && pec.Volumen != 0)
                    {
                        // Hay que asegurarse que la cantidad de materia prima escogida es como máximo la disponible en el hueco
                        if (pec.HistorialHuecoAlmacenaje.ProductoTerminado.TipoProductoTerminado.MedidoEnUnidades == true)
                        {
                            pec.Unidades = (pec.Unidades > pec.HistorialHuecoAlmacenaje.UnidadesRestantes) ? (pec.HistorialHuecoAlmacenaje.UnidadesRestantes) : (pec.Unidades);
                        }
                        else
                        {
                            pec.Volumen = (pec.Volumen > pec.HistorialHuecoAlmacenaje.VolumenRestante) ? (pec.HistorialHuecoAlmacenaje.UnidadesRestantes) : (pec.Volumen);
                        }
                        pec.HistorialHuecoAlmacenaje = null;
                        pec.HistorialHuecoId = hhaId;
                        pec.ProductoEnvasado = productoEnvasado;
                        productosEnvasadosComposiciones.Add(pec);
                    }
                }
                context.ProductosEnvasadosComposiciones.AddRange(productosEnvasadosComposiciones);
                context.SaveChanges();

                CargarProductosEnvasados();
            }
        }
        #endregion


        #region Borrar Producto Envasado    
        public ICommand BorrarProductoEnvasadoComando => _borrarProductoEnvasadoComando ??
            (_borrarProductoEnvasadoComando = new RelayCommandGenerico<IList<object>>(
                param => BorrarProductoEnvasado(),
                param => ProductoEnvasadoSeleccionado != null
            ));

        private async void BorrarProductoEnvasado()
        {
            string pregunta = ProductosEnvasadosSeleccionados.Count == 1
                ? "¿Está seguro de que desea borrar el producto envasado con código " + ProductoEnvasadoSeleccionado.Codigo + "?"
                : "¿Está seguro de que desea borrar los productos envasados seleccionados?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {
                List<ProductoEnvasado> productosEnvasadosABorrar = new List<ProductoEnvasado>();

                foreach (var pe in ProductosEnvasadosSeleccionados)
                {
                    if (!context.ProductosEnvasadosComposiciones.Any(ptc => ptc.ProductoEnvasado.ProductoEnvasadoId == pe.ProductoEnvasadoId))
                    {
                        ProductosEnvasadosSeleccionados.Add(pe);
                    }
                }
                context.ProductosEnvasados.RemoveRange(productosEnvasadosABorrar);
                context.SaveChanges();

                if (ProductosEnvasadosSeleccionados.Count != ProductosEnvasadosSeleccionados.Count)
                {
                    string mensaje = ProductosEnvasadosSeleccionados.Count == 1
                           ? "No se ha podido borrar el producto envasado seleccionado."
                           : "No se han podido borrar todas los productos envasados seleccionados.";
                    mensaje += "\n\nAsegurese de no que no exista ningún producto envasado elaborado con dicho producto terminado.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
                CargarProductosEnvasados();
            }
        }
        #endregion


        #region Modificar Producto Envasado 
        public ICommand ModificarProductoEnvasadoComando => _modificarProductoEnvasadoComando ??
            (_modificarProductoEnvasadoComando = new RelayCommand(
                param => ModificarProductoEnvasado(),
                param => ProductoEnvasadoSeleccionado != null
             ));

        public async void ModificarProductoEnvasado()
        {
            var formProductoEnvasado = new FormProductoEnvasado(context, ProductoEnvasadoSeleccionado);
            var formProductoEnvasadoDataContext = formProductoEnvasado.DataContext as FormProductoEnvasadoViewModel;
            var productosEnvasadosComposicionesIniciales = formProductoEnvasadoDataContext.ProductosEnvasadosComposiciones.ToList();
            if ((bool)await DialogHost.Show(formProductoEnvasado, "RootDialog"))
            {
                ProductoEnvasadoSeleccionado.TipoProductoEnvasadoId = formProductoEnvasadoDataContext.TipoProductoEnvasado.TipoProductoEnvasadoId;
                ProductoEnvasadoSeleccionado.Unidades = formProductoEnvasadoDataContext.Unidades;
                ProductoEnvasadoSeleccionado.Volumen = formProductoEnvasadoDataContext.Volumen;
                ProductoEnvasadoSeleccionado.Observaciones = formProductoEnvasadoDataContext.Observaciones;
                
                if (!context.ProductosEnvasadosComposiciones.Any(pec => pec.ProductoId == ProductoEnvasadoSeleccionado.ProductoEnvasadoId))
                {
                    // Se borran todos los productos envasados composiciones antiguos y se añaden los nuevos
                    context.ProductosEnvasadosComposiciones.RemoveRange(productosEnvasadosComposicionesIniciales);
                    var productosEnvasadosComposiciones = new List<ProductoEnvasadoComposicion>();
                    foreach (var pec in formProductoEnvasadoDataContext.ProductosEnvasadosComposiciones)
                    {
                        var hhaId = pec.HistorialHuecoAlmacenaje.HistorialHuecoAlmacenajeId;
                        // Los huecos que no se ha añadido ninguna cantidad no se añaden
                        if (pec.Unidades != 0 && pec.Volumen != 0)
                        {
                            // Hay que asegurarse que la cantidad de materia prima escogida es como máximo la disponible en el hueco
                            if (pec.HistorialHuecoAlmacenaje.ProductoTerminado.TipoProductoTerminado.MedidoEnUnidades == true)
                            {
                                pec.Unidades = (pec.Unidades > pec.HistorialHuecoAlmacenaje.UnidadesRestantes) ? (pec.HistorialHuecoAlmacenaje.UnidadesRestantes) : (pec.Unidades);
                            }
                            else
                            {
                                pec.Volumen = (pec.Volumen > pec.HistorialHuecoAlmacenaje.VolumenRestante) ? (pec.HistorialHuecoAlmacenaje.UnidadesRestantes) : (pec.Volumen);
                            }
                            pec.HistorialHuecoAlmacenaje = null;
                            pec.HistorialHuecoId = hhaId;
                            pec.ProductoEnvasado = ProductoEnvasadoSeleccionado;
                            productosEnvasadosComposiciones.Add(pec);
                        }
                    }
                    context.ProductosEnvasadosComposiciones.AddRange(productosEnvasadosComposiciones);

                }

                context.SaveChanges();
                ProductosEnvasadosView.Refresh();
            }
        }
        #endregion


        #region Refrescar Productos Envasados
        public ICommand RefrescarProductosEnvasadosComando => _refrescarProductosEnvasadoComando ??
            (_refrescarProductosEnvasadoComando = new RelayCommand(
                param => CargarProductosEnvasados(),
                param => OrdenEnvasadoSeleccionada != null
             ));
        #endregion


        #region Filtro Productos Envasados
        public ICommand FiltrarProductosEnvasadosComando => _filtrarProductosEnvasadosComando ??
           (_filtrarProductosEnvasadosComando = new RelayCommand(
                param => FiltrarProductosEnvasados()
           ));

        public void FiltrarProductosEnvasados()
        {
            ProductosEnvasadosView.Filter = FiltroProductosEnvasados;
            ProductosEnvasadosView.Refresh();
        }

        private bool FiltroProductosEnvasados(object item)
        {
            var productoEnvasado = item as ProductoEnvasado;
            string tipo = productoEnvasado.TipoProductoEnvasado.Nombre.ToLower();
            string grupo = productoEnvasado.TipoProductoEnvasado.GrupoProductoEnvasado.Nombre.ToLower();
            string volumen = productoEnvasado.Volumen.ToString();
            string unidades = productoEnvasado.Unidades.ToString();

            return (VolUniProductoEnvasadoSeleccionado == true ? (volumen.Contains(TextoFiltroProductosEnvasados) || unidades.Contains(TextoFiltroProductosEnvasados)) : false)
                || (TipoProductoEnvasadoSeleccionado == true ? tipo.Contains(TextoFiltroProductosEnvasados) : false)
                || (GrupoProductoEnvasadoSeleccionado == true ? grupo.Contains(TextoFiltroProductosEnvasados) : false);

        }
        #endregion

    }
}
