using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.BLL.Utilities;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserUtility _userUtility;

        public ScheduleService(IUnitOfWork unitOfWork, UserUtility userUtility)
        {
            _unitOfWork = unitOfWork;
            _userUtility = userUtility;
        }

        public async Task<ResponseDTO> GetAllSchedulesAsync()
        {
            // Lấy tất cả lịch làm việc
            var schedules = await _unitOfWork.ScheduleRepo.GetAll().ToListAsync();

            // Kiểm tra nếu không có lịch nào
            if (schedules == null || !schedules.Any())
            {
                return new ResponseDTO("No schedules.", 404, false);
            }

            // Nếu có lịch, trả về thông báo thành công cùng với danh sách lịch
            return new ResponseDTO("Successfully retrieved work schedules", 200, true, schedules);
        }


        public async Task<ResponseDTO> GetSchedulesByDateAsync(DateTime date)
        {
            var schedules = await _unitOfWork.ScheduleRepo.GetSchedulesByDateAsync(date);

            // Check if no schedules exist for that date
            if (schedules == null || !schedules.Any())
            {
                return new ResponseDTO($"No schedules found for {date:yyyy-MM-dd}.", 404, false);
            }

            // If schedules exist, return the list for that date
            return new ResponseDTO($"Successfully retrieved work schedule for {date:yyyy-MM-dd}", 200, true, schedules);
        }


        public async Task<ResponseDTO> GetScheduleByIdAsync(Guid id)
        {
            var schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(id);

            // If no schedule is found
            if (schedule == null)
            {
                return new ResponseDTO("Schedule not found", 404, false);
            }

            // If schedule is found
            return new ResponseDTO("Successfully retrieved work schedule", 200, true, schedule);
        }


        public async Task<ResponseDTO> CreateScheduleAsync(ScheduleDTO scheduleDto)
        {
            var staff = await _unitOfWork.UserRepo.GetByIdAsync(scheduleDto.UserId);
            if (staff == null)
                return new ResponseDTO("Staff not found", 404, false);

            var newSchedule = new Schedule
            {
                ScheduleId = Guid.NewGuid(),
                UserId = scheduleDto.UserId,
                StartTime = scheduleDto.StartTime,
                EndTime = scheduleDto.EndTime,
                Shift = Enum.TryParse<ShiftType>(scheduleDto.Shift, out var shiftType) ? shiftType : ShiftType.Morning
            };

            await _unitOfWork.ScheduleRepo.AddAsync(newSchedule);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Schedule created successfully", 201, true);
        }

        public async Task<ResponseDTO> UpdateScheduleAsync(Guid scheduleId, ScheduleDTO scheduleDto)
        {
            var schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(scheduleId);
            if (schedule == null)
                return new ResponseDTO("Schedule not found", 404, false);

            schedule.StartTime = scheduleDto.StartTime;
            schedule.EndTime = scheduleDto.EndTime;
            schedule.Shift = Enum.TryParse<ShiftType>(scheduleDto.Shift, out var shiftType) ? shiftType : schedule.Shift;

            await _unitOfWork.ScheduleRepo.UpdateAsync(schedule);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Schedule updated successfully", 200, true);
        }

        public async Task<ResponseDTO> DeleteScheduleAsync(Guid scheduleId)
        {
            var schedule = await _unitOfWork.ScheduleRepo.GetByIdAsync(scheduleId);
            if (schedule == null)
                return new ResponseDTO("Schedule not found", 404, false);

            await _unitOfWork.ScheduleRepo.DeleteAsync(scheduleId);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Schedule deleted successfully", 200, true);
        }
        public async Task<ResponseDTO> GetSchedulesByStaffIdAsync(Guid? staffId = null)
        {
            if (staffId == null)
            {
                staffId = _userUtility.GetUserIdFromToken();
            }

            var schedules = await _unitOfWork.ScheduleRepo.GetSchedulesByStaffIdAsync(staffId.Value);
            return new ResponseDTO("Successfully retrieved staff schedules", 200, true, schedules);
        }
    }
}
