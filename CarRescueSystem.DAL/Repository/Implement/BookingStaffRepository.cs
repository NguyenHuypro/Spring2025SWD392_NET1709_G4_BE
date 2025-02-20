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
    public class BookingStaffRepository : GenericRepository<BookingStaff>, IBookingStaffRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingStaffRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<BookingStaff?> GetByBookingIdAsync(Guid bookingId)
        {
            return await _context.BookingStaffs
                .Where(bs => bs.BookingId == bookingId)
                .FirstOrDefaultAsync();
        }

    }
}
