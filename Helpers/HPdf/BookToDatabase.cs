using ReadBook.Data;
using ReadBook.Models.Pdf;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ReadBook.Helpers.HPdf
{
    public class BookToDatabase
    {
        private readonly AppDbContext _context;
        private readonly PdfDocument doc = new ();
        private int bookId;
        public BookToDatabase(AppDbContext context)
        {
            _context = context;
        }

        public int BookContent(string fileName,string contentsName = "სარჩევი")
        {
            doc.LoadFromFile(@"././wwwroot/files/Book/" + fileName);
            int PCindex = 0;
            PdfTextFind[] results = null;
            foreach (PdfPageBase page in doc.Pages)
            {
                results = page.FindText(contentsName).Finds;
                foreach (PdfTextFind text in results)
                {
                    PointF p = text.Position;
                    PCindex = text.SearchPageIndex;
                    AllBook allbook = new AllBook()
                    {
                        BookName = fileName,
                        BookPages = doc.Pages.Count,
                    };
                    try
                    {
                        if (_context.allBooks.FirstOrDefault(a => a.BookName == fileName) == null)
                        {
                            _context.allBooks.Add(allbook);
                            _context.SaveChanges();
                        }
                    }
                    catch (Exception ex) { }

                    bookId = _context.allBooks.FirstOrDefault(a => a.BookName == fileName).Id;

                    Content Master = new Content()
                    {
                        Title = fileName,
                        ContentName = "სარჩევი",
                        PageIndex = PCindex + 1,
                        PdfRequestId = bookId,
                        TextPosition = String.Format("{0} | {1}", (double)p.X, (double)p.Y),
                        TextWidthAndHeight = String.Format("{0} | {1}", (double)text.Size.Width, (double)text.Size.Height)
                    };
                    if (_context.contents.FirstOrDefault(c => c.PdfRequestId == Master.PdfRequestId && c.TextPosition == Master.TextPosition && c.PageIndex == Master.PageIndex) == null)
                    {
                        _context.contents.Add(Master);
                        _context.SaveChanges(true);
                    }
                    PdfPageBase pdfPageBase = doc.Pages[PCindex];
                    string[] lines = pdfPageBase.ExtractText().Replace("\r", "").Split('\n');
                    foreach (string line in lines)
                    {
                        if (Regex.Match(line.Trim(), @"\d+").Value != "")
                        {
                            PdfTextFind[] result = page.FindText(line).Finds;
                            PointF textP = result[0].Position;
                            double x = (double)textP.X;
                            double y = (double)textP.Y;
                            string pageIndex = Regex.Match(line.Trim(), @"\d+").Value;
                            string lineText = line.Replace(pageIndex, "");
                            double with = (double)result[0].Size.Width;
                            double height = (double)result[0].Size.Height;
                            if (x == 0 && y == 0)
                            {
                                Content lastPosition = _context.contents.Where(c => c.PdfRequestId == bookId).ToList().Last();
                                string[] lastBox = _context.contents.Where(c => c.PdfRequestId == bookId).ToList().Last().TextWidthAndHeight.Split('|');
                                string[] lastPosition2 = _context.contents.Where(c => c.PdfRequestId == bookId & c.Id == lastPosition.Id - 1).First().TextPosition.Split("|");

                                x = Convert.ToSingle(lastPosition.TextPosition.Split("|")[0]);
                                y = Convert.ToSingle(lastPosition.TextPosition.Split("|")[1]) - ((Convert.ToSingle(lastPosition2[1]) - Convert.ToSingle(lastPosition.TextPosition.Split("|")[1])));
                                with = Convert.ToSingle(lastBox[0]);
                                height = Convert.ToSingle(lastBox[1]);
                            }

                            Content content = new ()
                            {
                                Title = fileName,
                                ContentName = lineText.Trim(),
                                PageIndex = PCindex + 1,
                                PdfRequestId = bookId,
                                LinkType = true,
                                GoToPage = int.Parse(pageIndex),
                                TextPosition = String.Format("{0} | {1}", x, y),
                                TextWidthAndHeight = String.Format("{0} | {1}", with, height)
                            };

                            if (_context.contents.FirstOrDefault(c => c.PdfRequestId == content.PdfRequestId && c.PageIndex == content.PageIndex && c.ContentName == content.ContentName) == null)
                            {
                                _context.contents.Add(content);
                                _context.SaveChanges(true);
                            }
                        }
                    }
                }
            }

            List<Content> goToPageCont = _context.contents.Where(c=>c.LinkType == true && c.Title == fileName).ToList();
            foreach (Content content in goToPageCont)
            {
                PdfTextFind goToResultText = null;
                try
                {
                    PdfPageBase goToPage = doc.Pages[content.GoToPage - 1];
                    goToResultText = goToPage.FindText(content.ContentName).Finds[0];
                    string contentName = content.ContentName;
                    for (int a = 1; a < contentName.Length; a++)
                    {
                        goToResultText = goToPage.FindText(contentName).Finds[0];
                        if (goToResultText == null)
                        {
                            contentName = contentName[0..^(contentName.Length-1)];
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    if (goToResultText != null)
                    {
                        PointF gP = goToResultText.Position;
                        float gWith = goToResultText.Size.Width;
                        float gHeight = goToResultText.Size.Height;
                        float gX = gP.X;
                        float gY = gP.Y;
                        content.GoToPageTextPostion = String.Format("{0} | {1}",gX,gY);
                        content.GoToPageTextWidthAndHeight = String.Format("{0} | {1}",gWith,gHeight); 
                        _context.contents.Update(content);
                        _context.SaveChanges();
                    }
                }
                catch
                {
                }
            }
            return bookId;
        }
    }
}
