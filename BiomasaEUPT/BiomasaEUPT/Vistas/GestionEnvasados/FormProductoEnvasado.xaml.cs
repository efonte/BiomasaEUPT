using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiomasaEUPT.Vistas.GestionEnvasados
{
    /// <summary>
    /// Lógica de interacción para FormProductoEnvasado.xaml
    /// </summary>
    public partial class FormProductoEnvasado : UserControl
    {

        private CollectionViewSource productosEnvasadosViewSource;
        private CollectionViewSource tiposProductosTerminadosViewSource;
        private CollectionViewSource gruposProductosTerminadosViewSource;
        private CollectionViewSource tiposProductosEnvasadosViewSource;
        private CollectionViewSource gruposProductosEnvasadosViewSource;
        private CollectionViewSource pickingViewSource;
        public TipoProductoEnvasado TipoProductoEnvasado { get; set; }
        private FormProductoEnvasadoViewModel viewModel;

        private BiomasaEUPTContext context;

        public FormProductoEnvasado(BiomasaEUPTContext context)
        {
            InitializeComponent();
            viewModel = new FormProductoEnvasadoViewModel();
            DataContext = viewModel;
            this.context = context;
        }

        public FormProductoEnvasado(BiomasaEUPTContext context, ProductoEnvasado productoEnvasado) : this(context)
        {
            gbTitulo.Header = "Editar Producto Envasado";

            cbGruposProductosEnvasados.SelectedValue = productoEnvasado.TipoProductoEnvasado.GrupoProductoEnvasado.GrupoProductoEnvasadoId;
            cbTiposProductosEnvasados.SelectedValue = productoEnvasado.TipoProductoEnvasado.TipoProductoEnvasadoId;
            //cbGruposMateriasPrimas.SelectedValue = productoTerminado.TipoMateriaPrima.GrupoMateriaPrima.GrupoMateriaPrimaId;
            //cbTiposMateriasPrimas.SelectedValue = productoTerminado.TipoMateriaPrima.TipoMateriaPrimaId;

            viewModel.Observaciones = productoEnvasado.Observaciones;
            if (productoEnvasado.TipoProductoEnvasado.MedidoEnUnidades == true)
            {
                viewModel.Cantidad = productoEnvasado.Unidades.Value;
            }
            else
            {
                viewModel.Cantidad = productoEnvasado.Volumen.Value;
            }
            viewModel.ProductosEnvasadosComposiciones = new ObservableCollection<ProductoEnvasadoComposicion>(context.ProductosEnvasadosComposiciones.Where(ptc => ptc.ProductoId == productoEnvasado.ProductoEnvasadoId).ToList());
            CalcularCantidades();

            // Si ya se han envasado algún producto envasado con dicho producto terminado entonces los controles estarán deshabilitados
            if (context.ProductosEnvasadosComposiciones.Any(pec => pec.ProductoEnvasado.ProductoEnvasadoId == productoEnvasado.ProductoEnvasadoId))
            {
                cbGruposProductosTerminados.IsEnabled = false;
                cbTiposProductosTerminados.IsEnabled = false;
                cbPicking.IsEnabled = false;
                tbCantidad.IsEnabled = false;
                wpProductosEnvasadosComposiciones.IsEnabled = false;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            productosEnvasadosViewSource = ((CollectionViewSource)(FindResource("productosEnvasadosViewSource")));
            tiposProductosEnvasadosViewSource = ((CollectionViewSource)(FindResource("tiposProductosEnvasadosViewSource")));
            gruposProductosEnvasadosViewSource = ((CollectionViewSource)(FindResource("gruposProductosEnvasadosViewSource")));
            tiposProductosTerminadosViewSource = ((CollectionViewSource)(FindResource("tiposProductosTerminadosViewSource")));
            gruposProductosTerminadosViewSource = ((CollectionViewSource)(FindResource("gruposProductosTerminadosViewSource")));
            pickingViewSource = ((CollectionViewSource)(FindResource("pickingViewSource")));


            context.ProductosEnvasados.Load();
            context.TiposProductosEnvasados.Load();
            context.GruposProductosEnvasados.Load();
            context.Picking.Load();
            context.TiposProductosTerminados.Load();
            context.GruposProductosTerminados.Load();
            context.Picking.Load();

            productosEnvasadosViewSource.Source = context.ProductosEnvasados.Local;
            tiposProductosEnvasadosViewSource.Source = context.TiposProductosEnvasados.Local;
            gruposProductosEnvasadosViewSource.Source = context.GruposProductosEnvasados.Local;
            tiposProductosTerminadosViewSource.Source = context.TiposProductosTerminados.Local;
            gruposProductosTerminadosViewSource.Source = context.GruposProductosTerminados.Local;
            pickingViewSource.Source = context.Picking.Local;

            viewModel.PickingDisponible = new ObservableCollection<Picking>(context.Picking.ToList());
        }

        private void cbGruposProductosEnvasados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tiposProductosEnvasadosViewSource.Source = context.TiposProductosEnvasados.Where(d => d.GrupoId == ((GrupoProductoEnvasado)cbGruposProductosEnvasados.SelectedItem).GrupoProductoEnvasadoId).ToList();
        }

        private void cbGruposProductosTerminados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tiposProductosTerminadosViewSource.Source = context.TiposProductosTerminados.Where(d => d.GrupoId == ((GrupoProductoTerminado)cbGruposProductosTerminados.SelectedItem).GrupoProductoTerminadoId).ToList();
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
            CalcularCantidades();
        }

        private void cbTiposProductosTerminados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Se añaden todos los HistorialHuecosAlmacenajes que contienen tienen el TipoProductoTerminado seleccionado
            viewModel.HistorialHuecosAlmacenajesDisponibles = new ObservableCollection<HistorialHuecoAlmacenaje>(context.HistorialHuecosAlmacenajes.Where(hha => hha.ProductoTerminado.TipoId == ((TipoProductoTerminado)cbTiposProductosTerminados.SelectedItem).TipoProductoTerminadoId && (viewModel.TipoProductoTerminado.MedidoEnUnidades == true ? (hha.UnidadesRestantes > 0) : (hha.VolumenRestante > 0))).ToList());

            // Se borran los HistorialHuecosAlmacenajes que ya se han añadido (convertidos en ProductosEnvasadosComposiciones)
            viewModel.ProductosEnvasadosComposiciones.ToList().ForEach(pec => viewModel.HistorialHuecosAlmacenajesDisponibles.Remove(pec.HistorialHuecoAlmacenaje));
        }

        private void cbPicking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void lbHistorialHuecosAlmacenajes_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var parent = sender as ListBox;
            var historialHuecoAlmacenaje = GetDataFromListBox(lbHistorialHuecosAlmacenajes, e.GetPosition(parent)) as HistorialHuecoAlmacenaje;
            if (historialHuecoAlmacenaje != null)
            {
                DataObject dragData = new DataObject("HistorialHuecoAlmacenaje", historialHuecoAlmacenaje);
                DragDrop.DoDragDrop(parent, dragData, DragDropEffects.Move);
            }
        }

        private void spProductosEnvasadosComposiciones_Drop(object sender, DragEventArgs e)
        {
            var historialHuecoAlmacenaje = e.Data.GetData("HistorialHuecoAlmacenaje") as HistorialHuecoAlmacenaje;
            var productoEnvasadoComposicion = new ProductoEnvasadoComposicion() { HistorialHuecoAlmacenaje = historialHuecoAlmacenaje };
            viewModel.ProductosEnvasadosComposiciones.Add(productoEnvasadoComposicion);
            viewModel.HistorialHuecosAlmacenajesDisponibles.Remove(historialHuecoAlmacenaje);
        }

        private void cProductosEnvasadosComposiciones_DeleteClick(object sender, RoutedEventArgs e)
        {
            var chip = sender as Chip;
            int historialHuecoAlmacenajeId = int.Parse(chip.CommandParameter.ToString());
            var productoEnvasadoComposicion = viewModel.ProductosEnvasadosComposiciones.Single(pec => pec.HistorialHuecoAlmacenaje.HistorialHuecoAlmacenajeId == historialHuecoAlmacenajeId);
            viewModel.ProductosEnvasadosComposiciones.Remove(productoEnvasadoComposicion);
            if (productoEnvasadoComposicion.HistorialHuecoAlmacenaje.ProductoTerminado.TipoId == (cbTiposProductosTerminados.SelectedItem as TipoProductoTerminado).TipoProductoTerminadoId)
            {
                viewModel.HistorialHuecosAlmacenajesDisponibles.Add(productoEnvasadoComposicion.HistorialHuecoAlmacenaje);
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
                    viewModel.PickingDisponible = new ObservableCollection<Picking>(context.Picking.Where(p => p.UnidadesRestantes >= viewModel.Unidades).ToList());

                }
                else
                {
                    viewModel.Volumen = viewModel.Cantidad;
                    viewModel.PickingDisponible = new ObservableCollection<Picking>(context.Picking.Where(p => p.VolumenRestante >= viewModel.Volumen).ToList());

                }
                cbPicking.SelectedIndex = 0;
            }
            CalcularCantidades();
        }

        private void CalcularCantidades()
        {
            if (viewModel.TipoProductoTerminado != null && viewModel.TipoProductoTerminado.MedidoEnUnidades == true)
            {
                var unidadesRestantes = viewModel.Unidades;
                foreach (var p in viewModel.PickingDisponible)
                {
                    if (p.UnidadesTotales <= unidadesRestantes)
                    {
                        unidadesRestantes -= p.UnidadesTotales;
                        p.UnidadesTotales = p.UnidadesTotales;
                    }
                    else
                    {
                        p.UnidadesTotales = unidadesRestantes;
                        unidadesRestantes = 0;
                    }
                }
                viewModel.QuedaCantidadPorAlmacenar = unidadesRestantes > 0 || viewModel.Cantidad == 0;
            }
            else
            {
                var volumenRestante = viewModel.Volumen;
                foreach (var p in viewModel.PickingDisponible)
                {
                    if (p.VolumenTotal <= volumenRestante)
                    {
                        volumenRestante -= p.VolumenTotal;
                        p.VolumenTotal = p.VolumenTotal;
                    }
                    else
                    {
                        p.VolumenTotal = volumenRestante;
                        volumenRestante = 0;
                    }
                }
                viewModel.QuedaCantidadPorAlmacenar = volumenRestante > 0 || viewModel.Cantidad == 0;
            }
        }
    }
}
