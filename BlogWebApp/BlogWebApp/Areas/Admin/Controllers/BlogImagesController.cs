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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static System.Net.Mime.MediaTypeNames;
using BlogWebApp.Data.Migrations;
using Microsoft.AspNetCore.Identity;

namespace BlogWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _applicationUser;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //Constructor to inject dependencies.
        public BlogImagesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> applicationUser)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _applicationUser = applicationUser;

        }

        // GET - Method to retrieve and display the list of blogs images.
        public async Task<IActionResult> Index()
        {
            //Retrieves blogs from the database relating to this entity.
            var applicationDbContext = _context.BlogImages.Include(b => b.subCategory);
            //Returns list of blog images.
            return View(await applicationDbContext.ToListAsync());
        }

        // GET - Method to retrieve and display details of the selected blog image.
        public async Task<IActionResult> Details(int? id)
        {
            //Checks if id or Db context is null. If so returns error message.
            if (id == null || _context.BlogImages == null)
            {
                return NotFound();
            }

            //Retrieves information of the selected blog image using these entities.
            var blogImages = await _context.BlogImages
                .Include(b => b.subCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            //Checks if selected blog image is null. If so, returns error.
            if (blogImages == null)
            {
                return NotFound();
            }

            //Retrieves and displays details of selected blog images.
            return View(blogImages);
        }

        // GET - action method to Create a new blog image.
        public IActionResult Create()
        {
            //Initialises a new instance of BlogVM.
            BlogImagesVM obj = new()
            {
                //Initialises new blog image instance.
                BlogImages = new(),
                //Creates a subcategory list containing SubCategory Id and SubCategoryName.
                SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName")

            };
            //Passes the initialised BlogVM object to the Create view.
            return View(obj);
        }

        // POST - Action method to upload the added blog image.

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(BlogImagesVM obj, List<IFormFile>? files)
        {
            //Initialises list to store BlogImages object.
            List<BlogImages> objList = new List<BlogImages>();

            //Populates SubCategoryList properties of BlogVM.
            obj.SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName");

            {
                //Retrieves root path of wwwroot.
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                //Checks if file uploaded.
                if (files != null && files.Count > 0)
                {
                    //Iterates through uploaded files.
                    foreach (var file in files)
                    {
                        //Initialises new BlogImages instance.
                        BlogImages objSingle = new BlogImages();
                        //Generates unique file name and defines where it will be saved.
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"blogImages/images/");
                        //Gets file extension.
                        var extension = Path.GetExtension(file.FileName);
                        //Creates file stream and copies upoaded file to that directory.
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        //Sets these properties foe the BlogImages object.
                        obj.BlogImages.ImageUrl = @"\blogImages\images\" + fileName + extension;
                        obj.BlogImages.CreatedBy = _applicationUser.GetUserId(HttpContext.User);
                        objSingle.BlogTitle = obj.BlogImages.BlogTitle;
                        objSingle.ImageUrl = obj.BlogImages.ImageUrl;
                        objSingle.SubCategoryId = obj.BlogImages.SubCategoryId;
                        objSingle.CreatedBy = obj.BlogImages.CreatedBy;
                        //Adds BlogImages to the list.
                        objList.Add(objSingle);
                    }

                    //Adds list of BlogImage objects to database and saves changes.
                    _context.BlogImages.AddRange(objList);
                    await _context.SaveChangesAsync();
                    //Success message displayed and redirected to Index.
                    TempData["success"] = "Blog Images uploaded successfully!";
                    return RedirectToAction("Index");
                }
            }
            //If no files uploaded, return view.
            return View(obj);
        }

        //Method to Edit blog images.
        public async Task<IActionResult> Edit(int? id)
        {
            //Checks if blog image id or DbSet is null. If so, returns error message.
            if (id == null || _context.BlogImages == null)
            {
                return NotFound();
            }

            //Retrieves selected blog image by its id.
            var blogImages = await _context.BlogImages.FindAsync(id);
            //Check if image is blog image is null. If so, redirect to Index.
            if (blogImages == null)
            {
                return RedirectToAction("Index");
            }

            //Initialises new instance of BlogImagesVM.
            BlogImagesVM objVM = new()
            {
                //Sets 'BlogImages' property with retrieved blog image object.
                BlogImages = blogImages,
                //For subcategory list containing SubCategory Id and SubCategoryName.
                SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName")
            };
            
            //Passes initialised BlogImagesVM object to the Edit view.
            return View(objVM);
         }


        // POST - Method to Edit blog images. 

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(BlogImagesVM obj, List<IFormFile>? files)
        {
            //Initialises list to store BlogImages objects.
            List<BlogImages> objList = new List<BlogImages>();
            //Populates SubCategoryList with this data. 
            obj.SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName");

            {
                //Retrieves root path.
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                //Check if files uploaded.
                if (files != null && files.Count > 0)
                {
                    //Iterates through each uploaded image.
                    foreach (var file in files)
                    {
                        //Initialises new object.
                        BlogImages objSingle = new BlogImages();
                        //Created unique file name and defines where it will be saved.
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"blogImages/images/");
                        //Gets file extension.
                        var extension = Path.GetExtension(file.FileName);

                        //Check if blog image already has an image.
                        if (obj.BlogImages.ImageUrl !=null)
                        {
                            //Constructs old image path and deletes pre-existing one.
                            var oldImagePath = Path.Combine(wwwRootPath, obj.BlogImages.ImageUrl.TrimStart('\\'));
                            if(System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }

                        }

                        //Creates file stream and copies uploaded file to defined directory.
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        //Updates changes ade to these properties.
                        obj.BlogImages.ImageUrl = @"\blogImages\images\" + fileName + extension;
                        obj.BlogImages.ModifiedBy = _applicationUser.GetUserId(HttpContext.User);
                        obj.BlogImages.ModifiedDate = DateTime.Now;

                    }

                    //Updates to database context and saves changes to database.
                    _context.BlogImages.Update(obj.BlogImages);
                    _context.SaveChanges();
                    
                    //Success message displayes and redirected to Index.
                    TempData["success"] = "Blog Images updated successfully!";
                    return RedirectToAction("Index");
                }
            }
            //Returns view if no images uploaded.
            return View(obj);
        }

        // GET - Method to Delete a blog image.
        public async Task<IActionResult> Delete(int? id)
        {
            //Checks if image id or DbSet is null. If so, returns error message.
            if (id == null || _context.BlogImages == null)
            {
                return NotFound();
            }

            //Retrieves image by id, realted to subcategory.
            var blogImages = await _context.BlogImages
                .Include(b => b.subCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            //Checks if retrieved image is null. If so, returns error message.
            if (blogImages == null)
            {
                return NotFound();
            }

            //Passes retrieved image details to Delete view.
            return View(blogImages);
        }

        // POST - Method to Delete a blog image.

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Checks if BlogImages DbSet is null. If so, returns problem response.
            if (_context.BlogImages == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BlogImages'  is null.");
            }
            //Retrieves selected blog image path from database by id.
            var blogImages = await _context.BlogImages.FindAsync(id);
            //Checks if image exists.
            if (blogImages != null)
            {
                //Removes image from Db context.
                _context.BlogImages.Remove(blogImages);
            }
            
            //Saves changes to database and redirects to Index.
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Helper method to check if blog image with specified id exists and return true/false.
        private bool BlogImagesExists(int id)
        {
          return (_context.BlogImages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
