using System.Text.Json;

namespace Amica.API.WebServer.ConfigurationManagers {
    public class JsonConfig<Model> {
        private IConfiguration configuration;
        public JsonConfig(IConfiguration configuration) {
            this.configuration = configuration;
        }

        public string Get(string field) {
            return configuration[$"{typeof(Model).Name}:{field}"] ?? throw new JsonException($"{typeof(Model).Name} Setting missing: {field}");
        }
        public int GetInt(string field) {
            return int.Parse(Get(field));
        }

        public string this[string field] {
            get {
                return Get(field);
            }
        }
        public int this[string field, bool castToInt] {
            get {
                return GetInt(field);
            }
        }
    }
}
