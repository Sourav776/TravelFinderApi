using Microsoft.AspNetCore.Mvc;
using TravelFinderApi.Models;
using TravelFinderApi.Services.Interface;

namespace TravelFinderApi.Controllers
{
    public class TravelController : BaseApiController
    {
        private readonly IWeatherService _weatherService;
        public TravelController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        [HttpPost]
        public async Task<IActionResult> Recommend([FromBody] TravelRequest req)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(await _weatherService.CompareTravelAsync(req));
        }
    }
}
