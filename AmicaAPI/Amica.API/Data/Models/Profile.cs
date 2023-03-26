using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Amica.API.Data.Models {
    public class Profile {
        [Key]
        public long ID { get; set; }

        public string? ProfilePhoto { get; set; }
        public string? FullName { get; set; }

        public string? AccountID { get; set; }

        [ForeignKey(nameof(AccountID))]
        [JsonIgnore]
        public Account? Account { get; set; }

        public string? NickName { get; set; }

        [Column("Follower")]
        public virtual ICollection<Profile> Followers { get; set; } = new List<Profile>();

        [Column("WhomFollowing")]
        public virtual ICollection<Profile> Followings { get; set; } = new List<Profile>();
    }
}
