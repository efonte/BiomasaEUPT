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

namespace BiomasaEUPT.Vistas.GestionVentas
{
    /// <summary>
    /// Lógica de interacción para FormProductoEnvasado.xaml
    /// </summary>
    public partial class FormProductoEnvasado : UserControl
    {

        private CollectionViewSource productosEnvasadosViewSource;
        private CollectionViewSource pickingViewSource;
        private CollectionViewSource pedidoDetalleViewSource;
        private CollectionViewSource productoTerminadoViewSource;
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

            viewModel.Observaciones = productoEnvasado.Observaciones;
            viewModel.ProductosEnvasadosComposiciones = new ObservableCollection<ProductoEnvasadoComposicion>(context.ProductosEnvasadosComposiciones.Where(pec => pec.ProductoId == productoEnvasado.ProductoEnvasadoId).ToList());
            //viewModel.HistorialHuecosAlmacenajes = new ObservableCollection<HistorialHuecoAlmacenaje>(context.HistorialHuecosAlmacenajes.Where(hha => hha.ProductoId == productoEnvasado.ProductoEnvasadoId).ToList());
            CalcularCantidades();


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            productosEnvasadosViewSource = ((CollectionViewSource)(FindResource("productosEnvasadosViewSource")));
            pickingViewSource = ((CollectionViewSource)(FindResource("pickingViewSource")));
            pedidoDetalleViewSource = ((CollectionViewSource)(FindResource("pedidoDetalleViewSource")));
            productoTerminadoViewSource = ((CollectionViewSource)(FindResource("productoTerminadoViewSource")));

            context.ProductosEnvasados.Load();
            context.Picking.Load();
            context.PedidosDetalles.Load();
            context.ProductosTerminados.Load();

            productosEnvasadosViewSource.Source = context.ProductosEnvasados.Local;
            pickingViewSource.Source = context.Picking.Local;
            pedidoDetalleViewSource.Source = context.PedidosDetalles.Local;
            productoTerminadoViewSource.Source = context.ProductosTerminados.Local;

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

        /*private void spHuecosAlmacenajes_Drop(object sender, DragEventArgs e)
        {
            var huecoAlmacenaje = e.Data.GetData("HuecoAlmacenaje") as HuecoAlmacenaje;
            var historialHuecoAlmacenaje = new HistorialHuecoAlmacenaje() { HuecoAlmacenaje = huecoAlmacenaje };
            viewModel.HistorialHuecosAlmacenajes.Add(historialHuecoAlmacenaje);
            viewModel.HuecosAlmacenajesDisponibles.Remove(huecoAlmacenaje);
            CalcularCantidades();
        }*/

        /*private void cHueco_DeleteClick(object sender, RoutedEventArgs e)
        {
            var chip = sender as Chip;
            int huecoAlmacenajeId = int.Parse(chip.CommandParameter.ToString());
            HistorialHuecoAlmacenaje historialHuecoAlmacenaje = (from hha in viewModel.HistorialHuecosAlmacenajes where hha.HuecoAlmacenaje.HuecoAlmacenajeId == huecoAlmacenajeId select hha).First();
            viewModel.HistorialHuecosAlmacenajes.Remove(historialHuecoAlmacenaje);
            //if (historialHuecoAlmacenaje.HuecoAlmacenaje.SitioId == (cbSitiosAlmacenajes.SelectedItem as SitioAlmacenaje).SitioAlmacenajeId)
            //{
                viewModel.HuecosAlmacenajesDisponibles.Add(historialHuecoAlmacenaje.HuecoAlmacenaje);
            //}
            CalcularCantidades();
        }*/

        private void tbCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewModel.TipoProductoTerminado != null)
            {
                if (viewModel.TipoProductoTerminado.MedidoEnUnidades == true)
                {
                    viewModel.Volumen = Convert.ToInt32(viewModel.Cantidad);
                }
                else
                {
                    viewModel.Volumen = viewModel.Cantidad;
                }
            }
            CalcularCantidades();
        }

        private void CalcularCantidades()
        {
            if (viewModel.TipoProductoTerminado != null && viewModel.TipoProductoTerminado.MedidoEnVolumen == true)
            {
                var unidadesRestantes = viewModel.Unidades;
                foreach (var hha in viewModel.HistorialHuecosAlmacenajes)
                {
                    if (hha.HuecoAlmacenaje.UnidadesTotales <= unidadesRestantes)
                    {
                        unidadesRestantes -= hha.HuecoAlmacenaje.UnidadesTotales;
                        hha.Unidades = hha.HuecoAlmacenaje.UnidadesTotales;
                    }
                    else
                    {
                        hha.Unidades = unidadesRestantes;
                        unidadesRestantes = 0;
                    }
                }
                viewModel.QuedaCantidadPorAlmacenar = unidadesRestantes > 0 || viewModel.Cantidad == 0;
            }
            else
            {
                var volumenRestante = viewModel.Volumen;
                foreach (var hha in viewModel.HistorialHuecosAlmacenajes)
                {
                    if (hha.HuecoAlmacenaje.VolumenTotal <= volumenRestante)
                    {
                        volumenRestante -= hha.HuecoAlmacenaje.VolumenTotal;
                        hha.Volumen = hha.HuecoAlmacenaje.VolumenTotal;
                    }
                    else
                    {
                        hha.Volumen = volumenRestante;
                        volumenRestante = 0;
                    }
                }
                viewModel.QuedaCantidadPorAlmacenar = volumenRestante > 0 || viewModel.Cantidad == 0;
            }

            viewModel.HistorialHuecosAlmacenajes = new ObservableCollection<HistorialHuecoAlmacenaje>(viewModel.HistorialHuecosAlmacenajes.ToList());
        }
    }
}
