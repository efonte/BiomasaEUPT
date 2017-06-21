using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
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
    public class InformePDF
    {
        private string ruta;

        internal static PdfFont helvetica = null;
        internal static PdfFont helveticaNegrita = null;
        internal static PdfFont calibri = null;
        internal static PdfFont cambriaNegrita = null;

        internal static float GROSOR_BORDE_CELDA = 0.5f;
        internal static float GROSOR_BORDE_TITULO = 0.75f;

        internal static PdfNumber INVERTEDPORTRAIT = new PdfNumber(180);
        internal static PdfNumber LANDSCAPE = new PdfNumber(90);
        internal static PdfNumber PORTRAIT = new PdfNumber(0);
        internal static PdfNumber SEASCAPE = new PdfNumber(270);

        internal static Style estiloCelda = null;

        public InformePDF(string ruta)
        {
            this.ruta = ruta;

            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);

            helvetica = PdfFontFactory.CreateFont(FontConstants.HELVETICA);
            helveticaNegrita = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);
            PdfFontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "/fonts/calibri.ttf", "Calibri");
            PdfFontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "/fonts/cambriab.ttf", "Cambria Negrita");
            calibri = PdfFontFactory.CreateRegisteredFont("Calibri", PdfEncodings.IDENTITY_H, true);
            cambriaNegrita = PdfFontFactory.CreateRegisteredFont("Cambria Negrita", PdfEncodings.IDENTITY_H, true);

            estiloCelda = new Style().SetFont(calibri).SetFontSize(13).SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.CENTER);
        }

        public string GenerarPDFMateriaPrima(Proveedor proveedor)
        {
            var materiaPrima = proveedor.Recepciones.First().MateriasPrimas.First();

            // Se guarda en una variable la fecha de creación para que tanto la fecha del nombre del PDF como la que hay dentro del PDF sean las mismas.
            var fechaCreacion = DateTime.Now;
            var nombrePdf = ruta + "Materia Prima #" + materiaPrima.Codigo + " " + fechaCreacion.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";

            PdfWriter writer = new PdfWriter(nombrePdf);

            PdfDocument pdf = new PdfDocument(writer);
            PdfDocumentInfo info = pdf.GetDocumentInfo();
            info.AddCreationDate();
            info.SetAuthor("BiomasaEUPT");
            info.SetCreator("BiomasaEUPT");
            info.SetTitle("Materia Prima #" + materiaPrima.Codigo + " " + fechaCreacion.ToString("dd/MM/yyyy HH:mm:ss"));
            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new MyEventHandler(this));

            OrientacionPaginaEventHandler orientacionPaginaEventHandler = new OrientacionPaginaEventHandler();
            pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, orientacionPaginaEventHandler);

            Document doc = new Document(pdf, PageSize.A4.Rotate());
            //doc.SetMargins(70, 70, 85, 85);

            doc.Add(Titulo("Recepción"));
            Table tablaRecepcion = new Table(new float[] { 1, 1 }).SetWidthPercent(30);
            tablaRecepcion.AddCell(CeldaTituloVertical("Fecha Recepción"));
            tablaRecepcion.AddCell(CeldaVertical(proveedor.Recepciones.First().FechaRecepcion.ToString("dd/MM/yyyy HH:mm")));
            tablaRecepcion.AddCell(CeldaTituloVertical("Nº de Albarán"));
            tablaRecepcion.AddCell(CeldaVertical(proveedor.Recepciones.First().NumeroAlbaran).SetFont(calibri));
            doc.Add(tablaRecepcion);

            doc.Add(new Paragraph("\n"));

            doc.Add(Titulo("Proveedor"));
            Table tablaProveedor = new Table(new float[] { 1, 1 }).SetWidthPercent(30);
            tablaProveedor.AddCell(CeldaTituloVertical("Razon Social"));
            tablaProveedor.AddCell(CeldaVertical(proveedor.RazonSocial));
            tablaProveedor.AddCell(CeldaTituloVertical("NIF"));
            tablaProveedor.AddCell(CeldaVertical(proveedor.Nif));
            tablaProveedor.AddCell(CeldaTituloVertical("Tipo"));
            tablaProveedor.AddCell(CeldaVertical(proveedor.TipoProveedor.Nombre));
            doc.Add(tablaProveedor);

            doc.Add(new Paragraph("\n"));

            doc.Add(Titulo("Materia Prima"));
            /*Table tablaMP = new Table(new float[] { 1, 1, 1, 1, 1, 1, 1, 1 }).SetWidthPercent(100);
            var cantidadMateriaPrima = (materiaPrima.TipoMateriaPrima.MedidoEnUnidades == true) ? ($"{materiaPrima.Unidades} ud") : ($"{materiaPrima.Volumen} m³");
            tablaMP.AddHeaderCell(CeldaTitulo(materiaPrima.TipoMateriaPrima.Nombre + " (" + cantidadMateriaPrima + ")", 1, 8));
            tablaMP.AddHeaderCell(CeldaTitulo("Recepción", 1, 4));
            tablaMP.AddHeaderCell(CeldaTitulo("Elaboración", 1, 4));
            tablaMP.AddHeaderCell(CeldaTitulo("Sitio"));
            tablaMP.AddHeaderCell(CeldaTitulo("Hueco"));
            tablaMP.AddHeaderCell(CeldaTitulo("Capacidad Total"));
            tablaMP.AddHeaderCell(CeldaTitulo("Unidades Almacenadas"));
            tablaMP.AddHeaderCell(CeldaTitulo("Unidades Utilizadas"));
            tablaMP.AddHeaderCell(CeldaTitulo("Producto Terminado"));
            tablaMP.AddHeaderCell(CeldaTitulo("Sitio"));

            foreach (var sr in materiaPrima.HistorialHuecosRecepciones.Select(hhr => hhr.HuecoRecepcion.SitioRecepcion).Distinct())
            {
                var historialHuecosRecepciones = materiaPrima.HistorialHuecosRecepciones.Where(hhr => hhr.HuecoRecepcion.SitioRecepcion == sr).ToList();
                tablaMP.AddCell(Celda(sr.Nombre, historialHuecosRecepciones.Count()));
                foreach (var hhr in historialHuecosRecepciones)
                {
                    tablaMP.AddCell(Celda(hhr.HuecoRecepcion.Nombre));
                    tablaMP.AddCell(Celda($"{hhr.HuecoRecepcion.VolumenTotal.ToString()} m³ / {hhr.HuecoRecepcion.UnidadesTotales} ud"));
                    var cantidadHhr = (materiaPrima.TipoMateriaPrima.MedidoEnUnidades == true) ? (hhr.Unidades + " ud") : (hhr.Volumen + " m³");
                    tablaMP.AddCell(cantidadHhr);
                    tablaMP.AddCell(Celda("-"));
                    tablaMP.AddCell(Celda("-"));
                    tablaMP.AddCell(Celda("-"));
                    tablaMP.AddCell(Celda("-"));
                }
            }*/

            var tablaMP = new Table(new float[] { 1, 1, 1, 1, 1, 1, 1 }).SetWidthPercent(100);
            var cantidadMateriaPrima = (materiaPrima.TipoMateriaPrima.MedidoEnUnidades == true) ? ($"{materiaPrima.Unidades} ud.") : ($"{materiaPrima.Volumen} m³");
            tablaMP.AddHeaderCell(CeldaTitulo(materiaPrima.TipoMateriaPrima.Nombre + " (" + cantidadMateriaPrima + ")", 1, 7));
            tablaMP.AddHeaderCell(CeldaTitulo("Recepción", 1, 4));
            tablaMP.AddHeaderCell(CeldaTitulo("Elaboración", 1, 3));
            tablaMP.AddHeaderCell(CeldaTitulo("Sitio"));
            tablaMP.AddHeaderCell(CeldaTitulo("Hueco"));
            tablaMP.AddHeaderCell(CeldaTitulo("Capacidad Total"));
            tablaMP.AddHeaderCell(CeldaTitulo("Unidades Almacenadas"));
            tablaMP.AddHeaderCell(CeldaTitulo("Unidades Utilizadas"));
            tablaMP.AddHeaderCell(CeldaTitulo("Producto Terminado"));
            tablaMP.AddHeaderCell(CeldaTitulo("Sitio"));

            foreach (var sr in materiaPrima.HistorialHuecosRecepciones.Select(hhr => hhr.HuecoRecepcion.SitioRecepcion).Distinct())
            {
                var historialHuecosRecepciones = materiaPrima.HistorialHuecosRecepciones.Where(hhr => hhr.HuecoRecepcion.SitioRecepcion == sr).ToList();

                // Sitios de recepción
                tablaMP.AddCell(Celda(sr.Nombre));
                // Huecos de recepción
                var tablaHR = new Table(new float[] { 1 }).SetWidthPercent(100);
                // Capacidad Total de cada uno de los huecos de recepción
                var tablaHRCapacidadTotal = new Table(new float[] { 1 }).SetWidthPercent(100);
                // Cantidades de materias primas almacenadas en cada uno de los huecos de recepción
                var tablaHRCantidad = new Table(new float[] { 1 }).SetWidthPercent(100);
                foreach (var hhr in historialHuecosRecepciones)
                {
                    tablaHR.AddCell(Celda(hhr.HuecoRecepcion.Nombre));
                    tablaHRCapacidadTotal.AddCell(Celda($"{hhr.HuecoRecepcion.VolumenTotal.ToString()} m³ / {hhr.HuecoRecepcion.UnidadesTotales} ud."));
                    tablaHRCantidad.AddCell(Celda((materiaPrima.TipoMateriaPrima.MedidoEnUnidades == true) ? (hhr.Unidades + " ud.") : (hhr.Volumen + " m³")));

                    var productosTerminadosComposiciones = hhr.ProductosTerminadosComposiciones.ToList();
                    foreach (var ptc in productosTerminadosComposiciones)
                    {
                        Console.WriteLine(ptc.Unidades+" "+ptc.Volumen+" "+ptc.ProductoTerminado.TipoProductoTerminado.Nombre);
                    }
                }

                // Se quitan los bordes inferiores de cada última celda para que no hayan dos bordes juntos
                tablaHR.GetCell(tablaHR.GetNumberOfRows() - 1, 0).SetBorderBottom(Border.NO_BORDER);
                tablaHRCapacidadTotal.GetCell(tablaHRCapacidadTotal.GetNumberOfRows() - 1, 0).SetBorderBottom(Border.NO_BORDER);
                tablaHRCantidad.GetCell(tablaHRCantidad.GetNumberOfRows() - 1, 0).SetBorderBottom(Border.NO_BORDER);

                // Se añaden las subtablas en cada una de las celdas correspondientes
                tablaMP.AddCell(Celda(tablaHR));
                tablaMP.AddCell(Celda(tablaHRCapacidadTotal));
                tablaMP.AddCell(Celda(tablaHRCantidad));

                // Unidades utilizadas
                tablaMP.AddCell(Celda("-"));
                // Productos terminados
                tablaMP.AddCell(Celda("-"));
                // Sitios de almacenaje
                tablaMP.AddCell(Celda("-"));
            }
            doc.Add(tablaMP);

            orientacionPaginaEventHandler.Orientacion = LANDSCAPE;
            doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            Table table = new Table(new float[] { 25, 50 })
                             .AddCell(new Cell().Add(new Paragraph("cell 1, 1").SetRotationAngle((Math.PI / 2))))
                             .AddCell(new Cell().Add(new Paragraph("cell 1, 2").SetRotationAngle((Math.PI / 3))))
                             .AddCell(new Cell().Add(new Paragraph("cell 2, 1").SetRotationAngle(-(Math.PI / 2))))
                             .AddCell(new Cell().Add(new Paragraph("cell 2, 2").SetRotationAngle((Math.PI))));
            doc.Add(table);

            doc.Close();

            return nombrePdf;
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
            return new Cell(rowspan, colspan).Add(new Paragraph(texto.ToUpper()).SetBackgroundColor(Color.GRAY)
                .SetPaddings(2, 5, 2, 5)).AddStyle(estiloCelda).SetBold().SetTextAlignment(TextAlignment.LEFT);
        }


        protected internal class MyEventHandler : IEventHandler
        {
            public virtual void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                int pageNumber = pdfDoc.GetPageNumber(page);
                Rectangle pageSize = page.GetPageSize();
                PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
                //Set background
                Color limeColor = new DeviceCmyk(0.208f, 0, 0.584f, 0);
                Color blueColor = new DeviceCmyk(0.445f, 0.0546f, 0, 0.0667f);
                pdfCanvas.SaveState().SetFillColor(pageNumber % 2 == 1 ? limeColor : blueColor).Rectangle(pageSize.GetLeft
                    (), pageSize.GetBottom(), pageSize.GetWidth(), pageSize.GetHeight()).Fill().RestoreState();
                //Add header and footer
                pdfCanvas.BeginText().SetFontAndSize(helvetica, 9).MoveText(pageSize.GetWidth() / 2 - 60, pageSize
                    .GetTop() - 20).ShowText("CABECERA").MoveText(60, -pageSize.GetTop() + 30).ShowText(pageNumber
                    .ToString()).EndText();
            }

            internal MyEventHandler(InformePDF _enclosing)
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
