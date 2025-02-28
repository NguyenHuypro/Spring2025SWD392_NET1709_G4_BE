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
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Booking> GetByIdWithBookingStaffsAsync(Guid bookingId)
        {
            return await _context.Bookings
                .Include(b => b.BookingStaffs) // Load danh sách BookingStaffs
                    .ThenInclude(bs => bs.Staff)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(Guid customerId)
        {
            return await _context.Bookings.Where(b => b.CustomerId == customerId).ToListAsync();
        }

    }
}
