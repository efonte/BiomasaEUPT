using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace BiomasaEUPT.Vistas
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource clientesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientesViewSource")));
            // Cargar datos estableciendo la propiedad CollectionViewSource.Source:
            // clientesViewSource.Source = [origen de datos genérico]
            System.Windows.Data.CollectionViewSource tipos_clientesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tipos_clientesViewSource")));
            // Cargar datos estableciendo la propiedad CollectionViewSource.Source:
            // tipos_clientesViewSource.Source = [origen de datos genérico]
        }
    }
}
