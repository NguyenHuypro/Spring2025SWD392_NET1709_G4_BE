using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;
using VNPAY.NET.Models;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IVnPayService
    {
        Task<string> CreatePaymentUrlAsync(PaymentRequest request, Guid userId, Guid? bookingId = null, Guid? packageId = null, Guid? carId = null, Guid? transactionId = null);
        Task<ResponseDTO> CallBackVnPay(Guid id);
        Task<ResponseDTO> CallBackOfBooking(Guid id);
    }
}
