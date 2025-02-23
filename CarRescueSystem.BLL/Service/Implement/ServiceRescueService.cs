using System;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.DTO;
using CarRescueSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class ServiceRescueService : IServiceRescueService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceRescueService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetAllService()
        {
            try
            {
                var listService = await _unitOfWork.ServiceRepo.GetAll().ToListAsync(); 

                if (listService == null || !listService.Any())
                {
                    return new ResponseDTO("No services found.", 404, false);
                }

                return new ResponseDTO("Successfully retrieved service list.", 200, true, listService);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Internal server error: {ex.Message}", 500, false);
            }
        }


    }
}
