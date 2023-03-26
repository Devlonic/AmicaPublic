using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Amica.API.Data.Models {

    [Table(name: "Posts")]
    public class Post {
        #region ID
        [Key]
        public long ID { get; set; }
        #endregion

        #region Title
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string? Title { get; set; }

        #endregion

        #region Author
        [Required]
        public long ID_Author { get; set; }
        [ForeignKey(nameof(ID_Author))]
        public Profile? Author { get; set; }
        #endregion

        #region Dates
        public DateTime DateCreated { get; set; }

        //public DateTime DateUpdated { get; set; }
        #endregion

        #region Collections One-To-Many
        public virtual ICollection<Image> Images { get; set; } = null!;
        //public virtual ICollection<PostComment>? Comments { get; set; }
        #endregion

        #region Not Mapped Sql
        [NotMapped]
        public long CountLikes { get; set; }
        [NotMapped]
        public long CountComments { get; set; }

        [NotMapped]
        public string? PrimaryImage { get; set; }
        #endregion

        public static string? GetTableName() {
            var type = typeof(Post);
            var table = type.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;
            var tableName = table?.Name;
            return tableName;
        }
    }
}
