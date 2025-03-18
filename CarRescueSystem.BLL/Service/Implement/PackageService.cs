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
            var packages = await _unitOfWork.PackageRepo.GetAllPackageWithServiceAsync(); // Ensure you're using async if needed

            if (packages == null || !packages.Any())
            {
                return new ResponseDTO("No packages found.", 404, false);
            }

            // Map the packages to the GetAllPackageDTO
            var packageDTOs = packages.Select(package => new GetAllPackageDTO
            {
                id = package.id,
                name = package.name,
                
                price = package.price,
                services = package.ServicePackages.Select(servicePackage => new ServiceInPackageDTO
                {
                    name = servicePackage.Service.name,
                    price = servicePackage.Service.price.ToString("F2") // Format the price to two decimal places
                }).ToList()
            }).ToList();

            return new ResponseDTO("Packages retrieved successfully.", 200, true, packageDTOs);
        }


        public async Task<ResponseDTO> GetByIdAsync(Guid id)
        {
            var package = await _unitOfWork.PackageRepo.GetPackageByIdWithServiceAsync(id);
            if (package == null)
            {
                return new ResponseDTO("Package not found.", 404, false);
            }
            // Chuyển đổi sang DTO
            var packageDTO = new GetAllPackageDTO
            {
                id = package.id,
                name = package.name,
                
                price = package.price,
                services = package.ServicePackages.Select(sp => new ServiceInPackageDTO
                {
                    name = sp.Service.name,
                    price = sp.Service.price.ToString("N0") // Format số nếu cần
                }).ToList() ?? new List<ServiceInPackageDTO>()
            };

            return new ResponseDTO("Package retrieved successfully.", 200, true, packageDTO);

    
        }

        public async Task<ResponseDTO> AddAsync(PackageDTO packageDTO)
        {
            // Kiểm tra dữ liệu đầu vào
            if (packageDTO == null || string.IsNullOrWhiteSpace(packageDTO.name))
            {
                return new ResponseDTO("Invalid package data.", 400, false);
            }

            // Ép kiểu string -> decimal
            if (!decimal.TryParse(packageDTO.price, out decimal price) || price <= 0)
            {
                return new ResponseDTO("Invalid price value.", 400, false);
            }

            var package = new Package
            {
                id = Guid.NewGuid(),
                name = packageDTO.name,
                price = price,

            };

            var result = await _unitOfWork.PackageRepo.AddAsync(package);


            if (packageDTO.services != null && packageDTO.services.Any())
            {
                var serviceGuids = packageDTO.services.Select(Guid.Parse).ToList(); // Chuyển đổi List<string> -> List<Guid>

                var serviceResponse = await _unitOfWork.PackageRepo.AddServiceToPackageAsync(package.id, serviceGuids);
                if (!serviceResponse)
                {
                    return new ResponseDTO("Package created, but failed to link services.", 500, false);
                }
            }


            return new ResponseDTO("Package created successfully.", 201, true, packageDTO);
        }

        public async Task<ResponseDTO> UpdateAsync(Guid packageId, PackageDTO packageDTO)
        {
            // Kiểm tra dữ liệu đầu vào
            if (packageDTO == null || string.IsNullOrWhiteSpace(packageDTO.name))
            {
                return new ResponseDTO("Invalid package data.", 400, false);
            }

            // Ép kiểu string -> decimal
            if (!decimal.TryParse(packageDTO.price, out decimal price) || price <= 0)
            {
                return new ResponseDTO("Invalid price value.", 400, false);
            }

            var existingPackage = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (existingPackage == null)
            {
                return new ResponseDTO("Package not found.", 404, false);
            }

            // Cập nhật thông tin package
            existingPackage.name = packageDTO.name;
            existingPackage.price = price;

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
