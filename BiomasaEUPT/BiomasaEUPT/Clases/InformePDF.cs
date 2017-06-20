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
        internal static PdfFont helveticaBold = null;
        internal static PdfFont calibri = null;
        internal static PdfFont cambria = null;

        public InformePDF(string ruta)
        {
            this.ruta = ruta;

            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);

            helvetica = PdfFontFactory.CreateFont(FontConstants.HELVETICA);
            helveticaBold = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);

            PdfFontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "/fonts/calibri.ttf", "Calibri");
            PdfFontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "/fonts/cambria.ttc", "Cambria");

            calibri = PdfFontFactory.CreateRegisteredFont("Calibri");
            cambria = PdfFontFactory.CreateRegisteredFont("Cambria");
            //calibri = PdfFontFactory.CreateFont(Environment.GetEnvironmentVariable("SystemRoot") + "/fonts/calibri.ttf");
        }

        public string GenerarPDFMateriaPrima(Proveedor proveedor)
        {
            var materiaPrima = proveedor.Recepciones.First().MateriasPrimas.First();

            // Se guarda en una variable la fecha de creación para que tanto la fecha del nombre del PDF como la que hay dentro del PDF sean las mismas.
            var fechaCreacion = DateTime.Now;
            var nombrePdf = ruta + "Materia Prima #" + materiaPrima.Codigo + " " + fechaCreacion.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
            /* Document doc = new Document(PageSize.A4, 70, 70, 85, 85);
            

             PdfWriter writer = PdfWriter.GetInstance(doc,
                                         new FileStream(nombrePdf, FileMode.Create));

             // Metadatos
             doc.AddTitle("Informe #" + codigo + " " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss"));
             doc.AddCreator("BiomasaEUPT");
             doc.AddAuthor("BiomasaEUPT");
             doc.AddCreationDate();

             doc.Open();

             // Creamos el tipo de Font que vamos utilizar
             FontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\cambria.ttc", "Cambria");
             FontFactory.Register(Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\calibri.ttf", "Calibri");
             // Font fuenteEstandar = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK);
             Font fuenteNormal = FontFactory.GetFont("Calibri", 12, Font.NORMAL, BaseColor.BLACK);
             Font fuenteNegrita = FontFactory.GetFont("Calibri", 12, Font.BOLD, BaseColor.BLACK);
             Font fuenteTitulo = FontFactory.GetFont("Cambria", 16, Font.BOLD, new BaseColor(79, 129, 189));


             doc.Add(new Paragraph("Recepción", fuenteTitulo));
             doc.Add(Chunk.NEWLINE);
             */
            /* PdfPTable tblRecepcion = new PdfPTable(2)
             {
                 WidthPercentage = 100
             };

             PdfPCell clProveedor = new PdfPCell(new Phrase("Proveedor", fuenteEstandar))
             {
                 BorderWidth = 0,
                 BorderWidthBottom = 0.75f
             };

             PdfPCell clAlbaran = new PdfPCell(new Phrase("Nº de Albarán", fuenteEstandar))
             {
                 BorderWidth = 0,
                 BorderWidthBottom = 0.75f
             };

             tblRecepcion.AddCell(clProveedor);
             tblRecepcion.AddCell(clAlbaran);

             // Llenamos la tabla con información
             clProveedor = new PdfPCell(new Phrase(DateTime.Now.ToString(), fuenteEstandar))
             {
                 BorderWidth = 0
             };
             clAlbaran = new PdfPCell(new Phrase("231313", fuenteEstandar))
             {
                 BorderWidth = 0
             };

             tblRecepcion.AddCell(clProveedor);
             tblRecepcion.AddCell(clAlbaran);

             doc.Add(tblRecepcion);*/

            /* PdfPTable tblRecepcion = new PdfPTable(2)
             {
                 HorizontalAlignment = 0,
                 WidthPercentage = 50
             };

             var clProveedorTitulo = new PdfPCell(new Phrase("Proveedor", fuenteNegrita))
             {
                 BorderWidth = 0
             };

             var clProveedor = new PdfPCell(new Phrase(DateTime.Now.ToString(), fuenteNormal))
             {
                 BorderWidth = 0
             };

             var clAlbaranTitulo = new PdfPCell(new Phrase("Nº de Albarán", fuenteNegrita))
             {
                 BorderWidth = 0
             };
             var clAlbaran = new PdfPCell(new Phrase("231313", fuenteNormal))
             {
                 BorderWidth = 0
             };

             tblRecepcion.AddCell(clProveedorTitulo);
             tblRecepcion.AddCell(clProveedor);

             tblRecepcion.AddCell(clAlbaranTitulo);
             tblRecepcion.AddCell(clAlbaran);
             doc.Add(tblRecepcion);

             doc.Close();
             writer.Close();*/

            PdfWriter writer = new PdfWriter(nombrePdf);

            PdfDocument pdf = new PdfDocument(writer);
            PdfDocumentInfo info = pdf.GetDocumentInfo();
            info.AddCreationDate();
            info.SetAuthor("BiomasaEUPT");
            info.SetCreator("BiomasaEUPT");
            info.SetTitle("Materia Prima #" + materiaPrima.Codigo + " " + fechaCreacion.ToString("dd-MM-yyyy HH-mm-ss"));
            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new MyEventHandler(this));

            Document doc = new Document(pdf, PageSize.A4);
            doc.SetMargins(70, 70, 85, 85);

            Paragraph p = new Paragraph("Recepción").SetTextAlignment(TextAlignment.CENTER).SetFont(cambria).SetBold().SetFontSize(16);
            doc.Add(p);
            Table tablaRecepcion = new Table(new float[] { 1, 1 }).SetWidthPercent(50);
            tablaRecepcion.AddHeaderCell(new Cell().Add(new Paragraph("Fecha Recepción").SetFont(calibri).SetBold()).SetFontSize(13).SetBorder(null));
            tablaRecepcion.AddHeaderCell(new Cell().Add(new Paragraph(proveedor.Recepciones.First().FechaRecepcion.ToString("dd/MM/yyyy HH:mm")).SetFont(calibri)).SetFontSize(13).SetBorder(null));
            tablaRecepcion.AddHeaderCell(new Cell().Add(new Paragraph("Nº de Albarán").SetFont(calibri).SetBold()).SetFontSize(13).SetBorder(null));
            tablaRecepcion.AddHeaderCell(new Cell().Add(new Paragraph(proveedor.Recepciones.First().NumeroAlbaran).SetFont(calibri)).SetFontSize(13).SetBorder(null));
            doc.Add(tablaRecepcion);

            doc.Add(new Paragraph("\n"));
            doc.Add(new Paragraph("Proveedor").SetFont(cambria).SetBold().SetFontSize(16));
            Table tablaProveedor = new Table(new float[] { 1, 1 }).SetWidthPercent(50);
            tablaProveedor.AddHeaderCell(new Cell().Add(new Paragraph("Razon Social").SetFont(calibri).SetBold()).SetFontSize(13).SetBorder(null));
            tablaProveedor.AddHeaderCell(new Cell().Add(new Paragraph(proveedor.RazonSocial).SetFont(calibri)).SetFontSize(13).SetBorder(null));
            tablaProveedor.AddHeaderCell(new Cell().Add(new Paragraph("NIF").SetFont(calibri).SetBold()).SetFontSize(13).SetBorder(null));
            tablaProveedor.AddHeaderCell(new Cell().Add(new Paragraph(proveedor.Nif).SetFont(calibri)).SetFontSize(13).SetBorder(null));
            tablaProveedor.AddHeaderCell(new Cell().Add(new Paragraph("Tipo").SetFont(calibri).SetBold()).SetFontSize(13).SetBorder(null));
            tablaProveedor.AddHeaderCell(new Cell().Add(new Paragraph(proveedor.TipoProveedor.Nombre).SetFont(calibri)).SetFontSize(13).SetBorder(null));
            doc.Add(tablaProveedor);

            doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));


            doc.Close();

            return nombrePdf;
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
