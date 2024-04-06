using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Areas.TestArea.Controllers
{
    public class TestDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
