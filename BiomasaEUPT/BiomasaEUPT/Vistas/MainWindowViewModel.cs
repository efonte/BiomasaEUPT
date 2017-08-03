using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.Ajustes;
using BiomasaEUPT.Vistas.GestionClientes;
using BiomasaEUPT.Vistas.GestionElaboraciones;
using BiomasaEUPT.Vistas.GestionProveedores;
using BiomasaEUPT.Vistas.GestionRecepciones;
using BiomasaEUPT.Vistas.GestionTrazabilidad;
using BiomasaEUPT.Vistas.GestionUsuarios;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ViewModelBase> Tabs { get; set; }

        private ViewModelBase _tabSeleccionada;
        public ViewModelBase TabSeleccionada
        {
            get => _tabSeleccionada;
            set
            {
                _tabSeleccionada = value;
                TabSeleccionada.Inicializar();
            }
        }

        private Usuario _usuario;
        public Usuario Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                CargarTabs();
            }
        }

        public int WidthVentana { get; set; } = 900;
        public int HeightVentana { get; set; } = 650;
        public int TopVentana { get; set; }
        public int LeftVentana { get; set; }
        public WindowState EstadoVentana { get; set; }

        private ICommand _windowClosingComando;
        private ICommand _cerrarSesionComando;
        private ICommand _mostrarAjustesComando;
        private ICommand _mostrarAcercaDeComando;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            CargarAjustes();
        }

        private void CargarTabs()
        {
            switch (Usuario.TipoId)
            {
                case 1:
                    Tabs = new ObservableCollection<ViewModelBase>()
                    {
                        new TabUsuariosViewModel(),
                        new TabClientesViewModel(),
                        new TabProveedoresViewModel(),
                        new TabRecepcionesViewModel(),
                        new TabElaboracionesViewModel(),
                        //new TabVentasViewModel(),
                        new TabTrazabilidadViewModel()
                    };
                    break;
                case 2:
                    Tabs = new ObservableCollection<ViewModelBase>()
                    {
                        new TabClientesViewModel(),
                        new TabProveedoresViewModel(),
                        new TabTrazabilidadViewModel()
                    };
                    break;
                case 3:
                    Tabs = new ObservableCollection<ViewModelBase>()
                    {
                        new TabRecepcionesViewModel(),
                        new TabElaboracionesViewModel(),
                        //new TabVentasViewModel(),
                        new TabTrazabilidadViewModel()
                    };
                    break;
            }
        }

        public void CargarAjustes()
        {
            if (Properties.Settings.Default.VentanaMaximizada)
                EstadoVentana = WindowState.Maximized;

            if (Properties.Settings.Default.TamanoVentana != "")
            {
                var m = Regex.Match(Properties.Settings.Default.TamanoVentana, @"(\d+)x(\d+)");
                if (m.Success)
                {
                    WidthVentana = Int32.Parse(m.Groups[1].Value);
                    HeightVentana = Int32.Parse(m.Groups[2].Value);
                }
            }

            if (Properties.Settings.Default.PosicionVentana != "")
            {
                var m = Regex.Match(Properties.Settings.Default.PosicionVentana, @"(\d+),(\d+)");
                if (m.Success)
                {
                    LeftVentana = Int32.Parse(m.Groups[1].Value);
                    TopVentana = Int32.Parse(m.Groups[2].Value);
                }
            }

            // Se obtiene la primera pestaña disponible
            /*TabItem tabItem = tabItem = tcTabs.Items.OfType<TabItem>().First();
            if (Properties.Settings.Default.TabActiva != "")
            {
                tabItem = tcTabs.Items.OfType<TabItem>().SingleOrDefault(n => n.Name == Properties.Settings.Default.TabActiva);
            }
            if (tabItem == null)
            {
                // Si la pestaña que se quería seleccionar no existe (de otro tipo de usuario)
                // se obtiene la primera disponible
                tabItem = tcTabs.Items.OfType<TabItem>().First();
            }
            tabItem.IsSelected = true;
            // InicializarTab hay que ejecutarlo después de que se cargue la vista
            tabItem.Loaded += (s, e1) => { InicializarTab(tabItem); };*/

            var paletteHelper = new PaletteHelper();
            paletteHelper.SetLightDark(Properties.Settings.Default.ModoNocturno);
            paletteHelper.ReplacePrimaryColor(Properties.Settings.Default.ColorPrimario);
            paletteHelper.ReplaceAccentColor(Properties.Settings.Default.ColorSecundario);
        }

        public void GuardarAjustes()
        {
            if (Properties.Settings.Default.TamanoVentana != "")
                Properties.Settings.Default.TamanoVentana = WidthVentana + "x" + HeightVentana;

            if (Properties.Settings.Default.PosicionVentana != "")
                Properties.Settings.Default.PosicionVentana = LeftVentana + "," + TopVentana;

            // if (Properties.Settings.Default.TabActiva != "")
            //     Properties.Settings.Default.TabActiva = (tcTabs.SelectedItem as TabItem).Name;

            Properties.Settings.Default.Save();
        }


        #region Cerrar Sesión
        public ICommand WindowClosingComando => _windowClosingComando ??
           (_windowClosingComando = new RelayCommandGenerico<CancelEventArgs>(
               param => CerrarAplicacion(param)
           ));
        public void CerrarAplicacion(CancelEventArgs e)
        {
            GuardarAjustes();
            //Application.Current.MainWindow.Close();
        }
        #endregion


        #region Cerrar Sesión
        public ICommand CerrarSesionComando => _cerrarSesionComando ??
            (_cerrarSesionComando = new RelayCommand(
                param => CerrarSesion()
             ));

        public void CerrarSesion()
        {
            Properties.Settings.Default.contrasena = "";
            Properties.Settings.Default.Save();
            Login login = new Login();
            Application.Current.MainWindow.Close();
            login.Show();
            login.tbUsuario.Text = Properties.Settings.Default.usuario;
        }
        #endregion


        #region Mostrar Ajustes
        public ICommand MostrarAjustesComando => _mostrarAjustesComando ??
            (_mostrarAjustesComando = new RelayCommand(
                param => MostrarAjustes()
             ));

        public void MostrarAjustes()
        {
            Console.WriteLine(Application.Current.MainWindow);
            WinAjustes ajustes = new WinAjustes()
            {
                Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)
            };
            ajustes.ShowDialog();
        }
        #endregion


        #region Mostrar Acerca De
        public ICommand MostrarAcercaDeComando => _mostrarAcercaDeComando ??
            (_mostrarAcercaDeComando = new RelayCommand(
                param => MostrarAcercaDe()
             ));

        public void MostrarAcercaDe()
        {
            AcercaDe acercaDe = new AcercaDe()
            {
                Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)
            };
            acercaDe.ShowDialog();
        }
        #endregion
    }
}
