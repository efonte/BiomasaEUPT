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

        // Checkbox Filtro Usuarios
        public bool NombreSeleccionado { get; set; } = true;
        public bool EmailSeleccionado { get; set; } = true;
        public bool BaneadoSeleccionado { get; set; } = false;

        private string _textoFiltroUsuarios;
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

        public TabUsuariosViewModel()
        {
            FiltroTablaViewModel = new FiltroTablaViewModel()
            {
                ViewModel = this
            };
        }

        public override void Inicializar()
        {
            CargarUsuarios();
            FiltroTablaViewModel.CargarFiltro();
        }

        public void CargarUsuarios()
        {
            using (var context = new BiomasaEUPTContext())
            {
                Usuarios = new ObservableCollection<Usuario>(context.Usuarios.ToList());
                UsuariosView = (CollectionView)CollectionViewSource.GetDefaultView(Usuarios);
                // UsuariosView.Filter = OnFilterMovie;
                TiposUsuarios = new ObservableCollection<TipoUsuario>(context.TiposUsuarios.ToList());
                //(ucContador as Contador).Actualizar();
                UsuarioSeleccionado = null;
            }
        }

        // Asigna el valor de UsuariosSeleccionados ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGUsuarios_SelectionChangedComando => new RelayCommand2<IList<object>>(param => UsuariosSeleccionados = param.Cast<Usuario>().ToList());

        #region Editar Celda
        public ICommand DGUsuarios_CellEditEndingComando => _dgUsuarios_CellEditEndingComando ??
            (_dgUsuarios_CellEditEndingComando = new RelayCommand2<DataGridCellEditEndingEventArgs>(
                 param => EditarCeldaUsuario(param)
            ));

        private void EditarCeldaUsuario(DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var usuarioSeleccionado = e.Row.DataContext as Usuario;

                if (e.Column.DisplayIndex == 1) // 1 = Posición columna contraseña
                {
                    ContentPresenter contentPresenter = e.EditingElement as ContentPresenter;
                    DataTemplate editingTemplate = contentPresenter.ContentTemplate;
                    PasswordBox contrasena = editingTemplate.FindName("pbContrasena", contentPresenter) as PasswordBox;
                    string hashContrasena = ContrasenaHashing.obtenerHashSHA256(ContrasenaHashing.SecureStringToString(contrasena.SecurePassword));
                    usuarioSeleccionado.Contrasena = hashContrasena;
                }
                else if (e.Column.DisplayIndex == 4) // 4 = Posición columna baneado
                {
                    Console.WriteLine(usuarioSeleccionado.Baneado);
                }
                if (e.Column.DisplayIndex == 3) // 3 = Posición columna tipo usuario
                {
                    //   (ucContador as Contador).Actualizar();
                }
                using (var context = new BiomasaEUPTContext())
                {
                    Console.WriteLine(usuarioSeleccionado.Nombre);
                    var usuario = context.Usuarios.Single(u => u.UsuarioId == usuarioSeleccionado.UsuarioId);

                    usuario.Nombre = usuarioSeleccionado.Nombre;
                    usuario.Contrasena = usuarioSeleccionado.Contrasena;
                    usuario.TipoId = usuarioSeleccionado.TipoUsuario.TipoUsuarioId;
                    usuario.Email = usuarioSeleccionado.Email;
                    usuario.Baneado = usuarioSeleccionado.Baneado;
                    //usuario = usuarioSeleccionado;
                    // context.Usuarios.Attach(usuarioSeleccionado);
                    context.SaveChanges();
                }
            }
        }
        #endregion

        #region Añadir Usuario
        public ICommand AnadirUsuarioComando => _anadirUsuarioComando ??
            (_anadirUsuarioComando = new RelayComando(
                param => AnadirUsuario()
            ));

        private async void AnadirUsuario()
        {
            var formUsuario = new FormUsuario();

            if ((bool)await DialogHost.Show(formUsuario, "RootDialog"))
            {
                string hashContrasena = ContrasenaHashing.obtenerHashSHA256(formUsuario.Contrasena);
                using (var context = new BiomasaEUPTContext())
                {
                    context.Usuarios.Add(new Usuario()
                    {
                        Nombre = formUsuario.Nombre,
                        Email = formUsuario.Email,
                        Contrasena = hashContrasena,
                        Baneado = formUsuario.Baneado
                    });
                    context.SaveChanges();
                }
            }
        }
        #endregion

        #region Borrar Usuario    
        public ICommand BorrarUsuarioComando => _borrarUsuarioComando ??
            (_borrarUsuarioComando = new RelayCommand2<IList<object>>(
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
                using (var context = new BiomasaEUPTContext())
                {
                    context.Usuarios.RemoveRange(UsuariosSeleccionados);
                    context.SaveChanges();
                }
                CargarUsuarios();
            }
        }
        #endregion

        #region Modificar Usuario
        public ICommand ModificarUsuarioComando => _modificarUsuarioComando ??
            (_modificarUsuarioComando = new RelayComando(
                param => ModificarUsuario(),
                param => UsuarioSeleccionado != null
             ));

        private async void ModificarUsuario()
        {
            var formUsuario = new FormUsuario(UsuarioSeleccionado);
            if ((bool)await DialogHost.Show(formUsuario, "RootDialog"))
            {
                string hashContrasena = ContrasenaHashing.obtenerHashSHA256(formUsuario.Contrasena);
                UsuarioSeleccionado.Nombre = formUsuario.Nombre;
                UsuarioSeleccionado.Email = formUsuario.Email;
                UsuarioSeleccionado.TipoId = (formUsuario.cbTiposUsuarios.SelectedItem as TipoUsuario).TipoUsuarioId;
                UsuarioSeleccionado.Contrasena = hashContrasena;
                UsuarioSeleccionado.Baneado = formUsuario.Baneado;

                using (var context = new BiomasaEUPTContext())
                {
                    context.SaveChanges();
                }

            }
        }
        #endregion


        #region Refrescar Usuarios
        public ICommand RefrescarUsuariosComando => _refrescarUsuariosComando ??
            (_refrescarUsuariosComando = new RelayComando(
                param => CargarUsuarios()
             ));
        #endregion


        #region Filtro Usuarios
        public ICommand FiltrarUsuariosComando => _filtrarUsuariosComando ??
           (_filtrarUsuariosComando = new RelayComando(
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
