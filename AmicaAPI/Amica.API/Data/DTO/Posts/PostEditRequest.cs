using Amica.API.Data.Models;

namespace Amica.API.Data.DTO.Posts {
    public class PostEditRequest {
        public string? Title { get; set; }

        public int EditPost(Post post) {
            int countModifyed = 0;
            if ( Title is not null ) {
                post.Title = Title;
                countModifyed++;
            }
            return countModifyed;
        }
    }
}
