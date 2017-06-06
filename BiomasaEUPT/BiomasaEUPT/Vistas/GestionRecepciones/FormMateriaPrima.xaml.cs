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
        //public ObservableCollection<TipoMateriaPrima> TiposMateriasPrimas { get; set; }
        public TipoMateriaPrima TipoMateriaPrimaSeleccionada { get; set; }
        public ObservableCollection<HuecoRecepcion> HuecosRecepcionesDisponibles { get; set; }
        public ObservableCollection<HuecoRecepcion> HuecosRecepciones { get; set; }
        public int Unidades { get; set; }
        public double Volumen { get; set; }

        public FormMateriaPrima()
        {
            InitializeComponent();
            DataContext = this;
            //TiposMateriasPrimas = new ObservableCollection<TipoMateriaPrima>();
            HuecosRecepcionesDisponibles = new ObservableCollection<HuecoRecepcion>();
            HuecosRecepciones = new ObservableCollection<HuecoRecepcion>();
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
            //TiposMateriasPrimas.Clear();
            //context.TiposMateriasPrimas.Where(d => d.GrupoId == ((GrupoMateriaPrima)cbGruposMateriasPrimas.SelectedItem).GrupoMateriaPrimaId).ToList().ForEach(TiposMateriasPrimas.Add);
            //cbTiposMateriasPrimas.SelectedIndex = 0;
            Console.WriteLine(TipoMateriaPrimaSeleccionada.Nombre);
        }

        private void cbSitiosRecepciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // No se puede crear un nuevo ObservableCollection ya que sino no se actualiza la vista. Hay que añadirlos al ya existente.
            HuecosRecepcionesDisponibles.Clear();
            context.HuecosRecepciones.Where(d => d.SitioId == ((SitioRecepcion)cbSitiosRecepciones.SelectedItem).SitioRecepcionId).ToList().Except(HuecosRecepciones).ToList().ForEach(HuecosRecepcionesDisponibles.Add);
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

        private static object GetDataFromListBox(ListBox source, Point point)
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
            HuecosRecepciones.Add(huecoRecepcion);
            HuecosRecepcionesDisponibles.Remove(huecoRecepcion);
        }

        private void cHueco_DeleteClick(object sender, RoutedEventArgs e)
        {
            var chip = sender as Chip;
            int huecoRecepcionId = int.Parse(chip.CommandParameter.ToString());
            HuecoRecepcion huecoRecepcion = (from hr in HuecosRecepciones where hr.HuecoRecepcionId == huecoRecepcionId select hr).First();

            HuecosRecepciones.Remove(huecoRecepcion);
            if (huecoRecepcion.SitioId == (cbSitiosRecepciones.SelectedItem as SitioRecepcion).SitioRecepcionId)
            {
                HuecosRecepcionesDisponibles.Add(huecoRecepcion);
            }
        }
    }
}
