using BiomasaEUPT.Clases;
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
    /// Lógica de interacción para TabClientes.xaml
    /// </summary>
    public partial class TabProveedores : UserControl
    {
        private BiomasaEUPTEntidades context;
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
                context = BaseDeDatos.Instancia.biomasaEUPTEntidades;
                proveedoresViewSource = ((CollectionViewSource)(ucTablaProveedores.FindResource("proveedoresViewSource")));
                tiposProveedoresViewSource = ((CollectionViewSource)(ucTablaProveedores.FindResource("tipos_proveedoresViewSource")));
                context.proveedores.Load();
                context.tipos_proveedores.Load();
                proveedoresViewSource.Source = context.proveedores.Local;
                tiposProveedoresViewSource.Source = context.tipos_proveedores.Local;

                ucFiltroTabla.lbFiltro.SelectionChanged += (s, e1) => { FiltrarTabla(); };
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
                ucTablaProveedores.cbPoblacion.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaProveedores.cbPoblacion.Unchecked += (s, e1) => { FiltrarTabla(); };
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
            var proveedor = e.Item as proveedores;
            string razonSocial = proveedor.razon_social.ToLower();
            string nif = proveedor.nif.ToLower();
            string email = proveedor.email.ToLower();
            string calle = proveedor.calle.ToLower();
            string codigoPostal = proveedor.direcciones.codigo_postal.ToLower();
            string poblacion = proveedor.direcciones.poblacion.ToLower();
            string tipo = proveedor.tipos_proveedores.nombre.ToLower();
            // Filtra todos
            if (ucFiltroTabla.lbFiltro.SelectedItems.Count == 0)
            {
                e.Accepted = (ucTablaProveedores.cbRazonSocial.IsChecked == true ? razonSocial.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbNif.IsChecked == true ? nif.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbCalle.IsChecked == true ? calle.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbCodigoPostal.IsChecked == true ? codigoPostal.Contains(textoBuscado) : false) ||
                             (ucTablaProveedores.cbPoblacion.IsChecked == true ? poblacion.Contains(textoBuscado) : false);
            }
            else
            {
                foreach (tipos_proveedores tipoProveedor in ucFiltroTabla.lbFiltro.SelectedItems)
                {
                    if (tipoProveedor.nombre.ToLower().Equals(tipo))
                    {
                        // Si lo encuentra en el ListBox del filtro no hace falta que siga haciendo el foreach
                        e.Accepted = (ucTablaProveedores.cbRazonSocial.IsChecked == true ? razonSocial.Contains(textoBuscado) : false) ||
                                     (ucTablaProveedores.cbNif.IsChecked == true ? nif.Contains(textoBuscado) : false) ||
                                     (ucTablaProveedores.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                                     (ucTablaProveedores.cbCalle.IsChecked == true ? calle.Contains(textoBuscado) : false) ||
                                     (ucTablaProveedores.cbCodigoPostal.IsChecked == true ? codigoPostal.Contains(textoBuscado) : false) ||
                                     (ucTablaProveedores.cbPoblacion.IsChecked == true ? poblacion.Contains(textoBuscado) : false);
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
            return context != null && context.HayCambios<clientes>();
        }

        private async void ConfirmarCambios()
        {
            try
            {
                context.GuardarCambios<proveedores>();
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
                proveedores proveedorSeleccionado = ucTablaProveedores.dgProveedores.SelectedItem as proveedores;
                //Console.WriteLine(clienteSeleccionado.razon_social);
                return proveedorSeleccionado != null;
            }
            return false;
        }

        private async void BorrarProveedor()
        {
            string pregunta = ucTablaProveedores.dgProveedores.SelectedItems.Count == 1
                ? "¿Está seguro de que desea borrar al proveedor " + (ucTablaProveedores.dgProveedores.SelectedItem as proveedores).razon_social + "?"
                : "¿Está seguro de que desea borrar los proveedores seleccionados?";

            var mensaje = new MensajeConfirmacion(pregunta);
            mensaje.MaxHeight = ActualHeight;
            mensaje.MaxWidth = ActualWidth;

            var resultado = (bool)await DialogHost.Show(mensaje, "RootDialog");

            if (resultado)
            {
                context.proveedores.RemoveRange(ucTablaProveedores.dgProveedores.SelectedItems.Cast<proveedores>().ToList());
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
