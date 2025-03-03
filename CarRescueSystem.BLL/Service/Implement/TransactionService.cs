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
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> CreateTransaction(TransactionDTO transactionDTO)
        {
            var transaction = new Transaction
            {
                TransactionId = transactionDTO.TransactionId,
                UserId = transactionDTO.UserId,
                Amount = transactionDTO.Amount,
                CreatedAt = transactionDTO.CreatedAt,
                Description = transactionDTO.Description
            };

            await _unitOfWork.TransactionRepo.AddAsync(transaction);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDTO("Transaction recorded successfully.", 201, true, transaction);
        }
    }
}
