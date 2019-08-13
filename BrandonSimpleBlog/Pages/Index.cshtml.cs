using System.Collections.Generic;
using BrandonSimpleBlog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrandonSimpleBlog.Pages
{

    public class IndexModel : PageModel
    {
        private readonly IBlogRepository _blogRepo;

        public IndexModel(IBlogRepository blogRepository)
        {
            _blogRepo = blogRepository;
        }
        public IEnumerable<CategoryEntry> Categories { get; set; }
        public IEnumerable<BlogPostDescription> FeaturedPosts { get; set; }
        public BlogResult Posts { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Resultpage { get; set; }
        public void OnGet()
        {
            FeaturedPosts = _blogRepo.GetFeaturedPosts();
            Categories = _blogRepo.GetCategories(true);
            Posts = _blogRepo.GetPosts(true,10, Resultpage > 0 ? Resultpage : 1);
                 
        }
    }
}