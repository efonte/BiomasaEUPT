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

namespace BiomasaEUPT.Vistas.GestionClientes
{
    /// <summary>
    /// Lógica de interacción para TabClientes.xaml
    /// </summary>
    public partial class TabClientes : UserControl
    {
        private BiomasaEUPTContext context;
        private CollectionViewSource clientesViewSource;
        private CollectionViewSource tiposClientesViewSource;
        private CollectionViewSource gruposClientesViewSource;

        public TabClientes()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = new BiomasaEUPTContext();
                clientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("clientesViewSource")));
                tiposClientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("tiposClientesViewSource")));
                gruposClientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("gruposClientesViewSource")));
                context.Clientes.Load();
                context.TiposClientes.Load();
                context.GruposClientes.Load();
                clientesViewSource.Source = context.Clientes.Local;
                tiposClientesViewSource.Source = context.TiposClientes.Local;
                gruposClientesViewSource.Source = context.GruposClientes.Local;

                ucFiltroTabla.lbFiltroTipo.SelectionChanged += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbRazonSocial.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbRazonSocial.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbNif.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbNif.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbEmail.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbEmail.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbCalle.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbCalle.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbCodigoPostal.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbCodigoPostal.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbMunicipio.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbMunicipio.Unchecked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.bAnadirCliente.Click += BAnadirCliente_Click;
            }
        }

        private async void BAnadirCliente_Click(object sender, RoutedEventArgs e)
        {
            var formCliente = new FormCliente();

            if ((bool)await DialogHost.Show(formCliente, "RootDialog"))
            {
                context.Clientes.Add(new Cliente()
                {
                    RazonSocial = formCliente.tbRazonSocial.Text,
                    Nif = formCliente.tbNif.Text,
                    Email = formCliente.tbEmail.Text,
                    Calle = formCliente.tbCalle.Text,
                    TipoCliente = formCliente.cbTiposClientes.SelectedItem as TipoCliente,
                    Municipio = formCliente.cbMunicipios.SelectedItem as Municipio,
                    Observaciones = formCliente.tbObservaciones.Text
                });
                context.SaveChanges();
            }
        }

        public void FiltrarTabla()
        {
            clientesViewSource.Filter += new FilterEventHandler(FiltroTabla);
        }

        private void FiltroTabla(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaClientes.tbBuscar.Text.ToLower();
            var cliente = e.Item as Cliente;
            string razonSocial = cliente.RazonSocial.ToLower();
            string nif = cliente.Nif.ToLower();
            string email = cliente.Email.ToLower();
            string calle = cliente.Calle.ToLower();
            string codigoPostal = cliente.Municipio.CodigoPostal.ToLower();
            string municipio = cliente.Municipio.Nombre.ToLower();
            string tipo = cliente.TipoCliente.Nombre.ToLower();

            var condicion = (ucTablaClientes.cbRazonSocial.IsChecked == true ? razonSocial.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbNif.IsChecked == true ? nif.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbCalle.IsChecked == true ? calle.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbCodigoPostal.IsChecked == true ? codigoPostal.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbMunicipio.IsChecked == true ? municipio.Contains(textoBuscado) : false);

            // Filtra todos
            if (ucFiltroTabla.lbFiltroTipo.SelectedItems.Count == 0)
            {
                e.Accepted = condicion;
            }
            else
            {
                foreach (TipoCliente tipoCliente in ucFiltroTabla.lbFiltroTipo.SelectedItems)
                {
                    if (tipoCliente.Nombre.ToLower().Equals(tipo))
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
            //return context != null && context.HayCambios<Cliente>();
            return context != null && context.ChangeTracker.HasChanges();
            // return true;
        }

        private async void ConfirmarCambios()
        {
            try
            {
                context.GuardarCambios<Cliente>();
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
            }
            //clientesViewSource.View.Refresh();
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
                        param => BorrarCliente(),
                        param => CanBorrar()
                    );
                }
                return _borrarComando;
            }
        }

        private bool CanBorrar()
        {
            if (ucTablaClientes.dgClientes.SelectedIndex != -1)
            {
                Cliente clienteSeleccionado = ucTablaClientes.dgClientes.SelectedItem as Cliente;
                //Console.WriteLine(clienteSeleccionado.razon_social);
                return clienteSeleccionado != null;
            }
            return false;
        }

        private async void BorrarCliente()
        {
            string pregunta = ucTablaClientes.dgClientes.SelectedItems.Count == 1
                ? "¿Está seguro de que desea borrar al cliente " + (ucTablaClientes.dgClientes.SelectedItem as Cliente).RazonSocial + "?"
                : "¿Está seguro de que desea borrar los clientes seleccionados?";

            var mensaje = new MensajeConfirmacion(pregunta);
            mensaje.MaxHeight = ActualHeight;
            mensaje.MaxWidth = ActualWidth;

            var resultado = (bool)await DialogHost.Show(mensaje, "RootDialog");

            if (resultado)
            {
                context.Clientes.RemoveRange(ucTablaClientes.dgClientes.SelectedItems.Cast<Cliente>().ToList());
            }
        }
        #endregion


        /*  #region AñadirCliente
          private ICommand _anadirClienteComando;

          public ICommand AnadirClienteComando
          {
              get
              {
                  if (_anadirClienteComando == null)
                  {
                      _anadirClienteComando = new RelayComando(
                          param => AnadirCliente(),
                          param => true
                      );
                  }
                  return _anadirClienteComando;
              }
          }


          private void AnadirCliente()
          {

          }

      }
      #endregion*/




    }
}
