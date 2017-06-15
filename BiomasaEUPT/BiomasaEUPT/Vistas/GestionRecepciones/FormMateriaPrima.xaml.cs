using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

namespace BiomasaEUPT.Vistas.GestionRecepciones
{
    /// <summary>
    /// Lógica de interacción para FormMateriaPrima.xaml
    /// </summary>
    public partial class FormMateriaPrima : UserControl
    {
        private CollectionViewSource tiposMateriasPrimasViewSource;
        private CollectionViewSource gruposMateriasPrimasViewSource;
        private CollectionViewSource procedenciasViewSource;
        private CollectionViewSource sitiosRecepcionesViewSource;
        private BiomasaEUPTContext context;
        public TipoMateriaPrima TipoMateriaPrima { get; set; }
        public ObservableCollection<HuecoRecepcion> HuecosRecepcionesDisponibles { get; set; }
        //public ObservableCollection<HuecoRecepcion> HuecosRecepciones { get; set; }
        public ObservableCollection<HistorialHuecoRecepcion> HistorialHuecosRecepciones { get; set; }
        public int Unidades { get; set; }
        public double Volumen { get; set; }
        //public String Codigo { get; set; }
        public String Observaciones { get; set; }
        public DateTime? FechaBaja { get; set; }
        public DateTime? HoraBaja { get; set; }

        public FormMateriaPrima(BiomasaEUPTContext context)
        {
            InitializeComponent();
            DataContext = this;
            this.context = context;
            HuecosRecepcionesDisponibles = new ObservableCollection<HuecoRecepcion>();
            //HuecosRecepciones = new ObservableCollection<HuecoRecepcion>();
            HistorialHuecosRecepciones = new ObservableCollection<HistorialHuecoRecepcion>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tiposMateriasPrimasViewSource = ((CollectionViewSource)(FindResource("tiposMateriasPrimasViewSource")));
            gruposMateriasPrimasViewSource = ((CollectionViewSource)(FindResource("gruposMateriasPrimasViewSource")));
            procedenciasViewSource = ((CollectionViewSource)(FindResource("procedenciasViewSource")));
            sitiosRecepcionesViewSource = ((CollectionViewSource)(FindResource("sitiosRecepcionesViewSource")));

            context.GruposMateriasPrimas.Load();
            context.Procedencias.Load();
            context.SitiosRecepciones.Load();
            gruposMateriasPrimasViewSource.Source = context.GruposMateriasPrimas.Local;
            procedenciasViewSource.Source = context.Procedencias.Local;
            sitiosRecepcionesViewSource.Source = context.SitiosRecepciones.Local;
            //GenerarCodigo();
        }

        private void cbGruposMateriasPrimas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tiposMateriasPrimasViewSource.Source = context.TiposMateriasPrimas.Where(d => d.GrupoId == ((GrupoMateriaPrima)cbGruposMateriasPrimas.SelectedItem).GrupoMateriaPrimaId).ToList();
        }

        private void cbSitiosRecepciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // No se puede crear un nuevo ObservableCollection ya que sino no se actualiza la vista. Hay que añadirlos al ya existente.
            HuecosRecepcionesDisponibles.Clear();
            //context.HuecosRecepciones.Where(d => d.SitioId == ((SitioRecepcion)cbSitiosRecepciones.SelectedItem).SitioRecepcionId).ToList().Except(HuecosRecepciones).ToList().ForEach(HuecosRecepcionesDisponibles.Add);

