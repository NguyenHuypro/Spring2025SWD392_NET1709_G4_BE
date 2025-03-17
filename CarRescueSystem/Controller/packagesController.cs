using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarRescueSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class packagesController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public packagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        /// <summary>
        /// Lấy danh sách tất cả các gói dịch vụ
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetAllPackages()
        {
            var response = await _packageService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Lấy thông tin gói dịch vụ theo ID
        /// </summary>
        [HttpGet("{packageId}")]
        public async Task<IActionResult> GetPackageById(Guid packageId)
        {
            var response = await _packageService.GetByIdAsync(packageId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Tạo mới một gói dịch vụ
        /// </summary>
        [HttpPost()]
        public async Task<IActionResult> CreatePackage([FromBody] PackageDTO packageDTO)
        {
            var response = await _packageService.AddAsync(packageDTO);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Cập nhật thông tin gói dịch vụ
        /// </summary>
        [HttpPut("update/{packageId}")]
        public async Task<IActionResult> UpdatePackage(Guid packageId, [FromBody] PackageDTO packageDTO)
        {
            var response = await _packageService.UpdateAsync(packageId, packageDTO);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Xóa gói dịch vụ theo ID
        /// </summary>
        [HttpDelete("delete/{packageId}")]
        public async Task<IActionResult> DeletePackage(Guid packageId)
        {
            var response = await _packageService.DeleteAsync(packageId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
