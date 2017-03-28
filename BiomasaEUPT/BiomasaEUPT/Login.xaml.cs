using BiomasaEUPT.Domain;
using System;
using System.Collections.Generic;
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
                /*try
                 {
                     // Shouldn't get to here if the controls do not have valid values.
                     String Usuario = ViewModel.Usuario;
                     //double y = ViewModel.y.Value;

                     //ViewModel.Sum = CalculationService.Add(x, y);
                 }
                 catch (Exception)
                 {
                     return;
                 }*/
            }, ViewModel.PuedeIniciarSesion);


            DataContext = ViewModel;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
