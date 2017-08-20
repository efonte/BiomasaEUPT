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
using System.Windows.Shapes;

namespace BiomasaEUPT.Vistas.Ajustes
{
    /// <summary>
    /// Lógica de interacción para WinAjustes.xaml
    /// </summary>
    public partial class WinAjustes : Window
    {
        public WinAjustes()
        {
            InitializeComponent();
            DataContext = new WinAjustesViewModel();
            ucTabGeneral.cbComprobarActualizaciones.IsChecked = Properties.Settings.Default.ActualizarPrograma;
            ucTabVentana.cbVentanaMaximizada.IsChecked = Properties.Settings.Default.VentanaMaximizada;
            ucTabVentana.cbTamanoVentana.IsChecked = Properties.Settings.Default.RecordarTamanoVentana;
            ucTabVentana.cbPosicionVentana.IsChecked = Properties.Settings.Default.RecordarPosicionVentana;
            ucTabVentana.cbTabActiva.IsChecked = Properties.Settings.Default.RecordarTabActiva;
            ucTabApariencia.cbModoNocturno.IsChecked = Properties.Settings.Default.ModoNocturno;

            ucTabGeneral.cbComprobarActualizaciones.Checked += CheckBox_Checked;
            ucTabVentana.cbVentanaMaximizada.Checked += CheckBox_Checked;
            ucTabVentana.cbTamanoVentana.Checked += CheckBox_Checked;
            ucTabVentana.cbPosicionVentana.Checked += CheckBox_Checked;
            ucTabVentana.cbTabActiva.Checked += CheckBox_Checked;
            ucTabApariencia.cbModoNocturno.Checked += CheckBox_Checked;

            ucTabGeneral.cbComprobarActualizaciones.Unchecked += CheckBox_Unchecked;
            ucTabVentana.cbVentanaMaximizada.Unchecked += CheckBox_Unchecked;
            ucTabVentana.cbTamanoVentana.Unchecked += CheckBox_Unchecked;
            ucTabVentana.cbPosicionVentana.Unchecked += CheckBox_Unchecked;
            ucTabVentana.cbTabActiva.Unchecked += CheckBox_Unchecked;
            ucTabApariencia.cbModoNocturno.Unchecked += CheckBox_Unchecked;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = e.OriginalSource as CheckBox;

            if (checkBox == ucTabGeneral.cbComprobarActualizaciones)
                Properties.Settings.Default.ActualizarPrograma = true;

            if (checkBox == ucTabVentana.cbVentanaMaximizada)
                Properties.Settings.Default.VentanaMaximizada = true;

            if (checkBox == ucTabVentana.cbTamanoVentana)
            {
                Properties.Settings.Default.RecordarTamanoVentana = true;
                Properties.Settings.Default.TamanoVentana = (Owner as MainWindow).Width + "x" + (Owner as MainWindow).Height;
            }

            if (checkBox == ucTabVentana.cbPosicionVentana)
            {
                Properties.Settings.Default.RecordarPosicionVentana = true;
                Properties.Settings.Default.PosicionVentana = (Owner as MainWindow).Left + "," + (Owner as MainWindow).Top;
            }

            if (checkBox == ucTabVentana.cbTabActiva)
            {
                Properties.Settings.Default.RecordarTabActiva = true;
                Properties.Settings.Default.TabActiva = ((Owner as MainWindow).tcTabs.SelectedItem as TabItem).Name;
            }

            if (checkBox == ucTabApariencia.cbModoNocturno)
            {
                Properties.Settings.Default.ModoNocturno = true;
                new PaletteHelper().SetLightDark(true);
            }

            Properties.Settings.Default.Save();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = e.OriginalSource as CheckBox;

            if (checkBox == ucTabGeneral.cbComprobarActualizaciones)
                Properties.Settings.Default.ActualizarPrograma = false;

            if (checkBox == ucTabVentana.cbVentanaMaximizada)
                Properties.Settings.Default.VentanaMaximizada = false;

            if (checkBox == ucTabVentana.cbTamanoVentana)
                Properties.Settings.Default.RecordarTamanoVentana = false;

            if (checkBox == ucTabVentana.cbPosicionVentana)
                Properties.Settings.Default.RecordarPosicionVentana = false;

            if (checkBox == ucTabVentana.cbTabActiva)
                Properties.Settings.Default.RecordarTabActiva = false;

            if (checkBox == ucTabApariencia.cbModoNocturno)
            {
                Properties.Settings.Default.ModoNocturno = false;
                new PaletteHelper().SetLightDark(false);
            }
            Properties.Settings.Default.Save();
        }
    }
}
