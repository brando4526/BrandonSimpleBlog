using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrandonSimpleBlog.Data
{
    public class BlogRepository : IBlogRepository
    {
        private ApplicationDbContext _context;
        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddPost(BlogPost post)
        {
            _context.Add(post);
            return _context.SaveChanges() > 0;
        }

        public bool DeletePost(int postid)
        {
            var post = _context.BlogPosts.Where<BlogPost>(i => i.PostId == postid).FirstOrDefault();
            if (post!=null)
            {
                _context.Remove(post); 
                _context.SaveChanges();
                return true;
            }
            return false;
            
        }


        public IEnumerable<string> GetArchiveMonths()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<string, int>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public BlogPost GetPost(int id)
        {
            return _context.BlogPosts.Where<BlogPost>(b => b.PostId == id).FirstOrDefault();
        }

        public BlogPost GetPost(string slug)
        {
            return _context.BlogPosts.Where<BlogPost>(b => b.Slug == slug).FirstOrDefault();
        }

        public BlogResult GetAllPosts(int pageSize = 10, int page = 1)
        {
            var totalResults = _context.BlogPosts.Count();
            var blogResult = new BlogResult()
            {
                CurrentPage = page,
                TotalPages = ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                TotalReults = totalResults,
                Posts = _context.BlogPosts.OrderByDescending(o => o.DatePublished).Skip((page - 1) * pageSize).Take(pageSize)

            };
            return blogResult;
        }

        public BlogResult GetPublishedPosts(int pageSize = 10, int page = 1)
        {
            var totalResults = _context.BlogPosts.Where(p => p.IsPublished).Count();
            var blogResult = new BlogResult()
            {
                CurrentPage=page,
                TotalPages= ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                TotalReults=totalResults,
                Posts= _context.BlogPosts.Where(p=>p.IsPublished).OrderByDescending(o => o.DatePublished).Skip((page - 1) * pageSize).Take(pageSize)

            };
            return blogResult;
        }

        public BlogResult GetPublishedPostsByCategory(string category, int pageSize, int page)
        {
            var totalResults = _context.BlogPosts.Where(p => p.IsPublished).Count();
            var categoryId = _context.BlogCategories.Where(c => c.Name == category).FirstOrDefault().CategoryId;
            var blogResult = new BlogResult()
            {
                CurrentPage = page,
                TotalPages = ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                TotalReults = totalResults,
                //Posts = _context.BlogPosts.Where(p => p.IsPublished && p.Categories.Contains(c=>c).).OrderByDescending(o => o.DatePublished).Skip((page - 1) * pageSize).Take(pageSize)

            };
            return blogResult;
        }

        public BlogResult GetPublishedPostsByMonth(int month, int year, int pageSize, int page)
        {
            var totalResults = _context.BlogPosts.Where(p => p.IsPublished).Count();
            var blogResult = new BlogResult()
            {
                CurrentPage = page,
                TotalPages = ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                TotalReults = totalResults,
                Posts = _context.BlogPosts.Where(p => p.IsPublished && p.DatePublished.Year==year && p.DatePublished.Month==month).OrderByDescending(o => o.DatePublished).Skip((page - 1) * pageSize).Take(pageSize)

            };
            return blogResult;
        }

        public BlogResult GetPublishedPostsByTerm(string term, int pageSize, int page)
        {
            var totalResults = _context.BlogPosts.Where(p => p.IsPublished).Count();
            var blogResult = new BlogResult()
            {
                CurrentPage = page,
                TotalPages = ((int)(totalResults / pageSize)) + ((totalResults % pageSize) > 0 ? 1 : 0),
                TotalReults = totalResults,
                Posts = _context.BlogPosts.Where(p => p.IsPublished && p.Title.ToLowerInvariant().Contains(term.ToLowerInvariant())).OrderByDescending(o => o.DatePublished).Skip((page - 1) * pageSize).Take(pageSize)

            };
            return blogResult;
        }
    }
}
