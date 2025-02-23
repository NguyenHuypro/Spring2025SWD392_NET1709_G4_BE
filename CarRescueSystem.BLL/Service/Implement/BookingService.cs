using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.BLL.Utilities;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.Repository.Implement;
using CarRescueSystem.DAL.Repository.Interface;
using CarRescueSystem.DAL.UnitOfWork;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserUtility _userUtility;

        public BookingService(IUnitOfWork unitOfWork, UserUtility userUtility)
        {
            _unitOfWork = unitOfWork;
            _userUtility = userUtility;
        }

        public async Task<ResponseDTO> CreateBookingAsync(CreatingBookingDTO request)
        {
            try
            {
               
                // Kiểm tra Customer có tồn tại không
                var customer = await _unitOfWork.UserRepo.GetByIdAsync(_userUtility.GetUserIdFromToken());
                if (customer == null)
                {
                    return new ResponseDTO("Customer not found", 404, false);
                }
                    
                // Kiểm tra VehicleId hợp lệ (nếu có)
                Vehicle? vehicle = null;
                if (request.VehicleId.HasValue)
                {
                    vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(request.VehicleId.Value);
                    if (vehicle == null)
                    {
                        return new ResponseDTO("Vehicle not found", 404, false);
                    }
                }

                // Tạo Booking mới
                var newBooking = new Booking
                {
                    BookingId = Guid.NewGuid(),
                    CustomerId = request.CustomerId,
                    VehicleId = request.VehicleId ,
                    Description = request.Description,
                    Evidence = request.Evidence,
                    Location = request.Location,
                    CreatedAt = DateTime.UtcNow,
                    Status = BookingStatus.Pending
                };

                // Lưu vào DB
                await _unitOfWork.BookingRepo.AddAsync(newBooking);
                await _unitOfWork.SaveChangeAsync();

                // Trả về ResponseDTO
                return new ResponseDTO("Booking created successfully", 201, true, newBooking);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }
        public async Task<ResponseDTO> ConfirmBookingAsync(Guid bookingId)
        {
            try
            {
                // 1️⃣ Kiểm tra Booking có tồn tại không
                var booking = await _unitOfWork.BookingRepo.GetByIdAsync(bookingId);
                if (booking == null)
                {
                    return new ResponseDTO("Booking not found", 404, false);
                }

                // 2️⃣ Kiểm tra trạng thái Booking (Chỉ cập nhật nếu là Pending)
                if (booking.Status != BookingStatus.Pending)
                {
                    return new ResponseDTO("Booking is not in a valid state for confirmation", 400, false);
                }

                // 3️⃣ Cập nhật trạng thái Booking → Confirmed
                booking.Status = BookingStatus.Confirmed;

                // 4️⃣ Lưu vào database
                await _unitOfWork.BookingRepo.UpdateAsync(booking);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Booking confirmed successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> AssignStaffToBookingAsync(Guid bookingId)
        {
            try
            {
                // 1️⃣ Kiểm tra Booking có tồn tại không
                var booking = await _unitOfWork.BookingRepo.GetByIdAsync(bookingId);
                if (booking == null)
                {
                    return new ResponseDTO("Booking not found", 404, false);
                }

                // 2️⃣ Kiểm tra trạng thái Booking (Chỉ assign staff khi đã Confirmed)
                if (booking.Status != BookingStatus.Confirmed)
                {
                    return new ResponseDTO("Booking is not confirmed yet", 400, false);
                }

                // 3️⃣ Lấy danh sách 2 staff Active
                var activeStaffs = await _unitOfWork.UserRepo.GetActiveStaffsAsync(2);
                if (activeStaffs.Count < 2)
                {
                    return new ResponseDTO("Not enough active staff available", 400, false);
                }

                var assignedStaffs = new List<BookingStaff>();

                foreach (var staff in activeStaffs)
                {
                    // 4️⃣ Gán Staff vào Booking
                    var bookingStaff = new BookingStaff
                    {
                        BookingStaffId = Guid.NewGuid(),
                        BookingId = bookingId,
                        StaffId = staff.UserId
                    };
                    assignedStaffs.Add(bookingStaff);
                    await _unitOfWork.BookingStaffRepo.AddAsync(bookingStaff);

                    // 5️⃣ Cập nhật trạng thái Staff → Inactive
                    staff.StaffStatus = StaffStatus.Inactive;
                    await _unitOfWork.UserRepo.UpdateAsync(staff);
                }

                // 6️⃣ Cập nhật trạng thái Booking → InProgress
                booking.Status = BookingStatus.InProgress;
                booking.StartAt = DateTime.UtcNow;
                await _unitOfWork.BookingRepo.UpdateAsync(booking);

                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO($"Assigned {activeStaffs.Count} staff to booking and updated status to InProgress", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }


        public async Task<ResponseDTO> AddServiceToBookingAsync(Guid bookingId, List<Guid> serviceIds)
        {
            var booking = await _unitOfWork.BookingRepo.GetByIdWithBookingStaffsAsync(bookingId);
            if (booking == null)
                return new ResponseDTO("Booking not found", 404, false);

            var userId = booking.CustomerId;
            var userPackages = await _unitOfWork.UserPackageRepo.GetUserPackagesListAsync(userId);

            decimal totalPrice = 0;
            decimal totalDiscount = 0; // Tổng số tiền giảm giá từ package

            foreach (var serviceId in serviceIds)
            {
                // Kiểm tra service có tồn tại không
                var service = await _unitOfWork.ServiceRepo.GetByIdAsync(serviceId);
                if (service == null) continue;

                // Kiểm tra xem service đã có trong booking chưa
                var existingService = await _unitOfWork.ServiceOfBookingRepo.GetByBookingAndServiceAsync(bookingId, serviceId);
                if (existingService == null)
                {
                    // Thêm service vào booking
                    var newService = new ServiceOfBooking
                    {
                        BookingId = bookingId,
                        ServiceId = serviceId
                    };
                    await _unitOfWork.ServiceOfBookingRepo.AddAsync(newService);
                }

                decimal originalPrice = service.ServicePrice;
                decimal discountAmount = 0; // Mặc định không giảm

                if (userPackages != null && userPackages.Any())
                {
                    // Kiểm tra trong các package có service này không
                    foreach (var userPackage in userPackages)
                    {
                        var serviceInPackage = await _unitOfWork.PackageRepo.GetServiceInPackageAsync(userPackage.PackageId, serviceId);
                        if (serviceInPackage != null && userPackage.Quantity > 0)
                        {
                            // Giảm 20% số tiền dịch vụ
                            discountAmount = originalPrice * 0.2m;

                            // Trừ số lần sử dụng của package
                            userPackage.Quantity -= 1;
                            await _unitOfWork.UserPackageRepo.UpdateAsync(userPackage);
                            break; // Chỉ sử dụng giảm giá từ 1 package
                        }
                    }
                }

                totalDiscount += discountAmount;
                totalPrice += (originalPrice - discountAmount);
            }

            // Cập nhật tổng giá Booking
            booking.TotalPrice += totalPrice;
            await _unitOfWork.BookingRepo.UpdateAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Services added and total price updated", 200, true, new
            {
                TotalPrice = totalPrice,
                TotalDiscount = totalDiscount
            });
        }



        public async Task<ResponseDTO> CompleteOrCancelBookingAsync(Guid bookingId, bool isCompleted)
        {
            var booking = await _unitOfWork.BookingRepo.GetByIdWithBookingStaffsAsync(bookingId);
            if (booking == null)
                return new ResponseDTO("Booking not found", 404, false);

            booking.Status = isCompleted ? BookingStatus.Completed : BookingStatus.Cancelled;
            await _unitOfWork.BookingRepo.UpdateAsync(booking);

            foreach (var staffBooking in booking.BookingStaffs)
            {
                var staff = await _unitOfWork.UserRepo.GetByIdAsync(staffBooking.StaffId);
                
                staff.StaffStatus = StaffStatus.Active;
                await _unitOfWork.UserRepo.UpdateAsync(staff);
                
            }

            await _unitOfWork.SaveChangeAsync();

            string message = isCompleted ? "Booking completed successfully" : "Booking cancelled";
            return new ResponseDTO(message, 200, true);
        }


    }

}
