namespace Lumen.Modules.SteamPartner.Common.Models {
    public class SteamGames {
        public int AppId { get; set; }
        public int? PackageId { get; set; }
        public int SteamRunAs { get; set; }
        public string Name { get; set; } = null!;
    }
}
