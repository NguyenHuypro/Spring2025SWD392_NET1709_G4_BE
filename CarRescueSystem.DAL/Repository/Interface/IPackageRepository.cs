using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.DAL.Repository.Interface
{
    public interface IPackageRepository : IGenericRepository<Package>
    {
        
        Task<ServicePackage?> GetServiceInPackageAsync(Guid packageId, Guid serviceId);
        Task<bool> UpdateServiceInPackageAsync(Guid packageId, Guid serviceId);
        // Thêm phương thức mới
        // Sửa lại trả về bool
        Task<bool> AddServiceToPackageAsync(Guid packageId, List<Guid> serviceIds);
        Task<List<Package>> GetAllPackageWithServiceAsync();
        Task<Package> GetPackageByIdWithServiceAsync(Guid id);
    }


}
