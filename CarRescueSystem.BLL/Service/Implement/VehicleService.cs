using CarRescueSystem.DAL.Repository.Interface;
using AutoMapper;
using CarRescueSystem.Common.DTO.Vehicle;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.BLL.Service.Interface;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<VehicleResponse> CreateAsync(CreateVehicleDTO createDto)
        {
            try
            {
                var vehicle = _mapper.Map<Vehicle>(createDto);
                var created = await _repository.AddAsync(vehicle);
                return new VehicleResponse
                {
                    Success = true,
                    Message = "Vehicle created successfully",
                    Data = _mapper.Map<VehicleDTO>(created)
                };
            }
            catch (Exception ex)
            {
                return new VehicleResponse
                {
                    Success = false,
                    Message = $"Error creating vehicle: {ex.Message}"
                };
            }
        }

        public async Task<VehicleResponse> GetByIdAsync(Guid id)
        {
            try
            {
                var vehicle = await _repository.GetByIdAsync(id);
                if (vehicle == null)
                    return new VehicleResponse
                    {
                        Success = false,
                        Message = $"Vehicle with ID {id} not found"
                    };

                return new VehicleResponse
                {
                    Success = true,
                    Data = _mapper.Map<VehicleDTO>(vehicle)
                };
            }
            catch (Exception ex)
            {
                return new VehicleResponse
                {
                    Success = false,
                    Message = $"Error retrieving vehicle: {ex.Message}"
                };
            }
        }

        public async Task<VehicleListResponse> GetAllAsync()
        {
            try
            {
                var vehicles = await _repository.ToListAsync();
                return new VehicleListResponse
                {
                    Success = true,
                    Data = _mapper.Map<IEnumerable<VehicleDTO>>(vehicles)
                };
            }
            catch (Exception ex)
            {
                return new VehicleListResponse
                {
                    Success = false,
                    Message = $"Error retrieving vehicles: {ex.Message}"
                };
            }
        }

        public async Task<VehicleResponse> UpdateAsync(Guid id, UpdateVehicleDTO updateDto)
        {
            try
            {
                var vehicle = await _repository.GetByIdAsync(id);
                if (vehicle == null)
                    return new VehicleResponse
                    {
                        Success = false,
                        Message = $"Vehicle with ID {id} not found"
                    };

                _mapper.Map(updateDto, vehicle);
                var updated = await _repository.UpdateAsync(vehicle);
                return new VehicleResponse
                {
                    Success = true,
                    Message = "Vehicle updated successfully",
                    Data = _mapper.Map<VehicleDTO>(updated)
                };
            }
            catch (Exception ex)
            {
                return new VehicleResponse
                {
                    Success = false,
                    Message = $"Error updating vehicle: {ex.Message}"
                };
            }
        }

        public async Task<VehicleResponse> DeleteAsync(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return new VehicleResponse
                {
                    Success = true,
                    Message = "Vehicle deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new VehicleResponse
                {
                    Success = false,
                    Message = $"Error deleting vehicle: {ex.Message}"
                };
            }
        }
    }
} 