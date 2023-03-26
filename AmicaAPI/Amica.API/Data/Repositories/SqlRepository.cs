using Amica.API.Data;
using Amica.API.WebServer.ConfigurationManagers;

namespace Amica.API.WebServer.Data.Repositories {
    public abstract class SqlRepository<Model> : ConfiguredRepository<Model> {
        protected readonly AmicaDbContext db;
        protected SqlRepository(JsonConfig<Model> config, ILogger<Repository> logger, AmicaDbContext db) : base(config, logger) {
            this.db = db;
        }
    }
}