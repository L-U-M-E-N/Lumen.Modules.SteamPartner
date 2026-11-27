using Dysnomia.Common.SteamWebAPI;
using Dysnomia.Common.SteamWebAPI.Models;

using Lumen.Modules.SteamPartner.Business.Interfaces;
using Lumen.Modules.SteamPartner.Common.Models;
using Lumen.Modules.SteamPartner.Data;

using System.Globalization;

namespace Lumen.Modules.SteamPartner.Business.Implementations {
    public class SteamPartnerStats(SteamPartnerContext context, ISteamCommunity steamCommunity, ISteamPartner steamPartner, IPartnerFinancialsService partnerFinancialsService, IUserReviewsService userReviewsService) : ISteamPartnerStats {
        private static readonly NumberFormatInfo numberFormatInfo = new() { NumberDecimalSeparator = "." };

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

                await Task.Delay(500);
            }

            await context.SaveChangesAsync();
        }

        public async Task StoreReviewsAsync(string API_KEY) {
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

                await Task.Delay(500);
            }

            await context.SaveChangesAsync();
        }
        public async Task StoreWishlistsAsync(string API_KEY) {
            var gamesList = context.SteamGames.ToList();

            foreach (var game in gamesList) {
                if (game.Name.EndsWith(" Demo")) {
                    continue; // Some old demo had wishlists, and this would make the process extra long for no reason
                }

                var appId = (ulong)game.AppId;
                var currentDate = new DateOnly(2000, 04, 04);
                if (context.SteamWishlists.Any(x => x.AppId == appId)) {
                    currentDate = context.SteamWishlists.Where(x => x.AppId == appId).Max(x => x.Date).AddDays(-3);
                }

                var now = DateOnly.FromDateTime(DateTime.UtcNow);
                while (currentDate <= now) {
                    var dayWishlists = await partnerFinancialsService.GetAppWishlistReporting(API_KEY, appId, currentDate);
                    if (dayWishlists.response.app_min_date is null) {
                        break; // No wishlists at all
                    }


                    if (dayWishlists.response.wishlist_summary is null) {
                        var minDate = DateOnly.ParseExact(dayWishlists.response.app_min_date, "yyyy-MM-dd");
                        if (minDate > currentDate) {
                            currentDate = minDate;
                        } else {
                            currentDate = currentDate.AddDays(1);
                        }
                        await Task.Delay(500);
                        continue;
                    }

                    context.SteamWishlists.RemoveRange(context.SteamWishlists.Where(x => x.AppId == appId && x.Date == currentDate));
                    context.SteamWishlists.Add(GenerateWishlistEntry(appId, currentDate, null, null, null, null, dayWishlists.response.wishlist_summary));

                    foreach (var country in dayWishlists.response.country_summary!) {
                        context.SteamWishlists.Add(GenerateWishlistEntry(appId, currentDate, country.country_code, country.country_name, country.region, null, country.summary_actions));
                    }

                    foreach (var language in dayWishlists.response.language_summary!) {
                        context.SteamWishlists.Add(GenerateWishlistEntry(appId, currentDate, null, null, null, language.language_name, language.summary_actions));
                    }

                    currentDate = currentDate.AddDays(1);

                    await Task.Delay(500);
                }
            }
            await context.SaveChangesAsync();
        }

        public static SteamWishlist GenerateWishlistEntry(ulong appId, DateOnly date, string? countryCode, string? countryName, string? countryRegion, string? language, Wishlist_Summary summary) {
            return new SteamWishlist {
                AppId = appId,
                Date = date,
                CountryCode = countryCode,
                CountryName = countryName,
                CountryRegion = countryRegion,
                Language = language,
                WishlistsAdditions = summary.wishlist_adds,
                WishlistDeletions = summary.wishlist_deletes,
                WishlistsGifts = summary.wishlist_gifts,
                WishlistsPurchases = summary.wishlist_purchases,
                WishlistsAdditionsWindows = summary.wishlist_adds_windows,
                WishlistsAdditionsMac = summary.wishlist_adds_mac,
                WishlistsAdditionsLinux = summary.wishlist_adds_linux,
            };
        }

        public async Task StorePackageSalesAsync(string API_KEY) {
            var currentDate = new DateOnly(2000, 1, 1);
            if (context.SteamPackageSales.Any()) {
                currentDate = context.SteamPackageSales.Max(x => x.Date).AddDays(-35);
            }

            var now = DateOnly.FromDateTime(DateTime.UtcNow);
            while (currentDate <= now) {
                ulong highwatermark_id = 0;
                while (true) {
                    var daySales = await partnerFinancialsService.GetDetailedSales(API_KEY, currentDate, highwatermark_id);
                    if (daySales.response.max_id is null) {
                        await Task.Delay(500);
                        break; // No sales or final page
                    }
                    var maxId = ulong.Parse(daySales.response.max_id);
                    if (maxId == highwatermark_id) {
                        await Task.Delay(500);
                        break; // No sales or final page
                    }

                    if (highwatermark_id == 0) {
                        context.SteamPackageSales.RemoveRange(context.SteamPackageSales.Where(x => x.Date == currentDate));
                    }

                    context.SteamPackageSales.AddRange(daySales.response.results!.Select((res) => {
                        var country = daySales.response.country_info!.First(x => x.country_code == res.country_code);
                        return new SteamPackageSale {
                            PackageId = res.packageid,
                            BundleId = res.bundleid,
                            AppId = res.primary_appid,
                            Date = currentDate,
                            PackageSaleType = res.package_sale_type,
                            Platform = res.platform,
                            CountryCode = res.country_code,
                            CountryName = country.country_name,
                            CountryRegion = country.region,
                            BasePrice = res.base_price == null ? null : decimal.Parse(res.base_price, numberFormatInfo),
                            SalePrice = res.sale_price == null ? null : decimal.Parse(res.sale_price, numberFormatInfo),
                            Currency = res.currency,
                            GrossUnitsActivated = res.gross_units_activated,
                            GrossUnitsSold = res.gross_units_sold,
                            GrossUnitsReturned = res.gross_units_returned,
                            NetUnitsSold = res.net_units_sold,
                            GrossSalesUsd = res.gross_sales_usd == null ? null : decimal.Parse(res.gross_sales_usd, numberFormatInfo),
                            GrossReturnsUsd = res.gross_returns_usd == null ? null : decimal.Parse(res.gross_returns_usd, numberFormatInfo),
                            NetTaxUsd = res.net_tax_usd == null ? null : decimal.Parse(res.net_tax_usd, numberFormatInfo),
                            NetSalesUsd = res.net_sales_usd == null ? null : decimal.Parse(res.net_sales_usd, numberFormatInfo),
                            AdditionalRevenueShareTier = res.additional_revenue_share_tier,
                            CombinedDiscountId = res.combined_discount_id,
                            AvgSalePriceUsd = res.avg_sale_price_usd == null ? null : decimal.Parse(res.avg_sale_price_usd, numberFormatInfo),
                            KeyRequestId = res.key_request_id,
                        };
                    }));

                    await SyncDiscountsAsync(currentDate, daySales);

                    highwatermark_id = maxId;
                    await Task.Delay(500);
                }

                currentDate = currentDate.AddDays(1);

                await context.SaveChangesAsync();

                await Task.Delay(1000);
            }
        }

        public async Task SyncDiscountsAsync(DateOnly currentDate, DetailedSalesResponse response) {
            if (response.response.combined_discount_info is null) {
                return;
            }

            foreach (var combinedDiscount in response.response.combined_discount_info) {
                if (!response.response.results!.Any((x) => x.combined_discount_id == combinedDiscount.combined_discount_id && x.gross_units_sold > 0)) {
                    continue; // Ignore returns
                }

                var item = context.SteamEvents.FirstOrDefault((x) => x.CombinedDiscountId == combinedDiscount.combined_discount_id);
                if (item is not null) {
                    if (item.Start > currentDate) {
                        item.Start = currentDate;
                    }
                    if (item.End < currentDate) {
                        item.End = currentDate;
                    }
                } else {
                    item = new SteamEvent {
                        CombinedDiscountId = (uint)combinedDiscount.combined_discount_id,
                        Name = combinedDiscount.combined_discount_name,
                        Start = currentDate,
                        End = currentDate,
                    };
                    context.SteamEvents.Add(item);
                }

                foreach (var discountId in combinedDiscount.discount_ids) {
                    if (item.Discounts.Any((x) => x.DiscountId == discountId)) {
                        continue;
                    }

                    var relatedDiscount = response.response.discount_info!.First((x) => x.discountid == discountId);
                    var relatedAppId = response.response.results!.FirstOrDefault((x) => x.combined_discount_id == combinedDiscount.combined_discount_id)?.primary_appid ?? 0;
                    item.Discounts.Add(new SteamEventDiscount {
                        DiscountId = (uint)discountId,
                        AppId = relatedAppId,
                        DiscountPercentage = (uint)relatedDiscount.discount_percentage,
                    });

                    item.Description = relatedDiscount.discount_description;
                    item.Group = relatedDiscount.discount_group;
                }
            }
        }
    }
}
