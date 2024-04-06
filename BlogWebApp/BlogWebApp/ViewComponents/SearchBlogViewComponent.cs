//using BlogWebApp.Data;
//using Microsoft.AspNetCore.Mvc;

//namespace BlogWebApp.ViewComponents
//{
//    public class SearchBlogViewComponent : ViewComponent
//    {
//        private readonly ApplicationDbContext _context;

//        public SearchBlogViewComponent(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        //public async Task<IViewComponentResult> InvokeAsync()
//        //{
//        //    var blogs = _context.Blog.ToList();
//        //    foreach (var item in blogs)
//        //    {
//        //        item.SubCategories = _context.SubCategory.Where(s => s.CategoryId == item.Id).ToList();
//        //    }

//        //    return await Task.FromResult((IViewComponentResult)View("TopMenu", blogs));


//        //}
//    }
//}

