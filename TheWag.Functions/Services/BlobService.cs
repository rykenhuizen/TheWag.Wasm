using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TheWag.Wasm.Util;

namespace TheWag.Functions.Services
{

    public class BlobService
    {
        private readonly ILogger<BlobService> _logger;
        private readonly AppSettings _appSettings;
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(ILogger<BlobService> logger, AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
            _blobServiceClient = new BlobServiceClient(_appSettings.StorageConnectionString);
        }

        public string SavePic(IFormFile image, string container)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(container);
                containerClient.CreateIfNotExists();
                using var myBlob = image.OpenReadStream();
                var blobClient = containerClient.GetBlobClient(image.FileName);
                blobClient.Upload(myBlob);

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in saving picture to blob storage: {image.FileName}", image.FileName);
                throw;
            }
        }

        public List<string> GetFileList(string containerName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobs = containerClient.GetBlobs();
                var blobNames = new List<string>();
                foreach (var blob in blobs)
                {
                    blobNames.Add(blob.Name);
                }
                return blobNames;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting picture list from container{containerName}", containerName);
                throw;
            }
        }

        public MemoryStream GetPicStream(string containerName, string blobName)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);
                var ms = new MemoryStream();
                blobClient.DownloadTo(ms);
                ms.Position = 0;
                return ms;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting picture {blobName} from {containerName}", blobName, containerName );
                throw;
            }
        }

        //public async Task DeletePic(string containerName, string blobName)
        //{
        //    try
        //    {
        //        var blobServiceClient = new BlobServiceClient(_appSettings.StorageConnectionString);
        //        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        //        var blobClient = containerClient.GetBlobClient(blobName);
        //        await blobClient.DeleteIfExistsAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in deleting picture");
        //        throw;
        //    }
        //}

        public async Task MovePic(string sourceContainerName, string destinationContainerName, string blobName)
        {
            try
            {
                var sourceContainerClient = _blobServiceClient.GetBlobContainerClient(sourceContainerName);
                var destinationContainerClient = _blobServiceClient.GetBlobContainerClient(destinationContainerName);
                await destinationContainerClient.CreateIfNotExistsAsync();
                var sourceBlobClient = sourceContainerClient.GetBlobClient(blobName);
                var destinationBlobClient = destinationContainerClient.GetBlobClient(blobName);
                await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);
                await sourceBlobClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in moving picture [{}] from {} to {}", blobName, sourceContainerName, destinationContainerName);
                throw;
            }
        }

        //public async Task<string> GetBlobSasUri(string containerName, string blobName)
        //{
        //    try
        //    {
        //        var blobServiceClient = new BlobServiceClient(_appSettings.StorageConnectionString);
        //        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        //        var blobClient = containerClient.GetBlobClient(blobName);
        //        var sasToken = await blobClient.GenerateSasUriAsync(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));
        //        return sasToken.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error in getting SAS URI");
        //        throw;
        //    }
        //}


    }
}
