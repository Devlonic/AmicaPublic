using Amica.API.Data.Models;
using Amica.API.WebServer.ConfigurationManagers;
using Microsoft.Extensions.Caching.Memory;
using System.Xml;

namespace Amica.API.WebServer.Data.Services {
    public class AccountThrottleService {
        private readonly IMemoryCache cache;
        private readonly JsonConfig<Account> config;
        private readonly ILogger<AccountThrottleService> logger;

        public AccountThrottleService(IMemoryCache cache, JsonConfig<Account> config, ILogger<AccountThrottleService> logger) {
            this.cache = cache;
            this.config = config;
            this.logger = logger;
        }

        public async Task ResetThrottleAsync(string account_id) {
            await Task.Run(() => {
                lock ( this ) {
                    DateTimeOffset expiration = DateTimeOffset.Now + TimeSpan.FromSeconds(config["ResetIntervalAtSeconds", true]);
                    logger.LogDebug($"{account_id} count reset at: {expiration}");
                    cache.Set(account_id, new CacheData() {
                        Value = config["ActionsForDefaultUser", true],
                        Expiration = expiration
                    }, expiration);
                }
            });
        }

        public async Task<bool> CanAct(string account_id) {
            var inCache = cache.TryGetValue(account_id, out CacheData? remain);
            if ( inCache is false ) {
                await ResetThrottleAsync(account_id);
                remain = new CacheData() { Value = config["ActionsForDefaultUser", true] };
            }
            else {
                if ( remain?.Value > 0 ) {
                    cache.Set(account_id, new CacheData() {
                        Value = remain.Value - 1,
                        Expiration = remain.Expiration
                    }, remain.Expiration);
                }
            }
            return remain?.Value > 0;
        }
    }
}
