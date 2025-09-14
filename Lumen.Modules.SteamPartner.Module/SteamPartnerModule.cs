using Dysnomia.Common.SteamWebAPI;

using Lumen.Modules.Sdk;
using Lumen.Modules.SteamPartner.Business.Implementations;
using Lumen.Modules.SteamPartner.Business.Interfaces;
using Lumen.Modules.SteamPartner.Data;

using Microsoft.EntityFrameworkCore;

namespace Lumen.Modules.SteamPartner.Module {
    public class SteamPartnerModule(IEnumerable<ConfigEntry> configEntries, ILogger<LumenModuleBase> logger, IServiceProvider provider) : LumenModuleBase(configEntries, logger, provider) {
        public override async Task InitAsync(LumenModuleRunsOnFlag currentEnv) {
            await RunAsync(currentEnv, DateTime.UtcNow);
        }

        public override async Task RunAsync(LumenModuleRunsOnFlag currentEnv, DateTime date) {
            try {
                var steamPartnerStats = provider.GetRequiredService<ISteamPartnerStats>();

                await steamPartnerStats.StoreCurrentFollowersAsync();
            } catch (Exception ex) {
                logger.LogError(ex, "Error when updating steam followers");
            }
        }

        public override bool ShouldRunNow(LumenModuleRunsOnFlag currentEnv, DateTime date) {
            return currentEnv == LumenModuleRunsOnFlag.API && date.Second == 0 && date.Minute == 0 && date.Hour == 6;
        }

        public override Task ShutdownAsync() {
            // Nothing to do
            return Task.CompletedTask;
        }

        public static new void SetupServices(LumenModuleRunsOnFlag currentEnv, IServiceCollection serviceCollection, string? postgresConnectionString) {
            if (currentEnv == LumenModuleRunsOnFlag.API) {
                serviceCollection.AddDbContext<SteamPartnerContext>(o => o.UseNpgsql(postgresConnectionString, x => x.MigrationsHistoryTable("__EFMigrationsHistory", SteamPartnerContext.SCHEMA_NAME)));
                serviceCollection.AddTransient<ISteamPartnerStats, SteamPartnerStats>();
                serviceCollection.AddTransient<ISteamCommunity, SteamCommunity>();
                serviceCollection.AddTransient<ISteamPartner, Dysnomia.Common.SteamWebAPI.SteamPartner>();
                serviceCollection.AddHttpClient();
            }
        }

        public override Type GetDatabaseContextType() {
            return typeof(SteamPartnerContext);
        }
    }
}
