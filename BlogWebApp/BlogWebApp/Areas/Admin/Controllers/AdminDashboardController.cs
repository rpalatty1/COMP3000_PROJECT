using BlogWebApp.Data;
using BlogWebApp.Data.Migrations;
using BlogWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Index()
        {
            var blogs = _context.Blog.Include(b => b.ApplicationUser);
            ViewData["BlogCount"] = blogs.Count();
            ViewData["CategoryCount"] = _context.Category.Count();
            //ViewData["Author"] = new SelectList(_context.ApplicationUsers, "Id", "ApplicationUsers", ApplicationId.ApplicationUser);
            return View(await blogs.Take(5).ToListAsync());


        }

        // GET: /Admin/Users

        public async Task<IActionResult> Users()

        {
            var users = await _applicationUser.Users.ToListAsync();
            return View(users);
        }

        // POST: /Admin/DeleteUser/{id}

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteUser(string id)

        {

            var user = await _applicationUser.FindByIdAsync(id);
            if (user == null)

            {
                return NotFound();
            }

            var result = await _applicationUser.DeleteAsync(user);
            if (!result.Succeeded)

            {
                ModelState.AddModelError("", "Failed to delete user.");
            }

            return RedirectToAction(nameof(Users));

        }

    }
}

