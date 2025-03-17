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
    public class ScheduleRepository : GenericRepository<Schedule> , IScheduleRepository
    {
        private readonly ApplicationDbContext _context;
        public ScheduleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Schedule>> GetSchedulesByDateAsync(DateTime date)
        {
            return await _context.Schedules
                .Where(s => s.startTime.Date == date.Date)
                .Include(s => s.Staff) // Lấy thông tin Staff (User)
                .ToListAsync();
        }
        public async Task<IEnumerable<Schedule>> GetSchedulesByStaffIdAsync(Guid staffId)
        {
            return await _context.Schedules
                .Where(s => s.userId == staffId)
                .ToListAsync();
        }
    }
}
