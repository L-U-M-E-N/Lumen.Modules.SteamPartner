using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewWishlistsAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SteamWishlists",
                schema: "steampartner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: true),
                    CountryName = table.Column<string>(type: "text", nullable: true),
                    CountryRegion = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "text", nullable: true),
                    WishlistsAdditions = table.Column<long>(type: "bigint", nullable: false),
                    WishlistDeletions = table.Column<long>(type: "bigint", nullable: false),
                    WishlistsPurchases = table.Column<long>(type: "bigint", nullable: false),
                    WishlistsGifts = table.Column<long>(type: "bigint", nullable: false),
                    WishlistsAdditionsWindows = table.Column<long>(type: "bigint", nullable: false),
                    WishlistsAdditionsMac = table.Column<long>(type: "bigint", nullable: false),
                    WishlistsAdditionsLinux = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteamWishlists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SteamWishlists_AppId_Date_CountryCode_Language",
                schema: "steampartner",
                table: "SteamWishlists",
                columns: new[] { "AppId", "Date", "CountryCode", "Language" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SteamWishlists",
                schema: "steampartner");
        }
    }
}
