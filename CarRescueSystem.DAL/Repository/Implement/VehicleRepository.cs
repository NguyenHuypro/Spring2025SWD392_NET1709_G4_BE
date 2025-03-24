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
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        private readonly ApplicationDbContext _context;
        public VehicleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
        {
            var check = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.licensePlate == licensePlate);
            if (check == null)
            {
                return null;
            }
            return check;
        }
        public async Task<List<Vehicle>> GetVehiclesByUserIdAsync(Guid userId)
        {
            return await _context.Vehicles
                .Where(v => v.customerId == userId)
                .Include(v => v.Package)
                .ToListAsync();
        }

    }
}
