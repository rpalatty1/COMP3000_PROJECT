using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Areas.Author.Controllers
{
    [Area("Author")]
    public class AuthorDashboardController : Controller
    {
        //Returns Index view.
        public IActionResult Index()
        {
            return View();
        }
    }
}
