using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica de interacción para Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public Splash()
        {
            InitializeComponent();
            iniciarConfig();
            iniciarLogger();
            iniciarCarpetas();
        }

        private void iniciarConfig()
        {
            if (!File.Exists("BiomasaEUPT.exe.config"))
            {
                File.WriteAllText(@"BiomasaEUPT.exe.config", Properties.Resources.App);
            }
        }

        private void iniciarLogger()
        {
            if (Properties.Settings.Default.ModoDebug)
            {
                if (File.Exists("BiomasaEUPT.log"))
                {
                    File.Delete("BiomasaEUPT.log");
                }
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File("BiomasaEUPT.log")
                    .CreateLogger();
            }

            Log.Information("SPLASH: Logger inicializado.");
        }

        private void iniciarCarpetas()
        {
            if (!Directory.Exists("carpeta"))
            {
                Directory.CreateDirectory("carpeta");
            }
        }

        private async void Window_ContentRendered(object sender, EventArgs e)
        {
            await Task.Run(() => iniciarPrograma());
            await Task.Run(() => inicioFinalizado());

            Log.Information("SPLASH: Inicialización completa.");
            Login login = new Login();
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.usuario) && !string.IsNullOrWhiteSpace(Properties.Settings.Default.contrasena))
            {
                if (login.iniciarSesion(Properties.Settings.Default.usuario, Properties.Settings.Default.contrasena))
                {
                    login.Close();
                    MainWindow main = new MainWindow();
                    main.Show();
                    Close();
                }
                else
                {
                    login.Show();
                    login.tbUsuario.Text = Properties.Settings.Default.usuario;
                    Close();
                    MessageBox.Show("El usuario y/o la contraseña son incorrectos.", "Login incorrecto", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                login.Show();
                login.tbUsuario.Text = Properties.Settings.Default.usuario;
                Close();
            }


        }

        private void iniciarPrograma()
        {
            if (Properties.Settings.Default.ActualizarPrograma)
            {
                Actualizador actualizador = new Actualizador();

                // Estado 1 - Actualización programa
                Dispatcher.Invoke(() =>
                {
                    lInfoProgreso.Text = "Buscando actualizaciones...";
                    pbProgreso.Value = 10;
                });
                // Thread.Sleep(500);
                if (actualizador.ComprobarActualizacionPrograma())
                {
                    Dispatcher.Invoke(() =>
                    {
                        lInfoProgreso.Text = "¡Actualización encontrada!";
                    });
                    // Thread.Sleep(500);

                    Dispatcher.Invoke(() =>
                    {
                        lInfoProgreso.Text = "Actualizando...";
                        pbProgreso.Value = 25;
                    });
                    actualizador.actualizarPrograma();
                    // Thread.Sleep(500);
                }

                // Estado 2 - Conexión BD
                Dispatcher.Invoke(() =>
                {
                    lInfoProgreso.Text = "Conectándose a la BD";
                    pbProgreso.Value = 75;
                });
                // Thread.Sleep(500);
            }
        }

        private void inicioFinalizado()
        {
            Dispatcher.Invoke(() =>
            {
                lInfoProgreso.Text = "Iniciando...";
                pbProgreso.Value = 100;
            });

            // Thread.Sleep(500);
        }
    }
}
