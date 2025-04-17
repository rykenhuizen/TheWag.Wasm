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

        public AppSettings(IConfiguration configuration, ILogger<AppSettings> logger)
        {
            ArgumentNullException.ThrowIfNull(logger);
            try
            {
                ArgumentNullException.ThrowIfNull(configuration);
          
                BlobHostUrl = configuration["BlobHostUrl"] ?? throw new KeyNotFoundException(nameof(BlobHostUrl));
                FunctionHostUrl = configuration["FunctionHostUrl"] ?? throw new KeyNotFoundException(nameof(FunctionHostUrl));
                ValidContainerName = configuration["ValidContainer"] ?? throw new KeyNotFoundException(nameof(ValidContainerName));
                InvalidContainerName = configuration["InvalidContainer"] ?? throw new KeyNotFoundException(nameof(InvalidContainerName));
                TempContainerName = configuration["TempContainer"] ?? throw new KeyNotFoundException(nameof(TempContainerName));
                CartSessionKey = configuration["CartSessionKey"] ?? throw new KeyNotFoundException(nameof(CartSessionKey));
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex, "Configuration is null");
                throw;
            }
        }
  
    }
}
