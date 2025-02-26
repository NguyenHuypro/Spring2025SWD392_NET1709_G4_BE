using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRescueSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MigrationTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ServicePackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("12345678-90ab-cdef-1234-567890abcdef"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("13456789-0abc-def1-2345-67890abcdefa"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("14567890-abcd-ef12-3456-7890abcdefab"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("22345678-90ab-cdef-1234-567890abcdef"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("23456789-0abc-def1-2345-67890abcdefa"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("23456789-0abc-def1-2345-67890abcdefb"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("24567890-abcd-ef12-3456-7890abcdefab"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("32345678-90ab-cdef-1234-567890abcdef"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("33456789-0abc-def1-2345-67890abcdefb"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("34567890-abcd-ef12-3456-7890abcdefab"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("34567890-abcd-ef12-3456-7890abcdefac"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("42345678-90ab-cdef-1234-567890abcdef"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("43456789-0abc-def1-2345-67890abcdefb"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("44567890-abcd-ef12-3456-7890abcdefac"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("52345678-90ab-cdef-1234-567890abcdef"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("53456789-0abc-def1-2345-67890abcdefb"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("54567890-abcd-ef12-3456-7890abcdefac"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("62345678-90ab-cdef-1234-567890abcdef"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("63456789-0abc-def1-2345-67890abcdefb"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("64567890-abcd-ef12-3456-7890abcdefac"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("72345678-90ab-cdef-1234-567890abcdef"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("73456789-0abc-def1-2345-67890abcdefb"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("74567890-abcd-ef12-3456-7890abcdefac"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("82345678-90ab-cdef-1234-567890abcdef"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("83456789-0abc-def1-2345-67890abcdefb"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("84567890-abcd-ef12-3456-7890abcdefac"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("92345678-90ab-cdef-1234-567890abcdef"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("93456789-0abc-def1-2345-67890abcdefb"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("94567890-abcd-ef12-3456-7890abcdefac"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("a2345678-90ab-cdef-1234-567890abcdef"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("a3456789-0abc-def1-2345-67890abcdefb"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("a4567890-abcd-ef12-3456-7890abcdefac"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("b3456789-0abc-def1-2345-67890abcdefa"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("b4567890-abcd-ef12-3456-7890abcdefab"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("b4567890-abcd-ef12-3456-7890abcdefac"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("c3456789-0abc-def1-2345-67890abcdefa"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("c4567890-abcd-ef12-3456-7890abcdefab"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("d3456789-0abc-def1-2345-67890abcdefa"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("d4567890-abcd-ef12-3456-7890abcdefab"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("e3456789-0abc-def1-2345-67890abcdefa"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("e4567890-abcd-ef12-3456-7890abcdefab"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("f3456789-0abc-def1-2345-67890abcdefa"),
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ServicePackages",
                keyColumn: "ServicePackageId",
                keyValue: new Guid("f4567890-abcd-ef12-3456-7890abcdefab"),
                column: "Quantity",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserPackages_PackageId",
                table: "UserPackages",
                column: "PackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPackages");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ServicePackages");
        }
    }
}
