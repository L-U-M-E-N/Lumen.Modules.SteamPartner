using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewSteamPackageSalesAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SteamPackageSales",
                schema: "steampartner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageId = table.Column<long>(type: "bigint", nullable: true),
                    BundleId = table.Column<long>(type: "bigint", nullable: true),
                    AppId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    PackageSaleType = table.Column<string>(type: "text", nullable: true),
                    Platform = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    CountryName = table.Column<string>(type: "text", nullable: false),
                    CountryRegion = table.Column<string>(type: "text", nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric", nullable: true),
                    SalePrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    GrossUnitsActivated = table.Column<long>(type: "bigint", nullable: true),
                    GrossUnitsSold = table.Column<long>(type: "bigint", nullable: true),
                    GrossUnitsReturned = table.Column<long>(type: "bigint", nullable: true),
                    NetUnitsSold = table.Column<decimal>(type: "numeric", nullable: true),
                    GrossSalesUsd = table.Column<decimal>(type: "numeric", nullable: true),
                    GrossReturnsUsd = table.Column<decimal>(type: "numeric", nullable: true),
                    NetTaxUsd = table.Column<decimal>(type: "numeric", nullable: true),
                    NetSalesUsd = table.Column<decimal>(type: "numeric", nullable: true),
                    AdditionalRevenueShareTier = table.Column<int>(type: "integer", nullable: true),
                    CombinedDiscountId = table.Column<int>(type: "integer", nullable: true),
                    AvgSalePriceUsd = table.Column<int>(type: "integer", nullable: true),
                    KeyRequestId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteamPackageSales", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SteamPackageSales",
                schema: "steampartner");
        }
    }
}
