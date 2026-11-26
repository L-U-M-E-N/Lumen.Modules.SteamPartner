namespace Lumen.Modules.SteamPartner.Business.Interfaces {
    public interface ISteamPartnerStats {
        Task StoreCurrentFollowersAsync();
        Task StoreCurrentReviewsAsync(string API_KEY);
        Task QuerySaleAndWishlistDataAsync(string steamLoginSecureToken);
        Task StoreCurrentWishlistsAsync(string API_KEY);
    }
}
