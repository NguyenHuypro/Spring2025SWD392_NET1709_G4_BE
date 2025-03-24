using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IPackageService
    {
        Task<ResponseDTO> GetAllAsync();
        Task<ResponseDTO> GetByIdAsync(Guid id);
        Task<ResponseDTO> AddAsync(AddPackageDTO dto);
        Task<ResponseDTO> UpdateAsync(Guid id, PackageDTO packageDTO);
        Task<ResponseDTO> DeleteAsync(Guid id);
    }
}
