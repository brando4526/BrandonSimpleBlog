using BrandonSimpleBlog.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;

namespace BrandonSimpleBlog.Data
{
    public class AppDataInitializer
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userMgr;
        private IMediaStorageService _mediaStorage;

        public AppDataInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userMgr, IMediaStorageService mediaStorage)
        {
            _context = context;
            _userMgr = userMgr;
            _mediaStorage = mediaStorage;
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
                DateCreated=DateTime.Now,
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
                DateCreated=DateTime.Now.AddDays(1),
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
                DateCreated=DateTime.Now.AddDays(2),
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


            for(int i = 5; i < 88;i++)
            {
                var blogPostSampleloop = new BlogPost()
                {
                    AuthorId = user.Id,
                    DatePublished = DateTime.Now.AddDays(i),
                    DateCreated= DateTime.Now.AddDays(i),
                    Title = "Sample Blog "+i,
                    Excerpt = "This is a third sample blog post only meant for initial database creation.",
                    Content = "This is the content of this third sample blog post. HTML content will be placed here. The quick Brown Fox jumped yada yada yada.",
                    IsPublished = true,
                    Categories = "Sample,Sample2",
                    Slug = "sample-post-"+i,
                    IsFeatured = false
                };
                _context.BlogPosts.Add(blogPostSampleloop);
            }

            _context.SaveChanges();


            //seed placeholder images for blog posts
            IFileProvider physicalProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
            IDirectoryContents contents = physicalProvider.GetDirectoryContents("Data/SeedMedia/BlogPost");
            foreach (var file in contents)
            {
                MemoryStream ms = new MemoryStream();
                file.CreateReadStream().CopyTo(ms);
                _mediaStorage.SaveImageToStorage(ms.ToArray(), "post/" + file.Name);
            }
            //seed placeholder images for avatar images
            contents = physicalProvider.GetDirectoryContents("Data/SeedMedia/Avatar");
            foreach (var file in contents)
            {
                MemoryStream ms = new MemoryStream();
                file.CreateReadStream().CopyTo(ms);
                _mediaStorage.SaveImageToStorage(ms.ToArray(), "avatar/" + file.Name);
            }
            //seed placeholder images for profile images
            contents = physicalProvider.GetDirectoryContents("Data/SeedMedia/Profile");
            foreach (var file in contents)
            {
                MemoryStream ms = new MemoryStream();
                file.CreateReadStream().CopyTo(ms);
                _mediaStorage.SaveImageToStorage(ms.ToArray(), "profile/" + file.Name);
            }


        }
    }
}
