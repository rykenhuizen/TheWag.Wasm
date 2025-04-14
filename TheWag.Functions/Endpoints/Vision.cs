
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Util.Azure.ComputerVision;

namespace TheWag.Functions.Endpoints
{
    public class Vision
    {
        private readonly ILogger<Vision> _logger;
        private readonly ComputerVisionService _visionClient;

        public Vision(ILogger<Vision> logger, ComputerVisionService cvc)
        {
            _logger = logger;
            _visionClient = cvc;
        }

        [Function("GetAnalysis")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("GetAnalysis called");

            var file = req.Form.Files[0];

            using (var ms = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(ms);
                var bd = new BinaryData(ms.ToArray());

                var imageAnalysis = _visionClient.GetImageAnalysis(bd);
                return new OkObjectResult(imageAnalysis);
            }
        }
    }
}
