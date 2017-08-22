using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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
    /// Lógica de interacción para FormProductoEnvasado.xaml
    /// </summary>
    public partial class FormProductoEnvasado : UserControl
    {

        private CollectionViewSource productosEnvasadosViewSource;
        private CollectionViewSource tiposProductosTerminadosViewSource;
        private CollectionViewSource pickingViewSource;

        public DateTime FechaBaja { get; set; }
        public DateTime HoraBaja { get; set; }
        private BiomasaEUPTContext context;

        public FormProductoEnvasado(BiomasaEUPTContext context)
        {
            InitializeComponent();
            DataContext = this;
            FechaBaja = DateTime.Now;
            HoraBaja = DateTime.Now;
            this.context = context;
        }

        public FormProductoEnvasado(BiomasaEUPTContext context, ProductoEnvasado productoEnvasado) : this(context)
        {
            gbTitulo.Header = "Editar Producto Envasado";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            productosEnvasadosViewSource = ((CollectionViewSource)(FindResource("productosEnvasadosViewSource")));
            tiposProductosTerminadosViewSource = ((CollectionViewSource)(FindResource("tiposProductosTerminadosViewSource")));
            pickingViewSource = ((CollectionViewSource)(FindResource("pickingViewSource")));

            context.ProductosEnvasados.Load();
            context.TiposProductosTerminados.Load();
            context.Picking.Load();

            productosEnvasadosViewSource.Source = context.ProductosEnvasados.Local;
            tiposProductosTerminadosViewSource.Source = context.TiposProductosTerminados.Local;
            pickingViewSource.Source = context.Picking.Local;

            dpFechaBaja.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
        }
    }
}
