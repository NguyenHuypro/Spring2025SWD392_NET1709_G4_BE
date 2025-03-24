using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Data;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CarRescueSystem.DAL.Repository.Implement
{
    public class ServicePackageRepository : GenericRepository<ServicePackage>, IServicePackageRepository
    {
        private readonly ApplicationDbContext _context;
        public ServicePackageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServicePackage>> GetAllAsync(Expression<Func<ServicePackage, bool>> predicate)
        {
            return await _context.ServicePackages.Where(predicate).ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<ServicePackage> entities)
        {
            await _context.ServicePackages.AddRangeAsync(entities);
        }

        public void DeleteRange(IEnumerable<ServicePackage> entities)
        {
            _context.ServicePackages.RemoveRange(entities);
        }
    
}
}
