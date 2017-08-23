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


namespace BiomasaEUPT.Vistas.GestionVentas
{
    class TabVentasViewModel : ViewModelBase
    {

        public ObservableCollection<PedidoCabecera> PedidosCabeceras { get; set; }
        public CollectionView PedidosCabecerasView { get; private set; }
        public IList<PedidoCabecera> PedidosCabecerasSeleccionados { get; set; }
        public PedidoCabecera PedidoCabeceraSeleccionado { get; set; }

        public ObservableCollection<ProductoEnvasado> ProductosEnvasados { get; set; }
        public CollectionView ProductosEnvasadosView { get; private set; }
        public IList<ProductoEnvasado> ProductosEnvasadosSeleccionados { get; set; }
        public ProductoEnvasado ProductoEnvasadoSeleccionado { get; set; }

        public int IndiceMasOpciones { get; set; }

        // Checkbox Filtro Pedidos
        public bool FechaPedidoSeleccionado { get; set; } = true;
        public bool FechaFinalizacionSeleccionado { get; set; } = true;
        public bool EstadoPedidoSeleccionado { get; set; } = false;
        public bool ClientePedidoSeleccionado { get; set; } = true;

        private string _textoFiltroPedidos;
        public string TextoFiltroPedidos
        {
            get => _textoFiltroPedidos;
            set
            {
                _textoFiltroPedidos = value.ToLower();
                FiltrarPedidos();
            }
        }

        // Checkbox Filtro Productos Envasados
        public bool FechaBajaProductoEnvasadoSeleccionado { get; set; } = false;
        public bool TipoProductoTerminadoSeleccionado { get; set; } = true;
        public bool GrupoProductoTerminadoSeleccionado { get; set; } = true;
        public bool VolumenSeleccionado { get; set; } = false;
        public bool PickingNombreSeleccionado { get; set; } = false;

        private string _textoFiltroProductosEnvasados;
        public string TextoFiltroProductosEnvasados
        {
            get => _textoFiltroProductosEnvasados;
            set
            {
                _textoFiltroProductosEnvasados = value.ToLower();
                FiltrarProductosEnvasados();
            }
        }

        private ICommand _anadirPedidoCabeceraComando;
        private ICommand _modificarPedidoCabeceraComando;
        private ICommand _borrarPedidoCabeceraComando;
        private ICommand _refrescarPedidosCabecerasComando;
        private ICommand _filtrarPedidosCabecerasComando;
        private ICommand _dgPedidosCabeceras_SelectionChangedComando;

        private ICommand _anadirProductoEnvasadoComando;
        private ICommand _modificarProductoEnvasadoComando;
        private ICommand _borrarProductoEnvasadoComando;
        private ICommand _refrescarProductosEnvasadosComando;
        private ICommand _filtrarProductosEnvasadosComando;

        private ICommand _masOpcionesComando;

        private BiomasaEUPTContext context;
        public PaginacionViewModel PaginacionViewModel { get; set; }
        public MasOpcionesVentasViewModel MasOpcionesVentasViewModel { get; set; }

        public TabVentasViewModel()
        {
            PaginacionViewModel = new PaginacionViewModel();
            MasOpcionesVentasViewModel = new MasOpcionesVentasViewModel();
            // MasOpcionesActivado = false;
        }

        public override void Inicializar()
        {
            using (new CursorEspera())
            {
                IndiceMasOpciones = 0;
                context = new BiomasaEUPTContext();
                CargarPedidosCabeceras();
                PaginacionViewModel.GetItemsTotales = () => { return context.Recepciones.Count(); };
                PaginacionViewModel.ActualizarContadores();
                PaginacionViewModel.CargarItems = CargarPedidosCabeceras;
            }
        }

        public void CargarPedidosCabeceras(int cantidad = 10, int saltar = 0)
        {
            PedidosCabeceras = new ObservableCollection<PedidoCabecera>(
                context.PedidosCabeceras
                .Include(pc => pc.EstadoPedido).Include(pc => pc.Cliente)
                .OrderBy(pc => pc.EstadoId).Skip(saltar).Take(cantidad).ToList());
            PedidosCabecerasView = (CollectionView)CollectionViewSource.GetDefaultView(PedidosCabeceras);

            // Por defecto no está seleccionada ninguna fila del datagrid pedidos
            PedidoCabeceraSeleccionado = null;
        }

