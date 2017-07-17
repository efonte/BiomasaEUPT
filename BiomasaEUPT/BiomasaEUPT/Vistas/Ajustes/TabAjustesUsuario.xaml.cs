using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

namespace BiomasaEUPT.Vistas.Ajustes
{
    /// <summary>
    /// Lógica de interacción para TabAjustesUsuario.xaml
    /// </summary>
    public partial class TabAjustesUsuario : UserControl
    {
        private WinAjustes winAjustes;

        public TabAjustesUsuario()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DependencyObject ucParent = Parent;

            while (!(ucParent is Window))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }
            winAjustes = (WinAjustes)ucParent;
        }

        private void pbContrasena_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pBox = sender as PasswordBox;
            PasswordBoxAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
        }

        private async void bCambiarContrasena_Click(object sender, RoutedEventArgs e)
        {
            int usuarioId = (winAjustes.Owner as MainWindow).Usuario.UsuarioId;
            var viewModel = DataContext as WinAjustesViewModel;
            var hashContrasena = ContrasenaHashing.ObtenerHashSHA256(
                    ContrasenaHashing.SecureStringToString(viewModel.Contrasena)
                );

            using (var context = new BiomasaEUPTContext())
            {
                var usuario = context.Usuarios.Single(u => u.UsuarioId == usuarioId);
                usuario.Contrasena = hashContrasena;
                context.SaveChanges();
            }
            var mensaje = new MensajeInformacion()
            {
                Titulo = "Contraseña Cambiada",
                Mensaje = "La contraseña se ha cambiado correctamente."
            };
            var resultado = await DialogHost.Show(mensaje, "RootDialog");

        }

    }
}
