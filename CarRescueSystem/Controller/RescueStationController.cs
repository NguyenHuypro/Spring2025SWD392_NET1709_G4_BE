using CarRescueSystem.BLL.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CarRescueSystem.Controller
{
    [Route("api/stations")]
    [ApiController]
    public class RescueStationController : ControllerBase
    {
        private readonly IRescueStationService _rescueStationService;

        public RescueStationController(IRescueStationService rescueStationService)
        {
            _rescueStationService = rescueStationService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllRescueStation()
        {
            var response = await _rescueStationService.GetAllRescueStation();
            return StatusCode(response.StatusCode, response);
        }


    }
}