        public void CargarProductosEnvasados()
        {
            if (PedidoCabeceraSeleccionado != null)
            {
                using (new CursorEspera())
                {
                    ProductosEnvasados = new ObservableCollection<ProductoEnvasado>(
                          context.ProductosEnvasados.
                          Where(pe => pe.ProductoEnvasadoId == PedidoCabeceraSeleccionado.EstadoId)
                          .Include(pe => pe.ProductosEnvasadosComposiciones)
                          .Include(pe => pe.Picking)
                          .ToList());
                    ProductosEnvasadosView = (CollectionView)CollectionViewSource.GetDefaultView(ProductosEnvasados);

                    ProductoEnvasadoSeleccionado = null;
                }
            }
            else
            {
                ProductosEnvasados = null;
            }
        }

        // Asigna el valor de PedidosCabecerasSeleccinodos ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGPedidosabeceras_SelectionChangedComando => _dgPedidosCabeceras_SelectionChangedComando ??
            (_dgPedidosCabeceras_SelectionChangedComando = new RelayCommandGenerico<IList<object>>(
                param => DGPedidosabeceras_SelectionChanged(param)
            ));

        private void DGPedidosabeceras_SelectionChanged(IList<object> recepcionesSeleccionadas)
        {
            PedidosCabecerasSeleccionados = PedidosCabecerasSeleccionados.Cast<PedidoCabecera>().ToList();
            CargarProductosEnvasados();
        }

        // Asigna el valor de ProducosEnvasadosSeleccionados ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGProductosEnvasados_SelectionChangedComando => new RelayCommandGenerico<IList<object>>(param => ProductosEnvasadosSeleccionados = param.Cast<ProductoEnvasado>().ToList());


        #region Añadir Pedido Cabecera
        public ICommand AnadirPedidoCabeceraComando => _anadirPedidoCabeceraComando ??
            (_anadirPedidoCabeceraComando = new RelayCommand(
                param => AnadirPedidoCabecera()
            ));

        private async void AnadirPedidoCabecera()
        {
            var formPedido = new FormPedido(context);

            if ((bool)await DialogHost.Show(formPedido, "RootDialog"))
            {
                context.PedidosCabeceras.Add(new PedidoCabecera()
                {
                    FechaPedido = new DateTime(formPedido.FechaPedido.Year, formPedido.FechaPedido.Month, formPedido.FechaPedido.Day, formPedido.HoraPedido.Hour, formPedido.HoraPedido.Minute, formPedido.HoraPedido.Second),
                    FechaFinalizacion = new DateTime(formPedido.FechaFinalizacion.Year, formPedido.FechaFinalizacion.Month, formPedido.FechaFinalizacion.Day, formPedido.HoraFinalizacion.Hour, formPedido.HoraFinalizacion.Minute, formPedido.HoraFinalizacion.Second),
                    ClienteId = (formPedido.cbClientes.SelectedItem as Cliente).ClienteId,
                    EstadoId = 1
                });
                context.SaveChanges();
            }
        }
        #endregion


        #region Borrar Pedido Cabecera
        public ICommand BorrarPedidoCabeceraComando => _borrarPedidoCabeceraComando ??
            (_borrarPedidoCabeceraComando = new RelayCommandGenerico<IList<object>>(
                param => BorrarPedidoCabecera(),
                param => PedidoCabeceraSeleccionado != null
            ));

        private async void BorrarPedidoCabecera()
        {
            string pregunta = PedidosCabecerasSeleccionados.Count == 1
                   ? "¿Está seguro de que desea borrar el pedido " + PedidoCabeceraSeleccionado.PedidoCabeceraId + "?"
                   : "¿Está seguro de que desea borrar los pedidos seleccionados?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {
                var pedidosABorrar = new List<PedidoCabecera>();

                foreach (var pedido in PedidosCabecerasSeleccionados)
                {
                    if (!context.ProductosEnvasados.Any(pe => pe.ProductoEnvasadoId == pedido.PedidoCabeceraId))
                    {
                        pedidosABorrar.Add(pedido);
                    }
                }
                context.PedidosCabeceras.RemoveRange(pedidosABorrar);
                context.SaveChanges();

                if (PedidosCabecerasSeleccionados.Count != pedidosABorrar.Count)
                {
                    string mensaje = PedidosCabecerasSeleccionados.Count == 1
                           ? "No se ha podido borrar el pedido seleccionado."
                           : "No se han podido borrar todas los pedidos seleccionados.";
                    mensaje += "\n\nAsegurese de no que no exista ningún producto envasado asociada a dicho pedido.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
                PaginacionViewModel.Refrescar();
            }
        }
        #endregion


        #region Modificar Pedido Cabecera
        public ICommand ModificarPedidoCabeceraComando => _modificarPedidoCabeceraComando ??
            (_modificarPedidoCabeceraComando = new RelayCommand(
                param => ModificarPedidoCabecera(),
                param => PedidoCabeceraSeleccionado != null
             ));

