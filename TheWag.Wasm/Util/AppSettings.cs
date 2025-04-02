namespace TheWag.Wasm.Util
{
    public class AppSettings
    {
        public string BlobHostUrl { get; private set; }
        public string FunctionHostUrl { get; private set; }
        public string ContainerName { get; private set; }
        public string CartSessionKey { get; private set; }

        public AppSettings(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            BlobHostUrl = configuration["BlobHostUrl"] ?? throw new ArgumentNullException(nameof(BlobHostUrl));
            FunctionHostUrl = configuration["FunctionHostUrl"] ?? throw new ArgumentNullException(nameof(FunctionHostUrl));
            ContainerName = configuration["ContainerName"] ?? throw new ArgumentNullException(nameof(ContainerName));
            CartSessionKey = configuration["CartSessionKey"] ?? throw new ArgumentNullException(nameof(CartSessionKey));
        }
    }
}
