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
        public bool ObservacionesEnEdicion { get; set; }
        public FiltroViewModel<TipoCliente> FiltroTiposViewModel { get; set; }
        public FiltroViewModel<GrupoCliente> FiltroGruposViewModel { get; set; }
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

        private ICommand _anadirTipoComando;
        private ICommand _modificarTipoComando;
        private ICommand _borrarTipoComando;
        private ICommand _anadirGrupoComando;
        private ICommand _modificarGrupoComando;
        private ICommand _borrarGrupoComando;

        public BiomasaEUPTContext Context { get; set; }


        public TabClientesViewModel()
        {
            FiltroTiposViewModel = new FiltroViewModel<TipoCliente>()
            {
                FiltrarItems = FiltrarClientes,
                AnadirComando = AnadirTipoComando,
                ModificarComando = ModificarTipoComando,
                BorrarComando = BorrarTipoComando
            };
            FiltroGruposViewModel = new FiltroViewModel<GrupoCliente>()
            {
                Titulo = "Filtro Grupo",
                FiltrarItems = FiltrarClientes,
                AnadirComando = AnadirGrupoComando,
                ModificarComando = ModificarGrupoComando,
                BorrarComando = BorrarGrupoComando
            };
            ContadorViewModel = new ContadorViewModel<TipoCliente>();
        }


        public override void Inicializar()
        {
            Context = new BiomasaEUPTContext();
            CargarClientes();
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
                FiltroTiposViewModel.Items = TiposClientes;
                FiltroGruposViewModel.Items = GruposClientes;

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
            // debido a que se guardan en una cache
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
            if (FiltroTiposViewModel.ItemsSeleccionados == null || FiltroTiposViewModel.ItemsSeleccionados.Count == 0)
            {
                itemAceptado = condicion;
            }
            else
            {
                foreach (TipoCliente tipoCliente in FiltroTiposViewModel.ItemsSeleccionados)
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
                if (FiltroGruposViewModel.ItemsSeleccionados == null || FiltroGruposViewModel.ItemsSeleccionados.Count == 0)
                {
                    itemAceptado = condicion;
                }
                else
                {
                    foreach (GrupoCliente grupoCliente in FiltroGruposViewModel.ItemsSeleccionados)
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



        #region Añadir Tipo
        public ICommand AnadirTipoComando => _anadirTipoComando ??
           (_anadirTipoComando = new RelayCommand(
               param => AnadirTipo()
           ));

        private async void AnadirTipo()
        {
            var formTipo = new FormTipo();
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "TipoCliente";

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                Context.TiposClientes.Add(new TipoCliente()
                {
                    Nombre = formTipo.Nombre,
                    Descripcion = formTipo.Descripcion
                });
                Context.SaveChanges();
                CargarClientes();
            }
        }
        #endregion


        #region Añadir Grupo
        public ICommand AnadirGrupoComando => _anadirGrupoComando ??
           (_anadirGrupoComando = new RelayCommand(
               param => AnadirGrupo()
           ));

        private async void AnadirGrupo()
        {
            var formGrupo = new FormTipo("Nuevo Grupo");
            formGrupo.vNombreUnico.Atributo = "Nombre";
            formGrupo.vNombreUnico.Tipo = "GrupoCliente";

            if ((bool)await DialogHost.Show(formGrupo, "RootDialog"))
            {
                Context.GruposClientes.Add(new GrupoCliente()
                {
                    Nombre = formGrupo.Nombre,
                    Descripcion = formGrupo.Descripcion
                });
                Context.SaveChanges();
                CargarClientes();
            }
        }
        #endregion


        #region Borrar Tipo
        public ICommand BorrarTipoComando => _borrarTipoComando ??
          (_borrarTipoComando = new RelayCommand(
              param => BorrarTipo(),
              param => FiltroTiposViewModel.ItemSeleccionado != null
          ));

        private async void BorrarTipo()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el tipo " + FiltroTiposViewModel.ItemSeleccionado.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!Context.Clientes.Any(t => t.TipoId == FiltroTiposViewModel.ItemSeleccionado.TipoClienteId))
                {
                    Context.TiposClientes.Remove(FiltroTiposViewModel.ItemSeleccionado);
                    Context.SaveChanges();
                    CargarClientes();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el tipo debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Tipo
        public ICommand ModificarTipoComando
        {
            get
            {
                if (_modificarTipoComando == null)
                {
                    _modificarTipoComando = new RelayCommand(
                        param => ModificarTipo(),
                        param => FiltroTiposViewModel.ItemSeleccionado != null
                    );
                }
                return _modificarTipoComando;
            }
        }

        private async void ModificarTipo()
        {
            var formTipo = new FormTipo("Editar Tipo");
            formTipo.Nombre = FiltroTiposViewModel.ItemSeleccionado.Nombre;
            formTipo.Descripcion = FiltroTiposViewModel.ItemSeleccionado.Descripcion;
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "TipoCliente";
            formTipo.vNombreUnico.NombreActual = FiltroTiposViewModel.ItemSeleccionado.Nombre;
            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                FiltroTiposViewModel.ItemSeleccionado.Nombre = formTipo.Nombre;
                FiltroTiposViewModel.ItemSeleccionado.Descripcion = formTipo.Descripcion;
                Context.SaveChanges();
                CargarClientes();
            }
        }
        #endregion


        #region Borrar Grupo
        public ICommand BorrarGrupoComando
        {
            get
            {
                if (_borrarGrupoComando == null)
                {
                    _borrarGrupoComando = new RelayCommand(
                        param => BorrarGrupo(),
                        param => FiltroGruposViewModel.ItemSeleccionado != null
                    );
                }
                return _borrarGrupoComando;
            }
        }

        private async void BorrarGrupo()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el grupo " + FiltroGruposViewModel.ItemSeleccionado.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!Context.GruposClientes.Any(gc => gc.GrupoClienteId == FiltroGruposViewModel.ItemSeleccionado.GrupoClienteId))
                {
                    Context.GruposClientes.Remove(FiltroGruposViewModel.ItemSeleccionado);
                    Context.SaveChanges();
                    CargarClientes();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el grupo debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Grupo
        public ICommand ModificarGrupoComando
        {
            get
            {
                if (_modificarGrupoComando == null)
                {
                    _modificarGrupoComando = new RelayCommand(
                        param => ModificarGrupo(),
                        param => FiltroGruposViewModel.ItemSeleccionado != null
                    );
                }
                return _modificarGrupoComando;
            }
        }

        private async void ModificarGrupo()
        {
            var formGrupo = new FormTipo("Editar Grupo");
            formGrupo.Nombre = FiltroGruposViewModel.ItemSeleccionado.Nombre;
            formGrupo.Descripcion = FiltroGruposViewModel.ItemSeleccionado.Descripcion;
            formGrupo.vNombreUnico.Atributo = "Nombre";
            formGrupo.vNombreUnico.Tipo = "GrupoCliente";
            formGrupo.vNombreUnico.NombreActual = FiltroGruposViewModel.ItemSeleccionado.Nombre;
            if ((bool)await DialogHost.Show(formGrupo, "RootDialog"))
            {
                FiltroGruposViewModel.ItemSeleccionado.Nombre = formGrupo.Nombre;
                FiltroGruposViewModel.ItemSeleccionado.Descripcion = formGrupo.Descripcion;

                FiltroGruposViewModel.ItemSeleccionado.Nombre = formGrupo.Nombre;
                FiltroGruposViewModel.ItemSeleccionado.Descripcion = formGrupo.Descripcion;
                Context.SaveChanges();
                CargarClientes();
            }
        }
        #endregion

    }
}
