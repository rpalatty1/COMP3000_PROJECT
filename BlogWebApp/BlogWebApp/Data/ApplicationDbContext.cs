using BlogWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        //public DbSet<Customer> Customers { get; set; }
        //public DbSet<Professional> Professionals { get; set; }
        //public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<Account>(entity => { entity.ToTable("Accounts"); });
            //builder.Entity<Customer>(entity => { entity.ToTable("Customers"); });
            //builder.Entity<Professional>(entity => { entity.ToTable("Professionals"); });
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
            public DbSet<AdminBlogPost> AdminBlogPosts { get; set; }
            public DbSet<Category> Category { get; set; }
            public DbSet<SubCategory> SubCategory { get; set; }
            public DbSet<BlogImages> BlogImages { get; set; }
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<BlogPost> BlogPost { get; set; }


    }
}