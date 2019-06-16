using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BrandonSimpleBlog.Services
{
    public class AzureStorageService : IMediaStorageService
    {
        private readonly IConfiguration _config;

        public AzureStorageService(IConfiguration config)
        {
            _config = config;
        }
        public Task<MemoryStream> DownloadFromStorage(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFromStorage(string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveProfileImage(byte[] image, string fileName, string userId)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                _config["BlobService:StorageConnectionString"]);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("images");

            await container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Container, null, null);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("profile/" + userId + "." + fileName.Split('.').Last());
            blockBlob.Properties.ContentType = DeriveMIME(fileName);
            bool shouldUpload = true;
            if (await blockBlob.ExistsAsync())
            {
                await blockBlob.FetchAttributesAsync();
                if (blockBlob.Properties.Length == image.Length)
                {
                    shouldUpload = false;
                }
            }

            if (shouldUpload)
                await blockBlob.UploadFromByteArrayAsync(image, 0, image.Length);

            return shouldUpload;
        }

        public async Task<bool> SaveAvatarImage(byte[] image, string fileName, string userId)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                _config["BlobService:StorageConnectionString"]);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("images");

            await container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Container, null, null);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("avatar/" +userId + "." + fileName.Split('.').Last());
            blockBlob.Properties.ContentType = DeriveMIME(fileName);
            bool shouldUpload = true;
            if (await blockBlob.ExistsAsync())
            {
                await blockBlob.FetchAttributesAsync();
                if (blockBlob.Properties.Length == image.Length)
                {
                    shouldUpload = false;
                }
            }

            if (shouldUpload)
                await blockBlob.UploadFromByteArrayAsync(image, 0, image.Length);

            return shouldUpload;
        }

        public async Task<bool> SaveBlogPostImage(byte[] image, string fileName, string slug, int postId)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                 _config["BlobService:StorageConnectionString"]);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("images");

            await container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Container, null, null);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("post/"+postId+slug +"."+ fileName.Split('.').Last());
            blockBlob.Properties.ContentType = DeriveMIME(fileName);
            bool shouldUpload = true;
            if (await blockBlob.ExistsAsync())
            {
                await blockBlob.FetchAttributesAsync();
                if (blockBlob.Properties.Length == image.Length)
                {
                    shouldUpload = false;
                }
            }

            if (shouldUpload)
                await blockBlob.UploadFromByteArrayAsync(image, 0, image.Length);

            return shouldUpload;
        }

        public async Task<bool> SaveFileToStorage(byte[] file, string fileName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                _config["BlobService:StorageConnectionString"]);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("files");

            await container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Container, null, null);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = DeriveMIME(fileName);
            await blockBlob.SetPropertiesAsync();
            bool shouldUpload = true;
            if (await blockBlob.ExistsAsync())
            {
                await blockBlob.FetchAttributesAsync();
                if (blockBlob.Properties.Length == file.Length)
                {
                    shouldUpload = false;
                }
            }

            if (shouldUpload)
                await blockBlob.UploadFromByteArrayAsync(file, 0, file.Length);

            return shouldUpload;
        }

        public async Task<bool> SaveImageToStorage(byte[] image, string fileName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                _config["BlobService:StorageConnectionString"]);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("images");

            await container.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Container, null, null);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = DeriveMIME(fileName);
            bool shouldUpload = true;
            if (await blockBlob.ExistsAsync())
            {
                await blockBlob.FetchAttributesAsync();
                if (blockBlob.Properties.Length == image.Length)
                {
                    shouldUpload = false;
                }
            }

            if (shouldUpload)
                await blockBlob.UploadFromByteArrayAsync(image, 0, image.Length);
           
            return shouldUpload;
        }


        private static string DeriveMIME(string filename)
        {
            if (filename.EndsWith("jpg")||filename.EndsWith("jpeg"))
            {
                return "image/jpg";
            }
            else if (filename.EndsWith("png"))
            {
                return "image/png";
            }
            else if (filename.EndsWith("gif"))
            {
                return "image/gif";
            }
            else if (filename.EndsWith("txt"))
            {
                return "text/plain";
            }

            return "";
        }

        
    }
}
