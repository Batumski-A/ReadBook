using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Navigation;
using ReadBook.Data;
using ReadBook.Models.Pdf;

namespace ReadBook.Helpers.HPdf
{
    public class BookAddHiperlink
    {
        private readonly AppDbContext _context;

        public BookAddHiperlink(AppDbContext context)
        {
            _context = context;
        }
        public void AddHiperlink(int bookId)
        {
            List<Content> contents = _context.contents.Where(c => c.PdfRequestId == bookId).ToList();
            int count = 0;

            string SRC = @"././wwwroot/files/Book/" + contents.First().Title;
            string DST = @"././wwwroot/files/LinkedBook/" + contents.First().Title;

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(DST));
            foreach (Content cont in contents)
            {
                count++;
                PdfExplicitDestination GoToDestination = PdfExplicitDestination.CreateFit(pdfDoc.GetPage(cont.PageIndex));
                if (cont.GoToPage != 0)
                {
                    GoToDestination = PdfExplicitDestination.CreateFit(pdfDoc.GetPage(cont.GoToPage));
                }
                float height = pdfDoc.GetPage(cont.PageIndex).GetPageSize().GetHeight();
                textPos defaultConv = convertor(cont.TextPosition, cont.TextWidthAndHeight, height);
                textPos goToConv = convertor(cont.GoToPageTextPostion, cont.GoToPageTextWidthAndHeight, height);

                //float positionY = Convert.ToSingle(pdfDoc.GetPage(cont.PageIndex).GetPageSize().GetHeight() - Convert.ToDouble(cont.TextPosition.Split("|")[1]) - 10);
                Rectangle linkLocation = new Rectangle(defaultConv.X, defaultConv.Y, defaultConv.Width, defaultConv.Height);

                Rectangle goToLinkLocation = new Rectangle(goToConv.X, goToConv.Y, goToConv.Width, goToConv.Height);
                if (cont.LinkType)
                {
                    PdfAnnotation annotation = new PdfLinkAnnotation(linkLocation)
                       .SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT)
                       .SetAction(PdfAction.CreateGoTo(GoToDestination))
                       .SetBorder(new PdfArray(new int[] { 0, 0, 1 }));

                    
                    PdfAnnotation secondAnnotation = new PdfLinkAnnotation(goToLinkLocation)
                     .SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT)
                     .SetAction(PdfAction.CreateURI(String.Format("?title={0}&page={1}", cont.PdfRequestId, cont.GoToPage)))
                     .SetBorder(new PdfArray(new int[] { 0, 0, 1 }));
                    System.Diagnostics.Debug.WriteLine("GoToPage: " + cont.GoToPage + " XYWH" + goToConv.X + " " + goToConv.Width);

                    pdfDoc.GetPage(cont.PageIndex).AddAnnotation(annotation);
                    pdfDoc.GetPage(cont.GoToPage).AddAnnotation(secondAnnotation);
                }

                if (contents.Count == count)
                {
                    pdfDoc.Close();
                }
            }
        }
        private textPos convertor(string posXY, string textWH,float PageHeight)
        {
            if (posXY.Contains('|') && textWH.Contains('|'))
            {
                textPos pos = new textPos
                {
                    X = Convert.ToSingle(posXY.Split("|")[0]),
                    Y = PageHeight - Convert.ToSingle(posXY.Split("|")[1]) - 10,
                    Width = Convert.ToSingle(textWH.Split("|")[0]),
                    Height = Convert.ToSingle(textWH.Split("|")[1])
                }; return pos;
            }
            else
            {
                textPos pos = new textPos
                {
                    X = 0,
                    Y = 0,
                    Width = 0,
                    Height = 0
                }; return pos;
            }
        }

        private class textPos
        {
            public float X;
            public float Y;
            public float Width;
            public float Height;
        }
    }
}
