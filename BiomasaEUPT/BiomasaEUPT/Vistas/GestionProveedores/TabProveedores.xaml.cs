using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

namespace BiomasaEUPT.Vistas.GestionProveedores
{
    /// <summary>
    /// Lógica de interacción para TabProveedores.xaml
    /// </summary>
    public partial class TabProveedores : UserControl
    {
        private BiomasaEUPTContext context;
        private CollectionViewSource proveedoresViewSource;
        private CollectionViewSource tiposProveedoresViewSource;

        public TabProveedores()
        {
            InitializeComponent();
            DataContext = this;
            context = new BiomasaEUPTContext();

            ucTablaProveedores.dgProveedores.CellEditEnding += DgProveedores_CellEditEnding;
            ucFiltroTabla.lbFiltroTipo.SelectionChanged += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbRazonSocial.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbRazonSocial.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbNif.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbNif.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbEmail.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbEmail.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbCalle.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbCalle.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbCodigoPostal.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbCodigoPostal.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbMunicipio.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.cbMunicipio.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaProveedores.bAnadirProveedor.Click += BAnadirProveedor_Click;
            ucTablaProveedores.bRefrescar.Click += (s, e1) => { CargarProveedores(); };
            ucTablaProveedores.bEditarObservaciones.Click += BEditarObservaciones_Click;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            proveedoresViewSource = ((CollectionViewSource)(ucTablaProveedores.FindResource("proveedoresViewSource")));
            tiposProveedoresViewSource = ((CollectionViewSource)(ucTablaProveedores.FindResource("tiposProveedoresViewSource")));
            // CargarProveedores();
        }

        public void CargarProveedores()
        {
            using (new CursorEspera())
            {
                proveedoresViewSource.Source = context.Proveedores.ToList();
                tiposProveedoresViewSource.Source = context.TiposProveedores.ToList();
                ucTablaProveedores.dgProveedores.SelectedIndex = -1;
            }
        }

