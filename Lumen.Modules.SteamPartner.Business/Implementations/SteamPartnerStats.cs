using Dysnomia.Common.SteamWebAPI;

using Lumen.Modules.SteamPartner.Business.Interfaces;
using Lumen.Modules.SteamPartner.Common.Models;
using Lumen.Modules.SteamPartner.Data;

namespace Lumen.Modules.SteamPartner.Business.Implementations {
    public class SteamPartnerStats(SteamPartnerContext context, ISteamCommunity steamCommunity, ISteamPartner steamPartner) : ISteamPartnerStats {
        public async Task StoreCurrentFollowersAsync() {
            var gamesList = context.SteamGames.ToList();
            var currDate = DateOnly.FromDateTime(DateTime.UtcNow);

            foreach (var game in gamesList) {
                if (!context.SteamFollowers.Any(x => x.Date == currDate && x.GameName == game.Name)) {
                    var followersAmount = await steamCommunity.GetAppGroupMemberListAsync((ulong)game.AppId, 1);
                    context.SteamFollowers.Add(new Common.Models.SteamFollower {
                        Date = currDate,
                        GameName = game.Name,
                        FollowerAmount = (int)followersAmount.groupDetails.memberCount,
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task QuerySaleAndWishlistDataAsync(string steamLoginSecureToken) {
            var gamesList = context.SteamGames.ToList();

            foreach (var game in gamesList) {
                if (game is null) { continue; }

                var cookie = $"steamLoginSecure={steamLoginSecureToken}; steamworksRunas={game.SteamRunAs};";
                await SyncWishlists(cookie, game);
                if (game.PackageId is not null) {
                    await SyncSales(cookie, game);
                }
            }

            await context.SaveChangesAsync();

        }

        private async Task SyncWishlists(string cookie, SteamGames game) {
            var minDateWishlists = context.WishlistActions.Where(x => x.Game == game.Name).Max(x => x.Date).AddDays(-35);
            context.WishlistActions.RemoveRange(context.WishlistActions.Where(x => x.Date >= minDateWishlists));

            var wishlists = await steamPartner.QueryWishlistActionsAsync((ulong)game.AppId, game.Name, minDateWishlists, DateOnly.FromDateTime(DateTime.UtcNow), cookie);
            context.WishlistActions.AddRange(wishlists);
        }

        private async Task SyncSales(string cookie, SteamGames game) {
            var minDateSales = context.PackagesSales.Where(x => x.Game == game.Name).Max(x => x.Date).AddDays(-35);
            context.PackagesSales.RemoveRange(context.PackagesSales.Where(x => x.Date >= minDateSales));

            var sales = await steamPartner.QueryPackageSalesAsync((ulong)game.PackageId!, game.Name, minDateSales, DateOnly.FromDateTime(DateTime.UtcNow), cookie);
            context.PackagesSales.AddRange(sales);
        }
    }
}
