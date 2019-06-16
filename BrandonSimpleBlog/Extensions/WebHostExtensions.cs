using BrandonSimpleBlog.Data;
using BrandonSimpleBlog.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BrandonSimpleBlog.Extensions
{
    public static class WebHostExtensions
    {
        public static IWebHost SeedData(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                try
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetService<ApplicationDbContext>();
                    var userService = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var storageService = services.GetService<IMediaStorageService>();


                    // now we have the DbContext. Run migrations
                    //context.Database.Migrate();

                    // now that the database is up to date. Let's seed
                    new AppDataInitializer(context, userService,storageService).SeedData();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            return webHost;
        }
    }
}