        private void DgProveedores_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var proveedor = e.Row.DataContext as Proveedor;
                context.SaveChanges();
                if (e.Column.DisplayIndex == 3) // 3 = Posición tipo proveedor
                {
                    (ucContador as Contador).Actualizar();
                }
            }
        }

        private async void BAnadirProveedor_Click(object sender, RoutedEventArgs e)
        {
            var formProveedor = new FormProveedor();

            if ((bool)await DialogHost.Show(formProveedor, "RootDialog"))
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
                CargarProveedores();
            }
        }

        #region FiltroTabla
        public void FiltrarTabla()
        {
            proveedoresViewSource.Filter += new FilterEventHandler(FiltroTabla);
        }

        private void FiltroTabla(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaProveedores.tbBuscar.Text.ToLower();
            var proveedor = e.Item as Proveedor;
            string razonSocial = proveedor.RazonSocial.ToLower();
            string nif = proveedor.Nif.ToLower();
            string email = proveedor.Email.ToLower();
            string calle = proveedor.Calle.ToLower();
            string codigoPostal = proveedor.Municipio.CodigoPostal.ToLower();
            string municipio = proveedor.Municipio.Nombre.ToLower();
            string tipo = proveedor.TipoProveedor.Nombre.ToLower();

            var condicion = (ucTablaProveedores.cbRazonSocial.IsChecked == true ? razonSocial.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbNif.IsChecked == true ? nif.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbCalle.IsChecked == true ? calle.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbCodigoPostal.IsChecked == true ? codigoPostal.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbMunicipio.IsChecked == true ? municipio.Contains(textoBuscado) : false);

            // Filtra todos
            if (ucFiltroTabla.lbFiltroTipo.SelectedItems.Count == 0)
            {
                e.Accepted = condicion;
            }
            else
            {
                foreach (TipoProveedor tipoProveedor in ucFiltroTabla.lbFiltroTipo.SelectedItems)
                {
                    if (tipoProveedor.Nombre.ToLower().Equals(tipo))
                    {
                        // Si lo encuentra en el ListBox del filtro no hace falta que siga haciendo el foreach
                        e.Accepted = condicion;
                        break;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }
        }
        #endregion

        #region ConfirmarCambios
        private ICommand _confirmarCambiosComando;

        public ICommand ConfirmarCambiosComando
        {
            get
            {
                if (_confirmarCambiosComando == null)
                {
                    _confirmarCambiosComando = new RelayComando(
                        param => ConfirmarCambios(),
                        param => CanConfirmarCambios()
                    );
                }
                return _confirmarCambiosComando;
            }
        }

        private bool CanConfirmarCambios()
        {
            return context != null && context.HayCambios<Proveedor>();
        }

        private async void ConfirmarCambios()
        {
            try
            {
                context.GuardarCambios<Proveedor>();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("\n", errorMessages);

                await DialogHost.Show(new MensajeInformacion("No pueden guardar los cambios:\n\n" + fullErrorMessage), "RootDialog");
                //Console.WriteLine(fullErrorMessage);               
            }
            //proveedoresViewSource.View.Refresh();
        }
        #endregion

        private bool HayUnProveedorSeleccionado()
        {
            if (ucTablaProveedores.dgProveedores.SelectedIndex != -1)
            {
                var proveedorSeleccionado = ucTablaProveedores.dgProveedores.SelectedItem as Proveedor;
                return proveedorSeleccionado != null;
            }
            return false;
        }

        #region Borrar
        private ICommand _borrarComando;

        public ICommand BorrarComando
        {
            get
            {
                if (_borrarComando == null)
                {
                    _borrarComando = new RelayComando(
                        param => BorrarProveedor(),
                        param => HayUnProveedorSeleccionado()
                    );
                }
                return _borrarComando;
            }
        }

        private async void BorrarProveedor()
        {
            var proveedoresABorrar = new List<Proveedor>();
            var proveedoresSeleccionados = ucTablaProveedores.dgProveedores.SelectedItems.Cast<Proveedor>().ToList();
            foreach (var proveedor in proveedoresSeleccionados)
            {
                if (!context.Recepciones.Any(r => r.ProveedorId == proveedor.ProveedorId))
                {
                    proveedoresABorrar.Add(proveedor);
                }
            }
            context.Proveedores.RemoveRange(proveedoresABorrar);
            context.SaveChanges();
            CargarProveedores();
            if (proveedoresSeleccionados.Count != proveedoresABorrar.Count)
            {
                string mensaje = ucTablaProveedores.dgProveedores.SelectedItems.Count == 1
                       ? "No se ha podido borrar el proveedor seleccionado."
                       : "No se han podido borrar todos los proveedores seleccionados.";
                mensaje += "\n\nAsegurese de no que no exista ninguna recepción asociada a dicho proveedor.";
                await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
            }
        }
        #endregion


        #region Editar
        private ICommand _modificarComando;
        public ICommand ModificarComando
        {
            get
            {
                if (_modificarComando == null)
                {
                    _modificarComando = new RelayComando(
                        param => ModificarProveedor(),
                        param => HayUnProveedorSeleccionado()
                    );
                }
                return _modificarComando;
            }
        }

        private async void ModificarProveedor()
        {
            var proveedorSeleccionado = ucTablaProveedores.dgProveedores.SelectedItem as Proveedor;
            var formProveedor = new FormProveedor(proveedorSeleccionado);
            if ((bool)await DialogHost.Show(formProveedor, "RootDialog"))
            {
                proveedorSeleccionado.RazonSocial = formProveedor.RazonSocial;
                proveedorSeleccionado.Nif = formProveedor.Nif;
                proveedorSeleccionado.Email = formProveedor.Email;
                proveedorSeleccionado.TipoId = (formProveedor.cbTiposProveedores.SelectedItem as TipoProveedor).TipoProveedorId;
                proveedorSeleccionado.MunicipioId = (formProveedor.cbMunicipios.SelectedItem as Municipio).MunicipioId;
                proveedorSeleccionado.Calle = formProveedor.Calle;
                proveedorSeleccionado.Observaciones = formProveedor.Observaciones;

                context.SaveChanges();
                proveedoresViewSource.View.Refresh();
            }
        }
        #endregion


        // Usado para el FormDireccion
        public BiomasaEUPTContext GetContext()
        {
            return context;
        }

        private void BEditarObservaciones_Click(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
            ucTablaProveedores.tbEditarObservaciones.IsChecked = false;
        }




    }
}
