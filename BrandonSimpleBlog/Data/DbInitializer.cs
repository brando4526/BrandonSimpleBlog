using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace BrandonSimpleBlog.Data
{
    public class DbInitializer
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userMgr;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userMgr)
        {
            _context = context;
            _userMgr = userMgr;
        }

        public void SeedData()
        {
            _context.Database.EnsureCreated();

            if (!_context.Roles.Any())
            {
                _context.Roles.Add(new IdentityRole() { Name = "Administrator", NormalizedName = "ADMINISTRATOR" });
                _context.Roles.Add(new IdentityRole() { Name = "Guest", NormalizedName = "GUEST" });
        
                _context.SaveChangesAsync().Wait();
            }

            if (_context.Users.Any())
            {
                return;   // users have already been seeded
            }

            var user = new ApplicationUser()
            {
                Email = "admin@admin.com",
                UserName = "admin",
                EmailConfirmed = true,
                LastName="Admin",
                FirstName="Brandon",
                HasAvatarImage=false
                
                
            };

            _userMgr.CreateAsync(user, "Skittles123!").Wait(); // Temp Password

            _userMgr.AddToRoleAsync(user, "Administrator").Wait();

            _context.SaveChangesAsync().Wait();

            //adding sample blog posts
            var blogPostSample1 = new BlogPost()
            {
                AuthorId=user.Id,
                DatePublished = DateTime.Now,
                Title = "Sample Blog 1",
                Excerpt="This is a sample blog post only meant for initial database creation.",
                Content="This is the content of this sample blog post. HTML content will be placed here. The quick Brown Fox jumped yada yada yada.",
                IsPublished = true,
                Categories = "Sample",
                Slug = "sample-post-1",
                IsFeatured=true
            };

            var blogPostSample2 = new BlogPost()
            {
                AuthorId = user.Id,
                DatePublished = DateTime.Now.AddDays(1),
                Title = "Sample Blog 2",
                Excerpt = "This is a second sample blog post only meant for initial database creation.",
                Content = "This is the content of this second sample blog post. HTML content will be placed here. The quick Brown Fox jumped yada yada yada.",
                IsPublished = true,
                Categories = "Sample,Sample2",
                Slug = "sample-post-2",
                IsFeatured = true
            };

            var blogPostSample3 = new BlogPost()
            {
                AuthorId = user.Id,
                DatePublished = DateTime.Now.AddDays(2),
                Title = "Sample Blog 3",
                Excerpt = "This is a third sample blog post only meant for initial database creation.",
                Content = "This is the content of this third sample blog post. HTML content will be placed here. The quick Brown Fox jumped yada yada yada.",
                IsPublished = true,
                Categories = "Sample,Sample3",
                Slug = "sample-post-3",
                IsFeatured = false
            };

            _context.BlogPosts.Add(blogPostSample1);
            _context.BlogPosts.Add(blogPostSample2);
            _context.BlogPosts.Add(blogPostSample3);
            _context.SaveChanges();

        }
    }
}
