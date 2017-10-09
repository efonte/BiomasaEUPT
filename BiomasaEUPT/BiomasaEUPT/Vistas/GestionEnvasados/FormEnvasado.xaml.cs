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

namespace BiomasaEUPT.Vistas.GestionEnvasados
{
    /// <summary>
    /// Lógica de interacción para FormOrdenEnvasado.xaml
    /// </summary>
    public partial class FormEnvasado : UserControl
    {

        private CollectionViewSource ordenesEnvasadosViewSource;
        private CollectionViewSource estadosEnvasadosViewSource;
        private CollectionViewSource gruposProductosEnvasadosViewSource;
        public ObservableCollection<TipoProductoEnvasado> TiposProductosEnvasadosDisponibles { get; set; }
        public ObservableCollection<ProductoEnvasado> ProductosEnvasados { get; set; }

        public String Descripcion { get; set; }
        private BiomasaEUPTContext context;

        public FormEnvasado(BiomasaEUPTContext context)
        {
            InitializeComponent();
            DataContext = this;
            Descripcion = this.Descripcion;
            this.context = context;
            TiposProductosEnvasadosDisponibles = new ObservableCollection<TipoProductoEnvasado>();
            ProductosEnvasados = new ObservableCollection<ProductoEnvasado>();
        }

        public FormEnvasado(BiomasaEUPTContext context, string _titulo) : this(context)
        {
            gbTitulo.Header = _titulo;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ordenesEnvasadosViewSource = ((CollectionViewSource)(FindResource("ordenesEnvasadosViewSource")));
            estadosEnvasadosViewSource = ((CollectionViewSource)(FindResource("estadosEnvasadosViewSource")));
            gruposProductosEnvasadosViewSource = ((CollectionViewSource)(FindResource("gruposProductosEnvasadosViewSource")));

            context.OrdenesEnvasados.Load();
            context.EstadosEnvasados.Load();
            context.GruposProductosEnvasados.Load();

            ordenesEnvasadosViewSource.Source = context.OrdenesEnvasados.Local;
            estadosEnvasadosViewSource.Source = context.EstadosEnvasados.Local;
            gruposProductosEnvasadosViewSource.Source = context.GruposProductosEnvasados.Local;


        }

        private void cbGruposProductosEnvasados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TiposProductosEnvasadosDisponibles.Clear();

            // Se añaden todos los TiposProductosEnvasados del GrupoProductoEnvasados seleccionado
            context.TiposProductosEnvasados.Where(tpe => tpe.GrupoId == ((GrupoProductoEnvasado)cbGruposProductosEnvasados.SelectedItem).GrupoProductoEnvasadoId).ToList().ForEach(TiposProductosEnvasadosDisponibles.Add);

            // Se borran los TiposProductosEnvasados que ya se han añadido
            ProductosEnvasados.ToList().ForEach(pe => TiposProductosEnvasadosDisponibles.Remove(pe.TipoProductoEnvasado));
        }

        private void lbTiposProductosEnvasados_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var parent = sender as ListBox;
            var tipoProductoEnvasado = GetDataFromListBox(lbTiposProductosEnvasados, e.GetPosition(parent)) as TipoProductoEnvasado;
            if (tipoProductoEnvasado != null)
            {
                DataObject dragData = new DataObject("TipoProductoEnvasado", tipoProductoEnvasado);
                DragDrop.DoDragDrop(parent, dragData, DragDropEffects.Move);
            }
        }

        private object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }

        private void spProductosEnvasados_Drop(object sender, DragEventArgs e)
        {
            var tipoProductoEnvasado = e.Data.GetData("TipoProductoEnvasado") as TipoProductoEnvasado;
            var productoEnvasado = new ProductoEnvasado() { TipoProductoEnvasado = tipoProductoEnvasado };
            ProductosEnvasados.Add(productoEnvasado);
            TiposProductosEnvasadosDisponibles.Remove(tipoProductoEnvasado);
        }

        private void cProductoEnvasado_DeleteClick(object sender, RoutedEventArgs e)
        {
            var chip = sender as Chip;
            int tipoProductoEnvasadoId = int.Parse(chip.CommandParameter.ToString());
            ProductoEnvasado productoEnvasado = ProductosEnvasados.Single(pe => pe.TipoProductoEnvasado.TipoProductoEnvasadoId == tipoProductoEnvasadoId);
            ProductosEnvasados.Remove(productoEnvasado);
            if (productoEnvasado.TipoProductoEnvasado.GrupoProductoEnvasado.GrupoProductoEnvasadoId == (cbGruposProductosEnvasados.SelectedItem as GrupoProductoEnvasado).GrupoProductoEnvasadoId)
            {
                TiposProductosEnvasadosDisponibles.Add(productoEnvasado.TipoProductoEnvasado);
            }
        }

        private void bAnadir_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
