namespace TheWag.Wasm.Util
{
    public class AppSettings
    {
        public string VisionKey { get; private set; }
        public string VisionEndpoint { get; private set; }
        public string ValidContainerName { get; private set; }
        public string InvalidContainerName { get; private set; }
        public string TempContainerName { get; private set; }
        public string StorageConnectionString { get; private set; }
        public string SendGridApiKey { get; private set; } 

        public AppSettings()
        {
            VisionKey = Environment.GetEnvironmentVariable("VISION_KEY") ?? throw new ArgumentNullException(nameof(VisionKey), "VISION_KEY environment variable is not set.");
            VisionEndpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT") ?? throw new ArgumentNullException(nameof(VisionEndpoint), "VISION_ENDPOINT environment variable is not set.");
            ValidContainerName = Environment.GetEnvironmentVariable("ValidContainer") ?? throw new ArgumentNullException(nameof(ValidContainerName), "ValidContainer environment variable is not set.");
            InvalidContainerName = Environment.GetEnvironmentVariable("InvalidContainer") ?? throw new ArgumentNullException(nameof(InvalidContainerName), "InvalidContainer environment variable is not set.");
            TempContainerName = Environment.GetEnvironmentVariable("TempContainer") ?? throw new ArgumentNullException(nameof(TempContainerName), "TempContainer environment variable is not set.");
           StorageConnectionString = Environment.GetEnvironmentVariable("StorageConnectionString") ?? throw new ArgumentNullException(nameof(StorageConnectionString), "StorageConnectionString environment variable is not set.");
            SendGridApiKey = Environment.GetEnvironmentVariable("Sendgrid_Key") ?? throw new ArgumentNullException(nameof(SendGridApiKey), "Sendgrid_Key environment variable is not set.");
        }
    }
}
