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
    public class RescueStationRepository : GenericRepository<RescueStation>, IRescueStationRepository
    {
        private readonly ApplicationDbContext _context;
        public RescueStationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<RescueStation>> GetAllAsync()
        {
            return await _context.RescueStations.ToListAsync();
        }

    }
}
