using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.BLL.Utilities;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.UnitOfWork;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;
        private readonly UserUtility _userUtility;

        public UserService(IUnitOfWork unitOfWork, UserUtility userUtility)
        {
            _unitOfWork = unitOfWork;
            _userUtility = userUtility;
        }
        //public Task<ResponseDTO> CreateStaff(CreateStaffDTO)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<ResponseDTO> GetAllCustomers()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ResponseDTO> GetAllStaffs()
        {
            // Get the list of staff members using the repository method
            var staffs = await _unitOfWork.UserRepo.GetAllStaffsAsync();

            // Check if any staff are found
            if (staffs == null || !staffs.Any())
            {
                return new ResponseDTO("No staff found.", 404, false);
            }

            // Map the staff entities to a list of staff DTOs (if necessary, create a StaffDTO)
            var staffDTOs = staffs.Select(staff => new ListStaffDTO
            {
                Id = staff.id,            // Map staff ID
                FullName = staff.fullName, // Map full name
                Phone = staff.phone,      // Map phone number
                Email = staff.email,
                Role = staff.role.ToString() // Map role as string
            }).ToList();

            // Return a successful response with the staff data
            return new ResponseDTO("Staff members retrieved successfully.", 200, true, staffDTOs);
        }

        public async Task<ResponseDTO> GetAllCustomers()
        {
            try
            {
                var customers = await _unitOfWork.UserRepo.GetAllCustomersAsync(); // Assuming this method exists in your repo
                if (customers == null || !customers.Any())
                {
                    return new ResponseDTO("No customers found.", 404, false);
                }

                var customerDTOs = customers.Select(c => new CustomerDTO
                {
                    id = c.id,
                    fullName = c.fullName,
                    email = c.email,
                    phone = c.phone,
                    status = c.staffStatus.ToString() // assuming status is an enum
                }).ToList();

                return new ResponseDTO("Customers retrieved successfully.", 200, true, customerDTOs);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }
        public async Task<ResponseDTO> UpdateUser(ProfileDTO profileDTO)
        {
            var userID = _userUtility.GetUserIdFromToken();
            var user = await _unitOfWork.UserRepo.GetByIdAsync(userID);
            if (user == null)
            {
                return new ResponseDTO("không thấy người dùng", 200, false);
            }
            user.fullName = profileDTO.fullName;
            user.phone = profileDTO.phone;

            await _unitOfWork.UserRepo.UpdateAsync(user);
            await _unitOfWork.SaveChangeAsync();

            var userDTO = new UserDTO
            {
                id = user.id,
                fullName = user.fullName,
                email = user.email,
                phone = user.phone,
                role = user.role.ToString()
            };

            return new ResponseDTO("thay đổi thành công", 200, true, userDTO);
        }

        public async Task<ResponseDTO> UpdateStaff(UpdateUserDTO dto) 
            {
            
            var user = await _unitOfWork.UserRepo.GetByIdAsync(dto.id);
            if (user == null)
            {
                return new ResponseDTO("không tìm thấy người dùng", 200, false);
            }
            user.fullName = dto.fullName;
            user.phone = dto.phone;

            await _unitOfWork.UserRepo.UpdateAsync(user);
            await _unitOfWork.SaveChangeAsync();

            var userDTO = new UserDTO
            {
                id = user.id,
                fullName = user.fullName,
                email = user.email,

                phone = user.phone,
                role = user.role.ToString()

            };
            return new ResponseDTO("Cập nhật người dùng thành công", 200, true, userDTO);
        } 

        public async Task<ResponseDTO> DeleteUser(Guid id)
        {
            var user = await _unitOfWork.UserRepo.GetByIdAsync(id);
            if (user == null)
            {
                return new ResponseDTO("không tìm thấy người dùng", 200, false);
            }
            await _unitOfWork.UserRepo.DeleteAsync(id);
            await _unitOfWork.SaveChangeAsync();
            return new ResponseDTO("Xóa người dùng thành công", 200, true);

        }
    }
}
