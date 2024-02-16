using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Areas.Subscriber.Controllers
{
    public class SubscriberDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
