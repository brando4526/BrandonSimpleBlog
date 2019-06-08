using BrandonSimpleBlog.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace BrandonSimpleBlog.Pages
{
    public class ArchiveModel : PageModel
    {
        private readonly IBlogRepository _blogRepo;

        public ArchiveModel(IBlogRepository blogRepository)
        {
            _blogRepo = blogRepository;
        }

        public IEnumerable<ArchiveEntry> ArchiveMonths { get; set; }

        public void OnGet()
        {
            ArchiveMonths = _blogRepo.GetPublishedArchiveMonths();
            
        }
    }
}