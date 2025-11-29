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

            return await SafeGetAsync(url);
        }
        public async Task<string> GetMultiLocationAirQuality(bool isRanking, string latitudes, string longitudes, DateTime? startDate = null, int days = 7)
        {
            string url = $"{ApplicationConstants.Get<string>("AirQualityBaseUrl")}air-quality?" +
                $"latitude={latitudes}&longitude={longitudes}&" +
                $"{(isRanking
                 ? $"forecast_days={days}"
                 : $"start_date={startDate?.ToString("yyyy-MM-dd")}&end_date={startDate?.AddDays(days - 1).ToString("yyyy-MM-dd")}")}" +
                $"&hourly=pm2_5&timezone=auto";

            return await SafeGetAsync(url);
        }

        private async Task<string> SafeGetAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return $"success: false, error: {response.StatusCode} : {response.ReasonPhrase}";
                }
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"success: false, error: Unexpected error: {ex.Message}";
            }
        }

    }
}
