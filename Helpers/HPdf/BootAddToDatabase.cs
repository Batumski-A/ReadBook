using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Text;

namespace ReadBook.Helpers.HPdf
{
    public class BootAddToDatabase
    {
        public string qweqwe(string fileName)
        {
            StringBuilder text = new StringBuilder();
            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new (fileName);
                PdfDocument pdfdoc = new (pdfReader);
                for (int page = 1; page <= pdfdoc.GetNumberOfPages(); page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string data = PdfTextExtractor.GetTextFromPage(pdfdoc.GetPage(page),strategy);
                    string currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(
                        Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes("სარჩევი")));
                                        System.Diagnostics.Debug.WriteLine(data);

                }
            }
            return text.ToString();
        }
    }

    
}
