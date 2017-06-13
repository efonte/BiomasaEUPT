﻿using BiomasaEUPT.Modelos;
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

namespace BiomasaEUPT.Vistas.GestionElaboraciones
{
    /// <summary>
    /// Lógica de interacción para FormOrdenElaboracion.xaml
    /// </summary>
    public partial class FormOrdenElaboracion : UserControl
    {
        private BiomasaEUPTContext context;
        private CollectionViewSource gruposProductosTerminadosViewSource;
        // private CollectionViewSource tiposProductosTerminadosViewSource;
        public ObservableCollection<TipoProductoTerminado> TiposProductosTerminadosDisponibles { get; set; }
        public ObservableCollection<ProductoTerminado> ProductosTerminados { get; set; }

        public FormOrdenElaboracion()
        {
            InitializeComponent();
            DataContext = this;
            TiposProductosTerminadosDisponibles = new ObservableCollection<TipoProductoTerminado>();
            ProductosTerminados = new ObservableCollection<ProductoTerminado>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = new BiomasaEUPTContext();
            gruposProductosTerminadosViewSource = ((CollectionViewSource)(FindResource("gruposProductosTerminadosViewSource")));
            context.GruposProductosTerminados.Load();
            gruposProductosTerminadosViewSource.Source = context.GruposProductosTerminados.Local;
        }

        private void cbGruposProductosTerminados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TiposProductosTerminadosDisponibles.Clear();
            //context.HuecosRecepciones.Where(d => d.SitioId == ((SitioRecepcion)cbSitiosRecepciones.SelectedItem).SitioRecepcionId).ToList().Except(HuecosRecepciones).ToList().ForEach(HuecosRecepcionesDisponibles.Add);

            // Se añaden todos los HuecosRecepciones del SitioRecepcion seleccionado
            context.TiposProductosTerminados.Where(tpt => tpt.GrupoId == ((GrupoProductoTerminado)cbGruposProductosTerminados.SelectedItem).GrupoProductoTerminadoId).ToList().ForEach(TiposProductosTerminadosDisponibles.Add);
            // Se borran los HuecosRecepciones que ya se han añadido (convertidos en HuecosMateriasPrimas)
            ProductosTerminados.ToList().ForEach(pt => TiposProductosTerminadosDisponibles.Remove(pt.TipoProductoTerminado));
        }

        private void lbTiposProductosTerminados_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var parent = sender as ListBox;
            var tipoProductoTerminado = GetDataFromListBox(lbTiposProductosTerminados, e.GetPosition(parent)) as TipoProductoTerminado;
            if (tipoProductoTerminado != null)
            {
                DataObject dragData = new DataObject("TipoProductoTerminado", tipoProductoTerminado);
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

        private void spProductosTerminados_Drop(object sender, DragEventArgs e)
        {
            var tipoProductoTerminado = e.Data.GetData("TipoProductoTerminado") as TipoProductoTerminado;
            var productoTerminado = new ProductoTerminado() { TipoProductoTerminado = tipoProductoTerminado };
            ProductosTerminados.Add(productoTerminado);
            TiposProductosTerminadosDisponibles.Remove(tipoProductoTerminado);
        }

        private void cProductoTerminado_DeleteClick(object sender, RoutedEventArgs e)
        {
            var chip = sender as Chip;
            int tipoProductoTerminadoId = int.Parse(chip.CommandParameter.ToString());
            /* TipoMateriaPrima tipoMateriaPrima = (from hmp in HuecosMateriasPrimas where hmp.HuecoRecepcion.HuecoRecepcionId == huecoRecepcionId select hmp).First();
             HuecosMateriasPrimas.Remove(tipoMateriaPrima);
             if (tipoMateriaPrima.HuecoRecepcion.SitioId == (cbSitiosRecepciones.SelectedItem as SitioRecepcion).SitioRecepcionId)
             {
                 TiposProductosTerminadosDisponibles.Add(tipoMateriaPrima.HuecoRecepcion);
             }
             CalcularUnidadesVolumen();*/

        }
    }
}