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

            // https://msdn.microsoft.com/en-us/library/jj574232(v=vs.113).aspx
            context.Configuration.LazyLoadingEnabled = false;
        }

        public Proveedor MateriaPrima(string codigo)
        {
            var materiaPrima = context.MateriasPrimas
                .Include("Recepcion.Proveedor.TipoProveedor")
                .Include("Recepcion.Proveedor.Municipio.Provincia.Comunidad.Pais")
                .Include("Recepcion.EstadoRecepcion")
                .Include("TipoMateriaPrima")
                .Include("Procedencia")
                .Include("HistorialHuecosRecepciones.HuecoRecepcion.SitioRecepcion")
                .Include("HistorialHuecosRecepciones.ProductosTerminadosComposiciones.ProductoTerminado.TipoProductoTerminado")
                .Include("HistorialHuecosRecepciones.ProductosTerminadosComposiciones.ProductoTerminado.HistorialHuecosAlmacenajes.HuecoAlmacenaje.SitioAlmacenaje")
                .Single(mp => mp.Codigo == codigo);
            var recepcion = materiaPrima.Recepcion;
            recepcion.MateriasPrimas = new List<MateriaPrima>() { materiaPrima };
            var proveedor = recepcion.Proveedor;
            proveedor.Recepciones = new List<Recepcion>() { recepcion };
            return proveedor;
        }

        public Proveedor Recepcion(string numeroAlbaran)
        {
            var recepcion = context.Recepciones
                .Include("EstadoRecepcion")
                .Include("Proveedor.TipoProveedor")
                .Include("Proveedor.Municipio.Provincia.Comunidad.Pais")
                .Include("MateriasPrimas.Procedencia")
                .Include("MateriasPrimas.TipoMateriaPrima")
                .Include("MateriasPrimas.HistorialHuecosRecepciones.HuecoRecepcion.SitioRecepcion")
                .Include("MateriasPrimas.HistorialHuecosRecepciones.ProductosTerminadosComposiciones.ProductoTerminado.TipoProductoTerminado")
                .Include("MateriasPrimas.HistorialHuecosRecepciones.ProductosTerminadosComposiciones.ProductoTerminado.HistorialHuecosAlmacenajes.HuecoAlmacenaje.SitioAlmacenaje")
                .Single(r => r.NumeroAlbaran == numeroAlbaran);
            var proveedor = recepcion.Proveedor;
            proveedor.Recepciones = new List<Recepcion>() { recepcion };
            return proveedor;
        }

        public List<Proveedor> ProductoTerminado(string codigo)
        {
            ////////////////////////////////////////////////////////////////// ESTÁ MAL
            var productoTerminado = context.ProductosTerminados
                .Include("TipoProductoTerminado")
                .Include("HistorialHuecosAlmacenajes.HuecoAlmacenaje.SitioAlmacenaje")
                .Include("ProductosTerminadosComposiciones.HistorialHuecoRecepcion.ProductosTerminadosComposiciones.HistorialHuecoRecepcion.HuecoRecepcion.SitioRecepcion")
                .Include("ProductosTerminadosComposiciones.HistorialHuecoRecepcion.ProductosTerminadosComposiciones.HistorialHuecoRecepcion.MateriaPrima.TipoMateriaPrima")
                .Include("ProductosTerminadosComposiciones.HistorialHuecoRecepcion.ProductosTerminadosComposiciones.HistorialHuecoRecepcion.MateriaPrima.Recepcion.Proveedor")
                .Single(mp => mp.Codigo == codigo);
            var productosTerminadosComposiciones = productoTerminado.ProductosTerminadosComposiciones.ToList();
            var materiasPrimas = new List<MateriaPrima>();
            foreach (var ptc in productosTerminadosComposiciones)
            {
                if (!materiasPrimas.Contains(ptc.HistorialHuecoRecepcion.MateriaPrima))
                {
                    materiasPrimas.Add(ptc.HistorialHuecoRecepcion.MateriaPrima);
                }
            }
            var recepciones = new List<Recepcion>();
            foreach (var mp in materiasPrimas)
            {
                if (!recepciones.Contains(mp.Recepcion))
                {
                    recepciones.Add(mp.Recepcion);
                }
            }
            var proveedores = new List<Proveedor>();
            foreach (var r in recepciones)
            {
                if (!proveedores.Contains(r.Proveedor))
                {
                    proveedores.Add(r.Proveedor);
                }
            }
            return proveedores;
        }
    }
}
