using Amica.API.Data.Models;
using Amica.API.WebServer.Data.DTO.Profiles;

namespace Amica.API.WebServer.Data.DTO.Posts {
    public class CommentDTO {
        public string? ID { get; set; }
        public string? Text { get; set; }
        public DateTime? Date { get; set; }
        public ProfileDTO? Author { get; set; }
    }
}
