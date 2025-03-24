using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class RescueStationService : IRescueStationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RescueStationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<RescueStation>> FindNearestStationsAsync(double bookingLat, double bookingLng)
        {
            var stations = await _unitOfWork.RescueStationRepo.GetAllAsync();
            if (stations == null || !stations.Any()) return new List<RescueStation>();

            return stations
                .Select(station => new
                {
                    Station = station,
                    Distance = GetDistance(bookingLat, bookingLng, station.latitude, station.longitude)
                })
                .OrderBy(x => x.Distance)  // Sắp xếp theo khoảng cách
                .Select(x => x.Station)    // Chỉ lấy đối tượng trạm
                .ToList();
        }


        //Hàm tính khoảng cách giữa 2 tọa độ (Haversine Formula)
        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Bán kính Trái Đất (km)
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        public async Task<ResponseDTO> GetAllRescueStation()
        {
            var stations = await _unitOfWork.RescueStationRepo.GetAll().ToListAsync();

            if (!stations.Any())
            {
                return new ResponseDTO("No rescue station found", 404, false);
            }

            var stationsDto = stations.Select(station => new RescueStationDTO
            {
                id = station.id,
                name = station.name,
                address = station.address,
                email = station.email,
            }).ToList();

            return new ResponseDTO("Get all rescue stations successfully", 200, true, stationsDto);
        }

        //public async Task<ResponseDTO> AddStaffIntoStation(Guid staffId, Guid stationId)
        //{
        //    var staff = await _unitOfWork.UserRepo.GetByIdAsync(staffId);
        //    if (staff == null)
        //    {
        //        return new ResponseDTO("Không tìm thấy nhân viên", 404, false);
        //    }

        //    if (staff.rescueStationId == stationId)
        //    {
        //        return new ResponseDTO("Nhân viên đã thuộc trạm cứu hộ này", 400, false);
        //    }

        //    var station = await _unitOfWork.RescueStationRepo.GetByIdAsync(stationId);
        //    if (station == null)
        //    {
        //        return new ResponseDTO("Không tìm thấy trạm cứu hộ", 404, false);
        //    }

        //    staff.rescueStationId = stationId;
        //    staff.RescueStation = station;

        //    await _unitOfWork.UserRepo.UpdateAsync(staff);
        //    await _unitOfWork.SaveChangeAsync();

        //    return new ResponseDTO("Thêm nhân viên vào trạm cứu hộ thành công", 200, true);
        //}

    }
}
