using Microsoft.AspNetCore.Mvc;
using ReadBook.Models.Pdf;

namespace ReadBook.Controllers
{
    public class EditPdfController : Controller
    {
        private string _fileName = "";

        public EditPdfController()
        {
        }
        [HttpGet]
        public IActionResult Index(string fileName, int page, string Coordinates, string TextWithAndHeight)
        {
            _fileName = fileName;
            AllBook allBook = new AllBook();
            allBook.BookName = fileName;
            allBook.BookPages = page;
            allBook.Coordinates = Coordinates;
            allBook.WithHeight = TextWithAndHeight;
            return View(allBook);
        }
    }
}
