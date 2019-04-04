using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrandonSimpleBlog.Data
{
    public class BlogPost
    {
        [Key]
        public int PostId { get; set; }
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public string ImageURL { get; set; }
        public DateTime DatePublished { get; set; }
        public int MyProperty { get; set; }

        public bool IsPublished { get; set; }
        public string Slug { get; set; }

        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }
        public virtual ICollection<BlogCategoryAssignment> Categories { get; set; }
    }
}
