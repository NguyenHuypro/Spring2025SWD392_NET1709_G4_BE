using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarRescueSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class servicesController : ControllerBase
    {
        private readonly IServiceRescueService _serviceRescueService;

        public servicesController(IServiceRescueService serviceRescueService)
        {
            _serviceRescueService = serviceRescueService;
        }

        /// <summary>
        /// Lấy danh sách tất cả dịch vụ
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetAllServices()
        {
            var response = await _serviceRescueService.GetAllService();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Lấy chi tiết dịch vụ theo ID
        /// </summary>
        [HttpGet("{serviceId}")]
        public async Task<IActionResult> GetServiceById(Guid serviceId)
        {
            var response = await _serviceRescueService.GetServiceById(serviceId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Tạo một dịch vụ mới
        /// </summary>
        [HttpPost()]
        public async Task<IActionResult> CreateService([FromBody] ServiceDTO serviceDTO)
        {
            var response = await _serviceRescueService.CreateService(serviceDTO);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Cập nhật thông tin dịch vụ
        /// </summary>
        [HttpPut("{serviceId}")]
        public async Task<IActionResult> UpdateService(Guid serviceId, [FromBody] ServiceDTO serviceDTO)
        {
            var response = await _serviceRescueService.UpdateService(serviceId, serviceDTO);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Xóa dịch vụ theo ID
        /// </summary>
        [HttpDelete("{serviceId}")]
        public async Task<IActionResult> DeleteService(Guid serviceId)
        {
            var response = await _serviceRescueService.DeleteService(serviceId);
            return StatusCode(response.StatusCode, response);
        }

        // /api/services/package/:packageId method: GET
        /// <summary>
        /// Lấy danh sách dịch vụ theo packageId
        /// </summary>
        [HttpGet("package/{packageId}")]
        public async Task<IActionResult> GetServicesByPackageId(Guid packageId)
        {
            var response = await _serviceRescueService.GetServicesByPackageId(packageId);
            return StatusCode(response.StatusCode, response);
        }


    }
}
