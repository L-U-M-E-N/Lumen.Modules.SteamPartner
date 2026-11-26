namespace Lumen.Modules.SteamPartner.Business.Interfaces {
    public interface ISteamPartnerStats {
        Task StoreCurrentFollowersAsync();
        Task StoreReviewsAsync(string API_KEY);
        Task StoreWishlistsAsync(string API_KEY);
        Task StorePackageSalesAsync(string API_KEY);
    }
}
