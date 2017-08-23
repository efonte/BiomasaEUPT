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
        private FormMateriaPrimaViewModel viewModel;


        public FormMateriaPrima(BiomasaEUPTContext context)
        {
            InitializeComponent();
            viewModel = new FormMateriaPrimaViewModel();
            DataContext = viewModel;
            this.context = context;
        }

        public FormMateriaPrima(BiomasaEUPTContext context, MateriaPrima materiaPrima) : this(context)
        {
            gbTitulo.Header = "Editar Materia Prima";

            cbGruposMateriasPrimas.SelectedValue = materiaPrima.TipoMateriaPrima.GrupoMateriaPrima.GrupoMateriaPrimaId;
            cbTiposMateriasPrimas.SelectedValue = materiaPrima.TipoMateriaPrima.TipoMateriaPrimaId;
            cbProcedencias.SelectedValue = materiaPrima.Procedencia.ProcedenciaId;
            viewModel.FechaBaja = materiaPrima.FechaBaja;
            viewModel.HoraBaja = materiaPrima.FechaBaja;
            viewModel.Observaciones = materiaPrima.Observaciones;
            if (materiaPrima.TipoMateriaPrima.MedidoEnUnidades == true)
            {
                viewModel.Cantidad = materiaPrima.Unidades.Value;
            }
            else
            {
                viewModel.Cantidad = materiaPrima.Volumen.Value;
            }
            viewModel.HistorialHuecosRecepciones = new ObservableCollection<HistorialHuecoRecepcion>(context.HistorialHuecosRecepciones.Where(hhr => hhr.MateriaPrimaId == materiaPrima.MateriaPrimaId).ToList());
            CalcularCantidades();

            // Si ya se han fabricado algún producto terminado con dicha materia prima entonces los controles estarán deshabilitados
            if (context.ProductosTerminadosComposiciones.Any(ptc => ptc.HistorialHuecoRecepcion.MateriaPrimaId == materiaPrima.MateriaPrimaId))
            {

                cbGruposMateriasPrimas.IsEnabled = false;
                cbTiposMateriasPrimas.IsEnabled = false;
                cbSitiosRecepciones.IsEnabled = false;
                lbHuecosRecepciones.IsEnabled = false;
                tbCantidad.IsEnabled = false;
                wpHuecosRecepciones.IsEnabled = false;
            }
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

        private void cbTiposMateriasPrimas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel.TipoMateriaPrima.MedidoEnUnidades == true)
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

        private void cbSitiosRecepciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Se añaden todos los HuecosRecepciones del SitioRecepcion seleccionado
            viewModel.HuecosRecepcionesDisponibles = new ObservableCollection<HuecoRecepcion>(context.HuecosRecepciones.Where(hr => hr.SitioId == ((SitioRecepcion)cbSitiosRecepciones.SelectedItem).SitioRecepcionId && !hr.Ocupado.Value).ToList());

            // Se borran los HuecosRecepciones que ya se han añadido (convertidos en HistorialHuecosRecepciones)
            viewModel.HistorialHuecosRecepciones.ToList().ForEach(hhr => viewModel.HuecosRecepcionesDisponibles.Remove(hhr.HuecoRecepcion));
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
            viewModel.HistorialHuecosRecepciones.Add(historialHuecoRecepcion);
            viewModel.HuecosRecepcionesDisponibles.Remove(huecoRecepcion);
            CalcularCantidades();
        }

        private void cHueco_DeleteClick(object sender, RoutedEventArgs e)
        {
            var chip = sender as Chip;
            int huecoRecepcionId = int.Parse(chip.CommandParameter.ToString());
            HistorialHuecoRecepcion historialHuecoRecepcion = (from hhr in viewModel.HistorialHuecosRecepciones where hhr.HuecoRecepcion.HuecoRecepcionId == huecoRecepcionId select hhr).First();
            viewModel.HistorialHuecosRecepciones.Remove(historialHuecoRecepcion);
            if (historialHuecoRecepcion.HuecoRecepcion.SitioId == (cbSitiosRecepciones.SelectedItem as SitioRecepcion).SitioRecepcionId)
            {
                viewModel.HuecosRecepcionesDisponibles.Add(historialHuecoRecepcion.HuecoRecepcion);
            }
            CalcularCantidades();
        }

        /*  private void tbVolumen_TextChanged(object sender, TextChangedEventArgs e)
          {
              CalcularUnidadesVolumen();
          }

          private void tbUnidades_TextChanged(object sender, TextChangedEventArgs e)
          {
              CalcularUnidadesVolumen();
          }*/

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

        /*private void bCodigo_Click(object sender, RoutedEventArgs e)
        {
            GenerarCodigo();
        }*/

        private void CalcularCantidades()
        {
            if (viewModel.TipoMateriaPrima != null && viewModel.TipoMateriaPrima.MedidoEnUnidades == true)
            {
                var unidadesRestantes = viewModel.Unidades;
                foreach (var hhr in viewModel.HistorialHuecosRecepciones)
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
                viewModel.QuedaCantidadPorAlmacenar = unidadesRestantes > 0 || viewModel.Cantidad == 0;
            }
            else
            {
                var volumenRestante = viewModel.Volumen;
                foreach (var hhr in viewModel.HistorialHuecosRecepciones)
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
                viewModel.QuedaCantidadPorAlmacenar = volumenRestante > 0 || viewModel.Cantidad == 0;
            }
            viewModel.HistorialHuecosRecepciones = new ObservableCollection<HistorialHuecoRecepcion>(viewModel.HistorialHuecosRecepciones.ToList());
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
