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
        public FiltroTablaViewModel FiltroTablaViewModel { get; set; }
        public ContadorViewModel<TipoUsuario> ContadorViewModel { get; set; }

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
        private ICommand _dgUsuarios_CellEditEndingComando;

        private BiomasaEUPTContext context;

        public TabUsuariosViewModel()
        {
            FiltroTablaViewModel = new FiltroTablaViewModel()
            {
                ViewModel = this
            };
        }

        public override void Inicializar()
        {
            context = new BiomasaEUPTContext();
            CargarUsuarios();
            FiltroTablaViewModel.CargarFiltro();
        }

        public void CargarUsuarios()
        {
            using (new CursorEspera())
            {
                Usuarios = new ObservableCollection<Usuario>(context.Usuarios.ToList());
                UsuariosView = (CollectionView)CollectionViewSource.GetDefaultView(Usuarios);
                TiposUsuarios = new ObservableCollection<TipoUsuario>(context.TiposUsuarios.ToList());
                ContadorViewModel.Tipos = TiposUsuarios;

                // Por defecto no está seleccionada ninguna fila del datagrid usuarios
                UsuarioSeleccionado = null;
            }
        }

        // Asigna el valor de UsuariosSeleccionados ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGUsuarios_SelectionChangedComando => new RelayCommandGenerico<IList<object>>(param => UsuariosSeleccionados = param.Cast<Usuario>().ToList());

        #region Editar Celda
        public ICommand DGUsuarios_CellEditEndingComando => _dgUsuarios_CellEditEndingComando ??
            (_dgUsuarios_CellEditEndingComando = new RelayCommandGenerico<DataGridCellEditEndingEventArgs>(
                 param => EditarCeldaUsuario(param)
            ));

        private async void EditarCeldaUsuario(DataGridCellEditEndingEventArgs e)
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

                // Si aún quedan admins activos en el sistema se procede a borrar los usuarios
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
        #endregion


        #region Modificar Usuario
        public ICommand ModificarUsuarioComando => _modificarUsuarioComando ??
            (_modificarUsuarioComando = new RelayCommand(
                param => ModificarUsuario(),
                param => UsuarioSeleccionado != null
             ));

        private async void ModificarUsuario()
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
        #endregion


        #region Refrescar Usuarios
        public ICommand RefrescarUsuariosComando => _refrescarUsuariosComando ??
            (_refrescarUsuariosComando = new RelayCommand(
                param => RefrescarUsurios()
             ));

        private void RefrescarUsurios()
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
            if (FiltroTablaViewModel.TiposSeleccionados == null || FiltroTablaViewModel.TiposSeleccionados.Count == 0)
            {
                itemAceptado = condicion;
            }
            else
            {
                foreach (TipoUsuario tipoUsuario in FiltroTablaViewModel.TiposSeleccionados)
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
