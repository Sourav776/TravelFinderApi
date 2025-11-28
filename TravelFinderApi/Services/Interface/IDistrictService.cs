using TravelFinderApi.Models;

namespace TravelFinderApi.Services.Interface
{
    public interface IDistrictService
    {
        Task<List<District>> GetDistrictsAsync();
        Task CacheDistrictsAsync();
    }
}
