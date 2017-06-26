using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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

namespace BiomasaEUPT.Vistas.GestionUsuarios
{
    /// <summary>
    /// Lógica de interacción para TabUsuarios.xaml
    /// </summary>
    public partial class TabUsuarios : UserControl
    {
        private BiomasaEUPTContext context;
        private CollectionViewSource usuariosViewSource;
        private CollectionViewSource tiposUsuariosViewSource;


        public TabUsuarios()
        {
            InitializeComponent();
            DataContext = this;
            ucTablaUsuarios.dgUsuarios.RowEditEnding += DgUsuarios_RowEditEnding;
            ucTablaUsuarios.dgUsuarios.CellEditEnding += DgUsuarios_CellEditEnding;
            ucTablaUsuarios.cbNombre.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbNombre.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbEmail.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbEmail.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbBaneado.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbBaneado.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbTipo.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbTipo.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucOpcionesUsuarios.bAnadir.Click += BAnadirUsuario_Click;
            ucTablaUsuarios.bAnadirUsuario.Click += BAnadirUsuario_Click;
            ucOpcionesUsuarios.bRefrescar.Click += (s, e1) => { CargarUsuarios(); };
            ucTablaUsuarios.bRefrescar.Click += (s, e1) => { CargarUsuarios(); };

            /*   Style style = new Style(typeof(CheckBox));
               style.Setters.Add(new EventSetter(CheckBox.CheckedEvent, new RoutedEventHandler(BaneadoColumna_Checked)));
               style.Setters.Add(new EventSetter(CheckBox.UncheckedEvent, new RoutedEventHandler(BaneadoColumna_Checked)));
               ucTablaUsuarios.baneadoColumna.CellStyle = style;*/
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = new BiomasaEUPTContext();
            usuariosViewSource = ((CollectionViewSource)(ucTablaUsuarios.FindResource("usuariosViewSource")));
            tiposUsuariosViewSource = ((CollectionViewSource)(ucTablaUsuarios.FindResource("tiposUsuariosViewSource")));
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            using (new CursorEspera())
            {
                context.Usuarios.Load();
                context.TiposUsuarios.Load();
                usuariosViewSource.Source = context.Usuarios.Local;
                tiposUsuariosViewSource.Source = context.TiposUsuarios.Local;
                usuariosViewSource.View.Refresh();
                tiposUsuariosViewSource.View.Refresh();
                ActualizarContador();
                ucTablaUsuarios.dgUsuarios.SelectedIndex = -1;
            }
        }

        private async void DgUsuarios_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Usuario usuario = e.Row.DataContext as Usuario;

