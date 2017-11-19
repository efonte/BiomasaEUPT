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

        public ObservableCollection<PedidoLinea> PedidosLineas { get; set; }
        public CollectionView PedidosLineasView { get; private set; }
        public IList<PedidoLinea> PedidosLineasSeleccionados { get; set; }
        public PedidoLinea PedidoLineaSeleccionado { get; set; }

        public ObservableCollection<PedidoDetalle> PedidosDetalles { get; set; }
        public CollectionView PedidosDetallesView { get; private set; }
        public IList<PedidoDetalle> PedidosDetallesSeleccionados { get; set; }
        public PedidoDetalle PedidoDetalleSeleccionado { get; set; }


        public int IndiceMasOpciones { get; set; }

        // Checkbox Filtro Pedidos Cabececeras
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

        // Checkbox Filtro Pedidos Líneas
        public bool VolumenSeleccionado { get; set; } = true;
        public bool UnidadesSeleccionadas { get; set; } = true;
        public bool TipoProductoEnvasadoSeleccionado { get; set; } = true;

        private string _textoFiltroPedidosLineas;
        public string TextoFiltroPedidosLineas
        {
            get => _textoFiltroPedidosLineas;
            set
            {
                _textoFiltroPedidosLineas = value.ToLower();
                FiltrarPedidosLineas();
            }
        }

        // Checkbox Filtro Pedidos Detalles
        public bool LineaVolumenSeleccionado { get; set; } = true;
        public bool LineaUnidadesSeleccionadas { get; set; } = true;
        public bool LineaTipoProductoEnvasadoSeleccionado { get; set; } = true;

        private string _textoFiltroPedidosDetalles;
        public string TextoFiltroPedidosDetalles
        {
            get => _textoFiltroPedidosDetalles;
            set
            {
                _textoFiltroPedidosDetalles = value.ToLower();
                FiltrarPedidosDetalles();
            }
        }


        private ICommand _anadirPedidoCabeceraComando;
        private ICommand _modificarPedidoCabeceraComando;
        private ICommand _borrarPedidoCabeceraComando;
        private ICommand _refrescarPedidosCabecerasComando;
        private ICommand _filtrarPedidosCabecerasComando;
        private ICommand _dgPedidosCabeceras_SelectionChangedComando;

        private ICommand _anadirPedidoLineaComando;
        private ICommand _modificarPedidoLineaComando;
        private ICommand _borrarPedidoLineaComando;
        private ICommand _refrescarPedidosLineasComando;
        private ICommand _filtrarPedidosLineasComando;
        private ICommand _dgPedidosLineas_SelectionChangedComando;

        private ICommand _anadirPedidoDetalleComando;
        private ICommand _anadirPedidoDetalleLectorComando;
        private ICommand _modificarPedidoDetalleComando;
        private ICommand _borrarPedidoDetalleComando;
        private ICommand _refrescarPedidosDetallesComando;
        private ICommand _filtrarPedidosDetallesComando;

        private ICommand _masOpcionesComando;

        private BiomasaEUPTContext context;
        public PaginacionViewModel PaginacionViewModel { get; set; }

        public TabVentasViewModel()
        {
            PaginacionViewModel = new PaginacionViewModel();
            // MasOpcionesActivado = false;
        }

        public override void Inicializar()
        {
            using (new CursorEspera())
            {
                context = new BiomasaEUPTContext();
                CargarPedidosCabeceras();
                PaginacionViewModel.GetItemsTotales = () => { return context.PedidosCabeceras.Count(); };
                PaginacionViewModel.ActualizarContadores();
                PaginacionViewModel.CargarItems = CargarPedidosCabeceras;
                CargarPedidosLineas();
                PaginacionViewModel.GetItemsTotales = () => { return context.PedidosLineas.Count(); };
                PaginacionViewModel.ActualizarContadores();
                PaginacionViewModel.CargarItems = CargarPedidosLineas;
                CargarPedidosDetalles();
                PaginacionViewModel.GetItemsTotales = () => { return context.PedidosDetalles.Count(); };
                PaginacionViewModel.ActualizarContadores();
                PaginacionViewModel.CargarItems = CargarPedidosDetalles;
            }
        }

        public void CargarPedidosCabeceras(int cantidad = 10, int saltar = 0)
        {

            using (new CursorEspera())
            {
                // Si los pedidos disponibles son menores que la cantidad a coger,
                // se obtienen todas
                if (context.PedidosCabeceras.Count() < cantidad)
                {

                    PedidosCabeceras = new ObservableCollection<PedidoCabecera>(context.PedidosCabeceras.ToList());
                }
                else
                {
                    PedidosCabeceras = new ObservableCollection<PedidoCabecera>(
                context.PedidosCabeceras
                .Include(pc => pc.EstadoPedido).Include(pc => pc.Cliente)
                .OrderBy(pc => pc.EstadoId).Skip(saltar).Take(cantidad).ToList());
                }
                PedidosCabecerasView = (CollectionView)CollectionViewSource.GetDefaultView(PedidosCabeceras);

                // Por defecto no está seleccionada ninguna fila del datagrid pedidos
                PedidoCabeceraSeleccionado = null;
            }
        }


        public void CargarPedidosLineas(int cantidad = 10, int saltar = 0)
        {

            using (new CursorEspera())
            {
                // Si los pedidos disponibles son menores que la cantidad a coger,
                // se obtienen todas
                if (context.PedidosLineas.Count() < cantidad)
                {

                    PedidosLineas = new ObservableCollection<PedidoLinea>(context.PedidosLineas.ToList());
                }
                else
                {
                    PedidosLineas = new ObservableCollection<PedidoLinea>(
                context.PedidosLineas
                .Include(pl => pl.TipoProductoEnvasado).Include(pl => pl.PedidoCabecera)
                .OrderBy(pl => pl.PedidoLineaId).Skip(saltar).Take(cantidad).ToList());
                }
                PedidosLineasView = (CollectionView)CollectionViewSource.GetDefaultView(PedidosLineas);

                // Por defecto no está seleccionada ninguna fila del datagrid pedidos
                PedidoLineaSeleccionado = null;
            }
        }


        public void CargarPedidosDetalles(int cantidad = 10, int saltar = 0)
        {

            using (new CursorEspera())
            {
                // Si los pedidos disponibles son menores que la cantidad a coger,
                // se obtienen todas
                if (context.PedidosDetalles.Count() < cantidad)
                {
                    PedidosDetalles = new ObservableCollection<PedidoDetalle>(context.PedidosDetalles.ToList());
                }
                else
                {
                    PedidosDetalles = new ObservableCollection<PedidoDetalle>(
                context.PedidosDetalles
                .Include(pd => pd.PedidoDetalleId).Include(pd => pd.ProductoEnvasado)
                .OrderBy(pd => pd.PedidoLineaId).Skip(saltar).Take(cantidad).ToList());
                }
                PedidosDetallesView = (CollectionView)CollectionViewSource.GetDefaultView(PedidosDetalles);

                // Por defecto no está seleccionada ninguna fila del datagrid pedidos
                PedidoDetalleSeleccionado = null;
            }
        }


        // Asigna el valor de PedidosCabecerasSeleccinodos ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGPedidosCabeceras_SelectionChangedComando => _dgPedidosCabeceras_SelectionChangedComando ??
            (_dgPedidosCabeceras_SelectionChangedComando = new RelayCommandGenerico<IList<object>>(
                param => DGPedidosCabeceras_SelectionChanged(param)
            ));

        private void DGPedidosCabeceras_SelectionChanged(IList<object> recepcionesSeleccionadas)
        {
            PedidosCabecerasSeleccionados = PedidosCabecerasSeleccionados.Cast<PedidoCabecera>().ToList();
            CargarPedidosLineas();
        }

        // Asigna el valor de PedidosLineasSeleccinodos ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGPedidosLineas_SelectionChangedComando => _dgPedidosLineas_SelectionChangedComando ??
            (_dgPedidosLineas_SelectionChangedComando = new RelayCommandGenerico<IList<object>>(
                param => DGPedidosLineas_SelectionChanged(param)
            ));

        private void DGPedidosLineas_SelectionChanged(IList<object> pedidosLineasSeleccionadas)
        {
            PedidosLineasSeleccionados = PedidosLineasSeleccionados.Cast<PedidoLinea>().ToList();
            CargarPedidosDetalles();
        }


        #region Añadir Pedido Cabecera
        public ICommand AnadirPedidoCabeceraComando => _anadirPedidoCabeceraComando ??
            (_anadirPedidoCabeceraComando = new RelayCommand(
                param => AnadirPedidoCabecera()
            ));

        private async void AnadirPedidoCabecera()
        {

            var formPedido = new FormPedido(context);
            //var formPedidoDataContext = formPedido.DataContext as FormPedidoViewModel;

            if ((bool)await DialogHost.Show(formPedido, "RootDialog"))
            {

                var pedidoCabecera = new PedidoCabecera()
                {

                    FechaPedido = new DateTime(formPedido.FechaPedido.Year, formPedido.FechaPedido.Month, formPedido.FechaPedido.Day, formPedido.HoraPedido.Hour, formPedido.HoraPedido.Minute, formPedido.HoraPedido.Second),
                    ClienteId = (formPedido.cbClientes.SelectedItem as Cliente).ClienteId,
                    EstadoId = 1
                };

                context.PedidosCabeceras.Add(pedidoCabecera);
                context.SaveChanges();

                RefrescarPedidosCabeceras();
                CargarPedidosCabeceras();
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
                    /*if (!context.PedidosDetalles.Any(pd => pd.PedidoCabeceraId == pedido.PedidoCabeceraId))
                    {
                        pedidosABorrar.Add(pedido);
                    }*/
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
            formPedido.cbEstadosPedidos.Visibility = Visibility.Visible;
            formPedido.cbEstadosPedidos.SelectedValue = PedidoCabeceraSeleccionado.EstadoPedido.EstadoPedidoId;
            formPedido.cbClientes.SelectedValue = PedidoCabeceraSeleccionado.Cliente.ClienteId;

            if ((bool)await DialogHost.Show(formPedido, "RootDialog"))
            {
                PedidoCabeceraSeleccionado.FechaPedido = new DateTime(formPedido.FechaPedido.Year, formPedido.FechaPedido.Month, formPedido.FechaPedido.Day, formPedido.FechaPedido.Hour, formPedido.HoraPedido.Minute, formPedido.HoraPedido.Second);
                PedidoCabeceraSeleccionado.ClienteId = (formPedido.cbClientes.SelectedItem as Cliente).ClienteId;
                PedidoCabeceraSeleccionado.EstadoId = (formPedido.cbEstadosPedidos.SelectedItem as EstadoPedido).EstadoPedidoId;
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
            string cliente = pedido.Cliente.RazonSocial.ToLower();
            string estado = pedido.EstadoPedido.Nombre.ToLower();

            return (FechaPedidoSeleccionado == true ? fechaPedido.Contains(TextoFiltroPedidos) : false)
                || (ClientePedidoSeleccionado == true ? cliente.Contains(TextoFiltroPedidos) : false)
                || (EstadoPedidoSeleccionado == true ? estado.Contains(TextoFiltroPedidos) : false);
        }
        #endregion


        #region Añadir Pedido Linea
        public ICommand AnadirPedidoLineaComando => _anadirPedidoLineaComando ??
            (_anadirPedidoLineaComando = new RelayCommand(
                param => AnadirPedidoLinea(),
                param => CanAnadirPedidoLinea()
            ));

        private bool CanAnadirPedidoLinea()
        {
            if (PedidoCabeceraSeleccionado != null)
            {
                return PedidoCabeceraSeleccionado.EstadoId == 2; // Preparar
            }
            return false;
        }

        private async void AnadirPedidoLinea()
        {

            var formPedidoLinea = new FormPedidoLinea(context);

            if ((bool)await DialogHost.Show(formPedidoLinea, "RootDialog"))
            {
                var formPedidoLineaDataContext = formPedidoLinea.DataContext as FormPedidoLineaViewModel;

                var pedidoLinea = new PedidoLinea()
                {
                    PedidoCabeceraId = PedidoCabeceraSeleccionado.PedidoCabeceraId,
                    TipoProductoEnvasadoId = (formPedidoLinea.cbTiposProductosEnvasados.SelectedItem as TipoProductoEnvasado).TipoProductoEnvasadoId,
                    Volumen = formPedidoLineaDataContext.Volumen,
                    Unidades = formPedidoLineaDataContext.Unidades
                };

                if (pedidoLinea.TipoProductoEnvasado.MedidoEnUnidades == true)
                {
                    pedidoLinea.UnidadesPreparadas = 0;
                }
                else
                {
                    pedidoLinea.VolumenPreparado = 0;
                }

                context.PedidosLineas.Add(pedidoLinea);
                context.SaveChanges();

                CargarPedidosLineas();
                PedidosLineasView.Refresh();
            }
        }
        #endregion


        #region Borrar Pedido Linea
        public ICommand BorrarPedidoLineaComando => _borrarPedidoLineaComando ??
            (_borrarPedidoCabeceraComando = new RelayCommandGenerico<IList<object>>(
                param => BorrarPedidoLinea(),
                param => PedidoLineaSeleccionado != null
            ));

        private async void BorrarPedidoLinea()
        {
            string pregunta = PedidosLineasSeleccionados.Count == 1
                   ? "¿Está seguro de que desea borrar el pedido línea " + PedidoLineaSeleccionado.PedidoLineaId + "?"
                   : "¿Está seguro de que desea borrar los pedidos líneas seleccionados?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {
                var pedidosLineasABorrar = new List<PedidoLinea>();

                foreach (var pedidoLinea in PedidosLineasSeleccionados)
                {
                    if (!context.PedidosLineas.Any(pl => pl.PedidoLineaId == pedidoLinea.PedidoLineaId))
                    {
                        pedidosLineasABorrar.Add(pedidoLinea);
                    }
                }
                context.PedidosLineas.RemoveRange(pedidosLineasABorrar);
                context.SaveChanges();

                if (PedidosLineasSeleccionados.Count != pedidosLineasABorrar.Count)
                {
                    string mensaje = PedidosLineasSeleccionados.Count == 1
                           ? "No se ha podido borrar el pedido línea seleccionado."
                           : "No se han podido borrar todas los pedidos líneas seleccionados.";
                    mensaje += "\n\nAsegurese de no que no exista ningún pedido cabecera asociada a dicho pedido línea.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
                PaginacionViewModel.Refrescar();
            }
        }
        #endregion


        #region Modificar Pedido Linea
        public ICommand ModificarPedidoLineaComando => _modificarPedidoLineaComando ??
            (_modificarPedidoLineaComando = new RelayCommand(
                param => ModificarPedidoLinea(),
                param => PedidoLineaSeleccionado != null
             ));

        public async void ModificarPedidoLinea()
        {
            var formPedidoLinea = new FormPedidoLinea(context, PedidoLineaSeleccionado);
            var formPedidoLineaDataContext = formPedidoLinea.DataContext as FormPedidoLineaViewModel;
            if ((bool)await DialogHost.Show(formPedidoLinea, "RootDialog"))
            {
                PedidoLineaSeleccionado.TipoProductoEnvasadoId = formPedidoLineaDataContext.TipoProductoEnvasado.TipoProductoEnvasadoId;
                PedidoLineaSeleccionado.Unidades = formPedidoLineaDataContext.Unidades;
                PedidoLineaSeleccionado.Volumen = formPedidoLineaDataContext.Volumen;
            }

            context.SaveChanges();
            PedidosLineasView.Refresh();
        }
        #endregion


        #region Refrescar Pedidos Lineas
        public ICommand RefrescarPedidosLineasComando => _refrescarPedidosLineasComando ??
            (_refrescarPedidosLineasComando = new RelayCommand(
                param => RefrescarPedidosLineas()
             ));

        public void RefrescarPedidosLineas()
        {
            PaginacionViewModel.Refrescar();
            PedidoLineaSeleccionado = null;
        }
        #endregion


        #region Filtro Pedidos Lineas
        public ICommand FiltrarPedidosLineasComando => _filtrarPedidosLineasComando ??
           (_filtrarPedidosLineasComando = new RelayCommand(
                param => FiltrarPedidosLineas()
           ));

        public void FiltrarPedidosLineas()
        {
            PedidosLineasView.Filter = FiltroPedidosLineas;
            PedidosLineasView.Refresh();
        }

        private bool FiltroPedidosLineas(object item)
        {
            var pedidoLinea = item as PedidoLinea;
            string volumen = pedidoLinea.Volumen.ToString();
            string unidades = pedidoLinea.Unidades.ToString();
            string tipoProducto = pedidoLinea.TipoProductoEnvasado.Nombre.ToLower();

            return (LineaVolumenSeleccionado == true ? volumen.Contains(TextoFiltroPedidos) : false)
                || (LineaUnidadesSeleccionadas == true ? unidades.Contains(TextoFiltroPedidos) : false)
                || (LineaTipoProductoEnvasadoSeleccionado == true ? tipoProducto.Contains(TextoFiltroPedidos) : false);
        }
        #endregion


        #region Añadir Pedido Detalle
        public ICommand AnadirPedidoDetalleComando => _anadirPedidoDetalleComando ??
            (_anadirPedidoDetalleComando = new RelayCommand(
                param => AnadirPedidoDetalle()
            ));


        private async void AnadirPedidoDetalle()
        {
            var formPedidoDetalle = new FormPedidoDetalle(context, PedidoLineaSeleccionado);

            if ((bool)await DialogHost.Show(formPedidoDetalle, "RootDialog"))
            {
                var formPedidoDetalleDataContext = formPedidoDetalle.DataContext as FormPedidoDetalleViewModel;
                var pedidoDetalle = new PedidoDetalle()
                {
                    ProductoEnvasadoId =context.ProductosEnvasados.Single(pe=>pe.Codigo== formPedidoDetalleDataContext.Codigo).ProductoEnvasadoId,
                    PedidoLineaId = PedidoLineaSeleccionado.PedidoLineaId,
                    Volumen = formPedidoDetalleDataContext.Volumen,
                    Unidades = formPedidoDetalleDataContext.Unidades

                };

                if(pedidoDetalle.Unidades != null)
                {
                    PedidoLineaSeleccionado.UnidadesPreparadas = pedidoDetalle.Unidades + formPedidoDetalleDataContext.Unidades;
                }
                else
                {
                    PedidoLineaSeleccionado.Volumen = pedidoDetalle.Unidades + formPedidoDetalleDataContext.Volumen;
                }

                context.PedidosDetalles.Add(pedidoDetalle);
                context.SaveChanges();
                CargarPedidosDetalles();
            }
        }
        #endregion


        #region Añadir Pedido Detalle Lector
        public ICommand AnadirPedidoDetalleLectorComando => _anadirPedidoDetalleComando ??
            (_anadirPedidoDetalleComando = new RelayCommand(
                param => AnadirPedidoDetalleLector()
            ));


        private async void AnadirPedidoDetalleLector()
        {
            var formPedidoDetalle = new FormPedidoDetalle(context,PedidoLineaSeleccionado);

            if ((bool)await DialogHost.Show(formPedidoDetalle, "RootDialog"))
            {
                var formPedidoDetalleDataContext = formPedidoDetalle.DataContext as FormPedidoDetalleViewModel;
                var pedidoDetalle = new PedidoDetalle()
                {
                    ProductoEnvasadoId = context.ProductosEnvasados.Single(pe => pe.Codigo == formPedidoDetalleDataContext.Codigo).ProductoEnvasadoId,
                    PedidoLineaId = PedidoLineaSeleccionado.PedidoLineaId,
                    Volumen = formPedidoDetalleDataContext.Volumen,
                    Unidades = formPedidoDetalleDataContext.Unidades

                };
                context.PedidosDetalles.Add(pedidoDetalle);
                context.SaveChanges();
                CargarPedidosDetalles();
            }
        }
        #endregion


        #region Borrar Pedido Detalle
        public ICommand BorrarPedidoDetalleComando => _borrarPedidoCabeceraComando ??
            (_borrarPedidoCabeceraComando = new RelayCommandGenerico<IList<object>>(
                param => BorrarPedidoDetalle(),
                param => PedidoDetalleSeleccionado != null
            ));

        private async void BorrarPedidoDetalle()
        {

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(), "RootDialog"))
            {
                var pedidosABorrar = new List<PedidoDetalle>();

                foreach (var pedidoDetalle in PedidosDetallesSeleccionados)
                {
                    if (!context.PedidosDetalles.Any(pd => pd.PedidoLineaId == pedidoDetalle.PedidoLineaId))
                    {
                        pedidosABorrar.Add(pedidoDetalle);
                    }
                }
                context.PedidosDetalles.RemoveRange(pedidosABorrar);
                context.SaveChanges();

                if (PedidosDetallesSeleccionados.Count != pedidosABorrar.Count)
                {
                    string mensaje = PedidosDetallesSeleccionados.Count == 1
                           ? "No se ha podido borrar el pedido detalle seleccionado."
                           : "No se han podido borrar todas los pedidos detalles seleccionados.";
                    mensaje += "\n\nAsegurese de no que no exista ningún pedido detalle asociada a dicho pedido.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
                PaginacionViewModel.Refrescar();
            }
        }
        #endregion


        #region Modificar Pedido Detalle
        public ICommand ModificarPedidoDetalleComando => _modificarPedidoCabeceraComando ??
            (_modificarPedidoCabeceraComando = new RelayCommand(
                param => ModificarPedidoDetalle()
                //param => PedidoDetalleSeleccionado != null
             ));

        public async void ModificarPedidoDetalle()
        {
            var formPedidoDetalle = new FormPedidoDetalle(context,PedidoLineaSeleccionado, PedidoDetalleSeleccionado);
            var formPedidoDetalleDataContext = formPedidoDetalle.DataContext as FormPedidoDetalleViewModel;

            //formPedido.cbClientes.SelectedValue = PedidoCabeceraSeleccionado.Cliente.ClienteId;

            if ((bool)await DialogHost.Show(formPedidoDetalle, "RootDialog"))
            {
                PedidoDetalleSeleccionado.ProductoEnvasadoId = formPedidoDetalleDataContext.ProductoEnvasado.ProductoEnvasadoId;
                PedidoDetalleSeleccionado.Unidades = formPedidoDetalleDataContext.Unidades;
                PedidoDetalleSeleccionado.Volumen = formPedidoDetalleDataContext.Volumen;
                //PedidoCabeceraSeleccionado.FechaPedido = new DateTime(formPedido.FechaPedido.Year, formPedido.FechaPedido.Month, formPedido.FechaPedido.Day, formPedido.FechaPedido.Hour, formPedido.HoraPedido.Minute, formPedido.HoraPedido.Second);
                //PedidoCabeceraSeleccionado.ClienteId = (formPedido.cbClientes.SelectedItem as Cliente).ClienteId;
                context.SaveChanges();
                PedidosDetallesView.Refresh();
            }
        }
        #endregion


        #region Refrescar Pedidos Detalles
        public ICommand RefrescarPedidosDetallesComando => _refrescarPedidosCabecerasComando ??
            (_refrescarPedidosDetallesComando = new RelayCommand(
                param => RefrescarPedidosDetalles()
             ));

        public void RefrescarPedidosDetalles()
        {
            PaginacionViewModel.Refrescar();
            //PedidoDetalleSeleccionado = null;
        }
        #endregion


        #region Filtro Pedidos Detalles
        public ICommand FiltrarPedidosDetallesComando => _filtrarPedidosDetallesComando ??
           (_filtrarPedidosDetallesComando = new RelayCommand(
                param => FiltrarPedidosDetalles()
           ));

        public void FiltrarPedidosDetalles()
        {
            PedidosDetallesView.Filter = FiltroPedidosDetalles;
            PedidosDetallesView.Refresh();
        }

        private bool FiltroPedidosDetalles(object item)
        {
            var pedidoDetalle = item as PedidoDetalle;
            string volumen = pedidoDetalle.Volumen.ToString();
            string unidades = pedidoDetalle.Unidades.ToString();
            string tipoProducto = pedidoDetalle.ProductoEnvasado.TipoProductoEnvasado.Nombre.ToLower();

            return (VolumenSeleccionado == true ? volumen.Contains(TextoFiltroPedidos) : false)
                || (UnidadesSeleccionadas == true ? unidades.Contains(TextoFiltroPedidos) : false)
                || (TipoProductoEnvasadoSeleccionado == true ? tipoProducto.Contains(TextoFiltroPedidos) : false);
        }
        #endregion

    }
}
