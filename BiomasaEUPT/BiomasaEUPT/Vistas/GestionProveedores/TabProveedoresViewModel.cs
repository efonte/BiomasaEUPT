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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.GestionProveedores
{
    public class TabProveedoresViewModel : ViewModelBase
    {

        public ObservableCollection<Proveedor> Proveedores { get; set; }
        public CollectionView ProveedoresView { get; private set; }
        public ObservableCollection<TipoProveedor> TiposProveedores { get; set; }
        public IList<Proveedor> ProveedoresSeleccionados { get; set; }
        public Proveedor ProveedorSeleccionado { get; set; }
        public FiltroTablaViewModel FiltroTablaViewModel { get; set; }
        public bool ObservacionesEnEdicion { get; set; }

        private ICommand _anadirComando;
        private ICommand _modificarComando;
        private ICommand _borrarComando;
        private ICommand _dgProveedores_CellEditEndingComando;
        private ICommand _dgProveedores_RowEditEndingComando;
        private ICommand _modificarObservacionesComando;

        public TabProveedoresViewModel()
        {
            FiltroTablaViewModel = new FiltroTablaViewModel()
            {
                ViewModel = this
            };
        }

        public override void Inicializar()
        {
            CargarProveedores();
            FiltroTablaViewModel.CargarFiltro();
        }

        public void CargarProveedores()
        {
            using (new CursorEspera())
            {
                using (var context = new BiomasaEUPTContext())
                {
                    Proveedores = new ObservableCollection<Proveedor>(context.Proveedores.Include(p => p.Municipio.Provincia.Comunidad.Pais).ToList());
                    ProveedoresView = (CollectionView)CollectionViewSource.GetDefaultView(Proveedores);
                    TiposProveedores = new ObservableCollection<TipoProveedor>(context.TiposProveedores.ToList());

                    // Por defecto no está seleccionada ninguna fila del datagrid proveedores
                    ProveedorSeleccionado = null;
                }
            }
        }

        // Asigna el valor de ProveedoresSeleccionados ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGProveedores_SelectionChangedComando => new RelayCommand2<IList<object>>(param => ProveedoresSeleccionados = param.Cast<Proveedor>().ToList());

        #region Editar Celda
        public ICommand DGProveedores_CellEditEndingComando => _dgProveedores_CellEditEndingComando ??
            (_dgProveedores_CellEditEndingComando = new RelayCommand2<DataGridCellEditEndingEventArgs>(
                 param => EditarCeldaProveedor(param)
            ));

        private void EditarCeldaProveedor(DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var proveedorSeleccionado = e.Row.DataContext as Proveedor;
                using (var context = new BiomasaEUPTContext())
                {
                    var proveedor = context.Proveedores.Single(u => u.ProveedorId == proveedorSeleccionado.ProveedorId);

                    proveedor.RazonSocial = proveedorSeleccionado.RazonSocial;
                    proveedor.Nif = proveedorSeleccionado.Nif;
                    proveedor.Email = proveedorSeleccionado.Email;
                    proveedor.TipoId = proveedorSeleccionado.TipoProveedor.TipoProveedorId;
                    proveedor.Calle = proveedorSeleccionado.Calle;
                    proveedor.MunicipioId = proveedorSeleccionado.Municipio.MunicipioId;
                    // cliente.Observaciones = clienteSeleccionado.Observaciones;
                    Console.WriteLine(proveedor.MunicipioId);
                    context.SaveChanges();
                }

                if (e.Column.DisplayIndex == 3) // 3 = Posición tipo proveedor
                {
                    //(ucContador as Contador).Actualizar();
                }
            }
        }
        #endregion

        #region Editar Fila
        public ICommand DGProveedores_RowEditEndingComando => _dgProveedores_RowEditEndingComando ??
            (_dgProveedores_RowEditEndingComando = new RelayCommand2<DataGridRowEditEndingEventArgs>(
                 param => EditarFilaProveedor(param)
            ));

        private void EditarFilaProveedor(DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var proveedorSeleccionado = e.Row.DataContext as Proveedor;
                using (var context = new BiomasaEUPTContext())
                {
                    var proveedor = context.Proveedores.Single(u => u.ProveedorId == proveedorSeleccionado.ProveedorId);

                    proveedor.RazonSocial = proveedorSeleccionado.RazonSocial;
                    proveedor.Nif = proveedorSeleccionado.Nif;
                    proveedor.Email = proveedorSeleccionado.Email;
                    proveedor.TipoId = proveedorSeleccionado.TipoProveedor.TipoProveedorId;
                    proveedor.Calle = proveedorSeleccionado.Calle;
                    proveedor.MunicipioId = proveedorSeleccionado.Municipio.MunicipioId;
                    Console.WriteLine(proveedor.MunicipioId);
                    context.SaveChanges();
                }

                /* if (e.Column.DisplayIndex == 3) // 3 = Posición tipo proveedor
                 {
                     //(ucContador as Contador).Actualizar();
                 }*/
            }
        }
        #endregion

        #region Añadir
        public ICommand AnadirComando => _anadirComando ??
            (_anadirComando = new RelayComando(
                param => AnadirProveedor()
            ));

        private async void AnadirProveedor()
        {
            var formProveedor = new FormProveedor();

            if ((bool)await DialogHost.Show(formProveedor, "RootDialog"))
            {
                using (var context = new BiomasaEUPTContext())
                {
                    context.Proveedores.Add(new Proveedor()
                    {
                        RazonSocial = formProveedor.RazonSocial,
                        Nif = formProveedor.Nif,
                        Email = formProveedor.Email,
                        Calle = formProveedor.Calle,
                        TipoId = (formProveedor.cbTiposProveedores.SelectedItem as TipoProveedor).TipoProveedorId,
                        MunicipioId = (formProveedor.cbMunicipios.SelectedItem as Municipio).MunicipioId,
                        Observaciones = formProveedor.Observaciones
                    });
                    context.SaveChanges();
                }
                //CargarClientes();
            }
        }
        #endregion

        #region Borrar     
        public ICommand BorrarComando => _borrarComando ??
            (_borrarComando = new RelayCommand2<IList<object>>(
                param => BorrarProveedor(),
                param => ProveedorSeleccionado != null
            ));

        private async void BorrarProveedor()
        {
            string pregunta = ProveedoresSeleccionados.Count == 1
               ? "¿Está seguro de que desea borrar al proveedor " + ProveedorSeleccionado.RazonSocial + "?"
               : "¿Está seguro de que desea borrar los proveedores seleccionados?";

            var resultado = (bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog");

            if (resultado)
            {
                var proveedoresABorrar = new List<Proveedor>();

                using (var context = new BiomasaEUPTContext())
                {
                    foreach (var proveedor in ProveedoresSeleccionados)
                    {
                        if (!context.Recepciones.Any(pc => pc.ProveedorId == proveedor.ProveedorId))
                        {
                            proveedoresABorrar.Add(proveedor);
                        }
                    }
                    context.Proveedores.RemoveRange(proveedoresABorrar);
                    context.SaveChanges();
                    CargarProveedores();
                }
                if (ProveedoresSeleccionados.Count != proveedoresABorrar.Count)
                {
                    string mensaje = ProveedoresSeleccionados.Count == 1
                           ? "No se ha podido borrar el proveedor seleccionado."
                           : "No se han podido borrar todos los proveedores seleccionados.";
                    mensaje += "\n\nAsegurese de no que no exista ninguna recepción asociada a dicho proveedor.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
            }
        }
        #endregion

        #region Editar
        public ICommand ModificarComando => _modificarComando ??
            (_modificarComando = new RelayComando(
                param => ModificarProveedor(),
                param => ProveedorSeleccionado != null
             ));

        private async void ModificarProveedor()
        {
            var formProveedor = new FormProveedor(ProveedorSeleccionado);
            if ((bool)await DialogHost.Show(formProveedor, "RootDialog"))
            {
                ProveedorSeleccionado.RazonSocial = formProveedor.RazonSocial;
                ProveedorSeleccionado.Nif = formProveedor.Nif;
                ProveedorSeleccionado.Email = formProveedor.Email;
                ProveedorSeleccionado.TipoId = (formProveedor.cbTiposProveedores.SelectedItem as TipoProveedor).TipoProveedorId;
                ProveedorSeleccionado.MunicipioId = (formProveedor.cbMunicipios.SelectedItem as Municipio).MunicipioId;
                ProveedorSeleccionado.Calle = formProveedor.Calle;
                ProveedorSeleccionado.Observaciones = formProveedor.Observaciones;
                using (var context = new BiomasaEUPTContext())
                {
                    context.SaveChanges();
                }
            }
        }
        #endregion

        #region Modificar Observaciones Proveedor
        public ICommand ModificarObservacionesComando => _modificarObservacionesComando ??
            (_modificarObservacionesComando = new RelayComando(
                param => ModificarObservacionesProveedor(),
                param => ProveedorSeleccionado != null
             ));

        private void ModificarObservacionesProveedor()
        {
            using (var context = new BiomasaEUPTContext())
            {
                var proveedor = context.Proveedores.Single(u => u.ProveedorId == ProveedorSeleccionado.ProveedorId);

                proveedor.Observaciones = ProveedorSeleccionado.Observaciones;
                context.SaveChanges();
            }
            ObservacionesEnEdicion = false;
        }
        #endregion

    }
}
