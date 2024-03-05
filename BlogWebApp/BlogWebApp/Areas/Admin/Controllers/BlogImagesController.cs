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

namespace BlogWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogImagesController(ApplicationDbContext context)
        {
            _context = context;
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
                SubCategoryList = _context.BlogImages.Include(b => b.subCategory).Select(i => new SelectListItem
            {
                    Text = i.subCategory.ToString(),
                    Value = i.Id.ToString()

                } )

            };  
            return View(obj);
        }

        // POST: Admin/BlogImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogTitle,ImageUrl,SubCategoryId,Id,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] BlogImages blogImages)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogImages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "SubCategoryName", blogImages.SubCategoryId);
            return View(blogImages);
        }

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
                return NotFound();
            }
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "SubCategoryName", blogImages.SubCategoryId);
            return View(blogImages);
        }

        // POST: Admin/BlogImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogTitle,ImageUrl,SubCategoryId,Id,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] BlogImages blogImages)
        {
            if (id != blogImages.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogImages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogImagesExists(blogImages.Id))
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
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "SubCategoryName", blogImages.SubCategoryId);
            return View(blogImages);
        }

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
