using Dysnomia.Common.SteamWebAPI;

using Lumen.Modules.Sdk;
using Lumen.Modules.SteamPartner.Business.Implementations;
using Lumen.Modules.SteamPartner.Business.Interfaces;
using Lumen.Modules.SteamPartner.Data;

using Microsoft.EntityFrameworkCore;

namespace Lumen.Modules.SteamPartner.Module {
    public class SteamPartnerModule(IEnumerable<ConfigEntry> configEntries, ILogger<LumenModuleBase> logger, IServiceProvider provider) : LumenModuleBase(configEntries, logger, provider) {
        public const string PLAYLIST_ID = nameof(PLAYLIST_ID);
        public const string API_KEY = nameof(API_KEY);

        public override Task InitAsync(LumenModuleRunsOnFlag currentEnv) {
            // Nothing to do
            return Task.CompletedTask;
        }

        public override async Task RunAsync(LumenModuleRunsOnFlag currentEnv) {
            var steamPartnerStats = provider.GetRequiredService<ISteamPartnerStats>();

            await steamPartnerStats.StoreCurrentFollowersAsync();
        }

        public override bool ShouldRunNow(LumenModuleRunsOnFlag currentEnv) {
            return currentEnv == LumenModuleRunsOnFlag.API && DateTime.UtcNow.Second == 0 && DateTime.UtcNow.Minute == 0 && DateTime.UtcNow.Hour == 6;
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
