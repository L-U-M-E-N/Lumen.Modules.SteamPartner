using Dysnomia.Common.SteamWebAPI.Models;

using Lumen.Modules.SteamPartner.Common.Models;

using Microsoft.EntityFrameworkCore;

namespace Lumen.Modules.SteamPartner.Data {
    public class SteamPartnerContext : DbContext {
        public const string SCHEMA_NAME = "steampartner";

        public SteamPartnerContext(DbContextOptions<SteamPartnerContext> options) : base(options) {
        }

        public DbSet<SteamFollower> SteamFollowers { get; set; } = null!;
        public DbSet<SteamGames> SteamGames { get; set; } = null!;
        public DbSet<PackageSales> PackagesSales { get; set; } = null!;
        public DbSet<WishlistActions> WishlistActions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema(SCHEMA_NAME);

            var steamFollowersModelBuilder = modelBuilder.Entity<SteamFollower>();
            steamFollowersModelBuilder.Property(x => x.Date)
                .HasColumnType("date");

            steamFollowersModelBuilder.HasKey(x => new { x.Date, x.GameName });

            var steamGamesModelBuilder = modelBuilder.Entity<SteamGames>();
            steamGamesModelBuilder.HasKey(x => x.AppId);

            var wishlistActionsModelBuilder = modelBuilder.Entity<WishlistActions>();
            wishlistActionsModelBuilder.HasKey(x => new { x.Date, x.Game });

            var packageSalesModelBuilder = modelBuilder.Entity<PackageSales>();
            packageSalesModelBuilder.HasNoKey();
        }
    }
}
