using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IWalletService
    {
        Task<ResponseDTO> GetWalletByUserId(Guid userId);
        Task<ResponseDTO> DeductAmount(Guid userId, decimal amount);
        Task<ResponseDTO> AddFunds(Guid userId, decimal amount);
    }
}
