using System.Threading.Tasks;
using BrandonSimpleBlog.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrandonSimpleBlog.Areas.Admin.Pages
{
    public class ManageBlogPostsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlogRepository _blogRepo;

        public ManageBlogPostsModel(IBlogRepository blogRepository, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _blogRepo = blogRepository;
        }

        public BlogResult PostResults { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Resultpage { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (User.IsInRole("Administrator"))
            {
                PostResults = _blogRepo.GetPosts(false, 20, Resultpage > 0 ? Resultpage : 1);
                
            }
            else
            {
                PostResults = _blogRepo.GetPostsByAuthor(user.Id, false, 20, Resultpage > 0 ? Resultpage : 1);
            }
            
        }
    }
}