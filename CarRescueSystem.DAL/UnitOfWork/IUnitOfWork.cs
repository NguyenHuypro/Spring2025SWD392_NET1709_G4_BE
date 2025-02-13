using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Repository.Interface;

namespace CarRescueSystem.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepo { get; }

        // add more
        ITokenRepository TokenRepo { get; }
        IRoleRepository RoleRepo { get; }

        Task<int> SaveAsync();
        Task<bool> SaveChangeAsync();
    }
}
