using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_Coffe.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedDataToHashPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"Users\" WHERE \"Id\" != '99999999-9999-9999-9999-999999999999';");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "$2a$11$q2LzNjZBwt754ssEkedFN.dNj4FT7PWJrYsnRmMAgwFl4MWGECg9i");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"),
                column: "PasswordHash",
                value: "Admin@123");
        }
    }
}
