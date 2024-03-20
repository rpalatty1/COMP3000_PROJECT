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

        public BlogImagesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> applicationUser)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _applicationUser = applicationUser;

        }

        // GET: Admin/BlogImages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BlogImages.Include(b => b.subCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/BlogImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BlogImages == null)
            {
                return NotFound();
            }

            var blogImages = await _context.BlogImages
                .Include(b => b.subCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogImages == null)
            {
                return NotFound();
            }

            return View(blogImages);
        }

        // GET: Admin/BlogImages/Create
        public IActionResult Create()
        {
            //ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "SubCategoryName");
            //return View();

            BlogImagesVM obj = new()
            {
                BlogImages = new(),
                SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName")
                //{
                //        Text = i.subCategory.ToString(),
                //        Value = i.Id.ToString()

                //    } )

            };
            return View(obj);
        }

        // POST: Admin/BlogImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(BlogImagesVM obj, List<IFormFile>? files)
        {
            List<BlogImages> objList = new List<BlogImages>();

            obj.SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName");

            //if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        BlogImages objSingle = new BlogImages();
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"blogImages/images/");
                        var extension = Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        obj.BlogImages.ImageUrl = @"\blogImages\images\" + fileName + extension;
                        obj.BlogImages.CreatedBy = _applicationUser.GetUserId(HttpContext.User);
                        objSingle.BlogTitle = obj.BlogImages.BlogTitle;
                        objSingle.ImageUrl = obj.BlogImages.ImageUrl;
                        objSingle.SubCategoryId = obj.BlogImages.SubCategoryId;
                        objSingle.CreatedBy = obj.BlogImages.CreatedBy;
                        objList.Add(objSingle);
                    }

                    _context.BlogImages.AddRange(objList);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Blog Images uploaded successfully!";
                    return RedirectToAction("Index");
                }
            }

            return View(obj);
        }

        //public async Task<IActionResult> Create([Bind("BlogTitle,ImageUrl,SubCategoryId,Id,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] BlogImages blogImages)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(blogImages);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "SubCategoryName", blogImages.SubCategoryId);
        //    return View(blogImages);
        //}

        // GET: Admin/BlogImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BlogImages == null)
            {
                return NotFound();
            }

            var blogImages = await _context.BlogImages.FindAsync(id);
            if (blogImages == null)
            {
                return RedirectToAction("Index");
            }

            BlogImagesVM objVM = new()
            {
                BlogImages = blogImages,
                SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName")
            };

            return View(objVM);
         }

            //ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "SubCategoryName", blogImages.SubCategoryId);
            //return View(blogImages);
     //}

        // POST: Admin/BlogImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(BlogImagesVM obj, List<IFormFile>? files)
        {
            List<BlogImages> objList = new List<BlogImages>();

            obj.SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName");

            //if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        BlogImages objSingle = new BlogImages();
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"blogImages/images/");
                        var extension = Path.GetExtension(file.FileName);

                        if (obj.BlogImages.ImageUrl !=null)
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, obj.BlogImages.ImageUrl.TrimStart('\\'));
                            if(System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }

                        }

                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        
                        obj.BlogImages.ImageUrl = @"\blogImages\images\" + fileName + extension;
                        obj.BlogImages.ModifiedBy = _applicationUser.GetUserId(HttpContext.User);
                        obj.BlogImages.ModifiedDate = DateTime.Now;

                        //objSingle.BlogTitle = obj.BlogImages.BlogTitle;
                        //objSingle.ImageUrl = obj.BlogImages.ImageUrl;
                        //objSingle.SubCategoryId = obj.BlogImages.SubCategoryId;
                        //objSingle.CreatedBy = obj.BlogImages.CreatedBy;
                        //objList.Add(objSingle);
                    }

                    _context.BlogImages.Update(obj.BlogImages);
                    _context.SaveChanges();
                    //await _context.SaveChangesAsync();
                    TempData["success"] = "Blog Images updated successfully!";
                    return RedirectToAction("Index");
                }
            }

            return View(obj);
        }
        //public async Task<IActionResult> Edit(int id, [Bind("BlogTitle,ImageUrl,SubCategoryId,Id,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] BlogImages blogImages)
        //{
        //    if (id != blogImages.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(blogImages);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BlogImagesExists(blogImages.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "SubCategoryName", blogImages.SubCategoryId);
        //    return View(blogImages);
        //}

        // GET: Admin/BlogImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BlogImages == null)
            {
                return NotFound();
            }

            var blogImages = await _context.BlogImages
                .Include(b => b.subCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogImages == null)
            {
                return NotFound();
            }

            return View(blogImages);
        }

        // POST: Admin/BlogImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BlogImages == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BlogImages'  is null.");
            }
            var blogImages = await _context.BlogImages.FindAsync(id);
            if (blogImages != null)
            {
                _context.BlogImages.Remove(blogImages);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogImagesExists(int id)
        {
          return (_context.BlogImages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
