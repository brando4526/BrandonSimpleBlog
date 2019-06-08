using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrandonSimpleBlog.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrandonSimpleBlog.Areas.Admin.Pages
{
    public class EditBlogPostModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        

        public void OnGet()
        {
        }
    }
}