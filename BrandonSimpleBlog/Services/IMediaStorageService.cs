using System.IO;
using System.Threading.Tasks;

namespace BrandonSimpleBlog.Services
{
    public interface IMediaStorageService
    {
        Task<bool> SaveFileToStorage(byte[] file, string fileName);
        Task<bool> SaveImageToStorage(byte[] image, string fileName);
        Task<bool> SaveAvatarImage(byte[] image, string fileName, string userId);
        Task<bool> SaveProfileImage(byte[] image, string fileName, string userId);
        Task<bool> SaveBlogPostImage(byte[] image, string fileName, string slug, int postId);
        Task<bool> RemoveFromStorage(string fileName);
        Task<MemoryStream> DownloadFromStorage(string fileName);
    }
}
