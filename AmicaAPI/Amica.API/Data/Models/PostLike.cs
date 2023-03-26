using Amica.API.Data.Models.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Amica.API.Data.Models {
    public class PostLike {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [CompoundIndexField]
        [BsonElement("post_id")]
        public long PostID { get; set; }

        [CompoundIndexField]
        [BsonElement("author_id")]
        public long AuthorID { get; set; }

        [BsonElement("date")]
        public DateTime? Date { get; set; }
    }
}
