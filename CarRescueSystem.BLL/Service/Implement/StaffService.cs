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

            // 2️⃣ Lấy tọa độ từ địa chỉ
            var coordinates = await _osmService.GetCoordinatesFromAddressAsync(booking.location);
            if (coordinates == null)
            {
                return new ResponseDTO("Failed to retrieve coordinates for booking location", 400, false);
            }

            // ✅ Cập nhật tọa độ
            booking.latitude = coordinates.latitude;
            booking.longitude = coordinates.longitude;

            // 3️⃣ Tìm trạm cứu hộ gần nhất
            var nearestStation = await _rescueStationService.FindNearestStationAsync(booking.latitude ?? 0.0, booking.longitude ?? 0.0);
            if (nearestStation == null)
            {
                return new ResponseDTO("No rescue station found nearby", 404, false);
            }

            // 4️⃣ Lấy danh sách nhân viên có sẵn tại trạm cứu hộ
            var availableStaffs = await _unitOfWork.UserRepo.GetAvailableStaffByStationAsync(nearestStation.id);
            if (availableStaffs == null || !availableStaffs.Any())
            {
                return new ResponseDTO("No available staff found at the rescue station", 404, false);
            }

            // 5️⃣ Cập nhật thông tin Booking với trạm cứu hộ
            booking.rescueStationId = nearestStation.id;

            //// 6️⃣ Thêm tất cả nhân viên vào BookingStaff
            //var bookingStaffList = availableStaffs.Select(staff => new BookingStaff
            //{
            //    id = Guid.NewGuid(),
            //    bookingId = booking.id,
            //    staffId = staff.id
            //}).ToList();

            // Lưu thông tin BookingStaff và cập nhật Booking
            //await _unitOfWork.BookingStaffRepo.AddRangeAsync(bookingStaffList);
            await _unitOfWork.BookingRepo.UpdateAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            // Chuyển đổi thông tin nhân viên thành ListStaffDTO
            var staffDTOs = availableStaffs.Select(staff => new ListStaffDTO
            {
                Id = staff.id,
                FullName = staff.fullName,
                
            }).ToList();

            // Trả về thông tin thành công với danh sách nhân viên
            return new ResponseDTO("Booking confirmed and assigned to nearest rescue station", 200, true, staffDTOs);
        }
    
    }
}
