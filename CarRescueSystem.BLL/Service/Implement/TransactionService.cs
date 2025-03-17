using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.BLL.Utilities;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.UnitOfWork;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserUtility _userUtility;

        public TransactionService(IUnitOfWork unitOfWork, UserUtility userUtility)
        {
            _unitOfWork = unitOfWork;
            _userUtility = userUtility;
        }

        public async Task CreateTransaction(Guid? bookingId = null, Guid? packageId = null)
        {
            var userId = _userUtility.GetUserIdFromToken(); // Lấy user ID từ token
            decimal amount = 0;
            string description = "";

            if (bookingId.HasValue)
            {
                var booking = await _unitOfWork.BookingRepo.GetByIdAsync(bookingId.Value);
                if (booking == null) return;

                amount = booking.totalPrice ?? 0;
                description = $"Payment for Booking {bookingId}";
            }
            else if (packageId.HasValue)
            {
                var package = await _unitOfWork.PackageRepo.GetByIdAsync(packageId.Value);
                if (package == null) return;

                amount = package.price;
                description = $"Payment for Package {packageId}";
            }

            var transaction = new Transaction
            {
                id = Guid.NewGuid(),
                userId = userId,
                amount = amount,
                createdAt = DateTime.UtcNow,

                bookingId = bookingId,
                packageId = packageId
            };

            await _unitOfWork.TransactionRepo.AddAsync(transaction);
            await _unitOfWork.SaveChangeAsync();
        }

    }
}
