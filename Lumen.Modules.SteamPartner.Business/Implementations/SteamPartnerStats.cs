using Dysnomia.Common.SteamWebAPI;
using Dysnomia.Common.SteamWebAPI.Models;

using Lumen.Modules.SteamPartner.Business.Interfaces;
using Lumen.Modules.SteamPartner.Common.Models;
using Lumen.Modules.SteamPartner.Data;

namespace Lumen.Modules.SteamPartner.Business.Implementations {
    public class SteamPartnerStats(SteamPartnerContext context, ISteamCommunity steamCommunity, ISteamPartner steamPartner, IUserReviewsService userReviewsService) : ISteamPartnerStats {
        public async Task StoreCurrentFollowersAsync() {
            var gamesList = context.SteamGames.ToList();
            var currDate = DateOnly.FromDateTime(DateTime.UtcNow);

            foreach (var game in gamesList) {
                if (!game.QueryFollowers) {
                    continue;
                }

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
        public async Task StoreCurrentReviewsAsync(string API_KEY) {
            var gamesList = context.SteamGames.ToList();

            foreach (var game in gamesList) {
                List<Review> reviews = [];

                string? cursor = null;
                while (true) {
                    var response = await userReviewsService.GetAppReviews(
                            API_KEY,
                            (ulong)game.AppId,
                            filter: Dysnomia.Common.SteamWebAPI.Enums.EUserReviewsAppReviewsFilter.k_EUserReviewsAppReviewsFilter_Recent,
                            languages: "languages[0]=all",
                            review_type: Dysnomia.Common.SteamWebAPI.Enums.EUserReviewsReviewType.k_EUserReviewsReviewType_All,
                            purchase_type: Dysnomia.Common.SteamWebAPI.Enums.EUserReviewsPurchaseType.k_EUserReviewsPurchaseType_All,
                            cursor: cursor
                    );

                    if (response.response.total_matching == 0 || response.response.query_summary?.num_reviews == 0 || response.response.reviews is null) {
                        break;
                    }

                    cursor = response.response.cursor;
                    reviews.AddRange(response.response.reviews);
                }

                foreach (var review in reviews) {
                    var found = context.SteamReviews.FirstOrDefault(x => x.RecommendationId == review.recommendationid);
                    if (found is not null) {
                        found.ReviewUpdate = DateTime.UnixEpoch.AddSeconds(review.timestamp_updated);
                        found.Positive = review.voted_up;
                        found.SteamPurchase = review.steam_purchase;
                        found.RecievedForFree = review.received_for_free;
                        found.Refunded = review.refunded;
                        found.Language = review.language;
                        continue;
                    }

                    context.SteamReviews.Add(new Common.Models.SteamReview {
                        RecommendationId = review.recommendationid,
                        AppId = (ulong)game.AppId,
                        AuthorSteamId = review.author.steamid,
                        ReviewCreation = DateTime.UnixEpoch.AddSeconds(review.timestamp_created),
                        ReviewUpdate = DateTime.UnixEpoch.AddSeconds(review.timestamp_updated),
                        Positive = review.voted_up,
                        SteamPurchase = review.steam_purchase,
                        RecievedForFree = review.received_for_free,
                        Refunded = review.refunded,
                        Language = review.language,
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
        }

        private async Task SyncWishlists(string cookie, SteamGames game) {
            var minDateWishlists = new DateOnly(2000, 04, 04);
            if (context.WishlistActions.Any(x => x.Game == game.Name)) {
                minDateWishlists = context.WishlistActions.Where(x => x.Game == game.Name).Max(x => x.Date).AddDays(-35);
            }
            var wishlists = await steamPartner.QueryWishlistActionsAsync((ulong)game.AppId, game.Name, minDateWishlists, DateOnly.FromDateTime(DateTime.UtcNow), cookie);

            context.WishlistActions.RemoveRange(context.WishlistActions.Where(x => x.Date >= minDateWishlists && x.Game == game.Name));
            await context.SaveChangesAsync();

            context.WishlistActions.AddRange(wishlists);
            await context.SaveChangesAsync();
        }

        private async Task SyncSales(string cookie, SteamGames game) {
            var minDateSales = new DateOnly(2000, 04, 04);
            if (context.PackagesSales.Any(x => x.Game == game.Name)) {
                minDateSales = context.PackagesSales.Where(x => x.Game == game.Name).Max(x => x.Date).AddDays(-35);
            }
            var sales = await steamPartner.QueryPackageSalesAsync((ulong)game.PackageId!, game.Name, minDateSales, DateOnly.FromDateTime(DateTime.UtcNow), cookie);

            context.PackagesSales.RemoveRange(context.PackagesSales.Where(x => x.Date >= minDateSales && x.Game == game.Name));
            await context.SaveChangesAsync();

            context.PackagesSales.AddRange(sales);
            await context.SaveChangesAsync();
        }
    }
}
