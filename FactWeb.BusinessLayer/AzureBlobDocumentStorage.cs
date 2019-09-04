using FactWeb.Infrastructure;
using FactWeb.Model;
using FactWeb.Model.InterfaceItems;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FactWeb.BusinessLayer
{
    public class AzureBlobDocumentStorage : IDocumentLibrary
    {
        const string Url = "https://factweb.blob.core.windows.net/";

        public CloudBlobContainer GetContainer(string organizationName)
        {
            var storageAccount =
                CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings[Constants.ConfigurationConstants.StorageConnectionString]);

            var blobClient = storageAccount.CreateCloudBlobClient();

            var org = FormatContainerName(organizationName);

            var container = blobClient.GetContainerReference(org);

            container.CreateIfNotExists();

            return container;
        }

        public async Task<CloudBlobContainer> GetContainerAsync(string organizationName)
        {
            var storageAccount =
                CloudStorageAccount.Parse(
                    ConfigurationManager.AppSettings[Constants.ConfigurationConstants.StorageConnectionString]);

            var blobClient = storageAccount.CreateCloudBlobClient();

            var org = FormatContainerName(organizationName);
            
            var container = blobClient.GetContainerReference(org);

            await container.CreateIfNotExistsAsync();

            return container;
        }

        private string FormatContainerName(string containerName)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            containerName = rgx.Replace(containerName, "");
            containerName = containerName.ToLower();
            if (containerName.Length > 50)
            {
                containerName = containerName.Substring(0, 50);
            }
            return containerName;
        }

        private string GetFileType(string fileName)
        {
            fileName = fileName.ToLower();

            if (fileName.Contains(".jpg") || fileName.Contains(".jpeg"))
            {
                return "image/jpeg";
            }
            else if (fileName.Contains(".bm") || fileName.Contains(".bmp"))
            {
                return "image/bmp";
            }
            else if (fileName.Contains(".gif"))
            {
                return "image/gif";
            }
            else if (fileName.Contains(".txt"))
            {
                return "text/plain";
            }
            else if (fileName.Contains(".doc") || fileName.Contains(".docx"))
            {
                return "application/msword";
            }
            else if (fileName.Contains(".xls") || fileName.Contains(".docx"))
            {
                return "application/x-msexcel";
            }
            else if (fileName.Contains(".pdf"))
            {
                return "application/pdf";
            }

            return "text/plain";
        }

        public Document AddFile(string organizationName, string fileName, byte[] file)
        {
            var container = this.GetContainer(organizationName);

            if (container == null) throw new NullReferenceException("BlobContainer");

            var blockBlob = container.GetBlockBlobReference(fileName);

            var fileType = this.GetFileType(fileName);

            blockBlob.Properties.ContentType = fileType;
            blockBlob.Metadata["filename"] = fileName;
            blockBlob.Metadata["filemime"] = fileType;

            using (var stream = new MemoryStream(file))
            {
                blockBlob.UploadFromStream(stream);
            }

            return new Document
            {
               // Path = string.Format("{0}/{2}", container.StorageUri.PrimaryUri, fileName)
            };
        }

        public async Task<Document> AddFileAsync(string organizationName, string fileName, byte[] file)
        {
            var container = await this.GetContainerAsync(organizationName);

            if (container == null) throw new NullReferenceException("BlobContainer");

            var blockBlob = container.GetBlockBlobReference(fileName);

            var fileType = this.GetFileType(fileName);

            blockBlob.Properties.ContentType = fileType;
            blockBlob.Metadata["filename"] = fileName;
            blockBlob.Metadata["filemime"] = fileType;

            using (var stream = new MemoryStream(file))
            {
                await blockBlob.UploadFromStreamAsync(stream);
            }

            return new Document
            {
                //Path = string.Format("{0}/{1}", container.StorageUri.PrimaryUri, fileName)
            };
        }

        public Document AddFile(string organizationName, string fileName, string fileType, byte[] file)
        {
            var container = this.GetContainer(organizationName);

            if (container == null) throw new NullReferenceException("BlobContainer");

            var blockBlob = container.GetBlockBlobReference(fileName);

            blockBlob.Properties.ContentType = fileType;
            blockBlob.Metadata["filename"] = fileName;
            blockBlob.Metadata["filemime"] = fileType;

            using (var stream = new MemoryStream(file))
            {
                blockBlob.UploadFromStream(stream);
            }

            return new Document
            {
                //Path = string.Format("{0}/{2}", container.StorageUri.PrimaryUri, fileName)
            };
        }

        public async Task<Document> AddFileAsync(string organizationName, string fileName, string fileType, byte[] file)
        {
            var container = await this.GetContainerAsync(organizationName);

            if (container == null) throw new NullReferenceException("BlobContainer");

            var blockBlob = container.GetBlockBlobReference(fileName);

            blockBlob.Properties.ContentType = fileType;
            blockBlob.Metadata["filename"] = fileName;
            blockBlob.Metadata["filemime"] = fileType;

            using (var stream = new MemoryStream(file))
            {
                await blockBlob.UploadFromStreamAsync(stream);
            }

            return new Document
            {
                //Path = string.Format("{0}/{1}", container.StorageUri.PrimaryUri, fileName)
            };
        }

        public MemoryStream GetFileAsStream(string organizationName, string fileName)
        {
            var container = this.GetContainer(organizationName);

            var blockBlob = container.GetBlockBlobReference(fileName);

            var stream = new MemoryStream();
            blockBlob.DownloadToStream(stream);

            return stream;
        }

        public DocumentDownload GetFile(string organizationName, string fileName)
        {
            var response = new DocumentDownload();
            var container = this.GetContainer(organizationName);

            var blockBlob = container.GetBlockBlobReference(fileName);

            using (var stream = new MemoryStream())
            {
                blockBlob.DownloadToStream(stream);

                response.File = stream.ToArray();
            }
                
            response.FileLength = blockBlob.Properties.Length;
            response.ContentType = blockBlob.Properties.ContentType;

            return response;
        }

        public async Task<DocumentDownload> GetFileAsync(string organizationName, string fileName)
        {
            var response = new DocumentDownload();
            var container = await this.GetContainerAsync(organizationName);

            var blockBlob = container.GetBlockBlobReference(fileName);

            using (var stream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(stream);

                response.File = stream.ToArray();
            }

            response.FileLength = blockBlob.Properties.Length;
            response.ContentType = blockBlob.Properties.ContentType;

            return response;
        }

        public void Remove(string organizationName, string fileName)
        {
            var container = this.GetContainer(organizationName);

            var blockBlob = container.GetBlockBlobReference(fileName);

            blockBlob.Delete();
        }

        public async Task RemoveAsync(string organizationName, string fileName)
        {
            var container = await this.GetContainerAsync(organizationName);

            var blockBlob = container.GetBlockBlobReference(fileName);

            await blockBlob.DeleteAsync();
        }
    }
}
