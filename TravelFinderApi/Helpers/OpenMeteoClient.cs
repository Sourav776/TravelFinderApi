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
        public async Task<string> GetMultiLocationTemperature(string latitudes, string longitudes, DateTime? startDate, int days = 7)
        {
            string url = $"{ApplicationConstants.Get<string>("TemperatureBaseUrl")}" +
                $"forecast?latitude={latitudes}&longitude={longitudes}&start_date={startDate?.ToString("yyyy-MM-dd")}" +
                $"&end_date={startDate?.AddDays(-1 + days).ToString("yyyy-MM-dd")}&hourly=temperature_2m&timezone=auto";
            var response = await _httpClient.GetStringAsync(url);
            return response;
        }
        public async Task<string> GetMultiLocationAirQuality(string latitudes, string longitudes, int days = 7)
        {
            string url = $"{ApplicationConstants.Get<string>("AirQualityBaseUrl")}" +
                $"air-quality?latitude={latitudes}&longitude={longitudes}&forecast_days={days}&hourly=pm2_5&timezone=auto";
            var response = await _httpClient.GetStringAsync(url);
            return response;
        }
    }
}
