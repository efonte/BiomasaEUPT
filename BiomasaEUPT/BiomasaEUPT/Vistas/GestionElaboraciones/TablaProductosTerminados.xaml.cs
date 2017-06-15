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

namespace BiomasaEUPT.Vistas.GestionElaboraciones
{
    /// <summary>
    /// Lógica de interacción para TablaProductosTerminados.xaml
    /// </summary>
    public partial class TablaProductosTerminados : UserControl
    {
        public TablaProductosTerminados()
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

            TabElaboraciones tabElaboraciones = (TabElaboraciones)ucParent;

            //tabElaboraciones.FiltrarTablaProductosTerminados();
        }
    }
}
