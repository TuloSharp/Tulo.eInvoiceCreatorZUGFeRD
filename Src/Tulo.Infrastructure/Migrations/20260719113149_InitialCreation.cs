using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tulo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    PartyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdSchemeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VatId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaxRegistrationFcId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FiscalId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LegalOrganizationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LeitwegId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LeitwegIdSchemeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GlobalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GlobalIdSchemeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GeneralEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPersonName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Zip = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    PartyId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdSchemeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VatId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaxRegistrationFcId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FiscalId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LegalOrganizationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LeitwegId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LeitwegIdSchemeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GlobalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GlobalIdSchemeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GeneralEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPersonName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Name",
                table: "Customers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PartyId",
                table: "Customers",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_VatId",
                table: "Customers",
                column: "VatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Sellers");
        }
    }
}
