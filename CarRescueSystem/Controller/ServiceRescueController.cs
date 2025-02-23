using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CarRescueSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRescueController : ControllerBase
    {
        private readonly IServiceRescueService _serviceRescueService;

        public ServiceRescueController(IServiceRescueService serviceRescueService)
        {
            _serviceRescueService = serviceRescueService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllServices()
        {
            var response = await _serviceRescueService.GetAllService();

            if (!response.IsSuccess)
            {
                return StatusCode(response.StatusCode, response);
            }

            return Ok(response);
        }
    }
}
