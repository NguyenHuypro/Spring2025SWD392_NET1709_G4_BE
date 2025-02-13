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

        public DbSet<User> Users { get; set; } // Thay thế với các model của bạn
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<RefreshToken>().HasKey(rt => rt.RefreshTokenId);

            //Token
            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.UserId);

            modelBuilder.Entity<RefreshToken>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            //User-role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleID);

            base.OnModelCreating(modelBuilder);

            DbSeeder.Seed(modelBuilder);
        }
    }
}
