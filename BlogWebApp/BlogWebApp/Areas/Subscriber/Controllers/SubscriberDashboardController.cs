using BlogWebApp.Data;
using BlogWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlogWebApp.Controllers
{
    [Area("Subscriber")]

    //Returns Index view.
    public class SubscriberDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}