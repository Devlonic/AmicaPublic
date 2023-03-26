using Amica.API.Data.DTO.Posts;
using Amica.API.Data.Models;
using Amica.API.WebServer.ConfigurationManagers;
using Amica.API.WebServer.Data.DTO.Posts;
using Amica.API.WebServer.Data.DTO.Profiles;
using Amica.API.WebServer.Data.Repositories;
using Amica.API.WebServer.Data.Repositories.Comments;
using Amica.API.WebServer.Data.Repositories.Followers;
using Amica.API.WebServer.Data.Repositories.Likes;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using Image = Amica.API.Data.Models.Image;

namespace Amica.API.Data.Repositories {
    public class PostsRepository : SqlRepository<Post>, IPostsRepository {
        private readonly ILikesRepository likes;
        private readonly ICommentsRepository comments;
        private readonly IImagesRepository images;
        private readonly IFollowersRepository followers;

        public PostsRepository(JsonConfig<Post> config, ILogger<Repository> logger, AmicaDbContext db, ILikesRepository likes, ICommentsRepository comments, IImagesRepository images, IFollowersRepository followers) : base(config, logger, db) {
            this.likes = likes;
            this.comments = comments;
            this.images = images;
            this.followers = followers;
        }

        public async Task<Post?> CreatePost(long author_id, PostCreateRequest post) {
            IDbContextTransaction? currentTransaction = null;
            try {
                currentTransaction = await db.Database.BeginTransactionAsync();

                var newpost = new Post() {
                    Title = post.Title,
                    ID_Author = author_id
                };

                this.db.Posts.Add(newpost);

                var uploaded = await images.UploadImagesAsync(post.Images);

                newpost.Images = uploaded.Select(s => new Image() {
                    Uri = s
                }).ToList();

                // mark first uploaded image as primary
                newpost.Images.First().IsPrimary = true;

                await this.db.SaveChangesAsync();

                // dummy like from dummy user for quick existence checking
                if ( await this.LikePost(-1, newpost.ID, false) is false )
                    throw new Exception("Dummy create like error");

                // dummy comment from dummy user for quick existence checking
                if ( await this.CommentPost(
                    profile_id: -1,
                    post_id: newpost.ID,
                    request: new PostCommentCreateRequest() { CommentText = "" },
                    checkPostExistence: false) is null )
                    throw new Exception("Dummy create comment error");

                await currentTransaction.CommitAsync();

                return newpost;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);

                if ( currentTransaction is not null )
                    await currentTransaction.RollbackAsync();

                throw;
            }
        }

        public async Task<PostDTO?> GetFullInfoPost(long id, long requesterProfile_id) {
            var post = await db.Posts.FindAsync(id);

            if ( post is null )
                return null;

            await db.Entry(post).Collection(x => x.Images).LoadAsync();

            await db.Entry(post).Reference(p => p.Author).LoadAsync();

            var countLikes = await likes.GetCountLikers(id);
            var countComments = await comments.GetCountComments(id);
            post.CountLikes = countLikes;
            post.CountComments = countComments;
            post.PrimaryImage = post.Images.First(i => i.IsPrimary is true).Uri;

            bool isInLikers = await likes.IsInLikers(requesterProfile_id, id);

            return new PostDTO(post) {
                RequestProfileIsInLikers = isInLikers
            };
        }

        public async Task<ICollection<PostDTO>?> GetPostsForProfile(long profile_id, PaginationGetRequest request, long requesterProfile_id) {
            List<PostDTO> posts = new List<PostDTO>();
            var x = config["ProfilePostsPageSize", true];
            posts =
               await db.
               Posts.
               Where(p => p.ID_Author == profile_id).
               OrderByDescending(p => p.DateCreated).
               Skip(config["ProfilePostsPageSize", true] * request.PageNumber).
               Take(config["ProfilePostsPageSize", true]).
               Select(p => new PostDTO() {
                   ID = p.ID,
                   CountLikes = -1, // load below from nosql
                   CountComments = -1, // load below from nosql
               }).ToListAsync();

            // remake ADO to EF at future
            // loading addition data
            foreach ( var p in posts ) {
                p.CountLikes = await likes.GetCountLikers(p.ID);
                p.CountComments = await comments.GetCountComments(p.ID);
                p.RequestProfileIsInLikers = await likes.IsInLikers(requesterProfile_id, p.ID);
            }

            await images.SetPrimaryImagesForPostsUriAsync(posts, profile_id);



            return posts;
        }

