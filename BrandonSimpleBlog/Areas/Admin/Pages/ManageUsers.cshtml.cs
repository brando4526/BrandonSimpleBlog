using System.Collections.Generic;
using System.Threading.Tasks;
using BrandonSimpleBlog.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace BrandonSimpleBlog.Areas.Admin.Pages
{
    [Authorize(Roles = "Administrator")]
    public class ManageUsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public ManageUsersModel(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public IEnumerable<ApplicationUser> Users { get; set; }
        public string DefaultAvatarURL { get; set; }

        public string AvatarURLPrefix { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = _userManager.Users;
            DefaultAvatarURL = _configuration.GetSection("BlobService")["StorageURL"] + "images/avatar/default.jpg";
            AvatarURLPrefix = _configuration.GetSection("BlobService")["StorageURL"] + "images/avatar/";
            return Page();
            
        }
    }
}