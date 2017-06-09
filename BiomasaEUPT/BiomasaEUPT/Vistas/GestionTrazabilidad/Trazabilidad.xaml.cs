using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

namespace BiomasaEUPT.Vistas.GestionTrazabilidad
{
    /// <summary>
    /// Lógica de interacción para Trazabilidad.xaml
    /// </summary>
    public partial class Trazabilidad : UserControl
    {
        private BiomasaEUPTContext context;
        public Trazabilidad()
        {
            InitializeComponent();
            context = new BiomasaEUPTContext();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void tbCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            string codigo = (sender as TextBox).Text;
            spTrazabilidad.Children.Clear();
            Grid.SetColumnSpan(tbCodigo, 2);
            rdCodigo.Height = new GridLength(1, GridUnitType.Star);
            rdTrazabilidad.Height = GridLength.Auto;

            if (codigo.Length == 10)
            {
                Grid.SetColumnSpan(tbCodigo, 1);
                rdCodigo.Height = GridLength.Auto;
                rdTrazabilidad.Height = new GridLength(1, GridUnitType.Star);

                switch (codigo[0].ToString())
                {
                    case Constantes.CODIGO_MATERIAS_PRIMAS:
                        if (context.MateriasPrimas.Any(mp => mp.Codigo == codigo))
                        {
                            var materiaPrima = context.MateriasPrimas.Single(mp => mp.Codigo == codigo);
                            var treeView = new TreeView();
                            spTrazabilidad.Children.Add(treeView);
                            var tviRecepcionTexto = new TextBlock() { Text = materiaPrima.Recepcion.NumeroAlbaran, Margin = new Thickness(8, 0, 0, 0) };
                            var tviRecepcionIcono = new PackIcon() { Kind = PackIconKind.Truck };
                            var tviRecepcionHeader = new StackPanel() { Orientation = Orientation.Horizontal };
                            tviRecepcionHeader.Children.Add(tviRecepcionIcono);
                            tviRecepcionHeader.Children.Add(tviRecepcionTexto);
                            var tviRecepcion = new TreeViewItem() { Header = tviRecepcionHeader, IsExpanded = true };
                            treeView.Items.Add(tviRecepcion);
                            var tviMateriaPrima = new TreeViewItem() { Header = materiaPrima.TipoMateriaPrima.Nombre, IsExpanded = true };
                            tviRecepcion.Items.Add(tviMateriaPrima);
                            // spTrazabilidad.Children.Add(new PlantillaRMecepcion() { DataContext = materiaPrima.Recepcion, Margin=new Thickness(10,10,10,10) });
                            // spTrazabilidad.Children.Add(new PlantillaMateriaPrima() { Margin = new Thickness(10, 10, 10, 10) });
                            //var huecosMateriasPrimas = context.HuecosMateriasPrimas.Where(hmp => hmp.MateriaPrima == materiaPrima).ToList();
                            // 1726251362
                            var sitiosRecepciones = (from hmp in context.HuecosMateriasPrimas
                                                     join hr in context.HuecosRecepciones on hmp.HuecoRecepcionId equals hr.HuecoRecepcionId
                                                     join sr in context.SitiosRecepciones on hr.SitioId equals sr.SitioRecepcionId
                                                     where hmp.MateriaPrimaId == materiaPrima.MateriaPrimaId
                                                     select new
                                                     {
                                                         sr
                                                     });


                            var spSitiosAlmacenajes = new StackPanel();
                            // spTrazabilidad.Children.Add(spSitiosAlmacenajes);
                            foreach (var sitioRecepcion in sitiosRecepciones.Select(sr => sr.sr).Distinct().ToList())
                            {
                                // spSitiosAlmacenajes.Children.Add(new PlantillaSitioRecepcion() { DataContext = sitioRecepcion });
                                var tviSitioRecepcion = new TreeViewItem() { Header = sitioRecepcion.Nombre, IsExpanded = true };
                                tviMateriaPrima.Items.Add(tviSitioRecepcion);
                                foreach (var huecoMateriaPrima in context.HuecosMateriasPrimas.Where(hmp => hmp.HuecoRecepcion.SitioId == sitioRecepcion.SitioRecepcionId && hmp.MateriaPrimaId == materiaPrima.MateriaPrimaId).ToList())
                                {
                                    var tviHuecoMateriaPrima = new TreeViewItem() { Header = huecoMateriaPrima.HuecoRecepcion.Nombre, IsExpanded = true };
                                    tviSitioRecepcion.Items.Add(tviHuecoMateriaPrima);
                                }
                            }
                        }
                        break;
                    case Constantes.CODIGO_ELABORACIONES:
                        break;
                }
            }
        }
    }
}
