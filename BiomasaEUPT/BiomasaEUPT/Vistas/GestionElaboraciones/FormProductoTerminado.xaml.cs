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

namespace BiomasaEUPT.Vistas.GestionElaboraciones
{
    /// <summary>
    /// Lógica de interacción para FormProductoTerminado.xaml
    /// </summary>
    public partial class FormProductoTerminado : UserControl
    {

        private CollectionViewSource productosTerminadosViewSource;
        private CollectionViewSource tiposProductosTerminadosViewSource;
        private CollectionViewSource gruposProductosTerminadosViewSource;
        private CollectionViewSource tiposMateriasPrimasViewSource;
        private CollectionViewSource gruposMateriasPrimasViewSource;
        private CollectionViewSource sitiosAlmacenajesViewSource;
        private CollectionViewSource huecosAlmacenajesViewSource;
        public TipoProductoTerminado TipoProductoTerminado { get; set; }
        private FormProductoTerminadoViewModel viewModel;

        private BiomasaEUPTContext context;


        public FormProductoTerminado(BiomasaEUPTContext context)
        {
            InitializeComponent();
            viewModel = new FormProductoTerminadoViewModel();
            DataContext = viewModel;
            this.context = context;

        }

        public FormProductoTerminado(BiomasaEUPTContext context, ProductoTerminado productoTerminado) : this(context)
        {
            gbTitulo.Header = "Editar Producto Terminado";

            cbGruposProductosTerminados.SelectedValue = productoTerminado.TipoProductoTerminado.GrupoProductoTerminado.GrupoProductoTerminadoId;
            cbTiposProductosTerminados.SelectedValue = productoTerminado.TipoProductoTerminado.TipoProductoTerminadoId;
            //cbGruposMateriasPrimas.SelectedValue = productoTerminado.TipoMateriaPrima.GrupoMateriaPrima.GrupoMateriaPrimaId;
            //cbTiposMateriasPrimas.SelectedValue = productoTerminado.TipoMateriaPrima.TipoMateriaPrimaId;

            viewModel.FechaBaja = productoTerminado.FechaBaja;
            viewModel.HoraBaja = productoTerminado.FechaBaja;
            viewModel.Observaciones = productoTerminado.Observaciones;
            if (productoTerminado.TipoProductoTerminado.MedidoEnUnidades == true)
            {
                viewModel.Cantidad = productoTerminado.Unidades.Value;
            }
            else
            {
                viewModel.Cantidad = productoTerminado.Volumen.Value;
            }
            viewModel.ProductosTerminadosComposiciones = new ObservableCollection<ProductoTerminadoComposicion>(context.ProductosTerminadosComposiciones.Where(ptc => ptc.ProductoId == productoTerminado.ProductoTerminadoId).ToList());
            viewModel.HistorialHuecosAlmacenajes = new ObservableCollection<HistorialHuecoAlmacenaje>(context.HistorialHuecosAlmacenajes.Where(hha => hha.ProductoTerminadoId == productoTerminado.ProductoTerminadoId).ToList());
            CalcularCantidades();

            // Si ya se han envasado algún producto envasado con dicho producto terminado entonces los controles estarán deshabilitados
            if (context.ProductosTerminadosComposiciones.Any(ptc => ptc.ProductoTerminado.ProductoTerminadoId == productoTerminado.ProductoTerminadoId))
            {
                cbGruposMateriasPrimas.IsEnabled = false;
                cbTiposMateriasPrimas.IsEnabled = false;
                cbSitiosAlmacenajes.IsEnabled = false;
                lbHuecosAlmacenajes.IsEnabled = false;
                tbCantidad.IsEnabled = false;
                wpHuecosAlmacenajes.IsEnabled = false;
                wpProductosTerminadosComposiciones.IsEnabled = false;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            productosTerminadosViewSource = ((CollectionViewSource)(FindResource("productosTerminadosViewSource")));
            tiposProductosTerminadosViewSource = ((CollectionViewSource)(FindResource("tiposProductosTerminadosViewSource")));
            gruposProductosTerminadosViewSource = ((CollectionViewSource)(FindResource("gruposProductosTerminadosViewSource")));
            sitiosAlmacenajesViewSource = ((CollectionViewSource)(FindResource("sitiosAlmacenajesViewSource")));
            huecosAlmacenajesViewSource = ((CollectionViewSource)(FindResource("huecosAlmacenajesViewSource")));
            tiposMateriasPrimasViewSource = ((CollectionViewSource)(FindResource("tiposMateriasPrimasViewSource")));
            gruposMateriasPrimasViewSource = ((CollectionViewSource)(FindResource("gruposMateriasPrimasViewSource")));

            context.ProductosTerminados.Load();
            context.TiposProductosTerminados.Load();
            context.GruposProductosTerminados.Load();
            context.SitiosAlmacenajes.Load();
            context.HuecosAlmacenajes.Load();
            context.TiposMateriasPrimas.Load();
            context.GruposMateriasPrimas.Load();

            productosTerminadosViewSource.Source = context.ProductosTerminados.Local;
            tiposProductosTerminadosViewSource.Source = context.TiposProductosTerminados.Local;
            gruposProductosTerminadosViewSource.Source = context.GruposProductosTerminados.Local;
            sitiosAlmacenajesViewSource.Source = context.SitiosAlmacenajes.Local;
            huecosAlmacenajesViewSource.Source = context.HuecosAlmacenajes.Local;
            tiposMateriasPrimasViewSource.Source = context.TiposMateriasPrimas.Local;
            gruposMateriasPrimasViewSource.Source = context.GruposMateriasPrimas.Local;

            dpFechaBaja.Language = System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name);
        }

