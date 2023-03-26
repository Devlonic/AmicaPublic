using Amica.API.Data.Models;
using Amica.API.Data.Models.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Linq;
using System.Xml.Linq;

namespace Amica.API.WebServer.Extentions {
    public static class MongoDbExtentions {
        public static IndexCreationResult EnsureCompoundIndexExist<Document>(this IMongoIndexManager<Document> manager, bool ensureUniqueIndexes = true) {
            // todo

            try {
                var type = typeof(Document);
                var props = type.GetProperties();
                List<string?> compoundFields = new List<string?>();
                foreach ( var prop in props ) {
                    var attrs = prop.GetCustomAttributes(true);
                    if ( attrs.FirstOrDefault(a => a as CompoundIndexFieldAttribute is not null) is not null ) {
                        var bsonName = ( attrs.FirstOrDefault(a => a as BsonElementAttribute is not null) as BsonElementAttribute )?.ElementName;
                        compoundFields.Add(bsonName);
                    }
                }

                if ( compoundFields.Count == 0 )
                    throw new Exception("No CompoundIndexFieldAttribute found");
                string name = $"Index_{type.Name}_({string.Join('+', compoundFields)})_Compount";
                if ( ensureUniqueIndexes )
                    name += "_Unique";

                var indexOptions = new CreateIndexOptions<Document>() {
                    Unique = ensureUniqueIndexes,
                    Name = name,
                    DefaultLanguage = "en",
                };
                var index = $"{{{string.Join(',', compoundFields.ConvertAll(s => string.Format("{0}: 1", s)))}}}";

                if ( manager.List().ToList().Find(i => i.Values.Contains(name)) is null ) {
#pragma warning disable CS0618 // Type or member is obsolete
                    var createdName = manager.CreateOne(index, indexOptions);
#pragma warning restore CS0618 // Type or member is obsolete

                    return new IndexCreationResult() {
                        Created = createdName == indexOptions.Name,
                        Success = createdName == indexOptions.Name,
                    };
                }
                else
                    return new IndexCreationResult() {
                        Created = false,
                        Success = true,
                    };
            }
            catch ( Exception e ) {
                return new IndexCreationResult() {
                    Created = false,
                    Success = false,
                    Exception = e
                };
            }
        }
    }
}
