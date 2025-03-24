using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IUserService
    {
        Task<ResponseDTO> GetAllStaffs();
        Task<ResponseDTO> GetAllCustomers();
        //Task<ResponseDTO> CreateStaff(CreateStaffDTO);
        //Task<ResponseDTO> UpdateStaff(UpdateStaffDTO);
        //Task<ResponseDTO> DeleteStaff(Guid id);
        Task<ResponseDTO> UpdateUser(ProfileDTO profileDTO);
        Task<ResponseDTO> UpdateStaff(UpdateUserDTO userDTO);
        Task<ResponseDTO> DeleteUser(Guid id);

    }
}
