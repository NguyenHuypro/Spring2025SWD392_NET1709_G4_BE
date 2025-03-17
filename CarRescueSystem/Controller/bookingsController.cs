using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace CarRescueSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Mặc định yêu cầu đăng nhập cho toàn bộ Controller
    public class bookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public bookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Tạo Booking mới (Chỉ Customer)
        /// </summary>
        [HttpPost()]
        //[Authorize(Roles = "Customer")] // Chỉ Customer được tạo Booking
        public async Task<IActionResult> CreateBooking([FromBody] CreatingBookingDTO bookingDto)
        {
            var response = await _bookingService.CreateBookingAsync(bookingDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("receptionist")] 
        public async Task<IActionResult> CreateBookingForReceptionist([FromBody] ReBookingDTO reBookingDTO)
        {
            var response = await _bookingService.CreateBookingforReceptionist(reBookingDTO);
            return StatusCode(response.StatusCode, response);
        }


        ///// <summary>
        ///// Receptionist xác nhận Booking
        ///// </summary>
        //[HttpPut("confirm/{bookingId}")]
        ////[Authorize(Roles = "Receptionist")] // Chỉ Receptionist được xác nhận Booking
        //public async Task<IActionResult> ConfirmBooking(Guid bookingId)
        //{
        //    var response = await _bookingService.ConfirmBookingAsync(bookingId);
        //    return StatusCode(response.StatusCode, response);
        //}



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



        ///// <summary>
        ///// Staff cập nhật trạng thái khi đã đến nơi
        ///// </summary>
        //[HttpPut("staff-arrived/{bookingId}")]
        //// [Authorize(Roles = "Staff")] // Chỉ Staff có quyền cập nhật
        //public async Task<IActionResult> MarkStaffArrived(Guid bookingId)
        //{
        //    var response = await _bookingService.MarkStaffArrivedAsync(bookingId);
        //    return StatusCode(response.StatusCode, response);
        //}



        /// <summary>
        /// Staff thêm dịch vụ vào Booking
        /// </summary>
        [HttpPost("add-service/{bookingId}")]
        // [Authorize(Roles = "Staff")] // Chỉ Staff được thêm dịch vụ
        public async Task<IActionResult> AddServiceToBooking(Guid bookingId, [FromBody] ListServiceDTO request)
        {
            if (request?.selectedServices == null || !request.selectedServices.Any())
            {
                return BadRequest(new { message = "Danh sách dịch vụ không hợp lệ." });
            }

            var response = await _bookingService.AddServiceToBookingAsync(bookingId, request.selectedServices);
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

        [HttpGet()]
        public async Task<IActionResult> GetAllBooking()
        {
            var response = await _bookingService.GetAllBookingAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("guest")]
        public async Task<IActionResult> GetAllBookingGuest()
        {
            var response = await _bookingService.GetBookingGuest();
            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("user")]
        public async Task<IActionResult> GetBookingByCustomerId()
        {
            var response = await _bookingService.GetBookingByCustomerIdAsync();
            return StatusCode(response.StatusCode, response);
        }

        // /api/bookings/:bookingId

        // /api/bookings/staff/:staffId

        // /api/bookings/:bookingId/status method: PUT

        // /api/bookings/:bookingId/assign method: PUT


        /// <summary>
        /// Lấy thông tin Booking theo ID
        /// </summary>
        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBookingById(Guid bookingId)
        {
            var response = await _bookingService.GetBookingByIdAsync(bookingId);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Lấy danh sách Booking của một Staff
        /// </summary>
        [HttpGet("staff/{staffId}")]
        public async Task<IActionResult> GetBookingsByStaff(Guid staffId)
        {
            var response = await _bookingService.GetBookingsByStaffAsync(staffId);
            return StatusCode(response.StatusCode, response);
        }



        /// <summary>
        /// Gán nhân viên vào Booking
        /// </summary>
        [HttpPut("{bookingId}/assign")]
        public async Task<IActionResult> AssignStaffToBooking(Guid bookingId, [FromBody] StaffAssignDTO staffAssignment)
        {
            Console.WriteLine("API đã gọi tới");

            // Kiểm tra nếu staffAssignment null
            if (staffAssignment == null)
            {
                Console.WriteLine("staffAssignment null");
                return BadRequest(new { message = "Staff assignment data is required." });
            }

            // Kiểm tra nếu các staffId hợp lệ
            if (staffAssignment.Staff1 == Guid.Empty || staffAssignment.Staff2 == Guid.Empty)
            {
                Console.WriteLine("staffId sai");
                return BadRequest(new { message = "Both staff IDs must be valid." });
            }

            // Kiểm tra nếu bookingId hợp lệ
            if (bookingId == Guid.Empty)
            {
                Console.WriteLine("bookingId sai");
                return BadRequest(new { message = "Invalid booking ID." });
            }

            // Gọi service để phân công nhân viên cho booking
            var response = await _bookingService.AssignStaffToBookingAsync(bookingId, new List<Guid> { staffAssignment.Staff1, staffAssignment.Staff2 });

            // Trả về kết quả
            return StatusCode(response.StatusCode, response);
        }



        [HttpPut("{bookingId}/status")]
        public async Task<IActionResult> UpdateBookingStatus(Guid bookingId, [FromBody] AllInOneBookingDTO? requestBody)
        {
            Console.WriteLine("nó vào đây");

            if (requestBody == null || string.IsNullOrEmpty(requestBody.status))
            {
                return BadRequest(new { message = "Booking status is required in request body." });
            }

            string status = requestBody.status.ToUpper();
            ResponseDTO response;

            switch (status)
            {
                case "CANCELLED":
                    response = await _bookingService.CompleteOrCancelBookingAsync(bookingId, false);
                    break;

                case "IN_PROGRESS":
                    response = await _bookingService.UpdateBookingToInProgressAsync(bookingId);
                    break;

                case "FINISHED":
                    response = await _bookingService.FinishedBooking(bookingId);
                    break;

                case "PENDING_PAYMENT":
                    
                    response = await _bookingService.AcceptBooking(bookingId); 
                    break;
                //case "PENDING":
                //    response = await _bookingService.CreateBookingforReceptionist(requestBody);
                //    break;

                default:
                    return BadRequest(new { message = "Invalid booking status." });
            }

            return StatusCode(response.StatusCode, response);
        }


        [HttpPost("{bookingId}/confirm-staff/{staffId}")]
        public async Task<IActionResult> ConfirmStaffArrival(Guid bookingId, Guid staffId)
        {
            var response = await _bookingService.ConfirmStaffArrivalAsync(bookingId, staffId);
            return StatusCode(response.StatusCode, response);
        }



    }
}
