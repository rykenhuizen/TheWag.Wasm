using Azure;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Extensions.Logging;
using TheWag.Functions.Endpoints;
using TheWag.Models;
using TheWag.Wasm.Util;

namespace Util.Azure.ComputerVision
{
    public class ComputerVisionService
    {
        private readonly ImageAnalysisClient _client;
        private readonly ILogger<Checkout> _logger;

        public ComputerVisionService(ILogger<ComputerVisionService> logger, AppSettings appSettings)
        {
                _client = new ImageAnalysisClient(
                new Uri(appSettings.VisionEndpoint),
                new AzureKeyCredential(appSettings.VisionKey));
        }

        public ImageAnalysisResults GetImageAnalysis(BinaryData imageData)
        {
            VisualFeatures visualFeatures =
                VisualFeatures.Caption |
                VisualFeatures.Tags |
                VisualFeatures.SmartCrops;

            var options = new ImageAnalysisOptions
            {
                GenderNeutralCaption = true,
                Language = "en",
                SmartCropsAspectRatios = new float[] { 0.9F, 1.33F }
            };

            ImageAnalysisResult result = _client.Analyze(
                                        imageData,
                                        visualFeatures,
                                        options);


            var results = new ImageAnalysisResults()
            {
                Description = result.Caption.Text,
                Tags = result.Tags.Values.Select(t => t.Name).ToList(),
                //CropRegions = result.SmartCrops.Values
            };

            return results;

        }
    }
}
