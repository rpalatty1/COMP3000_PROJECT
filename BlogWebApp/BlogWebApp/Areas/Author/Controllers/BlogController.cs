using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogWebApp.Data;
using BlogWebApp.Models;
using Microsoft.AspNetCore.Identity;
using BlogWebApp.Models.ViewModels;
using BlogWebApp.Data.Migrations;
using System.Security.Claims;

namespace BlogWebApp.Areas.Author.Controllers
{
    [Area("Author")]
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _applicationUser;

        //Constructor to inject dependencies.
        public BlogController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> applicationUser)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _applicationUser = applicationUser;

        }

        //GET - Method to retrieve and display blog information of user currently logged in.
        public async Task<IActionResult> Index()
        {
            //Gets current user's id.
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            //Gets blogs from database that belong to the user.
            var applicationDbContext = _context.Blog.Include(b => b.ApplicationUser).Include(b => b.Category).Include(b => b.SubCategory).Where(b => b.ApplicationUserId == userId);
            //Returns blogs to view.
            return View(await applicationDbContext.ToListAsync());
        }

        //GET - Method to retrieve and display details of selceted blog.
        public async Task<IActionResult> Details(int? id)
        {
            //Checks if blog id and entity set is available. If null, returns error message.
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            //Retrieves blog from dtabase based on id.
            var blog = await _context.Blog
                .Include(b => b.ApplicationUser)
                .Include(b => b.Category)
                .Include(b => b.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            //Check if blog found. If not, return error.
            if (blog == null)
            {
                return NotFound();
            }

            //Passes blog details to view.
            return View(blog);
        }


        //GET - Method to Create a new blog.
        public IActionResult Create()
        {
            //Adds new instance of BlogVM.
            BlogVM obj = new()
            {
                //Initialises new blog object.
                blog = new(),
                //To add category and subcategory data in lists for dropdown.
                CategoryList = new SelectList(_context.Category, "Id", "CategoryName"),
                SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName")
  
            };
            //Returns Create view with new object.
            return View(obj);
        }

        //POST - Method to Create and publish a new blog. 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogVM obj, IFormFile? file)
        {
         
            {
                //Checks if uploaded file is not null and gets root path.
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    //Generates unique file name and saves in specified directory.
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"blogImages/titleImages/");
                    //Gets file extension.
                    var extension = Path.GetExtension(file.FileName);
                    //Creates file stream to copy and save upoaded file.
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    //Sets TitleImageUrl of blog object to the file path.
                    obj.blog.TitleImageUrl = @"\blogImages\titleImages\" + fileName + extension;
                }

                //Sets these properties to blog.
                obj.blog.CreatedBy = _applicationUser.GetUserId(HttpContext.User);
                obj.blog.ApplicationUserId = _applicationUser.GetUserId(HttpContext.User);
                //Adds blog to Db context and saves changes to database.
                _context.Blog.Add(obj.blog);
                _context.SaveChanges();

                //Success message and redirected to Index.
                TempData["success"] = "Blog created successfully";
                return RedirectToAction("Index");

            }
            return View(obj);
        }
   
        //GET - Method to retrieve and display selected blog for Edit.
        public async Task<IActionResult> Edit(int? id)
        {
            //Checks if blog id or DbSet is null. If so, return error.
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            //Find blog in database by id.
            var blogObj = await _context.Blog.FindAsync(id);
            //Check if null. If so, return error and redirected to Index.
            if (blogObj == null)
            {
                //return NotFound();
                return RedirectToAction("Index");
            }

            //Creates new object to pass to view, including lists.
            BlogVM obj = new()
            {
                blog = blogObj,
                CategoryList = new SelectList(_context.Category, "Id", "CategoryName"),
                SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName")
 
            };

            //Returns to Edit view.
            return View(obj);
        }

        //POST - Method to Edit a blog.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogVM obj, IFormFile? file)
        {
            //To create category an dsubcategory dropdown lists.
            obj.CategoryList = new SelectList(_context.Category, "Id", "CategoryName");
            obj.SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName");

            {
                //Gets root path.
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                //Checks if file uploaded.
                if (file != null)
                {
                    //Initialises new blog object.
                    Blog objBlog = new Blog();
                    //Generated unique file name and saves in directory.
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"blogImages/titleImages/");
                    //Gets file extension.
                    var extension = Path.GetExtension(file.FileName);

                    //Delete old image if exists.
                    if (obj.blog.TitleImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.blog.TitleImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }
                    //To save new image file.
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    //Update with new details.
                    obj.blog.TitleImageUrl = @"\blogImages\titleImages\" + fileName + extension;
                    obj.blog.ModifiedBy = _applicationUser.GetUserId(HttpContext.User);
                    obj.blog.ModifiedDate = DateTime.Now;
                    obj.blog.ApplicationUserId = _applicationUser.GetUserId(HttpContext.User);
                }

                //Update and save changes to database.
                _context.Update(obj.blog);
                await _context.SaveChangesAsync();

                //Returns success message and redirected to Index.
                TempData["success"] = "Blog created successfully";
                return RedirectToAction("Index");

            }
            return View(obj);
        }

        //GET - Method to retrieve and display blog details for Delete.
        public async Task<IActionResult> Delete(int? id)
        {
            //Checks if blog id or set is null. If so, retuns error.
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            //Gets selected blog details from database.
            var blog = await _context.Blog
                .Include(b => b.ApplicationUser)
                .Include(b => b.Category)
                .Include(b => b.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            //Checks if blog exists. If not, return error.
            if (blog == null)
            {
                return NotFound();
            }

            //Passes selected blog details to Delete view.
            return View(blog);
        }

        //POST - Method to Delete a blog.

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Checks if blog set is null. If so, return error message.
            if (_context.Blog == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Blog'  is null.");
            }
            //Find selected blog from database by id.
            var blog = await _context.Blog.FindAsync(id);
            //Check if selected blog exists.
            if (blog != null)
            {
                //Remove blog from database.
                _context.Blog.Remove(blog);
            }

            //Save changes to database and redirect to Index.
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Helper method to check if blog exists by id and returns true/false.
        private bool BlogExists(int id)
        {
            return (_context.Blog?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
