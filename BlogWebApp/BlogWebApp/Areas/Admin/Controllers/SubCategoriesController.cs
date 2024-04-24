using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogWebApp.Data;
using BlogWebApp.Models;
using BlogWebApp.Models.ViewModels;

namespace BlogWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        //Constructor to inject dependency.
        public SubCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET - Method to retrieve and display the list of subcategories.
        public async Task<IActionResult> Index()
        {
            //Retrieves all subcategories from datbase, including related entity category.
            var applicationDbContext = _context.SubCategory.Include(s => s.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET - Method to retrieve and display details of the selected Category.
        public async Task<IActionResult> Details(int? id)
        {
            //Checks if id or Db context is null. If so, return error message.
            if (id == null || _context.SubCategory == null)
            {
                return NotFound();
            }

            //Retrieves information of selected subcategory using id.
            var subCategory = await _context.SubCategory
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            //Checks if null. If so, returns error.
            if (subCategory == null)
            {
                return NotFound();
            }

            //Passes retrieved details and displays in Details view.
            return View(subCategory);
        }

        // GET - Method to display Create view for SubCategory.
        public IActionResult Create()
        {
            //Creates list of categories to be used for dropdown in view.
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            //Returns Create view.
            return View();
        }

        // POST - Method to Create a new SubCategory.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubCategoryName,IsActive,CategoryId,Id,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] SubCategory subCategory)
        {
            //Checks if Model state is valid.
            if (ModelState.IsValid)
            {
                //Add new subcategory to Db context.
                _context.Add(subCategory);
                //Save changes to database.
                await _context.SaveChangesAsync();
                //If successful, redirect to Index.
                return RedirectToAction(nameof(Index));
            }
            //If state not valid, repopulate ViewData with category list and return view with the subcategory data.
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET - Method to retrieve and diaplay details for Edit.
        public async Task<IActionResult> Edit(int? id)
        {
            //Check if sybcategory id or DbSet is null. If so, return erro.
            if (id == null || _context.SubCategory == null)
            {
                return NotFound();
            }

            //Retrieve subcategory from database by id.
            var subCategory = await _context.SubCategory.FindAsync(id);
            //Check if retrieved subcategory os null. If so, return error.
            if (subCategory == null)
            {
                return NotFound();
            }
            //Populate ViewData with category list dropdown and pass retrieved subcategory details to Edit view.
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", subCategory.CategoryId);
            return View(subCategory);
        }

        // POST - Method to Edit a subcategory.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubCategoryName,IsActive,CategoryId,Id,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] SubCategory subCategory)
        {
            //Check if recieved id matches id or subcategory being edited. If not, return error.
            if (id != subCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Update subcategory details in Db context.
                    _context.Update(subCategory);
                    //Save changes to database.
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //Check if subcategory with the id. If not, return error.
                    if (!SubCategoryExists(subCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        //Throw exception if concurrency issue.
                        throw;
                    }
                }
                //Redirect to Index view.
                return RedirectToAction(nameof(Index));
            }
            //Add list of categories to ViewData and return view with provided subcategory data.
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET - Method to retrieve data to Delete a SubCategory.
        public async Task<IActionResult> Delete(int? id)
        {
            //Check if subcategory id or DbSet is null. If so, return error.
            if (id == null || _context.SubCategory == null)
            {
                return NotFound();
            }

            //Retrieve subcategory details from database by id, including category.
            var subCategory = await _context.SubCategory
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            //Check if subcategory is null. If so, return error.
            if (subCategory == null)
            {
                return NotFound();
            }

            //Pass retrieved subcategory details into Delete view.
            return View(subCategory);
        }

        // POST - Method to Delete a SubCategory.

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Check if SubCategory DbSet is null. If so, return problem response.
            if (_context.SubCategory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SubCategory'  is null.");
            }
            //Retrieve SubCategory from database by id.
            var subCategory = await _context.SubCategory.FindAsync(id);
            //heck if SubCategory exists.
            if (subCategory != null)
            {
                //Remove SubCategory from Db context
                _context.SubCategory.Remove(subCategory);
            }
            
            //Save changes to database and redirects to index.
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Helper method to check if SubCategory with specified id exists, and returns true/false.
        private bool SubCategoryExists(int id)
        {
          return (_context.SubCategory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
