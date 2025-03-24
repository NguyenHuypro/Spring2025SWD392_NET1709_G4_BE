using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;


namespace CarRescueSystem.DAL.Repository.Interface
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<List<Transaction>> GetAllTransactions();
        Task<List<Transaction>> GetAllTransactionsByUserId(Guid id);
        Task<List<Transaction>> GetUnpaidOrdersAsync(TimeSpan timeLimit);
    }
}
