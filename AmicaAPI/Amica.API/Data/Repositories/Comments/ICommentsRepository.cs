using Amica.API.Data.Models;
using Amica.API.WebServer.Data.DTO.Posts;
using MongoDB.Driver;

namespace Amica.API.WebServer.Data.Repositories.Comments {
    public interface ICommentsRepository {
        Task<ICollection<PostComment>?> GetCommentsForPost(long post_id, PaginationGetRequest request);
        Task<PostComment?> Comment(long post_id, long author_id, PostCommentCreateRequest request, bool checkPostExistence);
        Task<bool> DeleteComment(long requester_id, string comment_id);
        Task<bool> EditComment(long requester_id, string comment_id, PostCommentEditRequest request);
        Task<DeleteResult> ClearAll();
        Task<long> GetCountComments(long post_id);
        Task<DeleteResult> DeletePostComments(long post_id);
    }
}