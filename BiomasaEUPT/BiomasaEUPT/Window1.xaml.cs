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
using System.Windows.Shapes;

namespace BiomasaEUPT
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            BiomasaEUPT.BiomasaEUPTDataSet biomasaEUPTDataSet = ((BiomasaEUPT.BiomasaEUPTDataSet)(this.FindResource("biomasaEUPTDataSet")));
            // Cargar datos en la tabla vista_usuarios. Puede modificar este código según sea necesario.
            BiomasaEUPT.BiomasaEUPTDataSetTableAdapters.vista_usuariosTableAdapter biomasaEUPTDataSetvista_usuariosTableAdapter = new BiomasaEUPT.BiomasaEUPTDataSetTableAdapters.vista_usuariosTableAdapter();
            biomasaEUPTDataSetvista_usuariosTableAdapter.Fill(biomasaEUPTDataSet.vista_usuarios);
            System.Windows.Data.CollectionViewSource vista_usuariosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("vista_usuariosViewSource")));
            vista_usuariosViewSource.View.MoveCurrentToFirst();
        }
    }
}
