using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BiomasaEUPT.Vistas.GestionElaboraciones
{
    /// <summary>
    /// Lógica de interacción para TabElaboraciones.xaml
    /// </summary>
    public partial class TabElaboraciones : UserControl
    {
        private BiomasaEUPTContext context;
        private CollectionViewSource ordenesElaboracionesViewSource;

        public TabElaboraciones()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context = new BiomasaEUPTContext();
                ordenesElaboracionesViewSource = (CollectionViewSource)(FindResource("ordenesElaboracionesViewSource"));

                context.OrdenesElaboraciones.Load();

                ordenesElaboracionesViewSource.Source = context.OrdenesElaboraciones.Local;
            }
        }




    }
}
