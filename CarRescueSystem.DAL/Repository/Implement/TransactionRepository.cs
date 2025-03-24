using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Data;
using CarRescueSystem.DAL.Model;
using CarRescueSystem.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CarRescueSystem.DAL.Repository.Implement
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return _context.Transactions
                .Include(t => t.Package)
                .Include(t => t.Vehicle)
                .Include(t => t.Booking)
                .OrderByDescending(t => t.createdAt)
                .ToList();
        }

        public async Task<List<Transaction>> GetAllTransactionsByUserId(Guid id)
        {
            return _context.Transactions
                .Where(t => t.userId == id)
                .Include(t => t.Package)
                .Include(t => t.Vehicle)
                .Include(t => t.Booking)
                .OrderByDescending(t => t.createdAt)
                .ToList();
        }

        public async Task<List<Transaction>> GetUnpaidOrdersAsync(TimeSpan timeLimit)
        {
            var expiredTime = DateTime.UtcNow - timeLimit;
            return await _context.Transactions
                .Where(o => o.status.ToString() == "PENDING" && o.createdAt <= expiredTime)
                .ToListAsync();
        }

    }
}