        public async void ModificarPedidoCabecera()
        {

            var formPedido = new FormPedido(context, "Editar Pedido")
            {
                FechaPedido = PedidoCabeceraSeleccionado.FechaPedido,
                //HoraPedido = PedidoCabeceraSeleccionado.HoraPedido
            };
            formPedido.cbEstados.Visibility = Visibility.Visible;
            formPedido.cbEstados.SelectedValue = PedidoCabeceraSeleccionado.EstadoPedido.EstadoPedidoId;
            formPedido.cbClientes.SelectedValue = PedidoCabeceraSeleccionado.Cliente.ClienteId;
            if ((bool)await DialogHost.Show(formPedido, "RootDialog"))
            {
                PedidoCabeceraSeleccionado.FechaPedido = new DateTime(formPedido.FechaPedido.Year, formPedido.FechaPedido.Month, formPedido.FechaPedido.Day, formPedido.FechaPedido.Hour, formPedido.HoraPedido.Minute, formPedido.HoraPedido.Second);
                PedidoCabeceraSeleccionado.ClienteId = (formPedido.cbClientes.SelectedItem as Cliente).ClienteId;
                PedidoCabeceraSeleccionado.EstadoId = (formPedido.cbEstados.SelectedItem as EstadoPedido).EstadoPedidoId;
                context.SaveChanges();
                PedidosCabecerasView.Refresh();
            }
        }
        #endregion


        #region Refrescar Pedidos Cabeceras
        public ICommand RefrescarPedidosCabecerasComando => _refrescarPedidosCabecerasComando ??
            (_refrescarPedidosCabecerasComando = new RelayCommand(
                param => RefrescarPedidosCabeceras()
             ));

        public void RefrescarPedidosCabeceras()
        {
            PaginacionViewModel.Refrescar();
            PedidoCabeceraSeleccionado = null;
        }
        #endregion


        #region Filtro Pedidos Cabeceras
        public ICommand FiltrarPedidosCabecerasComando => _filtrarPedidosCabecerasComando ??
           (_filtrarPedidosCabecerasComando = new RelayCommand(
                param => FiltrarPedidos()
           ));

        public void FiltrarPedidos()
        {
            PedidosCabecerasView.Filter = FiltroPedidos;
            PedidosCabecerasView.Refresh();
        }

