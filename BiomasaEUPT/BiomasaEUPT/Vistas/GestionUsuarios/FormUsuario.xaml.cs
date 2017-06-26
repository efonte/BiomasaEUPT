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
        public string Contrasena { get; set; }
        public string ContrasenaConfirmacion { get; set; }

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
            context.TiposUsuarios.Load();
            tiposUsuariosViewSource.Source = context.TiposUsuarios.Local;
        }
    }
}
