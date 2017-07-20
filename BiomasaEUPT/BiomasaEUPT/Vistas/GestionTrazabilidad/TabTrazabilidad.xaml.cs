using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace BiomasaEUPT.Vistas.GestionTrazabilidad
{
    /// <summary>
    /// Lógica de interacción para TabTrazabilidad.xaml
    /// </summary>
    public partial class TabTrazabilidad : UserControl
    {
        public TabTrazabilidad()
        {
            InitializeComponent();
            DataContext = new TabTrazabilidadViewModel();

            /* NO FUNCIONA
            var bindingTvArbol = new Binding()
            {
                Path = new PropertyPath("ItemArbolSeleccionado"),
                Source = DataContext,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(tvArbol, Selector.SelectedItemProperty, bindingTvArbol);*/
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
