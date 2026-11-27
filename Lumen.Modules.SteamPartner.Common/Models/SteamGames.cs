namespace Lumen.Modules.SteamPartner.Common.Models {
    public class SteamGames {
        public int AppId { get; set; }
        public int[] PackageId { get; set; } = [];
        public string Name { get; set; } = null!;
        public bool QueryFollowers { get; set; } = true;
        public DateOnly? PublicStorePage { get; set; }
        public DateOnly? ReleaseDate { get; set; }
    }
}
