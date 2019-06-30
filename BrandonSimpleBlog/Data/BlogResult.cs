using System.Collections.Generic;

namespace BrandonSimpleBlog.Data
{
    public class BlogResult
    {
        public IEnumerable<BlogPostDescription> Posts { get; set; }
        public int TotalReults { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
