using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
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
            context = new BiomasaEUPTContext();

            ucTablaUsuarios.dgUsuarios.RowEditEnding += DgUsuarios_RowEditEnding;
            ucTablaUsuarios.dgUsuarios.CellEditEnding += DgUsuarios_CellEditEnding;
             ucFiltroTabla.lbFiltroTipo.SelectionChanged += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbNombre.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbNombre.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbEmail.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbEmail.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbBaneado.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbBaneado.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbTipo.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaUsuarios.cbTipo.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucOpciones.bAnadir.Click += BAnadirUsuario_Click;
            ucTablaUsuarios.bAnadirUsuario.Click += BAnadirUsuario_Click;
            ucOpciones.bRefrescar.Click += (s, e1) => { CargarUsuarios(); };
            ucTablaUsuarios.bRefrescar.Click += (s, e1) => { CargarUsuarios(); };

            /*   Style style = new Style(typeof(CheckBox));
               style.Setters.Add(new EventSetter(CheckBox.CheckedEvent, new RoutedEventHandler(BaneadoColumna_Checked)));
               style.Setters.Add(new EventSetter(CheckBox.UncheckedEvent, new RoutedEventHandler(BaneadoColumna_Checked)));
               ucTablaUsuarios.baneadoColumna.CellStyle = style;*/
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            usuariosViewSource = ((CollectionViewSource)(ucTablaUsuarios.FindResource("usuariosViewSource")));
            tiposUsuariosViewSource = ((CollectionViewSource)(ucTablaUsuarios.FindResource("tiposUsuariosViewSource")));
            // CargarUsuarios();
        }

        public void CargarUsuarios()
        {
            using (new CursorEspera())
            {
                // Hay que instanciar otro context para que se puedan refescar los datos
                // ya que sino no funciona el refresco al guardarse en la cache
                context.Dispose();
                context = new BiomasaEUPTContext();
                usuariosViewSource.Source = context.Usuarios.ToList();
                tiposUsuariosViewSource.Source = context.TiposUsuarios.ToList();
                usuariosViewSource.View.Refresh();
                tiposUsuariosViewSource.View.Refresh();
                (ucContador as Contador).Actualizar();
                ucTablaUsuarios.dgUsuarios.SelectedIndex = -1;
            }
        }

        private void DgUsuarios_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var usuario = e.Row.DataContext as Usuario;

                if (e.Column.DisplayIndex == 1) // 1 = Posición columna contraseña
                {
                    ContentPresenter contentPresenter = e.EditingElement as ContentPresenter;
                    DataTemplate editingTemplate = contentPresenter.ContentTemplate;
                    PasswordBox contrasena = editingTemplate.FindName("pbContrasena", contentPresenter) as PasswordBox;
                    string hashContrasena = ContrasenaHashing.obtenerHashSHA256(ContrasenaHashing.SecureStringToString(contrasena.SecurePassword));
                    usuario.Contrasena = hashContrasena;
                }
                else if (e.Column.DisplayIndex == 4) // 4 = Posición columna baneado
                {
                    Console.WriteLine(usuario.Baneado);
                }
                if (e.Column.DisplayIndex == 3) // 3 = Posición columna tipo usuario
                {
                    (ucContador as Contador).Actualizar();
                }
                context.SaveChanges();
            }
        }

        private void DgUsuarios_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                //context.SaveChanges();
                Usuario usuario = e.Row.DataContext as Usuario;
                //Console.WriteLine(usuario.Nombre + " - " + usuario.Email + " - " + usuario.Contrasena + " - " + usuario.TipoId + " - " + usuario.Baneado);
                (ucContador as Contador).Actualizar();
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

        #region FiltroTabla
        public void FiltrarTabla()
        {
            usuariosViewSource.Filter += new FilterEventHandler(FiltroTabla);
        }

        private void FiltroTabla(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaUsuarios.tbBuscar.Text.ToLower();
            var usuario = e.Item as Usuario;
            string nombre = usuario.Nombre.ToLower();
            string email = usuario.Email.ToLower();
            string tipo = usuario.TipoUsuario.Nombre.ToLower();

            var condicion = (ucTablaUsuarios.cbNombre.IsChecked == true ? nombre.Contains(textoBuscado) : false) ||
                           (ucTablaUsuarios.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                           (ucTablaUsuarios.cbBaneado.IsChecked == true ? usuario.Baneado == true : false);


            // Filtra todos
            if (ucFiltroTabla.lbFiltroTipo.SelectedItems.Count == 0)
            {
                e.Accepted = condicion;
            }
            else
            {
                foreach (TipoUsuario tipoUsuario in ucFiltroTabla.lbFiltroTipo.SelectedItems)
                {
                    if (tipoUsuario.Nombre.ToLower().Equals(tipo))
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

        private bool HayUnUsuarioSeleccionado()
        {
            if (ucTablaUsuarios.dgUsuarios.SelectedIndex != -1)
            {
                var usuarioSeleccionado = ucTablaUsuarios.dgUsuarios.SelectedItem as Usuario;
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
                context.SaveChanges();
                CargarUsuarios();
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
                usuariosViewSource.View.Refresh();
            }
        }
        #endregion

    }
}
