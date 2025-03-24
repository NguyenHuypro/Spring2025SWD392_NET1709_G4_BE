using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IRescueStationService
    {
        Task<List<RescueStation>> FindNearestStationsAsync(double bookingLat, double bookingLng);
        Task<ResponseDTO> GetAllRescueStation();
        //Task<ResponseDTO> AddStaffIntoStation(Guid staffId, Guid stationId);
    }
}
