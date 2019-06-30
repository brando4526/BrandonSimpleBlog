using BrandonSimpleBlog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrandonSimpleBlog.Pages
{
    public class CategoryModel : PageModel
    {
        private readonly IBlogRepository _blogRepo;

        public CategoryModel(IBlogRepository blogRepository)
        {
            _blogRepo = blogRepository;
        }

        public string CategoryName { get; set; }
        public BlogResult CategoryResults { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Resultpage { get; set; }
        public void OnGet(string categoryName)
        {
            CategoryName = categoryName;
            if (Resultpage > 0)
            {
                CategoryResults = _blogRepo.GetPublishedPostsByCategory(categoryName, 10, Resultpage);
            }
            else
            {
                CategoryResults = _blogRepo.GetPublishedPostsByCategory(categoryName);
            }
        }
    }
}