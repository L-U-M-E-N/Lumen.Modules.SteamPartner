using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewSteamGameParameter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "QueryFollowers",
                schema: "steampartner",
                table: "SteamGames",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QueryFollowers",
                schema: "steampartner",
                table: "SteamGames");
        }
    }
}
