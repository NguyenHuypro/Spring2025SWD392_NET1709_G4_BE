using CarRescueSystem.DAL.Repository.Interface;
using AutoMapper;
using CarRescueSystem.Common.DTO.Vehicle;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.DAL.UnitOfWork;
using CarRescueSystem.BLL.Utilities;
using CarRescueSystem.Common.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using System.Text.RegularExpressions;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class VehicleService : IVehicleService
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserUtility _userUtility;
        //private readonly IWalletService _walletService;
        private readonly IVnPayService _vpnPayService;

        public VehicleService(IUnitOfWork unitOfWork, UserUtility userUtility,  IVnPayService vnPayService)
        {
            
            _unitOfWork = unitOfWork;
            _userUtility = userUtility;
            //_walletService = walletService;
            _vpnPayService = vnPayService;
        }

        public async Task<ResponseDTO> CreateCar(CreateVehicleDTO request)
        {
            try
            {
                //check customer
                var customer = await _unitOfWork.UserRepo.GetByIdAsync(_userUtility.GetUserIdFromToken());
                if (customer == null)
                {
                    return new ResponseDTO("không thấy người dùng", 200, false);
                }

                //if (!IsValidLicensePlate(request.licensePlate))
                //{
                //    return new ResponseDTO("biển số xe không hợp lệ", 200, false);
                //}

                var checkLicensePlate = await _unitOfWork.VehicleRepo.GetByLicensePlateAsync(request.licensePlate);

                if (checkLicensePlate != null)
                {
                    return new ResponseDTO("không được trùng biển số xe", 200, false);
                }


                int[] validSeats = { 4, 5, 7, 16, 29, 32, 45 };

                if (request.numberOfSeats == null || !validSeats.Contains(request.numberOfSeats))
                {
                    return new ResponseDTO("Số chỗ ngồi cho xe cứu hộ chỉ có thể là 4, 5, 7, 16, 29, 32 hoặc 45!", 200, false);
                }



                var vehicle = new Vehicle{
                    customerId = customer.id,
                    id = Guid.NewGuid(),
                    model = request.model,
                    color = request.color,
                    brand = request.brand,
                    numberOfSeats = request.numberOfSeats,
                    licensePlate = request.licensePlate
                };

                await _unitOfWork.VehicleRepo.AddAsync(vehicle);
                await _unitOfWork.SaveChangeAsync();

                // Trả về ResponseDTO
                return new ResponseDTO("tạo xe thành công", 200, true, vehicle);
            }
                catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> GetVehicleByIDAsync(Guid id)
        {
            try
            {
                var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(id);
                if (vehicle == null)
                    return new ResponseDTO("No Vehicle with this ID found!", 404, false);


                return new ResponseDTO("Vehicle found successfully", 201, true, vehicle);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> GetAllVehicleAsync()
        {
            try
            {
                var vehicles = await _unitOfWork.VehicleRepo.ToListAsync();
                 if (!vehicles.Any())
                    return new ResponseDTO($"Error: {"No Vehicle found!"}", 404, false);

                return new ResponseDTO("Vehicle found successfully", 201, true, vehicles);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> UpdateAsync(Guid id, UpdateVehicleDTO request)
        {
            try
            {
                var oldvehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(id);
                if (oldvehicle == null)
                {
                    return new ResponseDTO("Error: No Vehicle with this ID found!", 404, false);
                }

                Console.WriteLine(request.model);
                Console.WriteLine(request.brand);
                Console.WriteLine(request.numberOfSeats);
                Console.WriteLine(request.color);

                // Cập nhật nếu dữ liệu hợp lệ
                if (!string.IsNullOrWhiteSpace(request.color))
                    oldvehicle.color = request.color;

                if (!string.IsNullOrWhiteSpace(request.brand))
                    oldvehicle.brand = request.brand;

                if (!string.IsNullOrWhiteSpace(request.model))
                    oldvehicle.model = request.model;

                if (request.numberOfSeats > 0)
                    oldvehicle.numberOfSeats = request.numberOfSeats;

                Console.WriteLine(oldvehicle.model);
                Console.WriteLine(oldvehicle.brand);
                Console.WriteLine(oldvehicle.numberOfSeats);
                Console.WriteLine(oldvehicle.color);





                await _unitOfWork.VehicleRepo.UpdateAsync(oldvehicle);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Vehicle updated successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }


        public async Task<ResponseDTO> DeleteAsync(Guid id)
        {
            try
            {
                var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(id);
                if (vehicle == null)
                    return new ResponseDTO($"Error: {"không thấy xe"}", 404, false);
                await _unitOfWork.VehicleRepo.DeleteAsync(id);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Vehicle deleted successfully", 200, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

        public async Task<ResponseDTO> PurchasePackage(Guid vehicleId, Guid packageId)
        {
            var userId = _userUtility.GetUserIdFromToken();
            var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(vehicleId);

            if (vehicle == null || vehicle.customerId != userId)
                return new ResponseDTO("Vehicle not found or unauthorized", 403, false);

            var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId);
            if (package == null)
                return new ResponseDTO("Package not found", 404, false);

            // ✅ Tạo giao dịch mới trước khi thanh toán
            var transaction = new Transaction
            {
                id = Guid.NewGuid(), // Tạo ID giao dịch
                userId = userId,
                packageId = packageId,
                carId = vehicleId, // Liên kết xe
                amount = package.price,
                createdAt = DateTime.UtcNow,
                status = Transaction.TransactionStatus.PENDING
            };

            await _unitOfWork.TransactionRepo.AddAsync(transaction);
            await _unitOfWork.SaveChangeAsync(); // ✅ Lưu vào DB

            // ✅ Lấy IP Address của người dùng
            var ipAddress = "127.0.0.1"; // Hoặc lấy từ request

            // ✅ Tạo PaymentRequest đúng format
            var paymentRequest = new PaymentRequest
            {
                PaymentId = DateTime.UtcNow.Ticks,
                Money = (double)transaction.amount,
                Description = $"{transaction.id}/package",
                IpAddress = ipAddress,
                BankCode = BankCode.ANY, // Cho phép chọn ngân hàng
                CreatedDate = DateTime.UtcNow,
                Currency = Currency.VND,
                Language = DisplayLanguage.Vietnamese,
                
            };

            // ✅ Gọi đúng `CreatePaymentUrlAsync`
            var paymentUrl = await _vpnPayService.CreatePaymentUrlAsync(paymentRequest, userId, null, packageId, vehicleId, transaction.id);

            return new ResponseDTO("Redirect to payment", 200, true, paymentUrl);
        }



        public async Task<ResponseDTO> GetVehiclePackage(Guid vehicleId)
        {
            var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(vehicleId);
            if (vehicle == null || vehicle.packageId == null)
                return new ResponseDTO("No package found for this vehicle", 404, false);

            var package = await _unitOfWork.PackageRepo.GetByIdAsync(vehicle.packageId.Value);
            var response = new
            {
                PackageId = package.id,
                PackageName = package.name,
          
                ExpirationDate = vehicle.expirationDate
            };

            return new ResponseDTO("Package retrieved successfully", 200, true, response);
        }

        public async Task<ResponseDTO> RemovePackage(Guid vehicleId)
        {
            var userId = _userUtility.GetUserIdFromToken();
            var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(vehicleId);

            if (vehicle == null || vehicle.packageId == null)
                return new ResponseDTO("No package found for this vehicle", 404, false);

            if (vehicle.customerId != userId)
                return new ResponseDTO("Unauthorized: You do not own this vehicle", 403, false);

            vehicle.packageId = null;
           
            vehicle.expirationDate = null;

            await _unitOfWork.VehicleRepo.UpdateAsync(vehicle);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Package removed successfully", 200, true);
        }

        //public async Task<ResponseDTO> UpgradePackage(Guid vehicleId, Guid newPackageId)
        //{
        //    var userId = _userUtility.GetUserIdFromToken();
        //    var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(vehicleId);

        //    if (vehicle == null || vehicle.customerId != userId)
        //        return new ResponseDTO("Vehicle not found or unauthorized", 403, false);

        //    var newPackage = await _unitOfWork.PackageRepo.GetByIdAsync(newPackageId);
        //    if (newPackage == null)
        //        return new ResponseDTO("New package not found", 404, false);

        //    if (vehicle.packageId == null)
        //    {
        //        return new ResponseDTO("lôi", 400, false);
        //    }

        //    var currentPackage = await _unitOfWork.PackageRepo.GetByIdAsync(vehicle.packageId.Value);
        //    if (currentPackage == null)
        //        return new ResponseDTO("Current package not found", 404, false);

        //    if (currentPackage.id == newPackage.id)
        //    {
        //        //var paymentUrl = _vpnPayService.CreatePaymentUrl(newPackage.price, $"PACKAGE_{vehicleId}_{newPackageId}");

        //        // ✅ Tạo transaction
        //        var transaction = new Transaction
        //        {
        //            id = Guid.NewGuid(),
        //            userId = userId,
        //            amount = newPackage.price,
        //            createdAt = DateTime.UtcNow,
        //            status = Transaction.TransactionStatus.PENDING,
        //            carId = vehicleId,
        //            packageId = newPackageId
        //        };
        //        await _unitOfWork.TransactionRepo.AddAsync(transaction);
        //        await _unitOfWork.SaveChangeAsync();

        //        return new ResponseDTO("Redirect to payment", 200, true);
        //    }

        //    if (IsUpgradeValid(currentPackage.name, newPackage.name))
        //    {
        //        decimal priceDifference = (newPackage.price as decimal? ?? 0m) - (currentPackage.price as decimal? ?? 0m);
        //        if (priceDifference <= 0)
        //            return new ResponseDTO("Invalid upgrade path", 400, false);

        //        //var paymentUrl = _vpnPayService.CreatePaymentUrl(priceDifference, $"UPGRADE_{vehicleId}_{newPackageId}");

        //        // ✅ Tạo transaction cho nâng cấp
        //        var transaction = new Transaction
        //        {
        //            id = Guid.NewGuid(),
        //            userId = userId,
        //            amount = priceDifference,
        //            createdAt = DateTime.UtcNow,
        //            status = Transaction.TransactionStatus.PENDING,
        //            carId = vehicleId,
        //            packageId = newPackageId
        //        };
        //        await _unitOfWork.TransactionRepo.AddAsync(transaction);
        //        await _unitOfWork.SaveChangeAsync();

        //        return new ResponseDTO("Redirect to payment", 200, true);
        //    }

        //    return new ResponseDTO("Invalid upgrade path", 400, false);
        //}
        //private bool IsUpgradeValid(string currentPackage, string newPackage)
        //{
        //    if (currentPackage == "Gói Cơ Bản" && (newPackage == "Gói Toàn Diện" || newPackage == "Gói Cao Cấp"))
        //        return true;
        //    if (currentPackage == "Gói Toàn Diện" && newPackage == "Gói Cao Cấp")
        //        return true;
        //    return false;
        //}


        public async Task<ResponseDTO> GetCarByUserId()
        {
            var userId = _userUtility.GetUserIdFromToken();

            var myCars = await _unitOfWork.VehicleRepo.GetVehiclesByUserIdAsync(userId);

            if (myCars == null || !myCars.Any())
            {
                return new ResponseDTO("Không tìm thấy xe nào!", 200, true);
            }

            // 🔥 Chuyển danh sách Vehicle thành danh sách GetMyCarDTO
            var carDTOs = myCars.Select(car => new GetMyCarDTO
            {
                id = car.id,
                model = car.model,
                brand = car.brand,
                color = car.color,
                numberOfSeats = car.numberOfSeats,
                licensePlate = car.licensePlate,
                expiredDate = car.expirationDate?.ToString("yyyy-MM-dd") ?? "",
                package = car.Package != null ? new PackageOfCarDTO
                {
                    id = car.Package.id,
                    name = car.Package.name
                } : null
            }).ToList();

            return new ResponseDTO("Lấy danh sách xe thành công!", 200, true, carDTOs);
        }

        private bool IsValidLicensePlate(string licensePlate)
        {
            string pattern = @"^\d{2}[A-Z]\d?-\d{4,5}$";
            return Regex.IsMatch(licensePlate, pattern);
        }


    }
} 