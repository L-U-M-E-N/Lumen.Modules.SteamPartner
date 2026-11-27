using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations;

/// <inheritdoc />
public partial class _20261127170838_RemoveWrongUniqueIndex : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_SteamEventDiscount_AppId",
            schema: "steampartner",
            table: "SteamEventDiscount");

        migrationBuilder.CreateIndex(
            name: "IX_SteamEventDiscount_AppId",
            schema: "steampartner",
            table: "SteamEventDiscount",
            column: "AppId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_SteamEventDiscount_AppId",
            schema: "steampartner",
            table: "SteamEventDiscount");

        migrationBuilder.CreateIndex(
            name: "IX_SteamEventDiscount_AppId",
            schema: "steampartner",
            table: "SteamEventDiscount",
            column: "AppId",
            unique: true);
    }
}
