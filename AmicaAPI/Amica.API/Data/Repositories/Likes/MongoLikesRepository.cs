using Amica.API.Data.Models;
using Amica.API.WebServer.ConfigurationManagers;
using Amica.API.WebServer.Data.DTO.Posts;
using Amica.API.WebServer.Data.Settings;
using Amica.API.WebServer.Extentions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Amica.API.WebServer.Data.Repositories.Likes {
    public class MongoLikesRepository : MongoRepository<PostLike>, ILikesRepository {
        private IMongoCollection<PostLike> likes => context;
        public MongoLikesRepository(
            ILogger<MongoRepository<PostLike>> logger,
            JsonConfig<PostLike> config,
            IOptions<NoSqlDatabaseSettings> options) : base(logger, config, options, true) {
        }

        public async Task<long> GetCountLikers(long post_id) {
            var count = await likes.CountDocumentsAsync(l => l.PostID == post_id);
            return count - 1; // ignore dummy like
        }

        public async Task<IEnumerable<long>?> GetLikers(long post_id, PaginationGetRequest request) {
            try {
                var res = await likes.
                    Find
                    (Builders<PostLike>.Filter.Eq(l => l.PostID, post_id)).
                    Skip(request.PageNumber * config["PostLikesPageSize", true]).
                    Limit(config["PostLikesPageSize", true]).
                    Project(l => l.AuthorID).
                    ToListAsync();
                res.Remove(-1);

                return res;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);
                return null;
            }
        }

        public async Task<bool> IsInLikers(long author_id, long post_id) {
            try {
                var res = await likes.CountDocumentsAsync(l => l.PostID == post_id && l.AuthorID == author_id);
                return res != 0;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);
                return false;
            }
        }

        public async Task<bool> Like(long author_id, long post_id, bool checkPostExistence = true) {
            try {
                // if post exist (check by dummy author like by existing at least one record)
                if ( checkPostExistence && await likes.Find(l => l.PostID == post_id).FirstOrDefaultAsync() is null )
                    return false;

                // if already found author`s like on this post
                if ( await likes.Find(l => l.PostID == post_id && l.AuthorID == author_id).FirstOrDefaultAsync() is not null )
                    return false;

                await likes.InsertOneAsync(new PostLike() {
                    AuthorID = author_id,
                    PostID = post_id,
                    Date = DateTime.UtcNow
                });
                return true;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);
                return false;
            }
        }

        public async Task<bool> UnLike(long author_id, long post_id) {
            try {
                var res = await likes.DeleteOneAsync(l => l.PostID == post_id && l.AuthorID == author_id);
                // if deleted more than 0 rows (only 1 is valid)
                return res.DeletedCount > 0;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);
                return false;
            }
        }

        public async Task<DeleteResult> ClearAll() {
            return await likes.DeleteManyAsync(l => true);
        }

        public async Task<DeleteResult> DeletePostLikes(long post_id) {
            return await likes.DeleteManyAsync(l => l.PostID == post_id);
        }
    }
}