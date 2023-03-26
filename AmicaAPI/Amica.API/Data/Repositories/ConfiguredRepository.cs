using Amica.API.WebServer.ConfigurationManagers;
using Amica.API.WebServer.Data.Settings;
using Microsoft.Extensions.Options;

namespace Amica.API.WebServer.Data.Repositories {
    public abstract class ConfiguredRepository<Model> : Repository {
        protected readonly JsonConfig<Model> config;

        protected ConfiguredRepository(JsonConfig<Model> config, ILogger<Repository> logger) : base(logger) {
            this.config = config;
        }
    }
}
