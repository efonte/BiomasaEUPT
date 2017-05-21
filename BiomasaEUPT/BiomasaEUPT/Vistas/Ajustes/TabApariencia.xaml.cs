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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiomasaEUPT.Vistas.Ajustes
{
    /// <summary>
    /// Lógica de interacción para TabApariencia.xaml
    /// </summary>
    public partial class TabApariencia : UserControl
    {
        public TabApariencia()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = e.OriginalSource as CheckBox;
            if (checkBox == cbVentanaMaximizada)
                Properties.Settings.Default.VentanaMaximizada = true;

            //if (checkBox == cbTamanoVentana)
                //Properties.Settings.Default.TamanoVentana =  "x";

            Properties.Settings.Default.Save();
        }
    }
}
