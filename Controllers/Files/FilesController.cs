using Microsoft.AspNetCore.Mvc;
using ReadBook.Data;
using ReadBook.Helpers.HPdf;
using ReadBook.Models;

namespace ReadBook.Controllers
{
    public class FilesController : Controller
    {
        IWebHostEnvironment _hostingEnvironment = null;
        private readonly AppDbContext _context;

        public FilesController(IWebHostEnvironment hostingEnvironment, AppDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(string fileName = "")
        {
            FileClass fileObj = new FileClass();
            fileObj.Name = fileName;
            string path = $"{_hostingEnvironment.WebRootPath}\\files\\Book";
            int nId = 1;

            foreach (string pdfPath in Directory.EnumerateFiles(path, "*.pdf"))
            {
                fileObj.Files.Add(new FileClass()
                {
                    FileId = nId++,
                    Name = Path.GetFileName(pdfPath),
                    Path = pdfPath
                });
            }
            return View(fileObj);
        }

        [HttpPost]
        public IActionResult Index(IFormFile file, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\Book\\{file.FileName}";
            TempData["msg"] = "File Uploaded successfully";
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            return Index();
        }

        public IActionResult PDFViewerNewTab(string fileName)
        {
            string path = _hostingEnvironment.WebRootPath + "\\files/Linked\\" + fileName;
            return File(System.IO.File.ReadAllBytes(path), "application/pdf");
        }

        [HttpGet("/AutoF")]
        public ActionResult AutoPdfFragmentation(string fileName)
        {
            BookToDatabase bookToDatabase = new BookToDatabase(_context);
            int fileNameId = bookToDatabase.BookContent(fileName);
            BookAddHiperlink bookAddHiperlink = new BookAddHiperlink(_context);
            bookAddHiperlink.AddHiperlink(fileNameId);
            return View(fileName);
        }
       /* [HttpGet("files/Book/ინანიშვილი - საბავშვო კრებულიedited.pd")]
        public string Book(string fileName= "ინანიშვილი - საბავშვო კრებულიedited.pdf", int page = 0,string Coordinates = "",string TextWithAndHeight = "")
        {
            return Coordinates;
        }*/
    }
}
