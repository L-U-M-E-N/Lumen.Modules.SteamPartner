namespace Lumen.Modules.SteamPartner.Common.Models {
    public class SteamWishlist {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ulong AppId { get; set; }
        public DateOnly Date { get; set; }
        public string? CountryCode { get; set; } = null;
        public string? CountryName { get; set; } = null;
        public string? CountryRegion { get; set; } = null;
        public string? Language { get; set; } = null;
        public uint WishlistsAdditions { get; set; }
        public uint WishlistDeletions { get; set; }
        public uint WishlistsPurchases { get; set; }
        public uint WishlistsGifts { get; set; }
        public uint WishlistsAdditionsWindows { get; set; }
        public uint WishlistsAdditionsMac { get; set; }
        public uint WishlistsAdditionsLinux { get; set; }
    }
}
