using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.DAL.Repository.Interface
{
    public interface IUserPackageRepository : IGenericRepository<UserPackage>
    {
        Task<List<UserPackage>> GetUserPackagesListAsync(Guid userId);
        Task<UserPackage?> GetUserPackagesAsync(Guid userId, Guid packageId);


    }
}
