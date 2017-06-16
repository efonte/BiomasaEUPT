using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
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

namespace BiomasaEUPT.Vistas.GestionRecepciones
{
    /// <summary>
    /// Lógica de interacción para TablaMateriasPrimas.xaml
    /// </summary>
    public partial class TablaMateriasPrimas : UserControl
    {
        //public ObservableCollection<MateriaPrima> MateriasPrimas { get; set; }

        public TablaMateriasPrimas()
        {
            InitializeComponent();
            /*MateriasPrimas = new ObservableCollection<MateriaPrima>();
            using (var context=new BiomasaEUPTContext())
            {
                context.MateriasPrimas.Where(mp => mp.RecepcionId == 1).ToList().ForEach(MateriasPrimas.Add);
            }*/
            DataContext = this;
        }

        private void tbBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            DependencyObject ucParent = Parent;

            while (!(ucParent is UserControl))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }

            TabRecepciones tabRecepciones = (TabRecepciones)ucParent;

            tabRecepciones.FiltrarTablaMateriasPrimas();
        }
    }
}
