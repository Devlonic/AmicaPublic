using Amica.API.Data.Models;
using Amica.API.Data;
using Amica.API.WebServer.ConfigurationManagers;
using Amica.API.WebServer.Data.DTO.Posts;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;
using Amica.API.WebServer.Data.DTO.Profiles;
using System.Xml.Linq;
using System.Data.Common;

namespace Amica.API.WebServer.Data.Repositories.Followers {
    public class FollowersRepository : SqlRepository<Profile>, IFollowersRepository {
        JsonConfig<Post> postsConfig;
        public FollowersRepository(JsonConfig<Profile> config, ILogger<Repository> logger, AmicaDbContext db, JsonConfig<Post> postsConfig) : base(config, logger, db) {
            this.postsConfig = postsConfig;
        }

        public async Task<bool> Follow(long requester_profile_id, long profile_id, bool checkProfileExistence = true) {
            try {
                // expected situation that signalize about client want follow by himself
                if ( requester_profile_id == profile_id )
                    return false;

                var who = await db.Profiles.FindAsync(requester_profile_id);
                var whom = await db.Profiles.FindAsync(profile_id);

                // unexpected situation that signalize about identity system critical error
                if ( who is null )
                    throw new ArgumentException(nameof(requester_profile_id));

                // expected situation that signalize about client wrong profile id
                if ( whom is null )
                    return false;

                who.Followings.Add(whom);

                var count = await db.SaveChangesAsync();
                return count > 0;
            }
            catch ( DbUpdateException e ) {
                // probably profile already follow
                logger.LogWarning(e.Message);

                return false;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);

                throw;
            }
        }

        public async Task<int> GetCountFollowers(long profile_id) {
            // remake to ORM from raw sql
            var con = db.Database.GetDbConnection();
            await con.OpenAsync();
            var cmd = con.CreateCommand();
            cmd.CommandText = $"SELECT COUNT(FOLLOWERS.FOLLOWINGSID) FROM FOLLOWERS WHERE FOLLOWERS.followingsid = {profile_id}";
            var res = await cmd.ExecuteScalarAsync();
            return (int)( res ?? throw new Exception("unexpected sql scalar value") );
        }

        public async Task<int> GetCountFollowing(long profile_id) {
            // remake to ORM from raw sql
            var con = db.Database.GetDbConnection();
            await con.OpenAsync();
            var cmd = con.CreateCommand();
            cmd.CommandText = $"SELECT COUNT(FOLLOWERS.FOLLOWERSID) FROM FOLLOWERS WHERE FOLLOWERS.followersid = {profile_id}";
            var res = await cmd.ExecuteScalarAsync();
            await con.CloseAsync();
            return (int)( res ?? throw new Exception("unexpected sql scalar value") );
        }

        public async Task<IEnumerable<ShortProfileDTO>?> GetFollowers(long profile_id, PaginationGetRequest request) {
            try {
                var res = await db.Profiles.FindAsync(profile_id);
                if ( res is null )
                    return null;

                await db.Entry(res).
                    Collection(p => p.Followers).
                    Query().
                    Skip(request.PageNumber * config["ProfileFollowersPageSize", true]).
                    Take(config["ProfileFollowersPageSize", true]).
                    LoadAsync();

                return res.Followers.Select(p => new ShortProfileDTO() {
                    ID = p.ID,
                    FullName = p.FullName,
                    NickName = p.NickName,
                    ProfilePhoto = p.ProfilePhoto,
                });
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<ShortProfileDTO>?> GetFollowings(long profile_id, PaginationGetRequest request) {
            try {
                var res = await db.Profiles.FindAsync(profile_id);
                if ( res is null )
                    return null;

                await db.Entry(res).
                    Collection(p => p.Followings).
                    Query().
                    Skip(request.PageNumber * config["ProfileFollowingPageSize", true]).
                    Take(config["ProfileFollowingPageSize", true]).
                    LoadAsync();

                return res.Followings.Select(p => new ShortProfileDTO() {
                    ID = p.ID,
                    FullName = p.FullName,
                    NickName = p.NickName,
                    ProfilePhoto = p.ProfilePhoto,
                });
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);
                return null;
            }
        }

        public async Task<IEnumerable<PostDTO>?> GetPostsByFollowings(long profile_id, PaginationGetRequest request) {
            // remake to ORM from raw sql
            var con = db.Database.GetDbConnection();
            await con.OpenAsync();
            var cmd = con.CreateCommand();
            cmd.CommandText = @$"
SELECT Posts.ID, Posts.ID_Author, Posts.Title, Posts.DateCreated, Images.Uri, Profiles.NickName, Profiles.ProfilePhoto
  FROM [AmicaDB].[dbo].[Followers]
  INNER JOIN Posts ON Posts.ID_Author = FollowingsID
  INNER JOIN Profiles ON Profiles.ID = FollowingsID
  INNER JOIN Images ON Images.PostID = Posts.ID AND Images.IsPrimary = 1
  WHERE FollowersID = {profile_id}
ORDER BY Posts.DateCreated DESC OFFSET {request.PageNumber * postsConfig["FeedPostsPageSize", true]} ROWS
FETCH NEXT {postsConfig["FeedPostsPageSize", true]} ROWS ONLY;";
            var r = await cmd.ExecuteReaderAsync();
            List<PostDTO> posts = new List<PostDTO>();
            while ( await r.ReadAsync() ) {
                PostDTO res = new PostDTO() {
                    ID = r.GetInt64(0),
                    ID_Author = r.GetInt64(1),
                    Title = r.GetString(2),
                    DateCreated = r.GetDateTime(3),
                    PrimaryImage = r.GetString(4),
                    Author = new Profile() {
                        ID = r.GetInt64(1),
                        NickName = r.GetString(5),
                        ProfilePhoto = r.GetString(6),
                    }
                };
                posts.Add(res);
            }
            await con.CloseAsync();
            return posts;
        }

        public async Task<bool> IsFollow(long requester_profile_id, long profile_id) {
            DbConnection? con = null;
            try {
                if ( requester_profile_id == profile_id )
                    return false;

                con = db.Database.GetDbConnection();
                if ( con.State == System.Data.ConnectionState.Closed )
                    await con.OpenAsync();

                var cmd = con.CreateCommand();
                cmd.CommandText = @$"SELECT COUNT(FollowersID) FROM [AmicaDB].[dbo].[Followers] WHERE FollowersID = {requester_profile_id} AND FollowingsID = {profile_id}";
                var r = ( await cmd.ExecuteScalarAsync() ) as int?;
                if ( r.HasValue ) {
                    return r.Value >= 1;
                }
                return false;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);

                throw;
            }
            finally {
                if ( con is not null )
                    await con.CloseAsync();
            }
        }

        public async Task<bool> UnFollow(long requester_profile_id, long profile_id, bool checkProfileExistence = true) {
            try {
                // expected situation that signalize about client want unfollow by himself
                if ( requester_profile_id == profile_id )
                    return false;

                // remake to ef style
                var count = await db.Database.ExecuteSqlRawAsync($"DELETE FROM FOLLOWERS WHERE FOLLOWERSID = {requester_profile_id} AND FOLLOWINGSID = {profile_id}");

                return count > 0;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);

                throw;
            }
        }
    }
}