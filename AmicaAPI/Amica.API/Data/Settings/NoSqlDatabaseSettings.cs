namespace Amica.API.WebServer.Data.Settings {
    public class NoSqlDatabaseSettings {
        public string ConnectionString { get; init; }
        public string DatabaseName { get; init; }
        public Dictionary<Type, string> CollectionNames { get; init; }

        public NoSqlDatabaseSettings(IConfiguration config, List<Type> models) {
            this.ConnectionString = config.GetConnectionString("Current");
            this.DatabaseName = config.GetValue<string>("DefaultDatabaseName");
            this.CollectionNames = new Dictionary<Type, string>();
            foreach ( var model in models ) {
                var collectionName = $"{model.Name}s";
                this.CollectionNames.Add(model, collectionName);
            }
        }

        public string this[Type model] => CollectionNames[model];
    }
}
