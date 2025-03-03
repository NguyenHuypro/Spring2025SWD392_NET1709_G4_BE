using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface ITransactionService
    {
        Task<ResponseDTO> CreateTransaction(TransactionDTO transactionDTO);
    }
}
