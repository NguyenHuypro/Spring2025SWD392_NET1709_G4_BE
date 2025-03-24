using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IAuthService 
    {
        Task<ResponseDTO> Login(LoginDTO loginDTO);
        Task<ResponseDTO> Register(RegisterDTO registerDTO);
        Task<ResponseDTO> RefreshBothTokens(string oldAccessToken, string refreshTokenKey);
        Task<ResponseDTO> LogoutAsync(string refreshTokenKey);

        Task<ResponseDTO> RegisterAdminAsync(CreateStaffDTO createStaffDTO);
        Task<ResponseDTO> ConfirmEmail(string email, string token);
    }
}
