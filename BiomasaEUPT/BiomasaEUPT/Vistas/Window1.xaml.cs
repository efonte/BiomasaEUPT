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

namespace BiomasaEUPT.Vistas
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        BiomasaEUPTDataSet biomasaEUPTDataSet;
        BiomasaEUPTDataSetTableAdapters.usuariosTableAdapter biomasaEUPTDataSetusuariosTableAdapter;
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            biomasaEUPTDataSet = ((BiomasaEUPTDataSet)(FindResource("biomasaEUPTDataSet")));
            // Cargar datos en la tabla usuarios. Puede modificar este código según sea necesario.
            biomasaEUPTDataSetusuariosTableAdapter = new BiomasaEUPTDataSetTableAdapters.usuariosTableAdapter();
            biomasaEUPTDataSetusuariosTableAdapter.Fill(biomasaEUPTDataSet.usuarios);
            CollectionViewSource usuariosViewSource = ((CollectionViewSource)(FindResource("usuariosViewSource")));
            usuariosViewSource.View.MoveCurrentToFirst();

            // Cargar datos en la tabla tipos_usuarios. Puede modificar este código según sea necesario.
            BiomasaEUPTDataSetTableAdapters.tipos_usuariosTableAdapter biomasaEUPTDataSettipos_usuariosTableAdapter = new BiomasaEUPTDataSetTableAdapters.tipos_usuariosTableAdapter();
            biomasaEUPTDataSettipos_usuariosTableAdapter.Fill(biomasaEUPTDataSet.tipos_usuarios);
            CollectionViewSource tipos_usuariosViewSource = ((CollectionViewSource)(FindResource("tipos_usuariosViewSource")));
            tipos_usuariosViewSource.View.MoveCurrentToFirst();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //biomasaEUPTDataSet.AcceptChanges();
            //biomasaEUPTDataSetusuariosTableAdapter.Fill(biomasaEUPTDataSet.usuarios);
            biomasaEUPTDataSetusuariosTableAdapter.Update(biomasaEUPTDataSet.usuarios);
        }
    }
}
