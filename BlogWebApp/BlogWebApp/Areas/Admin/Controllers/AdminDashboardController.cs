using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Areas.Admin.Controllers
{
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
