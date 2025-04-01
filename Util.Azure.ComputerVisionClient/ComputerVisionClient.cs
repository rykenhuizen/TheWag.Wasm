using Azure;
using Azure.AI.Vision.ImageAnalysis;
using TheWag.Models;

namespace Util.Azure.ComputerVision
{
    public class ComputerVisionClient
    {
        private readonly ImageAnalysisClient _client;
        //private readonly string? endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");
        //private readonly string? key = Environment.GetEnvironmentVariable("VISION_KEY");
        private readonly string? endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");
        private readonly string? key = Environment.GetEnvironmentVariable("VISION_KEY");
        public ComputerVisionClient()
        {
            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
            {
                throw new Exception("Missing VISION_ENDPOINT or VISION_KEY");
            }
            else
            {
                _client = new ImageAnalysisClient(
                new Uri(endpoint),
                new AzureKeyCredential(key));
            }   
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
