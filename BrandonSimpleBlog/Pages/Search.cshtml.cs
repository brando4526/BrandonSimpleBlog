using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public string term { get; set; }

        public BlogResult SearchResults { get; set; }

        public void OnGet()
        {
            SearchResults = _blogRepo.GetPublishedPostsByTerm(term);
        }
    }
}