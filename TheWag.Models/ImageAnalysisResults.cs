
namespace TheWag.Models
{
    public class ImageAnalysisResults
    {
        public required string Description { get; set; }
        public required IList<string> Tags { get; set; }
       // public IReadOnlyList<CropRegion>? CropRegions { get; set; }
    }
}
