using Azure.Storage.Blobs;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TheWag.Functions
{
    public class Blob
    {
        private readonly ILogger<Blob> _logger;

        public Blob(ILogger<Blob> logger)
        {
            _logger = logger;
        }

        [Function("SaveDogPic")]
        public IActionResult Save([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            var file = req.Form.Files[0];
            Stream myBlob = new MemoryStream();
            myBlob = file.OpenReadStream();
            string Connection = Environment.GetEnvironmentVariable("StorageConnectionString");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var blobClient = new BlobContainerClient(Connection, containerName);
            var blob = blobClient.GetBlobClient(file.FileName);
            var r = blob.Upload(myBlob);

            return new OkObjectResult($"{blobClient.Uri}/{blob.Name}");
        }
    }
}
