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

namespace BlogWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _applicationUser;

        public BlogController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> applicationUser)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _applicationUser = applicationUser;

        }

        // GET: Admin/Blog
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Blog.Include(b => b.ApplicationUser).Include(b => b.Category).Include(b => b.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog
                .Include(b => b.ApplicationUser)
                .Include(b => b.Category)
                .Include(b => b.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        //GET: Admin/Blog/GetSubCategoryList
        //public IActionResult GetSubCategoryList(int Id)
        //{
        //    var subCategoryList = _context.SubCategory.GetAll().Where(u => u.CategoryId == Id).ToList();
        //    return Json(SubCategoryList);
        //}

        // GET: Admin/Blog/Create
        public IActionResult Create()
        {
            BlogVM obj = new()
            {
                blog = new(),
                CategoryList = new SelectList(_context.Category, "Id", "CategoryName"),
                SubCategoryList = new SelectList(_context.SubCategory, "Id", "SubCategoryName")
                //{
                //        Text = i.subCategory.ToString(),
                //        Value = i.Id.ToString()

                //    } )

            };
            //obj.blog.ApplicationUserId = _applicationUser
            //ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            //ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id");
            //ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "Id");
            return View(obj);
        }

        // POST: Admin/Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Slug,Tags,Description,Content,TitleImageUrl,ApplicationUserId,CategoryId,SubCategoryId,IsActive,Id,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", blog.ApplicationUserId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", blog.CategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "Id", blog.SubCategoryId);
            return View(blog);
        }

        // GET: Admin/Blog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", blog.ApplicationUserId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", blog.CategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "Id", blog.SubCategoryId);
            return View(blog);
        }

        // POST: Admin/Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Slug,Tags,Description,Content,TitleImageUrl,ApplicationUserId,CategoryId,SubCategoryId,IsActive,Id,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", blog.ApplicationUserId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id", blog.CategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "Id", blog.SubCategoryId);
            return View(blog);
        }

        // GET: Admin/Blog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog
                .Include(b => b.ApplicationUser)
                .Include(b => b.Category)
                .Include(b => b.SubCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Blog == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Blog'  is null.");
            }
            var blog = await _context.Blog.FindAsync(id);
            if (blog != null)
            {
                _context.Blog.Remove(blog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
          return (_context.Blog?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
