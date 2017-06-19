using BiomasaEUPT.Clases;
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

namespace BiomasaEUPT.Vistas.GestionTrazabilidad
{
    /// <summary>
    /// Lógica de interacción para TablaTrazabilidad.xaml
    /// </summary>
    public partial class TablaTrazabilidad : UserControl
    {
        public TablaTrazabilidad()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InformePDF informe = new InformePDF("1000000001", @"C:\Users\usuario\Desktop\qqqwwweee\");
            System.Diagnostics.Process.Start(informe.GenerarPDF());
            // informe.CreatePDF();
        }
    }
}
