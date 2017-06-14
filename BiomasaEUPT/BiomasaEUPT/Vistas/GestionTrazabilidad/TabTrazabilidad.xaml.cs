using BiomasaEUPT.Modelos;
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
    /// Lógica de interacción para TabTrazabilidad.xaml
    /// </summary>
    public partial class TabTrazabilidad : UserControl
    {
        private BiomasaEUPTContext context;
        public TabTrazabilidad()
        {
            InitializeComponent();
            trazabilidadCodigos.tbCodigo.TextChanged += TbCodigo_TextChanged;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = new BiomasaEUPTContext();
        }

        private void TbCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            string codigo = (sender as TextBox).Text;
            trazabilidadCodigos.tvAlmacenamiento.Items.Clear();
            if (codigo.Length == 10)
            {
                switch (codigo[0].ToString())
                {
                    case Constantes.CODIGO_MATERIAS_PRIMAS:
                        if (context.MateriasPrimas.Any(mp => mp.Codigo == codigo))
                        {
                            var materiaPrima = context.MateriasPrimas.Single(mp => mp.Codigo == codigo);
                            var tviProveedor = TreeViewItemIcono(materiaPrima.Recepcion.Proveedor.RazonSocial, PackIconKind.Worker);
                            trazabilidadCodigos.tvAlmacenamiento.Items.Add(tviProveedor);
                            var tviRecepcion = TreeViewItemIcono(materiaPrima.Recepcion.NumeroAlbaran, PackIconKind.Truck);
                            tviProveedor.Items.Add(tviRecepcion);
                            var tviMateriaPrima = TreeViewItemIcono(materiaPrima.TipoMateriaPrima.Nombre, PackIconKind.Tree);
                            tviRecepcion.Items.Add(tviMateriaPrima);
                            var sitiosRecepciones = (from hhr in context.HistorialHuecosRecepciones
                                                     join hr in context.HuecosRecepciones on hhr.HuecoRecepcionId equals hr.HuecoRecepcionId
                                                     join sr in context.SitiosRecepciones on hr.SitioId equals sr.SitioRecepcionId
                                                     where hhr.MateriaPrimaId == materiaPrima.MateriaPrimaId
                                                     select new { sr });

                            foreach (var sitioRecepcion in sitiosRecepciones.Select(sr => sr.sr).Distinct().ToList())
                            {
                                var tviSitioRecepcion = TreeViewItemIcono(sitioRecepcion.Nombre, PackIconKind.Texture);
                                tviMateriaPrima.Items.Add(tviSitioRecepcion);
                                foreach (var historialHuecoRecepcion in context.HistorialHuecosRecepciones.Where(hmp => hmp.HuecoRecepcion.SitioId == sitioRecepcion.SitioRecepcionId && hmp.MateriaPrimaId == materiaPrima.MateriaPrimaId).ToList())
                                {
                                    var tviHistorialHuecoRecepcion = TreeViewItemIcono(historialHuecoRecepcion.HuecoRecepcion.Nombre, PackIconKind.Tree);
                                    tviSitioRecepcion.Items.Add(tviHistorialHuecoRecepcion);
                                }
                            }
                        }

                        break;


                    case Constantes.CODIGO_ELABORACIONES:

                        break;


                    case Constantes.CODIGO_VENTAS:

                        break;
                }
            }
        }

        private TreeViewItem TreeViewItemIcono(string texto, PackIconKind icono)
        {
            var tviTexto = new TextBlock() { Text = texto, Margin = new Thickness(8, 0, 0, 0) };
            var tviIcono = new PackIcon() { Kind = icono };
            var tviHeader = new StackPanel() { Orientation = Orientation.Horizontal };
            tviHeader.Children.Add(tviIcono);
            tviHeader.Children.Add(tviTexto);
            return new TreeViewItem() { Header = tviHeader, IsExpanded = true };
        }

    }
}
