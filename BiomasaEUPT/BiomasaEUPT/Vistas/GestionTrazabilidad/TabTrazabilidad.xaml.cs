using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
using System.Xml;

namespace BiomasaEUPT.Vistas.GestionTrazabilidad
{
    /// <summary>
    /// Lógica de interacción para TabTrazabilidad.xaml
    /// </summary>
    public partial class TabTrazabilidad : UserControl
    {
        private BiomasaEUPTContext context;
        private Trazabilidad trazabilidad;
        public TabTrazabilidad()
        {
            InitializeComponent();
            DataContext = this;
            trazabilidad = new Trazabilidad();
            ucTrazabilidadCodigos.tbCodigo.TextChanged += TbCodigo_TextChanged;
            ucTrazabilidadCodigos.bPdf.Click += BPdf_Click;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = new BiomasaEUPTContext();
            context.Configuration.LazyLoadingEnabled = false;

            // context.Configuration.ProxyCreationEnabled = false;
            //Console.WriteLine(context.Proveedores.Where(p => p.Recepciones.Where(r => r.MateriasPrimas.Where(mp => mp.Codigo == "1000000001").Any()).Any()).Count());
            /*var proveedor = context.Proveedores.SingleOrDefault(p => p.ProveedorId == 1);
              using (XmlWriter writer = XmlWriter.Create(@"C:\Users\usuario\Desktop\qqqwwweee\Agreement.xml"))
               {
                   DataContractSerializer serializer = new DataContractSerializer(proveedor.GetType());
                   serializer.WriteObject(writer, proveedor);
               }*/
        }

        private void TbCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            string codigo = (sender as TextBox).Text;
            ucTrazabilidadCodigos.ArbolRecepcion.Clear();
            ucTrazabilidadCodigos.bPdf.Visibility = Visibility.Collapsed;

            if (context.Recepciones.Any(r => r.NumeroAlbaran == codigo))
            {
                //ucTrazabilidadCodigos.bPdf.Visibility = Visibility.Visible;
                var proveedor = trazabilidad.Recepcion(codigo);
                ucTrazabilidadCodigos.ArbolRecepcion.Add(proveedor);
            }
            else if (codigo.Length == 10)
            {
                switch (codigo[0].ToString())
                {
                    case Constantes.CODIGO_MATERIAS_PRIMAS:
                        if (context.MateriasPrimas.Any(mp => mp.Codigo == codigo))
                        {
                            ucTrazabilidadCodigos.bPdf.Visibility = Visibility.Visible;
                            var proveedor = trazabilidad.MateriaPrima(codigo);
                            ucTrazabilidadCodigos.ArbolRecepcion.Add(proveedor);
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
                                            (hr1, sr) => new { hr1.mp, hr1.p, hr1.r, hr1.hhr, hr1.hr, sr }).Distinct().ToList();*/
                        break;


                    case Constantes.CODIGO_ELABORACIONES:
                        if (context.ProductosTerminados.Any(pt => pt.Codigo == codigo))
                        {
                            //ucTrazabilidadCodigos.bPdf.Visibility = Visibility.Visible;
                            var proveedores = trazabilidad.ProductoTerminado(codigo);
                            proveedores.ForEach(ucTrazabilidadCodigos.ArbolRecepcion.Add);
                        }
                        break;


                    case Constantes.CODIGO_VENTAS:
                        if (context.ProductosEnvasados.Any(mp => mp.Codigo == codigo))
                        {
                            //ucTrazabilidadCodigos.bPdf.Visibility = Visibility.Visible;
                            //var proveedores = trazabilidad.ProductoTerminado(codigo);
                            //proveedores.ForEach(ucTrazabilidadCodigos.ArbolRecepcion.Add);
                        }
                        break;
                }
            }
        }

        private void BPdf_Click(object sender, RoutedEventArgs e)
        {
            InformePDF informe = new InformePDF(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Informes\");
            var rutaInforme = "";
            var codigo = ucTrazabilidadCodigos.tbCodigo.Text;
            if (context.Recepciones.Any(r => r.NumeroAlbaran == codigo))
            {
                //rutaInforme = informe.GenerarPDFRecepcion(trazabilidad.Recepcion(codigo));
            }
            else
            {
                switch (codigo[0].ToString())
                {
                    case Constantes.CODIGO_MATERIAS_PRIMAS:
                        rutaInforme = informe.GenerarPDFMateriaPrima(trazabilidad.MateriaPrima(codigo));
                        break;

                    case Constantes.CODIGO_ELABORACIONES:
                        //rutaInforme = informe.GenerarPDFProductoTerminado(trazabilidad.ProductoTerminado(codigo));
                        break;

                    case Constantes.CODIGO_VENTAS:
                        //rutaInforme = informe.GenerarPDFProductoEnvasado(trazabilidad.ProductoEnvasado(codigo));
                        break;
                }
            }

            System.Diagnostics.Process.Start(rutaInforme);
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
