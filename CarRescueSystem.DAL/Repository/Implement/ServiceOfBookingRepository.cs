using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Data;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CarRescueSystem.DAL.Repository.Implement
{
    internal class ServiceOfBookingRepository : GenericRepository<ServiceOfBooking>, IServiceOfBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceOfBookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ServiceOfBooking?> GetByBookingAndServiceAsync(Guid bookingId, Guid serviceId)
        {
            return await _context.ServiceOfBookings
                .FirstOrDefaultAsync(sob => sob.BookingId == bookingId && sob.ServiceId == serviceId);
        }
        public async Task<bool> ServiceInPackageAsync(Guid serviceId)
        {
            return await _context.ServicePackages.AnyAsync(sp => sp.ServiceId == serviceId);
        }


    }
}
