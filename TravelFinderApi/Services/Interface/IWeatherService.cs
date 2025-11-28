using TravelFinderApi.Models.Dtos;

namespace TravelFinderApi.Services.Interface
{
    public interface IWeatherService
    {
        Task<List<DistrictRankingDto>> GetTopDistrictsAsync();
    }
}
