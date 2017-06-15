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

        public ObservableCollection<Proveedor> ArbolRecepcion { get; set; }
        public ObservableCollection<Proveedor> ArbolAlmacenamiento { get; set; }
        public ObservableCollection<Proveedor> ArbolVenta { get; set; }

        public TrazabilidadCodigos()
        {
            InitializeComponent();

            using (var context = new BiomasaEUPTContext())
            {
                context.MateriasPrimas.Load();
                // https://msdn.microsoft.com/en-us/data/jj574232.aspx
                var proveedores = context.Proveedores.Where(p => p.Recepciones.Where(r => r.MateriasPrimas.Where(mp => mp.Codigo == "1000000001").Any()).Any()).Include("Recepciones.MateriasPrimas.TipoMateriaPrima").Include("Recepciones.MateriasPrimas.HistorialHuecosRecepciones");
                //Console.WriteLine(proveedores.First().Recepciones.First().NumeroAlbaran);
                Console.WriteLine("---->>>>>>" + proveedores.First().Recepciones.First().MateriasPrimas.Count);
                ArbolRecepcion = new ObservableCollection<Proveedor>(proveedores);

                var consulta = context.MateriasPrimas.Where(mp => mp.Codigo == "1000000001")
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
                                            (hr1, sr) => new { hr1.mp, hr1.p, hr1.r, hr1.hhr, hr1.hr, sr }).Select(c => c.hr).Distinct().ToList();
                foreach (var c in consulta)
                {
                    Console.WriteLine(c.Nombre);
                }

                // ArbolRecepcion = new ObservableCollection<Proveedor>(consulta);

            };
            DataContext = this;
        }
    }
}
