using BiomasaEUPT.Modelos;
using BiomasaEUPT.Vistas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
        private SplashViewModel viewModel;
        private Actualizador actualizador;
        public Splash()
        {
            InitializeComponent();
            viewModel = new SplashViewModel();
            actualizador = new Actualizador();
            DataContext = viewModel;
            //IniciarConfig();
            BorrarBackups();
        }

        private void IniciarConfig()
        {
            // Si el usuario ha borradopor error el fichero de configuración se restaura
            if (!File.Exists("BiomasaEUPT.exe.config"))
            {
                File.WriteAllText(@"BiomasaEUPT.exe.config", Properties.Resources.App);
            }
        }

        private void BorrarBackups()
        {
            //Se borran todos los ficheros temporales (backups) que quedaron tras actualizar el programa.
            // AVISO -> ESTO PUEDE BORRAR TODOS LOS FICHEROS DE TODOS LOS SUBDIRECTORIOS DESDE DONDE SE ENCUENTRE EL EXE
            /* Directory.GetFiles(".", "*", SearchOption.AllDirectories)
                 .Where(f => f.EndsWith(".bak")).ToList()
                 .ForEach(f => File.Delete(f));*/
            var ficherosBak = new List<string>();
            actualizador.Ficheros.ForEach(f => ficherosBak.Add(f += ".bak"));
            ficherosBak.ToList().ForEach(f => { if (File.Exists(f)) File.Delete(f); });
            // Se borra la carpeta de actualización si existe
            if (Directory.Exists("actualizacion"))
            {
                Directory.Delete("actualizacion", true);
            }
        }

        private async void Window_ContentRendered(object sender, EventArgs e)
        {
            await Task.Run(() => IniciarPrograma());

            Login login = new Login();
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.usuario) && !string.IsNullOrWhiteSpace(Properties.Settings.Default.contrasena))
            {
                var usuario = login.ViewModel.IniciarSesion(Properties.Settings.Default.usuario, Properties.Settings.Default.contrasena);
                if (usuario != null)
                {
                    login.Close();
                    MainWindow main = new MainWindow();
                    (main.DataContext as MainWindowViewModel).Usuario = usuario;
                    await Task.Run(() => InicioFinalizado());
                    Close();
                    main.Show();

                }
                else
                {
                    login.Show();
                    login.tbUsuario.Text = Properties.Settings.Default.usuario;
                    Close();
                    login.ViewModel.MensajeLoginIncorrecto();
                }
            }
            else
            {
                login.Show();
                login.tbUsuario.Text = Properties.Settings.Default.usuario;
                Close();
            }


        }

        private void IniciarPrograma()
        {
            // Estado 1 - Actualización
            //Properties.Settings.Default.ActualizarPrograma = true;
            if (Properties.Settings.Default.ActualizarPrograma)
            {
                Dispatcher.Invoke(() =>
                {
                    viewModel.MensajeInformacion = "Buscando actualizaciones...";
                    viewModel.Progreso = 10;
                });
                if (actualizador.ComprobarActualizacionPrograma())
                {
                    Dispatcher.Invoke(() =>
                    {
                        viewModel.MensajeInformacion = "Actualización encontrada. Actualizando...";
                        viewModel.Progreso = 50;
                    });
                    try
                    {
                        actualizador.ActualizarPrograma();
                        Dispatcher.Invoke(() =>
                        {
                            viewModel.MensajeInformacion = "Actualización completada. Reiniciando la aplicación...";
                            viewModel.Progreso = 100;
                        });
#if (!DEBUG)
                        Thread.Sleep(2500);
#endif
                        Process.Start("BiomasaEUPT.exe"); // Inicia la aplicación actualizada
                        Process.GetCurrentProcess().Kill(); // Cierra la aplicación antigua
                    }
                    catch (WebException ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            viewModel.MensajeInformacion = "Actualización fallida.";
                            viewModel.Progreso = 100;
                        });
#if (!DEBUG)
                    Thread.Sleep(1000);
#endif
                    }
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        viewModel.MensajeInformacion = "No se ha encontrado ninguna actualización.";
                        viewModel.Progreso = 25;
                    });
                }
            }

            // Estado 2 - Conexión BD
            Dispatcher.Invoke(() =>
            {
                viewModel.MensajeInformacion = "Conectándose a la BD...";
                viewModel.Progreso = 50;

            });

            using (var context = new BiomasaEUPTContext())
            {
                try { context.Database.Connection.Open(); }
                catch
                {
                    Dispatcher.Invoke(() =>
                    {
                        viewModel.MensajeInformacion = "No se ha podido conectar con la Base de Datos. Saliendo...";
                        viewModel.Progreso = 100;
                    });
#if (!DEBUG)
                    Thread.Sleep(2000);
#endif
                    /// Cierra la aplicación
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        private void InicioFinalizado()
        {
            // Estado 3 - Iniciar
            Dispatcher.Invoke(() =>
            {
                viewModel.MensajeInformacion = "Iniciando...";
                viewModel.Progreso = 100;

            });
#if (!DEBUG)
            Thread.Sleep(500);
#endif
        }
    }
}
