using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.DAL.Repository.Interface
{
    public interface IServicePackageRepository : IGenericRepository<ServicePackage>
    {
        Task<IEnumerable<ServicePackage>> GetAllAsync(Expression<Func<ServicePackage, bool>> predicate);
        Task AddRangeAsync(IEnumerable<ServicePackage> entities);
        void DeleteRange(IEnumerable<ServicePackage> entities);
    }

}
