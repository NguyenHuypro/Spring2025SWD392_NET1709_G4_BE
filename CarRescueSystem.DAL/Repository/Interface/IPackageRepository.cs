using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.DAL.Repository.Interface
{
    public interface IPackageRepository : IGenericRepository<Package>
    {
        
        Task<ServicePackage?> GetServiceInPackageAsync(Guid packageId, Guid serviceId);
        Task<bool> UpdateServiceInPackageAsync(Guid packageId, Guid serviceId);
    }

}
