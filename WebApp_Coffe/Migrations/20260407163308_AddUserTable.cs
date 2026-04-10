using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_Coffe.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 289, DateTimeKind.Utc).AddTicks(8339));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 289, DateTimeKind.Utc).AddTicks(8734));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 289, DateTimeKind.Utc).AddTicks(8737));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 289, DateTimeKind.Utc).AddTicks(8739));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 289, DateTimeKind.Utc).AddTicks(8741));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 294, DateTimeKind.Utc).AddTicks(8811));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 294, DateTimeKind.Utc).AddTicks(8401));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 294, DateTimeKind.Utc).AddTicks(8790));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 294, DateTimeKind.Utc).AddTicks(8793));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 294, DateTimeKind.Utc).AddTicks(8795));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 294, DateTimeKind.Utc).AddTicks(8798));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 294, DateTimeKind.Utc).AddTicks(8800));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa7"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 294, DateTimeKind.Utc).AddTicks(8803));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa8"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 294, DateTimeKind.Utc).AddTicks(8805));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa9"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 16, 33, 7, 294, DateTimeKind.Utc).AddTicks(8807));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "Role" },
                values: new object[] { new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 4, 7, 16, 33, 7, 299, DateTimeKind.Utc).AddTicks(8051), "admin@coffeeshop.com", "System Admin", true, "Admin@123", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 488, DateTimeKind.Utc).AddTicks(9163));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 488, DateTimeKind.Utc).AddTicks(9545));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 488, DateTimeKind.Utc).AddTicks(9547));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 488, DateTimeKind.Utc).AddTicks(9549));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 488, DateTimeKind.Utc).AddTicks(9551));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8678));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8281));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8657));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8660));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8663));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8665));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8668));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa7"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8670));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa8"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8673));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa9"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8675));
        }
    }
}
