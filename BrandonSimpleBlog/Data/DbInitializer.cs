using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrandonSimpleBlog.Data
{
    public class DbInitializer
    {
        private ApplicationDbContext _ctx;
        private UserManager<ApplicationUser> _userMgr;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userMgr)
        {
            _ctx = context;
            _userMgr = userMgr;
        }

        public void SeedData()
        {
            _ctx.Database.EnsureCreated();

            if (_ctx.Users.Any())
            {
                return;   // users have already been seeded
            }

            var user = new ApplicationUser()
            {
                Email = "admin@admin.com",
                UserName = "admin",
                EmailConfirmed = true,
                LastName="Admin",
                FirstName="Brandon"
                
            };

            _userMgr.CreateAsync(user, "Skittles123!").Wait(); // Temp Password

            _ctx.SaveChangesAsync().Wait();
        }
    }
}
