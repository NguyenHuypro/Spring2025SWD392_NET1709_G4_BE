using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.DAL.Repository.Interface
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<Booking> GetByIdWithBookingStaffsAsync(Guid bookingId);
        Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Booking>> GetBookingsByStaffIdAsync(Guid staffId);


        Task<Booking> GetBookingForHistoryAsync(Guid bookingId);

        Task<List<Booking>> GetAllBookingsForManagerAsync();

        Task<List<Booking>> GetAllBookingsGuest();
        Task<IEnumerable<Booking>> CheckBookingsByCustomerIdAsync(Guid customerId);

    }
}
