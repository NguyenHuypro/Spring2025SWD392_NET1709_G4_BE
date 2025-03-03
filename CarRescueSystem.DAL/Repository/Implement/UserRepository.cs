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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<List<User>> GetActiveStaffsAsync(int count)
        {
            return await _context.Users
                .Where(u => u.Role.RoleName == "Staff" && u.StaffStatus == StaffStatus.ACTIVE)
                .Take(count)
                .ToListAsync();
        }
        public async Task<List<User>> GetAvailableStaffByStationAsync(Guid rescueStationId)
        {
            return await _context.Users
                .Where(u => u.RescueStationId == rescueStationId && u.StaffStatus == StaffStatus.ACTIVE)
                .ToListAsync();
        }

        public async Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds)
        {
            return await _context.Users
                .Where(u => userIds.Contains(u.UserId) && u.Role.RoleName == "Staff" && u.StaffStatus == StaffStatus.ACTIVE)
                .ToListAsync();
        }



    }
}
