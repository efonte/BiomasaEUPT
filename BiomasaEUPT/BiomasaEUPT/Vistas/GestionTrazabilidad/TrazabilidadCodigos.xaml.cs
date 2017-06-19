using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Z.EntityFramework.Plus;

namespace BiomasaEUPT.Vistas.GestionTrazabilidad
{
    /// <summary>
    /// Lógica de interacción para TrazabilidadCodigos.xaml
    /// </summary>
    public partial class TrazabilidadCodigos : UserControl
    {

        /*  public IList ArbolRecepcionHijo
          {
              get
              {
                  var _arbolRecepcionHijo = new List<Recepcion>();
                  using (var context = new BiomasaEUPTContext())
                  {
                      foreach (var mp in context.MateriasPrimas.Where(mp => mp.Recepcion == _arbolRecepcionHijo.First()))
                      {

                      }
                  }
                  return _arbolRecepcionHijo;
              }
          }*/

        public ObservableCollection<Proveedor> ArbolAlmacenamiento { get; set; }
        public ObservableCollection<Proveedor> ArbolElaboracion { get; set; }
        public ObservableCollection<Proveedor> ArbolVenta { get; set; }

        public TrazabilidadCodigos()
        {
            InitializeComponent();

            using (var context = new BiomasaEUPTContext())
            {
                //context.MateriasPrimas.Load();
                // https://msdn.microsoft.com/en-us/data/jj574232.aspx
                // https://msdn.microsoft.com/en-us/library/jj574232(v=vs.113).aspx
                context.Configuration.LazyLoadingEnabled = false;
                // var proveedores = context.Proveedores.Where(p => p.Recepciones.Any(r => r.MateriasPrimas.Any(mp => mp.Codigo == "1000000001"))).Include("Recepciones.MateriasPrimas.TipoMateriaPrima").Include("Recepciones.MateriasPrimas.HistorialHuecosRecepciones");
                //Console.WriteLine(proveedores.First().Recepciones.First().NumeroAlbaran);
                // Console.WriteLine("---->>>>>>" + proveedores.First().Recepciones.First().MateriasPrimas.Count);


                /*var materiaPrima = context.MateriasPrimas
                    .Include("Recepcion.Proveedor")
                    .Include("TipoMateriaPrima")
                    .Include("HistorialHuecosRecepciones.HuecoRecepcion.SitioRecepcion")
                    .Single(mp => mp.Codigo == "1000000001");
                var recepcion = materiaPrima.Recepcion;
                recepcion.MateriasPrimas = new List<MateriaPrima>() { materiaPrima };
                var proveedor = recepcion.Proveedor;
                proveedor.Recepciones = new List<Recepcion>() { recepcion };*/

                /*var proveedores = context.Proveedores
                    .Select(p => new
                    {
                        p,
                        Recepciones = p.Recepciones.Where(r => r.RecepcionId == 1).Select(r => new
                        {
                            r,
                            MateriasPrimas = r.MateriasPrimas.Where(mp => mp.Codigo == "1000000001")
                        }).Select(x => x.MateriasPrimas)
                    })
                    .Select(x => x.p)
                    .ToList();*/

                /*var proveedores = context.Proveedores.Where(p => p.Recepciones.Any(r => r.MateriasPrimas.Any(mp => mp.Codigo == "1000000001")))
                    .Select(p => new
                    {
                        Proveedor = p,
                        Recepciones = p.Recepciones.Where(r => r.MateriasPrimas.Any(mp => mp.Codigo == "1000000001"))
                        .Select(r => new
                        {
                            Recepcion = r,
                            MateriasPrimas = r.MateriasPrimas.Where(mp => mp.Codigo == "1000000001")
                        }).ToList().Select(x => x.Recepcion)
                    })
                    .ToList().Select(x => x.Proveedor);

                var proveedores2 = context.Proveedores.IncludeFilter(p => p.Recepciones.Where(r => r.RecepcionId == 1).Select(r => r.MateriasPrimas.Where(mp => mp.MateriaPrimaId == 1)))
                     .ToList();*/



                /*  var buses = context.Busses.Where(b => b.IsDriving)
                              .Select(b => new
                              {
                                  b,
                                  Passengers = b.Passengers
                                                             .Where(p => p.Awake)
                              })
                              .AsEnumerable()
                              .Select(x => x.b)
                              .ToList();*/

                // var proveedor = context.Proveedores.Find(1);

                /*var materiaPrima = context.MateriasPrimas
                    .Include(mp => mp.Recepcion)
                    .Include(mp => mp.Recepcion.Proveedor)
                    .Include(mp => mp.HistorialHuecosRecepciones)
                    .Include(mp => mp.TipoMateriaPrima)
                    .Single(mp => mp.Codigo == "1000000001");

                var HistorialesHuecosRecepciones = context.HistorialHuecosRecepciones
                    .Include(hhr => hhr.MateriaPrima.Recepcion.Proveedor)
                    .Include(hhr => hhr.MateriaPrima.TipoMateriaPrima)
                    .Include(hhr => hhr.HuecoRecepcion.SitioRecepcion);*/

                /*var proveedor1 = context.Proveedores
                    .Include(p => p.Recepciones)
                    .Include(p => p.Recepciones.Select(r => r.MateriasPrimas.Any(mp => mp.Codigo == "1000000001")));*/
                //.Include(mp => mp.HistorialHuecosRecepciones.Select(hhr=>hhr.)
                /*var c = entities.AccountDefinitions.Where(p => p.ORG_ID == id)
                .Include(a => a.SDOrganization)
                .Include(a2 => a2.SiteDefinitions)
                .Include(a3 => a3.SDOrganization.AaaPostalAddresses)
                .Include(a4 => a4.SiteDefinitions.Select(a5 => a5.DepartmentDefinitions.Select(a6 => a6.SDUsers.Select(a7 => a7.AaaUser))));
                */
                //Console.WriteLine(materiaPrima.HistorialHuecosRecepciones.Count());
                // var proveedor1 = context.Proveedores.Where(p => p.Recepciones.Any(r => r.MateriasPrimas.Any(mp => mp.Codigo == "1000000001")));
                // Console.WriteLine(proveedor1.First().Recepciones.Count());

                /*   context.Entry(proveedor)
                      .Collection(b => b.Recepciones)
                        .Query()
                        .Where(p => p.MateriasPrimas.Any(mp => mp.Codigo == "1000000001"))
                        .Include("MateriasPrimas.TipoMateriaPrima")
                        .Include("MateriasPrimas.HistorialHuecosRecepciones.HuecoRecepcion.SitioRecepcion")
                        .Load();*/

                // context.Entry(proveedor).Collection(p => p.Recepciones).Load();
                //Console.WriteLine(proveedor.Recepciones.First().MateriasPrimas.First().HistorialHuecosRecepciones);

                // Load the posts with the 'entity-framework' tag related to a given blog  
                // using a string to specify the relationship  
                /*  context.Entry(blog)
                      .Collection("Posts")
                      .Query()
                      .Where(p => p.Tags.Contains("entity-framework")
                      .Load();
                      */

                // ArbolAlmacenamiento = new ObservableCollection<Proveedor>() { proveedor };

                // ArbolAlmacenamiento = new ObservableCollection<Proveedor>(proveedores);
                // ArbolAlmacenamiento = new ObservableCollection<HistorialHuecoRecepcion>(HistorialesHuecosRecepciones);
                ArbolAlmacenamiento = new ObservableCollection<Proveedor>();
                ArbolElaboracion = new ObservableCollection<Proveedor>();
                ArbolVenta = new ObservableCollection<Proveedor>();

            };
            DataContext = this;
        }

    }
}
