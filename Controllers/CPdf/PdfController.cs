using Microsoft.AspNetCore.Mvc;
using ReadBook.Data;
using Spire.Pdf;
using ReadBook.Helpers.HPdf;
using ReadBook.Models;
using ReadBook.Models.Pdf;

namespace ReadBook.Controllers.Pdf
{
    public class PdfController : Controller
    {
        private readonly PdfDocument doc = new PdfDocument();
        private readonly AppDbContext _context;
        private IWebHostEnvironment _hostingEnvironment = null;

        public PdfController(AppDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index(string fileName = "ინანიშვილი - საბავშვო კრებული.pdf")
        {
            AllBook allBook = new AllBook();
            allBook = _context.allBooks.FirstOrDefault(b => b.BookName == fileName);
            if(allBook == null)
            {
                allBook = new AllBook
                {
                    BookName = fileName
                };
                _context.allBooks.Add(allBook);
                _context.SaveChanges();
            }
            FilesController filesController = new FilesController(_hostingEnvironment,_context);
            return View(filesController.Index());
        }

        [HttpPost("AutoDefragmentationPDF")]
        public string AutomaticPDFdefragmentation(string fileName,string contentsName = "სარჩევი")
        {
            BookToDatabase bookToDatabase = new BookToDatabase(_context);
            bookToDatabase.BookContent(fileName, contentsName);

            BookAddHiperlink bookAddHiperlink = new BookAddHiperlink(_context);
            bookAddHiperlink.AddHiperlink(0);

            return "";
        }
    }
}
