using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.DAL.Repository.Interface
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        Task<List<Schedule>> GetSchedulesByDateAsync(DateTime date);
        Task<IEnumerable<Schedule>> GetSchedulesByStaffIdAsync(Guid staffId);
    }
}
