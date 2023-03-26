using Amica.API.Data.Models.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amica.API.Data.Models {
    public class PostComment {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [CompoundIndexField]
        [BsonElement("post_id")]
        public long PostID { get; set; }

        [BsonElement("author_id")]
        public long AuthorID { get; set; }

        [BsonElement("text")]
        public string? Text { get; set; }

        [BsonElement("date")]
        public DateTime? Date { get; set; }
    }
}
