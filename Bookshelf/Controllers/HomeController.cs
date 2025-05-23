using Microsoft.AspNetCore.Mvc;

namespace Bookshelf.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
