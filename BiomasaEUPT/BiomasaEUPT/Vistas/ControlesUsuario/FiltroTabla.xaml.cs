using BiomasaEUPT.Clases;
using BiomasaEUPT.Vistas.GestionClientes;
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

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para FiltroTabla.xaml
    /// </summary>
    public partial class FiltroTabla : UserControl
    {
        private BiomasaEUPTEntidades context;
        private DependencyObject ucParent;
        public FiltroTabla()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = BaseDeDatos.Instancia.biomasaEUPTEntidades;
            ucParent = Parent;
            //lbFiltro.SelectAll();
            while (!(ucParent is UserControl))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }

            // Pestaña Clientes
            if (ucParent.GetType().Equals(typeof(TabClientes)))
            {
                TabClientes tabClientes = (TabClientes)ucParent;
                CollectionViewSource tiposClientesViewSource = ((CollectionViewSource)(tabClientes.ucTablaClientes.FindResource("tipos_clientesViewSource")));
                ccFiltro.Collection = tiposClientesViewSource.View;
                //   tabClientes.FiltrarTabla();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Pestaña Clientes
            if (ucParent.GetType().Equals(typeof(TabClientes)))
            {
                TabClientes tabClientes = (TabClientes)ucParent;
                CollectionViewSource tiposClientesViewSource = ((CollectionViewSource)(tabClientes.ucTablaClientes.FindResource("tipos_clientesViewSource")));
                Console.WriteLine("asasasasa");
                //   context.tipos_clientes.Add(new tipos_clientes() { nombre = "aa", descripcion = "bb" });
            }

        }

    }
}
