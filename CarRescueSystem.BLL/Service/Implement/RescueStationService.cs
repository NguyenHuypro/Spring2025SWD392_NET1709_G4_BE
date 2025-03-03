using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.UnitOfWork;
using Microsoft.Identity.Client;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class RescueStationService : IRescueStationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RescueStationService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<RescueStation?> FindNearestStationAsync(double bookingLat, double bookingLng)
        {
            var stations = await _unitOfWork.RescueStationRepo.GetAllAsync();
            if (stations == null || !stations.Any()) return null;

            return stations
                .Select(station => new
                {
                    Station = station,
                    Distance = GetDistance(bookingLat, bookingLng, station.Latitude, station.Longitude)
                })
                .OrderBy(x => x.Distance)
                .FirstOrDefault()?.Station;
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

    }
}
