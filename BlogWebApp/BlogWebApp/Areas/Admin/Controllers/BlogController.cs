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

namespace BlogWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        //Method to retrieve and display the list of blogs.
        public async Task<IActionResult> Index()
        {
            //Retrieves blogs from the database relating to these entities.
            var applicationDbContext = _context.Blog.Include(b => b.ApplicationUser).Include(b => b.Category).Include(b => b.SubCategory);
            //Returns a list view of blogs.
            return View(await applicationDbContext.ToListAsync());
        }

        //Action method to retrieve and display details of the selected blog. 
        public async Task<IActionResult> Details(int? id)
        {
            //Checks if Blog id or DbSet is null.
            if (id == null || _context.Blog == null)
            {
                //Returns a 404 Not Found error message.
                return NotFound();
            }

            //Retrieves information of the selected blog, includes these entities.
            var blog = await _context.Blog
                .Include(b => b.ApplicationUser)
                .Include(b => b.Category)
                .Include(b => b.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);

            //Checks if selected blog is null.
            if (blog == null)
            {
                //Returns a 404 Not Found error message.
                return NotFound();
            }

            //Passes selected blog's details to 'Details' view.
            return View(blog);
        }

        // GET - action method to create a new blog.
        public IActionResult Create()
        {
            //Initialises a new instance of BlogVM.
            BlogVM obj = new()
            {
                //Initialises new blog instance.
                blog = new(),
                //Creates a category list containing Category Id and CategoryName.
                CategoryList = new SelectList(_context.Category, "Id", "CategoryName"),
                //Creates a subcategory list containing SubCategory Id and SubCategoryName.
                SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName")

            };

            //Passes the initialised BlogVM object to the Create view.
            return View(obj);
        }

        // POST - Action method to publish the written blog.

        [HttpPost]
        [ValidateAntiForgeryToken] //To prevent cross-site request forgery attacks.

        public IActionResult Create(BlogVM obj, IFormFile? file)
        {
            {
                //Retrieves root path of wwwRootPath folder.
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                //Checks if file uploaded.
                if (file != null)
                {
                    //Generates unique file name using GUID where file is saved.
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"blogImages/titleImages/");
                    //Gets file extension.
                    var extension = Path.GetExtension(file.FileName);
                    //Creates file stream and copies file to directory.
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    //Sets titleImage URL for post.
                    obj.blog.TitleImageUrl = @"\blogImages\titleImages\" + fileName + extension;
                }

                //Sets these properties for the blog post.
                obj.blog.CreatedBy = _applicationUser.GetUserName(HttpContext.User);
                obj.blog.ApplicationUserId = _applicationUser.GetUserId(HttpContext.User);

                //Adds blog post to Db context and saves changes.
                _context.Blog.Add(obj.blog);
                _context.SaveChanges();

                //Success message displayed and redirected to Index.
                TempData["success"] = "Blog created successfully";
                return RedirectToAction("Index");

            }
           
            return View(obj);
        }
        
        //GET - Method to Edit a blog.
        public async Task<IActionResult> Edit(int? id)
        {
            //Checks if Blog id or DbSet is null.
            if (id == null || _context.Blog == null)
            {
                //Returns 404 Not Found error message.
                return NotFound();
            }

            //Retrieves blog with specified id from the database.
            var blogObj = await _context.Blog.FindAsync(id);
            //Checks if retrieved blog is null. If so, redirects to Index.
            if (blogObj == null)
            {
                return RedirectToAction("Index");
            }

            //Initialises new instance of BlogVM.
            BlogVM obj = new()
            {
                //Sets blog with retrieved blog object.
                blog = blogObj,
                //For Category and Subcategory lists.
                CategoryList = new SelectList(_context.Category, "Id", "CategoryName"),
                SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName")

            };

            return View(obj);
        }

        // POST - Method to Edit a blog.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogVM obj, IFormFile? file)
        {
            //Populates CategoryList and SubCategoryList properties of BlogVM.
            obj.CategoryList = new SelectList(_context.Category, "Id", "CategoryName");
            obj.SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName");

            {
                //Retrieves root path of wwwroot folder.
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                //Checks if file uploaded.
                if (file != null)
                {
                    //Initialises new blog object.
                    Blog objBlog = new Blog();
                    //Generates new unique file name and defines where it will be saved.
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"blogImages/titleImages/");
                    //Gets file extension.
                    var extension = Path.GetExtension(file.FileName);

                    //Checks if blog already has an image.
                    if (obj.blog.TitleImageUrl != null)
                    {
                        //Constructs old image pathway and deletes it if it exists.
                        var oldImagePath = Path.Combine(wwwRootPath, obj.blog.TitleImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }
                    //Creates file stream and copies uploaded image to directory.
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }

                    //Updates these entities.
                    obj.blog.TitleImageUrl = @"\blogImages\titleImages\" + fileName + extension;
                    obj.blog.ModifiedBy = _applicationUser.GetUserId(HttpContext.User);
                    obj.blog.ModifiedDate = DateTime.Now;
                    obj.blog.ApplicationUserId = _applicationUser.GetUserId(HttpContext.User);
                }

                //Updates blog object in Db context and saves changes.
                _context.Update(obj.blog);
                await _context.SaveChangesAsync();

                //Success message displayed and redirection to Index.
                TempData["success"] = "Blog created successfully";
                return RedirectToAction("Index");

            }
            return View(obj);
        }

        // GET - Retrieves and displays details of selected blog to delete.
        public async Task<IActionResult> Delete(int? id)
        {
            //Checks if Blog id or Db set is null.
            if (id == null || _context.Blog == null)
            {
                //Returns 404 Not Found error message.
                return NotFound();
            }

            //Retrieves these details of the selected blog.
            var blog = await _context.Blog
                .Include(b => b.ApplicationUser)
                .Include(b => b.Category)
                .Include(b => b.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            //Checks if selected blog is null.
            if (blog == null)
            {
                //Returns 404 Not Found error message.
                return NotFound();
            }

            //Passes retrieved details to the Delete view.
            return View(blog);
        }

        // POST - Method to Delete a blog.

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Checks if Blog Db set is null. If so, returns an error.
            if (_context.Blog == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Blog'  is null.");
            }
            //Retrieves selected blog by id from the database.
            var blog = await _context.Blog.FindAsync(id);
            //Checks if selected blog exists.
            if (blog != null)
            {
                //Removes blog from the Db context.
                _context.Blog.Remove(blog);
            }
            
            //Saves changes to the database and redirects to Index.
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
