using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet cho các bảng
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<ServiceOfBooking> ServiceOfBookings { get; set; }
        public DbSet<BookingStaff> BookingStaffs { get; set; }
        public DbSet<ServicePackage> ServicePackages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 Thiết lập khóa chính
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Role>().HasKey(r => r.RoleID);
            modelBuilder.Entity<RefreshToken>().HasKey(rt => rt.RefreshTokenId);
            modelBuilder.Entity<Booking>().HasKey(b => b.BookingId);
            modelBuilder.Entity<Vehicle>().HasKey(v => v.VehicleId);
            modelBuilder.Entity<Service>().HasKey(s => s.ServiceId);
            modelBuilder.Entity<Package>().HasKey(p => p.PackageId);
            modelBuilder.Entity<ServiceOfBooking>().HasKey(sb => sb.ServiceOfBookingId);
            modelBuilder.Entity<BookingStaff>().HasKey(bs => bs.BookingStaffId);
            modelBuilder.Entity<ServicePackage>().HasKey(sp => sp.ServicePackageId);

            // 🔹 User - Role (1-N)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 User - RefreshToken (1-N)
            modelBuilder.Entity<RefreshToken>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Booking - User (Customer) (1-N)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany()
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Booking - Vehicle (1-N)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Vehicle)
                .WithMany()
                .HasForeignKey(b => b.VehicleId)
                .OnDelete(DeleteBehavior.SetNull);

            // 🔹 Booking - Package (1-N)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Package)
                .WithMany()
                .HasForeignKey(b => b.PackageId)
                .OnDelete(DeleteBehavior.SetNull);

            // 🔹 Booking - Staff (N-N)
            modelBuilder.Entity<BookingStaff>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingStaffs)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingStaff>()
                .HasOne(bs => bs.Staff)
                .WithMany(u => u.BookingsStaffs)
                .HasForeignKey(bs => bs.StaffId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Booking - Service (N-N)
            modelBuilder.Entity<ServiceOfBooking>()
                .HasOne(sb => sb.Booking)
                .WithMany(b => b.ServiceBookings)
                .HasForeignKey(sb => sb.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceOfBooking>()
                .HasOne(sb => sb.Service)
                .WithMany(s => s.ServiceOfBookings)
                .HasForeignKey(sb => sb.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Service - Package (N-N)
            modelBuilder.Entity<ServicePackage>()
                .HasOne(sp => sp.Service)
                .WithMany(s => s.ServicePackages)
                .HasForeignKey(sp => sp.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServicePackage>()
                .HasOne(sp => sp.Package)
                .WithMany(p => p.ServicePackages)
                .HasForeignKey(sp => sp.PackageID)
                .OnDelete(DeleteBehavior.Cascade);
            //User 1-1 Package
            modelBuilder.Entity<User>()
    .HasOne(u => u.Package)
    .WithOne(p => p.User)
    .HasForeignKey<Package>(p => p.UserId);


            // 🔹 Định dạng kiểu tiền tệ
            modelBuilder.Entity<Service>()
                .Property(s => s.ServicePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Package>()
                .Property(p => p.PackagePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);

            DbSeeder.Seed(modelBuilder);
        }
    }
}
