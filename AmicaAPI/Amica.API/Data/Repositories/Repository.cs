namespace Amica.API.WebServer.Data.Repositories {
    public abstract class Repository {
        protected readonly ILogger<Repository> logger;

        protected Repository(ILogger<Repository> logger) {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
