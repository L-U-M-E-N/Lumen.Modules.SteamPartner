﻿// <auto-generated />
using System;
using Lumen.Modules.SteamPartner.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lumen.Modules.SteamPartner.Data.Migrations
{
    [DbContext(typeof(SteamPartnerContext))]
    [Migration("20250313165316_InitialStructure")]
    partial class InitialStructure
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("steampartner")
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Dysnomia.Common.SteamWebAPI.Models.PackageSales", b =>
                {
                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("BundleId")
                        .HasColumnType("integer");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<string>("Platform")
                        .HasColumnType("text");

                    b.Property<string>("CountryCode")
                        .HasColumnType("text");

                    b.Property<decimal>("SalePrice")
                        .HasColumnType("numeric");

                    b.Property<decimal>("BasePrice")
                        .HasColumnType("numeric");

                    b.Property<string>("BundleName")
                        .HasColumnType("text");

                    b.Property<int>("ChargebackAndReturns")
                        .HasColumnType("integer");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<string>("Currency")
                        .HasColumnType("text");

                    b.Property<string>("Game")
                        .HasColumnType("text");

                    b.Property<int>("GrossUnits")
                        .HasColumnType("integer");

                    b.Property<int>("NetUnits")
                        .HasColumnType("integer");

                    b.Property<string>("ProductName")
                        .HasColumnType("text");

                    b.Property<string>("Region")
                        .HasColumnType("text");

                    b.Property<string>("Tag")
                        .HasColumnType("text");

                    b.Property<decimal>("USDChargebackAndReturns")
                        .HasColumnType("numeric");

                    b.Property<decimal>("USDGrossSteamSales")
                        .HasColumnType("numeric");

                    b.Property<decimal>("USDNetSteamSales")
                        .HasColumnType("numeric");

                    b.HasKey("Date", "BundleId", "ProductId", "Type", "Platform", "CountryCode", "SalePrice");

                    b.ToTable("PackagesSales", "steampartner");
                });

            modelBuilder.Entity("Dysnomia.Common.SteamWebAPI.Models.WishlistActions", b =>
                {
                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Game")
                        .HasColumnType("text");

                    b.Property<int>("Adds")
                        .HasColumnType("integer");

                    b.Property<int>("Deletes")
                        .HasColumnType("integer");

                    b.Property<int>("Gifts")
                        .HasColumnType("integer");

                    b.Property<int>("PurchasesAndActivations")
                        .HasColumnType("integer");

                    b.HasKey("Date", "Game");

                    b.ToTable("WishlistActions", "steampartner");
                });

            modelBuilder.Entity("Lumen.Modules.SteamPartner.Common.Models.SteamFollower", b =>
                {
                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("GameName")
                        .HasColumnType("text");

                    b.Property<int>("FollowerAmount")
                        .HasColumnType("integer");

                    b.HasKey("Date", "GameName");

                    b.ToTable("SteamFollowers", "steampartner");
                });

            modelBuilder.Entity("Lumen.Modules.SteamPartner.Common.Models.SteamGames", b =>
                {
                    b.Property<int>("AppId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AppId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("PackageId")
                        .HasColumnType("integer");

                    b.Property<int>("SteamRunAs")
                        .HasColumnType("integer");

                    b.HasKey("AppId");

                    b.ToTable("SteamGames", "steampartner");
                });
#pragma warning restore 612, 618
        }
    }
}
