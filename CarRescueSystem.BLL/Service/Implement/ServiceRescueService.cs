using System;
using System.Linq;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class ServiceRescueService : IServiceRescueService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceRescueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Get all services
        public async Task<ResponseDTO> GetAllService()
        {
            try
            {
                var listService = await _unitOfWork.ServiceRepo.GetAll().ToListAsync();
                

                if (listService == null || !listService.Any())
                {
                    return new ResponseDTO("No services found.", 404, false);
                }

                var serviceDTOs = listService.Select(s => new ServiceDTO
                {
                    ServiceId = s.id,
                    ServiceName = s.name,
                    ServicePrice = s.price
                }).ToList();

                return new ResponseDTO("Successfully retrieved service list.", 200, true, listService);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Internal server error: {ex.Message}", 500, false);
            }
        }

        // Get service by ID
        public async Task<ResponseDTO> GetServiceById(Guid serviceId)
        {
            try
            {
                var service = await _unitOfWork.ServiceRepo.GetByIdAsync(serviceId);

                if (service == null)
                {
                    return new ResponseDTO("Service not found.", 404, false);
                }

                var serviceDTO = new ServiceDTO
                {
                    ServiceId = service.id,
                    ServiceName = service.name,
                    ServicePrice = service.price
                };

                return new ResponseDTO("Successfully retrieved service.", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Internal server error: {ex.Message}", 500, false);
            }
        }

        // Create a new service
        public async Task<ResponseDTO> CreateService(CreateServiceDTO createServiceDTO)
        {
            try
            {
                if (createServiceDTO == null)
                {
                    return new ResponseDTO("Invalid service data.", 400, false);
                }

                // Convert DTO to Entity
                var service = new DAL.Model.Service
                {
                    id = Guid.NewGuid(),
                    name = createServiceDTO.name,
                    price = decimal.Parse( createServiceDTO.price)
                };

                // Add new service
                await _unitOfWork.ServiceRepo.AddAsync(service);
                await _unitOfWork.SaveChangeAsync();

                var createdServiceDTO = new ServiceDTO
                {
                    ServiceId = service.id,
                    ServiceName = service.name,
                    ServicePrice = service.price
                };

                return new ResponseDTO("Service created successfully.", 201, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Internal server error: {ex.Message}", 500, false);
            }
        }

        // Update an existing service
        public async Task<ResponseDTO> UpdateService(Guid serviceId, ServiceDTO serviceDTO)
        {
            try
            {
                var service = await _unitOfWork.ServiceRepo.GetByIdAsync(serviceId);

                if (service == null)
                {
                    return new ResponseDTO("Service not found.", 404, false);
                }

                // Update service details from DTO
                service.name = serviceDTO.ServiceName ?? service.name;
                service.price = serviceDTO.ServicePrice != default ? serviceDTO.ServicePrice : service.price;

                await _unitOfWork.ServiceRepo.UpdateAsync(service);
                await _unitOfWork.SaveChangeAsync();

                var updatedServiceDTO = new ServiceDTO
                {
                    ServiceId = service.id,
                    ServiceName = service.name,
                    ServicePrice = service.price
                };

                return new ResponseDTO("Service updated successfully.", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Internal server error: {ex.Message}", 500, false);
            }
        }

        // Delete a service
        public async Task<ResponseDTO> DeleteService(Guid serviceId)
        {
            try
            {
                var service = await _unitOfWork.ServiceRepo.GetByIdAsync(serviceId);

                if (service == null)
                {
                    return new ResponseDTO("Service not found.", 404, false);
                }

                // Delete the service
                await _unitOfWork.ServiceRepo.DeleteAsync(serviceId);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Service deleted successfully.", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Internal server error: {ex.Message}", 500, false);
            }
        }
        public async Task<ResponseDTO> GetServicesByPackageId(Guid packageId)
        {
            var servicePackages =  _unitOfWork.ServicePackageRepo.GetAll();
            var services = servicePackages
                .Where(sp => sp.packageID == packageId)
                .Select(sp => sp.Service)
                .ToList();

            if (!services.Any())
            {
                return new ResponseDTO("Không tìm thấy dịch vụ nào trong gói này.", 404, false);
            }

            return new ResponseDTO("Lấy danh sách dịch vụ thành công", 200, true, services);
        }


    }
}
