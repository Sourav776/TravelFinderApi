using Microsoft.AspNetCore.Mvc;
using TravelFinderApi.Services.Interface;

namespace TravelFinderApi.Controllers
{
    public class RankingController : BaseApiController
    {
        private readonly IDistrictService _districtService;
        public RankingController(IDistrictService districtService)
        {
            _districtService = districtService;
        }
        [HttpGet]
        public async Task<IActionResult> GetDistricts()
        {
            return Ok(await _districtService.GetDistrictsAsync());
        }
    }
}
