using Amica.API.WebServer.Data.DTO.Posts;
using MongoDB.Driver;

namespace Amica.API.WebServer.Data.Repositories.Likes {
    public interface ILikesRepository {
        Task<bool> Like(long author_id, long post_id, bool checkPostExistence);
        Task<bool> UnLike(long author_id, long post_id);
        Task<IEnumerable<long>?> GetLikers(long post_id, PaginationGetRequest request);
        Task<long> GetCountLikers(long post_id);
        Task<bool> IsInLikers(long author_id, long post_id);
        Task<DeleteResult> ClearAll();
        Task<DeleteResult> DeletePostLikes(long post_id);
    }
}