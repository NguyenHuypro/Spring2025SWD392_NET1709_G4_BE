using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CarRescueSystem.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IStaffService _staffService;

        public UsersController(IUserService userService, IStaffService staffService)
        {
            _userService = userService;
            _staffService = staffService;
        }

        

        /// <summary>
        /// Lấy danh sách nhân viên có sẵn
        /// </summary>
        [HttpGet("staffs")]
        public async Task<IActionResult> GetAllStaffs()
        {
            var response = await _userService.GetAllStaffs();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Lấy danh sách khách hàng có sẵn
        /// </summary>
        [HttpGet("customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var response = await _userService.GetAllCustomers(); // Get the customer data from the service layer
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Lấy danh sách staff có sẵn
        /// </summary>
        [HttpGet("available-staffs")]
        public async Task<IActionResult> GetStaffAvailable(Guid bookingId)
        {
            Console.WriteLine("gọi dc rồi");
            var response = await _staffService.GetAvailableStaffAsync(bookingId); // Get the customer data from the service layer
            return StatusCode(response.StatusCode, response);
        }

    }
}
