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
                description = package.description,
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
                description = package.description,
                price = package.price,
                services = package.ServicePackages.Select(sp => new ServiceInPackageDTO
                {
                    name = sp.Service.name,
                    price = sp.Service.price.ToString("N0") // Format số nếu cần
                }).ToList() ?? new List<ServiceInPackageDTO>()
            };

            return new ResponseDTO("Package retrieved successfully.", 200, true, packageDTO);

    
        }

        public async Task<ResponseDTO> AddAsync(AddPackageDTO packageDTO)
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
                description = packageDTO.description,

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


            return new ResponseDTO("Package created successfully.", 201, true, package);
        }

        public async Task<ResponseDTO> UpdateAsync(Guid id, PackageDTO packageDTO)
        {
            

          

            var existingPackage = await _unitOfWork.PackageRepo.GetByIdAsync(id);
            if (existingPackage == null)
            {
                return new ResponseDTO("Package not found.", 404, false);
            }

            // ** Chỉ cập nhật nếu giá trị không phải null **
            if (!string.IsNullOrWhiteSpace(packageDTO.name))
            {
                existingPackage.name = packageDTO.name;
            }

            if (packageDTO.price.HasValue && packageDTO.price > 0)
            {
                existingPackage.price = packageDTO.price.Value;
            }


            if (!string.IsNullOrWhiteSpace(packageDTO.description))
            {
                existingPackage.description = packageDTO.description;
            }

            if (packageDTO.services != null && packageDTO.services.Any(s => !string.IsNullOrWhiteSpace(s)))
            {
                // Lọc ra các serviceId hợp lệ (loại bỏ null, rỗng, hoặc khoảng trắng)
                var validServiceIds = packageDTO.services
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();

                // ** Xóa các ServicePackage cũ trong DB **
                var existingServicePackages = await _unitOfWork.ServicePackageRepo
                    .GetAllAsync(sp => sp.packageID == existingPackage.id); // Lấy danh sách dịch vụ cũ

                if (existingServicePackages.Any())
                {
                    _unitOfWork.ServicePackageRepo.DeleteRange(existingServicePackages);
                }

                // ** Thêm mới danh sách ServicePackage **
                List<ServicePackage> newServicePackages = new List<ServicePackage>();
                List<string> notFoundServiceIds = new List<string>();

                foreach (var serviceIdStr in validServiceIds)
                {
                    if (!Guid.TryParse(serviceIdStr, out Guid serviceId))
                    {
                        return new ResponseDTO($"Invalid service ID format: {serviceIdStr}", 400, false);
                    }

                    var service = await _unitOfWork.ServiceRepo.GetByIdAsync(serviceId);
                    if (service != null)
                    {
                        newServicePackages.Add(new ServicePackage
                        {
                            id = Guid.NewGuid(),
                            packageID = existingPackage.id,
                            serviceId = serviceId
                        });
                    }
                    else
                    {
                        notFoundServiceIds.Add(serviceIdStr);
                    }
                }

                if (newServicePackages.Any())
                {
                    await _unitOfWork.ServicePackageRepo.AddRangeAsync(newServicePackages);
                }

                if (notFoundServiceIds.Any())
                {
                    return new ResponseDTO($"The following Service IDs were not found: {string.Join(", ", notFoundServiceIds)}", 404, false);
                }
            }
            else
            {
                return new ResponseDTO("No valid service IDs provided.", 400, false);
            }

            // Lưu thay đổi vào database
            await _unitOfWork.SaveChangeAsync();

            // ** Tạo response DTO **
            var packageResponse = new GetAllPackageDTO
            {
                id = existingPackage.id,
                name = existingPackage.name,
                description = existingPackage.description,
                price = existingPackage.price,
                services = existingPackage.ServicePackages.Select(sp => new ServiceInPackageDTO
                {
                    name = sp.Service.name,
                    price = sp.Service.price.ToString("N0") // Format số nếu cần
                }).ToList() ?? new List<ServiceInPackageDTO>()
            };

            return new ResponseDTO("Package updated successfully.", 200, true, packageResponse);
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
