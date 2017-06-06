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
        public ObservableCollection<HuecoMateriaPrima> HuecosMateriasPrimas { get; set; }
        public int Unidades { get; set; }
        public double Volumen { get; set; }

        public FormMateriaPrima()
        {
            InitializeComponent();
            DataContext = this;
            HuecosRecepcionesDisponibles = new ObservableCollection<HuecoRecepcion>();
            //HuecosRecepciones = new ObservableCollection<HuecoRecepcion>();
            HuecosMateriasPrimas = new ObservableCollection<HuecoMateriaPrima>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = new BiomasaEUPTContext();

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
            //HuecosRecepcionesDisponibles.ToList().RemoveAll(h => h.HuecosMateriasPrimas.Contains(HuecosMateriasPrimas));
            // var huecosRecepcionesExcluidos=HuecosMateriasPrimas.Where(hmp=>hmp.)
            //   context.HuecosRecepciones.Where(d => d.SitioId == ((SitioRecepcion)cbSitiosRecepciones.SelectedItem).SitioRecepcionId).ToList().Except().ToList().ForEach(HuecosRecepcionesDisponibles.Add);

            // Se añaden todos los HuecosRecepciones del SitioRecepcion seleccionado
            context.HuecosRecepciones.Where(d => d.SitioId == ((SitioRecepcion)cbSitiosRecepciones.SelectedItem).SitioRecepcionId).ToList().ForEach(HuecosRecepcionesDisponibles.Add);
            // Se borran los HuecosRecepciones que ya se han añadido (convertidos en HuecosMateriasPrimas)
            HuecosMateriasPrimas.ToList().ForEach(hmp => HuecosRecepcionesDisponibles.Remove(hmp.HuecoRecepcion));
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
            var huecoMateriaPrima = new HuecoMateriaPrima() { HuecoRecepcion = huecoRecepcion };
            HuecosMateriasPrimas.Add(huecoMateriaPrima);
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
            HuecoMateriaPrima huecoMateriaPrima = (from hmp in HuecosMateriasPrimas where hmp.HuecoRecepcion.HuecoRecepcionId == huecoRecepcionId select hmp).First();
            HuecosMateriasPrimas.Remove(huecoMateriaPrima);
            if (huecoMateriaPrima.HuecoRecepcion.SitioId == (cbSitiosRecepciones.SelectedItem as SitioRecepcion).SitioRecepcionId)
            {
                HuecosRecepcionesDisponibles.Add(huecoMateriaPrima.HuecoRecepcion);
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

            if (TipoMateriaPrima != null && TipoMateriaPrima.MedidoEnUnidades == true)
            {
                var unidadesRestantes = Unidades;
                //var nuevosHuecosMateriasPrimas = HuecosMateriasPrimas.Select(c => { c.Unidades = 4; return c; }).ToList();
                //HuecosMateriasPrimas.Clear();
                //nuevosHuecosMateriasPrimas.ForEach(HuecosMateriasPrimas.Add);
                foreach (var hmp in HuecosMateriasPrimas)
                {
                    if (hmp.HuecoRecepcion.UnidadesTotales <= unidadesRestantes)
                    {
                        unidadesRestantes -= hmp.HuecoRecepcion.UnidadesTotales;
                        hmp.Unidades = hmp.HuecoRecepcion.UnidadesTotales;
                        Console.WriteLine(unidadesRestantes);
                    }
                    else
                    {
                        hmp.Unidades = unidadesRestantes;
                        unidadesRestantes = 0;
                    }
                }
                var nuevosHuecosMateriasPrimas = HuecosMateriasPrimas.ToList();
                HuecosMateriasPrimas.Clear();
                nuevosHuecosMateriasPrimas.ForEach(HuecosMateriasPrimas.Add);
            }
            else
            {

            }
        }


    }
}
