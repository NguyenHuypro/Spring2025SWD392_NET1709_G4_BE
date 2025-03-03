using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IRescueStationService
    {
        Task<RescueStation?> FindNearestStationAsync(double bookingLat, double bookingLng);
    }
}
