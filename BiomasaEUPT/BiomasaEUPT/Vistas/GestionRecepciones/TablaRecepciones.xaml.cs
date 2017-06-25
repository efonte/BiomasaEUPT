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

namespace BiomasaEUPT.Vistas.GestionRecepciones
{
    /// <summary>
    /// Lógica de interacción para TablaRecepciones.xaml
    /// </summary>
    public partial class TablaRecepciones : UserControl
    {
        private Trazabilidad trazabilidad;

        public TablaRecepciones()
        {
            InitializeComponent();
            trazabilidad = new Trazabilidad();

        }

        private void tbBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            DependencyObject ucParent = Parent;

            while (!(ucParent is UserControl))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }

            TabRecepciones tabRecepciones = (TabRecepciones)ucParent;

            tabRecepciones.FiltrarTablaRecepciones();
        }

        private void bPdfRecepcion_Click(object sender, RoutedEventArgs e)
        {
            Recepcion recepcion = (sender as Button).DataContext as Recepcion;

            InformePDF informe = new InformePDF(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Informes\");
            System.Diagnostics.Process.Start(informe.GenerarPDFMateriaPrima(trazabilidad.Recepcion(recepcion.NumeroAlbaran)));
        }
    }
}
