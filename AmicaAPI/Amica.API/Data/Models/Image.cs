using Amica.API.Data.Models.Attributes;
using Amica.API.WebServer.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Reflection.Metadata;

namespace Amica.API.Data.Models {
    [Table(name: "Images")]
    public class Image {
        #region ID
        [Key]
        public long ID { get; set; }
        #endregion

        #region Title
        [Required]
        [Url]
        public string Uri { get; set; } = null!;
        #endregion

        public bool IsPrimary { get; set; }

        public long PostID { get; set; }


        public static string? GetTableName() {
            var type = typeof(Image);
            var table = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            var tableName = table?.Name;
            return tableName;
        }
    }
}
