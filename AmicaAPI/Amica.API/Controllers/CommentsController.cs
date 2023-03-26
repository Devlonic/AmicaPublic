using Amica.API.Data;
using Amica.API.Data.Repositories;
using Amica.API.WebServer.Data.DTO.Posts;
using Amica.API.WebServer.Data.Repositories.Profiles;
using Amica.API.WebServer.Data.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Amica.API.WebServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase {
        private readonly IPostsRepository posts;
        private readonly IProfilesRepository profiles;
        private readonly ILogger<CommentsController> logger;

        public CommentsController(IPostsRepository posts, ILogger<CommentsController> logger, IProfilesRepository profiles) {
            this.posts = posts;
            this.profiles = profiles;
            this.logger = logger;
        }

        #region Controller Services
        private long GetProfileByJwt(IEnumerable<Claim> claims) {
            var userId = long.Parse(claims.Single(c => {
                return c.Type == JwtClaims.TokenIdentity;
            })?.Value ?? null!);
            return userId;
        }
        #endregion

        #region Posts
        /// <summary>
        /// Get comments for post by post_id with pagination
        /// </summary>
        /// <param name="post_id"></param>
        /// <param name="request"></param>
        /// <response code="200">Success</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpGet]
        [Route("Post/{post_id}")]
        public async Task<IActionResult> GetComments([FromRoute] long post_id, [FromQuery] PaginationGetRequest request) {
            try {
                var res = await posts.GetCommentsForPost(post_id, request);
                return Ok(res);
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Add comment to post by post_id and identity token
        /// </summary>
        /// <param name="post_id"></param>
        /// <param name="request"></param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="403">Any user problems with request (post not found, etc...)</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpPost]
        [Authorize(Roles = AmicaRolesSettings.User)]
        [Route("Post/{post_id}")]
        public async Task<IActionResult> Comment([FromForm] PostCommentCreateRequest request, long post_id) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var comment = await posts.CommentPost(userId, post_id, request, true);

                if ( comment is not null )
                    return Ok(comment);
                else
                    return UnprocessableEntity();
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Delete comment by comment_id and identity token
        /// </summary>
        /// <param name="comment_id"></param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="403">Any user problems with request (comment not found, user is not own this comment, etc...)</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpDelete]
        [Authorize(Roles = AmicaRolesSettings.User)]
        [Route("Post/{comment_id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] string comment_id) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var isSuccess = await posts.DeleteCommentPost(userId, comment_id);

                if ( isSuccess )
                    return Ok();
                else
                    return UnprocessableEntity();
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Update comment by comment_id and identity token
        /// </summary>
        /// <param name="comment_id"></param>
        /// <param name="request"></param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="403">Any user problems with request (comment not found, user is not own this comment, etc...)</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpPatch]
        [Authorize(Roles = AmicaRolesSettings.User)]
        [Route("Post/{comment_id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] string comment_id, [FromForm] PostCommentEditRequest request) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var isSuccess = await posts.EditCommentPost(userId, comment_id, request);

                if ( isSuccess )
                    return Ok();
                else
                    return Forbid();
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }
        #endregion
    }
}
