using Amica.API.Data.Models;
using Amica.API.WebServer.Data.DTO.Posts;

namespace Amica.API.Data.Repositories {
    public interface IImagesRepository {
        Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> files);
        Task<string> UploadImageAsync(IFormFile file);
        Task<long> SetPrimaryImagesForPostsUriAsync(ICollection<PostDTO> posts, long profile_id);
    }
}
