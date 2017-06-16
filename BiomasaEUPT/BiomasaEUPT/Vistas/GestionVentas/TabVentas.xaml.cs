using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiomasaEUPT.Vistas.GestionVentas
{
    /// <summary>
    /// Lógica de interacción para TabVentas.xaml
    /// </summary>
    public partial class TabVentas : UserControl
    {

        private BiomasaEUPTContext context;
        private CollectionViewSource productosEnvasadosViewSource;
        private CollectionViewSource pedidosViewSource;

        public TabVentas()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = new BiomasaEUPTContext();
                productosEnvasadosViewSource = (CollectionViewSource)(FindResource("productosEnvasadosViewSource"));
                pedidosViewSource = (CollectionViewSource)(FindResource("pedidosViewSource"));
                context.ProductosEnvasados.Load();
                context.PedidosCabeceras.Load();

                productosEnvasadosViewSource.Source = context.ProductosEnvasados.Local;
                pedidosViewSource.Source = context.PedidosCabeceras.Local;
            }
        }
    }
}