        private void cbGruposProductosTerminados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tiposProductosTerminadosViewSource.Source = context.TiposProductosTerminados.Where(d => d.GrupoId == ((GrupoProductoTerminado)cbGruposProductosTerminados.SelectedItem).GrupoProductoTerminadoId).ToList();
        }

        private void cbGruposMateriasPrimas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tiposMateriasPrimasViewSource.Source = context.TiposMateriasPrimas.Where(d => d.GrupoId == ((GrupoMateriaPrima)cbGruposMateriasPrimas.SelectedItem).GrupoMateriaPrimaId).ToList();
        }

        private void cbTiposProductosTerminados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel.TipoProductoTerminado.MedidoEnUnidades == true)
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

        private void cbTiposMateriasPrimas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Se añaden todos los HistorialHuecosRecepciones que contienen tienen el TipoMateriaPrima seleccionada
            viewModel.HistorialHuecosRecepcionesDisponibles = new ObservableCollection<HistorialHuecoRecepcion>(context.HistorialHuecosRecepciones.Where(hhr => hhr.MateriaPrima.TipoId == ((TipoMateriaPrima)cbTiposMateriasPrimas.SelectedItem).TipoMateriaPrimaId && (viewModel.TipoMateriaPrima.MedidoEnUnidades == true ? (hhr.UnidadesRestantes > 0) : (hhr.VolumenRestante > 0))).ToList());

            // Se borran los HistorialHuecosRecepciones que ya se han añadido (convertidos en ProductosTerminadosComposiciones)
            viewModel.ProductosTerminadosComposiciones.ToList().ForEach(ptc => viewModel.HistorialHuecosRecepcionesDisponibles.Remove(ptc.HistorialHuecoRecepcion));
        }

        private void cbSitiosAlmacenajes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Se añaden todos los HuecosAlmacenajes del SitioAlmacenaje seleccionado
            viewModel.HuecosAlmacenajesDisponibles = new ObservableCollection<HuecoAlmacenaje>(context.HuecosAlmacenajes.Where(ha => ha.SitioId == ((SitioAlmacenaje)cbSitiosAlmacenajes.SelectedItem).SitioAlmacenajeId && !ha.Ocupado.Value).ToList());

            // Se borran los HuecosAlmacenajes que ya se han añadido (convertidos en HistorialHuecosAlmacenajes)
            viewModel.HistorialHuecosAlmacenajes.ToList().ForEach(hha => viewModel.HuecosAlmacenajesDisponibles.Remove(hha.HuecoAlmacenaje));
        }

        private void lbHistorialHuecosRecepciones_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var parent = sender as ListBox;
            var historialHuecoRecepcion = GetDataFromListBox(lbHistorialHuecosRecepciones, e.GetPosition(parent)) as HistorialHuecoRecepcion;
            if (historialHuecoRecepcion != null)
            {
                DataObject dragData = new DataObject("HistorialHuecoRecepcion", historialHuecoRecepcion);
                DragDrop.DoDragDrop(parent, dragData, DragDropEffects.Move);
            }
        }

        private void spProductosTerminadosComposiciones_Drop(object sender, DragEventArgs e)
        {
            var historialHuecoRecepcion = e.Data.GetData("HistorialHuecoRecepcion") as HistorialHuecoRecepcion;
            var productoTerminadoComposicion = new ProductoTerminadoComposicion() { HistorialHuecoRecepcion = historialHuecoRecepcion };
            viewModel.ProductosTerminadosComposiciones.Add(productoTerminadoComposicion);
            viewModel.HistorialHuecosRecepcionesDisponibles.Remove(historialHuecoRecepcion);
        }

        private void cProductoTerminadoComposicion_DeleteClick(object sender, RoutedEventArgs e)
        {
            var chip = sender as Chip;
            int historialHuecoRecepcionId = int.Parse(chip.CommandParameter.ToString());
            var productoTerminadoComposicion = viewModel.ProductosTerminadosComposiciones.Single(ptc => ptc.HistorialHuecoRecepcion.HistorialHuecoRecepcionId == historialHuecoRecepcionId);
            viewModel.ProductosTerminadosComposiciones.Remove(productoTerminadoComposicion);
            if (productoTerminadoComposicion.HistorialHuecoRecepcion.MateriaPrima.TipoId == (cbTiposMateriasPrimas.SelectedItem as TipoMateriaPrima).TipoMateriaPrimaId)
            {
                viewModel.HistorialHuecosRecepcionesDisponibles.Add(productoTerminadoComposicion.HistorialHuecoRecepcion);
            }
        }

        private void lbHuecosAlmacenajes_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var parent = sender as ListBox;
            var huecoAlmacenaje = GetDataFromListBox(lbHuecosAlmacenajes, e.GetPosition(parent)) as HuecoAlmacenaje;
            if (huecoAlmacenaje != null)
            {
                DataObject dragData = new DataObject("HuecoAlmacenaje", huecoAlmacenaje);
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

        private void spHuecosAlmacenajes_Drop(object sender, DragEventArgs e)
        {
            var huecoAlmacenaje = e.Data.GetData("HuecoAlmacenaje") as HuecoAlmacenaje;
            var historialHuecoAlmacenaje = new HistorialHuecoAlmacenaje() { HuecoAlmacenaje = huecoAlmacenaje };
            viewModel.HistorialHuecosAlmacenajes.Add(historialHuecoAlmacenaje);
            viewModel.HuecosAlmacenajesDisponibles.Remove(huecoAlmacenaje);
            CalcularCantidades();
        }

        private void cHueco_DeleteClick(object sender, RoutedEventArgs e)
        {
            var chip = sender as Chip;
            int huecoAlmacenajeId = int.Parse(chip.CommandParameter.ToString());
            HistorialHuecoAlmacenaje historialHuecoAlmacenaje = (from hha in viewModel.HistorialHuecosAlmacenajes where hha.HuecoAlmacenaje.HuecoAlmacenajeId == huecoAlmacenajeId select hha).First();
            viewModel.HistorialHuecosAlmacenajes.Remove(historialHuecoAlmacenaje);
            if (historialHuecoAlmacenaje.HuecoAlmacenaje.SitioId == (cbSitiosAlmacenajes.SelectedItem as SitioAlmacenaje).SitioAlmacenajeId)
            {
                viewModel.HuecosAlmacenajesDisponibles.Add(historialHuecoAlmacenaje.HuecoAlmacenaje);
            }
            CalcularCantidades();
        }

        private void tbCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewModel.TipoMateriaPrima != null)
            {
                if (viewModel.TipoMateriaPrima.MedidoEnUnidades == true)
                {
                    viewModel.Unidades = Convert.ToInt32(viewModel.Cantidad);
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
            if (viewModel.TipoMateriaPrima != null && viewModel.TipoMateriaPrima.MedidoEnUnidades == true)
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
