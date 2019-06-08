using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrandonSimpleBlog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BrandonSimpleBlog.Pages
{
    public class PostModel : PageModel
    {
        private readonly IBlogRepository _blogRepo;

        public PostModel(IBlogRepository blogRepository)
        {
            _blogRepo = blogRepository;
        }
        public int Id { get; set; }
        public string Slug { get; set; }

        public BlogPost Post { get; set; }

        public void OnGet(int id, string slug)
        {
            Id = id;
            Slug = slug;
            Post = _blogRepo.GetPost(Id);
        }
    }
}