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

namespace BiomasaEUPT.Vistas.GestionClientes
{
    /// <summary>
    /// Lógica de interacción para TablaClientes.xaml
    /// </summary>
    public partial class TablaClientes : UserControl
    {
        public TablaClientes()
        {
            InitializeComponent();
        }

        private void tbBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            DependencyObject ucParent = Parent;

            while (!(ucParent is UserControl))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }

            TabClientes tabClientes = (TabClientes)ucParent;

            tabClientes.FiltrarTabla();
        }

      /*  private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            tbObservaciones.Visibility = Visibility.Collapsed;
            rtbObservaciones.Visibility = Visibility.Visible;
            bObservaciones.IsEnabled = true;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            tbObservaciones.Visibility = Visibility.Visible;
            rtbObservaciones.Visibility = Visibility.Collapsed;
            bObservaciones.IsEnabled = false;
        }*/

        private void rtbObservaciones_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            // Elimina el formato del texto
            string textoPortapapeles = Clipboard.GetText();
            Clipboard.SetText(textoPortapapeles);
            //Console.WriteLine(new TextRange(rtbObservaciones.Document.ContentStart, rtbObservaciones.Document.ContentEnd).Text);
        }

    
    }
}
