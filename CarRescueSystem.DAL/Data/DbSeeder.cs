using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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
        // User 
        private static readonly Guid UserIDTest = Guid.Parse("B2DAB1C3-6D48-4B23-8369-2D1C9C828F22");
        private static readonly Guid AdminIDTest = Guid.Parse("C3DAB1C3-6D48-4B23-8369-2D1C9C828F23");
        private static readonly Guid StaffIDTest1 = Guid.Parse("D3DAB1C3-6D48-4B23-8369-2D1C9C828F24");
        private static readonly Guid StaffIDTest2 = Guid.Parse("E9DB2278-2C9A-40D9-82B8-F8AF452A382F");
        private static readonly Guid ReceptionistIDTest = Guid.Parse("E3DAB1C3-6D48-4B23-8369-2D1C9C828F25");


        // RoleId
        private static readonly Guid CustomerRole = Guid.Parse("A1A2A3A4-B5B6-C7C8-D9D0-E1E2E3E4E5E6");
        private static readonly Guid StaffRole = Guid.Parse("B1B2B3B4-C5C6-D7D8-E9E0-F1F2F3F4F5F6");
        private static readonly Guid AdminRole = Guid.Parse("C1C2C3C4-D5D6-E7E8-F9F0-A1A2A3A4A5A6");
        private static readonly Guid ReceptionistRole = Guid.Parse("D1D2D3D4-E5E6-F7F8-A9A0-B1B2B3B4B5B6");

        // Service
        private static readonly Guid Brakes = Guid.Parse("A1E2C3D4-5F6A-7B8C-9D0E-1F2A3B4C5D6E");
        private static readonly Guid ElectricalIssues = Guid.Parse("B2D3E4F5-6A7B-8C9D-0E1F-2A3B4C5D6E7F");
        private static readonly Guid SteeringSystem = Guid.Parse("C3D4E5F6-7A8B-9C0D-1E2F-3A4B5C6D7E8F");
        private static readonly Guid EngineIssues = Guid.Parse("D4E5F6A7-8B9C-0D1E-2F3A-4B5C6D7E8F9A");
        private static readonly Guid TireProblems = Guid.Parse("E5F6A7B8-9C0D-1E2F-3A4B-5C6D7E8F9A0B");
        private static readonly Guid BatteryJumpStart = Guid.Parse("F6A7B8C9-0D1E-2F3A-4B5C-6D7E8F9A0B1C");
        private static readonly Guid FuelRefill = Guid.Parse("A7B8C9D0-1E2F-3A4B-5C6D-7E8F9A0B1C2D");
        private static readonly Guid WrongFuelRefill = Guid.Parse("B8C9D0E1-2F3A-4B5C-6D7E-8F9A0B1C2D3E");
        private static readonly Guid LockedOut = Guid.Parse("C9D0E1F2-3A4B-5C6D-7E8F-9A0B1C2D3E4F");
        private static readonly Guid Collision = Guid.Parse("D0E1F2A3-4B5C-6D7E-8F9A-0B1C2D3E4F5A");
        private static readonly Guid MinorCrash = Guid.Parse("E1F2A3B4-5C6D-7E8F-9A0B-1C2D3E4F5A6B");
        private static readonly Guid Rollover = Guid.Parse("F2A3B4C5-6D7E-8F9A-0B1C-2D3E4F5A6B7C");
        private static readonly Guid FireOrExplosion = Guid.Parse("A3B4C5D6-7E8F-9A0B-1C2D-3E4F5A6B7C8D");
        private static readonly Guid VehicleFall = Guid.Parse("B4C5D6E7-8F9A-0B1C-2D3E-4F5A6B7C8D9E");
        private static readonly Guid SubmergedVehicle = Guid.Parse("C5D6E7F8-9A0B-1C2D-3E4F-5A6B7C8D9E0F");
        private static readonly Guid Hydrolock = Guid.Parse("D6E7F8A9-0B1C-2D3E-4F5A-6B7C8D9E0F1A");
        private static readonly Guid CarTowing = Guid.Parse("E7F8A9B0-1C2D-3E4F-5A6B-7C8D9E0F1A2B");

        // Package
        private static readonly Guid BasicPackage = Guid.Parse("11111111-2222-3333-4444-555555555555");
        private static readonly Guid ComprehensivePackage = Guid.Parse("66666666-7777-8888-9999-000000000000");
        private static readonly Guid PremiumPackage = Guid.Parse("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE");

        // ServicePackage
        // BASIC
        private static readonly Guid BasicService1 = Guid.Parse("12345678-90AB-CDEF-1234-567890ABCDEF");
        private static readonly Guid BasicService2 = Guid.Parse("22345678-90AB-CDEF-1234-567890ABCDEF");
        private static readonly Guid BasicService3 = Guid.Parse("32345678-90AB-CDEF-1234-567890ABCDEF");
        private static readonly Guid BasicService4 = Guid.Parse("42345678-90AB-CDEF-1234-567890ABCDEF");
        private static readonly Guid BasicService5 = Guid.Parse("52345678-90AB-CDEF-1234-567890ABCDEF");
        private static readonly Guid BasicService6 = Guid.Parse("62345678-90AB-CDEF-1234-567890ABCDEF");
        private static readonly Guid BasicService7 = Guid.Parse("72345678-90AB-CDEF-1234-567890ABCDEF");
        private static readonly Guid BasicService8 = Guid.Parse("82345678-90AB-CDEF-1234-567890ABCDEF");
        private static readonly Guid BasicService9 = Guid.Parse("92345678-90AB-CDEF-1234-567890ABCDEF");
        private static readonly Guid BasicService10 = Guid.Parse("A2345678-90AB-CDEF-1234-567890ABCDEF");

        // COMPREHENSIVE
        private static readonly Guid ComprehensiveService1 = Guid.Parse("23456789-0ABC-DEF1-2345-67890ABCDEFA");
        private static readonly Guid ComprehensiveService2 = Guid.Parse("B3456789-0ABC-DEF1-2345-67890ABCDEFA");
        private static readonly Guid ComprehensiveService3 = Guid.Parse("C3456789-0ABC-DEF1-2345-67890ABCDEFA");
        private static readonly Guid ComprehensiveService4 = Guid.Parse("D3456789-0ABC-DEF1-2345-67890ABCDEFA");
        private static readonly Guid ComprehensiveService5 = Guid.Parse("E3456789-0ABC-DEF1-2345-67890ABCDEFA");
        private static readonly Guid ComprehensiveService6 = Guid.Parse("F3456789-0ABC-DEF1-2345-67890ABCDEFA");
        private static readonly Guid ComprehensiveService7 = Guid.Parse("13456789-0ABC-DEF1-2345-67890ABCDEFA");
        private static readonly Guid ComprehensiveService8 = Guid.Parse("23456789-0ABC-DEF1-2345-67890ABCDEFB");
        private static readonly Guid ComprehensiveService9 = Guid.Parse("33456789-0ABC-DEF1-2345-67890ABCDEFB");
        private static readonly Guid ComprehensiveService10 = Guid.Parse("43456789-0ABC-DEF1-2345-67890ABCDEFB");
        private static readonly Guid ComprehensiveService11 = Guid.Parse("53456789-0ABC-DEF1-2345-67890ABCDEFB");
        private static readonly Guid ComprehensiveService12 = Guid.Parse("63456789-0ABC-DEF1-2345-67890ABCDEFB");
        private static readonly Guid ComprehensiveService13 = Guid.Parse("73456789-0ABC-DEF1-2345-67890ABCDEFB");
        private static readonly Guid ComprehensiveService14 = Guid.Parse("83456789-0ABC-DEF1-2345-67890ABCDEFB");
        private static readonly Guid ComprehensiveService15 = Guid.Parse("93456789-0ABC-DEF1-2345-67890ABCDEFB");
        private static readonly Guid ComprehensiveService16 = Guid.Parse("A3456789-0ABC-DEF1-2345-67890ABCDEFB");

        // PREMIUM
        private static readonly Guid PremiumService1 = Guid.Parse("34567890-ABCD-EF12-3456-7890ABCDEFAB");
        private static readonly Guid PremiumService2 = Guid.Parse("B4567890-ABCD-EF12-3456-7890ABCDEFAB");
        private static readonly Guid PremiumService3 = Guid.Parse("C4567890-ABCD-EF12-3456-7890ABCDEFAB");
        private static readonly Guid PremiumService4 = Guid.Parse("D4567890-ABCD-EF12-3456-7890ABCDEFAB");
        private static readonly Guid PremiumService5 = Guid.Parse("E4567890-ABCD-EF12-3456-7890ABCDEFAB");
        private static readonly Guid PremiumService6 = Guid.Parse("F4567890-ABCD-EF12-3456-7890ABCDEFAB");
        private static readonly Guid PremiumService7 = Guid.Parse("14567890-ABCD-EF12-3456-7890ABCDEFAB");
        private static readonly Guid PremiumService8 = Guid.Parse("24567890-ABCD-EF12-3456-7890ABCDEFAB");
        private static readonly Guid PremiumService9 = Guid.Parse("34567890-ABCD-EF12-3456-7890ABCDEFAC");
        private static readonly Guid PremiumService10 = Guid.Parse("44567890-ABCD-EF12-3456-7890ABCDEFAC");
        private static readonly Guid PremiumService11 = Guid.Parse("54567890-ABCD-EF12-3456-7890ABCDEFAC");
        private static readonly Guid PremiumService12 = Guid.Parse("64567890-ABCD-EF12-3456-7890ABCDEFAC");
        private static readonly Guid PremiumService13 = Guid.Parse("74567890-ABCD-EF12-3456-7890ABCDEFAC");
        private static readonly Guid PremiumService14 = Guid.Parse("84567890-ABCD-EF12-3456-7890ABCDEFAC");
        private static readonly Guid PremiumService15 = Guid.Parse("94567890-ABCD-EF12-3456-7890ABCDEFAC");
        private static readonly Guid PremiumService16 = Guid.Parse("A4567890-ABCD-EF12-3456-7890ABCDEFAC");
        private static readonly Guid PremiumService17 = Guid.Parse("B4567890-ABCD-EF12-3456-7890ABCDEFAC");

        // Vehicle ID mới
        private static readonly Guid VehicleCustomerTestId = Guid.Parse("12345678-90AB-CDEF-1234-567890ABCDEF");
        // ID của các trạm cứu hộ
        private static readonly Guid RescueStationId1 = Guid.Parse("A1B2C3D4-E5F6-7890-1234-56789ABCDEF1");
        private static readonly Guid RescueStationId2 = Guid.Parse("A1B2C3D4-E5F6-7890-1234-56789ABCDEF2");
        private static readonly Guid RescueStationId3 = Guid.Parse("A1B2C3D4-E5F6-7890-1234-56789ABCDEF3");
        private static readonly Guid RescueStationId4 = Guid.Parse("A1B2C3D4-E5F6-7890-1234-56789ABCDEF4");
        private static readonly Guid RescueStationId5 = Guid.Parse("A1B2C3D4-E5F6-7890-1234-56789ABCDEF5");

        public DbSeeder(ApplicationDbContext context)
        {
            _context = context;
        }
        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedUser(modelBuilder);
            SeedRoles(modelBuilder);
            SeedServices(modelBuilder);
            SeedPackages(modelBuilder);
            SeedServicePackage(modelBuilder);
            SeedVehicle(modelBuilder);
            SeedRescueStations(modelBuilder);
            SeedWallet(modelBuilder);
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
                    RoleID = new Guid("A1A2A3A4-B5B6-C7C8-D9D0-E1E2E3E4E5E6")
                },
                new User
                {
                    UserId = AdminIDTest,
                    FullName = "Test Admin",
                    Email = "testAdmin@gmail.com",
                    PasswordHash = fixedHashedPassword,
                    PasswordSalt = "",
                    PhoneNumber = "0999888777",
                    RoleID = new Guid("C1C2C3C4-D5D6-E7E8-F9F0-A1A2A3A4A5A6")
                },
                new User
                {
                    UserId = StaffIDTest1,
                    FullName = "Test Staff 1",
                    Email = "testStaff1@gmail.com",
                    PasswordHash = fixedHashedPassword,
                    PasswordSalt = "",
                    PhoneNumber = "0999888777",
                    RoleID = new Guid("B1B2B3B4-C5C6-D7D8-E9E0-F1F2F3F4F5F6"),
                    StaffStatus = StaffStatus.ACTIVE,
                    RescueStationId = RescueStationId1

                },
                new User
                {
                    UserId = StaffIDTest2,
                    FullName = "Test Staff 2",
                    Email = "testStaff2@gmail.com",
                    PasswordHash = fixedHashedPassword,
                    PasswordSalt = "",
                    PhoneNumber = "0999888777",
                    RoleID = new Guid("B1B2B3B4-C5C6-D7D8-E9E0-F1F2F3F4F5F6"),
                    StaffStatus = StaffStatus.ACTIVE,
                    RescueStationId = RescueStationId1

                },
                new User
                {
                    UserId = ReceptionistIDTest,
                    FullName = "Test Receptionist",
                    Email = "testReceptionist@gmail.com",
                    PasswordHash = fixedHashedPassword,
                    PasswordSalt = "",
                    PhoneNumber = "0999888777",
                    RoleID = new Guid("D1D2D3D4-E5E6-F7F8-A9A0-B1B2B3B4B5B6")
                },
                new User
                {
                    UserId = Guid.Parse("F1DAB1C3-6D48-4B23-8369-2D1C9C828F26"),
                    FullName = "Test Staff 3",
                    Email = "testStaff3@gmail.com",
                    PasswordHash = fixedHashedPassword,
                    PasswordSalt = "",
                    PhoneNumber = "0999888773",
                    RoleID = StaffRole,
                    StaffStatus = StaffStatus.ACTIVE,
                    RescueStationId = RescueStationId2
                },
            new User
            {
                UserId = Guid.Parse("F2DAB1C3-6D48-4B23-8369-2D1C9C828F27"),
                FullName = "Test Staff 4",
                Email = "testStaff4@gmail.com",
                PasswordHash = fixedHashedPassword,
                PasswordSalt = "",
                PhoneNumber = "0999888774",
                RoleID = StaffRole,
                StaffStatus = StaffStatus.ACTIVE,
                RescueStationId = RescueStationId2
            },
            new User
            {
                UserId = Guid.Parse("F3DAB1C3-6D48-4B23-8369-2D1C9C828F28"),
                FullName = "Test Staff 5",
                Email = "testStaff5@gmail.com",
                PasswordHash = fixedHashedPassword,
                PasswordSalt = "",
                PhoneNumber = "0999888775",
                RoleID = StaffRole,
                StaffStatus = StaffStatus.ACTIVE,
                RescueStationId = RescueStationId3
            },
            new User
            {
                UserId = Guid.Parse("F4DAB1C3-6D48-4B23-8369-2D1C9C828F29"),
                FullName = "Test Staff 6",
                Email = "testStaff6@gmail.com",
                PasswordHash = fixedHashedPassword,
                PasswordSalt = "",
                PhoneNumber = "0999888776",
                RoleID = StaffRole,
                StaffStatus = StaffStatus.ACTIVE,
                RescueStationId = RescueStationId3
            },
            new User
            {
                UserId = Guid.Parse("F5DAB1C3-6D48-4B23-8369-2D1C9C828F30"),
                FullName = "Test Staff 7",
                Email = "testStaff7@gmail.com",
                PasswordHash = fixedHashedPassword,
                PasswordSalt = "",
                PhoneNumber = "0999888778",
                RoleID = StaffRole,
                StaffStatus = StaffStatus.ACTIVE,
                RescueStationId = RescueStationId4
            },
            new User
            {
                UserId = Guid.Parse("F6DAB1C3-6D48-4B23-8369-2D1C9C828F31"),
                FullName = "Test Staff 8",
                Email = "testStaff8@gmail.com",
                PasswordHash = fixedHashedPassword,
                PasswordSalt = "",
                PhoneNumber = "0999888779",
                RoleID = StaffRole,
                StaffStatus = StaffStatus.ACTIVE,
                RescueStationId = RescueStationId4
            },
            new User
            {
                UserId = Guid.Parse("F7DAB1C3-6D48-4B23-8369-2D1C9C828F32"),
                FullName = "Test Staff 9",
                Email = "testStaff9@gmail.com",
                PasswordHash = fixedHashedPassword,
                PasswordSalt = "",
                PhoneNumber = "0999888780",
                RoleID = StaffRole,
                StaffStatus = StaffStatus.ACTIVE,
                RescueStationId = RescueStationId5
            },
            new User
            {
                UserId = Guid.Parse("F8DAB1C3-6D48-4B23-8369-2D1C9C828F33"),
                FullName = "Test Staff 10",
                Email = "testStaff10@gmail.com",
                PasswordHash = fixedHashedPassword,
                PasswordSalt = "",
                PhoneNumber = "0999888781",
                RoleID = StaffRole,
                StaffStatus = StaffStatus.ACTIVE,
                RescueStationId = RescueStationId5
            }

                );
        }
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = CustomerRole, RoleName = "CUSTOMER" },
                new Role { RoleID = StaffRole, RoleName = "STAFF" },
                new Role { RoleID = AdminRole, RoleName = "ADMIN" },
                new Role { RoleID = ReceptionistRole, RoleName = "RECEPTIONIST" }
            );
            
        }
        private static void SeedServices(ModelBuilder modelBuilder)
        {
            var services = new List<Service>
        {
            new Service { ServiceId = Brakes, ServiceName = "Brakes", ServicePrice = 500000 },
            new Service { ServiceId = ElectricalIssues, ServiceName = "Electrical Issues", ServicePrice = 400000 },
            new Service { ServiceId = SteeringSystem, ServiceName = "Steering System", ServicePrice = 450000 },
            new Service { ServiceId = EngineIssues, ServiceName = "Engine Issues", ServicePrice = 700000 },
            new Service { ServiceId = TireProblems, ServiceName = "Tire Problems", ServicePrice = 300000 },
            new Service { ServiceId = BatteryJumpStart, ServiceName = "Battery Jump Start", ServicePrice = 200000 },
            new Service { ServiceId = FuelRefill, ServiceName = "Fuel Refill", ServicePrice = 250000 },
            new Service { ServiceId = WrongFuelRefill, ServiceName = "Wrong Fuel Refill", ServicePrice = 350000 },  
            new Service { ServiceId = LockedOut, ServiceName = "Locked Out (Forgot Keys)", ServicePrice = 150000 },
            new Service { ServiceId = Collision, ServiceName = "Collision", ServicePrice = 1000000 },
            new Service { ServiceId = MinorCrash, ServiceName = "Minor Crash", ServicePrice = 800000 },
            new Service { ServiceId = Rollover, ServiceName = "Rollover", ServicePrice = 1200000 },
            new Service { ServiceId = FireOrExplosion, ServiceName = "Fire or Explosion", ServicePrice = 2000000 },
            new Service { ServiceId = VehicleFall, ServiceName = "Vehicle Fall", ServicePrice = 1100000 },
            new Service { ServiceId = SubmergedVehicle, ServiceName = "Submerged Vehicle", ServicePrice = 1300000 },
            new Service { ServiceId = Hydrolock, ServiceName = "Hydrolock", ServicePrice = 900000 },
            new Service { ServiceId = CarTowing, ServiceName = "Car Towing", ServicePrice = 500000 }
        };

            modelBuilder.Entity<Service>().HasData(services);
        }
        private static void SeedPackages(ModelBuilder modelBuilder)
        {
            var packages = new List<Package>
        {
            new Package { PackageId = BasicPackage, PackageName = "Basic Package", PackagePrice = 500000 },
            new Package { PackageId = ComprehensivePackage, PackageName = "Comprehensive Package", PackagePrice = 1000000 },
            new Package { PackageId = PremiumPackage, PackageName = "Premium Package", PackagePrice = 2000000 }
        };

            modelBuilder.Entity<Package>().HasData(packages);
        }
        private static void SeedServicePackage(ModelBuilder modelBuilder)
        {
            var servicePackage = new List<ServicePackage>
            {
                new ServicePackage { ServicePackageId = BasicService1, PackageID = new Guid("11111111-2222-3333-4444-555555555555"), ServiceId =new Guid( "A1E2C3D4-5F6A-7B8C-9D0E-1F2A3B4C5D6E")},
                new ServicePackage { ServicePackageId = BasicService2, PackageID = new Guid("11111111-2222-3333-4444-555555555555"), ServiceId =new Guid ( "B2D3E4F5-6A7B-8C9D-0E1F-2A3B4C5D6E7F")},
                new ServicePackage { ServicePackageId = BasicService3, PackageID = new Guid("11111111-2222-3333-4444-555555555555"), ServiceId = new Guid("C3D4E5F6-7A8B-9C0D-1E2F-3A4B5C6D7E8F")},
                new ServicePackage { ServicePackageId = BasicService4, PackageID = new Guid("11111111-2222-3333-4444-555555555555"), ServiceId = new Guid("D4E5F6A7-8B9C-0D1E-2F3A-4B5C6D7E8F9A")},
                new ServicePackage { ServicePackageId = BasicService5, PackageID = new Guid("11111111-2222-3333-4444-555555555555"), ServiceId = new Guid("E5F6A7B8-9C0D-1E2F-3A4B-5C6D7E8F9A0B")},
                new ServicePackage { ServicePackageId = BasicService6, PackageID = new Guid("11111111-2222-3333-4444-555555555555"), ServiceId = new Guid("F6A7B8C9-0D1E-2F3A-4B5C-6D7E8F9A0B1C")},
                new ServicePackage { ServicePackageId = BasicService7, PackageID = new Guid("11111111-2222-3333-4444-555555555555"), ServiceId = new Guid("D0E1F2A3-4B5C-6D7E-8F9A-0B1C2D3E4F5A")},
                new ServicePackage { ServicePackageId = BasicService8, PackageID = new Guid("11111111-2222-3333-4444-555555555555"), ServiceId = new Guid("E1F2A3B4-5C6D-7E8F-9A0B-1C2D3E4F5A6B")},
                new ServicePackage { ServicePackageId = BasicService9, PackageID = new Guid("11111111-2222-3333-4444-555555555555"), ServiceId = new Guid("F2A3B4C5-6D7E-8F9A-0B1C-2D3E4F5A6B7C")},
                new ServicePackage { ServicePackageId = BasicService10, PackageID = new Guid("11111111-2222-3333-4444-555555555555"), ServiceId = CarTowing},

                new ServicePackage { ServicePackageId = ComprehensiveService1, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid( "A1E2C3D4-5F6A-7B8C-9D0E-1F2A3B4C5D6E")},
                new ServicePackage { ServicePackageId = ComprehensiveService2, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid ( "B2D3E4F5-6A7B-8C9D-0E1F-2A3B4C5D6E7F")},
                new ServicePackage { ServicePackageId = ComprehensiveService3, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("C3D4E5F6-7A8B-9C0D-1E2F-3A4B5C6D7E8F")},
                new ServicePackage { ServicePackageId = ComprehensiveService4, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("D4E5F6A7-8B9C-0D1E-2F3A-4B5C6D7E8F9A")},
                new ServicePackage { ServicePackageId = ComprehensiveService5, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("E5F6A7B8-9C0D-1E2F-3A4B-5C6D7E8F9A0B")},
                new ServicePackage { ServicePackageId = ComprehensiveService6, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("F6A7B8C9-0D1E-2F3A-4B5C-6D7E8F9A0B1C")},
                new ServicePackage { ServicePackageId = ComprehensiveService7, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("A7B8C9D0-1E2F-3A4B-5C6D-7E8F9A0B1C2D")},
                new ServicePackage { ServicePackageId = ComprehensiveService8, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("B8C9D0E1-2F3A-4B5C-6D7E-8F9A0B1C2D3E")},
                new ServicePackage { ServicePackageId = ComprehensiveService9, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("C9D0E1F2-3A4B-5C6D-7E8F-9A0B1C2D3E4F")},
                new ServicePackage { ServicePackageId = ComprehensiveService10, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("D0E1F2A3-4B5C-6D7E-8F9A-0B1C2D3E4F5A")},
                new ServicePackage { ServicePackageId = ComprehensiveService11, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("E1F2A3B4-5C6D-7E8F-9A0B-1C2D3E4F5A6B")},
                new ServicePackage { ServicePackageId = ComprehensiveService12, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("F2A3B4C5-6D7E-8F9A-0B1C-2D3E4F5A6B7C")},
                new ServicePackage { ServicePackageId = ComprehensiveService13, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("A3B4C5D6-7E8F-9A0B-1C2D-3E4F5A6B7C8D")},
                new ServicePackage { ServicePackageId = ComprehensiveService14, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("B4C5D6E7-8F9A-0B1C-2D3E-4F5A6B7C8D9E")},
                new ServicePackage { ServicePackageId = ComprehensiveService15, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("C5D6E7F8-9A0B-1C2D-3E4F-5A6B7C8D9E0F")},
                new ServicePackage { ServicePackageId = ComprehensiveService16, PackageID = new Guid("66666666-7777-8888-9999-000000000000"), ServiceId = new Guid("E7F8A9B0-1C2D-3E4F-5A6B-7C8D9E0F1A2B")},

                new ServicePackage { ServicePackageId = PremiumService1, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid( "A1E2C3D4-5F6A-7B8C-9D0E-1F2A3B4C5D6E")},
                new ServicePackage { ServicePackageId = PremiumService2, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid ( "B2D3E4F5-6A7B-8C9D-0E1F-2A3B4C5D6E7F")},
                new ServicePackage { ServicePackageId = PremiumService3, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("C3D4E5F6-7A8B-9C0D-1E2F-3A4B5C6D7E8F")},
                new ServicePackage { ServicePackageId = PremiumService4, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("D4E5F6A7-8B9C-0D1E-2F3A-4B5C6D7E8F9A")},
                new ServicePackage { ServicePackageId = PremiumService5, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("E5F6A7B8-9C0D-1E2F-3A4B-5C6D7E8F9A0B")},
                new ServicePackage { ServicePackageId = PremiumService6, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("F6A7B8C9-0D1E-2F3A-4B5C-6D7E8F9A0B1C")},
                new ServicePackage { ServicePackageId = PremiumService7, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("A7B8C9D0-1E2F-3A4B-5C6D-7E8F9A0B1C2D")},
                new ServicePackage { ServicePackageId = PremiumService8, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("B8C9D0E1-2F3A-4B5C-6D7E-8F9A0B1C2D3E")},
                new ServicePackage { ServicePackageId = PremiumService9, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("C9D0E1F2-3A4B-5C6D-7E8F-9A0B1C2D3E4F")},
                new ServicePackage { ServicePackageId = PremiumService10, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("D0E1F2A3-4B5C-6D7E-8F9A-0B1C2D3E4F5A")},
                new ServicePackage { ServicePackageId = PremiumService11, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("E1F2A3B4-5C6D-7E8F-9A0B-1C2D3E4F5A6B")},
                new ServicePackage { ServicePackageId = PremiumService12, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("F2A3B4C5-6D7E-8F9A-0B1C-2D3E4F5A6B7C")},
                new ServicePackage { ServicePackageId = PremiumService13, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("A3B4C5D6-7E8F-9A0B-1C2D-3E4F5A6B7C8D")},
                new ServicePackage { ServicePackageId = PremiumService14, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("B4C5D6E7-8F9A-0B1C-2D3E-4F5A6B7C8D9E")},
                new ServicePackage { ServicePackageId = PremiumService15, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("C5D6E7F8-9A0B-1C2D-3E4F-5A6B7C8D9E0F")},
                new ServicePackage { ServicePackageId = PremiumService16, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("D6E7F8A9-0B1C-2D3E-4F5A-6B7C8D9E0F1A")},
                new ServicePackage { ServicePackageId = PremiumService17, PackageID = new Guid("AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE"), ServiceId = new Guid("E7F8A9B0-1C2D-3E4F-5A6B-7C8D9E0F1A2B")},

            };
            modelBuilder.Entity<ServicePackage>().HasData(servicePackage);
        }
        private static void SeedVehicle (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasData(new Vehicle
            {
                VehicleId = VehicleCustomerTestId,
                CustomerId = new Guid("B2DAB1C3-6D48-4B23-8369-2D1C9C828F22"),
                VehicleName = "Test",
                VehicleColor = "Test",
                VehicleBrand = "Test",
                NumberOfSeats = 4,
                LicensePlate = "30G-49344"
            }
            );
        }
        private static void SeedRescueStations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RescueStation>().HasData(
                new RescueStation
                {
                    RescueStationId = RescueStationId1,
                    Name = "Trạm Cứu Hộ Quận 1",
                    Address = "86 Đinh Tiên Hoàng, Đa Kao, Quận 1, TP.HCM",
                    Latitude = 10.782222,
                    Longitude = 106.699722,
                    PhoneNumber = "02812345678",
                    Email = "rescue.q1@carrescue.vn"
                },
                new RescueStation
                {
                    RescueStationId = RescueStationId2,
                    Name = "Trạm Cứu Hộ Quận 3",
                    Address = "273 Nguyễn Thiện Thuật, Phường 1, Quận 3, TP.HCM",
                    Latitude = 10.770049,
                    Longitude = 106.682846,
                    PhoneNumber = "02812345679",
                    Email = "rescue.q3@carrescue.vn"
                },
                new RescueStation
                {
                    RescueStationId = RescueStationId3,
                    Name = "Trạm Cứu Hộ Quận 5",
                    Address = "161 Trần Hưng Đạo, Phường 10, Quận 5, TP.HCM",
                    Latitude = 10.754345,
                    Longitude = 106.663983,
                    PhoneNumber = "02812345680",
                    Email = "rescue.q5@carrescue.vn"
                },
                new RescueStation
                {
                    RescueStationId = RescueStationId4,
                    Name = "Trạm Cứu Hộ Quận 7",
                    Address = "502 Nguyễn Văn Linh, Tân Phong, Quận 7, TP.HCM",
                    Latitude = 10.730745,
                    Longitude = 106.721640,
                    PhoneNumber = "02812345681",
                    Email = "rescue.q7@carrescue.vn"
                },
                new RescueStation
                {
                    RescueStationId = RescueStationId5,
                    Name = "Trạm Cứu Hộ Quận Bình Thạnh",
                    Address = "208 Xô Viết Nghệ Tĩnh, Phường 21, Bình Thạnh, TP.HCM",
                    Latitude = 10.803537,
                    Longitude = 106.713179,
                    PhoneNumber = "02812345682",
                    Email = "rescue.bt@carrescue.vn"
                }
            );
        }
        private static void SeedWallet(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>().HasData(new Wallet
            {
                UserId = UserIDTest,
                Balance = 50000000
            }
            );
        }
    }
}
