using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Areas.Author.Controllers
{
    [Area("Author")]
    public class AuthorDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
