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
        //[Authorize(Roles = "Customer, Receptionist")] // Customer tạo xe, Receptionist tạo xe khi nhận Emergency
        public async Task<IActionResult> CreateCar([FromBody] CreateVehicleDTO createVehicleDTO)
        {
            var response = await _vehicleService.CreateCar(createVehicleDTO);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _vehicleService.GetVehicleByIDAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _vehicleService.GetAllVehicleAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleDTO updateDto)
        {
            var response = await _vehicleService.UpdateAsync(id, updateDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _vehicleService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // Mua gói dịch vụ cho xe
        [HttpPost("purchase-package")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> PurchasePackage(Guid vehicleId, Guid packageId)
        {
            var response = await _vehicleService.PurchasePackage(vehicleId, packageId);
            return StatusCode(response.StatusCode, response);
        }

        // Lấy thông tin gói dịch vụ của xe
        [HttpGet("package/{vehicleId}")]
        public async Task<IActionResult> GetVehiclePackage(Guid vehicleId)
        {
            var response = await _vehicleService.GetVehiclePackage(vehicleId);
            return StatusCode(response.StatusCode, response);
        }

        // Xóa gói dịch vụ khỏi xe
        [HttpDelete("remove-package/{vehicleId}")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemovePackage(Guid vehicleId)
        {
            var response = await _vehicleService.RemovePackage(vehicleId);
            return StatusCode(response.StatusCode, response);
        }

        // Nâng cấp gói dịch vụ cho xe
        [HttpPut("upgrade-package")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpgradePackage(Guid vehicleId, Guid newPackageId)
        {
            var response = await _vehicleService.UpgradePackage(vehicleId, newPackageId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
