using System.ComponentModel;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TheWag.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TheWag.Functions
{
    public class Blob
    {
        private readonly ILogger<Blob> _logger;
        private readonly string _connection;
        private readonly string _validContainer;
        private readonly string _invalidContainer;
        private readonly string _tempContainer;
        //private readonly BlobContainerClient _blobClient;

        public Blob(ILogger<Blob> logger)
        {
            _logger = logger;
            _connection = Environment.GetEnvironmentVariable("StorageConnectionString") ?? throw new ArgumentNullException(nameof(_connection), "StorageConnectionString environment variable is not set.");
            _validContainer = Environment.GetEnvironmentVariable("ValidContainer") ?? throw new ArgumentNullException(nameof(_validContainer), "ValidContainer environment variable is not set.");
            _invalidContainer = Environment.GetEnvironmentVariable("InvalidContainer") ?? throw new ArgumentNullException(nameof(_invalidContainer), "InvalidContainer environment variable is not set.");
            _tempContainer = Environment.GetEnvironmentVariable("TempContainer") ?? throw new ArgumentNullException(nameof(_tempContainer), "TempContainer environment variable is not set.");
        }

        [Function("SaveTempPic")]
        public IActionResult Save([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            try
            {
                var file = req.Form.Files[0];
                SavePic(file, _tempContainer);

                return new OkObjectResult(file.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in saving TempPic");
                return new BadRequestObjectResult("Error in saving TempPic: " + ex.Message);
            }
        }

        [Function("SaveInvalidPic")]
        public IActionResult SaveInvalid([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            try
            {
                var file = req.Form.Files[0];
                SavePic(file, _invalidContainer);

                return new OkObjectResult(file.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in saving InvalidPic");
                return new BadRequestObjectResult("Error in saving InvalidPic: " + ex.Message);
            }
               
        }

        [Function("GetPicList")]
        public IActionResult Get([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function GetPicList called");
            var client = new BlobContainerClient(_connection, _validContainer);
            var blobs = client.GetBlobs();
            var blobNames = new List<string>();
            foreach (BlobItem blobItem in blobs)
            {
                blobNames.Add(blobItem.Name);
            }

            return new OkObjectResult(blobNames);
        }

        [Function("MoveTempPic")]
        public async Task<IActionResult> Move([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            try
            {
                var filename = req.ReadFromJsonAsync<string>().Result;
                //var body = (await new StreamReader(req.Body).ReadToEndAsync()).Trim();
                //var filename = JsonConvert.DeserializeObject(body);
                var bscl = new BlobServiceClient(_connection);
                var destinationContainer = bscl.GetBlobContainerClient(_validContainer);
                var destinationBlob = destinationContainer.GetBlobClient(filename);
                var sourceContainer = bscl.GetBlobContainerClient(_tempContainer);
                var sourceBlob = sourceContainer.GetBlobClient(filename);
                
                await destinationBlob.StartCopyFromUriAsync(sourceBlob.Uri);
                await sourceBlob.DeleteAsync();

                return new OkObjectResult(filename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in moving TempPic");
                return new BadRequestObjectResult("Error in moving TempPic: " + ex.Message);
            }
        }

        private void SavePic(IFormFile image, string container)
        {
            var client = new BlobContainerClient(_connection, container);
            using var myBlob = image.OpenReadStream();
            var blob = client.GetBlobClient(image.FileName);
            var r = blob.Upload(myBlob);
        }
    }
}
