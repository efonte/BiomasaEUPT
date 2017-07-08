using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Vistas.GestionRecepciones;
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
    /// Lógica de interacción para Paginacion.xaml
    /// </summary>
    public partial class Paginacion : UserControl
    {
        public Paginacion()
        {
            InitializeComponent();
            DataContext = new PaginacionViewSource();
        }

        private void cbCantidad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as PaginacionViewSource).CalcularItemsTotales();
            (DataContext as PaginacionViewSource).CargarItems();
        }
    }
}
