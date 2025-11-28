using System.Text.Json;
using TravelFinderApi.Helpers;
using TravelFinderApi.Models;
using TravelFinderApi.Models.Dtos;
using TravelFinderApi.Services.Interface;

namespace TravelFinderApi.Services.Implementation
{
    public class WeatherService : IWeatherService
    {
        private readonly IDistrictService _districtService;
        private readonly OpenMeteoClient _openMeteoClient;
        public WeatherService(IDistrictService districtService, OpenMeteoClient openMeteoClient)
        {
            _districtService = districtService;
            _openMeteoClient = openMeteoClient;
        }
        public async Task<List<DistrictRankingDto>> GetTopDistrictsAsync()
        {
            var districts = await _districtService.GetDistrictsAsync();
            string latQuery = string.Join(",", districts.Select(d => d.Lat));
            string longQuery = string.Join(",", districts.Select(d => d.Long));

            var temperatureTask = _openMeteoClient.GetMultiLocationTemperature(latQuery, longQuery);
            var airQualityTask = _openMeteoClient.GetMultiLocationAirQuality(latQuery, longQuery);

            await Task.WhenAll(temperatureTask, airQualityTask);

            var temperatureJson = JsonDocument.Parse(temperatureTask.Result);
            var airQualityJson = JsonDocument.Parse(airQualityTask.Result);

            List<DistrictRankingDto> rankedDistricts = RankDistricts(districts, temperatureJson, airQualityJson);
            return rankedDistricts.OrderBy(x => x.AverageTemp2PM).ThenBy(x => x.AverageAirPM25).Take(10).ToList();
        }

        private List<DistrictRankingDto> RankDistricts(List<District> districts, JsonDocument temperatureJson, JsonDocument airQualityJson)
        {
            var temps = temperatureJson.RootElement.EnumerateArray();
            var airQualities = airQualityJson.RootElement.EnumerateArray();
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

        private List<double> GetAirQualityValues(JsonElement curLocationAir)
        {
            if (!curLocationAir.TryGetProperty("hourly", out var hourly))
                return new List<double>();
            if (!hourly.TryGetProperty("pm2_5", out var pmArray))
                return new List<double>();

            var pmValues = hourly.GetProperty("pm2_5").EnumerateArray();

            var list = pmArray.EnumerateArray().Select(x => x.TryGetDouble(out double value) ? value : double.NaN).ToList();
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
                    tempList.Add(temps.Current.TryGetDouble(out double temp) ? temp : double.NaN);
            }
            return tempList;
        }

    }
}
