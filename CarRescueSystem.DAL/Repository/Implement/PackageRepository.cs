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
                .FirstOrDefaultAsync(ps => ps.packageID == packageId && ps.serviceId == serviceId);
        }
        public async Task<bool> UpdateServiceInPackageAsync(Guid packageId, Guid serviceId)
        {
            var packageService = await GetServiceInPackageAsync(packageId, serviceId);
            if (packageService == null)
                return false;

            
                _context.ServicePackages.Remove(packageService); // Xóa nếu hết số lượng
            

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddServiceToPackageAsync(Guid packageId, List<Guid> serviceIds)
        {
            var package = await _context.Packages.FindAsync(packageId);
            if (package == null) return false;

            var services = _context.Services.Where(s => serviceIds.Contains(s.id)).ToList();
            if (!services.Any()) return false;

            var servicePackages = services.Select(service => new ServicePackage
            {
                id = Guid.NewGuid(),
                packageID = packageId,
                serviceId = service.id
            }).ToList();

            await _context.ServicePackages.AddRangeAsync(servicePackages);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Package>> GetAllPackageWithServiceAsync()
        {
            return await _context.Packages
                .Include(p => p.ServicePackages)  // Eager load the related services
                    .ThenInclude(sp => sp.Service)
                .ToListAsync();            // Return the result as a list
        }
        public async Task<Package> GetPackageByIdWithServiceAsync(Guid id)
        {
            return await _context.Packages
                .Include(p => p.ServicePackages)  // Nạp danh sách dịch vụ trong gói
                    .ThenInclude(sp => sp.Service) // Nạp chi tiết từng dịch vụ
                .FirstOrDefaultAsync(p => p.id == id); // Lấy gói theo ID
        }

    }
}
