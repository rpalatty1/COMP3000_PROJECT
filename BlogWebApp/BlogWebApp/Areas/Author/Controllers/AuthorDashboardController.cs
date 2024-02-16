using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Areas.Author.Controllers
{
    public class AuthorDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
