using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using TravelFinderApi.Helpers;
using TravelFinderApi.Models;
using TravelFinderApi.Models.Dtos;
using TravelFinderApi.Services.Interface;

namespace TravelFinderApi.Services.Implementation
{
    public class WeatherService : IWeatherService
    {
        private const string _top10CacheKey = "top-10-districts";
        private readonly IDistrictService _districtService;
        private readonly OpenMeteoClient _openMeteoClient;
        private readonly IMemoryCache _cache;
        public WeatherService(IDistrictService districtService, OpenMeteoClient openMeteoClient, IMemoryCache cache)
        {
            _districtService = districtService;
            _openMeteoClient = openMeteoClient;
            _cache = cache;
        }
        public async Task CacheTopDistrictsAsync()
        {
            var rankedDistricts = await GetTopDistrictsAsync();
            _cache.Set(_top10CacheKey, rankedDistricts, TimeSpan.FromHours(1));
        }
        public async Task<List<DistrictRankingDto>> GetTopDistrictsAsync()
        {
            if (_cache.TryGetValue(_top10CacheKey, out List<DistrictRankingDto> cached))
                return cached;

            var districts = await _districtService.GetDistrictsAsync();
            string latQuery = string.Join(",", districts.Select(d => d.Lat));
            string longQuery = string.Join(",", districts.Select(d => d.Long));

            var temperatureTask = _openMeteoClient.GetMultiLocationTemperature(latQuery, longQuery, DateTime.Today.AddDays(1));
            var airQualityTask = _openMeteoClient.GetMultiLocationAirQuality(latQuery, longQuery);

            await Task.WhenAll(temperatureTask, airQualityTask);

            var temperatureJson = JsonDocument.Parse(temperatureTask.Result);
            var airQualityJson = JsonDocument.Parse(airQualityTask.Result);

            List<DistrictRankingDto> rankedDistricts = RankDistricts(districts, temperatureJson, airQualityJson);
            var top10Districts = rankedDistricts.OrderBy(x => x.AverageTemp2PM).ThenBy(x => x.AverageAirPM25).Take(10).ToList();
            _cache.Set(_top10CacheKey, districts, TimeSpan.FromHours(1));
            return top10Districts;
        }

        private List<DistrictRankingDto> RankDistricts(List<District> districts, JsonDocument temperatureJson, JsonDocument airQualityJson)
        {
            var result = new List<DistrictRankingDto>();

            for (int i = 0; i < districts.Count; i++)
            {
                var curLocationTemp = temperatureJson.RootElement[i];
                var curLocationAir = airQualityJson.RootElement[i];
                var tempAt2pm = GetTemperatureValues(curLocationTemp);
                var airPm25 = GetAirQualityValues(curLocationAir);

                result.Add(new DistrictRankingDto
                (
                    districts[i].Name,
                    Math.Round(tempAt2pm.Average(), 2),
                    Math.Round(airPm25.Average(), 2)
                ));
            }
            return result;
        }

        public async Task<RecommendDto> CompareTravelAsync(TravelRequest req)
        {
            var districts = await _districtService.GetDistrictsAsync();
            var dest = districts.FirstOrDefault(d => d.Name.Equals(req.DestinationDistrict, StringComparison.OrdinalIgnoreCase));

            if (dest == null)
            {
                return new RecommendDto
                (
                   "Not Recommended",
                   "Destination district not found."
               );
            }
            string latQuery = $"{req.CurLatitude},{dest.Lat}";
            string longQuery = $"{req.CurLongitude},{dest.Long}";

            var temperatureTask = _openMeteoClient.GetMultiLocationTemperature(latQuery, longQuery, req.TravelDate, 1);
            var airQualityTask = _openMeteoClient.GetMultiLocationAirQuality(latQuery, longQuery);

            await Task.WhenAll(temperatureTask, airQualityTask);

            var temperatureJson = JsonDocument.Parse(temperatureTask.Result);
            var airQualityJson = JsonDocument.Parse(airQualityTask.Result);

            var curLocationTemp = GetTemperatureValues(temperatureJson.RootElement[0]);
            var curLocationAir = GetAirQualityValues(airQualityJson.RootElement[0]);
            var destTemp = GetTemperatureValues(temperatureJson.RootElement[1]);
            var destAir = GetAirQualityValues(airQualityJson.RootElement[1]);

            string reason = string.Empty;
            if (destTemp.Average() < curLocationTemp.Average() && destAir.Average() < curLocationAir.Average())
            {
                reason = $"Your destination is {Math.Round((curLocationTemp.Average() - destTemp.Average()), 2)}°C cooler and has significantly better air quality. Enjoy your trip!";
                return new RecommendDto("Recommended", reason);
            }
            else
            {
                reason = $"Your destination is hotter and has worse air quality than your current location. It’s better to stay where you are.";
                return new RecommendDto("Not Recommended", reason);
            }
        }
        private List<double> GetAirQualityValues(JsonElement curLocationAir)
        {
            if (!curLocationAir.TryGetProperty("hourly", out var hourly))
                return new List<double>();
            if (!hourly.TryGetProperty("pm2_5", out var pmArray))
                return new List<double>();

            var list = pmArray.EnumerateArray().Where(x => x.ValueKind == JsonValueKind.Number)
                .Select(x => x.GetDouble()).ToList();
            return list;
        }

        private List<double> GetTemperatureValues(JsonElement curLocationTemp)
        {
            if (!curLocationTemp.TryGetProperty("hourly", out var hourly))
                return new List<double>();

            if (!hourly.TryGetProperty("temperature_2m", out var tempsJson) ||
                !hourly.TryGetProperty("time", out var timesJson))
                return new List<double>();

            var temps = tempsJson.EnumerateArray();
            var times = timesJson.EnumerateArray();

            var tempList = new List<double>();

            while (temps.MoveNext() && times.MoveNext())
            {
                var time = DateTime.Parse(times.Current.GetString());
                if (time.Hour == 14)
                    tempList.Add(temps.Current.ValueKind == JsonValueKind.Number && temps.Current.TryGetDouble(out double temp) ? temp : 0.0);
            }
            return tempList;
        }
    }
}
