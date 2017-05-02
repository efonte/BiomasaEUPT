using BiomasaEUPT.Clases;
using MaterialDesignThemes.Wpf;
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
    /// Lógica de interacción para OpcionesUsuarios.xaml
    /// </summary>
    public partial class OpcionesUsuarios : UserControl
    {
        // private BiomasaEUPTEntidades context;
        public OpcionesUsuarios()
        {
            InitializeComponent();
            // DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // context = BaseDeDatos.Instancia.biomasaEUPTEntidades;
        }

    }
}
