using CarRescueSystem.BLL.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CarRescueSystem.Controller
{
    [Route("api/my-transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Lấy danh sách hóa đơn
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetAllTransaction()
        {
            var response = await _transactionService.GetAllTransactionByUserId();
            return StatusCode(response.StatusCode, response);
        }

       
    }
}
