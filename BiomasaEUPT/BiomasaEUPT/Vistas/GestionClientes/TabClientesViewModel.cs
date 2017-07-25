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

namespace BiomasaEUPT.Vistas.GestionClientes
{
    public class TabClientesViewModel : ViewModelBase
    {
        public ObservableCollection<Cliente> Clientes { get; set; }
        public CollectionView ClientesView { get; private set; }
        public ObservableCollection<TipoCliente> TiposClientes { get; set; }
        public ObservableCollection<GrupoCliente> GruposClientes { get; set; }
        public IList<Cliente> ClientesSeleccionados { get; set; }
        public Cliente ClienteSeleccionado { get; set; }
        public FiltroTablaViewModel FiltroTablaViewModel { get; set; }
        public bool ObservacionesEnEdicion { get; set; }
        public ContadorViewModel<TipoCliente> ContadorViewModel { get; set; }

        // Checkbox Filtro Clientes
        public bool RazonSocialSeleccionada { get; set; } = true;
        public bool NifSeleccionado { get; set; } = true;
        public bool EmailSeleccionado { get; set; } = false;
        public bool CalleSeleccionada { get; set; } = false;
        public bool CodigoPostalSeleccionado { get; set; } = false;
        public bool MunicipioSeleccionado { get; set; } = false;

        private string _textoFiltroClientes = "";
        public string TextoFiltroClientes
        {
            get { return _textoFiltroClientes; }
            set
            {
                _textoFiltroClientes = value.ToLower();
                FiltrarClientes();
            }
        }

        private ICommand _anadirClienteComando;
        private ICommand _modificarClienteComando;
        private ICommand _borrarClienteComando;
        private ICommand _refrescarClientesComando;
        private ICommand _filtrarClientesComando;
        private ICommand _dgClientes_CellEditEndingComando;
        private ICommand _dgClientes_RowEditEndingComando;
        private ICommand _modificarObservacionesClienteComando;

        public BiomasaEUPTContext Context { get; set; }


        public TabClientesViewModel()
        {
            FiltroTablaViewModel = new FiltroTablaViewModel()
            {
                ViewModel = this
            };
        }


        public override void Inicializar()
        {
            Context = new BiomasaEUPTContext();
            CargarClientes();
            FiltroTablaViewModel.CargarFiltro();
        }

        public void CargarClientes()
        {
            using (new CursorEspera())
            {
                Clientes = new ObservableCollection<Cliente>(Context.Clientes.Include(c => c.Municipio.Provincia.Comunidad.Pais).ToList());
                ClientesView = (CollectionView)CollectionViewSource.GetDefaultView(Clientes);
                TiposClientes = new ObservableCollection<TipoCliente>(Context.TiposClientes.ToList());
                GruposClientes = new ObservableCollection<GrupoCliente>(Context.GruposClientes.ToList());
                ContadorViewModel.Tipos = TiposClientes;

                // Por defecto no está seleccionada ninguna fila del datagrid clientes 
                ClienteSeleccionado = null;
            }
        }


        // Asigna el valor de ClientesSeleccionados ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGClientes_SelectionChangedComando => new RelayCommandGenerico<IList<object>>(param => ClientesSeleccionados = param.Cast<Cliente>().ToList());


        #region Editar Celda
        public ICommand DGClientes_CellEditEndingComando => _dgClientes_CellEditEndingComando ??
            (_dgClientes_CellEditEndingComando = new RelayCommandGenerico<DataGridCellEditEndingEventArgs>(
                 param => EditarCeldaCliente(param)
            ));

        private void EditarCeldaCliente(DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var clienteSeleccionado = e.Row.DataContext as Cliente;

                /*var cliente = context.Clientes.Single(u => u.ClienteId == clienteSeleccionado.ClienteId);

                cliente.RazonSocial = clienteSeleccionado.RazonSocial;
                cliente.Nif = clienteSeleccionado.Nif;
                cliente.Email = clienteSeleccionado.Email;
                cliente.TipoId = clienteSeleccionado.TipoCliente.TipoClienteId;
                cliente.GrupoId = clienteSeleccionado.GrupoCliente.GrupoClienteId;
                cliente.Calle = clienteSeleccionado.Calle;
                cliente.MunicipioId = clienteSeleccionado.Municipio.MunicipioId;
                // cliente.Observaciones = clienteSeleccionado.Observaciones;*/
                Context.SaveChanges();

                if (e.Column.DisplayIndex == 3) // 3 = Posición tipo cliente
                {
                    ContadorViewModel.Tipos = new ObservableCollection<TipoCliente>(Context.TiposClientes.ToList());
                }
            }
        }
        #endregion


        #region Editar Fila
        public ICommand DGClientes_RowEditEndingComando => _dgClientes_RowEditEndingComando ??
            (_dgClientes_RowEditEndingComando = new RelayCommandGenerico<DataGridRowEditEndingEventArgs>(
                 param => EditarFilaCliente(param)
            ));

