using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private FormUsuarioViewModel viewModel;

        public FormUsuario()
        {
            InitializeComponent();
            viewModel = new FormUsuarioViewModel();
            DataContext = viewModel;
        }

        public FormUsuario(Usuario usuario) : this()
        {
            viewModel.FormTitulo = "Editar Usuario";
            viewModel.Nombre = usuario.Nombre;
            viewModel.Email = usuario.Email;
            viewModel.TipoUsuarioSeleccionado = viewModel.Context.TiposUsuarios.Single(tu => tu.TipoUsuarioId == usuario.TipoId);
            viewModel.Baneado = usuario.Baneado.Value;
            vNombreUnico.NombreActual = viewModel.Nombre;
            vEmailUnico.NombreActual = viewModel.Email;
        }

        private void pbContrasena_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pBox = sender as PasswordBox;
            PasswordBoxAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
        }
    }
}
