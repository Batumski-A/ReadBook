using Microsoft.AspNetCore.Mvc;

namespace ReadBook.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
