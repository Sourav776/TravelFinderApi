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
        public async Task<string> GetMultiLocationTemperature(bool isRanking, string latitudes, string longitudes, DateTime? startDate = null, int days = 7)
        {
            string url = $"{ApplicationConstants.Get<string>("TemperatureBaseUrl")}forecast?" +
             $"latitude={latitudes}&longitude={longitudes}&" +
             $"{(isRanking
                 ? $"forecast_days={days}"
                 : $"start_date={startDate?.ToString("yyyy-MM-dd")}&end_date={startDate?.AddDays(days - 1).ToString("yyyy-MM-dd")}")}" +
             $"&hourly=temperature_2m&timezone=auto";

            var response = await _httpClient.GetStringAsync(url);
            return response;
        }
        public async Task<string> GetMultiLocationAirQuality(bool isRanking, string latitudes, string longitudes, DateTime? startDate = null, int days = 7)
        {
            string url = $"{ApplicationConstants.Get<string>("AirQualityBaseUrl")}air-quality?" +
                $"latitude={latitudes}&longitude={longitudes}&" +
                $"{(isRanking
                 ? $"forecast_days={days}"
                 : $"start_date={startDate?.ToString("yyyy-MM-dd")}&end_date={startDate?.AddDays(days - 1).ToString("yyyy-MM-dd")}")}" +
                $"&hourly=pm2_5&timezone=auto";
            var response = await _httpClient.GetStringAsync(url);
            return response;
        }
    }
}
