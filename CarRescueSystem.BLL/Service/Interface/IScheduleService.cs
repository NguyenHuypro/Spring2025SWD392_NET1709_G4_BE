using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IScheduleService
    {
        Task<ResponseDTO> GetAllSchedulesAsync();
        Task<ResponseDTO> GetSchedulesByDateAsync(DateTime date);
        Task<ResponseDTO> GetScheduleByIdAsync(Guid id);
        Task<ResponseDTO> CreateScheduleAsync(ScheduleDTO scheduleDto); // Dùng DTO
        Task<ResponseDTO> UpdateScheduleAsync(Guid scheduleId, ScheduleDTO scheduleDto); // Truyền Id + DTO
        Task<ResponseDTO> DeleteScheduleAsync(Guid scheduleId);
        Task<ResponseDTO> GetSchedulesByStaffIdAsync(Guid? staffId = null);
    }
}
