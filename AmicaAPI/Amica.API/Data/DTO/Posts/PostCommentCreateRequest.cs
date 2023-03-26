using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Amica.API.WebServer.Data.DTO.Posts {
    public class PostCommentCreateRequest {
        [FromForm]
        [JsonPropertyName("text")]
        public string CommentText { get; set; } = null!;
    }
}
