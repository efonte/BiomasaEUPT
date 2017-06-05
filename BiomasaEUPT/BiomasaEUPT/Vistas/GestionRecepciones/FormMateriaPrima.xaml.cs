using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private CollectionViewSource huecosRecepcionesViewSource;
        private BiomasaEUPTContext context;
        public FormMateriaPrima()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = new BiomasaEUPTContext();

            tiposMateriasPrimasViewSource = ((CollectionViewSource)(FindResource("tiposMateriasPrimasViewSource")));
            gruposMateriasPrimasViewSource = ((CollectionViewSource)(FindResource("gruposMateriasPrimasViewSource")));
            procedenciasViewSource = ((CollectionViewSource)(FindResource("procedenciasViewSource")));
            sitiosRecepcionesViewSource = ((CollectionViewSource)(FindResource("sitiosRecepcionesViewSource")));
            huecosRecepcionesViewSource = ((CollectionViewSource)(FindResource("huecosRecepcionesViewSource")));
            context.TiposMateriasPrimas.Load();
            context.GruposMateriasPrimas.Load();
            context.Procedencias.Load();
            context.SitiosRecepciones.Load();
            tiposMateriasPrimasViewSource.Source = context.TiposMateriasPrimas.Local;
            gruposMateriasPrimasViewSource.Source = context.GruposMateriasPrimas.Local;
            procedenciasViewSource.Source = context.Procedencias.Local;
            sitiosRecepcionesViewSource.Source = context.SitiosRecepciones.Local;

        }

        private void cbSitiosRecepciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            huecosRecepcionesViewSource.Source = context.HuecosRecepciones.Where(d => d.SitioId == ((SitioRecepcion)cbSitiosRecepciones.SelectedItem).SitioRecepcionId).ToList();
        }

        private void lbHuecosRecepciones_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           ListBox parent = (ListBox)sender;
            //dragSource = parent;
            object data = GetDataFromListBox(lbHuecosRecepciones, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
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
            ListBox parent = (ListBox)sender;
            object data = e.Data.GetData(typeof(string));
            ((IList)lbHuecosRecepciones.ItemsSource).Remove(data);
            parent.Items.Add(data);
        }
    }
}
