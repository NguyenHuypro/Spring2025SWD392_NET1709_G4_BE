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
using Microsoft.EntityFrameworkCore;

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

        public async Task<ResponseDTO> GetAllTransactionByUserId()
        {

            var id = _userUtility.GetUserIdFromToken(); 

            if (id == Guid.Empty)
            {
                return new ResponseDTO("Không tìm thấy user !!!", 200, false);
            }

            var transactions = await _unitOfWork.TransactionRepo.GetAllTransactionsByUserId(id);

            if (transactions == null || !transactions.Any())
            {
                return new ResponseDTO("Không tìm thấy hóa đơn !!!", 200, false);
            }

            var transactionDTOs = new List<TransactionGetDTO>();

            foreach (var transaction in transactions)
            {
                var transactionDTO = new TransactionGetDTO
                {
                    id = transaction.id,
                    amount = transaction.amount,
                    packageName = transaction.Package?.name ?? "", // Nếu null, gán giá trị mặc định
                    licensePlate = transaction.Booking?.licensePlate ?? transaction.Vehicle?.licensePlate ?? "", // Nếu null, gán giá trị mặc định
                    status = transaction.status.ToString(),
                    createdAt = transaction.createdAt
                };
                transactionDTOs.Add(transactionDTO);
            }
            return new ResponseDTO("Lấy hóa đơn thành công", 200, true, transactionDTOs);
        }
    }
}
