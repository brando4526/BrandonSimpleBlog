using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public void OnGet(string categoryName)
        {
            CategoryName = categoryName;
            CategoryResults = _blogRepo.GetPublishedPostsByCategory(categoryName);
        }
    }
}