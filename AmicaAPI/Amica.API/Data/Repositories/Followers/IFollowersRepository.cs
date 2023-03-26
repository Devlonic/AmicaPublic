using Amica.API.WebServer.Data.DTO.Posts;
using Amica.API.WebServer.Data.DTO.Profiles;

namespace Amica.API.WebServer.Data.Repositories.Followers {
    public interface IFollowersRepository {
        Task<bool> Follow(long requester_profile_id, long profile_id, bool checkProfileExistence);
        Task<bool> UnFollow(long requester_profile_id, long profile_id, bool checkProfileExistence);
        Task<int> GetCountFollowers(long profile_id);
        Task<int> GetCountFollowing(long profile_id);
        Task<IEnumerable<ShortProfileDTO>?> GetFollowers(long profile_id, PaginationGetRequest request);
        Task<IEnumerable<ShortProfileDTO>?> GetFollowings(long profile_id, PaginationGetRequest request);

        Task<IEnumerable<PostDTO>?> GetPostsByFollowings(long profile_id, PaginationGetRequest request);

        Task<bool> IsFollow(long requester_profile_id, long profile_id);
    }
}