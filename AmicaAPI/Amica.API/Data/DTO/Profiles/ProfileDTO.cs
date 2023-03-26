using Amica.API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amica.API.WebServer.Data.DTO.Profiles {
    public class ProfileDTO {
        public ProfileDTO() {

        }
        public ProfileDTO(Profile original) : this(
            original.ID,
            original.ProfilePhoto,
            original.FullName,
            original.NickName,
            original.Account
            ) { }

        public ProfileDTO(long iD, string? profilePhoto, string? fullName, string? nickName, Account? account) {
            this.ID = iD;
            this.ProfilePhoto = profilePhoto;
            this.FullName = fullName;
            this.NickName = nickName;
            this.Account = account;
        }

        public long ID { get; set; }

        public string? ProfilePhoto { get; set; }
        public string? FullName { get; set; }
        public string? NickName { get; set; }

        public long CountPosts { get; set; }
        public long CountFollowers { get; set; }
        public long CountFollowing { get; set; }

        public bool IsRequesterOwnsProfile { get; set; }
        public bool IsRequesterFollowsProfile { get; set; }

        [JsonIgnore]
        public Account? Account { get; set; }
    }
}
