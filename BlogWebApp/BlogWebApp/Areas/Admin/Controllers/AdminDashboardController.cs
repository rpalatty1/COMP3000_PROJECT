using BlogWebApp.Data;
using BlogWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _applicationUser;

        public AdminDashboardController(ApplicationDbContext context, UserManager<ApplicationUser> applicationUser)
        {
            _context = context;
            _applicationUser = applicationUser;

        }
        //public async IActionResult Index()
        //{
        //    var applicationDbContext = _context.Blog.Include(b => b.ApplicationUser);
        //    return View(await );

        //}
    }
}
