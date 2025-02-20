using CarRescueSystem.BLL.Service;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CarRescueSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Tạo Booking mới
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] CreatingBookingDTO bookingDto)
        {
            var response = await _bookingService.CreateBookingAsync(bookingDto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Reception xác nhận Booking
        /// </summary>
        [HttpPut("confirm/{bookingId}")]
        public async Task<IActionResult> ConfirmBooking(Guid bookingId)
        {
            var response = await _bookingService.ConfirmBookingAsync(bookingId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Assign Staff vào Booking
        /// </summary>
        [HttpPut("assign-staff/{bookingId}")]
        public async Task<IActionResult> AssignStaff(Guid bookingId)
        {
            var response = await _bookingService.AssignStaffToBookingAsync(bookingId);
            return StatusCode(response.StatusCode, response);
        }

        

        /// <summary>
        /// Staff thêm dịch vụ vào Booking
        /// </summary>
        [HttpPost("add-service/{bookingId}")]
        public async Task<IActionResult> AddServiceToBooking(Guid bookingId, [FromBody] List<Guid> serviceIds)
        {
            var response = await _bookingService.AddServiceToBookingAsync(bookingId, serviceIds);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Hoàn thành hoặc hủy Booking
        /// </summary>
        [HttpPut("complete-or-cancel/{bookingId}")]
        public async Task<IActionResult> CompleteOrCancelBooking(Guid bookingId, [FromQuery] bool isCompleted)
        {
            var response = await _bookingService.CompleteOrCancelBookingAsync(bookingId, isCompleted);
            return StatusCode(response.StatusCode, response);
        }
    }
}
