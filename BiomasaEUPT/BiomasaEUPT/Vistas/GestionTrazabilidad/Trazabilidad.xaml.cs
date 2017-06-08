using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
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
            // Grid.SetColumn(tbCodigo, 0);
            Grid.SetColumnSpan(tbCodigo, 2);
            rdCodigo.Height = new GridLength(1, GridUnitType.Star);
            rdTrazabilidad.Height = GridLength.Auto;

            if (codigo.Length == 10)
            {
                // Grid.SetColumn(tbCodigo, 0);
                Grid.SetColumnSpan(tbCodigo, 1);
                rdCodigo.Height = GridLength.Auto;
                rdTrazabilidad.Height = new GridLength(1, GridUnitType.Star);

                switch (codigo[0].ToString())
                {
                    case Constantes.CODIGO_MATERIAS_PRIMAS:
                        if (context.MateriasPrimas.Any(mp => mp.Codigo == codigo))
                        {
                            var materiaPrima = context.MateriasPrimas.Single(mp => mp.Codigo == codigo);
                            Console.WriteLine(materiaPrima.Recepcion.NumeroAlbaran);
                            spTrazabilidad.Children.Add(new PlantillaRecepcion() { DataContext = materiaPrima.Recepcion, Margin=new Thickness(10,10,10,10) });
                            spTrazabilidad.Children.Add(new PlantillaMateriaPrima() { Margin = new Thickness(10, 10, 10, 10) });
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
                            spTrazabilidad.Children.Add(spSitiosAlmacenajes);
                            foreach (var sitioRecepcion in sitiosRecepciones.Select(sr => sr.sr).ToList())
                            {
                                spSitiosAlmacenajes.Children.Add(new PlantillaSitioRecepcion() { DataContext = sitioRecepcion });
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
