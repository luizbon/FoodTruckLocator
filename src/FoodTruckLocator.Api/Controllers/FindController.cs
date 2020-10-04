using System;
using System.Threading.Tasks;
using FoodTruckLocator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodTruckLocator.Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class FindController: ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<FindController> _logger;

        public FindController(ISearchService searchService, ILogger<FindController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(double lat, double lng)
        {
            if (lat < -90 || lat > 90)
            {
                return BadRequest("Invalid latitude");
            }

            if (lng < -180 || lng > 80)
            {
                return BadRequest("Invalid longitude");
            }

            try
            {
                return Ok(await _searchService.FindAsync(lat, lng));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to find");
                return Problem();
            }
        }
    }
}
