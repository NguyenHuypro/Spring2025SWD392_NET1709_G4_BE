using CarRescueSystem.BLL.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CarRescueSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPackageController : ControllerBase
    {
        private readonly IUserPackageService _userPackageService;

        public UserPackageController(IUserPackageService userPackageService)
        {
            _userPackageService = userPackageService;
        }

        /// <summary>
        /// Admin tạo package cho user
        /// </summary>
        [HttpPost("admin/create")]
        public async Task<IActionResult> CreatePackageForAdmin([FromQuery] Guid userId, [FromQuery] Guid packageId)
        {
            var result = await _userPackageService.CreatePackageForAdmin(userId, packageId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// User tự mua package
        /// </summary>
        [HttpPost("user/create")]
        public async Task<IActionResult> CreatePackageForUser([FromQuery] Guid packageId)
        {
            var result = await _userPackageService.CreatePackageForUser(packageId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
