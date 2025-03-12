using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Lumen.Modules.SteamPartner.Data {
    internal class SteamDbContextFactory : IDesignTimeDbContextFactory<SteamPartnerContext> {
        public SteamPartnerContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<SteamPartnerContext>();
            optionsBuilder.UseNpgsql();

            return new SteamPartnerContext(optionsBuilder.Options);
        }
    }
}