using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
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
            DataContext = this;
            ucTrazabilidadCodigos.tbCodigo.TextChanged += TbCodigo_TextChanged;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = new BiomasaEUPTContext();
            Console.WriteLine(context.Proveedores.Where(p => p.Recepciones.Where(r => r.MateriasPrimas.Where(mp => mp.Codigo == "1000000001").Any()).Any()).Count());
        }

        private void TbCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            string codigo = (sender as TextBox).Text;
            ucTrazabilidadCodigos.tvAlmacenamiento.Items.Clear();
            if (codigo.Length == 10)
            {
                switch (codigo[0].ToString())
                {
                    case Constantes.CODIGO_MATERIAS_PRIMAS:
                        if (context.MateriasPrimas.Any(mp => mp.Codigo == codigo))
                        {
                            var materiaPrima = context.MateriasPrimas.Single(mp => mp.Codigo == codigo);
                            var tviProveedor = TreeViewItemIcono(materiaPrima.Recepcion.Proveedor.RazonSocial, PackIconKind.Worker);
                            ucTrazabilidadCodigos.tvAlmacenamiento.Items.Add(tviProveedor);
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

                        /*  var consulta = context.MateriasPrimas
                                         .Join(context.Recepciones,
                                            mp => mp.RecepcionId,
                                            r => r.RecepcionId,
                                            (mp, r) => new { mp, r })
                                         .Join(context.Proveedores,
                                            r1 => r1.r.ProveedorId,
                                            p => p.ProveedorId,
                                            (r1, p) => new { r1.mp, r1.r, p })
                                         .Join(context.HistorialHuecosRecepciones,
                                            p1 => p1.mp.MateriaPrimaId,
                                            hhr => hhr.MateriaPrimaId,
                                            (p1, hhr) => new { p1.mp, p1.r, p1.p, hhr })
                                         .Join(context.HuecosRecepciones,
                                            hhr1 => hhr1.hhr.HuecoRecepcionId,
                                            hr => hr.HuecoRecepcionId,
                                            (hhr1, hr) => new { hhr1.mp, hhr1.r, hhr1.p, hhr1.hhr, hr })
                                         .Join(context.SitiosRecepciones,
                                            hr1 => hr1.hr.SitioId,
                                            sr => sr.SitioRecepcionId,
                                            (hr1, sr) => new { hr1.mp, hr1.p, hr1.r, hr1.hhr, hr1.hr, sr }).Distinct().ToList();

                           foreach (var c in consulta)
                           {
                               Console.WriteLine(c.sr.Nombre);                         
                               Console.WriteLine(c.hr.Nombre);
                           }*/

                        /* ucTrazabilidadCodigos.ArbolRecepcion.Clear();
                         var recepciones = new List<Recepcion>();
                         var materiaPrima = context.MateriasPrimas.Single(mp => mp.Codigo == codigo);
                         recepciones.Add(materiaPrima.Recepcion);
                         ucTrazabilidadCodigos.ArbolRecepcion.Add(new CollectionContainer { Collection = recepciones });*/
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
