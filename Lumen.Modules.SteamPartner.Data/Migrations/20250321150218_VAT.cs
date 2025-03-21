using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations
{
    /// <inheritdoc />
    public partial class VAT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "USDVATTaxes",
                schema: "steampartner",
                table: "PackagesSales",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "USDVATTaxes",
                schema: "steampartner",
                table: "PackagesSales");
        }
    }
}
