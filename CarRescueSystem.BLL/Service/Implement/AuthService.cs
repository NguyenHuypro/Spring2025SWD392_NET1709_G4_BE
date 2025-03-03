using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.Constants;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.Common.Settings;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.UnitOfWork;
using Microsoft.Win32;


namespace CarRescueSystem.BLL.Service.Implement
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseDTO> Login(LoginDTO loginDTO)
        {

            //kiểm tra người dùng
            var user = await _unitOfWork.UserRepo.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return new ResponseDTO("User not found", 404, false);
            }
            
            Console.WriteLine($"Stored Hash: {user.PasswordHash}");
            string password = "123";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            Console.WriteLine($"New Hashed Password: {hashedPassword}");


            // kiểm tra mật khẩu
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return new ResponseDTO("Invalid email or password.", 400, false);
            }

            //kiểm tra refreshToken
            var exitsRefreshToken = await _unitOfWork.TokenRepo.GetRefreshTokenByUserID(user.UserId);
            if (exitsRefreshToken != null)
            {
                // nếu có thì thu hồi
                exitsRefreshToken.IsRevoked = true;
                await _unitOfWork.TokenRepo.UpdateAsync(exitsRefreshToken); // cập nhật
            }
            //khởi tạo claim
            var claims = new List<Claim>();

            //thêm email
            claims.Add(new Claim(JwtConstant.KeyClaim.Email, user.Email));

            //thêm id
            claims.Add(new Claim(JwtConstant.KeyClaim.userId, user.UserId.ToString()));

            //thêm name
            claims.Add(new Claim(JwtConstant.KeyClaim.fullName, user.FullName));

            //tạo refesh token
            var refreshTokenKey = JwtProvider.GenerateRefreshToken(claims);

            //tạo access token
            var accessTokenKey = JwtProvider.GenerateAccessToken(claims);

            //new refreshToken model
            var refreshToken = new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                UserId = user.UserId,
                RefreshTokenKey = refreshTokenKey,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };
            _unitOfWork.TokenRepo.Add(refreshToken);
            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error saving refresh token: {ex.Message}", 500, false);
            }
            // Kiểm tra RoleID hợp lệ trước
            if (user.RoleID == null)
            {
                return new ResponseDTO("User role is missing", 400, false, null);
            }

            // Truy vấn role từ database (đảm bảo phương thức là async)
            var role =  _unitOfWork.RoleRepo.GetByGuid(user.RoleID);

            if (role == null)
            {
                return new ResponseDTO("Role not found", 400, false, null);
            }

            var roleName = role.RoleName;

            // Trả response
            return new ResponseDTO("Login successful", 200, true, new
            {
                AccessToken = accessTokenKey,
                RefeshToken = refreshTokenKey,
                UserID = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Role = roleName,
            });


        }
        public async Task<ResponseDTO> Register (RegisterDTO registerDTO)
        {
            if (string.IsNullOrWhiteSpace(registerDTO.FullName))
            {
                return new ResponseDTO("FullName cannot be blank.", 400, false);
            }
            if (string.IsNullOrWhiteSpace(registerDTO.Email))
            {
                return new ResponseDTO("Email cannot be blank.", 400, false);
            }

            if (string.IsNullOrWhiteSpace(registerDTO.Password))
            {
                return new ResponseDTO("Password cannot be blank.", 400, false);
            }

            if (registerDTO.Password != registerDTO.PasswordConfirmed)
            {
                return new ResponseDTO("Passwords do not match.", 400, false);
            }
            if (string.IsNullOrWhiteSpace(registerDTO.Phone))
            {
                return new ResponseDTO("PhoneNumber cannot be blank.", 400, false);
            }
            try
            {
                var existingUser = await _unitOfWork.UserRepo.FindByEmailAsync(registerDTO.Email);
                if (existingUser != null) 
                {
                    return new ResponseDTO("Email already exists.", 400, false);
                }

                // Hash password
                string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password, salt);

                //create
                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    FullName = registerDTO.FullName,
                    Email = registerDTO.Email,
                    PasswordHash = hashedPassword,
                    PasswordSalt = salt,
                    PhoneNumber = registerDTO.Phone, // Thêm số điện thoại vào user
                    //RoleID = new Guid("C3DAB1C3-6D48-4B23-8369-2D1C9C828F22"),
                    RoleID = new Guid("A1A2A3A4-B5B6-C7C8-D9D0-E1E2E3E4E5E6")
                };
                //save
                await _unitOfWork.UserRepo.AddAsync(newUser);

                //save all
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("User registered successfully.", 200, true);

            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }

        }
        public async Task<ResponseDTO> RefreshBothTokens(string oldAccessToken, string refreshTokenKey)
        {
            // Kiểm tra tính hợp lệ của refresh token
            var claimsPrincipal = JwtProvider.Validation(refreshTokenKey);
            if (claimsPrincipal == null)
            {
                return new ResponseDTO("Invalid refresh token", 400, false);
            }

            // Lấy đối tượng RefreshToken từ refresh token Key
            var refreshTokenDTO = await _unitOfWork.TokenRepo.GetRefreshTokenByKey(refreshTokenKey);
            if (refreshTokenDTO == null || refreshTokenDTO.IsRevoked)
            {
                return new ResponseDTO("Refresh token not found or has been revoked", 403, false);
            }

            // Kiểm tra nếu refresh token đã hết hạn
            var tokenExpirationDate = refreshTokenDTO.CreatedAt.AddDays(JwtSettingModel.ExpireDayRefreshToken);
            if (DateTime.UtcNow > tokenExpirationDate)
            {
                return new ResponseDTO("Refresh token expired, please login again", 403, false);
            }

            // Lấy thông tin người dùng từ UserId
            var user = await _unitOfWork.UserRepo.GetByIdAsync(refreshTokenDTO.UserId);
            if (user == null)
            {
                return new ResponseDTO("User not found", 404, false);
            }

            // Khởi tạo danh sách claims
            var claims = new List<Claim>();

            // Thêm email vào claims
            claims.Add(new Claim(JwtConstant.KeyClaim.Email, user.Email));

            

            // Thêm UserId vào claims
            claims.Add(new Claim(JwtConstant.KeyClaim.userId, user.UserId.ToString()));


            // Tạo access token mới
            var newAccessToken = JwtProvider.GenerateAccessToken(claims);

            // Lưu refresh token mới vào database
            var newRefreshToken = new RefreshToken
            {
                RefreshTokenId = Guid.NewGuid(),
                UserId = user.UserId,
                RefreshTokenKey = refreshTokenKey,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow // Lưu thời gian tạo
            };

            // Xóa refresh token cũ
            _unitOfWork.TokenRepo.Delete(refreshTokenDTO);
            // Thêm refresh token mới
            _unitOfWork.TokenRepo.Add(newRefreshToken);
            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error refreshing tokens: {ex.Message}", 500, false);
            }

            return new ResponseDTO("Token refreshed successfully", 200, true);
        }
        public async Task<ResponseDTO> LogoutAsync(string refreshTokenKey)
        {
            // Tìm refresh token trong cơ sở dữ liệu
            var refreshToken = await _unitOfWork.TokenRepo.GetRefreshTokenByKey(refreshTokenKey);

            // Kiểm tra xem refresh token có tồn tại không
            if (refreshToken == null)
            {
                return new ResponseDTO("Refresh token not found", 404, false);
            }

            // Đánh dấu refresh token là đã thu hồi
            refreshToken.IsRevoked = true;
            _unitOfWork.TokenRepo.UpdateAsync(refreshToken); // Cập nhật trạng thái token

            try
            {
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error during logout: {ex.Message}", 500, false);
            }

            return new ResponseDTO("Logout successful", 200, true);
        }
    }
}
