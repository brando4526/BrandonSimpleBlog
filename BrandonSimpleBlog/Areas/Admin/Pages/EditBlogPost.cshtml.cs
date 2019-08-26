using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using BrandonSimpleBlog.Data;
using BrandonSimpleBlog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace BrandonSimpleBlog.Areas.Admin.Pages
{
    public class EditBlogPostModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlogRepository _blogRepo;
        private readonly IConfiguration _configuration;
        private readonly IMediaStorageService _mediaStorageService;

        public EditBlogPostModel(IBlogRepository blogRepository, UserManager<ApplicationUser> userManager, IConfiguration configuration, IMediaStorageService mediaStorageService)
        {
            _blogRepo = blogRepository;
            _userManager = userManager;
            _configuration = configuration;
            _mediaStorageService = mediaStorageService;
        }

        public string PostImageURL { get; set; }
        public List<string> CategoryOptions { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            public int Id { get; set; }
            //[Required]
            [DataType(DataType.Text)]
            [Display(Name = "Title")]
            public string Title { get; set; }
            //[Required]
            [DataType(DataType.Html)]
            [Display(Name = "Post Content")]
            public string Content { get; set; }

            //[Required]
            [DataType(DataType.Text)]
            [Display(Name = "Excerpt")]
            public string Excerpt { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "Categories")]
            public string Categories { get; set; }
            [DataType(DataType.Upload)]
            [Display(Name = "Post Image(png format 750x300 or larger)")]
            public IFormFile Image { get; set; }
            [Display(Name = "Allow Comments?")]
            public bool AllowComments { get; set; }

        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            CategoryOptions = new List<string>();

            foreach(var category in _blogRepo.GetCategories(false))
            {
                CategoryOptions.Add(category.CategoryName);
            }
            if (id>0)
            {
                var user = await _userManager.GetUserAsync(User);
                var blogPost = _blogRepo.GetPost(id);
                
                if (!User.IsInRole("Administrator") && !user.Id.Equals(blogPost.AuthorId))
                {
                    return RedirectToPage("./ManageBlogPosts");
                }

                Input = new InputModel
                {
                    Id = blogPost.PostId,
                    Title=blogPost.Title,
                    Content=blogPost.Content,
                    Excerpt=blogPost.Excerpt,
                    Categories=blogPost.Categories
                  
                };

                if (blogPost.HasImage)
                {
                    PostImageURL = _configuration.GetSection("BlobService")["StorageURL"] + "images/post/" + blogPost.PostId + blogPost.Slug + ".png";
                }
                else
                {
                    PostImageURL = _configuration.GetSection("BlobService")["StorageURL"] + "images/post/750x300blogpost.png";
                }
                return Page();
            }
            else
            {
                Input = new InputModel
                {
                    Id = 0,
                    Categories="",
                    AllowComments=false
                };
                PostImageURL = _configuration.GetSection("BlobService")["StorageURL"] + "images/post/750x300blogpost.png";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CategoryOptions = new List<string>();

            foreach (var category in _blogRepo.GetCategories(false))
            {
                CategoryOptions.Add(category.CategoryName);
            }
            if (ModelState.IsValid)
            {
                
                if (Input.Id==0)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var blogPost = new BlogPost
                    {
                        Title = Input.Title,
                        Excerpt = Input.Excerpt,
                        Content = Input.Content,
                        AuthorId = user.Id,
                        Categories = Input.Categories,
                        HasImage = false,
                        
                        DateCreated = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        IsPublished = false,
                        IsFeatured = false,
                        //TODO: UniqueId
                        Slug = Input.Title.ToLowerInvariant().Replace(' ', '-'),
                        AllowComments=Input.AllowComments
                    };
                    

                    _blogRepo.AddPost(blogPost);
                    if (Input.Image != null)
                    {
                        if (Input.Image.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                Input.Image.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                bool imageSaved = await _mediaStorageService.SaveBlogPostImage(fileBytes, Input.Image.FileName, blogPost.Slug, blogPost.PostId);
                                blogPost.HasImage = imageSaved;
                                _blogRepo.UpdatePost(blogPost);
                            }
                        }
                        
                    }
                    return RedirectToPage("./EditBlogPost", new { id = blogPost.PostId });

                }
                else
                {
                    
                    var blogPost = _blogRepo.GetPost(Input.Id);
                    blogPost.Content = Input.Content;
                    blogPost.Excerpt = Input.Excerpt;
                    blogPost.Categories=Input.Categories;
                    blogPost.LastUpdated = DateTime.Now;
                    blogPost.AllowComments = Input.AllowComments;
                    if (Input.Image != null)
                    {
                        if (Input.Image.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                Input.Image.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                bool imageSaved = await _mediaStorageService.SaveBlogPostImage(fileBytes, Input.Image.FileName, blogPost.Slug, blogPost.PostId);
                                blogPost.HasImage = imageSaved;
                                
                            }
                        }

                    }

                    _blogRepo.UpdatePost(blogPost);
                }
                
            }
            
            return Page();
        }
    }
}