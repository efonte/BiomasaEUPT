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
            ucTabApariencia.cbVentanaMaximizada.IsChecked = Properties.Settings.Default.VentanaMaximizada;
            ucTabApariencia.cbTamanoVentana.IsChecked = Properties.Settings.Default.TamanoVentana != "";
            ucTabApariencia.cbPosicionVentana.IsChecked = Properties.Settings.Default.PosicionVentana != "";
            ucTabApariencia.cbTabActiva.IsChecked = Properties.Settings.Default.TabActiva != "";
            ucTabApariencia.cbModoNocturno.IsChecked = Properties.Settings.Default.ModoNocturno;

            ucTabApariencia.cbVentanaMaximizada.Checked += CheckBox_Checked;
            ucTabApariencia.cbTamanoVentana.Checked += CheckBox_Checked;
            ucTabApariencia.cbPosicionVentana.Checked += CheckBox_Checked;
            ucTabApariencia.cbTabActiva.Checked += CheckBox_Checked;
            ucTabApariencia.cbModoNocturno.Checked += CheckBox_Checked;

            ucTabApariencia.cbVentanaMaximizada.Unchecked += CheckBox_Unchecked;
            ucTabApariencia.cbTamanoVentana.Unchecked += CheckBox_Unchecked;
            ucTabApariencia.cbPosicionVentana.Unchecked += CheckBox_Unchecked;
            ucTabApariencia.cbTabActiva.Unchecked += CheckBox_Unchecked;
            ucTabApariencia.cbModoNocturno.Unchecked += CheckBox_Unchecked;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = e.OriginalSource as CheckBox;

            if (checkBox == ucTabApariencia.cbVentanaMaximizada)
                Properties.Settings.Default.VentanaMaximizada = true;

            if (checkBox == ucTabApariencia.cbTamanoVentana)
                Properties.Settings.Default.TamanoVentana = (Owner as MainWindow).Width + "x" + (Owner as MainWindow).Height;

            if (checkBox == ucTabApariencia.cbPosicionVentana)
                Properties.Settings.Default.PosicionVentana = (Owner as MainWindow).Left + "," + (Owner as MainWindow).Top;

            if (checkBox == ucTabApariencia.cbTabActiva)
                Properties.Settings.Default.TabActiva = ((Owner as MainWindow).tcTabs.SelectedItem as TabItem).Name;

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

            if (checkBox == ucTabApariencia.cbVentanaMaximizada)
                Properties.Settings.Default.VentanaMaximizada = false;

            if (checkBox == ucTabApariencia.cbTamanoVentana)
                Properties.Settings.Default.TamanoVentana = "";

            if (checkBox == ucTabApariencia.cbPosicionVentana)
                Properties.Settings.Default.PosicionVentana = "";

            if (checkBox == ucTabApariencia.cbTabActiva)
                Properties.Settings.Default.TabActiva = "";

            if (checkBox == ucTabApariencia.cbModoNocturno)
            {
                Properties.Settings.Default.ModoNocturno = false;
                new PaletteHelper().SetLightDark(false);
            }
            Properties.Settings.Default.Save();
        }
    }
}
