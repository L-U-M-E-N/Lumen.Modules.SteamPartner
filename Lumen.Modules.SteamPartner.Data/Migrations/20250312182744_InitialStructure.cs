using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "steampartner");

            migrationBuilder.CreateTable(
                name: "PackagesSales",
                schema: "steampartner",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    BundleId = table.Column<int>(type: "integer", nullable: false),
                    BundleName = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Game = table.Column<string>(type: "text", nullable: true),
                    Platform = table.Column<string>(type: "text", nullable: true),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    GrossUnits = table.Column<int>(type: "integer", nullable: false),
                    ChargebackAndReturns = table.Column<int>(type: "integer", nullable: false),
                    NetUnits = table.Column<int>(type: "integer", nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    SalePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    USDGrossSteamSales = table.Column<decimal>(type: "numeric", nullable: false),
                    USDChargebackAndReturns = table.Column<decimal>(type: "numeric", nullable: false),
                    USDNetSteamSales = table.Column<decimal>(type: "numeric", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SteamFollowers",
                schema: "steampartner",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    GameName = table.Column<string>(type: "text", nullable: false),
                    FollowerAmount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteamFollowers", x => new { x.Date, x.GameName });
                });

            migrationBuilder.CreateTable(
                name: "SteamGames",
                schema: "steampartner",
                columns: table => new
                {
                    AppId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PackageId = table.Column<int>(type: "integer", nullable: true),
                    SteamRunAs = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteamGames", x => x.AppId);
                });

            migrationBuilder.CreateTable(
                name: "WishlistActions",
                schema: "steampartner",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Game = table.Column<string>(type: "text", nullable: false),
                    Adds = table.Column<int>(type: "integer", nullable: false),
                    Deletes = table.Column<int>(type: "integer", nullable: false),
                    PurchasesAndActivations = table.Column<int>(type: "integer", nullable: false),
                    Gifts = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistActions", x => new { x.Date, x.Game });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackagesSales",
                schema: "steampartner");

            migrationBuilder.DropTable(
                name: "SteamFollowers",
                schema: "steampartner");

            migrationBuilder.DropTable(
                name: "SteamGames",
                schema: "steampartner");

            migrationBuilder.DropTable(
                name: "WishlistActions",
                schema: "steampartner");
        }
    }
}
