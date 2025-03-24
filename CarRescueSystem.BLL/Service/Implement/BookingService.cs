using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.BLL.Utilities;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.Repository.Implement;
using CarRescueSystem.DAL.Repository.Interface;
using CarRescueSystem.DAL.UnitOfWork;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserUtility _userUtility;
        private readonly IOsmService _osmService;
        private readonly IRescueStationService _rescueStationService;
        private readonly IStaffService _staffService;
        //private readonly IWalletService _walletService;
        private readonly IVnPayService _vpnPayService;
        private readonly ISupportFunction _support;

        public BookingService(IUnitOfWork unitOfWork, UserUtility userUtility, IOsmService osmService, IRescueStationService rescueStationService, IStaffService staffService, IVnPayService vnPayService, ISupportFunction support)
        {
            _unitOfWork = unitOfWork;
            _userUtility = userUtility;
            _osmService = osmService;
            _rescueStationService = rescueStationService;
            _staffService = staffService;
            //_walletService = walletService;
            _vpnPayService = vnPayService;
            _support = support;
        }

        public async Task<ResponseDTO> CreateBookingAsync(CreatingBookingDTO request)
        {
            try
            {
                // Kiểm tra Customer có tồn tại không
                var customer = await _unitOfWork.UserRepo.GetByIdAsync(_userUtility.GetUserIdFromToken());
                if (customer == null)
                {
                    return new ResponseDTO("Không tìm thấy khách hàng", 200, false);
                }

                // Kiểm tra VehicleId hoặc LicensePlate hợp lệ để lấy PackageId
                Vehicle? vehicle = null;
                Guid? packageId = null;

                if (request.carId.HasValue)
                {
                    // Tìm vehicle theo VehicleId
                    vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(request.carId.Value);
                }

                //// Nếu không tìm thấy vehicle bằng VehicleId, thử tìm bằng LicensePlate
                //if (vehicle == null && !string.IsNullOrEmpty(request.licensePlate))
                //{
                //    vehicle = await _unitOfWork.VehicleRepo.GetByLicensePlateAsync(request.licensePlate);
                //}

                // Nếu tìm được vehicle, lấy PackageId
                if (vehicle != null)
                {
                    packageId = vehicle.packageId;
                }

                var checkBooking = await _unitOfWork.BookingRepo.CheckBookingsByCustomerIdAsync(customer.id);
                if (checkBooking != null)
                {
                    return new ResponseDTO("Lỗi !!! Khách hàng đã có booking chưa hoàn thành.", 200, false);
                }
                

                // Mã hóa location để tránh lỗi ký tự đặc biệt
                string encodedAddress = Uri.EscapeDataString(request.location);

                // 2️⃣ Lấy tọa độ từ địa chỉ
                var coordinates = await _osmService.GetCoordinatesFromAddressAsync(encodedAddress);
                if (coordinates == null)
                {
                    return new ResponseDTO("Địa chỉ không hợp lệ !!!", 200, false);
                }

                //// ✅ Cập nhật tọa độ
                //booking.latitude = coordinates.latitude;
                //booking.longitude = coordinates.longitude;

                // Tạo Booking mới
                var newBooking = new Booking
                {
                    id = Guid.NewGuid(),
                    customerId = customer.id,
                    vehicleId = vehicle?.id,  
                    description = request.description,
                    evidence = request.evidence,
                    location = encodedAddress,
                    bookingDate = DateTime.UtcNow,
                    //--------------------------
                    status = BookingStatus.PENDING,
                    packageId = packageId, 
                    licensePlate = vehicle?.licensePlate,
                    phone = request.phone ,
                    bookingType = TypeBooking.MEMBER,
                    latitude = coordinates.latitude,
                    longitude = coordinates.longitude
                };

                // Lưu vào DB
                await _unitOfWork.BookingRepo.AddAsync(newBooking);
                await _unitOfWork.SaveChangeAsync();

                // Trả về ResponseDTO
                return new ResponseDTO("Tạo booking thành công", 200, true, newBooking);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }


        //public async Task<ResponseDTO> ConfirmBookingAsync(Guid bookingId)
        //{
        //    try
        //    {
        //        // 1️⃣ Kiểm tra Booking có tồn tại không
        //        var booking = await _unitOfWork.BookingRepo.GetByIdAsync(bookingId);
        //        if (booking == null)
        //        {
        //            return new ResponseDTO("Booking not found", 404, false);
        //        }

        //        // 2️⃣ Kiểm tra trạng thái Booking (Chỉ cập nhật nếu là Pending)
        //        if (booking.status != BookingStatus.PENDING)
        //        {
        //            return new ResponseDTO("Booking is not in a valid state for confirmation", 400, false);
        //        }

        //        var coordinates = await _osmService.GetCoordinatesFromAddressAsync(booking.location);
        //        if (coordinates == null)
        //        {
        //            return new ResponseDTO("Failed to retrieve coordinates for booking location", 400, false);
        //        }

        //        // ✅ Luôn cập nhật tọa độ
        //        booking.latitude = coordinates.latitude;
        //        booking.longitude = coordinates.longitude;



        //        // 4️⃣ Tìm trạm cứu hộ gần nhất
        //        var nearestStation = await _rescueStationService.FindNearestStationAsync(booking.latitude ?? 0.0, booking.longitude ?? 0.0);
        //        if (nearestStation == null)
        //        {
        //            return new ResponseDTO("No rescue station found nearby", 404, false);
        //        }

        //        // 5️⃣ Chọn nhân viên phù hợp từ trạm đó
        //        var availableStaff = await _staffService.GetAvailableStaffAsync(nearestStation.id);
        //        if (availableStaff == null || !availableStaff.Any())
        //        {
        //            return new ResponseDTO("No available staff at the nearest rescue station", 404, false);
        //        }

        //        // 6️⃣ Cập nhật thông tin RescueStationId và Staff cho Booking
                
        //        booking.rescueStationId = nearestStation.id;

        //        // ✅ Thêm tất cả nhân viên vào BookingStaff
        //        var bookingStaffList = availableStaff.Select(staff => new BookingStaff
        //        {
        //            id = Guid.NewGuid(),
        //            bookingId = booking.id,
        //            staffId = staff.id
        //        }).ToList();
        //        //Console.WriteLine(booking.Latitude);
        //        //Console.WriteLine(booking.Longitude);
        //        await _unitOfWork.BookingStaffRepo.AddRangeAsync(bookingStaffList);
        //        await _unitOfWork.BookingRepo.UpdateAsync(booking);
        //        await _unitOfWork.SaveChangeAsync();

        //        return new ResponseDTO("Booking confirmed and assigned to nearest rescue station", 200, true, new
        //        {
        //            BookingId = booking.id,
        //            RescueStation = nearestStation.name,
        //            AssignedStaffIds = bookingStaffList.Select(bs => bs.staffId).ToList() // Trả về danh sách StaffId
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseDTO($"Error: {ex.Message}", 500, false);
        //    }
        //}



        public async Task<ResponseDTO> AssignStaffToBookingAsync(Guid bookingId, List<Guid> staffIds)
        {
            try
            {
                // 1️⃣ Kiểm tra Booking có tồn tại không
                var booking = await _unitOfWork.BookingRepo.GetByIdAsync(bookingId);
                if (booking == null)
                {
                    return new ResponseDTO("Không tìm thấy booking !!!", 200, false);
                }

                //// 2️⃣ Kiểm tra trạng thái Booking (Chỉ assign staff khi đã Confirmed)
                //if (booking.status != BookingStatus.PENDING)
                //{
                //    return new ResponseDTO("Booking is not confirmed yet", 400, false);
                //}

                // 3️⃣ Kiểm tra danh sách staff có hợp lệ không
                if (staffIds == null || !staffIds.Any())
                {
                    return new ResponseDTO("Staff ID list is empty", 200, false);
                }

                // 4️⃣ Lấy danh sách staff theo ID
                var selectedStaffs = await _unitOfWork.UserRepo.GetUsersByIdsAsync(staffIds);
                if (selectedStaffs.Count != staffIds.Count)
                {
                    return new ResponseDTO("Không có staff hợp lệ", 200, false);
                }

                var assignedStaffs = new List<BookingStaff>();

                foreach (var staff in selectedStaffs)
                {
                    // 5️⃣ Gán Staff vào Booking
                    var bookingStaff = new BookingStaff
                    {
                        id = Guid.NewGuid(),
                        bookingId = bookingId,
                        staffId = staff.id
                    };
                    assignedStaffs.Add(bookingStaff);
                    await _unitOfWork.BookingStaffRepo.AddAsync(bookingStaff);

                    // 6️⃣ Cập nhật trạng thái Staff → Inactive
                    staff.staffStatus = staffStatus.INACTIVE;
                    await _unitOfWork.UserRepo.UpdateAsync(staff);
                }

                // 7️⃣ Cập nhật trạng thái Booking → InProgress
                booking.status = BookingStatus.COMING;
                //booking.arrivalDate = DateTime.UtcNow;
                await _unitOfWork.BookingRepo.UpdateAsync(booking);

                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO($"Assigned {selectedStaffs.Count} vào booking", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }




        // CHECK CONFIRM STAFF 
        // LẤY THG STAFF    
        // ĐỔI STATUS - ID
        // LẤY STATUS CONFIRM ( 2 THẰNG )
        // FALSE ( COMING - COMFIRM STATUS )
        // TRUE ( IN_PROGRESS )



        public async Task<ResponseDTO> AddServiceToBookingAsync(Guid bookingId, List<Guid> serviceIds)
        {
            var booking = await _unitOfWork.BookingRepo.GetByIdWithBookingStaffsAsync(bookingId);
            if (booking == null)
                return new ResponseDTO("Không tìm thấy booking", 200, false);

            decimal totalPrice = 0;
            decimal totalDiscount = 0;
            bool servicesAdded = false;

            // Kiểm tra xem Booking có Vehicle không
            Vehicle? vehicle = null;
            bool isPackageExpired = true;
            Guid? packageId = null;

            if (booking.vehicleId.HasValue)
            {
                vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(booking.vehicleId.Value);
                if (vehicle != null && vehicle.packageId.HasValue)
                {
                    packageId = vehicle.packageId;

                    // Kiểm tra gói còn hạn không
                    if (vehicle.expirationDate.HasValue && vehicle.expirationDate.Value >= DateTime.UtcNow)
                    {
                        isPackageExpired = false;
                    }
                }
            }
            
            foreach (var serviceId in serviceIds)
            {
                var service = await _unitOfWork.ServiceRepo.GetByIdAsync(serviceId);
                if (service == null) continue;

                var existingService = await _unitOfWork.ServiceOfBookingRepo.GetByBookingAndServiceAsync(bookingId, serviceId);
                if (existingService != null)
                {
                    await _unitOfWork.ServiceOfBookingRepo.DeleteAsync(existingService.id);
                }

                var newService = new ServiceOfBooking
                {
                    bookingId = bookingId,
                    serviceId = serviceId
                };
                await _unitOfWork.ServiceOfBookingRepo.AddAsync(newService);
                servicesAdded = true;

                decimal originalPrice = service.price;
                decimal discountAmount = 0;

                // Nếu có package và gói chưa hết hạn, kiểm tra xem dịch vụ có miễn phí không
                if (!isPackageExpired && packageId.HasValue)
                {
                    var serviceInPackage = await _unitOfWork.PackageRepo.GetServiceInPackageAsync(packageId.Value, serviceId);
                    if (serviceInPackage != null)
                    {
                        discountAmount = originalPrice;
                    }
                }

                totalDiscount += discountAmount;
                totalPrice += (originalPrice - discountAmount); // Chỉ cộng số tiền còn lại sau khi giảm giá
            }

            if (!servicesAdded)
            {
                return new ResponseDTO("Không có service nào đã add vô booking !!! lỗi code", 200, false);
            }

            // Cập nhật tổng giá Booking
            booking.totalPrice = totalPrice;
            await _unitOfWork.BookingRepo.UpdateAsync(booking);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Đã thêm Service và update Tổng tiền", 200, true, new
            {
                TotalPrice = totalPrice,
                TotalDiscount = totalDiscount
                //PackageExpired = isPackageExpired // Trả về thông tin gói có hết hạn không

            });
        }

        public async Task<ResponseDTO> AcceptBooking(Guid id)
        {
            var booking = await _unitOfWork.BookingRepo.GetByIdAsync(id);
            if (booking == null)
            {
                return new ResponseDTO("Không tìm thấy booking !!!", 200, false);
            }

            if (booking.bookingType == TypeBooking.MEMBER)
            {
                booking.status = BookingStatus.PENDING_PAYMENT;
            }
            else
            {
                booking.status = BookingStatus.IN_PROGRESS; 
            }


            await _unitOfWork.SaveChangeAsync();
            return new ResponseDTO("Booking đã được chấp nhận", 200, true);
        }

        public async Task<ResponseDTO> CompleteOrCancelBookingAsync(Guid bookingId, bool isCompleted)
        {
            var userId = _userUtility.GetUserIdFromToken();
            var booking = await _unitOfWork.BookingRepo.GetByIdWithBookingStaffsAsync(bookingId);
            if (booking == null)
                return new ResponseDTO("Không tìm thấy booking !!!", 200, false);

            decimal amount = booking.totalPrice.GetValueOrDefault();

            if (isCompleted)
            {

                // ✅ **Tạo transaction thanh toán**
                var transaction = new Transaction
                {
                    id = Guid.NewGuid(),
                    userId = booking.customerId ?? new Guid(""),
                    amount = amount,
                    createdAt = DateTime.UtcNow,
                    status = Transaction.TransactionStatus.PENDING,
                    bookingId = bookingId
                };

                await _unitOfWork.TransactionRepo.AddAsync(transaction);
                await _unitOfWork.SaveChangeAsync();

                // ✅ Lấy IP Address của người dùng
                var ipAddress = "127.0.0.1"; // Hoặc lấy từ request

                // ✅ Tạo PaymentRequest đúng format
                var paymentRequest = new PaymentRequest
                {
                    PaymentId = DateTime.UtcNow.Ticks,
                    Money = (double)transaction.amount,
                    Description = $"{transaction.id}/booking",
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY, // Cho phép chọn ngân hàng
                    CreatedDate = DateTime.UtcNow,
                    Currency = Currency.VND,
                    Language = DisplayLanguage.Vietnamese
                };

                // ✅ Gọi đúng `CreatePaymentUrlAsync`
                var paymentUrl = await _vpnPayService.CreatePaymentUrlAsync(paymentRequest, userId, bookingId,null, null ,transaction.id);

                return new ResponseDTO("Redirect to payment", 200, true, paymentUrl);
            }

            
            else
            {
                //  Nếu bị hủy, chỉ cập nhật trạng thái booking
                booking.status = BookingStatus.CANCELLED;
                booking.completedDate = DateTime.UtcNow;

                await _unitOfWork.BookingRepo.UpdateAsync(booking);

                // **Kích hoạt lại nhân viên**
                foreach (var staffBooking in booking.BookingStaffs)
                {
                    var staff = await _unitOfWork.UserRepo.GetByIdAsync(staffBooking.staffId);
                    staff.staffStatus = staffStatus.ACTIVE;
                    await _unitOfWork.UserRepo.UpdateAsync(staff);
                }

                await _unitOfWork.SaveChangeAsync();
                return new ResponseDTO("Booking đã bị hủy !!!", 200, true, null);
            }
        }

        public async Task<ResponseDTO> FinishedBooking(Guid id)
        {
            var booking = await _unitOfWork.BookingRepo.GetByIdWithBookingStaffsAsync(id);
            if (booking == null)
            {
                return new ResponseDTO("Không tìm thấy booking !!!", 200, false);
            }
            booking.status = BookingStatus.FINISHED;
            booking.completedDate = DateTime.UtcNow;
            // **Kích hoạt lại nhân viên**
            foreach (var staffBooking in booking.BookingStaffs)
            {
                var staff = await _unitOfWork.UserRepo.GetByIdAsync(staffBooking.staffId);
                staff.staffStatus = staffStatus.ACTIVE;
                await _unitOfWork.UserRepo.UpdateAsync(staff);
            }

            await _unitOfWork.SaveChangeAsync();
            await _unitOfWork.BookingRepo.UpdateAsync(booking);
            await _unitOfWork.SaveChangeAsync();
            return new ResponseDTO("Đã hoàn thành ", 200, true);
        }

        public async Task<ResponseDTO> GetAllBookingAsync()
        {
            var bookings = await _unitOfWork.BookingRepo.GetAllBookingsForManagerAsync();

            if (!bookings.Any())
            {
                return new ResponseDTO("Không tìm thấy booking nào", 404, false);
            }

            // Map bookings to GetAllBookingDTO
            var allbookingDTO = bookings.Select(booking => new GetAllBookingDTO
            {
                id = booking.id,
                name = booking.Customer?.fullName ?? booking.customerName ?? "không xác định", // Name of the customer
                phone = booking.Customer?.phone ?? booking.phone?? "không xác định", // Phone of the customer
                description = booking.description ?? "Không xác định",
                status = BookingStatusMap.GetValueOrDefault(booking.status, "Không xác định"),
                totalPrice = booking.totalPrice ?? 0,
                licensePlate = booking.Vehicle?.licensePlate ?? booking.licensePlate  ??"Không xác định", // License Plate of the vehicle
                location = string.IsNullOrEmpty(booking.location) ? "Không xác định" : Uri.UnescapeDataString(booking.location), // Giải mã location
                evidence = booking.evidence ?? "Không xác định", // Evidence of booking
                arrivalDate = booking.arrivalDate,
                completedDate = booking.completedDate,

                // Mapping services
                services = booking.ServiceBookings?
                    .Select(s => new ServiceDetailInBookingDTO
                    {
                        id = s.Service.id,
                        name = s.Service.name,
                        price = s.Service.price
                    }).ToList() ?? new List<ServiceDetailInBookingDTO>(),

                // Mapping staff1 and staff2
                staff1 = booking.BookingStaffs?.ElementAtOrDefault(0) != null
                    ? new StaffDTO
                    {
                        id = booking.BookingStaffs.ElementAtOrDefault(0)?.Staff.id,
                        fullName = booking.BookingStaffs.ElementAtOrDefault(0)?.Staff?.fullName ?? "Không xác định",
                        phone = booking.BookingStaffs.ElementAtOrDefault(0)?.Staff?.phone ?? "Không xác định"
                    }
                    : new StaffDTO(), // Fallback to an empty StaffDTO if no staff1

                staff2 = booking.BookingStaffs?.ElementAtOrDefault(1) != null
                    ? new StaffDTO
                    {
                        id = booking.BookingStaffs.ElementAtOrDefault(0)?.Staff.id,
                        fullName = booking.BookingStaffs.ElementAtOrDefault(1)?.Staff?.fullName ?? "Không xác định",
                        phone = booking.BookingStaffs.ElementAtOrDefault(1)?.Staff?.phone ?? "Không xác định"
                    }
                    : new StaffDTO() // Fallback to an empty StaffDTO if no staff2
            }).ToList(); // Ensure that it's a list of DTOs

            var sortList = _support.SortByArrivalDate(allbookingDTO, b => b.arrivalDate);

            return new ResponseDTO("Lấy tất cả booking thành công", 200, true, sortList);
        }

        public async Task<ResponseDTO> GetBookingByCustomerIdAsync(Guid? customerId = null)
        {
            customerId ??= _userUtility.GetUserIdFromToken();

            var bookings = await _unitOfWork.BookingRepo.GetBookingsByCustomerIdAsync(customerId.Value);

            if (!bookings.Any())
            {
                return new ResponseDTO("Không tìm thấy booking nào", 200, false);
            }

            var bookingDTOs = bookings.Select(booking => new BookingByUserIdDTO
            {
                id = booking.id,
                arrivalDate = booking.arrivalDate,
                completedDate = booking.completedDate,
                Description = booking.description ?? "Không xác định",
                Status = BookingStatusMap.GetValueOrDefault(booking.status, "Không xác định"),
                TotalPrice = booking.totalPrice ?? 0,
                LicensePlate = booking.Vehicle?.licensePlate ?? "Không xác định"
            }).ToList();

            return new ResponseDTO("Lấy dữ liệu thành công", 200, true, bookingDTOs);
        }



        /// <summary>
        /// Lấy thông tin Booking theo ID
        /// </summary>
        public async Task<ResponseDTO> GetBookingByIdAsync(Guid bookingId)
        {
            // Fetch a single booking (assuming GetBookingsForHistoryAsync might return multiple bookings)
            var booking = (await _unitOfWork.BookingRepo.GetBookingForHistoryAsync(bookingId));

            if (booking == null)
            {
                return new ResponseDTO("Không tìm thấy booking với ID này", 404, false);
            }

            var bookingDTOs = new DetailBookingDTO
            {
                id = booking.id,
                name = booking.Customer?.fullName ?? "Không xác định",
                phone = booking.Customer?.phone ?? "Không xác định",
                licensePlate = booking.Vehicle?.licensePlate ?? "Không xác định",
                location = string.IsNullOrEmpty(booking.location) ? "Không xác định" : Uri.UnescapeDataString(booking.location), // Giải mã location
                description = booking.description ?? "không xác định",
                evidence = booking.evidence ?? "Không xác định",
                status = BookingStatusMap.GetValueOrDefault(booking.status, "Không xác định"),
                arrivalDate = booking.arrivalDate ?? DateTime.MinValue,
                completedDate = booking.completedDate ?? DateTime.MinValue,

                services = booking.ServiceBookings?
                    .Select(s => new ServiceDetailInBookingDTO
                    {
                        id = s.Service.id,
                        name = s.Service.name
                    }).ToList() ?? new List<ServiceDetailInBookingDTO>(),

                totalPrice = booking.totalPrice ?? 0,

                // ✅ Lấy nhân viên thứ 1 (nếu có)
                staff1 = booking.BookingStaffs?.ElementAtOrDefault(0) != null
                    ? new StaffDTO
                    {
                        fullName = booking.BookingStaffs.ElementAtOrDefault(0)?.Staff?.fullName ?? "Không xác định",
                        phone = booking.BookingStaffs.ElementAtOrDefault(0)?.Staff?.phone ?? "Không xác định"
                    }
                    : new StaffDTO(),

                // ✅ Lấy nhân viên thứ 2 (nếu có)
                staff2 = booking.BookingStaffs?.ElementAtOrDefault(1) != null
                    ? new StaffDTO
                    {
                        fullName = booking.BookingStaffs.ElementAtOrDefault(1)?.Staff?.fullName ?? "Không xác định",
                        phone = booking.BookingStaffs.ElementAtOrDefault(1)?.Staff?.phone ?? "Không xác định"
                    }
                    : new StaffDTO()
            };

            return new ResponseDTO("Lấy thông tin booking thành công", 200, true, bookingDTOs);
        }

        /// <summary>
        /// Lấy danh sách Booking của một Staff
        /// </summary>
        public async Task<ResponseDTO> GetBookingsByStaffAsync(Guid staffId)
        {
            // Lấy danh sách bookings mà nhân viên tham gia
            var staffBookings = await _unitOfWork.BookingRepo.GetBookingsByStaffIdAsync(staffId);
            if (staffBookings == null || !staffBookings.Any())
            {
                return new ResponseDTO("Không tìm thấy booking nào cho staff này", 404, false);
            }

            // Tạo danh sách BookingDTO để trả về thông tin chi tiết booking và nhân viên
            var bookingDTOs = new List<DetailBookingDTO>();

            foreach (var booking in staffBookings)
            {
                var bookingDTO = new DetailBookingDTO
                {
                    id = booking.id,
                    name = booking.Customer?.fullName ?? booking.customerName ?? "",
                    phone = booking.Customer?.phone ?? booking.phone ?? "",
                    licensePlate = booking.Vehicle?.licensePlate ?? booking.licensePlate ?? "",
                    location = string.IsNullOrEmpty(booking.location) ? "Không xác định" : Uri.UnescapeDataString(booking.location),
                    description = booking.description ??  "",
                    evidence = booking.evidence ?? "Không xác định",
                    //status = BookingStatusMap.GetValueOrDefault(booking.status, "Không xác định"),
                    status = booking.status.ToString(),
                    arrivalDate = booking.arrivalDate ?? DateTime.MinValue,
                    completedDate = booking.completedDate ?? DateTime.MinValue,

                    services = booking.ServiceBookings?
                        .Select(s => new ServiceDetailInBookingDTO
                        {
                            id = s.Service.id,
                            name = s.Service.name
                        }).ToList() ?? new List<ServiceDetailInBookingDTO>(),

                    totalPrice = booking.totalPrice ?? 0
                };

                // Lấy danh sách nhân viên trong booking có staffId tương ứng
                var staffInBooking = booking.BookingStaffs
                    .Where(bs => bs.bookingId == booking.id)
                    
                    .ToList();

                if (staffInBooking.Any())
                {
                    bookingDTO.staff1 = new StaffDTO
                    {
                        id = staffInBooking[0].Staff?.id,
                        fullName = staffInBooking[0].Staff?.fullName ?? "Không xác định",
                        phone = staffInBooking[0].Staff?.phone ?? "Không xác định",
                        confirmStaff = staffInBooking[0].confirmStaff // Convert từ bool thành string ("true" hoặc "false")
                    };

                    
                    bookingDTO.staff2 = new StaffDTO
                        {
                            id = staffInBooking[1].Staff?.id,
                            fullName = staffInBooking[1].Staff?.fullName ?? "Không xác định",
                            phone = staffInBooking[1].Staff?.phone ?? "Không xác định",
                            confirmStaff = staffInBooking[1].confirmStaff
                        };
                    
                }

                bookingDTOs.Add(bookingDTO);
            }

            var sortList = _support.SortByArrivalDate(bookingDTOs, b => b.arrivalDate);

            // Trả về danh sách các booking cùng thông tin nhân viên
            return new ResponseDTO("Lấy danh sách booking thành công", 200, true, sortList);
        }



        public async Task<ResponseDTO> UpdateBookingToInProgressAsync(Guid bookingId)
        {
            try
            {
                var foundBooking = await _unitOfWork.BookingRepo.GetByIdAsync(bookingId);
                if (foundBooking == null)
                {
                    return new ResponseDTO("Không tìm thấy booking với ID này", 404, false);
                }

                // Cập nhật trạng thái và thời gian đến
                foundBooking.status = BookingStatus.IN_PROGRESS;
                //foundBooking.arrivalDate = DateTime.UtcNow;

                await _unitOfWork.BookingRepo.UpdateAsync(foundBooking);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Cập nhật trạng thái booking thành công", 200, true, foundBooking);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Lỗi: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> ConfirmStaffArrivalAsync(Guid bookingId, Guid staffId)
        {
            var booking = await _unitOfWork.BookingRepo.GetBookingForHistoryAsync(bookingId);
            if (booking == null)
            {
                return new ResponseDTO("Không tìm thấy Booking", 404, false);
            }

            var bookingStaffs = await _unitOfWork.BookingStaffRepo.GetBookingStaffsByBookingIdAsync(bookingId);

            var staff = bookingStaffs.FirstOrDefault(bs => bs.staffId == staffId);
            if (staff == null)
            {
                return new ResponseDTO("Nhân viên không thuộc Booking này", 400, false);
            }

            // Cập nhật confirmStaff cho nhân viên hiện tại
            staff.confirmStaff = true;
            await _unitOfWork.BookingStaffRepo.UpdateAsync(staff);

            // Kiểm tra xem tất cả nhân viên đã xác nhận chưa
            bool allConfirmed = bookingStaffs.All(bs => bs.confirmStaff == true);

            if (allConfirmed)
            {
                booking.status = BookingStatus.CHECKING;
                booking.arrivalDate = DateTime.UtcNow;
                await _unitOfWork.BookingRepo.UpdateAsync(booking);
            }

            await _unitOfWork.SaveChangeAsync();

            // Trả về thông tin Booking với StaffDTO mới
            var bookingDTOs = new DetailBookingDTO
            {
                id = booking.id,
                name = booking.Customer?.fullName ?? "Không xác định",
                phone = booking.Customer?.phone ?? "Không xác định",
                licensePlate = booking.Vehicle?.licensePlate ?? "Không xác định",
                location = string.IsNullOrEmpty(booking.location) ? "Không xác định" : Uri.UnescapeDataString(booking.location), // Giải mã location
                description = booking.description ?? "không xác định",
                evidence = booking.evidence ?? "Không xác định",
                status = BookingStatusMap.GetValueOrDefault(booking.status, "Không xác định"),
                arrivalDate = booking.arrivalDate ?? DateTime.MinValue,
                completedDate = booking.completedDate ?? DateTime.MinValue,

                services = booking.ServiceBookings?
                    .Select(s => new ServiceDetailInBookingDTO
                    {
                        id = s.Service.id,
                        name = s.Service.name
                    }).ToList() ?? new List<ServiceDetailInBookingDTO>(),

                totalPrice = booking.totalPrice ?? 0,

                // ✅ Lấy thông tin nhân viên từ bookingStaffs
                staff1 = bookingStaffs?.ElementAtOrDefault(0) != null
                    ? new StaffDTO
                    {
                        fullName = bookingStaffs.ElementAtOrDefault(0)?.Staff?.fullName ?? "Không xác định",
                        phone = bookingStaffs.ElementAtOrDefault(0)?.Staff?.phone ?? "Không xác định",
                        confirmStaff = bookingStaffs.ElementAtOrDefault(0)?.confirmStaff == true ? true : false
                    }
                    : new StaffDTO(),

                staff2 = bookingStaffs?.ElementAtOrDefault(1) != null
                    ? new StaffDTO
                    {
                        fullName = bookingStaffs.ElementAtOrDefault(1)?.Staff?.fullName ?? "Không xác định",
                        phone = bookingStaffs.ElementAtOrDefault(1)?.Staff?.phone ?? "Không xác định",
                        confirmStaff = bookingStaffs.ElementAtOrDefault(0)?.confirmStaff == true ? true : false
                    }
                    : new StaffDTO()
            };

            return new ResponseDTO(
                allConfirmed ? "Tất cả nhân viên đã xác nhận, chuyển sang IN_PROGRESS" : "Nhân viên đã xác nhận, chờ người còn lại",
                200, true, bookingDTOs
            );
        }
        public async Task<ResponseDTO> GetBookingGuest()
        {
            var bookings = await _unitOfWork.BookingRepo.GetAllBookingsGuest();

            if (!bookings.Any())
            {
                return new ResponseDTO("Không tìm thấy booking nào", 200, true);
            }

            // Map bookings to GetAllBookingDTO
            var allbookingDTO = bookings.Select(booking => new GetAllBookingDTO
            {
                id = booking.id,
                name = booking.Customer?.fullName ?? booking.customerName ?? "không xác định", // Name of the customer
                phone = booking.Customer?.phone ?? booking.phone ?? "không xác định", // Phone of the customer
                description = booking.description ?? "Không xác định",
                status = BookingStatusMap.GetValueOrDefault(booking.status, "Không xác định"),
                totalPrice = booking.totalPrice ?? 0,
                licensePlate = booking.Vehicle?.licensePlate ?? booking.licensePlate ?? "Không xác định", // License Plate of the vehicle
                location = string.IsNullOrEmpty(booking.location) ? "Không xác định" : Uri.UnescapeDataString(booking.location), // Giải mã location
                evidence = booking.evidence ?? "Không xác định", // Evidence of booking
                arrivalDate = booking.arrivalDate,
                completedDate = booking.completedDate,

                // Mapping services
                services = booking.ServiceBookings?
                    .Select(s => new ServiceDetailInBookingDTO
                    {
                        id = s.Service.id,
                        name = s.Service.name,
                        price = s.Service.price
                    }).ToList() ?? new List<ServiceDetailInBookingDTO>(),

                // Mapping staff1 and staff2
                staff1 = booking.BookingStaffs?.ElementAtOrDefault(0) != null
                    ? new StaffDTO
                    {
                        id = booking.BookingStaffs.ElementAtOrDefault(0)?.Staff.id,
                        fullName = booking.BookingStaffs.ElementAtOrDefault(0)?.Staff?.fullName ?? "Không xác định",
                        phone = booking.BookingStaffs.ElementAtOrDefault(0)?.Staff?.phone ?? "Không xác định"
                    }
                    : new StaffDTO(), // Fallback to an empty StaffDTO if no staff1

                staff2 = booking.BookingStaffs?.ElementAtOrDefault(1) != null
                    ? new StaffDTO
                    {
                        id = booking.BookingStaffs.ElementAtOrDefault(0)?.Staff.id,
                        fullName = booking.BookingStaffs.ElementAtOrDefault(1)?.Staff?.fullName ?? "Không xác định",
                        phone = booking.BookingStaffs.ElementAtOrDefault(1)?.Staff?.phone ?? "Không xác định"
                    }
                    : new StaffDTO() // Fallback to an empty StaffDTO if no staff2
            }).ToList(); // Ensure that it's a list of DTOs

            return new ResponseDTO("Lấy dữ liệu thành công", 200, true, allbookingDTO);
        }
        public async Task<ResponseDTO> CreateBookingforReceptionist(ReBookingDTO reBookingDTO)
        {

            try
            {
                //// Kiểm tra Customer có tồn tại không
                //var customer = await _unitOfWork.UserRepo.GetByIdAsync(_userUtility.GetUserIdFromToken());
                //if (customer == null)
                //{
                //    return new ResponseDTO("Customer not found", 404, false);
                //}

                //// Kiểm tra VehicleId hoặc LicensePlate hợp lệ để lấy PackageId
                Vehicle? vehicle = null;
                Guid? packageId = null;
                Guid? customerId = null;



                if (vehicle == null && !string.IsNullOrEmpty(reBookingDTO.licensePlate))
                {
                    vehicle = await _unitOfWork.VehicleRepo.GetByLicensePlateAsync(reBookingDTO.licensePlate);
                }

                // Nếu tìm được vehicle, lấy PackageId
                if (vehicle != null)
                {
                    packageId = vehicle.packageId;
                    
                }
                

                // Mã hóa location để tránh lỗi ký tự đặc biệt
                string encodedAddress = Uri.EscapeDataString(reBookingDTO.location);

                // Tạo Booking mới
                var newBooking = new Booking
                {
                    id = Guid.NewGuid(),
                    customerId = customerId,
                    customerName = reBookingDTO.customerName,
                    vehicleId = vehicle?.id,
                    description = reBookingDTO.description,
                    evidence =  "null",
                    location = encodedAddress,
                    bookingDate = DateTime.UtcNow,
                    //--------------------------
                    status = BookingStatus.PENDING,
                    packageId = packageId,
                    licensePlate = reBookingDTO.licensePlate,
                    phone = reBookingDTO.phone,
                    bookingType = TypeBooking.GUEST
                };

                // Lưu vào DB
                await _unitOfWork.BookingRepo.AddAsync(newBooking);
                await _unitOfWork.SaveChangeAsync();

                // Trả về ResponseDTO
                return new ResponseDTO("Tạo booking thành công", 200, true, newBooking);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"lỗi: {ex.Message}", 500, false);
            }

        }






        private static readonly Dictionary<BookingStatus, string> BookingStatusMap = new()
{
    { BookingStatus.DEPOSIT, "DEPOSIT" },
    { BookingStatus.PENDING, "PENDING" },
    { BookingStatus.COMING, "COMING" },
    { BookingStatus.CHECKING, "CHECKING" },
    { BookingStatus.IN_PROGRESS, "IN_PROGRESS" },
    { BookingStatus.CANCELLED, "CANCELLED" },
    { BookingStatus.FINISHED, "FINISHED" },
    { BookingStatus.PENDING_PAYMENT, "PENDING_PAYMENT" }
};

    }

}
