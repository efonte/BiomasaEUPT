using BiomasaEUPT.Clases;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
        private BiomasaEUPTEntidades context;
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
                context = BaseDeDatos.Instancia.biomasaEUPTEntidades;
                clientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("clientesViewSource")));
                tiposClientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("tipos_clientesViewSource")));
                gruposClientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("grupos_clientesViewSource")));
                context.clientes.Load();
                context.tipos_clientes.Load();
                context.grupos_clientes.Load();
                clientesViewSource.Source = context.clientes.Local;
                tiposClientesViewSource.Source = context.tipos_clientes.Local;
                gruposClientesViewSource.Source = context.grupos_clientes.Local;

                ucFiltroTabla.lbFiltro.SelectionChanged += (s, e1) => { FiltrarTabla(); };
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
                ucTablaClientes.cbPoblacion.Checked += (s, e1) => { FiltrarTabla(); };
                ucTablaClientes.cbPoblacion.Unchecked += (s, e1) => { FiltrarTabla(); };
            }
        }


        public void FiltrarTabla()
        {
            clientesViewSource.Filter += new FilterEventHandler(FiltroTabla);
        }

        private void FiltroTabla(object sender, FilterEventArgs e)
        {
            /* try
             {*/
            string textoBuscado = ucTablaClientes.tbBuscar.Text.ToLower();
            var cliente = e.Item as clientes;
            string razonSocial = cliente.razon_social.ToLower();
            string nif = cliente.nif.ToLower();
            string email = cliente.email.ToLower();
            string calle = cliente.calle.ToLower();
            string codigoPostal = cliente.direcciones.codigo_postal.ToLower();
            string poblacion = cliente.direcciones.poblacion.ToLower();
            string tipo = cliente.tipos_clientes.nombre.ToLower();
            // Filtra todos
            if (ucFiltroTabla.lbFiltro.SelectedItems.Count == 0)
            {
                e.Accepted = (ucTablaClientes.cbRazonSocial.IsChecked == true ? razonSocial.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbNif.IsChecked == true ? nif.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbCalle.IsChecked == true ? calle.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbCodigoPostal.IsChecked == true ? codigoPostal.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbPoblacion.IsChecked == true ? poblacion.Contains(textoBuscado) : false);
            }
            else
            {
                foreach (tipos_clientes tipoCliente in ucFiltroTabla.lbFiltro.SelectedItems)
                {
                    if (tipoCliente.nombre.ToLower().Equals(tipo))
                    {
                        // Si lo encuentra en el ListBox del filtro no hace falta que siga haciendo el foreach
                        e.Accepted = (ucTablaClientes.cbRazonSocial.IsChecked == true ? razonSocial.Contains(textoBuscado) : false) ||
                                     (ucTablaClientes.cbNif.IsChecked == true ? nif.Contains(textoBuscado) : false) ||
                                     (ucTablaClientes.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                                     (ucTablaClientes.cbCalle.IsChecked == true ? calle.Contains(textoBuscado) : false) ||
                                     (ucTablaClientes.cbCodigoPostal.IsChecked == true ? codigoPostal.Contains(textoBuscado) : false) ||
                                     (ucTablaClientes.cbPoblacion.IsChecked == true ? poblacion.Contains(textoBuscado) : false);
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
            Console.WriteLine(Validation.GetHasError(ucTablaClientes.dgClientes));
            return context != null && context.HayCambios<clientes>();
        }

        private void ConfirmarCambios()
        {
            context.GuardarCambios<clientes>();
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
                clientes clienteSeleccionado = ucTablaClientes.dgClientes.SelectedItem as clientes;
                //Console.WriteLine(clienteSeleccionado.razon_social);
                return clienteSeleccionado != null;
            }
            return false;
        }

        private async void BorrarCliente()
        {
            string pregunta = ucTablaClientes.dgClientes.SelectedItems.Count == 1
                ? "¿Está seguro de que desea borrar al cliente " + (ucTablaClientes.dgClientes.SelectedItem as clientes).razon_social + "?"
                : "¿Está seguro de que desea borrar los clientes seleccionados?";

            var mensaje = new MensajeConfirmacion(pregunta);
            mensaje.MaxHeight = ActualHeight;
            mensaje.MaxWidth = ActualWidth;

            var resultado = (bool)await DialogHost.Show(mensaje, "RootDialog");

            if (resultado)
            {
                context.clientes.RemoveRange(ucTablaClientes.dgClientes.SelectedItems.Cast<clientes>().ToList());
            }
        }
        #endregion


    }
}
