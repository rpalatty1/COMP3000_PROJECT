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

        //Constructor to inject dependencies.
        public AdminDashboardController(ApplicationDbContext context, UserManager<ApplicationUser> applicationUser)
        {
            _context = context;
            _applicationUser = applicationUser;

        }

        //Action method for index view to help display relevant blog details for the dashboard.
        public async Task<IActionResult> Index()
        {
            //Retrieves blogs from the database related to ApplicationUser.
            var blogs = _context.Blog.Include(b => b.ApplicationUser);

            //Passes data to view using ViewData.
            ViewData["BlogCount"] = blogs.Count();
            ViewData["CategoryCount"] = _context.Category.Count();
            //Returns view with a list of 5 blogs.
            return View(await blogs.Take(5).ToListAsync());


        }

        //Method to retrieve and display the list of users.
        public async Task<IActionResult> Users()
        {
            var users = await _applicationUser.Users.ToListAsync();
            return View(users);
        }

        //POST - allows Admin to DELETE users
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            //Finds user by id.
            var user = await _applicationUser.FindByIdAsync(id);
            //To check if user exists.
            if (user == null)
            {
                //If user not found, returns 404 not found error message.
                return NotFound();
            }

            //Attempts to delete user.
            var result = await _applicationUser.DeleteAsync(user);
            //Checks if deletion was successful.
            if (!result.Succeeded)
            {
                //If failed, displays error message.
                ModelState.AddModelError("", "Failed to delete user.");
            }

            //Redirects to action responsible for displaying user list ('UserManager').
            return RedirectToAction(nameof(Users));
        }

    }
}

