using System.ComponentModel;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TheWag.Wasm.Util;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TheWag.Functions.Endpoints
{
    public class Blob
    {
        private readonly ILogger<Blob> _logger;
        private readonly AppSettings _appSettings;

        public Blob(ILogger<Blob> logger, AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        [Function("SaveTempPic")]
        public IActionResult Save([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            try
            {
                var file = req.Form.Files[0];
                SavePic(file, _appSettings.TempContainerName);

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
                SavePic(file, _appSettings.InvalidContainerName);

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
            var client = new BlobContainerClient(_appSettings.StorageConnectionString, _appSettings.ValidContainerName);
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
                var bscl = new BlobServiceClient(_appSettings.StorageConnectionString);
                var destinationContainer = bscl.GetBlobContainerClient(_appSettings.ValidContainerName);
                var destinationBlob = destinationContainer.GetBlobClient(filename);
                var sourceContainer = bscl.GetBlobContainerClient(_appSettings.TempContainerName);
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
            var client = new BlobContainerClient(_appSettings.StorageConnectionString, container);
            using var myBlob = image.OpenReadStream();
            var blob = client.GetBlobClient(image.FileName);
            var r = blob.Upload(myBlob);
        }
    }
}
