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
        public ObservableCollection<int> ItemsPorPagina { get; }
        public int CantidadItems { get; set; } = 0;
        public int CantidadItemsTotales { get; set; } = 0;

        public Paginacion()
        {
            InitializeComponent();
            DataContext = this;
            ItemsPorPagina = new ObservableCollection<int>() { 10, 20, 30, 50, 100 };
        }
    }
}
