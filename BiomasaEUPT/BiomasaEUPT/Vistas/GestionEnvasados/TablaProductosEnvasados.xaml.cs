using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos.Tablas;
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

namespace BiomasaEUPT.Vistas.GestionEnvasados
{
    /// <summary>
    /// Lógica de interacción para TablaProductosEnvasados.xaml
    /// </summary>
    public partial class TablaProductosEnvasados : UserControl
    {

        private Trazabilidad trazabilidad;

        public TablaProductosEnvasados()
        {
            InitializeComponent();
            trazabilidad = new Trazabilidad();
        }

        private void bPdfProducto_Click(object sender, RoutedEventArgs e)
        {
            ProductoEnvasado productoEnvasado = (sender as Button).DataContext as ProductoEnvasado;

            InformePDF informe = new InformePDF(Properties.Settings.Default.DirectorioInformes);
            System.Diagnostics.Process.Start(informe.GenerarInformeProductoEnvasado(trazabilidad.ProductoEnvasado(productoEnvasado.Codigo)));
        }

        private void bCodigo_Click(object sender, RoutedEventArgs e)
        {
            ProductoEnvasado productoEnvasado = (sender as Button).DataContext as ProductoEnvasado;

            System.Diagnostics.Process.Start(new InformePDF().GenerarPDFCodigoProductoEnvasado(productoEnvasado));
        }
    }
}
