using BlogWebApp.Data;
using BlogWebApp.Models;
using BlogWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.Controllers
{
    public class AdminTagsController : Controller
    {
        private ApplicationDbContext _applicationDbContext;
        public AdminTagsController (ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddTagRequest addTagRequest) 
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            _applicationDbContext.Tags.Add(tag);
            _applicationDbContext.SaveChanges();
            return View("Add");
        }

        // GET: AdminTagsController
        public async Task<IActionResult> Index()
        {
            return _applicationDbContext.Tags != null ?
                        View(await _applicationDbContext.Tags.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Tags'  is null.");
        }

        // GET: AdminTags/Details
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _applicationDbContext.Tags == null)
            {
                return NotFound();
            }

            var tag = await _applicationDbContext.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: AdminTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminTags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Guid Id, Name, DisplayName")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                tag.Id = Guid.NewGuid();
                _applicationDbContext.Add(tag);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: AdminTags/Edit
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _applicationDbContext.Tags == null)
            {
                return NotFound();
            }

            var tag = await _applicationDbContext.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: AdminTags/Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,DisplayName")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _applicationDbContext.Update(tag);
                    await _applicationDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.Id))
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
            return View(tag);

        }

        // GET: AdminTags/Delete
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _applicationDbContext.Tags == null)
            {
                return NotFound();
            }

            var tag = await _applicationDbContext.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: AdminTags/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_applicationDbContext.Tags == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tags'  is null.");
            }
            var tag = await _applicationDbContext.Tags.FindAsync(id);
            if (tag != null)
            {
                _applicationDbContext.Tags.Remove(tag);
            }

            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TagExists(Guid id)
        {
            return (_applicationDbContext.Tags?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
