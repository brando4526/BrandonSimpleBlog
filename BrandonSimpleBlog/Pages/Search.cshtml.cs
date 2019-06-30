using BrandonSimpleBlog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrandonSimpleBlog.Pages
{
    public class SearchModel : PageModel
    {
        private readonly IBlogRepository _blogRepo;

        public SearchModel(IBlogRepository blogRepository)
        {
            _blogRepo = blogRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string Term { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Resultpage { get; set; }

        public BlogResult SearchResults { get; set; }

        public void OnGet()
        {
            if (Resultpage > 0)
            {
                SearchResults = _blogRepo.GetPublishedPostsByTerm(Term, 10, Resultpage);
            }
            else
            {
                SearchResults = _blogRepo.GetPublishedPostsByTerm(Term);
            }
        }
    }
}