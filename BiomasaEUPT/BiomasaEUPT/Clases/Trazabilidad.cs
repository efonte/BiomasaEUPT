using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Clases
{
    public class Trazabilidad
    {
        private BiomasaEUPTContext context;
        public Trazabilidad()
        {
            context = new BiomasaEUPTContext();
            context.Configuration.LazyLoadingEnabled = false;
        }

        public Proveedor CodigoMateriaPrima(string codigo)
        {
            var materiaPrima = context.MateriasPrimas
                              .Include("Recepcion.Proveedor.TipoProveedor")
                              .Include("TipoMateriaPrima")
                              .Include("HistorialHuecosRecepciones.HuecoRecepcion.SitioRecepcion")
                              .Include("HistorialHuecosRecepciones.ProductosTerminadosComposiciones.ProductoTerminado.TipoProductoTerminado")
                              .Single(mp => mp.Codigo == codigo);
            var recepcion = materiaPrima.Recepcion;
            //new ProductoTerminado().ProductosTerminadosComposiciones.First().HistorialHuecoRecepcion
            recepcion.MateriasPrimas = new List<MateriaPrima>() { materiaPrima };
            var proveedor = recepcion.Proveedor;
            proveedor.Recepciones = new List<Recepcion>() { recepcion };
            return proveedor;
        }

        public Proveedor NumeroAlbaranRecepcion(string numeroAlbaran)
        {
            var recepcion = context.Recepciones
                            .Include("Proveedor.TipoProveedor")
                            .Include("MateriasPrimas.TipoMateriaPrima")
                            .Include("MateriasPrimas.HistorialHuecosRecepciones.HuecoRecepcion.SitioRecepcion")
                            .Single(r => r.NumeroAlbaran == numeroAlbaran);
            var proveedor = recepcion.Proveedor;
            proveedor.Recepciones = new List<Recepcion>() { recepcion };
            return proveedor;
        }

    }
}
