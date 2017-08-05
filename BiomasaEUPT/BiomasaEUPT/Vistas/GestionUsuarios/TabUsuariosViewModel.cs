using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.GestionUsuarios
{
    public class TabUsuariosViewModel : ViewModelBase
    {
        public ObservableCollection<Usuario> Usuarios { get; set; }
        public CollectionView UsuariosView { get; private set; }
        public ObservableCollection<TipoUsuario> TiposUsuarios { get; set; }
        public IList<Usuario> UsuariosSeleccionados { get; set; }
        public Usuario UsuarioSeleccionado { get; set; }
        public FiltroViewModel<TipoUsuario> FiltroTiposViewModel { get; set; }
        public ContadorViewModel<TipoUsuario> ContadorViewModel { get; set; }
        public OpcionesViewModel OpcionesViewModel { get; set; }

        // Checkbox Filtro Usuarios
        public bool NombreSeleccionado { get; set; } = true;
        public bool EmailSeleccionado { get; set; } = true;
        public bool BaneadoSeleccionado { get; set; } = false;

        private string _textoFiltroUsuarios = "";
        public string TextoFiltroUsuarios
        {
            get { return _textoFiltroUsuarios; }
            set
            {
                _textoFiltroUsuarios = value.ToLower();
                FiltrarUsuarios();
            }
        }

        private ICommand _anadirUsuarioComando;
        private ICommand _modificarUsuarioComando;
        private ICommand _borrarUsuarioComando;
        private ICommand _refrescarUsuariosComando;
        private ICommand _filtrarUsuariosComando;
        private ICommand _dgUsuarios_BeginningEditComando;
        private ICommand _dgUsuarios_CellEditEndingComando;

        private BiomasaEUPTContext context;

        public TabUsuariosViewModel()
        {
            FiltroTiposViewModel = new FiltroViewModel<TipoUsuario>()
            {
                FiltrarItems = FiltrarUsuarios
            };
            ContadorViewModel = new ContadorViewModel<TipoUsuario>();
            OpcionesViewModel = new OpcionesViewModel()
            {
                AnadirComando = AnadirUsuarioComando,
                BorrarComando = BorrarUsuarioComando,
                ModificarComando = ModificarUsuarioComando,
                RefrescarComando = RefrescarUsuariosComando
            };
        }

        public override void Inicializar()
        {
            context = new BiomasaEUPTContext();
            CargarUsuarios();
        }

        public void CargarUsuarios()
        {
            using (new CursorEspera())
            {
                Usuarios = new ObservableCollection<Usuario>(context.Usuarios.ToList());
                UsuariosView = (CollectionView)CollectionViewSource.GetDefaultView(Usuarios);

                var usuarioLogeado = context.Usuarios.Single(u => u.Nombre == Properties.Settings.Default.usuario);
                var tiposUsuarios = context.TiposUsuarios.ToList();

                // Si el usuario no es Super Administrador no puede seleccionar dicho tipo
                if (usuarioLogeado.TipoId != 1)
                {
                    tiposUsuarios = tiposUsuarios.Where(tu => tu.TipoUsuarioId != 1).ToList();
                }

                // Si el usuario no tiene la gestión de permisos no puede seleccionar los tipos de usuarios con dicho permiso
                if (!usuarioLogeado.TipoUsuario.Permisos.Select(p => p.Tab).Contains(Tab.Permisos))
                {
                    var tiposUsuariosConPermisos = context.TiposUsuarios.Where(tu => tu.Permisos.Any(p => p.Tab == Tab.Permisos)).ToList();
                    tiposUsuarios = tiposUsuarios.Where(tui => !tiposUsuariosConPermisos.Any(tue => tui.TipoUsuarioId == tue.TipoUsuarioId)).ToList();

                }
                TiposUsuarios = new ObservableCollection<TipoUsuario>(tiposUsuarios);

                ContadorViewModel.Tipos = TiposUsuarios;
                FiltroTiposViewModel.Items = TiposUsuarios;

                // Por defecto no está seleccionada ninguna fila del datagrid usuarios
                UsuarioSeleccionado = null;
            }
        }

        // Asigna el valor de UsuariosSeleccionados ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGUsuarios_SelectionChangedComando => new RelayCommandGenerico<IList<object>>(param => UsuariosSeleccionados = param.Cast<Usuario>().ToList());


        #region Editar Celda Beginning
        public ICommand DGUsuarios_BeginningEditComando => _dgUsuarios_BeginningEditComando ??
             (_dgUsuarios_BeginningEditComando = new RelayCommandGenerico<DataGridBeginningEditEventArgs>(
                  param => EditarCeldaBeginningUsuario(param)
             ));

        private async void EditarCeldaBeginningUsuario(DataGridBeginningEditEventArgs e)
        {

            var usuarioSeleccionado = e.Row.DataContext as Usuario;
            var usuarioLogeado = context.Usuarios.Single(u => u.Nombre == Properties.Settings.Default.usuario);

            // El usuario Super Administrador no se puede borrar
            if (usuarioSeleccionado.TipoId == 1 && usuarioLogeado.TipoId != 1)
            {
                await DialogHost.Show(new MensajeInformacion()
                {
                    Mensaje = "No se puede modificar ese usuario."
                }, "RootDialog");
                //context.Entry(usuarioSeleccionado).State = EntityState.Unchanged;
                e.Cancel = true;
            }

            // Si el usuario logeado o tiene la gestión de permisos y el usuario seleccionado sí, entonces no se puede modificar
            else if (!usuarioLogeado.TipoUsuario.Permisos.Select(p => p.Tab).Contains(Tab.Permisos)
                     && usuarioSeleccionado.TipoUsuario.Permisos.Select(p => p.Tab).Contains(Tab.Permisos))
            {
                await DialogHost.Show(new MensajeInformacion()
                {
                    Mensaje = "No se puede modificar ese usuario ya que no se tiene permisos suficientes."
                }, "RootDialog");
                //context.Entry(usuarioSeleccionado).State = EntityState.Unchanged;
                e.Cancel = true;
            }
        }
        #endregion


        #region Editar Celda Ending
        public ICommand DGUsuarios_CellEditEndingComando => _dgUsuarios_CellEditEndingComando ??
            (_dgUsuarios_CellEditEndingComando = new RelayCommandGenerico<DataGridCellEditEndingEventArgs>(
                 param => EditarCeldaEndingUsuario(param)
            ));

        private async void EditarCeldaEndingUsuario(DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var usuarioSeleccionado = e.Row.DataContext as Usuario;
                if (e.Column.DisplayIndex == 1) // 1 = Posición columna contraseña
                {
                    ContentPresenter contentPresenter = e.EditingElement as ContentPresenter;
                    DataTemplate editingTemplate = contentPresenter.ContentTemplate;
                    PasswordBox contrasena = editingTemplate.FindName("pbContrasena", contentPresenter) as PasswordBox;
                    Console.WriteLine(ContrasenaHashing.SecureStringToString(contrasena.SecurePassword));
                    string hashContrasena = ContrasenaHashing.ObtenerHashSHA256(ContrasenaHashing.SecureStringToString(contrasena.SecurePassword));
                    usuarioSeleccionado.Contrasena = hashContrasena;
                }

                // Comprueba si se va a baneado al admin que haya a menos otro admin activo
                if (usuarioSeleccionado.TipoUsuario.TipoUsuarioId == 1 && usuarioSeleccionado.Baneado == true
                    && !context.Usuarios.Any(u => u.TipoId == 1 && u.Baneado != false && u.UsuarioId != usuarioSeleccionado.UsuarioId))
                {
                    usuarioSeleccionado.Baneado = false;
                    await DialogHost.Show(new MensajeInformacion()
                    {
                        Mensaje = "No se puede banear el usuario ya que al menos tiene que haber un admin activo."
                    }, "RootDialog");
                }

                context.SaveChanges();

                if (e.Column.DisplayIndex == 3) // 3 = Posición columna tipo usuario
                {
                    ContadorViewModel.Tipos = new ObservableCollection<TipoUsuario>(context.TiposUsuarios.ToList());
                }
            }
        }
        #endregion


        #region Añadir Usuario
        public ICommand AnadirUsuarioComando => _anadirUsuarioComando ??
            (_anadirUsuarioComando = new RelayCommand(
                param => AnadirUsuario()
            ));

        private async void AnadirUsuario()
        {
            var formUsuario = new FormUsuario();

            if ((bool)await DialogHost.Show(formUsuario, "RootDialog"))
            {
                string hashContrasena = ContrasenaHashing.ObtenerHashSHA256(formUsuario.Contrasena);

                context.Usuarios.Add(new Usuario()
                {
                    Nombre = formUsuario.Nombre,
                    Email = formUsuario.Email,
                    TipoId = (formUsuario.cbTiposUsuarios.SelectedItem as TipoUsuario).TipoUsuarioId,
                    Contrasena = hashContrasena,
                    Baneado = formUsuario.Baneado
                });
                context.SaveChanges();
                CargarUsuarios();
            }
        }
        #endregion


        #region Borrar Usuario    
        public ICommand BorrarUsuarioComando => _borrarUsuarioComando ??
            (_borrarUsuarioComando = new RelayCommandGenerico<IList<object>>(
                param => BorrarUsuario(),
                param => UsuarioSeleccionado != null
            ));

        private async void BorrarUsuario()
        {
            var usuarioLogeado = context.Usuarios.Single(u => u.Nombre == Properties.Settings.Default.usuario);

            // El usuario Super Administrador no se puede borrar
            if (UsuarioSeleccionado.TipoId == 1 && usuarioLogeado.TipoId != 1)
            {
                await DialogHost.Show(new MensajeInformacion()
                {
                    Mensaje = "No se puede borrar ese usuario."
                }, "RootDialog");
            }

            // Si el usuario logeado o tiene la gestión de permisos y el usuario seleccionado sí, entonces no se puede borrar
            else if (!usuarioLogeado.TipoUsuario.Permisos.Select(p => p.Tab).Contains(Tab.Permisos)
                     && UsuarioSeleccionado.TipoUsuario.Permisos.Select(p => p.Tab).Contains(Tab.Permisos))
            {
                await DialogHost.Show(new MensajeInformacion()
                {
                    Mensaje = "No se puede borrar ese usuario ya que no se tiene permisos suficientes."
                }, "RootDialog");
            }

            // Se procede a borrar
            else
            {
                string pregunta = UsuariosSeleccionados.Count == 1
                    ? "¿Está seguro de que desea borrar al usuario " + UsuarioSeleccionado.Nombre + "?"
                    : "¿Está seguro de que desea borrar los usuarios seleccionados?";

                var resultado = (bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog");

                if (resultado)
                {
                    var usuariosABorrar = new List<Usuario>();
                    var adminsABorrar = new List<Usuario>();
                    var noAdminsABorrar = new List<Usuario>();
                    foreach (var usuario in UsuariosSeleccionados)
                    {
                        if (usuario.TipoId == 1) { adminsABorrar.Add(usuario); }
                        else { noAdminsABorrar.Add(usuario); }
                    }

                    // Si aún quedan supearadmins activos en el sistema se procede a borrar los usuarios
                    if (context.Usuarios.Where(u => u.TipoId == 1 && u.Baneado != false).ToList().Except(adminsABorrar).Any())
                    {
                        context.Usuarios.RemoveRange(usuariosABorrar);
                        context.SaveChanges();
                    }
                    else
                    {
                        context.Usuarios.RemoveRange(noAdminsABorrar);
                        if (adminsABorrar.Any())
                        {
                            string mensaje = adminsABorrar.Count == 1
                             ? "No se ha podido borrar el administrador seleccionado."
                             : "No se han podido borrar los administradores seleccionados.";
                            mensaje += "\n\nDebe haber en el sistema al menos un admin activo.";
                            await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                        }
                        context.SaveChanges();
                    }
                    CargarUsuarios();
                }
            }
        }
        #endregion


        #region Modificar Usuario
        public ICommand ModificarUsuarioComando => _modificarUsuarioComando ??
            (_modificarUsuarioComando = new RelayCommand(
                param => ModificarUsuario(),
                param => UsuarioSeleccionado != null
             ));

        private async void ModificarUsuario()
        {
            var usuarioLogeado = context.Usuarios.Single(u => u.Nombre == Properties.Settings.Default.usuario);

            // El usuario Super Administrador no se puede borrar
            if (UsuarioSeleccionado.TipoId == 1 && usuarioLogeado.TipoId != 1)
            {
                await DialogHost.Show(new MensajeInformacion()
                {
                    Mensaje = "No se puede modificar ese usuario."
                }, "RootDialog");
            }

            // Si el usuario logeado o tiene la gestión de permisos y el usuario seleccionado sí, entonces no se puede modificar
            else if (!usuarioLogeado.TipoUsuario.Permisos.Select(p => p.Tab).Contains(Tab.Permisos)
                     && UsuarioSeleccionado.TipoUsuario.Permisos.Select(p => p.Tab).Contains(Tab.Permisos))
            {
                await DialogHost.Show(new MensajeInformacion()
                {
                    Mensaje = "No se puede modificar ese usuario ya que no se tiene permisos suficientes."
                }, "RootDialog");
            }

            // Se procede a modificar
            else
            {
                var formUsuario = new FormUsuario(UsuarioSeleccionado);
                if ((bool)await DialogHost.Show(formUsuario, "RootDialog"))
                {
                    string hashContrasena = ContrasenaHashing.ObtenerHashSHA256(formUsuario.Contrasena);
                    UsuarioSeleccionado.Nombre = formUsuario.Nombre;
                    UsuarioSeleccionado.Email = formUsuario.Email;
                    UsuarioSeleccionado.TipoId = (formUsuario.cbTiposUsuarios.SelectedItem as TipoUsuario).TipoUsuarioId;
                    UsuarioSeleccionado.Contrasena = hashContrasena;
                    UsuarioSeleccionado.Baneado = formUsuario.Baneado;
                    context.SaveChanges();
                }
            }
        }
        #endregion


        #region Refrescar Usuarios
        public ICommand RefrescarUsuariosComando => _refrescarUsuariosComando ??
            (_refrescarUsuariosComando = new RelayCommand(
                param => RefrescarUsuarios()
             ));

        private void RefrescarUsuarios()
        {
            // Hay que volver a instanciar un nuevo context ya que sino no se pueden refrescar los datos
            // debido a que se guardardan en una cache
            context.Dispose();
            context = new BiomasaEUPTContext();
            CargarUsuarios();
        }
        #endregion


        #region Filtro Usuarios
        public ICommand FiltrarUsuariosComando => _filtrarUsuariosComando ??
           (_filtrarUsuariosComando = new RelayCommand(
                param => FiltrarUsuarios()
           ));

        public void FiltrarUsuarios()
        {
            UsuariosView.Filter = FiltroUsuarios;
            UsuariosView.Refresh();
        }

        private bool FiltroUsuarios(object item)
        {
            var usuario = item as Usuario;
            string nombre = usuario.Nombre.ToLower();
            string email = usuario.Email.ToLower();
            string tipo = usuario.TipoUsuario.Nombre.ToLower();
            var itemAceptado = true;

            var condicion = (NombreSeleccionado == true ? nombre.Contains(TextoFiltroUsuarios) : false)
                || (EmailSeleccionado == true ? email.Contains(TextoFiltroUsuarios) : false)
                || (BaneadoSeleccionado == true ? usuario.Baneado == true : false);

            // Filtra todos
            if (FiltroTiposViewModel.ItemsSeleccionados == null || FiltroTiposViewModel.ItemsSeleccionados.Count == 0)
            {
                itemAceptado = condicion;
            }
            else
            {
                foreach (TipoUsuario tipoUsuario in FiltroTiposViewModel.ItemsSeleccionados)
                {
                    if (tipoUsuario.Nombre.ToLower().Equals(tipo))
                    {
                        // Si lo encuentra no hace falta que siga haciendo el foreach
                        itemAceptado = condicion;
                        break;
                    }
                    else
                    {
                        itemAceptado = false;
                    }
                }
            }
            return itemAceptado;
        }
        #endregion
    }
}
