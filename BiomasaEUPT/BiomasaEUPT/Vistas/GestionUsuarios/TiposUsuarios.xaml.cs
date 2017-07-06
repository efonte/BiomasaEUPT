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

namespace BiomasaEUPT.Vistas.GestionUsuarios
{
    /// <summary>
    /// Lógica de interacción para TiposUsuarios.xaml
    /// </summary>
    public partial class TiposUsuarios : UserControl
    {
        private bool primaraVez = false;
        // private BiomasaEUPTContext context;

        public TiposUsuarios()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //  context = new BiomasaEUPTContext();

            DependencyObject ucParent = Parent;

            while (!(ucParent is UserControl))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }

            // Pestaña Usuarios
            if (ucParent.GetType().Equals(typeof(TabUsuarios)))
            {
                TabUsuarios tabUsuarios = (TabUsuarios)ucParent;
                CollectionViewSource tiposUsuariosViewSource = ((CollectionViewSource)(tabUsuarios.ucTablaUsuarios.FindResource("tiposUsuariosViewSource")));
                if (tiposUsuariosViewSource.View != null)
                {
                    foreach (TipoUsuario tipoUsuario in tiposUsuariosViewSource.View)
                    {
                        lbTiposUsuarios.Items.Add(new ListBoxItem() { Content = tipoUsuario.Nombre, FontSize = 14 });
                    }
                    /*foreach (var tipoUsuario in context.tiposUsuarios.ToList())
                    {
                        lbTiposUsuarios.Items.Add(new ListBoxItem() { Content = tipoUsuario.Nombre, FontSize = 14 });
                    }*/
                }
            }

        }

        private void lbTiposUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (primaraVez) // Se ejecuta nada más cargar la vista. Con el booleano se evita que se ejecute la primera vez.
            {
                DependencyObject ucParent = Parent;

                while (!(ucParent is UserControl))
                {
                    ucParent = LogicalTreeHelper.GetParent(ucParent);
                }

                // Pestaña Usuarios
                if (ucParent.GetType().Equals(typeof(TabUsuarios)))
                {
                    TabUsuarios tabUsuarios = (TabUsuarios)ucParent;
                    tabUsuarios.FiltrarTabla();
                }

            }
            else
            {
                primaraVez = true;
            }
        }

    }
}
