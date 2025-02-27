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
        private readonly IVehicleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserUtility _userUtility;

        public VehicleService(IVehicleRepository repository, IUnitOfWork unitOfWork, UserUtility userUtility)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userUtility = userUtility;
        }

        public async Task<ResponseDTO> CreateCar(CreateVehicleDTO request)
        {
            try
            {
                //check customer
                var customer = await _unitOfWork.UserRepo.GetByIdAsync(_userUtility.GetUserIdFromToken());
                if (customer == null)
                {
                    return new ResponseDTO("Customer not found", 404, false);
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
                    return new ResponseDTO($"Error: {"No Vehicle with this id found!"}", 404, false);

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
                 if (vehicles == null)
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
                

                var updated = await _unitOfWork.VehicleRepo.UpdateAsync(oldvehicle);
                 await _unitOfWork.SaveChangeAsync(); // Ensure changes are persisted

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
    }
} 