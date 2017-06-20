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
    /// Lógica de interacción para FormElaboracion.xaml
    /// </summary>
    public partial class FormElaboracion : UserControl
    {

        private CollectionViewSource ordenesElaboracionesViewSource;
        private CollectionViewSource estadosElaboracionesViewSource;


        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public String Descripcion { get; set; }
        private BiomasaEUPTContext context;


        public FormElaboracion(BiomasaEUPTContext context)
        {
            InitializeComponent();
            DataContext = this;
            Fecha = DateTime.Now;
            Hora = DateTime.Now;
            Descripcion = this.Descripcion;
            this.context = context;
        }

        public FormElaboracion(BiomasaEUPTContext context, string _titulo) : this(context)
        {
            gbTitulo.Header = _titulo;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ordenesElaboracionesViewSource = ((CollectionViewSource)(FindResource("ordenesElaboracionesViewSource")));
            estadosElaboracionesViewSource = ((CollectionViewSource)(FindResource("estadosElaboracionesViewSource")));
            context.OrdenesElaboraciones.Load();
            context.EstadosElaboraciones.Load();
            ordenesElaboracionesViewSource.Source = context.OrdenesElaboraciones.Local;
            estadosElaboracionesViewSource.Source = context.EstadosElaboraciones.Local;

            dpFechaElaboracion.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
        }
    }
}
