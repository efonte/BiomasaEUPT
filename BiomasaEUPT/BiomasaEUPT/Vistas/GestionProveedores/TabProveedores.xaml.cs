using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = BaseDeDatos.Instancia.biomasaEUPTContext;
                proveedoresViewSource = ((CollectionViewSource)(ucTablaProveedores.FindResource("proveedoresViewSource")));
                tiposProveedoresViewSource = ((CollectionViewSource)(ucTablaProveedores.FindResource("tiposProveedoresViewSource")));
                context.Proveedores.Load();
                context.TiposProveedores.Load();
                proveedoresViewSource.Source = context.Proveedores.Local;
                tiposProveedoresViewSource.Source = context.TiposProveedores.Local;

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
                //ucTablaProveedores.bAnadirProveedor.Click += BAnadirProveedor_Click;
            }
        }

        /*private async void BAnadirProveedor_Click(object sender, RoutedEventArgs e)
        {
            var formProveedor = new FormProveedor();

            if ((bool)await DialogHost.Show(formProveedor, "RootDialog"))
            {
                context.clientes.Add(new clientes()
                {
                    razon_social = formProveedor.tbRazonSocial.Text,
                    nif = formProveedor.tbNif.Text,
                    email = formProveedor.tbEmail.Text,
                    calle = formProveedor.tbCalle.Text,
                    tipos_clientes = formProveedor.cbTiposClientes.SelectedItem as tipos_clientes,
                    direcciones = formProveedor.cbCodigosPostalesDirecciones.SelectedItem as direcciones,
                    observaciones = formProveedor.tbObservaciones.Text
                });
            }
        }*/

        public void FiltrarTabla()
        {
            proveedoresViewSource.Filter += new FilterEventHandler(FiltroTabla);
        }

        private void FiltroTabla(object sender, FilterEventArgs e)
        {
            /* try
             {*/
            string textoBuscado = ucTablaProveedores.tbBuscar.Text.ToLower();
            var proveedor = e.Item as Proveedor;
            string razonSocial = proveedor.RazonSocial.ToLower();
            string nif = proveedor.Nif.ToLower();
            string email = proveedor.Email.ToLower();
            string calle = proveedor.Calle.ToLower();
            string codigoPostal = proveedor.Municipio.CodigoPostal.ToLower();
            string municipio = proveedor.Municipio.Nombre.ToLower();
            string tipo = proveedor.TipoProveedor.Nombre.ToLower();
            // Filtra todos
            if (ucFiltroTabla.lbFiltroTipo.SelectedItems.Count == 0)
            {
                e.Accepted = (ucTablaProveedores.cbRazonSocial.IsChecked == true ? razonSocial.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbNif.IsChecked == true ? nif.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbCalle.IsChecked == true ? calle.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbCodigoPostal.IsChecked == true ? codigoPostal.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbMunicipio.IsChecked == true ? municipio.Contains(textoBuscado) : false);
            }
            else
            {
                foreach (TipoProveedor tipoProveedor in ucFiltroTabla.lbFiltroTipo.SelectedItems)
                {
                    if (tipoProveedor.Nombre.ToLower().Equals(tipo))
                    {
                        // Si lo encuentra en el ListBox del filtro no hace falta que siga haciendo el foreach
                        e.Accepted = (ucTablaProveedores.cbRazonSocial.IsChecked == true ? razonSocial.Contains(textoBuscado) : false) ||
                                     (ucTablaProveedores.cbNif.IsChecked == true ? nif.Contains(textoBuscado) : false) ||
                                     (ucTablaProveedores.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                                     (ucTablaProveedores.cbCalle.IsChecked == true ? calle.Contains(textoBuscado) : false) ||
                                     (ucTablaProveedores.cbCodigoPostal.IsChecked == true ? codigoPostal.Contains(textoBuscado) : false) ||
                                     (ucTablaProveedores.cbMunicipio.IsChecked == true ? municipio.Contains(textoBuscado) : false);
                        break;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }
            /* }
             // Ocurre cuando insertas una columna en la tabla pero no están todos los campos establecidos
             catch (NullReferenceException ex)
             {
                 e.Accepted = false;
             }*/
        }

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

        private bool asd(DbEntityEntry x)
        {
            foreach (var prop in x.OriginalValues.PropertyNames)
            {
                if (x.OriginalValues[prop].ToString() != x.CurrentValues[prop].ToString())
                {
                    return true;
                }
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
                        param => CanBorrar()
                    );
                }
                return _borrarComando;
            }
        }

        private bool CanBorrar()
        {
            if (ucTablaProveedores.dgProveedores.SelectedIndex != -1)
            {
                Proveedor proveedorSeleccionado = ucTablaProveedores.dgProveedores.SelectedItem as Proveedor;
                //Console.WriteLine(clienteSeleccionado.razon_social);
                return proveedorSeleccionado != null;
            }
            return false;
        }

        private async void BorrarProveedor()
        {
            string pregunta = ucTablaProveedores.dgProveedores.SelectedItems.Count == 1
                ? "¿Está seguro de que desea borrar al proveedor " + (ucTablaProveedores.dgProveedores.SelectedItem as Proveedor).RazonSocial + "?"
                : "¿Está seguro de que desea borrar los proveedores seleccionados?";

            var mensaje = new MensajeConfirmacion(pregunta);
            mensaje.MaxHeight = ActualHeight;
            mensaje.MaxWidth = ActualWidth;

            var resultado = (bool)await DialogHost.Show(mensaje, "RootDialog");

            if (resultado)
            {
                context.Proveedores.RemoveRange(ucTablaProveedores.dgProveedores.SelectedItems.Cast<Proveedor>().ToList());
            }
        }
        #endregion


        /*  #region AñadirProveedor
          private ICommand _anadirProveedorComando;

          public ICommand AnadirProveedorComando
          {
              get
              {
                  if (_anadirProveedorComando == null)
                  {
                      _anadirProveedorComando = new RelayComando(
                          param => AnadirProveedor(),
                          param => true
                      );
                  }
                  return _anadirProveedorComando;
              }
          }


          private void AnadirCliente()
          {

          }

      }
      #endregion*/




    }
}
