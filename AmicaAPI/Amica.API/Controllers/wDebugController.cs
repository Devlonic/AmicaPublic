using Amica.API.Data.Repositories;
using Amica.API.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Amica.API.Data.Models;
using Amica.API.WebServer.Data.Settings;
using Amica.API.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Amica.API.WebServer.Data.Repositories.Comments;
using Amica.API.WebServer.Data.Repositories.Likes;
using Amica.API.WebServer.Data.Repositories.Profiles;
using Amica.API.WebServer.Data.Repositories.Followers;

namespace Amica.API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class wDebugController : ControllerBase {
        private readonly IAccountsRepository accounts;
        private readonly IProfilesRepository profiles;
        private readonly IFollowersRepository followers;
        private readonly IPostsRepository posts;
        private readonly IImagesRepository images;
        private readonly AmicaDbContext sql;
        private readonly ILikesRepository likes;
        private readonly ICommentsRepository comments;
        private readonly ILogger<wDebugController> logger;

        public wDebugController(IAccountsRepository accounts, IPostsRepository posts, IImagesRepository images, AmicaDbContext sql, ILikesRepository likes, ICommentsRepository comments, ILogger<wDebugController> logger, IProfilesRepository profiles, IFollowersRepository followers) {
            this.accounts = accounts;
            this.posts = posts;
            this.images = images;
            this.sql = sql;
            this.likes = likes;
            this.comments = comments;
            this.logger = logger;
            this.profiles = profiles;
            this.followers = followers;
        }


