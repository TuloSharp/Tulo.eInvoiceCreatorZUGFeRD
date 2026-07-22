using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tulo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntitites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceHeaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvoiceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoicePositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LineStatusReasonCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UnitCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UnitPriceNet = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TaxPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    TaxCategory = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SellerAssignedId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GlobalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GlobalIdSchemeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OriginCountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    UnitCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UnitPriceNet = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PriceBasisQuantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    TaxPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    TaxCategory = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_Name",
                table: "Sellers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_VatId",
                table: "Sellers",
                column: "VatId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceHeaders_CustomerId",
                table: "InvoiceHeaders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceHeaders_InvoiceDate",
                table: "InvoiceHeaders",
                column: "InvoiceDate");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceHeaders_InvoiceNumber",
                table: "InvoiceHeaders",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePositions_InvoiceId",
                table: "InvoicePositions",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePositions_ProductId",
                table: "InvoicePositions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Description",
                table: "Products",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SellerAssignedId",
                table: "Products",
                column: "SellerAssignedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceHeaders");

            migrationBuilder.DropTable(
                name: "InvoicePositions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Sellers_Name",
                table: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_Sellers_VatId",
                table: "Sellers");
        }
    }
}
