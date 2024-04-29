using BlogWebApp.Data;
using BlogWebApp.Data.Migrations;
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

        //Constructor to inject dependencies.
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //Action method for Index to display list of blogs. 
        public async Task<IActionResult> Index( string searchString)
        {
            //Sets current value in ViewData dictionary.
            ViewData["CurrentFilter"] = searchString;
            //Retrieves all blogs from database including these entities.
            IEnumerable<Blog> objBlogList = _context.Blog.Include(b => b.ApplicationUser).Include(b => b.Category).Include(b => b.SubCategory);

            //If search string is provided, filter blog list based on title or tags.
            if (!String.IsNullOrEmpty(searchString))
            {
                objBlogList = objBlogList.Where(b => b.Title.Contains(searchString)
                                       || b.Tags.Contains(searchString));
            }
            
            //Pass filtered blogs in list to view.
            return View(objBlogList);
        }

        //Method to retrieve and display list of blogs depending on the Category.
        public IActionResult CategoryWiseBlog(string? categoryName)
        {
            //Retrieve all blogs from the database, including these entities.
            IEnumerable<Blog> objBlogList = _context.Blog.Include(b => b.ApplicationUser).Include(b => b.Category).Include(b => b.SubCategory);
            //To filter blog list based on the selected category.
            IEnumerable<Blog> catBlogList = objBlogList.Where(b => b.Category.CategoryName == categoryName).ToList(); 

            //Passes filtered blog list into view.
            return View(catBlogList);
        }

        //Method to retrieve and select a blog.
        public IActionResult TitleWiseBlog(string? categoryName, string? title)
        {
            //Retrieves selected blog from the database including these entities.
            IEnumerable<Blog> objBlogList = _context.Blog.Include(b => b.ApplicationUser).Include(b => b.Category).Include(b => b.SubCategory).Include(b => b.MainComments);
            //Finds blog with selected blog title.
            Blog blog = objBlogList.FirstOrDefault(b => b.Slug == title);
            //Retrieves and displays main comments with their respective subcomments.
            foreach (MainComment m in blog.MainComments)
            {
                m.SubComments = _context.SubComment.Where(s => s.MainCommentId == m.Id).ToList();
            }

            //Passes selected blog with its associated comments to view.
            return View(blog);

        }

        //Displayes privacy page.
        public IActionResult Privacy()
        {
            return View();
        }

        //Displays error page when error ocurs.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //Passes ErrorViewModel containing request ID to Error View.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Method to add comments to blog posts.
        [HttpPost]
        public async Task<IActionResult> Comment(CommentVM vm)
        {
            //Retrieves blog post corresponding to the comment.
            Blog post = await _context.Blog.FirstOrDefaultAsync(m => m.Id == vm.PostId);
            //Checks if comment is valid.
            if (!ModelState.IsValid)
                //If valid, redirects to selected blog post page.
                return RedirectToAction("TitleWiseBlog", new { categoryName = post.Category, title = post.Slug });

            //Adds main comment.
            if (vm.MainCommentId == 0)
            {
                //Initialises main comment.
                post.MainComments = post.MainComments ?? new List<MainComment>();

                //Adds new main comment to list.
                post.MainComments.Add(new MainComment
                {
                    Message = vm.Message,
                    Created = DateTime.Now,
                });

                //Updates comment to blog post in database.
                _context.Update(post);
                
            }
            else
            {
                //To add a subcomment.
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Message,
                    Created = DateTime.Now,
                };
                _context.SubComment.Add(comment);
            }

            //Saves changes to the database.
            await _context.SaveChangesAsync();

            //Redirects to selected blog post to show added comment or subcomment.
            return RedirectToAction("TitleWiseBlog", new { categoryName = post.Category, title = post.Slug });
        }


    }
}