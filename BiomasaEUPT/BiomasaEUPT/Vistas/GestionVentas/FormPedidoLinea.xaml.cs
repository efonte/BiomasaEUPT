using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
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

namespace BiomasaEUPT.Vistas.GestionVentas
{
    /// <summary>
    /// Lógica de interacción para FormPedidoLinea.xaml
    /// </summary>
    public partial class FormPedidoLinea : UserControl
    {
        private CollectionViewSource pedidosLineasViewSource;
        private CollectionViewSource tiposProductosEnvasadosViewSource;
        private CollectionViewSource gruposProductosEnvasadosViewSource;

        private FormPedidoLineaViewModel viewModel;
        private BiomasaEUPTContext context;


        public FormPedidoLinea(BiomasaEUPTContext context)
        {
            InitializeComponent();
            viewModel = new FormPedidoLineaViewModel();
            Console.WriteLine("ViewModel vale " + viewModel);
            DataContext = viewModel;
            this.context = context;
        }

        public FormPedidoLinea(BiomasaEUPTContext context, string _titulo) : this(context)
        {
            gbTitulo.Header = _titulo;

        }

        public FormPedidoLinea(BiomasaEUPTContext context, PedidoLinea pedidoLinea) : this(context)
        {
            gbTitulo.Header = "Editar pedidoLinea";

            cbGruposProductosEnvasados.SelectedValue = pedidoLinea.TipoProductoEnvasado.GrupoProductoEnvasado.GrupoProductoEnvasadoId;
            cbTiposProductosEnvasados.SelectedValue = pedidoLinea.TipoProductoEnvasado.TipoProductoEnvasadoId;

            if (pedidoLinea.TipoProductoEnvasado.MedidoEnUnidades == true)
            {
                viewModel.Cantidad = pedidoLinea.Unidades.Value;
            }
            else
            {
                viewModel.Cantidad = pedidoLinea.Volumen.Value;
            }

        }

            private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            pedidosLineasViewSource = ((CollectionViewSource)(FindResource("pedidosLineasViewSource")));
            gruposProductosEnvasadosViewSource = ((CollectionViewSource)(FindResource("gruposProductosEnvasadosViewSource")));
            tiposProductosEnvasadosViewSource = ((CollectionViewSource)(FindResource("tiposProductosEnvasadosViewSource")));
            
            context.PedidosLineas.Load();
            context.GruposProductosEnvasados.Load();
            context.TiposProductosEnvasados.Load();
            
            pedidosLineasViewSource.Source = context.PedidosLineas.Local;
            gruposProductosEnvasadosViewSource.Source = context.GruposProductosEnvasados.Local;
            tiposProductosEnvasadosViewSource.Source = context.TiposProductosEnvasados.Local;
            Console.WriteLine("Tipo PE " + tiposProductosEnvasadosViewSource);
            

        }

        private void cbGruposProductosEnvasados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tiposProductosEnvasadosViewSource.Source = context.TiposProductosEnvasados.Where(d => d.GrupoId == ((GrupoProductoEnvasado)cbGruposProductosEnvasados.SelectedItem).GrupoProductoEnvasadoId).ToList();
        }

        private void cbTiposProductosEnvasados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel.TipoProductoEnvasado.MedidoEnUnidades == true)
            {
                viewModel.CantidadHint = "Cantidad (ud.)";
                viewModel.Unidades = Convert.ToInt32(viewModel.Cantidad);
                viewModel.Volumen = null;
            }
            else
            {
                viewModel.CantidadHint = "Cantidad (m³)";
                viewModel.Volumen = viewModel.Cantidad;
                viewModel.Unidades = null;
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

        private void tbCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewModel.TipoProductoEnvasado != null)
            {
                if (viewModel.TipoProductoEnvasado.MedidoEnUnidades == true)
                {
                    viewModel.Unidades = Convert.ToInt32(viewModel.Cantidad);
                    //viewModel.PickingDisponible = new ObservableCollection<Picking>(context.Picking.Where(p => p.UnidadesRestantes >= viewModel.Unidades).ToList());

                }
                else
                {
                    viewModel.Volumen = viewModel.Cantidad;
                    //viewModel.PickingDisponible = new ObservableCollection<Picking>(context.Picking.Where(p => p.VolumenRestante >= viewModel.Volumen).ToList());

                }
                //cbPicking.SelectedIndex = 0;
            }
        }
    }
}
