using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogWebApp.Data;
using BlogWebApp.Models;

namespace BlogWebApp.Controllers
{
    public class AdminBlogPostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminBlogPostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminBlogPost
        public async Task<IActionResult> Index()
        {
              return _context.AdminBlogPosts != null ? 
                          View(await _context.AdminBlogPosts.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.AdminBlogPosts'  is null.");
        }

        // GET: AdminBlogPost/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AdminBlogPosts == null)
            {
                return NotFound();
            }

            var adminBlogPost = await _context.AdminBlogPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminBlogPost == null)
            {
                return NotFound();
            }

            return View(adminBlogPost);
        }

        // GET: AdminBlogPost/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminBlogPost/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Heading,PageTitle,Content,ShortDescription,FeaturedImageUrl,UrlHandle,PublishedDate,Author,Visible")] AdminBlogPost adminBlogPost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminBlogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminBlogPost);
        }

        // GET: AdminBlogPost/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AdminBlogPosts == null)
            {
                return NotFound();
            }

            var adminBlogPost = await _context.AdminBlogPosts.FindAsync(id);
            if (adminBlogPost == null)
            {
                return NotFound();
            }
            return View(adminBlogPost);
        }

        // POST: AdminBlogPost/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Heading,PageTitle,Content,ShortDescription,FeaturedImageUrl,UrlHandle,PublishedDate,Author,Visible")] AdminBlogPost adminBlogPost)
        {
            if (id != adminBlogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminBlogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminBlogPostExists(adminBlogPost.Id))
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
            return View(adminBlogPost);
        }

        // GET: AdminBlogPost/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AdminBlogPosts == null)
            {
                return NotFound();
            }

            var adminBlogPost = await _context.AdminBlogPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminBlogPost == null)
            {
                return NotFound();
            }

            return View(adminBlogPost);
        }

        // POST: AdminBlogPost/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AdminBlogPosts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AdminBlogPosts'  is null.");
            }
            var adminBlogPost = await _context.AdminBlogPosts.FindAsync(id);
            if (adminBlogPost != null)
            {
                _context.AdminBlogPosts.Remove(adminBlogPost);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminBlogPostExists(int id)
        {
          return (_context.AdminBlogPosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
