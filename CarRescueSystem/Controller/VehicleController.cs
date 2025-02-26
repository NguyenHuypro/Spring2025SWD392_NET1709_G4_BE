using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO.Vehicle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRescueSystem.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<VehicleResponse>> Create([FromBody] CreateVehicleDTO createDto)
        {
            var response = await _vehicleService.CreateAsync(createDto);
            if (!response.Success)
                return BadRequest(response);
            return CreatedAtAction(nameof(GetById), new { id = response.Data.VehicleId }, response);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<VehicleResponse>> GetById(Guid id)
        {
            var response = await _vehicleService.GetByIdAsync(id);
            if (!response.Success)
                return NotFound(response);
            return Ok(response);
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<VehicleListResponse>> GetAll()
        {
            var response = await _vehicleService.GetAllAsync();
            return Ok(response);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<VehicleResponse>> Update(Guid id, [FromBody] UpdateVehicleDTO updateDto)
        {
            var response = await _vehicleService.UpdateAsync(id, updateDto);
            if (!response.Success)
                return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<VehicleResponse>> Delete(Guid id)
        {
            var response = await _vehicleService.DeleteAsync(id);
            if (!response.Success)
                return NotFound(response);
            return Ok(response);
        }
    }
} 