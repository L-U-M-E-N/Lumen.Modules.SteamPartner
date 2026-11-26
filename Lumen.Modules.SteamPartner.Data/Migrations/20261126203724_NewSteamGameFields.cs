using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewSteamGameFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "PublicStorePage",
                schema: "steampartner",
                table: "SteamGames",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReleaseDate",
                schema: "steampartner",
                table: "SteamGames",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicStorePage",
                schema: "steampartner",
                table: "SteamGames");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                schema: "steampartner",
                table: "SteamGames");
        }
    }
}
