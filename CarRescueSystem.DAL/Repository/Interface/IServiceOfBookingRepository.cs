using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.DAL.Repository.Interface
{
    public interface IServiceOfBookingRepository : IGenericRepository<ServiceOfBooking>
    {
        Task<ServiceOfBooking?> GetByBookingAndServiceAsync(Guid bookingId, Guid serviceId);
        Task<bool> ServiceInPackageAsync(Guid serviceId);
    }
}
