using BlogWebApp.Data;
using BlogWebApp.Models;
using BlogWebApp.Models.Comments;
using BlogWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlogWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Blog> objBlogList = _context.Blog.Include(b => b.ApplicationUser).Include(b => b.Category).Include(b => b.SubCategory);
            return View(objBlogList);
        }

        public IActionResult CategoryWiseBlog(string? categoryName)
        {
            IEnumerable<Blog> objBlogList = _context.Blog.Include(b => b.ApplicationUser).Include(b => b.Category).Where(b => b.Category.CategoryName == categoryName).Include(b => b.SubCategory);
            return View(objBlogList);
        }

        public IActionResult TitleWiseBlog(string? categoryName, string? title)
        {
            IEnumerable<Blog> objBlogList = _context.Blog.Include(b => b.ApplicationUser).Include(b => b.Category).Include(b => b.SubCategory).Include(b => b.MainComments);
            Blog blog = objBlogList.FirstOrDefault(b => b.Slug == title);
            foreach (MainComment m in blog.MainComments)
            {
                m.SubComments = _context.SubComment.Where(s => s.MainCommentId == m.Id).ToList();
            }
            return View(blog);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //comment section
        [HttpPost]
        public async Task<IActionResult> Comment(CommentVM vm)
        {
            Blog post = await _context.Blog.FirstOrDefaultAsync(m => m.Id == vm.PostId);
            if (!ModelState.IsValid)
                return RedirectToAction("TitleWiseBlog", new { categoryName = post.Category, title = post.Slug });


            if (vm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = vm.Message,
                    Created = DateTime.Now,
                });

                //_context.Blog.SaveChange(post);
                _context.Update(post);
                
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Message,
                    Created = DateTime.Now,
                };
                _context.SubComment.Add(comment);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("TitleWiseBlog", new { categoryName = post.Category, title = post.Slug });
        }


    }
}