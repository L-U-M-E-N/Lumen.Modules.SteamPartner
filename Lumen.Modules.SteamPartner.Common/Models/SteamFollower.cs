namespace Lumen.Modules.SteamPartner.Common.Models {
    public class SteamFollower {
        public DateOnly Date { get; set; }
        public string GameName { get; set; } = null!;
        public int FollowerAmount { get; set; }
    }
}
