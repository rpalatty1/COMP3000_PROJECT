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
        /*public async Task<IActionResult> Index()
        {
            return _applicationDbContext.Tags != null ?
                        View(await _applicationDbContext.Tags.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Tags'  is null.");
        }*/
    }
}