        /// <summary>
        /// WARNING!
        /// Not for production!
        /// Drop sql database and create new (All data erased)
        /// Drop all Mongo database collections
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpDelete]
        public async Task<ActionResult> WipeAll() {
            try {
                string result = "";
                result += $"sql database rebuild success: {await sql.RebuildDatabaseAsync()}\n";
                result += "likes deleted: " + ( await likes.ClearAll() ).DeletedCount + "\n";
                result += "comments deleted: " + ( await comments.ClearAll() ).DeletedCount + "\n";
                return Ok(result);
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// WARNING!
        /// Not for production!
        /// Creates is not exists default Amica roles (User, Admin, Moderator, etc... (Watch AmicaRolesSettings) )
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpPost]
        public async Task<ActionResult> CreateRoles() {
            try {
                int countCreated = 0;

                if ( ( await accounts.CreateRole(AmicaRolesSettings.User) ).Succeeded == true )
                    countCreated++;

                if ( ( await accounts.CreateRole(AmicaRolesSettings.Admin) ).Succeeded == true )
                    countCreated++;

                return Ok("created " + countCreated);
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// WARNING!
        /// Not for production!
        /// Seeding data example data
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpPost]
        public async Task<ActionResult> Fill() {
            try {
                var magicNumber = Random.Shared.Next();

                var img1Stream = System.IO.File.OpenRead(@"Pics/a.png");
                var img2Stream = System.IO.File.OpenRead(@"Pics/b.jpg");
                var img3Stream = System.IO.File.OpenRead(@"Pics/c.jpg");
                var img4Stream = System.IO.File.OpenRead(@"Pics/d.jpg");
                var img1 = new FormFile(img1Stream, 0, img1Stream.Length, "ProfilePhoto", "a.png");
                var img2 = new FormFile(img2Stream, 0, img2Stream.Length, "ProfilePhoto", "b.jpg");
                var img3 = new FormFile(img3Stream, 0, img3Stream.Length, "ProfilePhoto", "c.jpg");
                var img4 = new FormFile(img4Stream, 0, img4Stream.Length, "ProfilePhoto", "d.jpg");
                //"form-data; name=\"ProfilePhoto\"; filename=\"a.png\""
                //"image/png"

                img1.Headers = new HeaderDictionary();
                img2.Headers = new HeaderDictionary();
                img3.Headers = new HeaderDictionary();
                img4.Headers = new HeaderDictionary();

                img1.ContentType = "image/png";
                img2.ContentType = "image/png";
                img3.ContentType = "image/png";
                img4.ContentType = "image/png";

                img1.ContentDisposition = "form-data; name=\"ProfilePhoto\"; filename=\"a.png\"";
                img2.ContentDisposition = "form-data; name=\"ProfilePhoto\"; filename=\"b.jpg\"";
                img3.ContentDisposition = "form-data; name=\"ProfilePhoto\"; filename=\"c.jpg\"";
                img4.ContentDisposition = "form-data; name=\"ProfilePhoto\"; filename=\"d.jpg\"";

                int countSuccess = 0;
                int countFails = 0;

                List<long?> profile_ids = new List<long?>();
                List<long?> post_ids = new List<long?>();

                var defAcc = await accounts.SignUpAccount(new SignUpRequest() {
                    Email = $"defaultusername@gmail.com",
                    FullName = $"Default Userfullname",
                    Nickname = $"defaultusername",
                    Password = "KAKAkika1234",
                    ProfilePhoto = img2,
                }, AmicaRolesSettings.RegUser, false);
                profile_ids.Add(defAcc.ProfileID);
                var post1 = await posts.CreatePost(defAcc.ProfileID ?? -1, new Data.DTO.Posts.PostCreateRequest() {
                    Title = $"Test post _1 from user {defAcc.ProfileID}",
                    Images = new List<IFormFile>() {
                        img1,
                        img3,
                        img4,
                    }
                });

                if ( post1 is not null )
                    countSuccess++;
                else
                    countFails++;
                var post2 = await posts.CreatePost(defAcc.ProfileID ?? -1, new Data.DTO.Posts.PostCreateRequest() {
                    Title = $"Test post _2 from user {defAcc.ProfileID}",
                    Images = new List<IFormFile>() {
                        img1,
                        img3,
                        img4,
                    }
                });

                if ( post2 is not null )
                    countSuccess++;
                else
                    countFails++;



                for ( int i = 0; i < 100; i++ ) {
                    // create 100 accounts
                    var account = await accounts.SignUpAccount(new SignUpRequest() {
                        Email = $"user{i}_{magicNumber}@gmail.com",
                        FullName = $"Ivan Smith {i}_{magicNumber}",
                        Nickname = $"user{i}_{magicNumber}",
                        Password = "KAKAkika1234",
                        ProfilePhoto = ( new[] {
                            img1,
                            img2,
                            img3,
                            img4,
                        } )[Random.Shared.Next(4)]
                    }, AmicaRolesSettings.RegUser, false);
                    profile_ids.Add(account.ProfileID);

                    if ( account.IsSignedUp is true )
                        countSuccess++;
                    else
                        countFails++;

                    // only each 3 profile have 3 posts
                    if ( Random.Shared.Next(3) == 0 ) {
                        // create 3 posts for current created profile
                        for ( int j = 0; j < 3; j++ ) {
                            var post = await posts.CreatePost(account.ProfileID ?? -1, new Data.DTO.Posts.PostCreateRequest() {
                                Title = $"Test post {j} from user {account.ProfileID}",
                                Images = new List<IFormFile>() {
                                img1,
                                img3,
                                img4,
                            }
                            });

                            if ( post is not null )
                                countSuccess++;
                            else
                                countFails++;

                            post_ids.Add(post?.ID);
                        }
                    }

                }

                // random follows 
                // possible failture, so ignore fails because here
                // is random followers and random whom follow
                for ( int i = 0; i < 5000; i++ ) {
                    var who = profile_ids[Random.Shared.Next(profile_ids.Count)];
                    var whom = profile_ids[Random.Shared.Next(profile_ids.Count)];

                    var status = await followers.Follow(who.Value, whom.Value, false);
                    if ( status is true )
                        countSuccess++;
                    else
                        countFails++;
                }

                // create (count_users)*10 random comments to present posts from present random profiles
                for ( long? i = 0,
                    curProfile = profile_ids[Random.Shared.Next(profile_ids.Count)],
                    curPost = post_ids[Random.Shared.Next(post_ids.Count)];
                    i < profile_ids.Count * 10;
                    i++,
                    curProfile = profile_ids[Random.Shared.Next(profile_ids.Count)],
                    curPost = post_ids[Random.Shared.Next(post_ids.Count)] ) {
                    var status = await comments.Comment(curPost.Value, curProfile.Value, new WebServer.Data.DTO.Posts.PostCommentCreateRequest() {
                        CommentText =
                            Random.Shared.Next(2) == 0 ?
                            $"Cool post {curPost}. I am {curProfile} and i like it!" :
                            $"Crap post {curPost}. I am {curProfile} and i hate it!"
                    }, true);

                    if ( status is not null )
                        countSuccess++;
                    else
                        countFails++;
                }

                // create (count_users)*20 (max. min is not defined) likes from random users to random posts
                for ( long? i = 0,
                    curProfile = profile_ids[Random.Shared.Next(profile_ids.Count)],
                    curPost = post_ids[Random.Shared.Next(post_ids.Count)];
                    i < profile_ids.Count * 20;
                    i++,
                    curProfile = profile_ids[Random.Shared.Next(profile_ids.Count)],
                    curPost = post_ids[Random.Shared.Next(post_ids.Count)]
                    ) {
                    // possible like failture, so ignore fails because here
                    // is random likers and random posts
                    var status = await likes.Like(curProfile.Value, curPost.Value, true);

                    if ( status is true )
                        countSuccess++;
                }

                return Ok(@$"{{""success"": {countSuccess}, ""fails"": {countFails}}}");
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }
    }
}
