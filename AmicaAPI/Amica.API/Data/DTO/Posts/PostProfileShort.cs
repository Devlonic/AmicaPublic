using Amica.API.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Amica.API.WebServer.Data.DTO.Posts {
    public class PostProfileShort {
        public long ID { get; set; }
        public long CountLikes { get; set; }
        public long CountComments { get; set; }
        public string? PrimaryImage { get; set; }
    }
}
