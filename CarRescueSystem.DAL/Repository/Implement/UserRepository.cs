using System;
using System.Collections.Generic;
using System.Linq;
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
            return await _context.Users.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<User> CheckTelephone(string phone)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.phone == phone);
        }

        public async Task<List<User>> GetActiveStaffsAsync(int count)
        {
            return await _context.Users
                .Where(u => u.role == RoleType.STAFF && u.staffStatus == staffStatus.ACTIVE)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<User>> GetAvailableStaffByStationAsync(Guid rescueStationId)
        {
            return await _context.Users
                .Where(u => u.rescueStationId == rescueStationId && u.role == RoleType.STAFF && u.staffStatus == staffStatus.ACTIVE)
                .ToListAsync();
        }

        public async Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds)
        {
            // Lấy các nhân viên có id trong userIds, role là STAFF và trạng thái là ACTIVE
            var users = await _context.Users
                .Where(u => userIds.Contains(u.id) && u.role == RoleType.STAFF && u.staffStatus == staffStatus.ACTIVE)
                .ToListAsync();

            // Kiểm tra xem số lượng nhân viên lấy được có khớp với danh sách userIds không
            if (users.Count != userIds.Count)
            {
                // Nếu số lượng không khớp, tức là có một hoặc nhiều ID không hợp lệ, bạn có thể trả về lỗi.
                throw new Exception("One or more staff IDs are invalid.");
            }

            return users;
        }


        public async Task<List<User>> GetAllStaffsAsync()
        {
            return await _context.Users
                .Where(u => u.role == RoleType.STAFF || u.role == RoleType.RECEPTIONIST)
                
                .ToListAsync();
        }

        public async Task<List<User>> GetAllCustomersAsync()
        {
            return await _context.Users
                .Where(u => u.role == RoleType.CUSTOMER)
                .ToListAsync();
        }

    }
}
