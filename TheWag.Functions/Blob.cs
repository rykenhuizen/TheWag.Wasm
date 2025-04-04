using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace TheWag.Functions
{
    public class Blob
    {
        private readonly ILogger<Blob> _logger;
        private readonly BlobContainerClient _blobClient;

        public Blob(ILogger<Blob> logger)
        {
            _logger = logger;
            string connection = Environment.GetEnvironmentVariable("StorageConnectionString");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");
            _blobClient = new BlobContainerClient(connection, containerName);
        }

        [Function("SaveDogPic")]
        public IActionResult Save([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function SaveDogPic called");
            var file = req.Form.Files[0];
            Stream myBlob = new MemoryStream();
            myBlob = file.OpenReadStream();
            var blob = _blobClient.GetBlobClient(file.FileName);
            var r = blob.Upload(myBlob);

            return new OkObjectResult($"blob.Name");
        }

        [Function("GetPicList")]
        public IActionResult Get([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function GetPicList called");
            var blobs = _blobClient.GetBlobs();
            var blobNames = new List<string>();
            foreach (BlobItem blobItem in blobs)
            {
                blobNames.Add(blobItem.Name);
            }

            return new OkObjectResult(blobNames);
        }
    }
}
