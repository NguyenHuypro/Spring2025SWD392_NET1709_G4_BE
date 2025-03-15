//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CarRescueSystem.DAL.Model;
//using Microsoft.EntityFrameworkCore;

//namespace CarRescueSystem.DAL.Data
//{
//    public interface IApplicationDbContext
//    {
//        DbSet<User> Users { get; set; }
//        DbSet<RefreshToken> RefreshTokens { get; set; }
//        DbSet<Booking> Bookings { get; set; }
//        DbSet<Vehicle> Vehicles { get; set; }
//        DbSet<Service> Services { get; set; }
//        DbSet<Package> Packages { get; set; }
//        DbSet<ServiceOfBooking> ServiceOfBookings { get; set; }
//        DbSet<BookingStaff> BookingStaffs { get; set; }
//        DbSet<ServicePackage> ServicePackages { get; set; }
//        DbSet<Schedule> Schedules { get; set; }
//        DbSet<RescueStation> RescueStations { get; set; }
//        //DbSet<Wallet> Wallets { get; set; }
//        DbSet<Transaction> Transactions { get; set; }

//        int SaveChanges();
//    }

//}