        private void EditarFilaCliente(DataGridRowEditEndingEventArgs e)
        {
            /* if (e.EditAction == DataGridEditAction.Commit)
             {
                 var clienteSeleccionado = e.Row.DataContext as Cliente;
                 using (var context = new BiomasaEUPTContext())
                 {
                     var cliente = context.Clientes.Single(u => u.ClienteId == clienteSeleccionado.ClienteId);

                     cliente.RazonSocial = clienteSeleccionado.RazonSocial;
                     cliente.Nif = clienteSeleccionado.Nif;
                     cliente.Email = clienteSeleccionado.Email;
                     cliente.TipoId = clienteSeleccionado.TipoCliente.TipoClienteId;
                     cliente.GrupoId = clienteSeleccionado.GrupoCliente.GrupoClienteId;
                     cliente.Calle = clienteSeleccionado.Calle;
                     cliente.MunicipioId = clienteSeleccionado.Municipio.MunicipioId;
                     // cliente.Observaciones = clienteSeleccionado.Observaciones;
                     Console.WriteLine(cliente.MunicipioId);
                     context.SaveChanges();
                 }

                  if (e.Column.DisplayIndex == 3) // 3 = Posición tipo cliente
                  {
                      //(ucContador as Contador).Actualizar();
                  }
             }*/
        }
        #endregion      


        #region Añadir Cliente
        public ICommand AnadirClienteComando => _anadirClienteComando ??
            (_anadirClienteComando = new RelayCommand(
                param => AnadirCliente()
            ));

        private async void AnadirCliente()
        {
            var formCliente = new FormCliente();

            if ((bool)await DialogHost.Show(formCliente, "RootDialog"))
            {
                var formClienteViewModel = formCliente.DataContext as FormClienteViewModel;

                Context.Clientes.Add(new Cliente()
                {
                    RazonSocial = formClienteViewModel.RazonSocial,
                    Nif = formClienteViewModel.Nif,
                    Email = formClienteViewModel.Email,
                    Calle = formClienteViewModel.Calle,
                    TipoId = formClienteViewModel.TipoClienteSeleccionado.TipoClienteId,
                    GrupoId = formClienteViewModel.GrupoClienteSeleccionado.GrupoClienteId,
                    MunicipioId = formClienteViewModel.MunicipioSeleccionado.MunicipioId,
                    Observaciones = formClienteViewModel.Observaciones
                });
                Context.SaveChanges();
                CargarClientes();
            }
        }
        #endregion


        #region Borrar Cliente    
        public ICommand BorrarClienteComando => _borrarClienteComando ??
            (_borrarClienteComando = new RelayCommandGenerico<IList<object>>(
                param => BorrarCliente(),
                param => ClienteSeleccionado != null
            ));

