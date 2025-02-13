using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace CarRescueSystem.DAL.Data
{
    public class DbSeeder
    {
        private readonly ApplicationDbContext _context;
        //
        private static readonly Guid UserIDTest = Guid.Parse("B2DAB1C3-6D48-4B23-8369-2D1C9C828F22");
        private static readonly Guid CustomerRole = Guid.Parse("C3DAB1C3-6D48-4B23-8369-2D1C9C828F22");

        public DbSeeder(ApplicationDbContext context)
        {
            _context = context;
        }
        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedUser(modelBuilder);
            SeedRoles(modelBuilder);
        }
        private static void SeedUser(ModelBuilder modelBuilder)
        {
            //pass: 123
            string fixedHashedPassword = "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm";
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = UserIDTest,
                    FullName = "Test User",
                    Email = "testUser@gmail.com",
                    PasswordHash = fixedHashedPassword,
                    PasswordSalt = "",
                    PhoneNumber = "0999888777",
                    RoleID = new Guid("C3DAB1C3-6D48-4B23-8369-2D1C9C828F22")
                }
                );
        }
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = CustomerRole, RoleName = "Customer" }
            );
        }
    }
}
