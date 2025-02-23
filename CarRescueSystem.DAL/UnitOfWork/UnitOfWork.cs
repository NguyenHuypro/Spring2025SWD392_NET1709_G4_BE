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
            VehicleRepo = new VehicleRepository(_context);
            BookingStaffRepo = new BookingStaffRepository(_context);
            BookingRepo = new BookingRepository(_context);
            ServiceRepo = new ServiceRepository(_context);
            ServiceOfBookingRepo = new ServiceOfBookingRepository(_context);
            PackageRepo = new PackageRepository(_context);
            UserPackageRepo = new UserPackageRepository(_context);
        }
        public IUserRepository UserRepo { get; private set; }
        public ITokenRepository TokenRepo { get; private set; }
        public IRoleRepository RoleRepo { get; private set; }
        public IVehicleRepository VehicleRepo { get; private set; }
        public IBookingRepository BookingRepo { get; private set; }
        public IBookingStaffRepository BookingStaffRepo { get; private set; }
        public IServiceRepository ServiceRepo { get; private set; }
        public IServiceOfBookingRepository ServiceOfBookingRepo { get; private set; }
        public IPackageRepository PackageRepo { get; private set; }
        public IUserPackageRepository UserPackageRepo { get; private set; }
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
