using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace CarRescueSystem.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Booking> Bookings { get; set; } // ✅ Thêm Booking
        public DbSet<Vehicle> Vehicles { get; set; } // ✅ Thêm Vehicle
        public DbSet<Service> Services { get; set; } // ✅ Thêm Service
        public DbSet<Package> Packages { get; set; } // ✅ Thêm Package
        public DbSet<ServiceOfBooking> ServiceOfBookings { get; set; } // ✅ Thêm ServiceOfBooking
        public DbSet<BookingStaff> BookingStaffs { get; set; } // ✅ Thêm BookingStaff
        public DbSet<ServicePackage> ServicePackages { get; set; } // ✅ Thêm Service-Package

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<RefreshToken>().HasKey(rt => rt.RefreshTokenId);

            // Token
            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.UserId);

            modelBuilder.Entity<RefreshToken>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleID);

            // Booking - Customer (User)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany()
                .HasForeignKey(b => b.CustomerId);

            // Booking - Vehicle
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Vehicle)
                .WithMany()
                .HasForeignKey(b => b.VehicleId)
                .OnDelete(DeleteBehavior.SetNull); // Nếu xóa xe thì Booking không bị mất

            // Booking - Service (N-N)
            modelBuilder.Entity<ServiceOfBooking>()
                .HasKey(sb => new { sb.BookingId, sb.ServiceId });

            modelBuilder.Entity<ServiceOfBooking>()
                .HasOne(sb => sb.Booking)
                .WithMany(b => b.Services)
                .HasForeignKey(sb => sb.BookingId);

            modelBuilder.Entity<ServiceOfBooking>()
                .HasOne(sb => sb.Service)
                .WithMany()
                .HasForeignKey(sb => sb.ServiceId);

            // Booking - Staff (1-N)
            modelBuilder.Entity<BookingStaff>()
                .HasKey(bs => new { bs.BookingId, bs.StaffId });

            modelBuilder.Entity<BookingStaff>()
                .HasOne(bs => bs.Booking)
                .WithMany()
                .HasForeignKey(bs => bs.BookingId);

            modelBuilder.Entity<BookingStaff>()
                .HasOne(bs => bs.Staff)
                .WithMany()
                .HasForeignKey(bs => bs.StaffId);

            // Service - Package (N-N)
            modelBuilder.Entity<ServicePackage>()
                .HasKey(sp => new { sp.ServiceId, sp.PackageID });

            modelBuilder.Entity<ServicePackage>()
                .HasOne(sp => sp.Service)
                .WithMany()
                .HasForeignKey(sp => sp.ServiceId);

            modelBuilder.Entity<ServicePackage>()
                .HasOne(sp => sp.Package)
                .WithMany()
                .HasForeignKey(sp => sp.PackageID);

            base.OnModelCreating(modelBuilder);

            DbSeeder.Seed(modelBuilder);
        }
    }
}
