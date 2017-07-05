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
            context = new BiomasaEUPTContext();
            ucTablaClientes.dgClientes.CellEditEnding += DgClientes_CellEditEnding;
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
            ucTablaClientes.bRefrescar.Click += (s, e1) => { CargarClientes(); };
            ucTablaClientes.bEditarObservaciones.Click += BEditarObservaciones_Click;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            clientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("clientesViewSource")));
            tiposClientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("tiposClientesViewSource")));
            gruposClientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("gruposClientesViewSource")));
            CargarClientes();
        }

        public void CargarClientes()
        {
            using (new CursorEspera())
            {
                clientesViewSource.Source = context.Clientes.ToList();
                tiposClientesViewSource.Source = context.TiposClientes.ToList();
                gruposClientesViewSource.Source = context.GruposClientes.ToList();

                // Por defecto no está seleccionada ninguna fila del datagrid clientes 
                ucTablaClientes.dgClientes.SelectedIndex = -1;
            }
        }

        private void DgClientes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var cliente = e.Row.DataContext as Cliente;
                context.SaveChanges();
                if (e.Column.DisplayIndex == 3) // 3 = Posición tipo cliente
                {
                    (ucContador as Contador).Actualizar();
                }
            }
        }

        private async void BAnadirCliente_Click(object sender, RoutedEventArgs e)
        {
            var formCliente = new FormCliente();

            if ((bool)await DialogHost.Show(formCliente, "RootDialog"))
            {
                context.Clientes.Add(new Cliente()
                {
                    RazonSocial = formCliente.RazonSocial,
                    Nif = formCliente.Nif,
                    Email = formCliente.Email,
                    Calle = formCliente.Calle,
                    TipoId = (formCliente.cbTiposClientes.SelectedItem as TipoCliente).TipoClienteId,
                    GrupoId = (formCliente.cbGruposClientes.SelectedItem as GrupoCliente).GrupoClienteId,
                    MunicipioId = (formCliente.cbMunicipios.SelectedItem as Municipio).MunicipioId,
                    Observaciones = formCliente.Observaciones
                });
                context.SaveChanges();
                CargarClientes();
            }
        }

        #region FiltroTabla
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
        #endregion

        private bool HayUnClienteSeleccionado()
        {
            if (ucTablaClientes.dgClientes.SelectedIndex != -1)
            {
                var clienteSeleccionado = ucTablaClientes.dgClientes.SelectedItem as Cliente;
                return clienteSeleccionado != null;
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
                        param => HayUnClienteSeleccionado()
                    );
                }
                return _borrarComando;
            }
        }

        private async void BorrarCliente()
        {
            var clientesABorrar = new List<Cliente>();
            var clientesSeleccionados = ucTablaClientes.dgClientes.SelectedItems.Cast<Cliente>().ToList();
            foreach (var cliente in clientesSeleccionados)
            {
                if (!context.PedidosCabeceras.Any(pc => pc.ClienteId == cliente.ClienteId))
                {
                    clientesABorrar.Add(cliente);
                }
            }
            context.Clientes.RemoveRange(clientesABorrar);
            context.SaveChanges();
            CargarClientes();
            if (clientesSeleccionados.Count != clientesABorrar.Count)
            {
                string mensaje = ucTablaClientes.dgClientes.SelectedItems.Count == 1
                       ? "No se ha podido borrar el cliente seleccionado."
                       : "No se han podido borrar todos los clientes seleccionados.";
                mensaje += "\n\nAsegurese de no que no exista ningún pedido asociado a dicho cliente.";
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
                        param => ModificarCliente(),
                        param => HayUnClienteSeleccionado()
                    );
                }
                return _modificarComando;
            }
        }

        private async void ModificarCliente()
        {
            var clienteSeleccionado = ucTablaClientes.dgClientes.SelectedItem as Cliente;
            var formCliente = new FormCliente(clienteSeleccionado);
            if ((bool)await DialogHost.Show(formCliente, "RootDialog"))
            {
                clienteSeleccionado.RazonSocial = formCliente.RazonSocial;
                clienteSeleccionado.Nif = formCliente.Nif;
                clienteSeleccionado.Email = formCliente.Email;
                clienteSeleccionado.TipoId = (formCliente.cbTiposClientes.SelectedItem as TipoCliente).TipoClienteId;
                clienteSeleccionado.GrupoId = (formCliente.cbGruposClientes.SelectedItem as GrupoCliente).GrupoClienteId;
                clienteSeleccionado.MunicipioId = (formCliente.cbMunicipios.SelectedItem as Municipio).MunicipioId;
                clienteSeleccionado.Calle = formCliente.Calle;
                clienteSeleccionado.Observaciones = formCliente.Observaciones;

                context.SaveChanges();
                clientesViewSource.View.Refresh();
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
            ucTablaClientes.tbEditarObservaciones.IsChecked = false;
        }
    }
}
