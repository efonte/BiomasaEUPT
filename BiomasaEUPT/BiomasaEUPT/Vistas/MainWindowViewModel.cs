using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.Ajustes;
using BiomasaEUPT.Vistas.GestionClientes;
using BiomasaEUPT.Vistas.GestionElaboraciones;
using BiomasaEUPT.Vistas.GestionPermisos;
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

                // Se añaden los atajos de teclado.
                // Hay que añadirlos cada vez porque sino al cambiar de pestaña se pierde el foco del teclado
                // y no funcionarían hasta que no se hiciera clic en algún componente de la vista.
                var ventana = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                if (ventana.InputBindings != null && ventana.InputBindings.Count >= 4)
                {
                    ventana.InputBindings.RemoveAt(3);
                    ventana.InputBindings.RemoveAt(2);
                    ventana.InputBindings.RemoveAt(1);
                    ventana.InputBindings.RemoveAt(0);
                }

                if (TabSeleccionada is TabPermisosViewModel)
                {
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabPermisosViewModel).
                           AnadirTipoUsuarioComando, new KeyGesture(Key.A, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabPermisosViewModel).
                           ModificarTipoUsuarioComando, new KeyGesture(Key.M, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabPermisosViewModel).
                           BorrarTipoUsuarioComando, new KeyGesture(Key.B, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabPermisosViewModel).
                           RefrescarTiposUsuariosComando, new KeyGesture(Key.R, (ModifierKeys.Control | ModifierKeys.Shift))));
                }
                else if (TabSeleccionada is TabUsuariosViewModel)
                {
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabUsuariosViewModel).
                           AnadirUsuarioComando, new KeyGesture(Key.A, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabUsuariosViewModel).
                           ModificarUsuarioComando, new KeyGesture(Key.M, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabUsuariosViewModel).
                           BorrarUsuarioComando, new KeyGesture(Key.B, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabUsuariosViewModel).
                           RefrescarUsuariosComando, new KeyGesture(Key.R, (ModifierKeys.Control | ModifierKeys.Shift))));
                }
                else if (TabSeleccionada is TabClientesViewModel)
                {
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabClientesViewModel).
                           AnadirClienteComando, new KeyGesture(Key.A, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabClientesViewModel).
                           ModificarClienteComando, new KeyGesture(Key.M, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabClientesViewModel).
                           BorrarClienteComando, new KeyGesture(Key.B, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabClientesViewModel).
                           RefrescarClientesComando, new KeyGesture(Key.R, (ModifierKeys.Control | ModifierKeys.Shift))));
                }
                else if (TabSeleccionada is TabProveedoresViewModel)
                {
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabProveedoresViewModel).
                           AnadirProveedorComando, new KeyGesture(Key.A, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabProveedoresViewModel).
                           ModificarProveedorComando, new KeyGesture(Key.M, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabProveedoresViewModel).
                           BorrarProveedorComando, new KeyGesture(Key.B, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabProveedoresViewModel).
                           RefrescarProveedoresComando, new KeyGesture(Key.R, (ModifierKeys.Control | ModifierKeys.Shift))));
                }
                else if (TabSeleccionada is TabRecepcionesViewModel)
                {
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabRecepcionesViewModel).
                           AnadirMateriaPrimaComando, new KeyGesture(Key.A, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabRecepcionesViewModel).
                           ModificarMateriaPrimaComando, new KeyGesture(Key.M, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabRecepcionesViewModel).
                           BorrarMateriaPrimaComando, new KeyGesture(Key.B, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabRecepcionesViewModel).
                           RefrescarMateriasPrimasComando, new KeyGesture(Key.R, (ModifierKeys.Control | ModifierKeys.Shift))));
                }
                else if (TabSeleccionada is TabElaboracionesViewModel)
                {
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabElaboracionesViewModel).
                           AnadirProductoTerminadoComando, new KeyGesture(Key.A, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabElaboracionesViewModel).
                           ModificarProductoTerminadoComando, new KeyGesture(Key.M, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabElaboracionesViewModel).
                           BorrarProductoTerminadoComando, new KeyGesture(Key.B, (ModifierKeys.Control | ModifierKeys.Shift))));
                    ventana.InputBindings.Add(new KeyBinding((TabSeleccionada as TabElaboracionesViewModel).
                           RefrescarProductosTerminadosComando, new KeyGesture(Key.R, (ModifierKeys.Control | ModifierKeys.Shift))));
                }

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
                CargarAjustes();
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

        private Dictionary<Tab, ViewModelBase> viewModelsDisponibles;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            viewModelsDisponibles = new Dictionary<Tab, ViewModelBase>()
            {
                { Tab.Permisos, new TabPermisosViewModel() },
                { Tab.Usuarios, new TabUsuariosViewModel() },
                { Tab.Clientes, new TabClientesViewModel() },
                { Tab.Proveedores, new TabProveedoresViewModel() },
                { Tab.Recepciones, new TabRecepcionesViewModel() },
                { Tab.Elaboraciones, new TabElaboracionesViewModel() },
               // { Tab.Ventas, new TabVentasViewModel() },
                { Tab.Trazabilidad, new TabTrazabilidadViewModel() },
            };
        }

        private void CargarTabs()
        {
            Tabs = new ObservableCollection<ViewModelBase>(
                Usuario.TipoUsuario.Permisos
                .Select(p => p.Tab)
                .Where(k => viewModelsDisponibles.ContainsKey(k))
                .Select(k => viewModelsDisponibles[k]).ToList());
        }

        public void CargarAjustes()
        {
            if (Properties.Settings.Default.VentanaMaximizada)
                EstadoVentana = WindowState.Maximized;

            if (Properties.Settings.Default.RecordarTamanoVentana)
            {
                var m = Regex.Match(Properties.Settings.Default.TamanoVentana, @"(\d+)x(\d+)");
                if (m.Success)
                {
                    WidthVentana = Int32.Parse(m.Groups[1].Value);
                    HeightVentana = Int32.Parse(m.Groups[2].Value);
                }
            }

            if (Properties.Settings.Default.RecordarPosicionVentana)
            {
                var m = Regex.Match(Properties.Settings.Default.PosicionVentana, @"(\d+),(\d+)");
                if (m.Success)
                {
                    LeftVentana = Int32.Parse(m.Groups[1].Value);
                    TopVentana = Int32.Parse(m.Groups[2].Value);
                }
            }
            else
            {
                // Centra la ventana a la pantalla
                LeftVentana = Convert.ToInt32((SystemParameters.PrimaryScreenWidth / 2) - (WidthVentana / 2));
                TopVentana = Convert.ToInt32((SystemParameters.PrimaryScreenHeight / 2) - (HeightVentana / 2));
            }

            // Se obtiene la primera pestaña disponible
            /*TabItem tabItem = tabItem = tcTabs.Items.OfType<TabItem>().First();
            if (Properties.Settings.Default.RecordarTabActiva)
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
            if (Properties.Settings.Default.RecordarTabActiva)
            {
                var tabASeleccionar = viewModelsDisponibles
                    .FirstOrDefault(vmd => vmd.Key.ToString() == Properties.Settings.Default.TabActiva)
                    .Value;
                if (tabASeleccionar != null && Tabs.Contains(tabASeleccionar))
                {
                    TabSeleccionada = tabASeleccionar;
                }
            }

            var paletteHelper = new PaletteHelper();
            paletteHelper.SetLightDark(Properties.Settings.Default.ModoNocturno);
            paletteHelper.ReplacePrimaryColor(Properties.Settings.Default.ColorPrimario);
            paletteHelper.ReplaceAccentColor(Properties.Settings.Default.ColorSecundario);
        }

        public void GuardarAjustes()
        {
            if (Properties.Settings.Default.RecordarTamanoVentana)
                Properties.Settings.Default.TamanoVentana = WidthVentana + "x" + HeightVentana;

            if (Properties.Settings.Default.RecordarPosicionVentana)
                Properties.Settings.Default.PosicionVentana = LeftVentana + "," + TopVentana;

            if (Properties.Settings.Default.RecordarTabActiva)
                Properties.Settings.Default.TabActiva = viewModelsDisponibles
                    .Single(vmd => vmd.Value == TabSeleccionada)
                    .Key.ToString();

            Properties.Settings.Default.Save();
        }


        #region Cerrar Aplicación
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
            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Close();
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
            Console.WriteLine(LeftVentana + " " + TopVentana);
            AcercaDe acercaDe = new AcercaDe()
            {
                Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)
            };
            acercaDe.ShowDialog();
        }
        #endregion
    }
}
