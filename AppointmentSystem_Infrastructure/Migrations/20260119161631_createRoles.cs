using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppointmentSystem_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-4789-9012-34567890abcd"), null, "Admin", "ADMIN" },
                    { new Guid("b2c3d4e5-f678-4890-1234-567890abcdef"), null, "Doctor", "DOCTOR" },
                    { new Guid("c3d4e5f6-7890-4901-2345-67890abcdef1"), null, "Patient", "PATIENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-4789-9012-34567890abcd"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f678-4890-1234-567890abcdef"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-7890-4901-2345-67890abcdef1"));
        }
    }
}
