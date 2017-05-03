using BiomasaEUPT.Clases;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace BiomasaEUPT.Vistas.GestionClientes
{
    /// <summary>
    /// Lógica de interacción para TabClientes.xaml
    /// </summary>
    public partial class TabClientes : UserControl
    {
        private BiomasaEUPTEntidades context;
        private CollectionViewSource clientesViewSource;
        private CollectionViewSource tiposClientesViewSource;
        private CollectionViewSource gruposClientesViewSource;

        public TabClientes()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = BaseDeDatos.Instancia.biomasaEUPTEntidades;
                clientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("clientesViewSource")));
                tiposClientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("tipos_clientesViewSource")));
                gruposClientesViewSource = ((CollectionViewSource)(ucTablaClientes.FindResource("grupos_clientesViewSource")));
                context.clientes.Load();
                context.tipos_clientes.Load();
                context.grupos_clientes.Load();
                clientesViewSource.Source = context.clientes.Local;
                tiposClientesViewSource.Source = context.tipos_clientes.Local;
                gruposClientesViewSource.Source = context.grupos_clientes.Local;
            }
        }
    }
}
