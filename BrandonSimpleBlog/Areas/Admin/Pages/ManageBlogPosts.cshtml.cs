using System;
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

        public class AjaxResult
        {
            public bool Success { get; set; }
            public string ResponseText { get; set; }
        }

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


        public JsonResult OnPostSetPublishStatus(int postId, bool published)
        {
            bool successStatus = false;
            if (User.IsInRole("Administrator"))
            {
                var post=_blogRepo.GetPost(postId);
                post.IsPublished = published;
                post.DatePublished = DateTime.Now;
                successStatus= _blogRepo.UpdatePost(post);
               
            }
            if (!successStatus)
            {
                return new JsonResult(new { Success = false, ResponseText = "Publish status not updated." });
            }
            else
            {
                return new JsonResult(new { Success = true, ResponseText = "Publish status updated." });
            }

        }

        public JsonResult OnPostSetFeaturedStatus(int postId, bool featured)
        {
            bool successStatus = false;
            if (User.IsInRole("Administrator"))
            {
                var post = _blogRepo.GetPost(postId);
                post.IsFeatured = featured;
                successStatus = _blogRepo.UpdatePost(post);

            }
            if (!successStatus)
            {
                return new JsonResult(new { Success = false, ResponseText = "Error:Featured status not updated." });
            }
            else
            {
                return new JsonResult(new { Success = true, ResponseText = "Featured status updated." });
            }

        }
    }
}