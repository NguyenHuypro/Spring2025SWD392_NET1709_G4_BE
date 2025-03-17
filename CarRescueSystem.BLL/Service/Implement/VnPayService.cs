using System;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using VNPAY.NET;
using VNPAY.NET.Models;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class VnPayService : IVnPayService
    {
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public VnPayService(IVnpay vnpay, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _vnpay = vnpay; // ✅ Không cần gọi _vnpay.Initialize() nữa vì đã làm trong Program.cs
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> CreatePaymentUrlAsync(PaymentRequest request, Guid userId, Guid? bookingId = null, Guid? packageId = null, Guid? carId = null, Guid? transactionId = null)
        {
            var transaction = _unitOfWork.TransactionRepo.GetByIdAsync(transactionId.GetValueOrDefault());
            await _unitOfWork.SaveChangeAsync(); // ✅ Lưu giao dịch vào DB

            return _vnpay.GetPaymentUrl(request);
        }

        //public async Task<Transaction?> ProcessPaymentResultAsync(IQueryCollection query)
        //{
        //    var paymentResult = _vnpay.GetPaymentResult(query);
        //    var transaction = await _unitOfWork.TransactionRepo.GetByIdAsync(paymentResult.PaymentId);

        //    if (transaction != null)
        //    {
        //        transaction.status = paymentResult.IsSuccess
        //            ? Transaction.TransactionStatus.SUCCESS
        //            : Transaction.TransactionStatus.FAILED;

        //        await _unitOfWork.SaveChangeAsync(); // ✅ Cập nhật trạng thái giao dịch trong DB
        //    }

        //    return transaction;
        //}
        public async Task<ResponseDTO> CallBackVnPay(Guid id)
        {
            var transaction = await _unitOfWork.TransactionRepo.GetByIdAsync(id);
            var vehicle = await _unitOfWork.VehicleRepo.GetByIdAsync(transaction.carId.GetValueOrDefault());
            if (transaction == null || vehicle == null)
            {
                return new ResponseDTO("loi get len", 400, false);

            }
            vehicle.packageId = transaction.packageId;
            vehicle.expirationDate = DateTime.UtcNow.AddYears(1);


            transaction.status = Transaction.TransactionStatus.SUCCESS;

            await _unitOfWork.SaveChangeAsync(); // ✅ Cập nhật trạng thái giao dịch trong DB
            return new ResponseDTO("thanh toan thanh cong", 200, true);

        }

        public async Task<ResponseDTO> CallBackOfBooking (Guid id)
        {
            var transaction = await _unitOfWork.TransactionRepo.GetByIdAsync(id);
            var booking = await _unitOfWork.BookingRepo.GetByIdAsync(transaction.bookingId.GetValueOrDefault());
            if (transaction == null || booking == null)
            {
                return new ResponseDTO("loi get len", 400, false);
            }
            booking.status = BookingStatus.IN_PROGRESS;
        
            transaction.status = Transaction.TransactionStatus.SUCCESS;
            await _unitOfWork.BookingRepo.UpdateAsync(booking);
            await _unitOfWork.SaveChangeAsync(); // ✅ Cập nhật trạng thái giao dịch trong DB
            return new ResponseDTO("thanh toan thanh cong", 200, true);
        }
    }
}
