using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using iText.Barcodes;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Clases
{
    public class InformePDF : IInforme
    {
        private string ruta;
        private DateTime fechaCreacion;

        internal static PdfFont helvetica = null;
        internal static PdfFont helveticaNegrita = null;
        internal static PdfFont calibri = null;
        internal static PdfFont cambriaNegrita = null;

        internal static float GROSOR_BORDE_CELDA = 0.5f;
        internal static float GROSOR_BORDE_TITULO = 0.75f;

        // Orientaciones Páginas
        internal static PdfNumber INVERTEDPORTRAIT = new PdfNumber(180);
        internal static PdfNumber LANDSCAPE = new PdfNumber(90);
        internal static PdfNumber PORTRAIT = new PdfNumber(0);
        internal static PdfNumber SEASCAPE = new PdfNumber(270);

        internal static Color COLOR_FONDO_PAGINA = null;

        internal static Style estiloCelda = null;


        public InformePDF()
        {
            helvetica = PdfFontFactory.CreateFont(FontConstants.HELVETICA);
            helveticaNegrita = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);
            PdfFontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "/fonts/calibri.ttf", "Calibri");
            PdfFontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "/fonts/cambriab.ttf", "Cambria Negrita");
            calibri = PdfFontFactory.CreateRegisteredFont("Calibri", PdfEncodings.IDENTITY_H, true);
            cambriaNegrita = PdfFontFactory.CreateRegisteredFont("Cambria Negrita", PdfEncodings.IDENTITY_H, true);

            estiloCelda = new Style().SetFont(calibri).SetFontSize(13).SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.CENTER);

            //COLOR_FONDO_PAGINA = new DeviceCmyk(0.208f, 0, 0.584f, 0);
        }

        public InformePDF(string ruta) : this()
        {
            this.ruta = ruta;

            // Si la ruta no terminado con "\" se le añade.
            if (!ruta.EndsWith("\\"))
                ruta += "\\";

            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);
        }

        public string GenerarInformeRecepcion(Proveedor proveedor)
        {
            var recepcion = proveedor.Recepciones.First();
            // Se guarda en una variable la fecha de creación para que tanto la fecha del nombre del PDF como la que hay dentro del PDF sean las mismas.
            fechaCreacion = DateTime.Now;
            var nombrePdf = ruta + "Recepción #" + recepcion.NumeroAlbaran + " " + fechaCreacion.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";

            PdfWriter writer = new PdfWriter(nombrePdf);

            PdfDocument pdfDoc = new PdfDocument(writer);
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.AddCreationDate();
            info.SetAuthor("BiomasaEUPT");
            info.SetCreator("BiomasaEUPT");
            info.SetTitle("Recepción #" + recepcion.NumeroAlbaran + " " + fechaCreacion.ToString("dd/MM/yyyy HH:mm:ss"));
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new FinPaginaEventHandler(this));

            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());
            //doc.SetMargins(70, 70, 85, 85);

            var tablaProvRecep = new Table(new float[] { 1, 1 }).SetWidthPercent(100);
            tablaProvRecep.AddCell(new Cell().Add(Titulo("Proveedor")).SetFont(calibri).SetFontSize(13).SetBorder(Border.NO_BORDER).SetPaddingRight(10));
            tablaProvRecep.AddCell(new Cell().Add(Titulo("Recepción")).SetFont(calibri).SetFontSize(13).SetBorder(Border.NO_BORDER).SetPaddingLeft(10));
            tablaProvRecep.AddCell(new Cell().Add(TablaProveedor(proveedor)).SetFont(calibri).SetFontSize(13).SetBorder(Border.NO_BORDER).SetPaddingRight(10));
            tablaProvRecep.AddCell(new Cell().Add(TablaRecepcion(recepcion)).SetFont(calibri).SetFontSize(13).SetBorder(Border.NO_BORDER).SetPaddingLeft(10));
            doc.Add(tablaProvRecep);
            bool primeraVez = true;
            foreach (var materiaPrima in recepcion.MateriasPrimas.ToList())
            {
                if (primeraVez)
                {
                    doc.Add(new Paragraph("\n"));
                    primeraVez = false;
                }
                else
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }
                doc.Add(Titulo("Materia Prima #" + materiaPrima.Codigo));
                var tablaCodigo = new Table(new float[] { 1, 1, 1, 1, 1 }).SetWidthPercent(100);
                tablaCodigo.AddCell(CeldaTituloVertical("Tipo").SetVerticalAlignment(VerticalAlignment.TOP));
                tablaCodigo.AddCell(CeldaVertical(materiaPrima.TipoMateriaPrima.Nombre).SetVerticalAlignment(VerticalAlignment.TOP));
                tablaCodigo.AddCell(CeldaTituloVertical("Cantidad").SetVerticalAlignment(VerticalAlignment.TOP));
                tablaCodigo.AddCell(CeldaVertical((materiaPrima.TipoMateriaPrima.MedidoEnUnidades == true) ? (materiaPrima.Unidades + " ud.") : (materiaPrima.Volumen + " m³")).SetVerticalAlignment(VerticalAlignment.TOP));
                var codigoBarras = new Barcode128(pdfDoc);
                codigoBarras.SetCodeType(Barcode128.CODE128);
                codigoBarras.SetCode(materiaPrima.Codigo);
                Rectangle rect = codigoBarras.GetBarcodeSize();
                Image imagenCodigo = new Image(codigoBarras.CreateFormXObject(pdfDoc)).SetWidth(100);
                tablaCodigo.AddCell(new Cell().Add(imagenCodigo).AddStyle(estiloCelda).SetPaddingLeft(25));
                doc.Add(tablaCodigo);

                doc.Add(new Paragraph("\n"));

                doc.Add(TablaMateriaPrima(materiaPrima));

            }

            doc.Close();

            return nombrePdf;
        }

        public string GenerarInformeMateriaPrima(Proveedor proveedor)
        {
            var materiaPrima = proveedor.Recepciones.First().MateriasPrimas.First();

            // Se guarda en una variable la fecha de creación para que tanto la fecha del nombre del PDF como la que hay dentro del PDF sean las mismas.
            fechaCreacion = DateTime.Now;
            var nombrePdf = ruta + "Materia Prima #" + materiaPrima.Codigo + " " + fechaCreacion.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";

            PdfWriter writer = new PdfWriter(nombrePdf);

            PdfDocument pdfDoc = new PdfDocument(writer);
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.AddCreationDate();
            info.SetAuthor("BiomasaEUPT");
            info.SetCreator("BiomasaEUPT");
            info.SetTitle("Materia Prima #" + materiaPrima.Codigo + " " + fechaCreacion.ToString("dd/MM/yyyy HH:mm:ss"));
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new FinPaginaEventHandler(this));

            //  OrientacionPaginaEventHandler orientacionPaginaEventHandler = new OrientacionPaginaEventHandler();
            //  pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, orientacionPaginaEventHandler);

            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());
            //doc.SetMargins(70, 70, 85, 85);

            /*  var barcode = new Barcode128(pdfDoc);
              barcode.SetCodeType(Barcode128.CODE128);
              barcode.SetCode(materiaPrima.Codigo);
              Rectangle rect = barcode.GetBarcodeSize();

              Image image = new Image(barcode.CreateFormXObject(pdfDoc));
              // image.SetRotationAngle(Math.PI / 2);
              //  image.SetAutoScale(true);
              doc.Add(image);

              doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));*/

            var tablaProvRecep = new Table(new float[] { 1, 1 }).SetWidthPercent(100);
            tablaProvRecep.AddCell(new Cell().Add(Titulo("Proveedor")).SetFont(calibri).SetFontSize(13).SetBorder(Border.NO_BORDER).SetPaddingRight(10));
            tablaProvRecep.AddCell(new Cell().Add(Titulo("Recepción")).SetFont(calibri).SetFontSize(13).SetBorder(Border.NO_BORDER).SetPaddingLeft(10));
            tablaProvRecep.AddCell(new Cell().Add(TablaProveedor(proveedor)).SetFont(calibri).SetFontSize(13).SetBorder(Border.NO_BORDER).SetPaddingRight(10));
            tablaProvRecep.AddCell(new Cell().Add(TablaRecepcion(proveedor.Recepciones.First())).SetFont(calibri).SetFontSize(13).SetBorder(Border.NO_BORDER).SetPaddingLeft(10));
            doc.Add(tablaProvRecep);

            doc.Add(new Paragraph("\n"));

            /*  doc.Add(Titulo("Proveedor"));
              doc.Add(TablaProveedor(proveedor));

              doc.Add(new Paragraph("\n"));

              doc.Add(Titulo("Recepción"));
              doc.Add(TablaRecepcion(proveedor.Recepciones.First()));

              doc.Add(new Paragraph("\n"));*/

            doc.Add(Titulo("Materia Prima"));
            var tablaCodigo = new Table(new float[] { 1, 1, 1, 1, 1 }).SetWidthPercent(100);
            tablaCodigo.AddCell(CeldaTituloVertical("Tipo").SetVerticalAlignment(VerticalAlignment.TOP));
            tablaCodigo.AddCell(CeldaVertical(materiaPrima.TipoMateriaPrima.Nombre).SetVerticalAlignment(VerticalAlignment.TOP));
            tablaCodigo.AddCell(CeldaTituloVertical("Cantidad").SetVerticalAlignment(VerticalAlignment.TOP));
            tablaCodigo.AddCell(CeldaVertical((materiaPrima.TipoMateriaPrima.MedidoEnUnidades == true) ? (materiaPrima.Unidades + " ud.") : (materiaPrima.Volumen + " m³")).SetVerticalAlignment(VerticalAlignment.TOP));
            var codigoBarras = new Barcode128(pdfDoc);
            codigoBarras.SetCodeType(Barcode128.CODE128);
            codigoBarras.SetCode(materiaPrima.Codigo);
            Rectangle rect = codigoBarras.GetBarcodeSize();
            Image imagenCodigo = new Image(codigoBarras.CreateFormXObject(pdfDoc)).SetWidth(100);
            tablaCodigo.AddCell(new Cell().Add(imagenCodigo).AddStyle(estiloCelda).SetPaddingLeft(25));
            doc.Add(tablaCodigo);

            doc.Add(new Paragraph("\n"));

            doc.Add(TablaMateriaPrima(materiaPrima));

            // orientacionPaginaEventHandler.Orientacion = LANDSCAPE;
            /*doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            Table table = new Table(new float[] { 25, 50 })
                             .AddCell(new Cell().Add(new Paragraph("cell 1, 1").SetRotationAngle((Math.PI / 2))))
                             .AddCell(new Cell().Add(new Paragraph("cell 1, 2").SetRotationAngle((Math.PI / 3))))
                             .AddCell(new Cell().Add(new Paragraph("cell 2, 1").SetRotationAngle(-(Math.PI / 2))))
                             .AddCell(new Cell().Add(new Paragraph("cell 2, 2").SetRotationAngle((Math.PI))));
            doc.Add(table);*/

            doc.Close();

            return nombrePdf;
        }

        public string GenerarInformeProductoTerminado(List<Proveedor> proveedores)
        {
            var productoTerminado = proveedores.First().Recepciones.First().MateriasPrimas.First().HistorialHuecosRecepciones.First().ProductosTerminadosComposiciones.First().ProductoTerminado;

            // Se guarda en una variable la fecha de creación para que tanto la fecha del nombre del PDF como la que hay dentro del PDF sean las mismas.
            fechaCreacion = DateTime.Now;
            var nombrePdf = ruta + "Producto Terminado #" + productoTerminado.Codigo + " " + fechaCreacion.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";

            PdfWriter writer = new PdfWriter(nombrePdf);

            PdfDocument pdfDoc = new PdfDocument(writer);
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.AddCreationDate();
            info.SetAuthor("BiomasaEUPT");
            info.SetCreator("BiomasaEUPT");
            info.SetTitle("Producto Terminado #" + productoTerminado.Codigo + " " + fechaCreacion.ToString("dd/MM/yyyy HH:mm:ss"));
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new FinPaginaEventHandler(this));

            //  OrientacionPaginaEventHandler orientacionPaginaEventHandler = new OrientacionPaginaEventHandler();
            //  pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, orientacionPaginaEventHandler);

            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());
            //doc.SetMargins(70, 70, 85, 85);          

            doc.Add(Titulo("Producto Terminado"));
            var tablaCodigo = new Table(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 }).SetWidthPercent(100);
            tablaCodigo.AddCell(CeldaTituloVertical("Tipo").SetVerticalAlignment(VerticalAlignment.TOP));
            tablaCodigo.AddCell(CeldaVertical(productoTerminado.TipoProductoTerminado.Nombre).SetVerticalAlignment(VerticalAlignment.TOP));
            tablaCodigo.AddCell(CeldaTituloVertical("Humedad").SetVerticalAlignment(VerticalAlignment.TOP));
            tablaCodigo.AddCell(CeldaVertical(productoTerminado.TipoProductoTerminado.Humedad + "%").SetVerticalAlignment(VerticalAlignment.TOP));
            tablaCodigo.AddCell(CeldaTituloVertical("Tamaño").SetVerticalAlignment(VerticalAlignment.TOP));
            tablaCodigo.AddCell(CeldaVertical(productoTerminado.TipoProductoTerminado.Tamano).SetVerticalAlignment(VerticalAlignment.TOP));
            tablaCodigo.AddCell(CeldaTituloVertical("Cantidad").SetVerticalAlignment(VerticalAlignment.TOP));
            tablaCodigo.AddCell(CeldaVertical((productoTerminado.TipoProductoTerminado.MedidoEnUnidades == true) ? (productoTerminado.Unidades + " ud.") : (productoTerminado.Volumen + " m³")).SetVerticalAlignment(VerticalAlignment.TOP));
            var codigoBarras = new Barcode128(pdfDoc);
            codigoBarras.SetCodeType(Barcode128.CODE128);
            codigoBarras.SetCode(productoTerminado.Codigo);
            Rectangle rect = codigoBarras.GetBarcodeSize();
            Image imagenCodigo = new Image(codigoBarras.CreateFormXObject(pdfDoc)).SetWidth(100);
            tablaCodigo.AddCell(new Cell().Add(imagenCodigo).AddStyle(estiloCelda).SetPaddingLeft(25));
            doc.Add(tablaCodigo);

            doc.Add(new Paragraph("\n"));

            doc.Add(TablaProductoTerminado(productoTerminado));

            doc.Close();

            return nombrePdf;
        }

        public string GenerarInformeProductoEnvasado(List<Proveedor> proveedores)
        {
            throw new NotImplementedException();
        }

        private Table TablaRecepcion(Recepcion recepcion)
        {
            var tablaRecepcion = new Table(new float[] { 1, 1 }).SetWidthPercent(100);
            tablaRecepcion.AddCell(CeldaTituloVertical("Fecha Recepción"));
            tablaRecepcion.AddCell(CeldaVertical(recepcion.FechaRecepcion.ToString("dd/MM/yyyy HH:mm")));
            tablaRecepcion.AddCell(CeldaTituloVertical("Nº de Albarán"));
            tablaRecepcion.AddCell(CeldaVertical(recepcion.NumeroAlbaran).SetFont(calibri));

            return tablaRecepcion;
        }

        private Table TablaProveedor(Proveedor proveedor)
        {
            var tablaProveedor = new Table(new float[] { 1, 1 }).SetWidthPercent(100);
            tablaProveedor.AddCell(CeldaTituloVertical("Razon Social"));
            tablaProveedor.AddCell(CeldaVertical(proveedor.RazonSocial));
            tablaProveedor.AddCell(CeldaTituloVertical("NIF"));
            tablaProveedor.AddCell(CeldaVertical(proveedor.Nif));
            tablaProveedor.AddCell(CeldaTituloVertical("Tipo"));
            tablaProveedor.AddCell(CeldaVertical(proveedor.TipoProveedor.Nombre));

            return tablaProveedor;
        }

        private Table TablaMateriaPrima(MateriaPrima materiaPrima)
        {
            var tablaMP = new Table(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 }).SetWidthPercent(100);
            tablaMP.AddHeaderCell(CeldaTitulo("Recepción", 1, 4).SetBorderBottom(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));
            tablaMP.AddHeaderCell(CeldaTitulo("Elaboración", 1, 5).SetBorderBottom(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));
            tablaMP.AddHeaderCell(CeldaTitulo("Sitio"));
            tablaMP.AddHeaderCell(CeldaTitulo("Hueco"));
            tablaMP.AddHeaderCell(CeldaTitulo("Capacidad Total"));
            tablaMP.AddHeaderCell(CeldaTitulo("Cantidad Almacenada"));
            tablaMP.AddHeaderCell(CeldaTitulo("Cantidad Utilizada"));
            tablaMP.AddHeaderCell(CeldaTitulo("Producto Terminado"));
            tablaMP.AddHeaderCell(CeldaTitulo("Cantidad Producida"));
            tablaMP.AddHeaderCell(CeldaTitulo("Sitio"));
            tablaMP.AddHeaderCell(CeldaTitulo("Hueco"));

            foreach (var sr in materiaPrima.HistorialHuecosRecepciones.Select(hhr => hhr.HuecoRecepcion.SitioRecepcion).Distinct().ToList())
            {
                var huecosAlmacenajesSR = materiaPrima.HistorialHuecosRecepciones.SelectMany(hhr => hhr.ProductosTerminadosComposiciones).Select(ptc => ptc.ProductoTerminado).SelectMany(pt => pt.HistorialHuecosAlmacenajes);
                var filasSR = huecosAlmacenajesSR.Count();

                // Sitio de recepción
                tablaMP.AddCell(Celda(sr.Nombre, filasSR));

                foreach (var hhr in materiaPrima.HistorialHuecosRecepciones.Where(hhr => hhr.HuecoRecepcion.SitioRecepcion == sr))
                {
                    var huecosAlmacenajesHHR = hhr.ProductosTerminadosComposiciones.Select(ptc => ptc.ProductoTerminado).SelectMany(pt => pt.HistorialHuecosAlmacenajes).ToList();
                    var filasHHR = 1;
                    var filasHRCapacidadTotal = 1;
                    var filasHRCantidadAlmacenada = 1;
                    if (huecosAlmacenajesHHR.Count() > 0)
                    {
                        filasHHR = huecosAlmacenajesHHR.Count();
                        filasHRCapacidadTotal = huecosAlmacenajesHHR.Count();
                        filasHRCantidadAlmacenada = huecosAlmacenajesHHR.Count();
                    }

                    // Hueco de recepción
                    tablaMP.AddCell(Celda(hhr.HuecoRecepcion.Nombre, filasHHR));

                    // Capacidad total de cada hueco de recepción                   
                    tablaMP.AddCell(Celda($"{hhr.HuecoRecepcion.VolumenTotal.ToString()} m³ / {hhr.HuecoRecepcion.UnidadesTotales} ud.", filasHRCapacidadTotal));

                    // Cantidad de materia prima almacenada en cada hueco de recepción
                    tablaMP.AddCell(Celda((materiaPrima.TipoMateriaPrima.MedidoEnUnidades == true) ? (hhr.Unidades + " ud.") : (hhr.Volumen + " m³"), filasHRCantidadAlmacenada));

                    if (huecosAlmacenajesHHR.Count() == 0)
                    {
                        // Cantidad de materia prima utilizada de cada hueco de recepción
                        tablaMP.AddCell(Celda("-"));
                        // Producto terminado
                        tablaMP.AddCell(Celda("-"));
                        // Cantidad de producto terminado producido
                        tablaMP.AddCell(Celda("-"));
                        // Sitio de almacenaje
                        tablaMP.AddCell(Celda("-"));
                        // Hueco de almacenaje
                        tablaMP.AddCell(Celda("-"));
                    }
                    else
                    {
                        foreach (var ptc in hhr.ProductosTerminadosComposiciones)
                        {
                            var huecosAlmacenajesPTC = ptc.ProductoTerminado.HistorialHuecosAlmacenajes.Select(hha => hha.HuecoAlmacenaje).ToList();
                            var sitiosAlmacenajesPTC = huecosAlmacenajesPTC.Select(ha => ha.SitioAlmacenaje).Distinct().ToList();

                            var filasHRCantidadUtilizada = huecosAlmacenajesPTC.Count();
                            var filasPT = huecosAlmacenajesPTC.Count();

                            // Cantidad de materia prima utilizada de cada hueco de recepción
                            tablaMP.AddCell(Celda((materiaPrima.TipoMateriaPrima.MedidoEnUnidades == true) ? (ptc.Unidades + " ud.") : (ptc.Volumen + " m³"), filasHRCantidadUtilizada));

                            // Producto terminado                        
                            tablaMP.AddCell(Celda(ptc.ProductoTerminado.TipoProductoTerminado.Nombre, filasPT));

                            // Cantidad producida del producto terminado
                            tablaMP.AddCell(Celda((ptc.ProductoTerminado.TipoProductoTerminado.MedidoEnUnidades == true) ? (ptc.ProductoTerminado.Unidades + " ud.") : (ptc.ProductoTerminado.Volumen + " m³"), filasHRCantidadUtilizada));

                            foreach (var sa in sitiosAlmacenajesPTC)
                            {
                                var historialesHuecosAlmacenajesSA = ptc.ProductoTerminado.HistorialHuecosAlmacenajes.Where(hha => hha.HuecoAlmacenaje.SitioAlmacenaje == sa).ToList();
                                var filasSA = historialesHuecosAlmacenajesSA.Count();

                                // Sitio de almacenaje
                                tablaMP.AddCell(Celda(sa.Nombre, filasSA));

                                foreach (var hha in historialesHuecosAlmacenajesSA)
                                {
                                    var filasHA = 1;

                                    // Hueco de almacenaje
                                    tablaMP.AddCell(Celda(hha.HuecoAlmacenaje.Nombre, filasHA));
                                }
                            }
                        }
                    }
                }

            }

            return tablaMP;
        }

        private Table TablaProductoTerminado(ProductoTerminado productoTerminado)
        {
            var tablaPT = new Table(new float[] { 1, 1, 1, 1, 1, 1, 1/*, 1, 1, 1*/}).SetWidthPercent(100);
            tablaPT.AddHeaderCell(CeldaTitulo("Recepción", 1, 6).SetBorderBottom(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));
            tablaPT.AddHeaderCell(CeldaTitulo("Elaboración", 1, /*4*/1).SetBorderBottom(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));
            tablaPT.AddHeaderCell(CeldaTitulo("Proveedor"));
            tablaPT.AddHeaderCell(CeldaTitulo("Albarán"));
            tablaPT.AddHeaderCell(CeldaTitulo("Materia Prima"));
            tablaPT.AddHeaderCell(CeldaTitulo("Sitio"));
            tablaPT.AddHeaderCell(CeldaTitulo("Hueco"));
            tablaPT.AddHeaderCell(CeldaTitulo("Cantidad Almacenada"));
            tablaPT.AddHeaderCell(CeldaTitulo("Cantidad Utilizada"));
            /*tablaPT.AddHeaderCell(CeldaTitulo("Cantidad Producida"));
            tablaPT.AddHeaderCell(CeldaTitulo("Sitio"));
            tablaPT.AddHeaderCell(CeldaTitulo("Hueco"));*/
            foreach (var r in productoTerminado.ProductosTerminadosComposiciones.Select(ptc => ptc.HistorialHuecoRecepcion.MateriaPrima).Distinct().Select(mp => mp.Recepcion).Distinct().ToList())
            {
                //var huecosAlmacenajesR = r.MateriasPrimas.SelectMany(mp => mp.HistorialHuecosRecepciones).SelectMany(hhr => hhr.ProductosTerminadosComposiciones).Select(ptc => ptc.ProductoTerminado).SelectMany(pt => pt.HistorialHuecosAlmacenajes);
                var huecosRecepcionesR = r.MateriasPrimas.SelectMany(mp => mp.HistorialHuecosRecepciones);
                var filasR = huecosRecepcionesR.Count();

                // Recepción
                tablaPT.AddCell(Celda(r.Proveedor.RazonSocial, filasR));

                // Proveedor
                tablaPT.AddCell(Celda(r.NumeroAlbaran, filasR));

                foreach (var mp in r.MateriasPrimas)
                {
                    //var huecosAlmacenajesMP = mp.HistorialHuecosRecepciones.SelectMany(hhr => hhr.ProductosTerminadosComposiciones).Select(ptc => ptc.ProductoTerminado).SelectMany(pt => pt.HistorialHuecosAlmacenajes).Distinct();
                    var huecosRecepcionesMP = mp.HistorialHuecosRecepciones;
                    var filasMP = huecosRecepcionesMP.Count();

                    // Materia prima
                    tablaPT.AddCell(Celda(mp.Codigo, filasMP));

                    foreach (var sr in mp.HistorialHuecosRecepciones.Select(hhr => hhr.HuecoRecepcion.SitioRecepcion).Distinct().ToList())
                    {
                        //var huecosAlmacenajesSR = mp.HistorialHuecosRecepciones.SelectMany(hhr => hhr.ProductosTerminadosComposiciones).Select(ptc => ptc.ProductoTerminado).SelectMany(pt => pt.HistorialHuecosAlmacenajes).Distinct();
                        var huecosRecepcionesSR = mp.HistorialHuecosRecepciones;
                        var filasSR = huecosRecepcionesSR.Count();

                        // Sitio de recepción
                        tablaPT.AddCell(Celda(sr.Nombre, filasSR));
                        foreach (var hhr in mp.HistorialHuecosRecepciones.Where(hhr => hhr.HuecoRecepcion.SitioRecepcion == sr))
                        {
                            //var huecosAlmacenajesHHR = hhr.ProductosTerminadosComposiciones.Select(ptc => ptc.ProductoTerminado).SelectMany(pt => pt.HistorialHuecosAlmacenajes).ToList().Distinct();
                            var huecosRecepcionesHHR = 1;
                            var filasHHR = 1;
                            var filasHRCapacidadTotal = 1;
                            var filasHRCantidadAlmacenada = 1;
                            if (huecosRecepcionesHHR > 0)
                            {
                                filasHHR = huecosRecepcionesHHR;
                                filasHRCapacidadTotal = huecosRecepcionesHHR;
                                filasHRCantidadAlmacenada = huecosRecepcionesHHR;
                            }

                            // Hueco de recepción
                            tablaPT.AddCell(Celda(hhr.HuecoRecepcion.Nombre, filasHHR));

                            // Cantidad de materia prima almacenada en cada hueco de recepción
                            tablaPT.AddCell(Celda((mp.TipoMateriaPrima.MedidoEnUnidades == true) ? (hhr.Unidades + " de " + hhr.HuecoRecepcion.UnidadesTotales + " ud.") : (hhr.Volumen + " de " + hhr.HuecoRecepcion.VolumenTotal.ToString() + " m³"), filasHRCantidadAlmacenada));


                            var huecosAlmacenajesHHR = hhr.ProductosTerminadosComposiciones.Select(ptc => ptc.ProductoTerminado).SelectMany(pt => pt.HistorialHuecosAlmacenajes).ToList().Distinct();
                            if (huecosAlmacenajesHHR.Count() == 0)
                            {
                                // Cantidad de materia prima utilizada de cada hueco de recepción
                                tablaPT.AddCell(Celda("-"));
                                // Cantidad de producto terminado producido
                                //tablaPT.AddCell(Celda("-"));
                                // Sitio de almacenaje
                                //tablaPT.AddCell(Celda("-"));
                                // Hueco de almacenaje
                                //tablaPT.AddCell(Celda("-"));
                            }
                            else
                            {                                
                                foreach (var ptc in hhr.ProductosTerminadosComposiciones)
                                {
                                    var huecosAlmacenajesPTC = ptc.ProductoTerminado.HistorialHuecosAlmacenajes.Select(hha => hha.HuecoAlmacenaje).ToList();
                                    var sitiosAlmacenajesPTC = huecosAlmacenajesPTC.Select(ha => ha.SitioAlmacenaje).Distinct().ToList();

                                    // var filasHRCantidadUtilizada = huecosAlmacenajesPTC.Count();
                                    var filasHRCantidadUtilizada = 1;
                                    var filasPT = huecosAlmacenajesPTC.Count();

                                    // Cantidad de materia prima utilizada de cada hueco de recepción
                                    tablaPT.AddCell(Celda((mp.TipoMateriaPrima.MedidoEnUnidades == true) ? (ptc.Unidades + " ud.") : (ptc.Volumen + " m³"), filasHRCantidadUtilizada));

                                    // Cantidad producida del producto terminado
                                    //tablaPT.AddCell(Celda((ptc.ProductoTerminado.TipoProductoTerminado.MedidoEnUnidades == true) ? (ptc.ProductoTerminado.Unidades + " ud.") : (ptc.ProductoTerminado.Volumen + " m³"), filasHRCantidadUtilizada));

                                    foreach (var sa in sitiosAlmacenajesPTC)
                                    {
                                        var historialesHuecosAlmacenajesSA = ptc.ProductoTerminado.HistorialHuecosAlmacenajes.Where(hha => hha.HuecoAlmacenaje.SitioAlmacenaje == sa).ToList();
                                        var filasSA = historialesHuecosAlmacenajesSA.Count();

                                        // Sitio de almacenaje
                                        //tablaPT.AddCell(Celda(sa.Nombre, filasSA));

                                        foreach (var hha in historialesHuecosAlmacenajesSA)
                                        {
                                            var filasHA = 1;

                                            // Hueco de almacenaje
                                            //tablaPT.AddCell(Celda(hha.HuecoAlmacenaje.Nombre, filasHA));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return tablaPT;
        }


        private Paragraph Titulo(string texto)
        {
            Style estiloTitulo = new Style().SetFont(cambriaNegrita).SetFontSize(16).SetBold();
            var p = new Paragraph();

            // Versalitas
            var minusculas = "";
            var mayusculas = "";
            void procesarMayusculas()
            {
                if (!string.IsNullOrEmpty(mayusculas))
                {
                    p.Add(new Text(mayusculas).AddStyle(estiloTitulo));
                    mayusculas = "";
                }
            }
            void procesarMinusculas()
            {
                if (!string.IsNullOrEmpty(minusculas))
                {
                    p.Add(new Text(minusculas.ToUpper()).AddStyle(estiloTitulo).SetFontSize(14));
                    minusculas = "";
                }
            }

            foreach (var c in texto)
            {
                if (char.IsUpper(c))
                {
                    procesarMinusculas();
                    mayusculas += c;
                }
                else
                {
                    procesarMayusculas();
                    minusculas += c;
                }
            }
            procesarMayusculas();
            procesarMinusculas();

            return p.SetMarginBottom(10).SetBackgroundColor(Color.BLACK, 0.15f).SetPaddingLeft(5);
        }

        private Cell Celda(string texto, int rowspan = 1, int colspan = 1)
        {
            return new Cell(rowspan, colspan).Add(new Paragraph(texto)).AddStyle(estiloCelda)
               .SetBorderBottom(new SolidBorder(GROSOR_BORDE_CELDA));
        }

        private Cell Celda(Table tabla, int rowspan = 1, int colspan = 1)
        {
            return new Cell(rowspan, colspan).Add(tabla).AddStyle(estiloCelda).SetPadding(0)
               .SetBorderBottom(new SolidBorder(GROSOR_BORDE_CELDA));
        }

        private Cell CeldaTitulo(string texto, int rowspan = 1, int colspan = 1)
        {
            return new Cell(rowspan, colspan).Add(new Paragraph(texto)).AddStyle(estiloCelda).SetBold()
               .SetBorderBottom(new SolidBorder(GROSOR_BORDE_CELDA)).SetBorderTop(new DoubleBorder(GROSOR_BORDE_CELDA));
        }

        private Cell CeldaVertical(string texto, int rowspan = 1, int colspan = 1)
        {
            return new Cell(rowspan, colspan).Add(new Paragraph(texto).SetPaddings(2, 5, 2, 5))
                .AddStyle(estiloCelda).SetTextAlignment(TextAlignment.LEFT);
        }

        private Cell CeldaTituloVertical(string texto, int rowspan = 1, int colspan = 1)
        {
            return new Cell(rowspan, colspan).Add(new Paragraph(texto.ToUpper()).SetBackgroundColor(Color.GRAY, 0.15f)
                .SetPaddings(2, 5, 2, 5)).AddStyle(estiloCelda).SetBold().SetTextAlignment(TextAlignment.LEFT);
        }


        protected internal class FinPaginaEventHandler : IEventHandler
        {
            public virtual void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage pagina = docEvent.GetPage();
                int numeroPagina = pdfDoc.GetPageNumber(pagina);
                Rectangle dimensionPagina = pagina.GetPageSize();
                PdfCanvas pdfCanvas = new PdfCanvas(pagina.NewContentStreamBefore(), pagina.GetResources(), pdfDoc);

                // Color limeColor = new DeviceCmyk(0.208f, 0, 0.584f, 0);
                // Color blueColor = new DeviceCmyk(0.445f, 0.0546f, 0, 0.0667f);
                int margenSuperior = 36;
                int margenInferior = 36;
                int margenIzquierdo = 36;
                int margenDerecho = 36;
                float anchuraCaracter = 3.25f; // Esto está a ojo ya que depende del tamaño de la fuente!!!!
                float alturaCaracter = 6f;
                int longitudLineaNumPagina = 20;
                int margenLineaNumPagina = 5;

                if (COLOR_FONDO_PAGINA != null)
                {
                    // Color de fondo
                    pdfCanvas.SaveState()//.SetFillColor(numeroPagina % 2 == 1 ? limeColor : blueColor)
                        .SetFillColor(COLOR_FONDO_PAGINA)
                        .Rectangle(dimensionPagina.GetLeft(), dimensionPagina.GetBottom(), dimensionPagina.GetWidth(), dimensionPagina.GetHeight())
                        .Fill().RestoreState();
                }

                // Cabecera página
                pdfCanvas.BeginText().SetFontAndSize(calibri, 11)
                    .MoveText(margenIzquierdo, dimensionPagina.GetHeight() - margenSuperior / 2 - +alturaCaracter / 2)
                    .ShowText("BiomasaEUPT")
                    .EndText();
                var formatoFecha = "dd/MM/yyyy HH:mm:ss";
                pdfCanvas.BeginText().BeginText().SetFontAndSize(calibri, 11)
                   .MoveText(dimensionPagina.GetWidth() - margenIzquierdo - margenDerecho - formatoFecha.Length * anchuraCaracter,
                   dimensionPagina.GetHeight() - margenSuperior / 2 - alturaCaracter / 2)
                   .ShowText(DateTime.Now.ToString(formatoFecha))
                   .EndText();

                // Pie de Página
                pdfCanvas.SetStrokeColor(Color.BLACK)
                    .SetLineWidth(.2f)
                    .MoveTo(dimensionPagina.GetWidth() / 2 - anchuraCaracter - margenLineaNumPagina - longitudLineaNumPagina, margenInferior / 2 + alturaCaracter / 2)
                    .LineTo(dimensionPagina.GetWidth() / 2 - anchuraCaracter - margenLineaNumPagina, margenInferior / 2 + alturaCaracter / 2)
                    .Stroke();
                pdfCanvas.BeginText().SetFontAndSize(calibri, 11)
                    .MoveText(dimensionPagina.GetWidth() / 2 - anchuraCaracter, margenInferior / 2)
                    .ShowText(numeroPagina.ToString())
                    .EndText();
                pdfCanvas.SetStrokeColor(Color.BLACK)
                    .SetLineWidth(.2f)
                    .MoveTo(dimensionPagina.GetWidth() / 2 + anchuraCaracter + margenLineaNumPagina, margenInferior / 2 + alturaCaracter / 2)
                    .LineTo(dimensionPagina.GetWidth() / 2 + anchuraCaracter + margenLineaNumPagina + longitudLineaNumPagina, margenInferior / 2 + alturaCaracter / 2)
                    .Stroke();
            }

            internal FinPaginaEventHandler(InformePDF _enclosing)
            {
                this._enclosing = _enclosing;
            }

            private readonly InformePDF _enclosing;
        }

        protected internal class OrientacionPaginaEventHandler : IEventHandler
        {
            //protected PdfNumber orientation = PORTRAIT;

            public PdfNumber Orientacion { get; set; }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                docEvent.GetPage().Put(PdfName.Rotate, Orientacion);
            }
        }


        public string GenerarPDFCodigoMateriaPrima(MateriaPrima materiaPrima)
        {
            // MemoryStream memStream = new MemoryStream();
            //  PdfDocument pdfDoc = new PdfDocument(new PdfWriter(memStream));

            var ruta = System.IO.Path.GetTempPath() + "/BiomasaEUPT/";
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);

            var rutaPDF = ruta + "Código Materia Prima #" + materiaPrima.Codigo + " " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(rutaPDF));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.AddCreationDate();
            info.SetAuthor("BiomasaEUPT");
            info.SetCreator("BiomasaEUPT");
            info.SetTitle("Código Materia Prima #" + materiaPrima.Codigo + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            Document doc = new Document(pdfDoc, new PageSize(60, 140));
            doc.SetMargins(5, 5, 5, 5);

            //pdfDoc.AddEventHandler(PdfDocumentEvent.START_PAGE, new OrientacionPaginaEventHandler() { Orientacion = LANDSCAPE });

            PdfFont bold = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);
            PdfFont regular = PdfFontFactory.CreateFont(FontConstants.HELVETICA);
            var primeraPagina = true;
            foreach (var hhr in materiaPrima.HistorialHuecosRecepciones.ToList())
            {
                if (!primeraPagina)
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }
                Paragraph p1 = new Paragraph();
                //p1.Add(new Text(hhr.HuecoRecepcion.SitioRecepcion.Nombre).SetFont(bold).SetFontSize(12));
                p1.Add(new Text(hhr.HuecoRecepcion.Nombre).SetFont(bold).SetFontSize(6));
                doc.Add(p1);

                Paragraph p2 = new Paragraph(materiaPrima.Recepcion.FechaRecepcion.ToString("dd/MM/yyyy HH:mm")).SetFont(regular).SetFontSize(4);
                p2.SetTextAlignment(TextAlignment.RIGHT);
                doc.Add(p2);

                Barcode128 barcode = new Barcode128(pdfDoc);
                barcode.SetCodeType(Barcode128.CODE128);
                barcode.SetCode(materiaPrima.Codigo);
                Rectangle rect = barcode.GetBarcodeSize();
                PdfFormXObject template = new PdfFormXObject(new Rectangle(rect.GetWidth(), rect.GetHeight() + 10));
                PdfCanvas templateCanvas = new PdfCanvas(template, pdfDoc);
                new Canvas(templateCanvas, pdfDoc, new Rectangle(rect.GetWidth(), rect.GetHeight() + 10))
                        .ShowTextAligned(new Paragraph(materiaPrima.TipoMateriaPrima.Nombre).SetFont(regular).SetFontSize(6), 0, rect.GetHeight() + 2, TextAlignment.LEFT);
                barcode.PlaceBarcode(templateCanvas, Color.BLACK, Color.BLACK);
                Image image = new Image(template);
                image.SetRotationAngle(Math.PI / 2);
                image.SetAutoScale(true);
                doc.Add(image);

                // Paragraph p3 = new Paragraph("SMALL").SetFont(regular).SetFontSize(6);
                // p3.SetTextAlignment(TextAlignment.CENTER);
                // doc.Add(p3);
                primeraPagina = false;
            }

            doc.Close();
            // return memStream;
            return rutaPDF;
        }


        public string GenerarPDFCodigoProductoTerminado(ProductoTerminado productoTerminado)
        {
            var ruta = System.IO.Path.GetTempPath() + "/BiomasaEUPT/";
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);

            var rutaPDF = ruta + "Código Producto Terminado #" + productoTerminado.Codigo + " " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(rutaPDF));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.AddCreationDate();
            info.SetAuthor("BiomasaEUPT");
            info.SetCreator("BiomasaEUPT");
            info.SetTitle("Código Producto Terminado #" + productoTerminado.Codigo + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            Document doc = new Document(pdfDoc, new PageSize(60, 140));
            doc.SetMargins(5, 5, 5, 5);

            PdfFont bold = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);
            PdfFont regular = PdfFontFactory.CreateFont(FontConstants.HELVETICA);
            var primeraPagina = true;
            foreach (var hha in productoTerminado.HistorialHuecosAlmacenajes.ToList())
            {
                if (!primeraPagina)
                {
                    doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }
                Paragraph p1 = new Paragraph();
                p1.Add(new Text(hha.HuecoAlmacenaje.Nombre).SetFont(bold).SetFontSize(6));
                doc.Add(p1);

                /* Paragraph p2 = new Paragraph(productoTerminado.OrdenElaboracion.FechaElaboracion.ToString("dd/MM/yyyy HH:mm")).SetFont(regular).SetFontSize(4);
                 p2.SetTextAlignment(TextAlignment.RIGHT);
                 doc.Add(p2);*/

                Barcode128 barcode = new Barcode128(pdfDoc);
                barcode.SetCodeType(Barcode128.CODE128);
                barcode.SetCode(productoTerminado.Codigo);
                Rectangle rect = barcode.GetBarcodeSize();
                PdfFormXObject template = new PdfFormXObject(new Rectangle(rect.GetWidth(), rect.GetHeight() + 10));
                PdfCanvas templateCanvas = new PdfCanvas(template, pdfDoc);
                new Canvas(templateCanvas, pdfDoc, new Rectangle(rect.GetWidth(), rect.GetHeight() + 10))
                        .ShowTextAligned(new Paragraph(productoTerminado.TipoProductoTerminado.Nombre + " (" + productoTerminado.TipoProductoTerminado.Tamano + " " + productoTerminado.TipoProductoTerminado.Humedad + "%)").SetFont(regular).SetFontSize(6), 0, rect.GetHeight() + 2, TextAlignment.LEFT);
                barcode.PlaceBarcode(templateCanvas, Color.BLACK, Color.BLACK);
                Image image = new Image(template);
                image.SetRotationAngle(Math.PI / 2);
                image.SetAutoScale(true);
                doc.Add(image);

                // Paragraph p3 = new Paragraph("SMALL").SetFont(regular).SetFontSize(6);
                // p3.SetTextAlignment(TextAlignment.CENTER);
                // doc.Add(p3);
                primeraPagina = false;
            }

            doc.Close();
            return rutaPDF;
        }

        /*
        private PdfTemplate PdfFooter(PdfContentByte cb)
        {
            BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            // Create the template and assign height
            PdfTemplate tmpFooter = cb.CreateTemplate(580, 70);
            // Move to the bottom left corner of the template
            tmpFooter.MoveTo(1, 1);
            // Place the footer content
            tmpFooter.Stroke();
            // Begin writing the footer
            tmpFooter.BeginText();
            // Set the font and size
            tmpFooter.SetFontAndSize(f_cn, 8);
            // Write out details from the payee table
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "BiomasaEUPT", 0, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Teruel", 0, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Teruel", 0, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Teruel", 0, 29, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "44003" + " " + "Teruel" + " " + "España", 0, 21, 0);
            // Bold text for ther headers
            tmpFooter.SetFontAndSize(f_cb, 8);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Phone", 215, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Mail", 215, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Web", 215, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Legal info", 400, 53, 0);
            // Regular text for infomation fields
            tmpFooter.SetFontAndSize(f_cn, 8);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "+34 998877665", 265, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "contacto@biomasaeupt.com", 265, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "https://www.biomasaeupt.com", 265, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "asass", 400, 45, 0);
            // End text
            tmpFooter.EndText();
            // Stamp a line above the page footer
            cb.SetLineWidth(0f);
            cb.MoveTo(30, 60);
            cb.LineTo(570, 60);
            cb.Stroke();
            // Return the footer template
            return tmpFooter;
        }
        */

        /*
        public void CreatePDF()
        {
            string fileName = string.Empty;

            DateTime fileCreationDatetime = DateTime.Now;

            fileName = string.Format("{0}.pdf", fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + fileCreationDatetime.ToString(@"HHmmss"));


            using (FileStream msReport = new FileStream(ruta + "asd.pdf", FileMode.Create))
            {
                //step 1
                using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 140f, 10f))
                {
                    try
                    {
                        // step 2
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, msReport);
                        pdfWriter.PageEvent = new ITextEvents();

                        //open the stream 
                        pdfDoc.Open();

                        for (int i = 0; i < 10; i++)
                        {
                            Paragraph para = new Paragraph("Hello world. Checking Header Footer", new Font(Font.FontFamily.HELVETICA, 22))
                            {
                                Alignment = Element.ALIGN_CENTER
                            };
                            pdfDoc.Add(para);

                            pdfDoc.NewPage();
                        }

                        pdfDoc.Close();

                    }
                    catch (Exception ex)
                    {
                        //handle exception
                    }

                    finally
                    {


                    }

                }

            }
        }
        */
    }




    /*
        public class ITextEvents : PdfPageEventHelper
        {

            // This is the contentbyte object of the writer
            PdfContentByte cb;

            // we will put the final number of pages in a template
            PdfTemplate headerTemplate, footerTemplate;

            // this is the BaseFont we are going to use for the header / footer
            BaseFont bf = null;

            // This keeps track of the creation time
            DateTime PrintTime = DateTime.Now;


            #region Fields
            private string _header;
            #endregion

            #region Properties
            public string Header
            {
                get { return _header; }
                set { _header = value; }
            }
            #endregion


            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                try
                {
                    PrintTime = DateTime.Now;
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb = writer.DirectContent;
                    headerTemplate = cb.CreateTemplate(100, 100);
                    footerTemplate = cb.CreateTemplate(50, 50);
                }
                catch (DocumentException de)
                {

                }
                catch (IOException ioe)
                {

                }
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                Font baseFontNormal = new Font(Font.FontFamily.HELVETICA, 12f, Font.NORMAL, BaseColor.BLACK);

                Font baseFontBig = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.BLACK);

                Phrase p1Header = new Phrase("Sample Header Here", baseFontNormal);

                //Create PdfTable object
                PdfPTable pdfTab = new PdfPTable(3);

                //We will have to create separate cells to include image logo and 2 separate strings
                //Row 1
                PdfPCell pdfCell1 = new PdfPCell();
                PdfPCell pdfCell2 = new PdfPCell(p1Header);
                PdfPCell pdfCell3 = new PdfPCell();
                String text = "Page " + writer.PageNumber + " of ";


                //Add paging to header
                {
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 12);
                    cb.SetTextMatrix(document.PageSize.GetRight(200), document.PageSize.GetTop(45));
                    cb.ShowText(text);
                    cb.EndText();
                    float len = bf.GetWidthPoint(text, 12);
                    //Adds "12" in Page 1 of 12
                    cb.AddTemplate(headerTemplate, document.PageSize.GetRight(200) + len, document.PageSize.GetTop(45));
                }
                //Add paging to footer
                {
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 12);
                    cb.SetTextMatrix(document.PageSize.GetRight(180), document.PageSize.GetBottom(30));
                    cb.ShowText(text);
                    cb.EndText();
                    float len = bf.GetWidthPoint(text, 12);
                    cb.AddTemplate(footerTemplate, document.PageSize.GetRight(180) + len, document.PageSize.GetBottom(30));
                }
                //Row 2
                PdfPCell pdfCell4 = new PdfPCell(new Phrase("Sub Header Description", baseFontNormal));
                //Row 3


                PdfPCell pdfCell5 = new PdfPCell(new Phrase("Date:" + PrintTime.ToShortDateString(), baseFontBig));
                PdfPCell pdfCell6 = new PdfPCell();
                PdfPCell pdfCell7 = new PdfPCell(new Phrase("TIME:" + string.Format("{0:t}", DateTime.Now), baseFontBig));


                //set the alignment of all three cells and set border to 0
                pdfCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell3.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell4.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell5.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell6.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfCell7.HorizontalAlignment = Element.ALIGN_CENTER;


                pdfCell2.VerticalAlignment = Element.ALIGN_BOTTOM;
                pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell4.VerticalAlignment = Element.ALIGN_TOP;
                pdfCell5.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell6.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfCell7.VerticalAlignment = Element.ALIGN_MIDDLE;


                pdfCell4.Colspan = 3;



                pdfCell1.Border = 0;
                pdfCell2.Border = 0;
                pdfCell3.Border = 0;
                pdfCell4.Border = 0;
                pdfCell5.Border = 0;
                pdfCell6.Border = 0;
                pdfCell7.Border = 0;


                //add all three cells into PdfTable
                pdfTab.AddCell(pdfCell1);
                pdfTab.AddCell(pdfCell2);
                pdfTab.AddCell(pdfCell3);
                pdfTab.AddCell(pdfCell4);
                pdfTab.AddCell(pdfCell5);
                pdfTab.AddCell(pdfCell6);
                pdfTab.AddCell(pdfCell7);

                pdfTab.TotalWidth = document.PageSize.Width - 80f;
                pdfTab.WidthPercentage = 70;
                //pdfTab.HorizontalAlignment = Element.ALIGN_CENTER;


                //call WriteSelectedRows of PdfTable. This writes rows from PdfWriter in PdfTable
                //first param is start row. -1 indicates there is no end row and all the rows to be included to write
                //Third and fourth param is x and y position to start writing
                pdfTab.WriteSelectedRows(0, -1, 40, document.PageSize.Height - 30, writer.DirectContent);
                //set pdfContent value

                //Move the pointer and draw line to separate header section from rest of page
                cb.MoveTo(40, document.PageSize.Height - 100);
                cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 100);
                cb.Stroke();

                //Move the pointer and draw line to separate footer section from rest of page
                cb.MoveTo(40, document.PageSize.GetBottom(50));
                cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(50));
                cb.Stroke();
            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);

                headerTemplate.BeginText();
                headerTemplate.SetFontAndSize(bf, 12);
                headerTemplate.SetTextMatrix(0, 0);
                headerTemplate.ShowText((writer.PageNumber - 1).ToString());
                headerTemplate.EndText();

                footerTemplate.BeginText();
                footerTemplate.SetFontAndSize(bf, 12);
                footerTemplate.SetTextMatrix(0, 0);
                footerTemplate.ShowText((writer.PageNumber - 1).ToString());
                footerTemplate.EndText();
            }
        }*/
}
