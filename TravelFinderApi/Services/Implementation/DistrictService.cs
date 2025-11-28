using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using TravelFinderApi.Configurations;
using TravelFinderApi.Models;
using TravelFinderApi.Services.Interface;

namespace TravelFinderApi.Services.Implementation
{
    public class DistrictService : IDistrictService
    {
        private const string _districtsCacheKey = "bd-districts";
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        public DistrictService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task CacheDistrictsAsync()
        {
            var districts = await GetDistrictsAsync();
            _cache.Set(_districtsCacheKey, districts, TimeSpan.FromDays(15));
        }

        public async Task<List<District>> GetDistrictsAsync()
        {
            if (_cache.TryGetValue(_districtsCacheKey, out List<District> cached))
                return cached;

            var json = await _httpClient.GetStringAsync(ApplicationConstants.Get<string>("DistrictsUrl"));
            var doc = JsonDocument.Parse(json);
            var list = new List<District>();
            foreach (var d in doc.RootElement.GetProperty("districts").EnumerateArray())
            {
                list.Add(new District
                {
                    Id = d.GetProperty("id").GetString(),
                    DivisionId = d.GetProperty("division_id").GetString(),
                    Name = d.GetProperty("name").GetString(),
                    BanglaName = d.GetProperty("bn_name").GetString(),
                    Lat = d.GetProperty("lat").GetString(),
                    Long = d.GetProperty("long").GetString()
                });
            }
            _cache.Set(_districtsCacheKey, list, TimeSpan.FromDays(15));
            return list;
        }
    }
}
