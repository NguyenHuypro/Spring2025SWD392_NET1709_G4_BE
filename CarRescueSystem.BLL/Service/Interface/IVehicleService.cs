using CarRescueSystem.Common.DTO.Vehicle;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IVehicleService
    {
        Task<VehicleResponse> CreateAsync(CreateVehicleDTO createDto);
        Task<VehicleResponse> GetByIdAsync(Guid id);
        Task<VehicleListResponse> GetAllAsync();
        Task<VehicleResponse> UpdateAsync(Guid id, UpdateVehicleDTO updateDto);
        Task<VehicleResponse> DeleteAsync(Guid id);
    }
} 