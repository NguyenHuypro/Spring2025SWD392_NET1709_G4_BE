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

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var customerCount = await _context.Users.CountAsync(u => u.Role.RoleName.ToUpper() == "CUSTOMER");
            var staffCount = await _context.Users.CountAsync(u => u.Role.RoleName.ToUpper() == "STAFF");
            var registeredCarCount = await _context.Vehicles.CountAsync();
            var bookingCount = await _context.Bookings.CountAsync();
            var monthlyRevenue = await _context.Bookings
                .Where(b => b.CreatedAt.Month == DateTime.Now.Month && b.CreatedAt.Year == DateTime.Now.Year)
                .SumAsync(b => b.TotalPrice) ?? 0;

            var monthlyRevenues = await _context.Bookings
                .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
                .Select(g => new MonthlyRevenueDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(b => b.TotalPrice) ?? 0
                })
                .ToListAsync();

            return new DashboardDto
            {
                CustomerCount = customerCount,
                StaffCount = staffCount,
                RegisteredCarCount = registeredCarCount,
                BookingCount = bookingCount,
                MonthlyRevenue = monthlyRevenue,
                MonthlyRevenues = monthlyRevenues
            };
        }
    }
}