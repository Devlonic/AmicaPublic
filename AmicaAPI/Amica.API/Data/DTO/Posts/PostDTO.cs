using Amica.API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Amica.API.WebServer.Data.DTO.Posts {
    public class PostDTO {
        public PostDTO() {

        }
        public PostDTO(Post original) : this(
            original.ID,
            original.Title,
            original.ID_Author,
            original.Author,
            original.DateCreated,
            original.Images,
            original.CountLikes,
            original.CountComments,
            original.PrimaryImage) { }

        public PostDTO(long iD, string? title, long iD_Author, Profile? author, DateTime dateCreated, ICollection<Image> images, long countLikes, long countComments, string? primaryImage) {
            this.ID = iD;
            this.Title = title;
            this.ID_Author = iD_Author;
            this.Author = author;
            this.DateCreated = dateCreated;
            this.Images = images;
            this.CountLikes = countLikes;
            this.CountComments = countComments;
            this.PrimaryImage = primaryImage;
        }

        public long ID { get; set; }

        public string? Title { get; set; }

        public long ID_Author { get; set; }
        public Profile? Author { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual ICollection<Image> Images { get; set; } = null!;
        public long CountLikes { get; set; }
        public long CountComments { get; set; }

        public string? PrimaryImage { get; set; }

        public bool RequestProfileIsInLikers { get; set; }


        public static explicit operator PostDTO(Post orig) => new PostDTO(orig);
    }
}
