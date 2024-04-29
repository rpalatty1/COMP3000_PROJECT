using BlogWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlogWebApp.Models.ViewModels;
using BlogWebApp.Models.Comments;

namespace BlogWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

            public DbSet<Category> Category { get; set; }
            public DbSet<SubCategory> SubCategory { get; set; }
            public DbSet<BlogImages> BlogImages { get; set; }
            public DbSet<Blog> Blog { get; set; }
            public DbSet<MainComment> MainComment { get; set; }
            public DbSet<SubComment> SubComment { get; set; }


    }
}