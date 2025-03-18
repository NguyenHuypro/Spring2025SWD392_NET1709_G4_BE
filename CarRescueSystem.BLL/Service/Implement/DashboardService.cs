using System;
using System.Linq;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Data;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.BLL.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDTO> GetDashboardDataAsync()
        {
            var customerCount = await _context.Users.CountAsync(u => u.role.ToString().Equals("CUSTOMER"));
            var staffCount = await _context.Users.CountAsync(u => u.role.ToString().Equals("STAFF"));
            var receptionistCount = await _context.Users.CountAsync(u => u.role.ToString().Equals("RECEPTIONIST"));
            var registeredCarCount = await _context.Vehicles.CountAsync();
            var packageCount = await _context.Packages.CountAsync();
            var serviceCount = await _context.Services.CountAsync();
            var bookingCount = await _context.Bookings.CountAsync();
            var monthlyRevenue = await _context.Bookings
                .Where(b => b.bookingDate.Month == DateTime.Now.Month && b.bookingDate.Year == DateTime.Now.Year)
                .SumAsync(b => b.totalPrice) ?? 0;

            var monthlyRevenues = await _context.Bookings
                .GroupBy(b => new { b.bookingDate.Year, b.bookingDate.Month })
                .Select(g => new MonthlyRevenueDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(b => b.totalPrice) ?? 0
                })
                .ToListAsync();

            var dashboardData = new DashboardDto
            {
                CustomerCount = customerCount,
                StaffCount = staffCount,
                ReceptionistCount = receptionistCount,
                PackageCount = packageCount,
                ServiceCount = serviceCount,
                RegisteredCarCount = registeredCarCount,
                BookingCount = bookingCount,
                MonthlyRevenue = monthlyRevenue,
                MonthlyRevenues = monthlyRevenues
            };

            return new ResponseDTO("lấy dashboard thành công", 200, true, dashboardData);
        }
    }
}