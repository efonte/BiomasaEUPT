using BiomasaEUPT.Vistas;
using BiomasaEUPT.Vistas.Ajustes;
using BiomasaEUPT.Vistas.GestionClientes;
using BiomasaEUPT.Vistas.GestionElaboraciones;
using BiomasaEUPT.Vistas.GestionProveedores;
using BiomasaEUPT.Vistas.GestionRecepciones;
using BiomasaEUPT.Vistas.GestionUsuarios;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace BiomasaEUPT
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CargarAjustes();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void menuSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GuardarAjustes();
        }

        private void menuAcercaDe_Click(object sender, RoutedEventArgs e)
        {
            AcercaDe acercaDe = new AcercaDe();
            acercaDe.Owner = GetWindow(this);
            acercaDe.ShowDialog();
        }

        private void menuGitHub_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/F0NT3/BiomasaEUPT");
        }

        private void menuCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.contrasena = "";
            Properties.Settings.Default.Save();
            Login login = new Login();
            login.Show();
            login.tbUsuario.Text = Properties.Settings.Default.usuario;
            Close();
        }

        private void ti_Selected(object sender, RoutedEventArgs e)
        {
            var tabItem = (e.Source as TabItem);
            if (tabItem != null)
            {
                InicializarTab(tabItem);
            }
        }

        private void InicializarTab(TabItem tabItem)
        {
            if (tabItem.Content is TabUsuarios)
            {
                var ti = (tabItem.Content as TabUsuarios);
                if (ti.IsLoaded)
                {
                    (ti.DataContext as ViewModelBase).Inicializar();
                }
            }

            else if (tabItem.Content is TabClientes)
            {
                var ti = (tabItem.Content as TabClientes);
                if (ti.IsLoaded)
                {
                    (ti.DataContext as ViewModelBase).Inicializar();
                    //ti.CargarClientes();
                    //ti.ucFiltroTabla.CargarFiltro();
                }
            }

            else if (tabItem.Content is TabProveedores)
            {
                var ti = (tabItem.Content as TabProveedores);
                if (ti.IsLoaded)
                {
                    (ti.DataContext as ViewModelBase).Inicializar();
                    //ti.CargarProveedores();
                    //ti.ucFiltroTabla.CargarFiltro();
                }
            }

            else if (tabItem.Content is TabRecepciones)
            {
                var ti = (tabItem.Content as TabRecepciones);
                if (ti.IsLoaded)
                {
                    (ti.DataContext as ViewModelBase).Inicializar();
                    //ti.CargarRecepciones();
                }
            }

            else if (tabItem.Content is TabElaboraciones)
            {
                var ti = (tabItem.Content as TabElaboraciones);
                if (ti.IsLoaded)
                {
                    //ti.CargarElaboraciones();
                }
            }
        }

        private void menuAjustes_Click(object sender, RoutedEventArgs e)
        {
            WinAjustes ajustes = new WinAjustes();
            ajustes.Owner = GetWindow(this);
            ajustes.ShowDialog();
        }

        private void CargarAjustes()
        {
            if (Properties.Settings.Default.VentanaMaximizada)
                WindowState = WindowState.Maximized;

            if (Properties.Settings.Default.TamanoVentana != "")
            {
                var m = Regex.Match(Properties.Settings.Default.TamanoVentana, @"(\d+)x(\d+)");
                if (m.Success)
                {
                    Width = Int32.Parse(m.Groups[1].Value);
                    Height = Int32.Parse(m.Groups[2].Value);
                }
            }

            if (Properties.Settings.Default.TabActiva != "")
            {
                var tabItem = tcTabs.Items.OfType<TabItem>().SingleOrDefault(n => n.Name == Properties.Settings.Default.TabActiva);
                tabItem.IsSelected = true;
                // InicializarTab hay que ejecutarlo después de que se cargue la vista
                tabItem.Loaded += (s, e1) => { InicializarTab(tabItem); };
            }

            var paletteHelper = new PaletteHelper();
            paletteHelper.SetLightDark(Properties.Settings.Default.ModoNocturno);
            paletteHelper.ReplacePrimaryColor(Properties.Settings.Default.ColorPrimario);
            paletteHelper.ReplaceAccentColor(Properties.Settings.Default.ColorSecundario);
        }

        private void GuardarAjustes()
        {
            if (Properties.Settings.Default.TamanoVentana != "")
                Properties.Settings.Default.TamanoVentana = Width + "x" + Height;

            if (Properties.Settings.Default.PosicionVentana != "")
                Properties.Settings.Default.PosicionVentana = Left + "," + Top;

            if (Properties.Settings.Default.TabActiva != "")
                Properties.Settings.Default.TabActiva = (tcTabs.SelectedItem as TabItem).Name;

            Properties.Settings.Default.Save();
        }

    }
}
