using BlogWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.ViewComponents
{
    public class TopHeaderMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public TopHeaderMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = _context.Category.ToList();
            foreach (var item in categories)
            {
                item.SubCategories = _context.SubCategory.Where(s => s.CategoryId == item.Id).ToList();            }

            return await Task.FromResult((IViewComponentResult)View("TopMenu", categories));


        }
    }
}
