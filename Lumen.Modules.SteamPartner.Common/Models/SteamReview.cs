namespace Lumen.Modules.SteamPartner.Common.Models {
    public class SteamReview {
        public string RecommendationId { get; set; } = null!;
        public ulong AppId { get; set; }
        public string AuthorSteamId { get; set; } = null!;
        public DateTime ReviewCreation { get; set; }
        public DateTime ReviewUpdate { get; set; }
        public bool Positive { get; set; }
        public bool SteamPurchase { get; set; }
        public bool RecievedForFree { get; set; }
        public bool Refunded { get; set; }
        public string Language { get; set; } = null!;
    }
}
