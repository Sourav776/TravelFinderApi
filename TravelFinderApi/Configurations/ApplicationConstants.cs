namespace TravelFinderApi.Configurations
{
    public class ApplicationConstants
    {
        private static IConfiguration _configuration;
        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static T Get<T>(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var result = _configuration.GetSection(key).Get<T>();
                return result;
            }
            return default;
        }
    }
}
