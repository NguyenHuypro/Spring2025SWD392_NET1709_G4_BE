using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IBookingService
    {
        Task<ResponseDTO> CreateBookingAsync(CreatingBookingDTO creatingBookingDTO);
        //Task<ResponseDTO> ConfirmBookingAsync(Guid bookingId); // Recep nhận đơn
        Task<ResponseDTO> AssignStaffToBookingAsync(Guid bookingId, List<Guid> staffIds);

        Task<ResponseDTO> AddServiceToBookingAsync(Guid bookingId, List<Guid> serviceIds);
        Task<ResponseDTO> CompleteOrCancelBookingAsync(Guid bookingId, bool isCompleted);

        Task<ResponseDTO> FinishedBooking(Guid id);
        Task<ResponseDTO> GetAllBookingAsync();
        Task<ResponseDTO> GetBookingByCustomerIdAsync(Guid? customerId = null);
    

        Task<ResponseDTO> GetBookingByIdAsync(Guid bookingId);
        Task<ResponseDTO> GetBookingsByStaffAsync(Guid staffId);
        Task<ResponseDTO> UpdateBookingToInProgressAsync(Guid bookingId);
        Task<ResponseDTO> ConfirmStaffArrivalAsync(Guid bookingId, Guid staffId);

        Task<ResponseDTO> AcceptBooking(Guid id);


    }

}
