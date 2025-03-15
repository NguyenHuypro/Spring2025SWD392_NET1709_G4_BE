using AutoMapper;
using CarRescueSystem.Common.DTO.Vehicle;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.BLL.Utilities
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, VehicleDTO>()
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer.fullName));
            
            CreateMap<CreateVehicleDTO, Vehicle>();
            CreateMap<UpdateVehicleDTO, Vehicle>();
        }
    }
} 