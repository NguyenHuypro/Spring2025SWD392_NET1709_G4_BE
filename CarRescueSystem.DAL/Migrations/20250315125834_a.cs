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
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RescueStations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    latitude = table.Column<double>(type: "double", nullable: false),
                    longitude = table.Column<double>(type: "double", nullable: false),
                    phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RescueStations", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    fullName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    passwordSalt = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role = table.Column<int>(type: "int", nullable: false),
                    staffStatus = table.Column<int>(type: "int", nullable: true),
                    rescueStationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_RescueStations_rescueStationId",
                        column: x => x.rescueStationId,
                        principalTable: "RescueStations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServicePackages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    serviceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    packageID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePackages", x => x.id);
                    table.ForeignKey(
                        name: "FK_ServicePackages_Packages_packageID",
                        column: x => x.packageID,
                        principalTable: "Packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicePackages_Services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    refreshTokenKey = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    isRevoked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    startTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    endTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    shift = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_Schedules_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    customerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    model = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    color = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brand = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numberOfSeats = table.Column<int>(type: "int", nullable: false),
                    licensePlate = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    packageId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    expirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Packages_packageId",
                        column: x => x.packageId,
                        principalTable: "Packages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Vehicles_Users_customerId",
                        column: x => x.customerId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    customerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    vehicleId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    status = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    evidence = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    location = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    latitude = table.Column<double>(type: "double", nullable: true),
                    longitude = table.Column<double>(type: "double", nullable: true),
                    totalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    bookingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    arrivalDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    completedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    licensePlate = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    packageId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    rescueStationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bookings_Packages_packageId",
                        column: x => x.packageId,
                        principalTable: "Packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bookings_RescueStations_rescueStationId",
                        column: x => x.rescueStationId,
                        principalTable: "RescueStations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_customerId",
                        column: x => x.customerId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Vehicles_vehicleId",
                        column: x => x.vehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BookingStaffs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    bookingId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    staffId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    assignedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    confirmStaff = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingStaffs", x => x.id);
                    table.ForeignKey(
                        name: "FK_BookingStaffs_Bookings_bookingId",
                        column: x => x.bookingId,
                        principalTable: "Bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingStaffs_Users_staffId",
                        column: x => x.staffId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServiceOfBookings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    bookingId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    serviceId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOfBookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_ServiceOfBookings_Bookings_bookingId",
                        column: x => x.bookingId,
                        principalTable: "Bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceOfBookings_Services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    userId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    bookingId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    packageId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    carId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transactions_Bookings_bookingId",
                        column: x => x.bookingId,
                        principalTable: "Bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Transactions_Packages_packageId",
                        column: x => x.packageId,
                        principalTable: "Packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Transactions_Vehicles_carId",
                        column: x => x.carId,
                        principalTable: "Vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "id", "name", "price" },
                values: new object[,]
                {
                    { new Guid("11111111-2222-3333-4444-555555555555"), "Gói Cơ Bản", 500000m },
                    { new Guid("66666666-7777-8888-9999-000000000000"), "Gói Toàn Diện", 1000000m },
                    { new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), "Gói Cao Cấp", 2000000m }
                });

            migrationBuilder.InsertData(
                table: "RescueStations",
                columns: new[] { "id", "address", "email", "latitude", "longitude", "name", "phone" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef1"), "86 Đinh Tiên Hoàng, Đa Kao, Quận 1, TP.HCM", "rescue.q1@carrescue.vn", 10.782222000000001, 106.69972199999999, "Trạm Cứu Hộ Quận 1", "02812345678" },
                    { new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef2"), "273 Nguyễn Thiện Thuật, Phường 1, Quận 3, TP.HCM", "rescue.q3@carrescue.vn", 10.770049, 106.682846, "Trạm Cứu Hộ Quận 3", "02812345679" },
                    { new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef3"), "161 Trần Hưng Đạo, Phường 10, Quận 5, TP.HCM", "rescue.q5@carrescue.vn", 10.754345000000001, 106.663983, "Trạm Cứu Hộ Quận 5", "02812345680" },
                    { new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef4"), "502 Nguyễn Văn Linh, Tân Phong, Quận 7, TP.HCM", "rescue.q7@carrescue.vn", 10.730745000000001, 106.72163999999999, "Trạm Cứu Hộ Quận 7", "02812345681" },
                    { new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef5"), "208 Xô Viết Nghệ Tĩnh, Phường 21, Bình Thạnh, TP.HCM", "rescue.bt@carrescue.vn", 10.803537, 106.713179, "Trạm Cứu Hộ Quận Bình Thạnh", "02812345682" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "id", "name", "price" },
                values: new object[,]
                {
                    { new Guid("a1e2c3d4-5f6a-7b8c-9d0e-1f2a3b4c5d6e"), "Phanh", 500000m },
                    { new Guid("a3b4c5d6-7e8f-9a0b-1c2d-3e4f5a6b7c8d"), "Hỏa hoạn hoặc nổ", 2000000m },
                    { new Guid("a7b8c9d0-1e2f-3a4b-5c6d-7e8f9a0b1c2d"), "Đổ nhiên liệu", 250000m },
                    { new Guid("b2d3e4f5-6a7b-8c9d-0e1f-2a3b4c5d6e7f"), "Vấn đề điện", 400000m },
                    { new Guid("b4c5d6e7-8f9a-0b1c-2d3e-4f5a6b7c8d9e"), "Xe rơi xuống vực", 1100000m },
                    { new Guid("b8c9d0e1-2f3a-4b5c-6d7e-8f9a0b1c2d3e"), "Đổ sai nhiên liệu", 350000m },
                    { new Guid("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f"), "Hệ thống lái", 450000m },
                    { new Guid("c5d6e7f8-9a0b-1c2d-3e4f-5a6b7c8d9e0f"), "Xe bị ngập nước", 1300000m },
                    { new Guid("c9d0e1f2-3a4b-5c6d-7e8f-9a0b1c2d3e4f"), "Bị khóa xe (quên chìa khóa)", 150000m },
                    { new Guid("d0e1f2a3-4b5c-6d7e-8f9a-0b1c2d3e4f5a"), "Va chạm", 1000000m },
                    { new Guid("d4e5f6a7-8b9c-0d1e-2f3a-4b5c6d7e8f9a"), "Vấn đề động cơ", 700000m },
                    { new Guid("d6e7f8a9-0b1c-2d3e-4f5a-6b7c8d9e0f1a"), "Thủy kích", 900000m },
                    { new Guid("e1f2a3b4-5c6d-7e8f-9a0b-1c2d3e4f5a6b"), "Va chạm nhẹ", 800000m },
                    { new Guid("e5f6a7b8-9c0d-1e2f-3a4b-5c6d7e8f9a0b"), "Vấn đề lốp xe", 300000m },
                    { new Guid("e7f8a9b0-1c2d-3e4f-5a6b-7c8d9e0f1a2b"), "Cứu hộ kéo xe", 500000m },
                    { new Guid("f2a3b4c5-6d7e-8f9a-0b1c-2d3e4f5a6b7c"), "Lật xe", 1200000m },
                    { new Guid("f6a7b8c9-0d1e-2f3a-4b5c-6d7e8f9a0b1c"), "Kích bình ắc quy", 200000m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "email", "fullName", "password", "passwordSalt", "phone", "rescueStationId", "role", "staffStatus" },
                values: new object[,]
                {
                    { new Guid("b2dab1c3-6d48-4b23-8369-2d1c9c828f22"), "testUser@gmail.com", "Test User", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888777", null, 0, null },
                    { new Guid("c3dab1c3-6d48-4b23-8369-2d1c9c828f23"), "testAdmin@gmail.com", "Test Admin", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888777", null, 3, null },
                    { new Guid("e1b2c3d4-f5a6-7890-1234-56789abcdef7"), "testManager@gmail.com", "Test Manager", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888777", null, 4, null },
                    { new Guid("e3dab1c3-6d48-4b23-8369-2d1c9c828f25"), "testReceptionist@gmail.com", "Test Receptionist", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888777", null, 2, null }
                });

            migrationBuilder.InsertData(
                table: "ServicePackages",
                columns: new[] { "id", "packageID", "serviceId" },
                values: new object[,]
                {
                    { new Guid("12345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), new Guid("a1e2c3d4-5f6a-7b8c-9d0e-1f2a3b4c5d6e") },
                    { new Guid("13456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("a7b8c9d0-1e2f-3a4b-5c6d-7e8f9a0b1c2d") },
                    { new Guid("14567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("a7b8c9d0-1e2f-3a4b-5c6d-7e8f9a0b1c2d") },
                    { new Guid("22345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), new Guid("b2d3e4f5-6a7b-8c9d-0e1f-2a3b4c5d6e7f") },
                    { new Guid("23456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("a1e2c3d4-5f6a-7b8c-9d0e-1f2a3b4c5d6e") },
                    { new Guid("23456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("b8c9d0e1-2f3a-4b5c-6d7e-8f9a0b1c2d3e") },
                    { new Guid("24567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("b8c9d0e1-2f3a-4b5c-6d7e-8f9a0b1c2d3e") },
                    { new Guid("32345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), new Guid("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f") },
                    { new Guid("33456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("c9d0e1f2-3a4b-5c6d-7e8f-9a0b1c2d3e4f") },
                    { new Guid("34567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("a1e2c3d4-5f6a-7b8c-9d0e-1f2a3b4c5d6e") },
                    { new Guid("34567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("c9d0e1f2-3a4b-5c6d-7e8f-9a0b1c2d3e4f") },
                    { new Guid("42345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), new Guid("d4e5f6a7-8b9c-0d1e-2f3a-4b5c6d7e8f9a") },
                    { new Guid("43456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("d0e1f2a3-4b5c-6d7e-8f9a-0b1c2d3e4f5a") },
                    { new Guid("44567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("d0e1f2a3-4b5c-6d7e-8f9a-0b1c2d3e4f5a") },
                    { new Guid("52345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), new Guid("e5f6a7b8-9c0d-1e2f-3a4b-5c6d7e8f9a0b") },
                    { new Guid("53456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("e1f2a3b4-5c6d-7e8f-9a0b-1c2d3e4f5a6b") },
                    { new Guid("54567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("e1f2a3b4-5c6d-7e8f-9a0b-1c2d3e4f5a6b") },
                    { new Guid("62345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), new Guid("f6a7b8c9-0d1e-2f3a-4b5c-6d7e8f9a0b1c") },
                    { new Guid("63456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("f2a3b4c5-6d7e-8f9a-0b1c-2d3e4f5a6b7c") },
                    { new Guid("64567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("f2a3b4c5-6d7e-8f9a-0b1c-2d3e4f5a6b7c") },
                    { new Guid("72345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), new Guid("d0e1f2a3-4b5c-6d7e-8f9a-0b1c2d3e4f5a") },
                    { new Guid("73456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("a3b4c5d6-7e8f-9a0b-1c2d-3e4f5a6b7c8d") },
                    { new Guid("74567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("a3b4c5d6-7e8f-9a0b-1c2d-3e4f5a6b7c8d") },
                    { new Guid("82345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), new Guid("e1f2a3b4-5c6d-7e8f-9a0b-1c2d3e4f5a6b") },
                    { new Guid("83456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("b4c5d6e7-8f9a-0b1c-2d3e-4f5a6b7c8d9e") },
                    { new Guid("84567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("b4c5d6e7-8f9a-0b1c-2d3e-4f5a6b7c8d9e") },
                    { new Guid("92345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), new Guid("f2a3b4c5-6d7e-8f9a-0b1c-2d3e4f5a6b7c") },
                    { new Guid("93456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("c5d6e7f8-9a0b-1c2d-3e4f-5a6b7c8d9e0f") },
                    { new Guid("94567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("c5d6e7f8-9a0b-1c2d-3e4f-5a6b7c8d9e0f") },
                    { new Guid("a2345678-90ab-cdef-1234-567890abcdef"), new Guid("11111111-2222-3333-4444-555555555555"), new Guid("e7f8a9b0-1c2d-3e4f-5a6b-7c8d9e0f1a2b") },
                    { new Guid("a3456789-0abc-def1-2345-67890abcdefb"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("e7f8a9b0-1c2d-3e4f-5a6b-7c8d9e0f1a2b") },
                    { new Guid("a4567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("d6e7f8a9-0b1c-2d3e-4f5a-6b7c8d9e0f1a") },
                    { new Guid("b3456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("b2d3e4f5-6a7b-8c9d-0e1f-2a3b4c5d6e7f") },
                    { new Guid("b4567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("b2d3e4f5-6a7b-8c9d-0e1f-2a3b4c5d6e7f") },
                    { new Guid("b4567890-abcd-ef12-3456-7890abcdefac"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("e7f8a9b0-1c2d-3e4f-5a6b-7c8d9e0f1a2b") },
                    { new Guid("c3456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f") },
                    { new Guid("c4567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("c3d4e5f6-7a8b-9c0d-1e2f-3a4b5c6d7e8f") },
                    { new Guid("d3456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("d4e5f6a7-8b9c-0d1e-2f3a-4b5c6d7e8f9a") },
                    { new Guid("d4567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("d4e5f6a7-8b9c-0d1e-2f3a-4b5c6d7e8f9a") },
                    { new Guid("e3456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("e5f6a7b8-9c0d-1e2f-3a4b-5c6d7e8f9a0b") },
                    { new Guid("e4567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("e5f6a7b8-9c0d-1e2f-3a4b-5c6d7e8f9a0b") },
                    { new Guid("f3456789-0abc-def1-2345-67890abcdefa"), new Guid("66666666-7777-8888-9999-000000000000"), new Guid("f6a7b8c9-0d1e-2f3a-4b5c-6d7e8f9a0b1c") },
                    { new Guid("f4567890-abcd-ef12-3456-7890abcdefab"), new Guid("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"), new Guid("f6a7b8c9-0d1e-2f3a-4b5c-6d7e8f9a0b1c") }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "email", "fullName", "password", "passwordSalt", "phone", "rescueStationId", "role", "staffStatus" },
                values: new object[,]
                {
                    { new Guid("d3dab1c3-6d48-4b23-8369-2d1c9c828f24"), "testStaff1@gmail.com", "Test Staff 1", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888777", new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef1"), 1, 0 },
                    { new Guid("e9db2278-2c9a-40d9-82b8-f8af452a382f"), "testStaff2@gmail.com", "Test Staff 2", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888777", new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef1"), 1, 0 },
                    { new Guid("f1dab1c3-6d48-4b23-8369-2d1c9c828f26"), "testStaff3@gmail.com", "Test Staff 3", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888773", new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef2"), 1, 0 },
                    { new Guid("f2dab1c3-6d48-4b23-8369-2d1c9c828f27"), "testStaff4@gmail.com", "Test Staff 4", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888774", new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef2"), 1, 0 },
                    { new Guid("f3dab1c3-6d48-4b23-8369-2d1c9c828f28"), "testStaff5@gmail.com", "Test Staff 5", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888775", new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef3"), 1, 0 },
                    { new Guid("f4dab1c3-6d48-4b23-8369-2d1c9c828f29"), "testStaff6@gmail.com", "Test Staff 6", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888776", new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef3"), 1, 0 },
                    { new Guid("f5dab1c3-6d48-4b23-8369-2d1c9c828f30"), "testStaff7@gmail.com", "Test Staff 7", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888778", new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef4"), 1, 0 },
                    { new Guid("f6dab1c3-6d48-4b23-8369-2d1c9c828f31"), "testStaff8@gmail.com", "Test Staff 8", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888779", new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef4"), 1, 0 },
                    { new Guid("f7dab1c3-6d48-4b23-8369-2d1c9c828f32"), "testStaff9@gmail.com", "Test Staff 9", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888780", new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef5"), 1, 0 },
                    { new Guid("f8dab1c3-6d48-4b23-8369-2d1c9c828f33"), "testStaff10@gmail.com", "Test Staff 10", "$2a$11$rTz6DZiEeBqhVrzF25CgTOBPf41jpn2Tg/nnIqnX8KS6uIerB/1dm", "", "0999888781", new Guid("a1b2c3d4-e5f6-7890-1234-56789abcdef5"), 1, 0 }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "id", "brand", "color", "customerId", "expirationDate", "licensePlate", "model", "numberOfSeats", "packageId" },
                values: new object[] { new Guid("12345678-90ab-cdef-1234-567890abcdef"), "Test", "Test", new Guid("b2dab1c3-6d48-4b23-8369-2d1c9c828f22"), null, "30G-49344", "Test", 4, null });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_customerId",
                table: "Bookings",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_packageId",
                table: "Bookings",
                column: "packageId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_rescueStationId",
                table: "Bookings",
                column: "rescueStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_vehicleId",
                table: "Bookings",
                column: "vehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingStaffs_bookingId",
                table: "BookingStaffs",
                column: "bookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingStaffs_staffId",
                table: "BookingStaffs",
                column: "staffId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_userId",
                table: "RefreshTokens",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_userId",
                table: "Schedules",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOfBookings_bookingId",
                table: "ServiceOfBookings",
                column: "bookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOfBookings_serviceId",
                table: "ServiceOfBookings",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_packageID",
                table: "ServicePackages",
                column: "packageID");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePackages_serviceId",
                table: "ServicePackages",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_bookingId",
                table: "Transactions",
                column: "bookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_carId",
                table: "Transactions",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_packageId",
                table: "Transactions",
                column: "packageId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_rescueStationId",
                table: "Users",
                column: "rescueStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_customerId",
                table: "Vehicles",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_packageId",
                table: "Vehicles",
                column: "packageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingStaffs");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "ServiceOfBookings");

            migrationBuilder.DropTable(
                name: "ServicePackages");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RescueStations");
        }
    }
}
