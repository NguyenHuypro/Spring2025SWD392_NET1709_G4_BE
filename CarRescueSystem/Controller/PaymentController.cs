using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace CarRescueSystem.Controller
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingService _bookingService;


        public PaymentController(IVnPayService vnPayService, IUnitOfWork unitOfWork, IBookingService bookingService)
        {
            _vnPayService = vnPayService;
            _unitOfWork = unitOfWork;
            _bookingService = bookingService;
        }

        

        [HttpPost("callback")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentCallbackDTO model)
        {
            
            var response = await _vnPayService.CallBackVnPay(model.transactionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("callback-booking")]
        public async Task<IActionResult> ConfirmBookingPayment([FromBody] PaymentCallbackDTO model)
        {

            var response = await _vnPayService.CallBackOfBooking(model.transactionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("booking")]
        public async Task<IActionResult> CreateBookingPayment([FromBody] PaymentBookingDTO request)
        {
            var response = await _bookingService.CompleteOrCancelBookingAsync(request.BookingId, true);
            return StatusCode(response.StatusCode, response);
        }



    }
}
