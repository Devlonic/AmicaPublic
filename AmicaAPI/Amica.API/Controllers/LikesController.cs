using Amica.API.Data.DTO.Posts;
using Amica.API.Data.Repositories;
using Amica.API.Data;
using Amica.API.WebServer.Data.DTO.Posts;
using Amica.API.WebServer.Data.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Amica.API.WebServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase {
        private readonly IPostsRepository posts;
        private readonly ILogger<LikesController> logger;

        public LikesController(IPostsRepository posts, ILogger<LikesController> logger) {
            this.posts = posts;
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
        /// Like post by post_id and identity token
        /// </summary>
        /// <param name="post_id"></param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="403">Any user problems with request (Already liked, post not found, etc...)</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpPost]
        [Authorize(Roles = AmicaRolesSettings.User)]
        [Route("Post/{post_id}/Like")]
        public async Task<IActionResult> Like(long post_id) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var isSuccess = await posts.LikePost(userId, post_id, true);

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

        /// <summary>
        /// UnLike post by post_id and identity token
        /// </summary>
        /// <param name="post_id"></param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="403">Any user problems with request (Not liked, post not found, etc...)</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpPost]
        [Authorize(Roles = AmicaRolesSettings.User)]
        [Route("Post/{post_id}/Unlike")]
        public async Task<IActionResult> UnLike(long post_id) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var isSuccess = await posts.UnLikePost(userId, post_id);

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

        /// <summary>
        /// Get likers of post by post_id
        /// </summary>
        /// <param name="post_id"></param>
        /// <param name="request"></param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="403">Any user problems with request (Not liked, post not found, etc...)</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpGet]
        [Route("Post/{post_id}/Likers")]
        public async Task<IActionResult> GetLikersForPost(long post_id, [FromQuery] PaginationGetRequest request) {
            try {
                var likers = await posts.GetLikers(post_id, request);

                return Ok(likers);
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }
        #endregion
    }
}
