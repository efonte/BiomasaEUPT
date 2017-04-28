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
using System.Windows.Shapes;

namespace BiomasaEUPT.Vistas
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        BiomasaEUPTEntidades context = new BiomasaEUPTEntidades();
        CollectionViewSource usuariosViewSource;
        CollectionViewSource tiposUsuariosViewSource;

        public Window1()
        {
            InitializeComponent();

            usuariosViewSource = ((CollectionViewSource)(FindResource("usuariosViewSource")));
            tiposUsuariosViewSource = ((CollectionViewSource)(FindResource("tipos_usuariosViewSource")));
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // System.Windows.Data.CollectionViewSource usuariosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("usuariosViewSource")));
            // Cargar datos estableciendo la propiedad CollectionViewSource.Source:
            // usuariosViewSource.Source = [origen de datos genérico]

           // System.Windows.Data.CollectionViewSource tipos_usuariosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tipos_usuariosViewSource")));
            // Cargar datos estableciendo la propiedad CollectionViewSource.Source:
            // tipos_usuariosViewSource.Source = [origen de datos genérico]


            // Load is an extension method on IQueryable,    
            // defined in the System.Data.Entity namespace.   
            // This method enumerates the results of the query,    
            // similar to ToList but without creating a list.   
            // When used with Linq to Entities this method    
            // creates entity objects and adds them to the context.   
            context.usuarios.Load();
            context.tipos_usuarios.Load();

            // After the data is loaded call the DbSet<T>.Local property    
            // to use the DbSet<T> as a binding source.   
            usuariosViewSource.Source = context.usuarios.Local;
            tiposUsuariosViewSource.Source = context.tipos_usuarios.Local;
        }

     

    }
}
