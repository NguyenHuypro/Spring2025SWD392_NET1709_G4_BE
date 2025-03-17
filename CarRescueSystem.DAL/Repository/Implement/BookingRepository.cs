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
                .FirstOrDefaultAsync(b => b.id == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(Guid customerId)
        {
            return await _context.Bookings
                .Where(b => b.customerId == customerId)
                .Include(b => b.Vehicle)
                .Include(b => b.BookingStaffs)
                    .ThenInclude(bs => bs.Staff)
                .Include(b => b.ServiceBookings) // ✅ Lấy danh sách dịch vụ
                    .ThenInclude(bs => bs.Service)
                .ToListAsync();
        }
        public async Task<IEnumerable<Booking>> GetBookingsByStaffIdAsync(Guid staffId)
        {
            return await _context.Bookings
                .Where(b => _context.BookingStaffs
                    .Any(bs => bs.staffId == staffId && bs.bookingId == b.id))
                .Include(b => b.ServiceBookings)  // Bao gồm dịch vụ
                    .ThenInclude(sb => sb.Service)
                .Include(b => b.Customer)  // Bao gồm khách hàng
                .Include(b => b.Vehicle)
                .Include(b => b.BookingStaffs)
                    .ThenInclude(bs => bs.Staff)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingForHistoryAsync(Guid bookingId)
        {
            return await _context.Bookings
                .Where(b => b.id == bookingId)
                .Include(b => b.Customer)        // Thông tin khách hàng
                .Include(b => b.Vehicle)         // Xe liên quan
                .Include(b => b.BookingStaffs)   // Nhân viên cứu hộ
                    .ThenInclude(bs => bs.Staff)
                .Include(b => b.ServiceBookings) // Dịch vụ đã làm
                    .ThenInclude(sb => sb.Service)
                .FirstOrDefaultAsync(); // Get the first (or only) booking or null if not found
        }

        public async Task<List<Booking>> GetAllBookingsForManagerAsync()
        {
            return await _context.Bookings
                
                .Include(b => b.Customer)
                .Include(b => b.Vehicle)
                .Include(b => b.BookingStaffs)
                    .ThenInclude(bs => bs.Staff)
                .Include(b => b.ServiceBookings) // ✅ Lấy danh sách dịch vụ
                    .ThenInclude(bs => bs.Service)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetAllBookingsGuest()
        {
            return await _context.Bookings
                .Where(b => b.bookingType == TypeBooking.GUEST)
                .Include(b => b.Customer)
                .Include(b => b.Vehicle)
                .Include(b => b.BookingStaffs)
                    .ThenInclude(bs => bs.Staff)
                .Include(b => b.ServiceBookings) // ✅ Lấy danh sách dịch vụ
                    .ThenInclude(bs => bs.Service)
                .ToListAsync();
        }








    }
}
