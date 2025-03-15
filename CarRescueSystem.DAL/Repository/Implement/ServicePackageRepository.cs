using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Data;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.Repository.Interface;

namespace CarRescueSystem.DAL.Repository.Implement
{
    public class ServicePackageRepository : GenericRepository<ServicePackage>, IServicePackageRepository
    {
        private readonly ApplicationDbContext _context;
        public ServicePackageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    
    }
}
