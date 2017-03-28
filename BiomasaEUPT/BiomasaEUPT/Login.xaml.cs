using BiomasaEUPT.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        public Login()
        {
            InitializeComponent();

            //DataContext = new LoginViewModel();

            Assembly assembly = typeof(LoginViewModel).Assembly;
            LoginViewModel ViewModel = (LoginViewModel)assembly.CreateInstance(typeof(LoginViewModel).FullName);
            if (ViewModel == null)
            {
                throw new Exception("No se puede crear ViewModel " + typeof(LoginViewModel).FullName);
            }


            ViewModel.IniciarSesionCmd = new RelayCommand((object z) =>
            {               
                using (var ctx = new BiomasaEUPTEntities())
                {
                    String hashContrasena = ContrasenaHashing.obtenerHashSHA256(ContrasenaHashing.SecureStringToString(ViewModel.Contrasena));

                    try
                    {
                        usuarios usuario = ctx.usuarios.Where(s => s.nombre == ViewModel.Usuario && s.contrasena == hashContrasena).First<usuarios>();
                        MessageBox.Show("Login Correctos.", "Login Correcto", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("El usuario y/o la contraseña son incorrectos.", "Login incorrecto", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }


                }
            }, ViewModel.PuedeIniciarSesion);


            DataContext = ViewModel;

            //BiomasaEUPTEntities ctx = new BiomasaEUPTEntities();
            //using (BiomasaEUPTEntities context = new BiomasaEUPTEntities())
            // {
            // var objectContext = (context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;

            //use objectContext here..
            //usuarios usuariosEntity = context.usuarios.FirstOrDefault<usuarios>;
            //var entityType = ObjectContext.GetObjectType(usuariosEntity.GetType());

            //}          

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void pbContrasena_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pBox = sender as PasswordBox;
            PasswordBoxMVVMAttachedProperties.SetEncryptedPassword(pBox, pBox.SecurePassword);
        }
    }
}
