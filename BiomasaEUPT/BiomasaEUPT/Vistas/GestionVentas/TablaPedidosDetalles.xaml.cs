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

namespace BiomasaEUPT.Vistas.GestionVentas
{
    /// <summary>
    /// Lógica de interacción para TablaPedidosDetalles.xaml
    /// </summary>
    public partial class TablaPedidosDetalles : UserControl
    {
        public TablaPedidosDetalles()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void tbBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            DependencyObject ucParent = Parent;

            while (!(ucParent is UserControl))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }

            TabVentas tabVentas = (TabVentas)ucParent;

            //tabVentas.FiltrarTablaPedidos();
        }
    }
}
