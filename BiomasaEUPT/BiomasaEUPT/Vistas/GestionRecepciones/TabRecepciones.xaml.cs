using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica de interacción para TabRecepciones.xaml
    /// </summary>
    public partial class TabRecepciones : UserControl
    {
        private TabRecepcionesViewModel viewModel;
        public TabRecepciones()
        {
            InitializeComponent();
            viewModel = new TabRecepcionesViewModel();
            DataContext = viewModel;

            // Hacer doble clic en una fila del datagrid de recepcions hará que se ejecuta el evento RowRecepciones_DoubleClick
            Style rowStyleRecepciones = new Style(typeof(DataGridRow), (Style)TryFindResource(typeof(DataGridRow)));
            rowStyleRecepciones.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler((s, e1) => { viewModel.ModificarRecepcion(); })));
            ucTablaRecepciones.dgRecepciones.RowStyle = rowStyleRecepciones;

            // Hacer doble clic en una fila del datagrid de materias primas hará que se ejecuta el evento RowMateriasPrimas_DoubleClick
            Style rowStyleMateriasPrimas = new Style(typeof(DataGridRow), (Style)TryFindResource(typeof(DataGridRow)));
            rowStyleMateriasPrimas.Setters.Add(new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler((s, e1) => { viewModel.ModificarMateriaPrima(); })));
            ucTablaMateriasPrimas.dgMateriasPrimas.RowStyle = rowStyleMateriasPrimas;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ucTablaRecepciones.ucPaginacion.DataContext = viewModel.PaginacionViewModel;
            ucMasOpcionesRecepciones.DataContext = viewModel.MasOpcionesRecepcionesViewModel;
        }

        private void bMasOpciones_Click(object sender, RoutedEventArgs e)
        {
            transicion.SelectedIndex = 1;
            viewModel.MasOpcionesRecepcionesViewModel.Inicializar();
        }

        private void bVolver_Click(object sender, RoutedEventArgs e)
        {
            transicion.SelectedIndex = 0;
            viewModel.Inicializar();
        }
    }
}
