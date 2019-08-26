using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BrandonSimpleBlog.Data
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddPost(BlogPost post)
        {
            _context.Add(post);
            return _context.SaveChanges() > 0;
        }

        public bool UpdatePost(BlogPost post)
        {
            _context.Update(post);
            return _context.SaveChanges() > 0;
        }

        public bool DeletePost(int postid)
        {
            var post = _context.BlogPosts.Where(i => i.PostId == postid).FirstOrDefault();
            if (post!=null)
            {
                _context.Remove(post); 
                return _context.SaveChanges()>0;
            }
            return false;      
        }


        public IEnumerable<ArchiveEntry> GetPublishedArchiveMonths()
        {
            var archiveList = _context.BlogPosts
                .Where(p => p.IsPublished)
                .GroupBy(o => new
                {
                    Month = o.DatePublished.Month,
                    Year = o.DatePublished.Year
                })
                .Select(g => new ArchiveEntry
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Total = g.Count()
                })
                .OrderByDescending(a => a.Year)
                .ThenByDescending(a => a.Month)
                .ToList();
            return archiveList;
        }
        
        public IEnumerable<CategoryEntry> GetCategories(bool onlyPublished)
        {
            if (onlyPublished)
            {
                var cats = _context.BlogPosts
                    .Where(p => p.IsPublished && p.Categories != null)
                    .Select(c => c.Categories.Split(new[] { ',' }, StringSplitOptions.None))
                    .ToList();

                var result = new List<string>();
                foreach (var s in cats)
                    result.AddRange(s);

                var catCount = result.GroupBy(c=>c).Select(o=> new CategoryEntry { CategoryName=o.Key, Total=o.Count()}).OrderBy(s => s.CategoryName);
                return catCount;
            }
            else
            {
                var cats = _context.BlogPosts
                    .Where(p=>p.Categories!=null)
                    .Select(c => c.Categories.Split(new[] { ',' }, StringSplitOptions.None))
                    .ToList();

                var result = new List<string>();
                foreach (var s in cats)
                    result.AddRange(s);

                var catCount = result.GroupBy(c => c).Select(o => new CategoryEntry { CategoryName = o.Key, Total = o.Count() }).OrderBy(s => s.CategoryName);
                return catCount;
            }
            
        }

        public BlogPost GetPost(int id)
        {
            return _context.BlogPosts.Where(b => b.PostId == id).FirstOrDefault();
        }

        public BlogPost GetPost(string slug)
        {
            return _context.BlogPosts.Where(b => b.Slug == slug).FirstOrDefault();
        }

        public BlogResult GetPosts(bool onlyPublished, int pageSize = 10, int page = 1)
        {
            if (onlyPublished)
            {
                var totalResults = _context.BlogPosts.Where(p => p.IsPublished).Count();
                var blogResult = new BlogResult()
                {
                    CurrentPage=page,
                    TotalPages= ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                    TotalReults=totalResults,
                    Posts= _context.BlogPosts.Where(p => p.IsPublished)
                    .OrderByDescending(o => o.DatePublished)
                    .Select(d=>new BlogPostDescription {
                        AuthorId=d.AuthorId,
                        AuthorName=d.Author.FirstName+" "+d.Author.LastName,
                        Categories=d.Categories,
                        Excerpt=d.Excerpt,
                        DateString= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.DatePublished.Month)+" "+d.DatePublished.Day+", "+d.DatePublished.Year,
                        PostId=d.PostId,
                        Slug=d.Slug,
                        Title=d.Title,
                        UniqueId=d.UniqueId,
                        IsPublished=d.IsPublished,
                        IsFeatured=d.IsFeatured,
                        AllowComments=d.AllowComments
                        
                    })
                    .Skip((page - 1) * pageSize).Take(pageSize)

                };
                return blogResult;
            }
            else
            {
                var totalResults = _context.BlogPosts.Count();
                var blogResult = new BlogResult()
                {
                    CurrentPage = page,
                    TotalPages = ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                    TotalReults = totalResults,
                    Posts = _context.BlogPosts.OrderByDescending(o => o.DateCreated)
                    .Select(d => new BlogPostDescription
                    {
                        AuthorId = d.AuthorId,
                        AuthorName = d.Author.FirstName + " " + d.Author.LastName,
                        Categories = d.Categories,
                        Excerpt = d.Excerpt,
                        DateString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.DateCreated.Month) + " " + d.DateCreated.Day + ", " + d.DateCreated.Year,
                        PostId = d.PostId,
                        Slug = d.Slug,
                        Title = d.Title,
                        UniqueId = d.UniqueId,
                        IsPublished=d.IsPublished,
                        IsFeatured = d.IsFeatured,
                        AllowComments=d.AllowComments
                    })
                    .Skip((page - 1) * pageSize).Take(pageSize)

                };
                return blogResult;
            }
            
        }

        public BlogResult GetPostsByAuthor(string authorId, bool onlyPublished, int pageSize = 10, int page = 1)
        {
            if (onlyPublished)
            {
                var totalResults = _context.BlogPosts.Where(p => p.IsPublished && p.AuthorId == authorId).Count();
                var blogResult = new BlogResult()
                {
                    CurrentPage = page,
                    TotalPages = ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                    TotalReults = totalResults,
                    Posts = _context.BlogPosts.Where(p => p.IsPublished && p.AuthorId==authorId)
                    .OrderByDescending(o => o.DatePublished)
                    .Select(d => new BlogPostDescription
                    {
                        AuthorId = d.AuthorId,
                        AuthorName = d.Author.FirstName + " " + d.Author.LastName,
                        Categories = d.Categories,
                        Excerpt = d.Excerpt,
                        DateString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.DatePublished.Month) + " " + d.DatePublished.Day + ", " + d.DatePublished.Year,
                        PostId = d.PostId,
                        Slug = d.Slug,
                        Title = d.Title,
                        UniqueId = d.UniqueId,
                        IsPublished=d.IsPublished,
                        IsFeatured = d.IsFeatured,
                        AllowComments=d.AllowComments
                    })
                    .Skip((page - 1) * pageSize).Take(pageSize)

                };
                return blogResult;
            }
            else
            {

                var totalResults = _context.BlogPosts.Where(p=>p.AuthorId==authorId).Count();
                var blogResult = new BlogResult()
                {
                    CurrentPage = page,
                    TotalPages = ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                    TotalReults = totalResults,
                    Posts = _context.BlogPosts.Where(p => p.AuthorId == authorId)
                    .OrderByDescending(o => o.DateCreated)
                    .Select(d => new BlogPostDescription
                    {
                        AuthorId = d.AuthorId,
                        AuthorName = d.Author.FirstName + " " + d.Author.LastName,
                        Categories = d.Categories,
                        Excerpt = d.Excerpt,
                        DateString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.DateCreated.Month) + " " + d.DateCreated.Day + ", " + d.DateCreated.Year,
                        PostId = d.PostId,
                        Slug = d.Slug,
                        Title = d.Title,
                        UniqueId = d.UniqueId,
                        IsPublished=d.IsPublished,
                        IsFeatured = d.IsFeatured,
                        AllowComments=d.AllowComments
                    })
                    .Skip((page - 1) * pageSize).Take(pageSize)

                };
                return blogResult;
            }
        }

        public BlogResult GetPublishedPostsByCategory(string category, int pageSize=10, int page=1)
        {
            var totalResults = _context.BlogPosts.Where(p => p.IsPublished && p.Categories.ToLowerInvariant().Contains(category.ToLowerInvariant())).Count();
            var blogResult = new BlogResult()
            {
                CurrentPage = page,
                TotalPages = ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                TotalReults = totalResults,
                Posts = _context.BlogPosts.Where(p => p.IsPublished && p.Categories.ToLowerInvariant().Contains(category.ToLowerInvariant())).OrderByDescending(o => o.DatePublished)
                .Select(d => new BlogPostDescription
                {
                    AuthorId = d.AuthorId,
                    AuthorName = d.Author.FirstName + " " + d.Author.LastName,
                    Categories = d.Categories,
                    Excerpt = d.Excerpt,
                    DateString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.DatePublished.Month) + " " + d.DatePublished.Day + ", " + d.DatePublished.Year,
                    PostId = d.PostId,
                    Slug = d.Slug,
                    Title = d.Title,
                    UniqueId = d.UniqueId,
                    IsPublished=d.IsPublished,
                    IsFeatured = d.IsFeatured,
                    AllowComments=d.AllowComments
                })
                .Skip((page - 1) * pageSize).Take(pageSize)

            };
            return blogResult;
        }

        public BlogResult GetPublishedPostsByMonth(int month, int year, int pageSize=10, int page=1)
        {
            var totalResults = _context.BlogPosts.Where(p => p.IsPublished && p.DatePublished.Year == year && p.DatePublished.Month == month).Count();
            var blogResult = new BlogResult()
            {
                CurrentPage = page,
                TotalPages = ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                TotalReults = totalResults,
                Posts = _context.BlogPosts.Where(p => p.IsPublished && p.DatePublished.Year==year && p.DatePublished.Month==month).OrderByDescending(o => o.DatePublished)
                .Select(d => new BlogPostDescription
                {
                    AuthorId = d.AuthorId,
                    AuthorName = d.Author.FirstName + " " + d.Author.LastName,
                    Categories = d.Categories,
                    Excerpt = d.Excerpt,
                    DateString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.DatePublished.Month) + " " + d.DatePublished.Day + ", " + d.DatePublished.Year,
                    PostId = d.PostId,
                    Slug = d.Slug,
                    Title = d.Title,
                    UniqueId = d.UniqueId,
                    IsPublished=d.IsPublished,
                    IsFeatured = d.IsFeatured,
                    AllowComments=d.AllowComments
                })
                .Skip((page - 1) * pageSize).Take(pageSize)

            };
            return blogResult;
        }

        public BlogResult GetPublishedPostsByTerm(string term, int pageSize=10, int page=1)
        {
            var totalResults = _context.BlogPosts.Where(p => p.IsPublished && p.Title.ToLowerInvariant().Contains(term.ToLowerInvariant())).Count();
            var blogResult = new BlogResult()
            {
                CurrentPage = page,
                TotalPages = ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                TotalReults = totalResults,
                Posts = _context.BlogPosts.Where(p => p.IsPublished && p.Title.ToLowerInvariant().Contains(term.ToLowerInvariant())).OrderByDescending(o => o.DatePublished)
                .Select(d => new BlogPostDescription
                {
                    AuthorId = d.AuthorId,
                    AuthorName = d.Author.FirstName + " " + d.Author.LastName,
                    Categories = d.Categories,
                    Excerpt = d.Excerpt,
                    DateString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.DatePublished.Month) + " " + d.DatePublished.Day + ", " + d.DatePublished.Year,
                    PostId = d.PostId,
                    Slug = d.Slug,
                    Title = d.Title,
                    UniqueId = d.UniqueId,
                    IsPublished=d.IsPublished,
                    IsFeatured = d.IsFeatured,
                    AllowComments=d.AllowComments
                })
                .Skip((page - 1) * pageSize).Take(pageSize)

            };
            return blogResult;
        }

        public IEnumerable<BlogPostDescription> GetFeaturedPosts()
        {
            var featuredPosts = _context.BlogPosts.Where(f => f.IsFeatured)
                .Select(d => new BlogPostDescription
                {
                    AuthorId = d.AuthorId,
                    AuthorName = d.Author.FirstName + " " + d.Author.LastName,
                    Categories = d.Categories,
                    Excerpt = d.Excerpt,
                    DateString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(d.DatePublished.Month) + " " + d.DatePublished.Day + ", " + d.DatePublished.Year,
                    PostId = d.PostId,
                    Slug = d.Slug,
                    Title = d.Title,
                    UniqueId = d.UniqueId,
                    IsPublished = d.IsPublished,
                    IsFeatured = d.IsFeatured,
                    AllowComments=d.AllowComments
                })
                .ToList();
            return featuredPosts;
        }

        
    }
}
