using System;
using System.IO;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Chat.Common
{
    public class AzureBlobStorage
    {
        private static CloudBlobContainer container;

        static AzureBlobStorage()
        {
            var accountName = "mediabox";
            var accountKey = "iHOCAxJTOscOrFakHOWKLyl0twX3fWJqTGRB8K4Iznap1UN9zBNkE7bs1Ul3Vs3BwSA6htILMDry6SUCVquWIg==";

            var creds = new StorageCredentials(accountName, accountKey);
            var storageAccount = new CloudStorageAccount(creds, useHttps: true);
            var blobStorage = storageAccount.CreateCloudBlobClient();
            container = blobStorage.GetContainerReference("webtemplate");
        }

        #region StoreText

        private static CloudBlobDirectory GetDirectory(int facilityId)
        {
            if (container.CreateIfNotExists())
            {
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }

            // Create or use existing directory
            var facilityDirectory = container.GetDirectoryReference(string.Format("Facility-{0}", facilityId));

            return facilityDirectory;
        }

        private static string GetContainerPath(CloudBlobDirectory cloudDirectory)
        {
            // AuditContainerPath
            var containerAbsolutePath = container.Uri.AbsolutePath;
            var auditDirectoryAbsolutePath = cloudDirectory.Uri.AbsolutePath;
            var auditContainerPath = auditDirectoryAbsolutePath.Replace(containerAbsolutePath, string.Empty).TrimStart('/');

            return auditContainerPath;
        }

        public static string StoreFile(HttpPostedFileBase file)
        {
            if (container.CreateIfNotExists())
            {
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }

            var blob = container.GetBlockBlobReference(UniqueBlobName("", file.FileName));
            blob.Properties.ContentType = file.ContentType;
            file.InputStream.Position = 0;
            using (var imageStream = new MemoryStream())
            {
                file.InputStream.CopyTo(imageStream);
                imageStream.Position = 0;
                blob.UploadFromStream(imageStream);
            }

            return blob.Uri.ToString();
        }

        #endregion StoreText

        #region StoreImage

        public static string StoreImage(HttpPostedFileBase image)
        {
            if (container.CreateIfNotExists())
            {
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }

            var blob = container.GetBlockBlobReference(UniqueBlobName());
            blob.Properties.ContentType = image.ContentType;
            image.InputStream.Position = 0;
            using (var imageStream = new MemoryStream())
            {
                image.InputStream.CopyTo(imageStream);
                imageStream.Position = 0;
                blob.UploadFromStream(imageStream);
            }

            return blob.Uri.ToString();
        }

        public static string StoreImage(string image64String, string original_path = "")
        {
            if (container.CreateIfNotExists())
            {
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }

            var blob = container.GetBlockBlobReference(UniqueBlobName());
            blob.Properties.ContentType = "image/png";
            using (var imageStream = new MemoryStream(System.Convert.FromBase64String(image64String)))
            {
                blob.UploadFromStream(imageStream);
            }

            return blob.Uri.ToString();
        }

        public static string StoreImage(Stream imageStream, string original_path)
        {
            if (container.CreateIfNotExists())
            {
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }

            var blob = container.GetBlockBlobReference(original_path);
            blob.Properties.ContentType = "image/jpg";
            blob.UploadFromStream(imageStream);

            return blob.Uri.ToString();
        }

        #endregion StoreImage

        private static string UniqueBlobName(string guid = "", string ext = ".png")
        {
            guid = String.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() + "/" : guid;

            return string.Format("{0}{1}", guid, ext).ToLowerInvariant();
        }

        private static string UniqueBlobProfileName(string guid = "", string ext = ".xml")
        {
            guid = String.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() + "/" : guid;

            return string.Format("{0}{1}", guid, ext).ToLowerInvariant();
        }

        public static string StaticAzureLink(string guid, string ext = ".png")
        {
            return container.GetBlockBlobReference(UniqueBlobName(guid)).Uri.ToString();
        }

        #region File Storage
        public static string StoreImage(string image64String, int facilityId)
        {
            var cloudDirectory = GetDirectory(facilityId);

            var blob = cloudDirectory.GetBlockBlobReference(UniqueBlobName());
            blob.Properties.ContentType = "image/png";

            using (var imageStream = new MemoryStream(System.Convert.FromBase64String(image64String)))
            {
                blob.UploadFromStream(imageStream);
            }

            return blob.Uri.ToString();
        }

        public static string StoreImage(HttpPostedFileBase image, int facilityId)
        {
            var cloudDirectory = GetDirectory(facilityId);

            var blob = cloudDirectory.GetBlockBlobReference(UniqueBlobName());
            blob.Properties.ContentType = image.ContentType;

            image.InputStream.Position = 0;
            using (var imageStream = new MemoryStream())
            {
                image.InputStream.CopyTo(imageStream);
                imageStream.Position = 0;
                blob.UploadFromStream(imageStream);
            }

            return blob.Uri.ToString();
        }
        #endregion

        public static void DeleteBlob(string blobname)
        {
            try
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(blobname);
                blob.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static Stream GetBlobResponse(string blobname)
        {
            try
            {
                var cloudBlob = container.GetBlockBlobReference(blobname);
                var blobStream = cloudBlob.OpenRead();
                blobStream.Seek(0, SeekOrigin.Begin);

                return blobStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
    }
}