        private bool FiltroPedidos(object item)
        {
            var pedido = item as PedidoCabecera;
            string fechaPedido = pedido.FechaPedido.ToString();
            string fechaFinalizacion = pedido.FechaFinalizacion.ToString();
            string cliente = pedido.Cliente.RazonSocial.ToLower();
            string estado = pedido.EstadoPedido.Nombre.ToLower();

            return (FechaPedidoSeleccionado == true ? fechaPedido.Contains(TextoFiltroPedidos) : false)
                || (FechaFinalizacionSeleccionado == true ? fechaFinalizacion.Contains(TextoFiltroPedidos) : false)
                || (ClientePedidoSeleccionado == true ? cliente.Contains(TextoFiltroPedidos) : false)
                || (EstadoPedidoSeleccionado == true ? estado.Contains(TextoFiltroPedidos) : false);
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
            if (PedidoCabeceraSeleccionado != null)
            {
                return PedidoCabeceraSeleccionado.EstadoId == 1; // Procesando
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
                    Observaciones = formProductoEnvasadoDataContext.Observaciones,
                    PickingId = (formProductoEnvasado.cbPicking.SelectedItem as Picking).PickingId,
                    //TipoProductoTerminadoId = (formProductoEnvasado.cbTipoProductoTerminado.SelectedItem as TipoProductoTerminado).TipoProductoTerminadoId,
                    Volumen = formProductoEnvasadoDataContext.Volumen,

                };

                /*if (formProductoEnvasadoDataContext.FechaBaja != null)
                {
                    productoEnvasado.FechaBaja = new DateTime(
                        formProductoEnvasadoDataContext.FechaBaja.Value.Year,
                        formProductoEnvasadoDataContext.FechaBaja.Value.Month,
                        formProductoEnvasadoDataContext.FechaBaja.Value.Day,
                        formProductoEnvasadoDataContext.HoraBaja.Value.Hour,
                        formProductoEnvasadoDataContext.HoraBaja.Value.Minute,
                        formProductoEnvasadoDataContext.HoraBaja.Value.Second);
                }*/
                context.ProductosEnvasados.Add(productoEnvasado);
                /*var huecosMateriasPrimas = new List<HistorialHuecoRecepcion>();
                foreach (var hmp in formMateriaPrimaDataContext.HistorialHuecosRecepciones)
                {
                    var hrId = hmp.HuecoRecepcion.HuecoRecepcionId;
                    // Los huecos que no se ha añadido ninguna cantidad no se añaden
                    if (hmp.Unidades != 0 && hmp.Volumen != 0)
                    {
                        hmp.HuecoRecepcion = null;
                        hmp.HuecoRecepcionId = hrId;
                        hmp.MateriaPrima = materiaPrima;
                        huecosMateriasPrimas.Add(hmp);
                    }
                }
                context.HistorialHuecosRecepciones.AddRange(huecosMateriasPrimas);
                context.SaveChanges();*/

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
            //var historialHuecosRecepionesIniciales = formProductoEnvasadoDataContext.HistorialHuecosRecepciones.ToList();
            if ((bool)await DialogHost.Show(formProductoEnvasado, "RootDialog"))
            {
                //ProductoEnvasadoSeleccionado.TipoProductoTerminadoId = formProductoEnvasadoDataContext.TipoProductoTerminado.TipoProductoTerminadoId;
                ProductoEnvasadoSeleccionado.PickingId = (formProductoEnvasado.cbPicking.SelectedItem as Picking).PickingId;
                ProductoEnvasadoSeleccionado.Volumen = formProductoEnvasadoDataContext.Volumen;
                ProductoEnvasadoSeleccionado.Observaciones = formProductoEnvasadoDataContext.Observaciones;
                /*if (formProductoEnvasadoDataContext.FechaBaja != null)
                {
                    ProductoEnvasadoSeleccionado.FechaBaja = new DateTime(
                        ProductoEnvasadoSeleccionado.FechaBaja.Value.Year,
                        ProductoEnvasadoSeleccionado.FechaBaja.Value.Month,
                        ProductoEnvasadoSeleccionado.FechaBaja.Value.Day,
                        ProductoEnvasadoSeleccionado.FechaBaja.Value.Hour,
                        ProductoEnvasadoSeleccionado.FechaBaja.Value.Minute,
                        ProductoEnvasadoSeleccionado.FechaBaja.Value.Second);
                }
                else
                {
                    ProductoEnvasadoSeleccionado.FechaBaja = null;
                }*/
                if (!context.ProductosEnvasadosComposiciones.Any(pec => pec.ProductoEnvasado.ProductoEnvasadoId == ProductoEnvasadoSeleccionado.ProductoEnvasadoId))
                {
                    // Se borran todos los historiales huecos recepciones antiguos y se añaden los nuevos
                    /*context.HistorialHuecosRecepciones.RemoveRange(historialHuecosRecepionesIniciales);
                    var huecosMateriasPrimas = new List<HistorialHuecoRecepcion>();
                    foreach (var hhr in formMateriaPrimaDataContext.HistorialHuecosRecepciones)
                    {
                        var hrId = hhr.HuecoRecepcion.HuecoRecepcionId;
                        // Los huecos que no se ha añadido ninguna cantidad no se añaden
                        if (hhr.Unidades != 0 && hhr.Volumen != 0)
                        {
                            hhr.HuecoRecepcion = null;
                            hhr.HuecoRecepcionId = hrId;
                            hhr.MateriaPrima = MateriaPrimaSeleccionada;
                            huecosMateriasPrimas.Add(hhr);
                        }
                    }
                    context.HistorialHuecosRecepciones.AddRange(huecosMateriasPrimas);*/
                }

                context.SaveChanges();
                ProductosEnvasadosView.Refresh();
                // CargarMateriasPrimas();
            }
        }
        #endregion


        #region Refrescar Productos Envasados
        public ICommand RefrescarProductosEnvasadosComando => _refrescarProductosEnvasadosComando ??
            (_refrescarProductosEnvasadosComando = new RelayCommand(
                param => CargarProductosEnvasados(),
                param => PedidoCabeceraSeleccionado != null
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
            //string tipo = productoEnvasado.TipoProductoTerminado.Nombre.ToLower();
            //string grupo = productoEnvasado.TipoProductoTerminado.GrupoProductoTerminado.Nombre.ToLower();
            string volumen = productoEnvasado.Volumen.ToString();
            string picking = productoEnvasado.Picking.Nombre.ToLower();
            //string fechaBaja = productoEnvasado.FechaBaja.ToString();

            return (VolumenSeleccionado == true ? (volumen.Contains(TextoFiltroProductosEnvasados)) : false)
                || (PickingNombreSeleccionado == true ? picking.Contains(TextoFiltroProductosEnvasados) : false);

        }
        #endregion
    }
}
