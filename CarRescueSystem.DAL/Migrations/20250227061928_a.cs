using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRescueSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PackagePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.PackageId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ServicePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaffStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServicePackages",
                columns: table => new
                {
                    ServicePackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackages", x => x.ServicePackageId);
                    table.ForeignKey(
                        name: "FK_ServicePackages_Packages_PackageID",
                        column: x => x.PackageID,
                        principalTable: "Packages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicePackages_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    RefreshTokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshTokenKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPackages",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPackages", x => new { x.UserId, x.PackageId });
                    table.ForeignKey(
                        name: "FK_UserPackages_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPackages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VehicleColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VehicleBrand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_Vehicles_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Evidence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "PackageId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BookingStaffs",
                columns: table => new
                {
                    BookingStaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingStaffs", x => x.BookingStaffId);
                    table.ForeignKey(
                        name: "FK_BookingStaffs_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingStaffs_Users_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceOfBookings",
                columns: table => new
                {
                    ServiceOfBookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOfBookings", x => x.ServiceOfBookingId);
                    table.ForeignKey(
                        name: "FK_ServiceOfBookings_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceOfBookings_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "PackageId", "PackageName", "PackagePrice" },
                values: new object[,]
                {
                    { new Guid("11111111-2222-3333-4444-555555555555"), "Basic Package", 500000m },
                    { new Guid("66666666-7777-8888-9999-000000000000"), "Comprehensive Package", 1000000m },
                    { new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), "Premium Package", 2000000m }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleID", "RoleName" },
                values: new object[,]
                {
                    { new Guid("a1a2a3a4-b5b6-c7c8-d9d0-e1e2e3e4e5e6"), "Customer" },
                    { new Guid("b1b2b3b4-c5c6-d7d8-e9e0-f1f2f3f4f5f6"), "Staff" },
                    { new Guid("c1c2c3c4-d5d6-e7e8-f9f0-a1a2a3a4a5a6"), "Admin" },
                    { new Guid("d1d2d3d4-e5e6-f7f8-a9a0-b1b2b3b4b5b6"), "Receptionist" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceId", "ServiceName", "ServicePrice" },
                values: new object[,]
                {
                    { new Guid("a1e2c3d4-5f6a-7b8c-9d0e-1f2a3b4c5d6e"), "Brakes", 500000m },
                    { new Guid("a3b4c5d6-7e8f-9a0b-1c2d-3e4f5a6b7c8d"), "Fire or Explosion", 2000000m },
                    { new Guid("a7b8c9d0-1e2f-3a4b-5c6d-7e8f9a0b1c2d"), "Fuel Refill", 250000m },
                    { new Guid("b2d3e4f5-6a7b-8c9d-0e1f-2a3b4c5d6e7f"), "Electrical Issues", 400000m },
                    { new Guid("b4c5d6e7-8f9a-0b1c-2d3e-4f5a6b7c8d9e"), "Vehicle Fall", 1100000m },
                    { new Guid("b8c9d0e1-2f3a-4b5c-6d7e-8f9a0b1c2d3e"), "Wrong Fuel Refill", 350000m },
                    { new Guid("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f"), "Steering System", 450000m },
                    { new Guid("c5d6e7f8-9a0b-1c2d-3e4f-5a6b7c8d9e0f"), "Submerged Vehicle", 1300000m },
                    { new Guid("c9d0e1f2-3a4b-5c6d-7e8f-9a0b1c2d3e4f"), "Locked Out (Forgot Keys)", 150000m },
                    { new Guid("d0e1f2a3-4b5c-6d7e-8f9a-0b1c2d3e4f5a"), "Collision", 1000000m },
                    { new Guid("d4e5f6a7-8b9c-0d1e-2f3a-4b5c6d7e8f9a"), "Engine Issues", 700000m },
                    { new Guid("d6e7f8a9-0b1c-2d3e-4f5a-6b7c8d9e0f1a"), "Hydrolock", 900000m },
                    { new Guid("e1f2a3b4-5c6d-7e8f-9a0b-1c2d3e4f5a6b"), "Minor Crash", 800000m },
                    { new Guid("e5f6a7b8-9c0d-1e2f-3a4b-5c6d7e8f9a0b"), "Tire Problems", 300000m },
                    { new Guid("e7f8a9b0-1c2d-3e4f-5a6b-7c8d9e0f1a2b"), "Car Towing", 500000m },
                    { new Guid("f2a3b4c5-6d7e-8f9a-0b1c-2d3e4f5a6b7c"), "Rollover", 1200000m },
                    { new Guid("f6a7b8c9-0d1e-2f3a-4b5c-6d7e8f9a0b1c"), "Battery Jump Start", 200000m }
                });

            migrationBuilder.InsertData(
                table: "ServicePackages",
                columns: new[] { "ServicePackageId", "PackageID", "Quantity", "ServiceId" },
                values: new object[,]
                {
                    { new Guid("12345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), 0, new Guid("a1e2c3d4-5f6a-7b8c-9d0e-1f2a3b4c5d6e") },
                    { new Guid("13456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("a7b8c9d0-1e2f-3a4b-5c6d-7e8f9a0b1c2d") },
                    { new Guid("14567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("a7b8c9d0-1e2f-3a4b-5c6d-7e8f9a0b1c2d") },
                    { new Guid("22345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), 0, new Guid("b2d3e4f5-6a7b-8c9d-0e1f-2a3b4c5d6e7f") },
                    { new Guid("23456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("a1e2c3d4-5f6a-7b8c-9d0e-1f2a3b4c5d6e") },
                    { new Guid("23456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("b8c9d0e1-2f3a-4b5c-6d7e-8f9a0b1c2d3e") },
                    { new Guid("24567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("b8c9d0e1-2f3a-4b5c-6d7e-8f9a0b1c2d3e") },
                    { new Guid("32345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), 0, new Guid("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f") },
                    { new Guid("33456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("c9d0e1f2-3a4b-5c6d-7e8f-9a0b1c2d3e4f") },
                    { new Guid("34567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("a1e2c3d4-5f6a-7b8c-9d0e-1f2a3b4c5d6e") },
                    { new Guid("34567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("c9d0e1f2-3a4b-5c6d-7e8f-9a0b1c2d3e4f") },
                    { new Guid("42345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), 0, new Guid("d4e5f6a7-8b9c-0d1e-2f3a-4b5c6d7e8f9a") },
                    { new Guid("43456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("d0e1f2a3-4b5c-6d7e-8f9a-0b1c2d3e4f5a") },
                    { new Guid("44567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("d0e1f2a3-4b5c-6d7e-8f9a-0b1c2d3e4f5a") },
                    { new Guid("52345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), 0, new Guid("e5f6a7b8-9c0d-1e2f-3a4b-5c6d7e8f9a0b") },
                    { new Guid("53456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("e1f2a3b4-5c6d-7e8f-9a0b-1c2d3e4f5a6b") },
                    { new Guid("54567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("e1f2a3b4-5c6d-7e8f-9a0b-1c2d3e4f5a6b") },
                    { new Guid("62345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), 0, new Guid("f6a7b8c9-0d1e-2f3a-4b5c-6d7e8f9a0b1c") },
                    { new Guid("63456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("f2a3b4c5-6d7e-8f9a-0b1c-2d3e4f5a6b7c") },
                    { new Guid("64567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("f2a3b4c5-6d7e-8f9a-0b1c-2d3e4f5a6b7c") },
                    { new Guid("72345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), 0, new Guid("d0e1f2a3-4b5c-6d7e-8f9a-0b1c2d3e4f5a") },
                    { new Guid("73456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("a3b4c5d6-7e8f-9a0b-1c2d-3e4f5a6b7c8d") },
                    { new Guid("74567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("a3b4c5d6-7e8f-9a0b-1c2d-3e4f5a6b7c8d") },
                    { new Guid("82345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), 0, new Guid("e1f2a3b4-5c6d-7e8f-9a0b-1c2d3e4f5a6b") },
                    { new Guid("83456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("b4c5d6e7-8f9a-0b1c-2d3e-4f5a6b7c8d9e") },
                    { new Guid("84567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("b4c5d6e7-8f9a-0b1c-2d3e-4f5a6b7c8d9e") },
                    { new Guid("92345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), 0, new Guid("f2a3b4c5-6d7e-8f9a-0b1c-2d3e4f5a6b7c") },
                    { new Guid("93456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("c5d6e7f8-9a0b-1c2d-3e4f-5a6b7c8d9e0f") },
                    { new Guid("94567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("c5d6e7f8-9a0b-1c2d-3e4f-5a6b7c8d9e0f") },
                    { new Guid("a2345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), 0, new Guid("e7f8a9b0-1c2d-3e4f-5a6b-7c8d9e0f1a2b") },
                    { new Guid("a3456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("e7f8a9b0-1c2d-3e4f-5a6b-7c8d9e0f1a2b") },
                    { new Guid("a4567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("d6e7f8a9-0b1c-2d3e-4f5a-6b7c8d9e0f1a") },
                    { new Guid("b3456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("b2d3e4f5-6a7b-8c9d-0e1f-2a3b4c5d6e7f") },
                    { new Guid("b4567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("b2d3e4f5-6a7b-8c9d-0e1f-2a3b4c5d6e7f") },
                    { new Guid("b4567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("e7f8a9b0-1c2d-3e4f-5a6b-7c8d9e0f1a2b") },
                    { new Guid("c3456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f") },
                    { new Guid("c4567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f") },
                    { new Guid("d3456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("d4e5f6a7-8b9c-0d1e-2f3a-4b5c6d7e8f9a") },
                    { new Guid("d4567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("d4e5f6a7-8b9c-0d1e-2f3a-4b5c6d7e8f9a") },
                    { new Guid("e3456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("e5f6a7b8-9c0d-1e2f-3a4b-5c6d7e8f9a0b") },
                    { new Guid("e4567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("e5f6a7b8-9c0d-1e2f-3a4b-5c6d7e8f9a0b") },
                    { new Guid("f3456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), 0, new Guid("f6a7b8c9-0d1e-2f3a-4b5c-6d7e8f9a0b1c") },
                    { new Guid("f4567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), 0, new Guid("f6a7b8c9-0d1e-2f3a-4b5c-6d7e8f9a0b1c") }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FullName", "PasswordHash", "PasswordSalt", "PhoneNumber", "RoleID", "StaffStatus" },
                values: new object[,]
                {
                    { new Guid("b2dab1c3-6d48-4b23-8369-2d1c9c828f22"), "testUser@gmail.com", "Test User", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888777", new Guid("a1a2a3a4-b5b6-c7c8-d9d0-e1e2e3e4e5e6"), null },
                    { new Guid("c3dab1c3-6d48-4b23-8369-2d1c9c828f23"), "testAdmin@gmail.com", "Test Admin", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888777", new Guid("c1c2c3c4-d5d6-e7e8-f9f0-a1a2a3a4a5a6"), null },
                    { new Guid("d3dab1c3-6d48-4b23-8369-2d1c9c828f24"), "testStaff1@gmail.com", "Test Staff 1", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888777", new Guid("b1b2b3b4-c5c6-d7d8-e9e0-f1f2f3f4f5f6"), 0 },
                    { new Guid("e3dab1c3-6d48-4b23-8369-2d1c9c828f25"), "testReceptionist@gmail.com", "Test Receptionist", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888777", new Guid("d1d2d3d4-e5e6-f7f8-a9a0-b1b2b3b4b5b6"), null },
                    { new Guid("e9db2278-2c9a-40d9-82b8-f8af452a382f"), "testStaff2@gmail.com", "Test Staff 2", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888777", new Guid("b1b2b3b4-c5c6-d7d8-e9e0-f1f2f3f4f5f6"), 0 }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "VehicleId", "CustomerId", "NumberOfSeats", "VehicleBrand", "VehicleColor", "VehicleName" },
                values: new object[] { new Guid("12345678-90ab-cdef-1234-567890abcdef"), new Guid("b2dab1c3-6d48-4b23-8369-2d1c9c828f22"), 4, "Test", "Test", "Test" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PackageId",
                table: "Bookings",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VehicleId",
                table: "Bookings",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingStaffs_BookingId",
                table: "BookingStaffs",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingStaffs_StaffId",
                table: "BookingStaffs",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOfBookings_BookingId",
                table: "ServiceOfBookings",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOfBookings_ServiceId",
                table: "ServiceOfBookings",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_PackageID",
                table: "ServicePackages",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_ServiceId",
                table: "ServicePackages",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPackages_PackageId",
                table: "UserPackages",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CustomerId",
                table: "Vehicles",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingStaffs");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "ServiceOfBookings");

            migrationBuilder.DropTable(
                name: "ServicePackages");

            migrationBuilder.DropTable(
                name: "UserPackages");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
