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

        public AuthService(IUnitOfWork unitOfWork)
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

            Console.WriteLine($"Stored Hash: {user.password}");
            string password = "123";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            Console.WriteLine($"New Hashed Password: {hashedPassword}");

            // kiểm tra mật khẩu
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.password);
            if (!isPasswordValid)
            {
                return new ResponseDTO("Invalid email or password.", 400, false);
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

            return new ResponseDTO("Login successful", 200, true, new
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
                return new ResponseDTO("All fields are required.", 400, false);
            }

            if (registerDTO.Password != registerDTO.PasswordConfirmed)
            {
                return new ResponseDTO("Passwords do not match.", 400, false);
            }

            try
            {
                var existingUser = await _unitOfWork.UserRepo.FindByEmailAsync(registerDTO.Email);
                if (existingUser != null)
                {
                    return new ResponseDTO("Email already exists.", 400, false);
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
                    role = RoleType.CUSTOMER
                };
                await _unitOfWork.UserRepo.AddAsync(newUser);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("User registered successfully.", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
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
                    return new ResponseDTO("Email already exists.", 400, false);
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
                    staffStatus = staffStatus.ACTIVE 
                };

                // Add the new user to the repository
                await _unitOfWork.UserRepo.AddAsync(newUser);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO("Staff registered successfully.", 200, true);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error in RegisterAdminAsync: {ex}");

                return new ResponseDTO("An unexpected error occurred. Please try again later.", 500, false);
            }
        }


    }
}
