using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Amica.API.WebServer.ConfigurationManagers {
    public class JwtConfig {
        private IConfiguration configuration;

        public JwtConfig(IConfiguration configuration) {
            this.configuration = configuration;
        }

        public string Get(string field) {
            return configuration[$"Jwt:{field}"] ?? throw new JsonException($"JWT Setting missing: {field}");
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
