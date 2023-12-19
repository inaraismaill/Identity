using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewPustok.Models;
using System.Drawing;

namespace NewPustok.Contexts
{
    public class NewPustokDbContext: IdentityDbContext
    {
        public NewPustokDbContext(DbContextOptions opt) : base(opt) { }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Settings>()
                .HasData(new Settings
                {
                    Address = "Azmiu Otaq 111",
                    Email = "Inara@Code.edu.az",
                    Number = "+994555555555",
                    Logo = "/image/logo.png",
                    Icon = "<i class='fa fa-user-o'></i>",
                    Id = 1
                });
            base.OnModelCreating(modelBuilder);
        }

    }
}
 