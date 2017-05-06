using BiomasaEUPT.Clases;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

                ucFiltroTabla.lbFiltro.SelectionChanged += LbFiltro_SelectionChanged;
            }
        }

        private void LbFiltro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FiltrarTabla();
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
            string tipo = cliente.tipos_clientes.nombre.ToLower();

            // Filtra todos
            if (ucFiltroTabla.lbFiltro.SelectedItems.Count == 0)
            {
                e.Accepted = razonSocial.Contains(textoBuscado) || nif.Contains(textoBuscado)
                             || email.Contains(textoBuscado) || calle.Contains(textoBuscado);
            }
            else
            {
                foreach (tipos_clientes tipoCliente in ucFiltroTabla.lbFiltro.SelectedItems)
                {
                    if (tipoCliente.nombre.ToLower().Equals(tipo))
                    {
                        // Si lo encuentra en el ListBox del filtro no hace falta que siga haciendo el foreach
                        e.Accepted = razonSocial.Contains(textoBuscado) || nif.Contains(textoBuscado)
                                     || email.Contains(textoBuscado) || calle.Contains(textoBuscado);
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
            return context != null && context.ChangeTracker.HasChanges();
        }

        private void ConfirmarCambios()
        {
            Console.WriteLine("^^^^^^^^^^^^^^");
            context.SaveChanges();
            clientesViewSource.View.Refresh();
        }
        #endregion


        #region Borrar
        private ICommand _borrarComando;

        public ICommand BorrarComando
        {
            get
            {
                if (_borrarComando == null)
                {
                    _borrarComando = new RelayComando(
                        param => BorrarUsuario(),
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

        private async void BorrarUsuario()
        {
            if (ucTablaClientes.dgClientes.SelectedItems.Count == 1)
            {
                clientes clienteSeleccionado = ucTablaClientes.dgClientes.SelectedItem as clientes;
                var mensaje = new MensajeConfirmacion("¿Está seguro de que desea borrar al cliente '" + clienteSeleccionado.razon_social + "'?");
                mensaje.MaxHeight = ActualHeight;
                mensaje.MaxWidth = ActualWidth;

                var resultado = (bool)await DialogHost.Show(mensaje, "RootDialog");

                if (resultado)
                {
                    context.clientes.Remove(clienteSeleccionado);
                }
            }
            else if (ucTablaClientes.dgClientes.SelectedItems.Count > 1)
            {
                /* foreach (var item in ucTablaClientes.dgClientes.SelectedItems)
                 {
                     clientes clienteSeleccionado = item as clientes;
                     Console.WriteLine(clienteSeleccionado.razon_social);
                     context.clientes.Local.Remove(clienteSeleccionado); // Produce excepción ARREGLAR!!!!!!!!!!!!!!!
                 }*/
                //ucTablaUsuarios.dgUsuarios.Items.Remove(ucTablaUsuarios.dgUsuarios.SelectedItems);
            }
        }
        #endregion


    }
}
