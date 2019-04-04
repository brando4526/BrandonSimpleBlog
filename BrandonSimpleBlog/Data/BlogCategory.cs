using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrandonSimpleBlog.Data
{
    public class BlogCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BlogCategoryAssignment> BlogPosts { get; set; }
    }
}
