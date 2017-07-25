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

    }
}
