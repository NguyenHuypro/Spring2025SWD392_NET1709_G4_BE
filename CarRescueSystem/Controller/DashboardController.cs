using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;

namespace CarRescueSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var dashboardData = await _dashboardService.GetDashboardDataAsync();
            return Ok(dashboardData);
        }
    }
} 