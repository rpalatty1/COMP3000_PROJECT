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
            public DbSet<BlogPost> BlogPosts { get; set; }
            public DbSet<Tag> Tags { get; set; }
            public DbSet<AdminBlogPost> AdminBlogPosts { get; set; }


    }
}