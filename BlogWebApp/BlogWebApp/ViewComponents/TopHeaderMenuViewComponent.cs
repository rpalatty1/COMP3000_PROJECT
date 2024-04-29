using BlogWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.ViewComponents
{
    public class TopHeaderMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        //Constructor to inject dependency.
        public TopHeaderMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        //Method to call view component.
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Retrieves all categories from database.
            var categories = _context.Category.ToList();
            //Loops through each category to retrieve itssubcategories.
            foreach (var item in categories)
            {
                //Retrieves and assigns subcategories for category.
                item.SubCategories = _context.SubCategory.Where(s => s.CategoryId == item.Id).ToList();            }

            //Returns view dropdown with category data.
            return await Task.FromResult((IViewComponentResult)View("TopMenu", categories));


        }
    }
}
