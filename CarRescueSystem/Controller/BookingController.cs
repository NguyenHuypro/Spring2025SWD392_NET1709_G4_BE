using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRescueSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Mặc định yêu cầu đăng nhập cho toàn bộ Controller
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Tạo Booking mới (Chỉ Customer)
        /// </summary>
        [HttpPost("create")]
        //[Authorize(Roles = "Customer")] // Chỉ Customer được tạo Booking
        public async Task<IActionResult> CreateBooking([FromBody] CreatingBookingDTO bookingDto)
        {
            var response = await _bookingService.CreateBookingAsync(bookingDto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Receptionist xác nhận Booking
        /// </summary>
        [HttpPut("confirm/{bookingId}")]
        //[Authorize(Roles = "Receptionist")] // Chỉ Receptionist được xác nhận Booking
        public async Task<IActionResult> ConfirmBooking(Guid bookingId)
        {
            var response = await _bookingService.ConfirmBookingAsync(bookingId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Receptionist gán nhân viên vào Booking
        /// </summary>
        [HttpPut("assign-staff/{bookingId}")]
        //[Authorize(Roles = "Receptionist")] // Chỉ Receptionist có quyền
        public async Task<IActionResult> AssignStaff(Guid bookingId, [FromBody] List<Guid> staffIds)
        {
            var response = await _bookingService.AssignStaffToBookingAsync(bookingId, staffIds);
            return StatusCode(response.StatusCode, response);
        }


        /// <summary>
        /// Staff thêm dịch vụ vào Booking
        /// </summary>
        [HttpPost("add-service/{bookingId}")]
        //[Authorize(Roles = "Staff")] // Chỉ Staff được thêm dịch vụ
        public async Task<IActionResult> AddServiceToBooking(Guid bookingId, [FromBody] List<Guid> serviceIds)
        {
            var response = await _bookingService.AddServiceToBookingAsync(bookingId, serviceIds);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Staff hoàn thành hoặc hủy Booking
        /// </summary>
        [HttpPut("complete-or-cancel/{bookingId}")]
        //[Authorize(Roles = "Staff")] // Chỉ Staff có quyền
        public async Task<IActionResult> CompleteOrCancelBooking(Guid bookingId, [FromQuery] bool isCompleted)
        {
            var response = await _bookingService.CompleteOrCancelBookingAsync(bookingId, isCompleted);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllBooking()
        {
            var response = await _bookingService.GetAllBookingAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("getByCustomerId")]
        public async Task<IActionResult> GetBookingByCustomerId()
        {
            var response = await _bookingService.GetBookingByCustomerIdAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
