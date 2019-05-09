using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        }
    }
}