        private async void BorrarCliente()
        {
            string pregunta = ClientesSeleccionados.Count == 1
               ? "¿Está seguro de que desea borrar al cliente " + ClienteSeleccionado.RazonSocial + "?"
               : "¿Está seguro de que desea borrar los clientes seleccionados?";

            var resultado = (bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog");

            if (resultado)
            {
                var clientesABorrar = new List<Cliente>();
                // var clientesSeleccionados = ucTablaClientes.dgClientes.SelectedItems.Cast<Cliente>().ToList();

                foreach (var cliente in ClientesSeleccionados)
                {
                    if (!Context.PedidosCabeceras.Any(pc => pc.ClienteId == cliente.ClienteId))
                    {
                        clientesABorrar.Add(cliente);
                    }
                }
                Context.Clientes.RemoveRange(clientesABorrar);
                Context.SaveChanges();
                CargarClientes();

                if (ClientesSeleccionados.Count != clientesABorrar.Count)
                {
                    string mensaje = ClientesSeleccionados.Count == 1
                           ? "No se ha podido borrar el cliente seleccionado."
                           : "No se han podido borrar todos los clientes seleccionados.";
                    mensaje += "\n\nAsegurese de no que no exista ningún pedido asociado a dicho cliente.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Cliente
        public ICommand ModificarClienteComando => _modificarClienteComando ??
            (_modificarClienteComando = new RelayCommand(
                param => ModificarCliente(),
                param => ClienteSeleccionado != null
             ));

        private async void ModificarCliente()
        {
            var formCliente = new FormCliente(ClienteSeleccionado);
            if ((bool)await DialogHost.Show(formCliente, "RootDialog"))
            {
                var formClienteViewModel = formCliente.DataContext as FormClienteViewModel;

                ClienteSeleccionado.RazonSocial = formClienteViewModel.RazonSocial;
                ClienteSeleccionado.Nif = formClienteViewModel.Nif;
                ClienteSeleccionado.Email = formClienteViewModel.Email;
                ClienteSeleccionado.TipoId = formClienteViewModel.TipoClienteSeleccionado.TipoClienteId;
                ClienteSeleccionado.GrupoId = formClienteViewModel.GrupoClienteSeleccionado.GrupoClienteId;
                ClienteSeleccionado.MunicipioId = formClienteViewModel.MunicipioSeleccionado.MunicipioId;
                ClienteSeleccionado.Calle = formClienteViewModel.Calle;
                ClienteSeleccionado.Observaciones = formClienteViewModel.Observaciones;

                Context.SaveChanges();
                CargarClientes();
            }
        }
        #endregion


        #region Refrescar Clientes
        public ICommand RefrescarClientesComando => _refrescarClientesComando ??
            (_refrescarClientesComando = new RelayCommand(
                param => RefrescarClientes()
             ));

        private void RefrescarClientes()
        {
            // Hay que volver a instanciar un nuevo context ya que sino no se pueden refrescar los datos
            // debido a que se guardardan en una cache
            Context.Dispose();
            Context = new BiomasaEUPTContext();
            CargarClientes();
        }
        #endregion


        #region Modificar Observación Cliente
        public ICommand ModificarObservacionesClienteComando => _modificarObservacionesClienteComando ??
            (_modificarObservacionesClienteComando = new RelayCommand(
                param => ModificarObservacionesCliente(),
                param => ClienteSeleccionado != null
             ));

        private void ModificarObservacionesCliente()
        {
            /*var cliente = context.Clientes.Single(u => u.ClienteId == ClienteSeleccionado.ClienteId);

            cliente.Observaciones = ClienteSeleccionado.Observaciones;*/
            Context.SaveChanges();

            ObservacionesEnEdicion = false;
        }
        #endregion


        #region Filtro Clientes
        public ICommand FiltrarClientesComando => _filtrarClientesComando ??
           (_filtrarClientesComando = new RelayCommand(
                param => FiltrarClientes()
           ));

        public void FiltrarClientes()
        {
            ClientesView.Filter = FiltroClientes;
            ClientesView.Refresh();
        }

        private bool FiltroClientes(object item)
        {
            var cliente = item as Cliente;
            string razonSocial = cliente.RazonSocial.ToLower();
            string nif = cliente.Nif.ToLower();
            string email = cliente.Email.ToLower();
            string calle = cliente.Calle.ToLower();
            string codigoPostal = cliente.Municipio.CodigoPostal.ToLower();
            string municipio = cliente.Municipio.Nombre.ToLower();
            string tipo = cliente.TipoCliente.Nombre.ToLower();
            string grupo = cliente.GrupoCliente.Nombre.ToLower();
            var itemAceptado = true;

            var condicion = (RazonSocialSeleccionada == true ? razonSocial.Contains(TextoFiltroClientes) : false)
                || (NifSeleccionado == true ? nif.Contains(TextoFiltroClientes) : false)
                || (EmailSeleccionado == true ? email.Contains(TextoFiltroClientes) : false)
                || (CalleSeleccionada == true ? calle.Contains(TextoFiltroClientes) : false)
                || (CodigoPostalSeleccionado == true ? codigoPostal.Contains(TextoFiltroClientes) : false)
                || (MunicipioSeleccionado == true ? municipio.Contains(TextoFiltroClientes) : false);

            // Filtra Tipos Clientes
            if (FiltroTablaViewModel.TiposSeleccionados == null || FiltroTablaViewModel.TiposSeleccionados.Count == 0)
            {
                itemAceptado = condicion;
            }
            else
            {
                foreach (TipoCliente tipoCliente in FiltroTablaViewModel.TiposSeleccionados)
                {
                    if (tipoCliente.Nombre.ToLower().Equals(tipo))
                    {
                        // Si lo encuentra no hace falta que siga haciendo el foreach
                        itemAceptado = condicion;
                        break;
                    }
                    else { itemAceptado = false; }
                }
            }

            // Si el cliente ha sido aceptado por el tipo falta comprobar el grupo
            if (itemAceptado)
            {
                // Filtra Grupos Clientes
                if (FiltroTablaViewModel.GruposSeleccionados == null || FiltroTablaViewModel.GruposSeleccionados.Count == 0)
                {
                    itemAceptado = condicion;
                }
                else
                {
                    foreach (GrupoCliente grupoCliente in FiltroTablaViewModel.GruposSeleccionados)
                    {
                        if (grupoCliente.Nombre.ToLower().Equals(grupo))
                        {
                            // Si lo encuentra no hace falta que siga haciendo el foreach
                            itemAceptado = condicion;
                            break;
                        }
                        else { itemAceptado = false; }
                    }
                }
            }
            return itemAceptado;
        }
        #endregion
    }
}
