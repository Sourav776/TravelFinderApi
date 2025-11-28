using TravelFinderApi.Configurations;

namespace TravelFinderApi.Helpers
{
    public class OpenMeteoClient
    {
        private readonly HttpClient _httpClient;
        public OpenMeteoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetMultiLocationTemperature(string latitudes, string longitudes)
        {
            var url = $"{ApplicationConstants.Get<string>("TemperatureBaseUrl")}forecast?latitude={latitudes}&longitude={longitudes}&hourly=temperature_2m&timezone=auto";
            var response = await _httpClient.GetStringAsync(url);
            return response;
        }
        public async Task<string> GetMultiLocationAirQuality(string latitudes, string longitudes)
        {
            var url = $"{ApplicationConstants.Get<string>("AirQualityBaseUrl")}air-quality?latitude={latitudes}&longitude={longitudes}&hourly=pm2_5&timezone=auto";
            var response = await _httpClient.GetStringAsync(url);
            return response;
        }
    }
}
