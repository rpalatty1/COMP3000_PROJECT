using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogWebApp.Data;
using BlogWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlogWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _applicationUser;
        private readonly IWebHostEnvironment _iWebHostEnvironment;

        //Constructor to inject dependencies.
        public CategoriesController(ApplicationDbContext context, UserManager<ApplicationUser> applicationUser, IWebHostEnvironment iWebHostEnvironment)
        {
            _context = context;
            _iWebHostEnvironment = iWebHostEnvironment;
            _applicationUser = applicationUser;
        }

        // GET - Method to retrieve and display the list of categories.
        public async Task<IActionResult> Index()
        {
            //Checks if categories are not null.
              return _context.Category != null ? 
                          //If not null, passes to view. If null returns problem response.
                          View(await _context.Category.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Category'  is null.");
        }

        // GET - Method to retrieve and display details of the selected Category.
        public async Task<IActionResult> Details(int? id)
        {
            //Checks if id or Db context is null. If so, return error message. 
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            //Retrieves information of selected category using id.
            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            //Checks if null. If so, returns error.
            if (category == null)
            {
                return NotFound();
            }

            //Passes retrieved details and displays in Details view.
            return View(category);
        }

        //Method to display Create view for Category.
        public IActionResult Create()
        {
            return View();
        }

        // POST - Method to Create a new category.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,CategorySlug,DefaultImageUrl,Description,IsActive,Id,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Category category, IFormFile? file)
        {
            {
                //Retrieves root path.
                string wwwRootPath = _iWebHostEnvironment.WebRootPath;
                //Checks if file uploaaded.
                if(file != null)
                {
                    //Generates unique file name and defines where it will be saved.
                    string fileName = Guid.NewGuid().ToString();
                    string uploads = Path.Combine(wwwRootPath, @"blogImages/category");
                    //Gets file extension.
                    var extension = Path.GetExtension(file.FileName);
                    //Creates file stream and copies file to directory.
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    //Sets DefaultImageUrl property with uploaded file path.
                    category.DefaultImageUrl = @"\blogImages\category\" + fileName + extension;
                }

                //Sets CreatedBy to current user's id.
                category.CreatedBy = _applicationUser.GetUserId(HttpContext.User);
                //Add category to Db context.
                _context.Add(category);
                //Changes saved to database, success message displayes and redirected to Index.
                await _context.SaveChangesAsync();
                TempData["success"] = "Category saved successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET - Method to retrieve data for category Edit.
        public async Task<IActionResult> Edit(int? id)
        {
            //Checks if selected category is null. If so, returns error message.
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            //Retrieves category from database by id.
            var category = await _context.Category.FindAsync(id);
            //Checks if null. If so, displays error message.
            if (category == null)
            {
                return NotFound();
            }
            //Passes retrieved details to Edit view.
            return View(category);
        }

        // POST - Method to Edit a category.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryName,CategorySlug,DefaultImageUrl,Description,IsActive,Id,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Category category)
        {
            //Check if category id matches model's id. If not, return error message.
            if (id != category.Id)
            {
                return NotFound();
            }

            {
                //To try updating category in database.
                try
                {
                    //Updates category in Db context and saves changes to the database.
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                //Catches concurrency conflicts.
                catch (DbUpdateConcurrencyException)
                {
                    //Checks if category doesn't exist. If so, returns error message.
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        //If concurrency conflct other than missing category, throw the exception.
                        throw;
                    }
                }
                //If successful, redirect to Index.
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET - Method to retrieve details to Delete a category.
        public async Task<IActionResult> Delete(int? id)
        {
            //Checks if category id or DbSet is null. If so, return error message.
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            //Retrieve selected category's details.
            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            //Check if selected category is null. If so, return error message.
            if (category == null)
            {
                return NotFound();
            }

            //Passes retrieved category details to Delete view.
            return View(category);
        }

        // POST - Method to Delete a category.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Checks if category DBSet is null. If so, returns problem response.
            if (_context.Category == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Category'  is null.");
            }
            //Retrieves selected category from database by id.
            var category = await _context.Category.FindAsync(id);
            //Checks if retrieved category exists.
            if (category != null)
            {
                //Removes category from Db context.
                _context.Category.Remove(category);
            }
            
            //Changes saved to database and redirected to Index.
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Helper method to check if category with specified id exists and returns true/false.
        private bool CategoryExists(int id)
        {
          return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
