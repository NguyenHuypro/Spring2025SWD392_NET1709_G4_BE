using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.DAL.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> FindByEmailAsync(string email);
        Task<User> CheckTelephone(string phone);
        Task<List<User>> GetActiveStaffsAsync(int count);
        Task<List<User>> GetAvailableStaffByStationAsync(Guid rescueStationId);
        Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds);
        Task<List<User>> GetAllStaffsAsync();
        Task<List<User>> GetAllCustomersAsync();
    }
}
