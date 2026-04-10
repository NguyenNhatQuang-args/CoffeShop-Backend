using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApp_Coffe.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Thumbnail = table.Column<string>(type: "text", nullable: true),
                    AuthorName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    OpenTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    CloseTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    GoogleMapUrl = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTags_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    SizeName = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Temperature = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "ImageUrl", "IsActive", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 4, 7, 15, 57, 39, 488, DateTimeKind.Utc).AddTicks(9163), "Premium roasted coffee", null, true, "Coffee", "coffee" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 4, 7, 15, 57, 39, 488, DateTimeKind.Utc).AddTicks(9545), "Freshly brewed tea", null, true, "Tea", "tea" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 4, 7, 15, 57, 39, 488, DateTimeKind.Utc).AddTicks(9547), "Fruit smoothies", null, true, "Smoothies", "smoothies" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 4, 7, 15, 57, 39, 488, DateTimeKind.Utc).AddTicks(9549), "Delicious cakes", null, true, "Cakes", "cakes" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 4, 7, 15, 57, 39, 488, DateTimeKind.Utc).AddTicks(9551), "Quick bites", null, true, "Snacks", "snacks" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsFeatured", "Name", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8678), null, null, true, false, "Strawberry Smoothie", "strawberry-smoothie", null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8281), null, null, true, false, "Espresso", "espresso", null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8657), null, null, true, false, "Americano", "americano", null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8660), null, null, true, false, "Latte", "latte", null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8663), null, null, true, false, "Cappuccino", "cappuccino", null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8665), null, null, true, false, "Mocha", "mocha", null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8668), null, null, true, false, "Green Tea", "green-tea", null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa7"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8670), null, null, true, false, "Black Tea", "black-tea", null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa8"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8673), null, null, true, false, "Peach Tea", "peach-tea", null },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa9"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 4, 7, 15, 57, 39, 493, DateTimeKind.Utc).AddTicks(8675), null, null, true, false, "Mango Smoothie", "mango-smoothie", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_Slug",
                table: "BlogPosts",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_ProductId",
                table: "ProductTags",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
