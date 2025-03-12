namespace Lumen.Modules.SteamPartner.Business.Interfaces {
    public interface ISteamPartnerStats {
        Task StoreCurrentFollowersAsync();
        Task QuerySaleAndWishlistDataAsync(string steamLoginSecureToken);
    }
}
