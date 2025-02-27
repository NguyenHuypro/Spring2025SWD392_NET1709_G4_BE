using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
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
         //[Authorize(Roles = "Customer", "Receptionist")] // Customer tạo Car, Recep tạo Car khi nhận Emergency(Car không trong hệ thống)
        public async Task<IActionResult> CreateCar([FromBody] CreateVehicleDTO createVehicleDTO)
        {
            var response = await _vehicleService.CreateCar(createVehicleDTO);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<VehicleResponse>> GetById(Guid id)
        {
            var response = await _vehicleService.GetVehicleByIDAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<VehicleListResponse>> GetAll()
        {
            var response = await _vehicleService.GetAllVehicleAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<VehicleResponse>> Update(Guid id, [FromBody] UpdateVehicleDTO updateDto)
        {
            var response = await _vehicleService.UpdateAsync(id, updateDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<VehicleResponse>> Delete(Guid id)
        {
            var response = await _vehicleService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
} 