using CarRescueSystem.DAL.Repository.Interface;
using AutoMapper;
using CarRescueSystem.Common.DTO.Vehicle;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.DAL.UnitOfWork;
using CarRescueSystem.BLL.Utilities;
using CarRescueSystem.Common.DTO;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class VehicleService : IVehicleService
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserUtility _userUtility;
        private readonly IWalletService _walletService;

        public VehicleService(IUnitOfWork unitOfWork, UserUtility userUtility, IWalletService walletService)
        {
            
            _unitOfWork = unitOfWork;
            _userUtility = userUtility;
            _walletService = walletService;
        }

        public async Task<ResponseDTO> CreateCar(CreateVehicleDTO request)
        {
            try
            {
                //check customer
                var customer = await _unitOfWork.UserRepo.GetByIdAsync(_userUtility.GetUserIdFromToken());
                if (customer == null)
                {
                    return new ResponseDTO("User not found", 404, false);
                }
                var vehicle = new Vehicle{
                    CustomerId = request.CustomerId,
                    VehicleId = Guid.NewGuid(),
                    VehicleName = request.VehicleName,
                    VehicleColor = request.VehicleColor,
                    VehicleBrand = request.VehicleBrand,
                    NumberOfSeats = request.NumberOfSeats,
                    LicensePlate = request.LicensePlate
                };
                await _unitOfWork.VehicleRepo.AddAsync(vehicle);
                await _unitOfWork.SaveChangeAsync();

                // Trả về ResponseDTO
                return new ResponseDTO("Vehicle created successfully", 201, true, vehicle);
            }
                catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> GetVehicleByIDAsync(Guid id)
        {
            try
            {
                var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(id);
                if (vehicle == null)
                    return new ResponseDTO("No Vehicle with this ID found!", 404, false);


                return new ResponseDTO("Vehicle found successfully", 201, true, vehicle);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> GetAllVehicleAsync()
        {
            try
            {
                var vehicles = await _unitOfWork.VehicleRepo.ToListAsync();
                 if (!vehicles.Any())
                    return new ResponseDTO($"Error: {"No Vehicle found!"}", 404, false);

                return new ResponseDTO("Vehicle found successfully", 201, true, vehicles);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> UpdateAsync(Guid id, UpdateVehicleDTO request)
        {
            try
            {      
                var oldvehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(id);
                if (oldvehicle == null)
                    return new ResponseDTO($"Error: {"No Vehicle with this id found!"}", 404, false);
                    //update oldvehicle
                    oldvehicle.VehicleName = request.VehicleName;
                    oldvehicle.VehicleColor = request.VehicleColor;
                    oldvehicle.VehicleBrand = request.VehicleBrand;
                    oldvehicle.NumberOfSeats = request.NumberOfSeats;
                    oldvehicle.LicensePlate = request.LicensePlate;


                if (!string.IsNullOrWhiteSpace(request.VehicleName))
                    oldvehicle.VehicleName = request.VehicleName;

                if (!string.IsNullOrWhiteSpace(request.VehicleColor))
                    oldvehicle.VehicleColor = request.VehicleColor;

                if (!string.IsNullOrWhiteSpace(request.VehicleBrand))
                    oldvehicle.VehicleBrand = request.VehicleBrand;

                if (request.NumberOfSeats > 0)
                    oldvehicle.NumberOfSeats = request.NumberOfSeats;

                if (!string.IsNullOrWhiteSpace(request.LicensePlate))
                    oldvehicle.LicensePlate = request.LicensePlate;

                await _unitOfWork.VehicleRepo.UpdateAsync(oldvehicle);
                await _unitOfWork.SaveChangeAsync();


                return new ResponseDTO("Vehicle updated successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> DeleteAsync(Guid id)
        {
            try
            {
                var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(id);
                if (vehicle == null)
                    return new ResponseDTO($"Error: {"No Vehicle with this id found!"}", 404, false);
                await _unitOfWork.VehicleRepo.DeleteAsync(id);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Vehicle deleted successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> PurchasePackage(Guid vehicleId, Guid packageId)
        {
            var userId = _userUtility.GetUserIdFromToken();
            var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(vehicleId);

            if (vehicle == null || vehicle.CustomerId != userId)
                return new ResponseDTO("Vehicle not found or unauthorized", 403, false);

            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (package == null)
                return new ResponseDTO("Package not found", 404, false);

            // Thanh toán
            decimal totalPrice = package.PackagePrice;
            var deductResponse = await _walletService.DeductAmount(userId, totalPrice);
            if (!deductResponse.IsSuccess)
                return deductResponse;

            // Gán package cho xe
            vehicle.PackageId = packageId;
         
            vehicle.ExpirationDate = DateTime.UtcNow.AddMonths(1); // Thêm thời hạn 1 tháng

            await _unitOfWork.VehicleRepo.UpdateAsync(vehicle);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Package purchased successfully", 200, true);
        }

        public async Task<ResponseDTO> GetVehiclePackage(Guid vehicleId)
        {
            var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(vehicleId);
            if (vehicle == null || vehicle.PackageId == null)
                return new ResponseDTO("No package found for this vehicle", 404, false);

            var package = await _unitOfWork.PackageRepo.GetByIdAsync(vehicle.PackageId.Value);
            var response = new
            {
                PackageId = package.PackageId,
                PackageName = package.PackageName,
          
                ExpirationDate = vehicle.ExpirationDate
            };

            return new ResponseDTO("Package retrieved successfully", 200, true, response);
        }

        public async Task<ResponseDTO> RemovePackage(Guid vehicleId)
        {
            var userId = _userUtility.GetUserIdFromToken();
            var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(vehicleId);

            if (vehicle == null || vehicle.PackageId == null)
                return new ResponseDTO("No package found for this vehicle", 404, false);

            if (vehicle.CustomerId != userId)
                return new ResponseDTO("Unauthorized: You do not own this vehicle", 403, false);

            vehicle.PackageId = null;
           
            vehicle.ExpirationDate = null;

            await _unitOfWork.VehicleRepo.UpdateAsync(vehicle);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Package removed successfully", 200, true);
        }

        public async Task<ResponseDTO> UpgradePackage(Guid vehicleId, Guid newPackageId)
        {
            var userId = _userUtility.GetUserIdFromToken();
            var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(vehicleId);

            if (vehicle == null || vehicle.CustomerId != userId)
                return new ResponseDTO("Vehicle not found or unauthorized", 403, false);

            var newPackage = await _unitOfWork.PackageRepo.GetByIdAsync(newPackageId);
            if (newPackage == null)
                return new ResponseDTO("New package not found", 404, false);

            if (vehicle.PackageId == null)
            {
                var deductResponse = await _walletService.DeductAmount(userId, newPackage.PackagePrice);
                if (!deductResponse.IsSuccess)
                    return deductResponse;

                vehicle.PackageId = newPackage.PackageId;
               
                vehicle.ExpirationDate = DateTime.UtcNow.AddMonths(1);

                await _unitOfWork.VehicleRepo.UpdateAsync(vehicle);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseDTO($"Purchased {newPackage.PackageName} package", 200, true);
            }

            var currentPackage = await _unitOfWork.PackageRepo.GetByIdAsync(vehicle.PackageId.Value);
            if (currentPackage == null)
                return new ResponseDTO("Current package not found", 404, false);

            if (currentPackage.PackageId == newPackage.PackageId)
            {
                var deductResponse = await _walletService.DeductAmount(userId, newPackage.PackagePrice);
                if (!deductResponse.IsSuccess)
                    return deductResponse;

                
                vehicle.ExpirationDate = vehicle.ExpirationDate?.AddMonths(1) ?? DateTime.UtcNow.AddMonths(1);

                await _unitOfWork.VehicleRepo.UpdateAsync(vehicle);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseDTO($"Extended {newPackage.PackageName} package (+50 uses)", 200, true);
            }

            if (IsUpgradeValid(currentPackage.PackageName, newPackage.PackageName))
            {
                decimal priceDifference = newPackage.PackagePrice - currentPackage.PackagePrice;
                if (priceDifference <= 0)
                    return new ResponseDTO("Invalid upgrade path", 400, false);

                vehicle.PackageId = newPackage.PackageId;
                
                vehicle.ExpirationDate = DateTime.UtcNow.AddMonths(1);

                await _unitOfWork.VehicleRepo.UpdateAsync(vehicle);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO($"Upgraded to {newPackage.PackageName} (Paid difference: {priceDifference})", 200, true);
            }

            return new ResponseDTO("Invalid upgrade path", 400, false);
        }
        private bool IsUpgradeValid(string currentPackage, string newPackage)
        {
            if (currentPackage == "Basic Package" && (newPackage == "Comprehensive Package" || newPackage == "Premium Package"))
                return true;
            if (currentPackage == "Comprehensive Package" && newPackage == "Premium Package")
                return true;
            return false;
        }
    }
} 