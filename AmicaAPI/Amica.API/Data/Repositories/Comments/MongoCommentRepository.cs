using Amica.API.Data.Models;
using Amica.API.WebServer.ConfigurationManagers;
using Amica.API.WebServer.Data.DTO.Posts;
using Amica.API.WebServer.Data.Repositories.Comments;
using Amica.API.WebServer.Data.Settings;
using Amica.API.WebServer.Extentions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Amica.API.WebServer.Data.Repositories.Comments {
    public class MongoCommentRepository : MongoRepository<PostComment>, ICommentsRepository {
        private IMongoCollection<PostComment> comments => context;
        public MongoCommentRepository(
            ILogger<MongoRepository<PostComment>> logger,
            JsonConfig<PostComment> config,
            IOptions<NoSqlDatabaseSettings> options) : base(logger, config, options, false) {
        }

        public async Task<DeleteResult> ClearAll() {
            return await comments.DeleteManyAsync(l => true);
        }

        public async Task<PostComment?> Comment(long post_id, long author_id, PostCommentCreateRequest request, bool checkPostExistence) {
            try {
                // if post exist (check by dummy author comment by existing at least one record)
                if ( checkPostExistence && await comments.Find(l => l.PostID == post_id).FirstOrDefaultAsync() is null )
                    return null;

                var comment = new PostComment() {
                    AuthorID = author_id,
                    PostID = post_id,
                    Date = DateTime.UtcNow,
                    Text = request.CommentText
                };

                await comments.InsertOneAsync(comment);
                return comment;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);
                return null;
            }
        }

        public async Task<bool> DeleteComment(long requester_id, string comment_id) {
            try {
                // authorization by comparing owner id of comment and requester id
                var res = await comments.DeleteOneAsync(c => c.Id == comment_id && c.AuthorID == requester_id);

                // expected only 1
                return res.DeletedCount >= 1;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);
                return false;
            }
        }

        public async Task<DeleteResult> DeletePostComments(long post_id) {
            return await comments.DeleteManyAsync(p => p.PostID == post_id);
        }

        public async Task<bool> EditComment(long requester_id, string comment_id, PostCommentEditRequest request) {
            try {
                // authorization by comparing owner id of comment and requester id
                var res = await comments.UpdateOneAsync(
                    c => c.Id == comment_id && c.AuthorID == requester_id,
                    Builders<PostComment>.Update.Set(c => c.Text, request.NewCommentText));

                // expected only 1
                return res.MatchedCount >= 1;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);
                return false;
            }
        }

        public async Task<ICollection<PostComment>?> GetCommentsForPost(long post_id, PaginationGetRequest request) {
            try {
                var res = await comments.
                    Find(c => c.PostID == post_id && c.AuthorID != -1). // ignore dummy comment
                    Skip(request.PageNumber * config["PostCommentsPageSize", true]).
                    Limit(config["PostCommentsPageSize", true]).
                    SortByDescending(c => c.Date).
                    ToListAsync();

                return res;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);
                return null;
            }
        }

        public async Task<long> GetCountComments(long post_id) {
            var count = await comments.CountDocumentsAsync(l => l.PostID == post_id);
            return count - 1; // ignore dummy comment
        }
    }
}