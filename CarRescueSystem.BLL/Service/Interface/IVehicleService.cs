using CarRescueSystem.Common.DTO;
using CarRescueSystem.Common.DTO.Vehicle;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IVehicleService
    {
        // 
        Task<ResponseDTO> CreateCar(CreateVehicleDTO request);
        Task<ResponseDTO> GetVehicleByIDAsync(Guid id);
        Task<ResponseDTO> GetAllVehicleAsync();
        Task<ResponseDTO> UpdateAsync(Guid id, UpdateVehicleDTO request);
        Task<ResponseDTO> DeleteAsync(Guid id);

        // 
        Task<ResponseDTO> PurchasePackage(Guid vehicleId, Guid packageId);
        Task<ResponseDTO> GetVehiclePackage(Guid vehicleId);
        Task<ResponseDTO> RemovePackage(Guid vehicleId);
        Task<ResponseDTO> UpgradePackage(Guid vehicleId, Guid newPackageId);
    }
}