            // Se añaden todos los HuecosRecepciones del SitioRecepcion seleccionado
            context.HuecosRecepciones.Where(hr => hr.SitioId == ((SitioRecepcion)cbSitiosRecepciones.SelectedItem).SitioRecepcionId && !hr.Ocupado.Value).ToList().ForEach(HuecosRecepcionesDisponibles.Add);
            // Se borran los HuecosRecepciones que ya se han añadido (convertidos en HuecosMateriasPrimas)
            HistorialHuecosRecepciones.ToList().ForEach(hhr => HuecosRecepcionesDisponibles.Remove(hhr.HuecoRecepcion));
        }

        private void lbHuecosRecepciones_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var parent = sender as ListBox;
            var huecoRecepcion = GetDataFromListBox(lbHuecosRecepciones, e.GetPosition(parent)) as HuecoRecepcion;
            if (huecoRecepcion != null)
            {
                DataObject dragData = new DataObject("HuecoRecepcion", huecoRecepcion);
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

        private void spHuecosRecepciones_Drop(object sender, DragEventArgs e)
        {
            var huecoRecepcion = e.Data.GetData("HuecoRecepcion") as HuecoRecepcion;
            var historialHuecoRecepcion = new HistorialHuecoRecepcion() { HuecoRecepcion = huecoRecepcion };
            HistorialHuecosRecepciones.Add(historialHuecoRecepcion);
            //HuecosMateriasPrimas.Add(huecoRecepcion);
            HuecosRecepcionesDisponibles.Remove(huecoRecepcion);
            CalcularUnidadesVolumen();
        }

        private void cHueco_DeleteClick(object sender, RoutedEventArgs e)
        {
            var chip = sender as Chip;
            int huecoRecepcionId = int.Parse(chip.CommandParameter.ToString());
            /* HuecoRecepcion huecoRecepcion = (from hr in HuecosRecepciones where hr.HuecoRecepcionId == huecoRecepcionId select hr).First();
              HuecosRecepciones.Remove(huecoRecepcion);            
              if (huecoRecepcion.SitioId == (cbSitiosRecepciones.SelectedItem as SitioRecepcion).SitioRecepcionId)
              {
                  HuecosRecepcionesDisponibles.Add(huecoRecepcion);
              }*/
            HistorialHuecoRecepcion historialHuecoRecepcion = (from hhr in HistorialHuecosRecepciones where hhr.HuecoRecepcion.HuecoRecepcionId == huecoRecepcionId select hhr).First();
            HistorialHuecosRecepciones.Remove(historialHuecoRecepcion);
            if (historialHuecoRecepcion.HuecoRecepcion.SitioId == (cbSitiosRecepciones.SelectedItem as SitioRecepcion).SitioRecepcionId)
            {
                HuecosRecepcionesDisponibles.Add(historialHuecoRecepcion.HuecoRecepcion);
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

        /*private void bCodigo_Click(object sender, RoutedEventArgs e)
        {
            GenerarCodigo();
        }*/

        private void CalcularUnidadesVolumen()
        {

            if (TipoMateriaPrima != null && TipoMateriaPrima.MedidoEnUnidades == true)
            {
                var unidadesRestantes = Unidades;
                foreach (var hhr in HistorialHuecosRecepciones)
                {
                    if (hhr.HuecoRecepcion.UnidadesTotales <= unidadesRestantes)
                    {
                        unidadesRestantes -= hhr.HuecoRecepcion.UnidadesTotales;
                        hhr.Unidades = hhr.HuecoRecepcion.UnidadesTotales;
                    }
                    else
                    {
                        hhr.Unidades = unidadesRestantes;
                        unidadesRestantes = 0;
                    }
                }
            }
            else
            {
                var volumenRestante = Volumen;
                foreach (var hhr in HistorialHuecosRecepciones)
                {
                    if (hhr.HuecoRecepcion.VolumenTotal <= volumenRestante)
                    {
                        volumenRestante -= hhr.HuecoRecepcion.VolumenTotal;
                        hhr.Volumen = hhr.HuecoRecepcion.VolumenTotal;
                    }
                    else
                    {
                        hhr.Volumen = volumenRestante;
                        volumenRestante = 0;
                    }
                }
            }
            var nuevosHistorialesHuecosRecepciones = HistorialHuecosRecepciones.ToList();
            HistorialHuecosRecepciones.Clear();
            nuevosHistorialesHuecosRecepciones.ForEach(HistorialHuecosRecepciones.Add);
        }

        /*private void GenerarCodigo()
        {
            Random r = new Random();
            int codigo;
            do
            {
                codigo = r.Next(0, 999999999) + 1000000000;
            } while (context.MateriasPrimas.Any(mp => mp.Codigo == codigo.ToString()));
            Codigo = codigo.ToString();
            lCodigo.Content = codigo;
        }*/
    }
}
