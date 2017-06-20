using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
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
        private CollectionViewSource tiposProductosTerminadosViewSource;
        private CollectionViewSource pickingViewSource;
        private CollectionViewSource clientesViewSource;
        private CollectionViewSource estadosPedidosViewSource;

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
                tiposProductosTerminadosViewSource = (CollectionViewSource)(FindResource("tiposProductosTerminadosViewSource"));
                pickingViewSource = (CollectionViewSource)(FindResource("pickingViewSource"));
                clientesViewSource = (CollectionViewSource)(FindResource("clientesViewSource"));
                estadosPedidosViewSource = (CollectionViewSource)(FindResource("estadosPedidosViewSource"));

                context.ProductosEnvasados.Load();
                context.PedidosCabeceras.Load();
                context.TiposProductosTerminados.Load();
                context.Picking.Load();
                context.Clientes.Load();
                context.EstadosPedidos.Load();
                
                productosEnvasadosViewSource.Source = context.ProductosEnvasados.Local;
                pedidosViewSource.Source = context.PedidosCabeceras.Local;
                tiposProductosTerminadosViewSource.Source = context.TiposProductosTerminados.Local;
                pickingViewSource.Source = context.Picking.Local;
                clientesViewSource.Source = context.Clientes.Local;
                estadosPedidosViewSource.Source = context.EstadosPedidos.Local;
            }
        }
    }
}
