using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Amica.API.Data.DTO {

    public class SignInRequest {
        [DefaultValue("defaultusername")]
        [JsonPropertyName("login")]
        public string? Login { get; set; }

        [DefaultValue("KAKAkika1234")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
