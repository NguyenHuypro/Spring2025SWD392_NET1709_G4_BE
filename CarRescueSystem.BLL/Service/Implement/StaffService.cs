using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.UnitOfWork;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOsmService _osmService; // Assuming you have this service for coordinates
        private readonly IRescueStationService _rescueStationService; // Assuming this is to find nearest rescue stations

        public StaffService(IUnitOfWork unitOfWork, IOsmService osmService, IRescueStationService rescueStationService)
        {
            _unitOfWork = unitOfWork;
            _osmService = osmService;
            _rescueStationService = rescueStationService;
        }

        public async Task<ResponseDTO> GetAvailableStaffAsync(Guid bookingId)
        {
            // 1️⃣ Kiểm tra Booking có tồn tại không
            var booking = await _unitOfWork.BookingRepo.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return new ResponseDTO("Booking not found", 404, false);
            }

            

            // 3️⃣ Lấy danh sách trạm cứu hộ theo khoảng cách
            var nearestStations = await _rescueStationService.FindNearestStationsAsync(booking.latitude ?? 0.0, booking.longitude ?? 0.0);

            // Nếu không có trạm nào, trả về lỗi
            if (nearestStations == null || !nearestStations.Any())
            {
                return new ResponseDTO("No rescue station found nearby", 404, false);
            }

            // 4️⃣ Duyệt từng trạm để tìm trạm có staff rảnh
            RescueStation? selectedStation = null;
            List<User>? availableStaffs = null;

            foreach (var station in nearestStations)
            {
                availableStaffs = await _unitOfWork.UserRepo.GetAvailableStaffByStationAsync(station.id);
                if (availableStaffs != null && availableStaffs.Any())
                {
                    selectedStation = station;
                    break; // Dừng khi tìm được trạm có staff
                }
            }

            // Nếu không tìm thấy trạm nào có staff rảnh
            if (selectedStation == null || availableStaffs == null || !availableStaffs.Any())
            {
                return new ResponseDTO("Không có staff rảnh", 200, false, null);
            }

            // 5️⃣ Cập nhật Booking với trạm cứu hộ được chọn
            booking.rescueStationId = selectedStation.id;
            await _unitOfWork.BookingRepo.UpdateAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            // Chuyển đổi thông tin nhân viên thành ListStaffDTO
            var staffDTOs = availableStaffs.Select(staff => new ListStaffDTO
            {
                Id = staff.id,
                FullName = staff.fullName,
                RescueStationName = selectedStation.name
                
            }).ToList();

            // Trả về thông tin thành công với danh sách nhân viên
            return new ResponseDTO("Booking confirmed and assigned to nearest rescue station", 200, true, staffDTOs);
        }
    
    }
}
