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

namespace BiomasaEUPT.Vistas.GestionRecepciones
{
    /// <summary>
    /// Lógica de interacción para FormRecepcion.xaml
    /// </summary>
    public partial class FormRecepcion : UserControl
    {
        private CollectionViewSource proveedoresViewSource;
        private CollectionViewSource estadosRecepcionesViewSource;

        public String NumeroAlbaran { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        private BiomasaEUPTContext context;


        public FormRecepcion(BiomasaEUPTContext context)
        {
            InitializeComponent();
            DataContext = this;
            Fecha = DateTime.Now;
            Hora = DateTime.Now;
            this.context = context;
        }

        public FormRecepcion(BiomasaEUPTContext context, string _titulo) : this(context)
        {
            gbTitulo.Header = _titulo;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            proveedoresViewSource = ((CollectionViewSource)(FindResource("proveedoresViewSource")));
            estadosRecepcionesViewSource = ((CollectionViewSource)(FindResource("estadosRecepcionesViewSource")));
            context.Proveedores.Load();
            context.EstadosRecepciones.Load();
            proveedoresViewSource.Source = context.Proveedores.Local;
            estadosRecepcionesViewSource.Source = context.EstadosRecepciones.Local;

            dpFechaRecepcion.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
        }
    }
}
