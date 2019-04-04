using System.ComponentModel.DataAnnotations;

namespace BrandonSimpleBlog.Data
{
    public class BlogCategoryAssignment
    {
        [Key]
        public int BlogCategoryAssignmentId { get; set; }
        public int PostId { get; set; }
        public int CategoryId { get; set; }

        public virtual BlogCategory Category { get; set; }
        public virtual BlogPost Post { get; set; }
    }
}
