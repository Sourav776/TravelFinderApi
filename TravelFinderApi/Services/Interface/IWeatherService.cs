using TravelFinderApi.Models;
using TravelFinderApi.Models.Dtos;

namespace TravelFinderApi.Services.Interface
{
    public interface IWeatherService
    {
        Task<List<DistrictRankingDto>> GetTopDistrictsAsync();
        Task<RecommendDto> CompareTravelAsync(TravelRequest req);
    }
}
