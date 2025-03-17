using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.DAL.Repository.Interface
{
    public interface IBookingStaffRepository : IGenericRepository<BookingStaff>
    {
        Task<BookingStaff?> GetByBookingIdAsync(Guid bookingId);

        Task AddRangeAsync(IEnumerable<BookingStaff> entities);
        Task<List<BookingStaff>> GetBookingStaffsByBookingIdAsync(Guid bookingId);

    }
}
