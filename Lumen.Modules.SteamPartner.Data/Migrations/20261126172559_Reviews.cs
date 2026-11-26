using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations
{
    /// <inheritdoc />
    public partial class Reviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SteamReviews",
                schema: "steampartner",
                columns: table => new
                {
                    RecommendationId = table.Column<string>(type: "text", nullable: false),
                    AppId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AuthorSteamId = table.Column<string>(type: "text", nullable: false),
                    ReviewCreation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReviewUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Positive = table.Column<bool>(type: "boolean", nullable: false),
                    SteamPurchase = table.Column<bool>(type: "boolean", nullable: false),
                    RecievedForFree = table.Column<bool>(type: "boolean", nullable: false),
                    Refunded = table.Column<bool>(type: "boolean", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteamReviews", x => x.RecommendationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SteamReviews_AppId",
                schema: "steampartner",
                table: "SteamReviews",
                column: "AppId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SteamReviews",
                schema: "steampartner");
        }
    }
}
