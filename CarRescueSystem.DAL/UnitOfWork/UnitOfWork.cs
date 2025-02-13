using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Data;
using CarRescueSystem.DAL.Repository.Implement;
using CarRescueSystem.DAL.Repository.Interface;

namespace CarRescueSystem.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork (ApplicationDbContext context)
        {
            _context = context;
            UserRepo = new UserRepository(_context);
            TokenRepo = new TokenRepository(_context);
            RoleRepo = new RoleRepository(_context);
        }
        public IUserRepository UserRepo { get; private set; }
        public ITokenRepository TokenRepo { get; private set; }
        public IRoleRepository RoleRepo { get; private set; }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<bool> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