        public async Task<IEnumerable<PostDTO>?> GetFeedPostsForProfile(PaginationGetRequest request, long requesterProfile_id) {
            var x = config["ProfilePostsPageSize", true];
            var posts = await followers.GetPostsByFollowings(requesterProfile_id, request);

            if ( posts is null )
                return null;

            // remake ADO to EF at future
            // loading addition data
            foreach ( var p in posts ) {
                p.CountLikes = await likes.GetCountLikers(p.ID);
                p.CountComments = await comments.GetCountComments(p.ID);
                p.RequestProfileIsInLikers = await likes.IsInLikers(requesterProfile_id, p.ID);
            }

            return posts;
        }


        #region Likes
        public async Task<IEnumerable<ShortProfileDTO>?> GetLikers(long post_id, PaginationGetRequest request) {
            var likers = await likes.GetLikers(post_id, request);
            var res = likers?.Join(db.Profiles, l => l, p => p.ID, (l, p) => p).Select(p => new ShortProfileDTO() {
                ProfilePhoto = p.ProfilePhoto,
                FullName = p.FullName,
                NickName = p.NickName,
            });

            return res;
        }

        public async Task<long> GetLikersCount(long post_id) {
            return await likes.GetCountLikers(post_id);
        }

        public async Task<bool> IsInLikers(long post_id, long profile_id) {
            return await likes.IsInLikers(post_id, profile_id);
        }

        public async Task<bool> LikePost(long profile_id, long post_id, bool checkPostExistence = true) {
            return await likes.Like(profile_id, post_id, checkPostExistence);
        }

        public async Task<bool> UnLikePost(long profile_id, long post_id) {
            return await likes.UnLike(profile_id, post_id);
        }


        #endregion

        #region Comments
        public async Task<PostComment?> CommentPost(long profile_id, long post_id, PostCommentCreateRequest request, bool checkPostExistence) {
            return await comments.Comment(post_id, profile_id, request, checkPostExistence);
        }

        public async Task<bool> DeleteCommentPost(long requester_id, string comment_id) {
            return await comments.DeleteComment(requester_id, comment_id);
        }

        public async Task<bool> EditCommentPost(long requester_id, string comment_id, PostCommentEditRequest request) {
            return await comments.EditComment(requester_id, comment_id, request);
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsForPost(long post_id, PaginationGetRequest request) {
            var commentsList = await comments.GetCommentsForPost(post_id, request);

            var res = commentsList?.Join(
                db.Profiles,
                c => c.AuthorID,
                p => p.ID,
                (c, p) => new CommentDTO() {
                    Author = new ProfileDTO() {
                        ProfilePhoto = p.ProfilePhoto,
                        NickName = p.NickName
                    },
                    Date = c.Date,
                    Text = c.Text,
                    ID = c.Id
                });

            return res ?? new List<CommentDTO>();
        }

        public async Task<bool?> DeletePost(long requester_id, long post_id) {
            IDbContextTransaction? currentTransaction = null;
            try {
                currentTransaction = await db.Database.BeginTransactionAsync();

                var post = await db.Posts.FindAsync(post_id);

                if ( post is null )
                    return false;

                if ( post?.ID_Author != requester_id )
                    throw new UnauthorizedAccessException("Unauthorized");

                // delete all referenced documents
                await likes.DeletePostLikes(post_id);
                await comments.DeletePostComments(post_id);

                // mark to delete post with post_id id
                db.Posts.Remove(post);

                var success = await this.db.SaveChangesAsync() > 0;

                await currentTransaction.CommitAsync();

                return success;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);

                if ( currentTransaction is not null )
                    await currentTransaction.RollbackAsync();

                throw;
            }
        }

        public async Task<int?> EditPost(long requester_id, long post_id, PostEditRequest request) {
            IDbContextTransaction? currentTransaction = null;
            try {
                currentTransaction = await db.Database.BeginTransactionAsync();

                var post = await db.Posts.FindAsync(post_id);

                if ( post is null )
                    return null;

                if ( post?.ID_Author != requester_id )
                    throw new UnauthorizedAccessException("Unauthorized");

                var countModifyed = request.EditPost(post);

                await this.db.SaveChangesAsync();

                await currentTransaction.CommitAsync();

                return countModifyed;
            }
            catch ( Exception e ) {
                logger.LogWarning(e.Message);

                if ( currentTransaction is not null )
                    await currentTransaction.RollbackAsync();

                throw;
            }
        }


        #endregion
    }
}