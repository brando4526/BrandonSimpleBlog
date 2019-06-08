using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
