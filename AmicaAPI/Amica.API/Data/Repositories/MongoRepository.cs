using Amica.API.Data.Models;
using Amica.API.WebServer.ConfigurationManagers;
using Amica.API.WebServer.Data.Settings;
using Amica.API.WebServer.Extentions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Amica.API.WebServer.Data.Repositories {
    public abstract class MongoRepository<Model> : ConfiguredRepository<Model> {
        protected readonly IMongoCollection<Model> context;

        private readonly IOptions<NoSqlDatabaseSettings> options;
        protected NoSqlDatabaseSettings Settings => options.Value;

        protected MongoRepository(ILogger<MongoRepository<Model>> logger, JsonConfig<Model> config, IOptions<NoSqlDatabaseSettings> options, bool ensureUniqueIndex) : base(config, logger) {
            this.options = options;

            var client = new MongoClient(Settings.ConnectionString);
            var db = client.GetDatabase(Settings.DatabaseName);
            context = db.GetCollection<Model>(Settings[typeof(Model)]);

            // ensure unique compound index is created
            var result = context.Indexes.EnsureCompoundIndexExist(ensureUniqueIndex);

            if ( result.Success is false && result.Exception is not null )
                this.logger.LogCritical(result.Exception.Message);

            if ( result.Created is true )
                this.logger.LogDebug($"{typeof(Model)} mongo index created");
        }
    }
}
