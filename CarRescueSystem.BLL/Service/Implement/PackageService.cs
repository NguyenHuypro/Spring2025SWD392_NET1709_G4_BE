using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.UnitOfWork;
using CarRescueSystem.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetAllAsync()
        {
            var packages =  _unitOfWork.PackageRepo.GetAll();
            if (packages == null || !packages.Any())
            {
                return new ResponseDTO("No packages found.", 404, false);
            }
            return new ResponseDTO("Packages retrieved successfully.", 200, true);
        }

        public async Task<ResponseDTO> GetByIdAsync(Guid id)
        {
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(id);
            if (package == null)
            {
                return new ResponseDTO("Package not found.", 404, false);
            }
            return new ResponseDTO("Package retrieved successfully.", 200, true);
        }

        public async Task<ResponseDTO> AddAsync(PackageDTO packageDTO)
        {
            // Kiểm tra dữ liệu đầu vào
            if (packageDTO == null || string.IsNullOrWhiteSpace(packageDTO.PackageName) || packageDTO.PackagePrice <= 0)
            {
                return new ResponseDTO("Invalid package data.", 400, false);
            }

            var package = new Package
            {
                PackageId = Guid.NewGuid(),
                PackageName = packageDTO.PackageName,
                PackagePrice = packageDTO.PackagePrice
            };

            var result = await _unitOfWork.PackageRepo.AddAsync(package);
            

            // Nếu có danh sách serviceId, thêm vào bảng trung gian ServicePackage
            if (packageDTO.ServiceIds != null && packageDTO.ServiceIds.Any())
            {
                var serviceResponse = await _unitOfWork.PackageRepo.AddServiceToPackageAsync(package.PackageId, packageDTO.ServiceIds);
                if (!serviceResponse)
                {
                    return new ResponseDTO("Package created, but failed to link services.", 500, false);
                }
            }

            return new ResponseDTO("Package created successfully.", 201, true);
        }

        public async Task<ResponseDTO> UpdateAsync(Guid packageId, PackageDTO packageDTO)
        {
            // Kiểm tra dữ liệu đầu vào
            if (packageId == Guid.Empty || packageDTO == null || string.IsNullOrWhiteSpace(packageDTO.PackageName) || packageDTO.PackagePrice <= 0)
            {
                return new ResponseDTO("Invalid package data.", 400, false);
            }

            var existingPackage = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (existingPackage == null)
            {
                return new ResponseDTO("Package not found.", 404, false);
            }

            // Cập nhật thông tin package
            existingPackage.PackageName = packageDTO.PackageName;
            existingPackage.PackagePrice = packageDTO.PackagePrice;

            var result = await _unitOfWork.PackageRepo.UpdateAsync(existingPackage);
            await _unitOfWork.SaveChangeAsync();
            

            return new ResponseDTO("Package updated successfully.", 200, true);
        }

        public async Task<ResponseDTO> DeleteAsync(Guid id)
        {
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(id);
            if (package == null)
            {
                return new ResponseDTO("Package not found.", 404, false);
            }

            var result = _unitOfWork.PackageRepo.DeleteAsync(id);
            await _unitOfWork.SaveChangeAsync();


            return new ResponseDTO("Package deleted successfully.", 200, true);
        }
    }
}
