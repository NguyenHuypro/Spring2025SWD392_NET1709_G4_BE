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
        //public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<ServiceOfBooking> ServiceOfBookings { get; set; }
        public DbSet<BookingStaff> BookingStaffs { get; set; }
        public DbSet<ServicePackage> ServicePackages { get; set; }

        public DbSet<Schedule> Schedules {  get; set; }
        public DbSet<RescueStation> RescueStations { get; set; }
        //public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 Thiết lập khóa chính
            modelBuilder.Entity<User>().HasKey(u => u.id);
            //modelBuilder.Entity<Role>().HasKey(r => r.RoleID);
            modelBuilder.Entity<RefreshToken>().HasKey(rt => rt.id);
            modelBuilder.Entity<Booking>().HasKey(b => b.id);
            modelBuilder.Entity<Vehicle>().HasKey(v => v.id);
            modelBuilder.Entity<Service>().HasKey(s => s.id);
            modelBuilder.Entity<Package>().HasKey(p => p.id);
            modelBuilder.Entity<ServiceOfBooking>().HasKey(sb => sb.id);
            modelBuilder.Entity<BookingStaff>().HasKey(bs => bs.id);
            modelBuilder.Entity<ServicePackage>().HasKey(sp => sp.id);
            modelBuilder.Entity<Transaction>().HasKey(b => b.id);
            //modelBuilder.Entity<Wallet>().HasKey(u => u.userId);

            // 🔹 User - Role (1-N)
            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.Role)
            //    .WithMany(r => r.Users)
            //    .HasForeignKey(u => u.RoleID)
            //    .OnDelete(DeleteBehavior.Restrict);

            // 🔹 User - RefreshToken (1-N)
            modelBuilder.Entity<RefreshToken>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(rt => rt.userId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Booking - User (Customer) (1-N)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany()
                .HasForeignKey(b => b.customerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Booking - Vehicle (1-N)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Vehicle)
                .WithMany()
                .HasForeignKey(b => b.vehicleId)
                .OnDelete(DeleteBehavior.SetNull);

            // 🔹 Booking - Package (1-N)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Package)
                .WithMany()
                .HasForeignKey(b => b.packageId)
                .OnDelete(DeleteBehavior.SetNull);

            // 🔹 Booking - Staff (N-N)
            modelBuilder.Entity<BookingStaff>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingStaffs)
                .HasForeignKey(bs => bs.bookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingStaff>()
                .HasOne(bs => bs.Staff)
                .WithMany(u => u.BookingsStaffs)
                .HasForeignKey(bs => bs.staffId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Booking - Service (N-N)
            modelBuilder.Entity<ServiceOfBooking>()
                .HasOne(sb => sb.Booking)
                .WithMany(b => b.ServiceBookings)
                .HasForeignKey(sb => sb.bookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceOfBooking>()
                .HasOne(sb => sb.Service)
                .WithMany(s => s.ServiceOfBookings)
                .HasForeignKey(sb => sb.serviceId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Service - Package (N-N)
            modelBuilder.Entity<ServicePackage>()
                .HasOne(sp => sp.Service)
                .WithMany(s => s.ServicePackages)
                .HasForeignKey(sp => sp.serviceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServicePackage>()
                .HasOne(sp => sp.Package)
                .WithMany(p => p.ServicePackages)
                .HasForeignKey(sp => sp.packageID)
                .OnDelete(DeleteBehavior.Cascade);
            


            // 🔹 Định dạng kiểu tiền tệ
            modelBuilder.Entity<Service>()
                .Property(s => s.price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Package>()
                .Property(p => p.price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Booking>()
                .Property(b => b.totalPrice)
                .HasColumnType("decimal(18,2)");


            // Quan hệ giữa Booking và RescueStation (1-N)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.RescueStation)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.rescueStationId)
                .OnDelete(DeleteBehavior.SetNull);

            // Quan hệ giữa User (Staff) và RescueStation (1-N)
            modelBuilder.Entity<User>()
                .HasOne(u => u.RescueStation)
                .WithMany(r => r.Staffs)
                .HasForeignKey(u => u.rescueStationId)
                .OnDelete(DeleteBehavior.SetNull);

            ////user - transaction
            //modelBuilder.Entity<Transaction>()
            //.HasOne(t => t.Wallet)
            //.WithMany()
            //.HasForeignKey(t => t.id);

            // transaction - booking/package
            modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Booking)
            .WithMany(b => b.Transactions)
            .HasForeignKey(t => t.bookingId)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Package)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.packageId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Vehicle)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.carId)
                .OnDelete(DeleteBehavior.SetNull);


            base.OnModelCreating(modelBuilder);



            DbSeeder.Seed(modelBuilder);
        }
        public override int SaveChanges() => base.SaveChanges();
    }
}
