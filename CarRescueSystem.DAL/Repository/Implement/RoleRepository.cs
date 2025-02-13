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
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    
    }
}
