using System.Text.Json;
using TravelFinderApi.Configurations;
using TravelFinderApi.Models;
using TravelFinderApi.Services.Interface;

namespace TravelFinderApi.Services.Implementation
{
    public class DistrictService : IDistrictService
    {
        private readonly HttpClient _httpClient;
        public DistrictService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<District>> GetDistrictsAsync()
        {
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
            return list;
        }
    }
}
