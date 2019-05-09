using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrandonSimpleBlog.Data
{
    interface IBlogRepository
    {
        BlogResult GetPosts(int pageSize=10, int page=1);
        BlogResult GetPostsByTerm(string term, int pageSize, int page);
        BlogResult GetPostsByCategory(string category, int pageSize, int page);
        BlogResult GetPostsByMonth(int month, int year, int pageSize, int page);
        IEnumerable<string> GetArchiveMonths();
        BlogPost GetPost(int id);
        BlogPost GetPost(string slug);
        bool AddPost(BlogPost post);
        bool DeletePost(int postid);

        IEnumerable<KeyValuePair<string,int>> GetCategories();
    }
}
