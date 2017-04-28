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

        public TiposUsuarios()
        {
            InitializeComponent();
        }

        private void lbTiposUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (primaraVez) // Se ejecuta nada más cargar la vista. Con el booleano se evita que se ejecute la primera vez.
            {
                Console.WriteLine("----");
                DependencyObject ucParent = Parent;

                while ((ucParent is null))
                {
                    ucParent = LogicalTreeHelper.GetParent(ucParent);
                }

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
