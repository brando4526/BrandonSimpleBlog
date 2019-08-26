using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrandonSimpleBlog.Data
{
    public class BlogPost
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int PostId { get; set; }
        public string AuthorId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool HasImage { get; set; }
        public bool IsPublished { get; set; }
        [Required]
        public string Slug { get; set; }

        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }
        public string Categories { get; set; }
        public string UniqueId { get; set; }
        public bool IsFeatured { get; set; }
        public bool AllowComments { get; set; }
    }
}
