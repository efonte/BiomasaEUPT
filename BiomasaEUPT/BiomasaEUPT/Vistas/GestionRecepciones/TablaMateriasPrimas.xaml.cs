using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para TablaMateriasPrimas.xaml
    /// </summary>
    public partial class TablaMateriasPrimas : UserControl
    {
        private Trazabilidad trazabilidad;

        public TablaMateriasPrimas()
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

            tabRecepciones.FiltrarTablaMateriasPrimas();
        }

        private void bPdfMateria_Click(object sender, RoutedEventArgs e)
        {
            /*for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility =
                    row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }*/
            MateriaPrima materiaPrima = (sender as Button).DataContext as MateriaPrima;

            InformePDF informe = new InformePDF(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Informes\");
            System.Diagnostics.Process.Start(informe.GenerarPDFMateriaPrima(trazabilidad.MateriaPrima(materiaPrima.Codigo)));

        }

        private void bCodigo_Click(object sender, RoutedEventArgs e)
        {
            MateriaPrima materiaPrima = (sender as Button).DataContext as MateriaPrima;

            System.Diagnostics.Process.Start(new InformePDF().ImprimirCodigoMateriaPrima(materiaPrima));
            /*  var visorPDF = new VisorPDFCodigos(new InformePDF().ImprimirCodigoMateriaPrima(materiaPrima));
              var resultado = await DialogHost.Show(visorPDF, "RootDialog");*/
        }
    }
}
