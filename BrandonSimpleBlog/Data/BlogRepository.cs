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
            throw new NotImplementedException();
        }

        public bool DeletePost(int postid)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public BlogPost GetPost(string slug)
        {
            throw new NotImplementedException();
        }

        public BlogResult GetPosts(int pageSize = 10, int page = 1)
        {
            throw new NotImplementedException();
        }

        public BlogResult GetPostsByCategory(string category, int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public BlogResult GetPostsByMonth(int month, int year, int pageSize, int page)
        {
            throw new NotImplementedException();
        }

        public BlogResult GetPostsByTerm(string term, int pageSize, int page)
        {
            throw new NotImplementedException();
        }
    }
}
