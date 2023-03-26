using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Amica.API.WebServer.Data.DTO.Posts {
    public class PostCommentEditRequest {
        [FromForm]
        [JsonPropertyName("new_text")]
        public string NewCommentText { get; set; } = null!;
    }
}
