using Amica.API.WebServer.Data.DTO.Profiles;
using System.Text.Json.Serialization;

namespace Amica.API.Data.DTO {
    public class SignInResponce {
        [JsonPropertyName("isSignedIn")]
        public bool? IsSignedIn { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; } = null;

        [JsonPropertyName("token")]
        public string? Token { get; set; } = null;

        public ProfileDTO? Profile { get; set; }
    }
}
