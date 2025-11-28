using Microsoft.AspNetCore.Mvc;
using TravelFinderApi.Services.Interface;

namespace TravelFinderApi.Controllers
{
    public class RankingController : BaseApiController
    {
        private readonly IWeatherService _weatherService;
        public RankingController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        [HttpGet]
        public async Task<IActionResult> GetTop10()
        {
            return Ok(await _weatherService.GetTopDistrictsAsync());
        }
    }
}
