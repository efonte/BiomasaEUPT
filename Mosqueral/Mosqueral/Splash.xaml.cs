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

namespace Mosqueral
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
            if (!File.Exists("Mosqueral.exe.config"))
            {
                File.WriteAllText(@"Mosqueral.exe.config", Properties.Resources.App);
            }
        }

        private void iniciarLogger()
        {
            if (Properties.Settings.Default.ModoDebug)
            {
                if (File.Exists("Mosqueral.log"))
                {
                    File.Delete("Mosqueral.log");
                }
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File("Mosqueral.log")
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

            MainWindow main = new MainWindow();

            await Task.Run(() => inicioFinalizado());

            Log.Information("SPLASH: Inicialización completa.");

            // Cerrar Splashscreen
            main.Show();
            Close();
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
                Thread.Sleep(500);
                if (actualizador.ComprobarActualizacionPrograma())
                {
                    Dispatcher.Invoke(() =>
                    {
                        lInfoProgreso.Text = "¡Actualización encontrada!";
                    });
                    Thread.Sleep(500);

                    Dispatcher.Invoke(() =>
                    {
                        lInfoProgreso.Text = "Actualizando...";
                        pbProgreso.Value = 25;
                    });
                    actualizador.actualizarPrograma();
                    Thread.Sleep(500);
                }

                // Estado 2 - Conexión BD
                Dispatcher.Invoke(() =>
                {
                    lInfoProgreso.Text = "Conectándose a la BD";
                    pbProgreso.Value = 75;
                });
                Thread.Sleep(500);
            }
        }

        private void inicioFinalizado()
        {
            Dispatcher.Invoke(() =>
            {
                lInfoProgreso.Text = "Iniciando...";
                pbProgreso.Value = 100;
            });

            Thread.Sleep(500);
        }
    }
}