                if (e.Column.DisplayIndex == 1) // 1 = Posición columna contraseña
                {
                    ContentPresenter contentPresenter = e.EditingElement as ContentPresenter;
                    DataTemplate editingTemplate = contentPresenter.ContentTemplate;
                    PasswordBox contrasena = editingTemplate.FindName("pbContrasena", contentPresenter) as PasswordBox;
                    string hashContrasena = ContrasenaHashing.obtenerHashSHA256(ContrasenaHashing.SecureStringToString(contrasena.SecurePassword));
                    usuario.Contrasena = hashContrasena;
                }
                if (e.Column.DisplayIndex == 4) // 4 = Posición columna baneado
                {
                    Console.WriteLine(usuario.Baneado);
                }
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException e1)
                {
                    var mensajeError = "No se ha podido modificar el campo.";
                    foreach (var entry in e1.Entries)
                    {
                        if (entry.Entity is Usuario)
                        {
                            if (entry.State == EntityState.Modified)
                            {
                                mensajeError = "No se ha podido modificar el usuario.\n\nAsegurese que el nombre de usuario y el email son únicos";
                                break;
                            }
                        }

                    }
                    var mensaje = new MensajeInformacion(mensajeError) { Width = 350 };
                    var resultado = await DialogHost.Show(mensaje, "RootDialog");
                }
            }
        }

        private void DgUsuarios_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Usuario usuario = e.Row.DataContext as Usuario;
                Console.WriteLine(usuario.Nombre + " - " + usuario.Email + " - " + usuario.Contrasena + " - " + usuario.TipoId + " - " + usuario.Baneado);
            }
        }

        private async void BAnadirUsuario_Click(object sender, RoutedEventArgs e)
        {
            var formUsuario = new FormUsuario();

            if ((bool)await DialogHost.Show(formUsuario, "RootDialog"))
            {
                string hashContrasena = ContrasenaHashing.obtenerHashSHA256(formUsuario.Contrasena);

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

        #region Filtro
        public void FiltrarTabla()
        {
            usuariosViewSource.Filter += new FilterEventHandler(FiltroTabla);
        }

        private void FiltroTabla(object sender, FilterEventArgs e)
        {
            ListBoxItem tbTiposUsuarios = (ListBoxItem)ucTiposUsuarios.lbTiposUsuarios.SelectedItem;

            string tipoFiltrado = tbTiposUsuarios.Content.ToString().ToLower();
            string textoBuscado = ucTablaUsuarios.tbBuscar.Text.ToLower();

            var usuario = e.Item as Usuario;
            string nombre = usuario.Nombre.ToLower();
            string email = usuario.Email.ToLower();
            string tipo = usuario.TipoUsuario.Nombre.ToLower();

            var condicion = (ucTablaUsuarios.cbNombre.IsChecked == true ? nombre.Contains(textoBuscado) : false) ||
                           (ucTablaUsuarios.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                           (ucTablaUsuarios.cbBaneado.IsChecked == true ? usuario.Baneado == true : false);

            if (nombre.Contains(textoBuscado) || email.Contains(textoBuscado))
            {
                if (!tipoFiltrado.Equals("todos"))
                {
                    e.Accepted = tipo.Equals(tipoFiltrado) && condicion;
                }
                else
                {
                    e.Accepted = condicion;
                }
            }
            else
                e.Accepted = false;
        }
        #endregion

        public void ActualizarContador()
        {
            ucInfoUsuarios.tbNumAdministradores.Text = context.Usuarios.Local.Where(u => u.TipoId == 1).Count().ToString();
            ucInfoUsuarios.tbNumTecnicosA.Text = context.Usuarios.Local.Where(u => u.TipoId == 2).Count().ToString();
            ucInfoUsuarios.tbNumTecnicosB.Text = context.Usuarios.Local.Where(u => u.TipoId == 3).Count().ToString();
            ucInfoUsuarios.tbNumTecnicosC.Text = context.Usuarios.Local.Where(u => u.TipoId == 4).Count().ToString();
        }

        private bool HayUnUsuarioSeleccionado()
        {
            if (ucTablaUsuarios.dgUsuarios.SelectedIndex != -1)
            {
                Usuario usuarioSeleccionado = ucTablaUsuarios.dgUsuarios.SelectedItem as Usuario;
                //Console.WriteLine(usuarioSeleccionado.nombre);
                return usuarioSeleccionado != null;
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
                        param => BorrarUsuario(),
                        param => HayUnUsuarioSeleccionado()
                    );
                }
                return _borrarComando;
            }
        }

        private async void BorrarUsuario()
        {
            string pregunta = ucTablaUsuarios.dgUsuarios.SelectedItems.Count == 1
                ? "¿Está seguro de que desea borrar al usuario " + (ucTablaUsuarios.dgUsuarios.SelectedItem as Usuario).Nombre + "?"
                : "¿Está seguro de que desea borrar los usuarios seleccionados?";

            var mensaje = new MensajeConfirmacion(pregunta);
            mensaje.MaxHeight = ActualHeight;
            mensaje.MaxWidth = ActualWidth;

            var resultado = (bool)await DialogHost.Show(mensaje, "RootDialog");

            if (resultado)
            {
                context.Usuarios.RemoveRange(ucTablaUsuarios.dgUsuarios.SelectedItems.Cast<Usuario>().ToList());
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
                        param => ModificarUsuario(),
                        param => HayUnUsuarioSeleccionado()
                    );
                }
                return _modificarComando;
            }
        }

        private async void ModificarUsuario()
        {
            var usuarioSeleccionado = ucTablaUsuarios.dgUsuarios.SelectedItem as Usuario;
            var formUsuario = new FormUsuario(usuarioSeleccionado);
            if ((bool)await DialogHost.Show(formUsuario, "RootDialog"))
            {
                string hashContrasena = ContrasenaHashing.obtenerHashSHA256(formUsuario.Contrasena);
                usuarioSeleccionado.Nombre = formUsuario.Nombre;
                usuarioSeleccionado.Email = formUsuario.Email;
                usuarioSeleccionado.TipoId = (formUsuario.cbTiposUsuarios.SelectedItem as TipoUsuario).TipoUsuarioId;
                usuarioSeleccionado.Contrasena = hashContrasena;
                usuarioSeleccionado.Baneado = formUsuario.Baneado;

                context.SaveChanges();
                //usuariosViewSource.View.Refresh();
            }
        }
        #endregion

    }
}
