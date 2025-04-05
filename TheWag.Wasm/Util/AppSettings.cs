namespace TheWag.Wasm.Util
{
    public class AppSettings
    {
        public string BlobHostUrl { get; private set; }
        public string FunctionHostUrl { get; private set; }
        public string ValidContainerName { get; private set; }
        public string InvalidContainerName { get; private set; }
        public string TempContainerName { get; private set; }
        public string CartSessionKey { get; private set; }

        public AppSettings(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            BlobHostUrl = configuration["BlobHostUrl"] ?? throw new ArgumentNullException(nameof(BlobHostUrl));
            FunctionHostUrl = configuration["FunctionHostUrl"] ?? throw new ArgumentNullException(nameof(FunctionHostUrl));
            ValidContainerName = configuration["ValidContainer"] ?? throw new ArgumentNullException(nameof(ValidContainerName));
            InvalidContainerName = configuration["InvalidContainer"] ?? throw new ArgumentNullException(nameof(InvalidContainerName));
            TempContainerName = configuration["TempContainer"] ?? throw new ArgumentNullException(nameof(TempContainerName));
            CartSessionKey = configuration["CartSessionKey"] ?? throw new ArgumentNullException(nameof(CartSessionKey));
        }
    }
}
