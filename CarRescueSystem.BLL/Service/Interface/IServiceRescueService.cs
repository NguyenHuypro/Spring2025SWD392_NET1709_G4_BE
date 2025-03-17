using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.Common.DTO;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IServiceRescueService
    {
        Task<ResponseDTO> GetAllService();
        Task<ResponseDTO> GetServiceById(Guid serviceId);
        Task<ResponseDTO> CreateService(CreateServiceDTO createServiceDTO);
        Task<ResponseDTO> UpdateService(Guid serviceId, ServiceDTO serviceDTO);
        Task<ResponseDTO> DeleteService(Guid serviceId);
        Task<ResponseDTO> GetServicesByPackageId(Guid packageId);
    }
}
