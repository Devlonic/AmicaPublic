using Amica.API.Data;
using Amica.API.Data.DTO.Posts;
using Amica.API.Data.Models;
using Amica.API.Data.Repositories;
using Amica.API.WebServer.Controllers.Attributes;
using Amica.API.WebServer.Data.DTO.Posts;
using Amica.API.WebServer.Data.Repositories.Profiles;
using Amica.API.WebServer.Data.Services;
using Amica.API.WebServer.Data.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Attributes;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Principal;

namespace Amica.API.WebServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase {
        private readonly IPostsRepository posts;
        private readonly ILogger<PostsController> logger;
        private readonly AccountThrottleService throttle;
        private readonly IAccountsRepository accounts;

        public PostsController(IPostsRepository posts, ILogger<PostsController> logger, AccountThrottleService throttle, IAccountsRepository accounts) {
            this.posts = posts;
            this.logger = logger;
            this.throttle = throttle;
            this.accounts = accounts;
        }

        #region Controller Services
        private long GetProfileByJwt(IEnumerable<Claim> claims) {
            var userId = long.Parse(claims.Single(c => {
                return c.Type == JwtClaims.TokenIdentity;
            })?.Value ?? null!);
            return userId;
        }
        private string GetAccountByJwt(IEnumerable<Claim> claims) {
            var userId = claims.Single(c => {
                return c.Type == JwtClaims.TokenIdentityAccount;
            }).Value;
            return userId;
        }
        private IActionResult TooManyRequests(string account_id) {
            return StatusCode(429, $"Too many requests per day for {account_id}");
        }
        #endregion

        #region Selectors
        /// <summary>
        /// Get full post by post_id
        /// </summary>
        /// <param name="post_id"></param>
        /// <response code="200">No problem</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="404">Post not found</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpGet]
        [Route("{post_id}")]
        [Authorize(Roles = AmicaRolesSettings.User)]
        public async Task<IActionResult> GetFull(long post_id) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var post = await posts.GetFullInfoPost(post_id, userId);
                if ( post is null )
                    return NotFound();
                return Ok(post);
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Get reduced posts by profile_id with pagination
        /// </summary>
        /// <param name="profile_id"></param>
        /// <param name="request"></param>
        /// <response code="200">No problem</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="404">Post not found</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpGet]
        [Route("GetByProfile/{profile_id}")]
        [Authorize(Roles = AmicaRolesSettings.User)]
        public async Task<IActionResult> GetByProfilePagination([FromRoute] long profile_id, [FromQuery] PaginationGetRequest request) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var postsList = await posts.GetPostsForProfile(profile_id, request, userId);
                if ( postsList is null || postsList.Count == 0 )
                    return NotFound();
                return Ok(postsList);
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Get reduced posts by profile_id by following for feed with pagination
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">No problem</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="404">Post not found</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpGet]
        [Route("Feeds")]
        [Authorize(Roles = AmicaRolesSettings.User)]
        public async Task<IActionResult> GetByProfileAndFollowingsPagination([FromQuery] PaginationGetRequest request) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var postsList = await posts.GetFeedPostsForProfile(request, userId);
                return Ok(postsList);
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }
        #endregion

        #region Actions
        /// <summary>
        /// Create post by identity token
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">Post created</response>
        /// <response code="409">Post create failed by user. No return</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpPost]
        [Authorize(Roles = AmicaRolesSettings.User)]
        public async Task<IActionResult> Create([FromForm] PostCreateRequest request) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);
                var accountId = GetAccountByJwt(User.Claims);

                if ( !await throttle.CanAct(accountId) )
                    return TooManyRequests(accountId);

                var created = await posts.CreatePost(userId, request);
                if ( created is null )
                    return Conflict();

                return Created(new Uri($"https://{HttpContext.Request.Host}/api/Posts/{created?.ID}"), new PostCreateResponce() {
                    Id = created?.ID ?? throw new Exception("Cannot create post")
                });
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Delete post by post_id
        /// </summary>
        /// <param name="post_id"></param>
        /// <response code="200">Post deleted</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="403">Unauthorized request (User is not owner of requested post)</response>
        /// <response code="404">Post with id post_id not found</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpDelete]
        [Route("{post_id}")]
        [Authorize(Roles = AmicaRolesSettings.User)]
        public async Task<IActionResult> Delete(long post_id) {
            // get profile id from JWT token
            var userId = GetProfileByJwt(User.Claims);

            try {
                var deletedStatus = await posts.DeletePost(userId, post_id);
                if ( deletedStatus is false )
                    return NotFound();

                return Ok();
            }
            catch ( UnauthorizedAccessException ) {
                return Forbid();
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Edit post by post_id
        /// </summary>
        /// <param name="post_id"></param>
        /// <param name="request"></param>
        /// <response code="200">Post edited</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="403">Unauthorized request (User is not owner of requested post)</response>
        /// <response code="404">Post with id post_id not found</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpPatch]
        [Route("{post_id}")]
        [Authorize(Roles = AmicaRolesSettings.User)]
        public async Task<IActionResult> Edit(long post_id, [FromForm] PostEditRequest request) {
            // get profile id from JWT token
            var userId = GetProfileByJwt(User.Claims);

            try {
                var editStatus = await posts.EditPost(userId, post_id, request);
                if ( editStatus is null )
                    return NotFound();

                return Ok($"{editStatus} fields modifyed");
            }
            catch ( UnauthorizedAccessException ) {
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