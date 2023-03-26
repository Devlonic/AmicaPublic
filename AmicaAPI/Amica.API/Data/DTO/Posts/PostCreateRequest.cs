using System.Text.Json.Serialization;

namespace Amica.API.Data.DTO.Posts {
    public class PostCreateRequest {
        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;
        [JsonPropertyName("images")]
        public List<IFormFile> Images { get; set; } = null!;
    }
}
