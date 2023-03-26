using Amica.API.Data.DTO.Posts;
using Amica.API.Data.Models;
using Amica.API.WebServer.Data.DTO.Posts;

namespace Amica.API.Data.Repositories {
    public interface IPostsRepository {
        Task<Post?> CreatePost(long profile_id, PostCreateRequest post);
        Task<bool?> DeletePost(long requester_id, long post_id);
        Task<int?> EditPost(long requester_id, long post_id, PostEditRequest request);

        Task<PostDTO?> GetFullInfoPost(long id, long requesterProfile_id);
        Task<IEnumerable<PostDTO>?> GetFeedPostsForProfile(PaginationGetRequest request, long requesterProfile_id);
        Task<ICollection<PostDTO>?> GetPostsForProfile(long profile_id, PaginationGetRequest request, long requesterProfile_id);

        #region Likes
        Task<bool> LikePost(long profile_id, long post_id, bool checkPostExistence);
        Task<bool> UnLikePost(long profile_id, long post_id);

        Task<IEnumerable<ShortProfileDTO>?> GetLikers(long post_id, PaginationGetRequest request);
        Task<long> GetLikersCount(long post_id);
        Task<bool> IsInLikers(long post_id, long profile_id);
        #endregion

        #region Comment
        Task<IEnumerable<CommentDTO>> GetCommentsForPost(long post_id, PaginationGetRequest request);
        Task<PostComment?> CommentPost(long profile_id, long post_id, PostCommentCreateRequest request, bool checkPostExistence);
        Task<bool> DeleteCommentPost(long requester_id, string comment_id);
        Task<bool> EditCommentPost(long requester_id, string comment_id, PostCommentEditRequest request);
        #endregion

    }
}