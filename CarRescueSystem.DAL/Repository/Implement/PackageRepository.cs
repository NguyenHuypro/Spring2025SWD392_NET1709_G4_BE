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
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        private readonly ApplicationDbContext _context;
        public PackageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        

        public async Task<ServicePackage?> GetServiceInPackageAsync(Guid packageId, Guid serviceId)
        {
            return await _context.ServicePackages
                .FirstOrDefaultAsync(ps => ps.PackageID == packageId && ps.ServiceId == serviceId);
        }
        public async Task<bool> UpdateServiceInPackageAsync(Guid packageId, Guid serviceId)
        {
            var packageService = await GetServiceInPackageAsync(packageId, serviceId);
            if (packageService == null)
                return false;

            if (packageService.Quantity > 1)
            {
                packageService.Quantity -= 1; // Giảm số lượng nếu còn
            }
            else
            {
                _context.ServicePackages.Remove(packageService); // Xóa nếu hết số lượng
            }

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
