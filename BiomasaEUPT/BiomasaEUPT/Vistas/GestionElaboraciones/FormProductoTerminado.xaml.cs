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

namespace BiomasaEUPT.Vistas.GestionElaboraciones
{
    /// <summary>
    /// Lógica de interacción para FormProductoTerminado.xaml
    /// </summary>
    public partial class FormProductoTerminado : UserControl
    {

        private CollectionViewSource productosTerminadosViewSource;
        private CollectionViewSource tiposProductosTerminadosViewSource;
        private CollectionViewSource gruposProductosTerminadosViewSource;

        public DateTime? FechaBaja { get; set; }
        public DateTime? HoraBaja { get; set; }
        public String Observaciones { get; set; }
        public int Unidades { get; set; }
        public double Volumen { get; set; }
        private BiomasaEUPTContext context;


        public FormProductoTerminado(BiomasaEUPTContext context)
        {
            InitializeComponent();
            DataContext = this;
            this.context = context;
            Observaciones = this.Observaciones;
            this.context = context;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            productosTerminadosViewSource = ((CollectionViewSource)(FindResource("productosTerminadosViewSource")));
            tiposProductosTerminadosViewSource = ((CollectionViewSource)(FindResource("tiposProductosTerminadosViewSource")));
            gruposProductosTerminadosViewSource = ((CollectionViewSource)(FindResource("gruposProductosTerminadosViewSource")));

            context.ProductosTerminados.Load();
            context.TiposProductosTerminados.Load();
            context.GruposProductosTerminados.Load();

            productosTerminadosViewSource.Source = context.ProductosTerminados.Local;
            tiposProductosTerminadosViewSource.Source = context.TiposProductosTerminados.Local;
            gruposProductosTerminadosViewSource.Source = context.GruposProductosTerminados.Local;

            dpFechaBaja.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
        }

        private void cbGruposProductosTerminados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
