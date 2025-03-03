using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.UnitOfWork;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionService _transactionService;

        public WalletService(IUnitOfWork unitOfWork, ITransactionService transactionService)
        {
            _unitOfWork = unitOfWork;
            _transactionService = transactionService;
        }

        public async Task<ResponseDTO> GetWalletByUserId(Guid userId)
        {
            var wallet = await _unitOfWork.WalletRepo.GetByIdAsync(userId);
            if (wallet == null)
            {
                return new ResponseDTO("Wallet not found.", 404, false);
            }
            return new ResponseDTO("Wallet retrieved successfully.", 200, true, wallet);
        }

        public async Task<ResponseDTO> DeductAmount(Guid userId, decimal amount)
        {
            var wallet = await _unitOfWork.WalletRepo.GetByIdAsync(userId);
            if (wallet == null)
            {
                return new ResponseDTO("Wallet not found.", 404, false);
            }

            if (wallet.Balance < amount)
            {
                return new ResponseDTO("Insufficient balance.", 400, false);
            }

            wallet.Balance -= amount;
            await _unitOfWork.WalletRepo.UpdateAsync(wallet);
            await _unitOfWork.SaveChangeAsync();

            // Lưu giao dịch vào TransactionService
            await _transactionService.CreateTransaction(new TransactionDTO
            {
                TransactionId = Guid.NewGuid(),
                UserId = userId,
                Amount = -amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Payment deducted from wallet"
            });

            return new ResponseDTO("Payment successful.", 200, true, wallet);
        }

        public async Task<ResponseDTO> AddFunds(Guid userId, decimal amount)
        {
            var wallet = await _unitOfWork.WalletRepo.GetByIdAsync(userId);
            if (wallet == null)
            {
                wallet = new Wallet
                {
                    UserId = userId,
                    Balance = amount
                };
                await _unitOfWork.WalletRepo.AddAsync(wallet);
            }
            else
            {
                wallet.Balance += amount;
                await _unitOfWork.WalletRepo.UpdateAsync(wallet);
            }
            await _unitOfWork.SaveChangeAsync();

            // Ghi nhận giao dịch nạp tiền
            await _transactionService.CreateTransaction(new TransactionDTO
            {
                TransactionId = Guid.NewGuid(),
                UserId = userId,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
                Description = "Funds added to wallet"
            });

            return new ResponseDTO("Funds added successfully.", 200, true, wallet);
        }
    }
    
    
}
