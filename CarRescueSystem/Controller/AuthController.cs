using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRescueSystem.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Đăng nhập người dùng
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.Login(loginDTO);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Đăng ký người dùng
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            var response = await _authService.Register(registerDTO);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Làm mới cả AccessToken và RefreshToken
        /// </summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDTO tokenDTO)
        {
            // Kiểm tra thông tin token
            if (tokenDTO == null || string.IsNullOrWhiteSpace(tokenDTO.RefreshToken) || string.IsNullOrWhiteSpace(tokenDTO.AccessToken))
            {
                return BadRequest(new ResponseDTO("Access token and refresh token are required.", 400, false)); 
            }

            var response = await _authService.RefreshBothTokens(tokenDTO.AccessToken, tokenDTO.RefreshToken);

            if (response.IsSuccess)
            {
                return Ok(response); 
            }

       
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Đăng xuất và thu hồi RefreshToken
        /// </summary>
        
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            var response = await _authService.LogoutAsync(refreshToken);
            return StatusCode(response.StatusCode, response);
        }

        // /api/auth/admin/register method: POST
        /// <summary>
        /// Đăng ký admin hoặc nhân viên
        /// </summary>
        [HttpPost("admin/register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] CreateStaffDTO createStaffDTO)
        {
           

            var response = await _authService.RegisterAdminAsync(createStaffDTO);

            return StatusCode(response.StatusCode, response);
        }

    }
}
