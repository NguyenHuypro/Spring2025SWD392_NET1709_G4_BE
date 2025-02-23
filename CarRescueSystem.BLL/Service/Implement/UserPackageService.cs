using System;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.BLL.Utilities;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;

using CarRescueSystem.DAL.Repository.Interface;
using CarRescueSystem.DAL.UnitOfWork;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class UserPackageService : IUserPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserUtility _userUtility;
        private const int ADDITIONAL_QUANTITY = 50; // Mỗi lần add mới +50 lần sử dụng

        public UserPackageService(IUnitOfWork unitOfWork, UserUtility userUtility)
        {
            _unitOfWork = unitOfWork;
            _userUtility = userUtility;
        }

        // Tạo package chung
        private async Task<ResponseDTO> CreatePackage(Guid userId, Guid packageId)
        {
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (package == null)
                return new ResponseDTO("Package not found", 404, false);

            var userPackage = await _unitOfWork.UserPackageRepo.GetUserPackagesAsync(userId, packageId);
            if (userPackage != null)
            {
                // Nếu user đã có package, tăng thêm 50 lần sử dụng
                userPackage.Quantity += ADDITIONAL_QUANTITY;
                await _unitOfWork.UserPackageRepo.UpdateAsync(userPackage);
            }
            else
            {
                // Nếu user chưa có, tạo mới với 50 lần sử dụng
                userPackage = new UserPackage
                {
                    UserId = userId,
                    PackageId = packageId,
                    Quantity = ADDITIONAL_QUANTITY,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.UserPackageRepo.AddAsync(userPackage);
            }

            await _unitOfWork.SaveChangeAsync();
            return new ResponseDTO("Package added successfully (+50 uses)", 200, true);
        }

        // Admin tạo package cho user
        public async Task<ResponseDTO> CreatePackageForAdmin(Guid userId, Guid packageId)
        {
            return await CreatePackage(userId, packageId);
        }

        // User tự mua package (Lấy ID từ UserUtility)
        public async Task<ResponseDTO> CreatePackageForUser(Guid packageId)
        {
            var userId = _userUtility.GetUserIdFromToken();
            return await CreatePackage(userId, packageId);
        }

      
    }
}
