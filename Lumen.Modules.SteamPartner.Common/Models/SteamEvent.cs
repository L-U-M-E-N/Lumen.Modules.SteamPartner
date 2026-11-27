namespace Lumen.Modules.SteamPartner.Common.Models {
    public class SteamEvent {
        public Guid Id { get; set; } = Guid.NewGuid();
        public uint? CombinedDiscountId { get; set; }
        public string? EventKey { get; set; }
        public string? Name { get; set; }
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }
        public string? Description { get; set; }
        public string? Group { get; set; }
        public List<SteamEventDiscount> Discounts { get; set; } = [];
    }

    public class SteamEventDiscount {
        public uint DiscountId { get; set; }
        public uint AppId { get; set; }
        public uint DiscountPercentage { get; set; }
    }
}
