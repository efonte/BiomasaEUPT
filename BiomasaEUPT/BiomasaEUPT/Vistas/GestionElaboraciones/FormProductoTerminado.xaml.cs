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
        public ObservableCollection<HuecoAlmacenaje> HuecosAlmacenajesDisponibles { get; set; }
        public ObservableCollection<HistorialHuecoAlmacenaje> HistorialHuecosAlmacenajes { get; set; }
        public ObservableCollection<HistorialHuecoRecepcion> HistorialHuecosRecepciones { get; set; }
        public ObservableCollection<ProductoTerminadoComposicion> ProductosTerminadosComposiciones { get; set; }

        public DateTime? FechaBaja { get; set; }
        public DateTime? HoraBaja { get; set; }
        public String Observaciones { get; set; }
        public int Unidades { get; set; }
        public double Volumen { get; set; }
        private BiomasaEUPTContext context;


        public FormProductoTerminado(BiomasaEUPTContext context)
        {
            InitializeComponent();
            DataContext = this;
            this.context = context;
            Observaciones = this.Observaciones;
            this.context = context;
            HuecosAlmacenajesDisponibles = new ObservableCollection<HuecoAlmacenaje>();
            HistorialHuecosAlmacenajes = new ObservableCollection<HistorialHuecoAlmacenaje>();
            HistorialHuecosRecepciones = new ObservableCollection<HistorialHuecoRecepcion>();
            ProductosTerminadosComposiciones = new ObservableCollection<ProductoTerminadoComposicion>();

        }

        public FormProductoTerminado(BiomasaEUPTContext context, string _titulo) : this(context)
        {
            gbTitulo.Header = _titulo;
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

        private void cbSitiosAlmacenajes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HuecosAlmacenajesDisponibles.Clear();


            // Se añaden todos los HuecosAlmacenajes del SitioAlmacenaje seleccionado
            context.HuecosAlmacenajes.Where(ha => ha.SitioId == ((SitioAlmacenaje)cbSitiosAlmacenajes.SelectedItem).SitioAlmacenajeId && !ha.Ocupado.Value).ToList().ForEach(HuecosAlmacenajesDisponibles.Add);
            // Se borran los HuecosAlmacenajes que ya se han añadido
            HistorialHuecosAlmacenajes.ToList().ForEach(hha => HuecosAlmacenajesDisponibles.Remove(hha.HuecoAlmacenaje));
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
            HistorialHuecosAlmacenajes.Add(historialHuecoAlmacenaje);
            HuecosAlmacenajesDisponibles.Remove(huecoAlmacenaje);
            CalcularUnidadesVolumen();
        }

        private void cHueco_DeleteClick(object sender, RoutedEventArgs e)
        {
            var chip = sender as Chip;
            int huecoAlmacenajeId = int.Parse(chip.CommandParameter.ToString());

            HistorialHuecoAlmacenaje historialHuecoAlmacenaje = (from hha in HistorialHuecosAlmacenajes where hha.HuecoAlmacenaje.HuecoAlmacenajeId == huecoAlmacenajeId select hha).First();
            HistorialHuecosAlmacenajes.Remove(historialHuecoAlmacenaje);
            if (historialHuecoAlmacenaje.HuecoAlmacenaje.SitioId == (cbSitiosAlmacenajes.SelectedItem as SitioAlmacenaje).SitioAlmacenajeId)
            {
                HuecosAlmacenajesDisponibles.Add(historialHuecoAlmacenaje.HuecoAlmacenaje);
            }
            CalcularUnidadesVolumen();

        }

        private void tbVolumen_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcularUnidadesVolumen();
        }

        private void tbUnidades_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcularUnidadesVolumen();
        }

        private void CalcularUnidadesVolumen()
        {

            if (TipoProductoTerminado != null && TipoProductoTerminado.MedidoEnUnidades == true)
            {
                var unidadesRestantes = Unidades;
                foreach (var hha in HistorialHuecosAlmacenajes)
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
            }
            else
            {
                var volumenRestante = Volumen;
                foreach (var hha in HistorialHuecosAlmacenajes)
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
            }
            var nuevosHistorialesHuecosAlmacenajes = HistorialHuecosAlmacenajes.ToList();
            HistorialHuecosAlmacenajes.Clear();
            nuevosHistorialesHuecosAlmacenajes.ForEach(HistorialHuecosAlmacenajes.Add);
        }

        private void lbHistorialHuecosRecepciones_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*var parent = sender as ListBox;
            var tipoProductoTerminado = GetDataFromListBox(lbTiposProductosTerminados, e.GetPosition(parent)) as TipoProductoTerminado;
            if (tipoProductoTerminado != null)
            {
                DataObject dragData = new DataObject("TipoProductoTerminado", tipoProductoTerminado);
                DragDrop.DoDragDrop(parent, dragData, DragDropEffects.Move);
            }*/
        }

        private void spProductosTerminadosComposiciones_Drop(object sender, DragEventArgs e)
        {
            /*var tipoProductoTerminado = e.Data.GetData("TipoProductoTerminado") as TipoProductoTerminado;
            var productoTerminadoComposicion = new ProductoTerminadoComposicion() { ProductoTerminado = productoTerminadoComposicion };
            ProductosTerminadosComposiciones.Add(productoTerminadoComposicion);
            HistorialHuecosRecepciones.Remove(tipoProductoTerminado);*/
        }

        private void cProductoTerminadoComposicion_DeleteClick(object sender, RoutedEventArgs e)
        {
            /*var chip = sender as Chip;
            int tipoProductoTerminadoId = int.Parse(chip.CommandParameter.ToString());
            ProductoTerminado productoTerminado = ProductosTerminados.Single(pt => pt.TipoProductoTerminado.TipoProductoTerminadoId == tipoProductoTerminadoId);
            ProductosTerminados.Remove(productoTerminado);
            if (productoTerminado.TipoProductoTerminado.GrupoProductoTerminado.GrupoProductoTerminadoId == (cbGruposProductosTerminados.SelectedItem as GrupoProductoTerminado).GrupoProductoTerminadoId)
            {
                TiposProductosTerminadosDisponibles.Add(productoTerminado.TipoProductoTerminado);
            }*/
        }
    }
}
