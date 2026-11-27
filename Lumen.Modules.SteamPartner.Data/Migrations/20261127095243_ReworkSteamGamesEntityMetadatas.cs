using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations {
    /// <inheritdoc />
    public partial class ReworkSteamGamesEntityMetadatas : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "SteamRunAs",
                schema: "steampartner",
                table: "SteamGames");

            migrationBuilder.DropColumn(
                name: "PackageId",
                schema: "steampartner",
                table: "SteamGames");

            migrationBuilder.AddColumn<int[]>(
                name: "PackageId",
                schema: "steampartner",
                table: "SteamGames",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterColumn<int>(
                name: "PackageId",
                schema: "steampartner",
                table: "SteamGames",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int[]),
                oldType: "integer[]");

            migrationBuilder.AddColumn<int>(
                name: "SteamRunAs",
                schema: "steampartner",
                table: "SteamGames",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
