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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = BaseDeDatos.Instancia.biomasaEUPTContext;
                usuariosViewSource = ((CollectionViewSource)(ucTablaUsuarios.FindResource("usuariosViewSource")));
                tiposUsuariosViewSource = ((CollectionViewSource)(ucTablaUsuarios.FindResource("tipos_usuariosViewSource")));
                context.Usuarios.Load();
                context.TiposUsuarios.Load();
                usuariosViewSource.Source = context.Usuarios.Local;
                tiposUsuariosViewSource.Source = context.TiposUsuarios.Local;
                //var result = from u in context.usuarios select u;
                //ucTablaUsuarios.dgUsuarios.ItemsSource = result.ToList();
                ActualizarContador();
            }
            ucTablaUsuarios.dgUsuarios.RowEditEnding += DgUsuarios_RowEditEnding;
            ucTablaUsuarios.dgUsuarios.CellEditEnding += DgUsuarios_CellEditEnding;
        }

        private void DgUsuarios_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (e.Column.DisplayIndex == 1) // 1 = Posición columna contraseña
                {
                    Usuario usuario = e.Row.DataContext as Usuario;
                    ContentPresenter contentPresenter = e.EditingElement as ContentPresenter;
                    DataTemplate editingTemplate = contentPresenter.ContentTemplate;
                    PasswordBox contrasena = editingTemplate.FindName("pbContrasena", contentPresenter) as PasswordBox;
                    string hashContrasena = ContrasenaHashing.obtenerHashSHA256(ContrasenaHashing.SecureStringToString(contrasena.SecurePassword));
                    usuario.Contrasena = hashContrasena;
                }
            }
        }

        private void DgUsuarios_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                Usuario usuario = e.Row.DataContext as Usuario;
                Console.WriteLine(usuario.Nombre + " - " + usuario.Email + " - " + usuario.Contrasena + " - " + usuario.TipoId + " - " + usuario.Baneado);
                /*var usuarioExistente = context.usuarios.Where(u => u.id_usuario == usuario.id_usuario).FirstOrDefault();
                if (usuarioExistente == null)
                {
                    Usuario nuevoUsuario = new Usuario()
                    {
                        nombre = usuario.nombre,
                        email = usuario.email,
                        tipo_id = usuario.tipo_id,
                        contrasena = usuario.contrasena,
                        baneado = false
                    };
                    Console.WriteLine(nuevoUsuario.nombre + " - " + nuevoUsuario.email + " - " + nuevoUsuario.contrasena + " - " + nuevoUsuario.tipo_id + " - " + nuevoUsuario.baneado);
                    context.usuarios.Add(nuevoUsuario);
                }
                else
                {
                    usuarioExistente.nombre = usuario.nombre;
                    usuarioExistente.email = usuario.email;
                    usuarioExistente.tipo_id = usuario.tipo_id;
                    usuarioExistente.contrasena = usuario.contrasena;
                    usuarioExistente.baneado = usuario.baneado;
                }
                context.SaveChanges();*/
            }

        }

        public void FiltrarTabla()
        {
            usuariosViewSource.Filter += new FilterEventHandler(FiltroTabla);
            //ICollectionView ItemList = usuariosViewSource.View;
            //ucTablaUsuarios.dgUsuarios.ItemsSource = ItemList;
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

            if (nombre.Contains(textoBuscado) || email.Contains(textoBuscado))
            {
                if (!tipoFiltrado.Equals("todos"))
                {
                    e.Accepted = tipo.Equals(tipoFiltrado);
                }
                else
                {
                    e.Accepted = true;
                }
            }
            else
                e.Accepted = false;
        }


        public void ActualizarContador()
        {
            ucInfoUsuarios.tbNumAdministradores.Text = context.Usuarios.Local.Where(u => u.TipoId == 1).Count().ToString();
            ucInfoUsuarios.tbNumTecnicosA.Text = context.Usuarios.Local.Where(u => u.TipoId == 2).Count().ToString();
            ucInfoUsuarios.tbNumTecnicosB.Text = context.Usuarios.Local.Where(u => u.TipoId == 3).Count().ToString();
            ucInfoUsuarios.tbNumTecnicosC.Text = context.Usuarios.Local.Where(u => u.TipoId == 4).Count().ToString();
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
            return context != null && context.HayCambios<Usuario>();
        }

        private void ConfirmarCambios()
        {
            /*try
            {*/
            context.GuardarCambios<Usuario>();
            //usuariosViewSource.View.Refresh();
            ActualizarContador();
            /*}
            catch (DbEntityValidationException ex)
            {
                var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                Console.WriteLine(error.ErrorMessage);
                //this.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }*/
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
            if (ucTablaUsuarios.dgUsuarios.SelectedIndex != -1)
            {
                Usuario usuarioSeleccionado = ucTablaUsuarios.dgUsuarios.SelectedItem as Usuario;
                //Console.WriteLine(usuarioSeleccionado.nombre);
                return usuarioSeleccionado != null;
            }
            return false;
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

    }
}
