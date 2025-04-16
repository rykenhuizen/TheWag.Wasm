using System.ComponentModel;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TheWag.Functions.Services;
using TheWag.Wasm.Util;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TheWag.Functions.Endpoints
{
    public class Blob
    {
        private readonly ILogger<Blob> _logger;
        private readonly AppSettings _appSettings;
        private readonly BlobService _BlobService;

        public Blob(ILogger<Blob> logger, AppSettings appSettings, BlobService blobService)
        {
            _logger = logger;
            _appSettings = appSettings; //TODO: Have the api caller pass these parameters 
            _BlobService = blobService;
        }

        [Function("SaveTempPic")]
        public IActionResult Save([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            try
            {
                var file = req.Form.Files[0];
                _BlobService.SavePic(file, _appSettings.TempContainerName);

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
                _BlobService.SavePic(file, _appSettings.InvalidContainerName);

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
            try
            {
                var blobNames = _BlobService.GetFileList(_appSettings.ValidContainerName);
                return new OkObjectResult(blobNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in getting picture list");
                return new BadRequestObjectResult("Error in getting picture list: " + ex.Message);
            }
        }

        [Function("MoveTempPic")]
        public async Task<IActionResult> Move([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            try
            {
                var filename = await req.ReadFromJsonAsync<string>();
                if (string.IsNullOrEmpty(filename))
                {
                    throw new ArgumentException("Filename cannot be null or empty", nameof(req));
                }

                await _BlobService.MovePic(filename, _appSettings.TempContainerName, _appSettings.ValidContainerName);
                return new OkObjectResult(filename);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in moving TempPic");
                return new BadRequestObjectResult("Error in moving TempPic: " + ex.Message);
            }
        }
    }
}
