using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations;

/// <inheritdoc />
public partial class _20261127102800_SteamEvents : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SteamEvents",
            schema: "steampartner",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                DiscountId = table.Column<long>(type: "bigint", nullable: true),
                EventKey = table.Column<string>(type: "text", nullable: true),
                Start = table.Column<DateOnly>(type: "date", nullable: false),
                End = table.Column<DateOnly>(type: "date", nullable: false),
                Description = table.Column<string>(type: "text", nullable: true),
                Group = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SteamEvents", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SteamEventDiscount",
            schema: "steampartner",
            columns: table => new
            {
                SteamEventId = table.Column<Guid>(type: "uuid", nullable: false),
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                AppId = table.Column<long>(type: "bigint", nullable: false),
                DiscountPercentage = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SteamEventDiscount", x => new { x.SteamEventId, x.Id });
                table.ForeignKey(
                    name: "FK_SteamEventDiscount_SteamEvents_SteamEventId",
                    column: x => x.SteamEventId,
                    principalSchema: "steampartner",
                    principalTable: "SteamEvents",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SteamEventDiscount_AppId",
            schema: "steampartner",
            table: "SteamEventDiscount",
            column: "AppId",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SteamEventDiscount",
            schema: "steampartner");

        migrationBuilder.DropTable(
            name: "SteamEvents",
            schema: "steampartner");
    }
}
