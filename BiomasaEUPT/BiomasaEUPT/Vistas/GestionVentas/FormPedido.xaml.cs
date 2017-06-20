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
    /// Lógica de interacción para FormPedido.xaml
    /// </summary>
    public partial class FormPedido : UserControl
    {

        private CollectionViewSource pedidosViewSource;
        private CollectionViewSource estadosPedidosViewSource;
        private CollectionViewSource clientesViewSource;

        public DateTime FechaPedido { get; set; }
        public DateTime HoraPedido { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public DateTime HoraFinalizacion { get; set; }
        private BiomasaEUPTContext context;

        public FormPedido(BiomasaEUPTContext context)
        {
            InitializeComponent();
            DataContext = this;
            FechaPedido = DateTime.Now;
            HoraPedido = DateTime.Now;
            FechaFinalizacion = DateTime.Now;
            HoraFinalizacion = DateTime.Now;
            this.context = context;
        }

        public FormPedido(BiomasaEUPTContext context, string _titulo) : this(context)
        {
            gbTitulo.Header = _titulo;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            pedidosViewSource = ((CollectionViewSource)(FindResource("pedidosViewSource")));
            estadosPedidosViewSource = ((CollectionViewSource)(FindResource("estadosPedidosViewSource")));
            clientesViewSource = ((CollectionViewSource)(FindResource("clientesViewSource")));

            context.PedidosCabeceras.Load();
            context.EstadosPedidos.Load();
            context.Clientes.Load();

            pedidosViewSource.Source = context.PedidosCabeceras.Local;
            estadosPedidosViewSource.Source = context.EstadosPedidos.Local;
            clientesViewSource.Source = context.Clientes.Local;

            dpFechaPedido.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
            dpFechaFinalizacion.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
        }
    }
}
