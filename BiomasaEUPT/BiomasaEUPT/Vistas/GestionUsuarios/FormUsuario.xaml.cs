using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
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

namespace BiomasaEUPT.Vistas.GestionUsuarios
{
    /// <summary>
    /// Lógica de interacción para FormUsuario.xaml
    /// </summary>
    public partial class FormUsuario : UserControl
    {
        private BiomasaEUPTContext context;
        private CollectionViewSource tiposUsuariosViewSource;
        public string Nombre { get; set; }
        public string Email { get; set; }
        public bool Baneado { get; set; }
        public bool ContrasenaIguales { get; set; }

        private string _contrasena;
        public string Contrasena
        {
            get { return _contrasena; }
            set
            {
                _contrasena = value;
                ContrasenaIguales = _contrasena == ContrasenaConfirmacion;
            }
        }

        private string _contrasenaConfirmacion;
        public string ContrasenaConfirmacion
        {
            get { return _contrasenaConfirmacion; }
            set
            {
                _contrasenaConfirmacion = value;
                ContrasenaIguales = Contrasena == _contrasenaConfirmacion;
            }
        }

        public FormUsuario()
        {
            InitializeComponent();
            DataContext = this;
        }

        public FormUsuario(Usuario usuario) : this()
        {
            gbTitulo.Header = "Editar Usuario";
            Nombre = usuario.Nombre;
            Email = usuario.Email;
            cbTiposUsuarios.SelectedValue = usuario.TipoId;
            Baneado = usuario.Baneado.Value;
            vNombreUnico.NombreActual = usuario.Nombre;
            vEmailUnico.NombreActual = usuario.Email;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = new BiomasaEUPTContext();
            tiposUsuariosViewSource = ((CollectionViewSource)(FindResource("tiposUsuariosViewSource")));

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

            tiposUsuariosViewSource.Source = tiposUsuarios;
        }
    }
}
