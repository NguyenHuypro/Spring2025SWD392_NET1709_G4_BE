using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly IEmailService _emailService;

        public AuthService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }
        public async Task<ResponseDTO> Login(LoginDTO loginDTO)
        {
            //kiểm tra người dùng
            var user = await _unitOfWork.UserRepo.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return new ResponseDTO("Không tìm thấy người dùng !!!", 404, false);
            }

           
            string password = "123";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            

            // kiểm tra mật khẩu
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.password);
            if (!isPasswordValid)
            {
                return new ResponseDTO("Sai password", 400, false);
            }

            if (!user.isActive)
            {
                return new ResponseDTO("Chưa confirm email", 200, false);
            }

            //kiểm tra refreshToken
            var exitsRefreshToken = await _unitOfWork.TokenRepo.GetRefreshTokenByUserID(user.id);
            if (exitsRefreshToken != null)
            {
                exitsRefreshToken.isRevoked = true;
                await _unitOfWork.TokenRepo.UpdateAsync(exitsRefreshToken);
            }

            //khởi tạo claim
            var claims = new List<Claim>
            {
                new Claim(JwtConstant.KeyClaim.Email, user.email),
                new Claim(JwtConstant.KeyClaim.userId, user.id.ToString()),
                new Claim(JwtConstant.KeyClaim.fullName, user.fullName),
                new Claim(JwtConstant.KeyClaim.Role, user.role.ToString())
            };

            //tạo refesh token
            var refreshTokenKey = JwtProvider.GenerateRefreshToken(claims);
            var accessTokenKey = JwtProvider.GenerateAccessToken(claims);

            var refreshToken = new RefreshToken
            {
                id = Guid.NewGuid(),
                userId = user.id,
                refreshTokenKey = refreshTokenKey,
                isRevoked = false,
                createdAt = DateTime.UtcNow
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

            return new ResponseDTO("Đăng nhập thành công", 200, true, new
            {
                AccessToken = accessTokenKey,
                RefeshToken = refreshTokenKey,
                UserID = user.id,
                FullName = user.fullName,
                Email = user.email,
                Phone = user.phone,
                Role = user.role.ToString(),
            });
        }

        public Task<ResponseDTO> LogoutAsync(string refreshTokenKey)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> RefreshBothTokens(string oldAccessToken, string refreshTokenKey)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> Register(RegisterDTO registerDTO)
        {
            if (string.IsNullOrWhiteSpace(registerDTO.FullName) ||
                string.IsNullOrWhiteSpace(registerDTO.Email) ||
                string.IsNullOrWhiteSpace(registerDTO.Password) ||
                string.IsNullOrWhiteSpace(registerDTO.Phone))
            {
                return new ResponseDTO("điền đủ đi", 200, false);
            }

            if (registerDTO.Password != registerDTO.PasswordConfirmed)
            {
                return new ResponseDTO("password không trùng", 200, false);
            }

            try
            {
                var existingUser = await _unitOfWork.UserRepo.FindByEmailAsync(registerDTO.Email);
                if (existingUser != null)
                {
                    return new ResponseDTO("Email đã tồn tại", 200, false);
                }

                string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password, salt);

                var newUser = new User
                {
                    id = Guid.NewGuid(),
                    fullName = registerDTO.FullName,
                    email = registerDTO.Email,
                    password = hashedPassword,
                    passwordSalt = salt,
                    phone = registerDTO.Phone,
                    role = RoleType.CUSTOMER,
                    isActive = false
                };
                await _unitOfWork.UserRepo.AddAsync(newUser);
                await _unitOfWork.SaveAsync();

                // 🔹 Tạo token xác nhận email
                string token = GenerateEmailToken(newUser.email);

                // 🔹 Gửi email xác nhận
                await _emailService.SendConfirmationEmailAsync(newUser.email, token);

                return new ResponseDTO("Đăng ký thành công.", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"lỗi: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> RegisterAdminAsync(CreateStaffDTO createStaffDTO)
        {
            //// Validate the input fields
            //if (string.IsNullOrWhiteSpace(createStaffDTO.email) ||
            //    string.IsNullOrWhiteSpace(createStaffDTO.fullName) ||
            //    string.IsNullOrWhiteSpace(createStaffDTO.password) ||
            //    string.IsNullOrWhiteSpace(createStaffDTO.phone) ||
            //    string.IsNullOrWhiteSpace(createStaffDTO.role)) // Kiểm tra role luôn
            //{
            //    return new ResponseDTO("All fields are required.", 400, false);
            //}

            try
            {
                // Check if the email already exists
                var existingUser = await _unitOfWork.UserRepo.FindByEmailAsync(createStaffDTO.email);

                

                if (existingUser != null)
                {
                    return new ResponseDTO("Email đã có người đăng ký !!!", 200, false);
                }

                var checkPhone = await _unitOfWork.UserRepo.CheckTelephone(createStaffDTO.phone);

                if (checkPhone != null)
                {
                    return new ResponseDTO("số điện thoại đã có người đăng ký !!!", 200, false);
                }


                // Generate salt and hash the password
                string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(createStaffDTO.password, salt);

                // Create a new User
                var newUser = new User
                {
                    id = Guid.NewGuid(),
                    fullName = createStaffDTO.fullName,
                    email = createStaffDTO.email,
                    password = hashedPassword,
                    passwordSalt = salt,
                    phone = createStaffDTO.phone,
                    role = createStaffDTO.role == "STAFF" ? RoleType.STAFF : RoleType.RECEPTIONIST,
                    staffStatus = staffStatus.ACTIVE ,
                    isActive = true,
                    rescueStationId = createStaffDTO.rescueStation ?? null

                };
                var response = new CreateStaffDTO
                {
                    email = newUser.email,
                    fullName = newUser.fullName,
                    phone = newUser.phone,
                    role = newUser.role.ToString(),
                };

                // Add the new user to the repository
                await _unitOfWork.UserRepo.AddAsync(newUser);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("Staff registered successfully.", 200, true, response);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error in RegisterAdminAsync: {ex}");

                return new ResponseDTO("An unexpected error occurred. Please try again later.", 500, false);
            }
        }

        

        public async Task<ResponseDTO> ConfirmEmail(string email, string token)
        {
            var user = await _unitOfWork.UserRepo.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResponseDTO("User not found.", 200, false);
            }

            // Kiểm tra token có hợp lệ không
            if (!ValidateEmailToken(email, token))
            {
                return new ResponseDTO("Invalid or expired token.", 200, false);
            }

            // Cập nhật trạng thái xác nhận email
            user.isActive = true;
            _unitOfWork.UserRepo.UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            return new ResponseDTO("Email confirmed successfully. You can now log in.", 200, true);
        }

        private string GenerateEmailToken(string email)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes("super-secret-key"));
            return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(email + DateTime.UtcNow.Date)));
        }

        private bool ValidateEmailToken(string email, string token)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes("super-secret-key"));
            string expectedToken = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(email + DateTime.UtcNow.Date)));

            return expectedToken == token;
        }

    }
}
