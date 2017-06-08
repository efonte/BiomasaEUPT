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

            if (codigo.Length == 10)
            {
                // Grid.SetColumn(tbCodigo, 0);
                Grid.SetColumnSpan(tbCodigo, 1);
                rdCodigo.Height = GridLength.Auto;

                switch (codigo[0].ToString())
                {
                    case Constantes.CODIGO_MATERIAS_PRIMAS:
                        if (context.MateriasPrimas.Any(mp => mp.Codigo == codigo))
                        {
                            var materiaPrima = context.MateriasPrimas.Single(mp => mp.Codigo == codigo);
                            Console.WriteLine(materiaPrima.Recepcion.NumeroAlbaran);
                            var pRecepcion = new PlantillaRecepcion();
                            pRecepcion.DataContext = materiaPrima.Recepcion;
                            spTrazabilidad.Children.Add(pRecepcion);
                            //Grid.SetRow(pRecepcion, 1);
                            //Grid.SetColumn(pRecepcion, 0);

                        }
                        break;
                    case Constantes.CODIGO_ELABORACIONES:
                        break;
                }
            }
        }
    }
}
