using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
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
        private TabElaboracionesViewModel viewModel;
        public TabElaboraciones()
        {
            InitializeComponent();
            viewModel = new TabElaboracionesViewModel();
            DataContext = viewModel;

            // Hacer doble clic en una fila del datagrid de elaboraciones hará que se ejecuta el evento RowElaboraciones_DoubleClick
            Style rowStyleElaboraciones = new Style(typeof(DataGridRow), (Style)TryFindResource(typeof(DataGridRow)));
            rowStyleElaboraciones.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler((s, e1) => { viewModel.ModificarOrdenElaboracion(); })));
            ucTablaElaboraciones.dgElaboraciones.RowStyle = rowStyleElaboraciones;

            // Hacer doble clic en una fila del datagrid de productos terminados hará que se ejecuta el evento RowMateriasPrimas_DoubleClick
            Style rowStyleProductosTerminados = new Style(typeof(DataGridRow), (Style)TryFindResource(typeof(DataGridRow)));
            rowStyleProductosTerminados.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler((s, e1) => { viewModel.ModificarProductoTerminado(); })));
            ucTablaProductosTerminados.dgProductosTerminados.RowStyle = rowStyleProductosTerminados;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ucTablaElaboraciones.ucPaginacion.DataContext = viewModel.PaginacionViewModel;
            ucMasOpcionesElaboraciones.DataContext = viewModel.MasOpcionesElaboracionesViewModel;
        }

        private void bMasOpciones_Click(object sender, RoutedEventArgs e)
        {
            transicion.SelectedIndex = 1;
            viewModel.MasOpcionesElaboracionesViewModel.Inicializar();
        }

        private void bVolver_Click(object sender, RoutedEventArgs e)
        {
            transicion.SelectedIndex = 0;
            viewModel.Inicializar();
        }





    }
}
