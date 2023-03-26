using System.Text.Json.Serialization;

namespace Amica.API.Data.DTO {
    public class SignUpResponce {
        [JsonPropertyName("isSignedUp")]
        public bool? IsSignedUp { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; } = null;

        [JsonIgnore]
        public long? ProfileID { get; set; } = null;
    }
}
