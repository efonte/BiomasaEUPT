using System;
using System.Collections.Generic;
using System.IO;
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

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para VisorPDFCodigos.xaml
    /// </summary>
    public partial class VisorPDFCodigos : UserControl
    {
        WebBrowser browser = new WebBrowser();

        public VisorPDFCodigos(MemoryStream memoryStream)
        {
            InitializeComponent();
            gVisor.Children.Add(browser);
              browser.Navigate(@"C:\Users\usuario\Desktop\Informes\Materia Prima #1000000002 23-06-2017 21-27-48.pdf");
            //  browser.NavigateToStream(memoryStream);
            gVisor.Opacity = 200;
        }
    }
}
