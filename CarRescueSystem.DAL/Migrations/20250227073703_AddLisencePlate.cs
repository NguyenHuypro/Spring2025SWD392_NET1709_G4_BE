using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRescueSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddLisencePlate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LicensePlate",
                table: "Vehicles",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Vehicles",
                keyColumn: "VehicleId",
                keyValue: new Guid("12345678-90ab-cdef-1234-567890abcdef"),
                column: "LicensePlate",
                value: "30G-49344");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicensePlate",
                table: "Vehicles");
        }
    }
}
