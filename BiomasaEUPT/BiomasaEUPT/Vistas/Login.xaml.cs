using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BiomasaEUPT
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public LoginViewModel ViewModel { get; set; }

        public Login()
        {
            InitializeComponent();
            ViewModel = new LoginViewModel();
            DataContext = ViewModel;
        }

        private void pbContrasena_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pBox = sender as PasswordBox;
            PasswordBoxAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
        }

        private void CargarVistaMain(Usuario usuario)
        {
            MainWindow mainWindows = new MainWindow(usuario);
            Close();
            mainWindows.Show();
        }

        private void bIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            String hashContrasena = "";
            if (ViewModel.Contrasena != null)
            {
                hashContrasena = ContrasenaHashing.ObtenerHashSHA256(ContrasenaHashing.SecureStringToString(ViewModel.Contrasena));
            }

            var usuario = ViewModel.IniciarSesion(ViewModel.Usuario, hashContrasena);
            if (usuario != null)
            {
                Properties.Settings.Default.contrasena = cbRecordarme.IsChecked == true ? hashContrasena : "";
                Properties.Settings.Default.usuario = ViewModel.Usuario;
                Properties.Settings.Default.Save();
                CargarVistaMain(usuario);
            }
            else
            {
                MensajeLoginIncorrecto();
            }
        }

        public async void MensajeLoginIncorrecto()
        {
            var mensaje = new MensajeInformacion()
            {
                Titulo = "Login incorrecto",
                Mensaje = "El usuario y/o la contraseña son incorrectos."
            };
            var resultado = await DialogHost.Show(mensaje, "RootDialog");
            // Console.WriteLine(mensaje.DataContext.GetType().GetProperty("Nombre").GetValue(mensaje.DataContext));
        }
    }
}
