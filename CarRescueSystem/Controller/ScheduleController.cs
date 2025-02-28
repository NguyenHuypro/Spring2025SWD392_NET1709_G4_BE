using System;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CarRescueSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // Get all schedules
        [HttpGet("getAll")]  // Route is now 'getAll' for fetching all schedules
        public async Task<IActionResult> GetAllSchedules()
        {
            var response = await _scheduleService.GetAllSchedulesAsync();
            return StatusCode(response.StatusCode, response);
        }

        // Get schedules by specific date
        [HttpGet("getByDate")]  // Descriptive route for getting schedules by date
        public async Task<IActionResult> GetSchedulesByDate([FromQuery] DateTime date)
        {
            var response = await _scheduleService.GetSchedulesByDateAsync(date);
            return StatusCode(response.StatusCode, response);
        }

        // Get a specific schedule by ID
        [HttpGet("getById/{id:guid}")]  // Descriptive route for getting schedule by ID
        public async Task<IActionResult> GetScheduleById(Guid id)
        {
            var response = await _scheduleService.GetScheduleByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // Create a new schedule
        [HttpPost("create")]  // Descriptive route for creating a new schedule
        public async Task<IActionResult> CreateSchedule([FromBody] ScheduleDTO scheduleDto)
        {
            var response = await _scheduleService.CreateScheduleAsync(scheduleDto);
            return StatusCode(response.StatusCode, response);
        }

        // Update an existing schedule
        [HttpPut("update/{id:guid}")]  // Descriptive route for updating a schedule by ID
        public async Task<IActionResult> UpdateSchedule(Guid id, [FromBody] ScheduleDTO scheduleDto)
        {
            var response = await _scheduleService.UpdateScheduleAsync(id, scheduleDto);
            return StatusCode(response.StatusCode, response);
        }

        // Delete a schedule by ID
        [HttpDelete("delete/{id:guid}")]  // Descriptive route for deleting a schedule by ID
        public async Task<IActionResult> DeleteSchedule(Guid id)
        {
            var response = await _scheduleService.DeleteScheduleAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // Get schedules by staff ID
        [HttpGet("getByStaff")]  // Descriptive route for getting schedules by staff
        public async Task<IActionResult> GetSchedulesByStaff([FromQuery] Guid? staffId = null)
        {
            var response = await _scheduleService.GetSchedulesByStaffIdAsync(staffId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
