using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

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
        public async Task<IActionResult> CreatePackage([FromBody] AddPackageDTO dto)
        {
            var response = await _packageService.AddAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Cập nhật thông tin gói dịch vụ
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage([FromRoute] Guid id, [FromBody] PackageDTO? packageDTO)
        {
            //// Ghi log toàn bộ thông tin của DTO
            //Console.WriteLine($"Nhận id: {id}");
            //Console.WriteLine("Nhận packageDTO: " + JsonConvert.SerializeObject(packageDTO, Formatting.Indented));



            var response = await _packageService.UpdateAsync(id, packageDTO);
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
