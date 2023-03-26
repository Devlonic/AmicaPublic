using Amica.API.Data;
using Amica.API.WebServer.ConfigurationManagers;

namespace Amica.API.WebServer.Data.Repositories {
    public abstract class HttpSqlRepository<Model> : SqlRepository<Model> {
        protected readonly HttpClient http;
        protected HttpSqlRepository(JsonConfig<Model> config, ILogger<Repository> logger, AmicaDbContext db, HttpClient http) : base(config, logger, db) {
            this.http = http;
        }
    }
}