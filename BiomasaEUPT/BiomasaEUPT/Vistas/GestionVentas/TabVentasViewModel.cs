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

        public ObservableCollection<PedidoDetalle> PedidosDetalles { get; set; }
        public CollectionView PedidosDetallesView { get; private set; }
        public IList<PedidoDetalle> PedidosDetallesSeleccionados { get; set; }

        
        public int IndiceMasOpciones { get; set; }

        // Checkbox Filtro Pedidos Cabceceras
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

        // Checkbox Filtro Pedidos Detalles
        public bool VolumenSeleccionado { get; set; } = true;
        public bool UnidadesSeleccionadas { get; set; } = true;
        public bool TipoProductoTerminadoSeleccionado { get; set; } = true;

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

        private ICommand _anadirProductoEnvasadoComando;
        private ICommand _modificarProductoEnvasadoComando;
        private ICommand _borrarProductoEnvasadoComando;
        private ICommand _refrescarProductosEnvasadosComando;
        private ICommand _filtrarProductosEnvasadosComando;

        private ICommand _anadirPedidoDetalleComando;
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

        
        public void CargarPedidosDetalles(int cantidad = 10, int saltar = 0)
        {

            /*using (new CursorEspera())
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
                .Include(pd => pd.PedidoCabeceraId).Include(pd => pd.TipoProductoEnvasado)
                .OrderBy(pd => pd.PedidoCabeceraId).Skip(saltar).Take(cantidad).ToList());
                }
                PedidosDetallesView = (CollectionView)CollectionViewSource.GetDefaultView(PedidosDetalles);

                // Por defecto no está seleccionada ninguna fila del datagrid pedidos
                //PedidoDetalleSeleccionado = null;
            }*/
        }


        // Asigna el valor de PedidosCabecerasSeleccinodos ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGPedidosCabeceras_SelectionChangedComando => _dgPedidosCabeceras_SelectionChangedComando ??
            (_dgPedidosCabeceras_SelectionChangedComando = new RelayCommandGenerico<IList<object>>(
                param => DGPedidosabeceras_SelectionChanged(param)
            ));

        private void DGPedidosabeceras_SelectionChanged(IList<object> recepcionesSeleccionadas)
        {
            PedidosCabecerasSeleccionados = PedidosCabecerasSeleccionados.Cast<PedidoCabecera>().ToList();
            CargarPedidosDetalles();
        }

        // Asigna el valor de ProducosEnvasadosSeleccionados ya que no se puede crear un Binding de SelectedItems desde el XAML
        //public ICommand DGProductosEnvasados_SelectionChangedComando => new RelayCommandGenerico<IList<object>>(param => ProductosEnvasadosSeleccionados = param.Cast<ProductoEnvasado>().ToList());


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
                //CargarPedidosCabeceras();
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
            formPedido.cbClientes.SelectedValue = PedidoCabeceraSeleccionado.Cliente.ClienteId;

            if ((bool)await DialogHost.Show(formPedido, "RootDialog"))
            {
                PedidoCabeceraSeleccionado.FechaPedido = new DateTime(formPedido.FechaPedido.Year, formPedido.FechaPedido.Month, formPedido.FechaPedido.Day, formPedido.FechaPedido.Hour, formPedido.HoraPedido.Minute, formPedido.HoraPedido.Second);
                PedidoCabeceraSeleccionado.ClienteId = (formPedido.cbClientes.SelectedItem as Cliente).ClienteId;
                PedidoCabeceraSeleccionado.EstadoId = 1;
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

        #region Añadir Pedido Detalle
        public ICommand AnadirPedidoDetalleComando => _anadirPedidoDetalleComando ??
            (_anadirPedidoDetalleComando = new RelayCommand(
                param => AnadirPedidoDetalle(),
                param => CanAnadirPedidoDetalle()
            ));

        private bool CanAnadirPedidoDetalle()
        {
            if (PedidoCabeceraSeleccionado != null)
            {
                return PedidoCabeceraSeleccionado.EstadoId == 1; // Nuevo 
            }
            return false;
        }

        private async void AnadirPedidoDetalle()
        {
            var formPedidoDetalle = new FormPedidoDetalle(context);

            if ((bool)await DialogHost.Show(formPedidoDetalle, "RootDialog"))
            {
                var formPedidoDetalleDataContext = formPedidoDetalle.DataContext as FormPedidoDetalleViewModel;
                var pedidoDetalle = new PedidoDetalle()
                {
                    //PedidoCabeceraId = PedidoCabeceraSeleccionado.PedidoCabeceraId,
                    Volumen = formPedidoDetalleDataContext.Volumen,
                    Unidades = formPedidoDetalleDataContext.Unidades
                    
                    
                };

                var pedidosDetalles = new List<PedidoDetalle>();
                foreach (var pd in context.PedidosDetalles)
                {
                    if (pd.Unidades != 0 && pd.Volumen != 0)
                    {

                        //pd.PedidoCabecera = PedidoCabeceraSeleccionado;
                        pedidosDetalles.Add(pd);
                    }
                }

                context.PedidosDetalles.Add(pedidoDetalle);

                context.SaveChanges();

                CargarPedidosDetalles();
            }
        }
        #endregion

        #region Borrar Pedido Detalle
        public ICommand BorrarPedidoDetalleComando => _borrarPedidoCabeceraComando ??
            (_borrarPedidoCabeceraComando = new RelayCommandGenerico<IList<object>>(
                param => BorrarPedidoDetalle()
                //param => PedidoDetalleSeleccionado != null
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

            var formPedidoDetalle = new FormPedidoDetalle(context, "Editar Pedido")
            {
                //FechaPedido = PedidoCabeceraSeleccionado.FechaPedido,
                //HoraPedido = PedidoCabeceraSeleccionado.HoraPedido
            };
            //formPedido.cbClientes.SelectedValue = PedidoCabeceraSeleccionado.Cliente.ClienteId;

            if ((bool)await DialogHost.Show(formPedidoDetalle, "RootDialog"))
            {
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

            return (FechaPedidoSeleccionado == true ? volumen.Contains(TextoFiltroPedidos) : false)
                || (ClientePedidoSeleccionado == true ? unidades.Contains(TextoFiltroPedidos) : false)
                || (TipoProductoTerminadoSeleccionado == true ? tipoProducto.Contains(TextoFiltroPedidos) : false);
        }
        #endregion

    }
}
