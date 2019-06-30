using System.Collections.Generic;

namespace BrandonSimpleBlog.Data
{
    public interface IBlogRepository
    {
        BlogResult GetPosts(bool onlyPublished, int pageSize = 10, int page = 1);
        BlogResult GetPostsByAuthor(string authorId, bool onlyPublished, int pageSize = 10, int page = 1);
        BlogResult GetPublishedPostsByTerm(string term, int pageSize = 10, int page = 1);
        BlogResult GetPublishedPostsByCategory(string category, int pageSize = 10, int page = 1);
        BlogResult GetPublishedPostsByMonth(int month, int year, int pageSize = 10, int page = 1);

        IEnumerable<BlogPostDescription> GetFeaturedPosts();
        IEnumerable<ArchiveEntry> GetPublishedArchiveMonths();
        BlogPost GetPost(int id);
        BlogPost GetPost(string slug);
        bool AddPost(BlogPost post);
        bool DeletePost(int postid);

        IEnumerable<CategoryEntry> GetCategories(bool onlyPublished);
    }
}
