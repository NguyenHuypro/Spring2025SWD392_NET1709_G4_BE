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
        private readonly IOsmService _osmService;
        private readonly IRescueStationService _rescueStationService;
        private readonly IStaffService _staffService;
        private readonly IWalletService _walletService;

        public BookingService(IUnitOfWork unitOfWork, UserUtility userUtility, IOsmService osmService, IRescueStationService rescueStationService, IStaffService staffService, IWalletService walletService)
        {
            _unitOfWork = unitOfWork;
            _userUtility = userUtility;
            _osmService = osmService;
            _rescueStationService = rescueStationService;
            _staffService = staffService;
            _walletService = walletService;
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

                // Kiểm tra VehicleId hoặc LicensePlate hợp lệ để lấy PackageId
                Vehicle? vehicle = null;
                Guid? packageId = null;

                if (request.VehicleId.HasValue)
                {
                    // Tìm vehicle theo VehicleId
                    vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(request.VehicleId.Value);
                }

                // Nếu không tìm thấy vehicle bằng VehicleId, thử tìm bằng LicensePlate
                if (vehicle == null && !string.IsNullOrEmpty(request.LicensePlate))
                {
                    vehicle = await _unitOfWork.VehicleRepo.GetByLicensePlateAsync(request.LicensePlate);
                }

                // Nếu tìm được vehicle, lấy PackageId
                if (vehicle != null)
                {
                    packageId = vehicle.PackageId;
                }

                // Mã hóa location để tránh lỗi ký tự đặc biệt
                string encodedAddress = Uri.EscapeDataString(request.Location);

                // Tạo Booking mới
                var newBooking = new Booking
                {
                    BookingId = Guid.NewGuid(),
                    CustomerId = request.CustomerId,
                    VehicleId = vehicle?.VehicleId,  
                    Description = request.Description,
                    Evidence = request.Evidence,
                    Location = encodedAddress,
                    CreatedAt = DateTime.UtcNow,
                    Status = BookingStatus.PENDING,
                    PackageId = packageId, 
                    LicensePlate = request.LicensePlate,
                    PhoneNumber = request.PhoneNumber  
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
                if (booking.Status != BookingStatus.PENDING)
                {
                    return new ResponseDTO("Booking is not in a valid state for confirmation", 400, false);
                }

                var coordinates = await _osmService.GetCoordinatesFromAddressAsync(booking.Location);
                if (coordinates == null)
                {
                    return new ResponseDTO("Failed to retrieve coordinates for booking location", 400, false);
                }

                // ✅ Luôn cập nhật tọa độ
                booking.Latitude = coordinates.Latitude;
                booking.Longitude = coordinates.Longitude;


                // 4️⃣ Tìm trạm cứu hộ gần nhất
                var nearestStation = await _rescueStationService.FindNearestStationAsync(booking.Latitude ?? 0.0, booking.Longitude ?? 0.0);
                if (nearestStation == null)
                {
                    return new ResponseDTO("No rescue station found nearby", 404, false);
                }

                // 5️⃣ Chọn nhân viên phù hợp từ trạm đó
                var availableStaff = await _staffService.GetAvailableStaffAsync(nearestStation.RescueStationId);
                if (availableStaff == null || !availableStaff.Any())
                {
                    return new ResponseDTO("No available staff at the nearest rescue station", 404, false);
                }

                // 6️⃣ Cập nhật thông tin RescueStationId và Staff cho Booking
                booking.Status = BookingStatus.CONFIRMED;
                booking.RescueStationId = nearestStation.RescueStationId;

                // ✅ Thêm tất cả nhân viên vào BookingStaff
                var bookingStaffList = availableStaff.Select(staff => new BookingStaff
                {
                    BookingId = booking.BookingId,
                    StaffId = staff.UserId
                }).ToList();
                Console.WriteLine(booking.Latitude);
                Console.WriteLine(booking.Longitude);
                await _unitOfWork.BookingStaffRepo.AddRangeAsync(bookingStaffList);
                await _unitOfWork.BookingRepo.UpdateAsync(booking);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Booking confirmed and assigned to nearest rescue station", 200, true, new
                {
                    BookingId = booking.BookingId,
                    RescueStation = nearestStation.Name,
                    AssignedStaffIds = bookingStaffList.Select(bs => bs.StaffId).ToList() // Trả về danh sách StaffId
                });
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }



        public async Task<ResponseDTO> AssignStaffToBookingAsync(Guid bookingId, List<Guid> staffIds)
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
                if (booking.Status != BookingStatus.CONFIRMED)
                {
                    return new ResponseDTO("Booking is not confirmed yet", 400, false);
                }

                // 3️⃣ Kiểm tra danh sách staff có hợp lệ không
                if (staffIds == null || !staffIds.Any())
                {
                    return new ResponseDTO("Staff ID list is empty", 400, false);
                }

                // 4️⃣ Lấy danh sách staff theo ID
                var selectedStaffs = await _unitOfWork.UserRepo.GetUsersByIdsAsync(staffIds);
                if (selectedStaffs.Count != staffIds.Count)
                {
                    return new ResponseDTO("One or more staff IDs are invalid", 400, false);
                }

                var assignedStaffs = new List<BookingStaff>();

                foreach (var staff in selectedStaffs)
                {
                    // 5️⃣ Gán Staff vào Booking
                    var bookingStaff = new BookingStaff
                    {
                        BookingStaffId = Guid.NewGuid(),
                        BookingId = bookingId,
                        StaffId = staff.UserId
                    };
                    assignedStaffs.Add(bookingStaff);
                    await _unitOfWork.BookingStaffRepo.AddAsync(bookingStaff);

                    // 6️⃣ Cập nhật trạng thái Staff → Inactive
                    staff.StaffStatus = StaffStatus.INACTIVE;
                    await _unitOfWork.UserRepo.UpdateAsync(staff);
                }

                // 7️⃣ Cập nhật trạng thái Booking → InProgress
                booking.Status = BookingStatus.INPROGRESS;
                booking.StartAt = DateTime.UtcNow;
                await _unitOfWork.BookingRepo.UpdateAsync(booking);

                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO($"Assigned {selectedStaffs.Count} staff to booking and updated status to InProgress", 200, true);
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

            decimal totalPrice = 0;
            decimal totalDiscount = 0;

            // Kiểm tra xem Booking có Vehicle không
            Vehicle? vehicle = null;
            Guid? packageId = null;
            bool isPackageExpired = true; // Giả định gói đã hết hạn

            if (booking.VehicleId.HasValue)
            {
                vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(booking.VehicleId.Value);
                if (vehicle != null)
                {
                    packageId = vehicle.PackageId;

                    // Kiểm tra nếu gói dịch vụ còn hạn sử dụng
                    if (vehicle.ExpirationDate.HasValue && vehicle.ExpirationDate.Value >= DateTime.UtcNow)
                    {
                        isPackageExpired = false; // Gói còn hạn
                    }
                }
            }

            foreach (var serviceId in serviceIds)
            {
                // Kiểm tra service có tồn tại không
                var service = await _unitOfWork.ServiceRepo.GetByIdAsync(serviceId);
                if (service == null) continue;

                // Kiểm tra service đã có trong booking chưa
                var existingService = await _unitOfWork.ServiceOfBookingRepo.GetByBookingAndServiceAsync(bookingId, serviceId);
                if (existingService == null)
                {
                    var newService = new ServiceOfBooking
                    {
                        BookingId = bookingId,
                        ServiceId = serviceId
                    };
                    await _unitOfWork.ServiceOfBookingRepo.AddAsync(newService);
                }

                decimal originalPrice = service.ServicePrice;
                decimal discountAmount = 0;

                // Nếu xe có package và gói còn hạn, kiểm tra xem service có trong package không
                if (packageId.HasValue && !isPackageExpired)
                {
                    var serviceInPackage = await _unitOfWork.PackageRepo.GetServiceInPackageAsync(packageId.Value, serviceId);
                    if (serviceInPackage != null)
                    {
                        // Giảm giá 20% nếu service có trong package và gói còn hạn
                        discountAmount = originalPrice * 0.2m;
                    }
                }

                totalDiscount += discountAmount;
                totalPrice += (originalPrice - discountAmount);
            }

            // Cập nhật tổng giá Booking
            booking.TotalPrice = totalPrice;
            await _unitOfWork.BookingRepo.UpdateAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Services added and total price updated", 200, true, new
            {
                TotalPrice = totalPrice,
                TotalDiscount = totalDiscount,
                PackageExpired = isPackageExpired // Trả về thông tin gói có hết hạn không
            });
        }



        public async Task<ResponseDTO> CompleteOrCancelBookingAsync(Guid bookingId, bool isCompleted)
        {
            var booking = await _unitOfWork.BookingRepo.GetByIdWithBookingStaffsAsync(bookingId);
            if (booking == null)
                return new ResponseDTO("Booking not found", 404, false);

            if (isCompleted)
            {
                // 🛠 **Kiểm tra số dư trong ví khách hàng**
                var response = await _walletService.GetWalletByUserId(booking.CustomerId);
                var wallet = response.Result as Wallet;
                if (wallet.Balance < booking.TotalPrice)
                {
                    return new ResponseDTO("Not enough balance to complete the booking", 400, false);
                }

                // 💰 **Trừ tiền nếu đủ số dư**
                if (!booking.TotalPrice.HasValue)
                {
                    return new ResponseDTO("Total price is not set", 400, false);
                }
                var deductResponse = await _walletService.DeductAmount(booking.CustomerId, booking.TotalPrice.Value);

                if (!deductResponse.IsSuccess)
                {
                    return new ResponseDTO("Payment failed", 400, false);
                }

                booking.Status = BookingStatus.COMPLETE;
            }
            else
            {
                // ❌ Nếu bị hủy, chỉ cập nhật trạng thái
                booking.Status = BookingStatus.CANCELLED;
            }

            await _unitOfWork.BookingRepo.UpdateAsync(booking);

            // ✅ **Kích hoạt lại nhân viên**
            foreach (var staffBooking in booking.BookingStaffs)
            {
                var staff = await _unitOfWork.UserRepo.GetByIdAsync(staffBooking.StaffId);
                staff.StaffStatus = StaffStatus.ACTIVE;
                await _unitOfWork.UserRepo.UpdateAsync(staff);
            }

            await _unitOfWork.SaveChangeAsync();

            string message = isCompleted ? "Booking completed successfully and payment processed" : "Booking cancelled";
            return new ResponseDTO(message, 200, true);
        }

        public async Task<ResponseDTO> GetAllBookingAsync()
        {
            var bookings =  _unitOfWork.BookingRepo.GetAll();
            return new ResponseDTO("Successfully retrieved all bookings", 200, true, bookings);
        }

        public async Task<ResponseDTO> GetBookingByCustomerIdAsync(Guid? customerId = null)
        {
            if (customerId == null)
            {
                customerId = _userUtility.GetUserIdFromToken();
            }

            var bookings = await _unitOfWork.BookingRepo.GetBookingsByCustomerIdAsync(customerId.Value);
            return new ResponseDTO("Successfully retrieved customer bookings", 200, true, bookings);
        }

    }

}
