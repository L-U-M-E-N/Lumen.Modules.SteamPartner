using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations;

/// <inheritdoc />
public partial class _20261127104430_SteamEventsPt2 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_SteamEventDiscount",
            schema: "steampartner",
            table: "SteamEventDiscount");

        migrationBuilder.DropColumn(
            name: "Id",
            schema: "steampartner",
            table: "SteamEventDiscount");

        migrationBuilder.RenameColumn(
            name: "DiscountId",
            schema: "steampartner",
            table: "SteamEvents",
            newName: "CombinedDiscountId");

        migrationBuilder.AddColumn<string>(
            name: "Name",
            schema: "steampartner",
            table: "SteamEvents",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<long>(
            name: "DiscountId",
            schema: "steampartner",
            table: "SteamEventDiscount",
            type: "bigint",
            nullable: false,
            defaultValue: 0L)
            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        migrationBuilder.AddPrimaryKey(
            name: "PK_SteamEventDiscount",
            schema: "steampartner",
            table: "SteamEventDiscount",
            column: "DiscountId");

        migrationBuilder.CreateIndex(
            name: "IX_SteamEventDiscount_SteamEventId",
            schema: "steampartner",
            table: "SteamEventDiscount",
            column: "SteamEventId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_SteamEventDiscount",
            schema: "steampartner",
            table: "SteamEventDiscount");

        migrationBuilder.DropIndex(
            name: "IX_SteamEventDiscount_SteamEventId",
            schema: "steampartner",
            table: "SteamEventDiscount");

        migrationBuilder.DropColumn(
            name: "Name",
            schema: "steampartner",
            table: "SteamEvents");

        migrationBuilder.DropColumn(
            name: "DiscountId",
            schema: "steampartner",
            table: "SteamEventDiscount");

        migrationBuilder.RenameColumn(
            name: "CombinedDiscountId",
            schema: "steampartner",
            table: "SteamEvents",
            newName: "DiscountId");

        migrationBuilder.AddColumn<int>(
            name: "Id",
            schema: "steampartner",
            table: "SteamEventDiscount",
            type: "integer",
            nullable: false,
            defaultValue: 0)
            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        migrationBuilder.AddPrimaryKey(
            name: "PK_SteamEventDiscount",
            schema: "steampartner",
            table: "SteamEventDiscount",
            columns: new[] { "SteamEventId", "Id" });
    }
}
