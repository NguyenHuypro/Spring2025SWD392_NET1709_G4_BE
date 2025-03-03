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
                    ServiceId = s.ServiceId,
                    ServiceName = s.ServiceName,
                    ServicePrice = s.ServicePrice
                }).ToList();

                return new ResponseDTO("Successfully retrieved service list.", 200, true);
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
                    ServiceId = service.ServiceId,
                    ServiceName = service.ServiceName,
                    ServicePrice = service.ServicePrice
                };

                return new ResponseDTO("Successfully retrieved service.", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Internal server error: {ex.Message}", 500, false);
            }
        }

        // Create a new service
        public async Task<ResponseDTO> CreateService(ServiceDTO serviceDTO)
        {
            try
            {
                if (serviceDTO == null)
                {
                    return new ResponseDTO("Invalid service data.", 400, false);
                }

                // Convert DTO to Entity
                var service = new DAL.Model.Service
                {
                    ServiceId = Guid.NewGuid(),
                    ServiceName = serviceDTO.ServiceName,
                    ServicePrice = serviceDTO.ServicePrice
                };

                // Add new service
                await _unitOfWork.ServiceRepo.AddAsync(service);
                await _unitOfWork.SaveChangeAsync();

                var createdServiceDTO = new ServiceDTO
                {
                    ServiceId = service.ServiceId,
                    ServiceName = service.ServiceName,
                    ServicePrice = service.ServicePrice
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
                service.ServiceName = serviceDTO.ServiceName ?? service.ServiceName;
                service.ServicePrice = serviceDTO.ServicePrice != default ? serviceDTO.ServicePrice : service.ServicePrice;

                await _unitOfWork.ServiceRepo.UpdateAsync(service);
                await _unitOfWork.SaveChangeAsync();

                var updatedServiceDTO = new ServiceDTO
                {
                    ServiceId = service.ServiceId,
                    ServiceName = service.ServiceName,
                    ServicePrice = service.ServicePrice
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
    }
}
