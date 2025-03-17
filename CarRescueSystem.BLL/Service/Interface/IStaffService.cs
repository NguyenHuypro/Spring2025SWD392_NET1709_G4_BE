using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IStaffService
    {
        Task<ResponseDTO> GetAvailableStaffAsync(Guid bookingId);
    }
}
