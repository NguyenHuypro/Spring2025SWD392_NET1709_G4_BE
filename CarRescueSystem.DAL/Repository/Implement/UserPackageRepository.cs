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
    public class UserPackageRepository : GenericRepository<UserPackage>, IUserPackageRepository
    {
        private readonly ApplicationDbContext _context;
        public UserPackageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UserPackage>> GetUserPackagesListAsync(Guid userId)
        {
            return await _context.UserPackages
                .Where(up => up.UserId == userId && up.Quantity > 0) // Chỉ lấy package còn sử dụng được
                .Include(up => up.Package) // Bao gồm thông tin Package
                .ToListAsync();
        }
        public async Task<UserPackage?> GetUserPackagesAsync(Guid userId, Guid packageId)
        {
            return await _context.UserPackages
                .Where(up => up.UserId == userId && up.PackageId == packageId && up.Quantity > 0) // Chỉ lấy package còn sử dụng được
                .Include(up => up.Package)
                .FirstOrDefaultAsync();

        }

    }
}
