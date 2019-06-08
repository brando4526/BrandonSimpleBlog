using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrandonSimpleBlog.Data
{
    public class BlogPostDescription
    {
        public int PostId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string DateString { get; set; }  
        public string Slug { get; set; }
        public string Categories { get; set; }
        public string UniqueId { get; set; }
    }
}
