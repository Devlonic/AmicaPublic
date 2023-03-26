using Amica.API.Data;
using Amica.API.WebServer.Data.DTO;
using Amica.API.WebServer.Data.DTO.Posts;
using Amica.API.WebServer.Data.Repositories.Followers;
using Amica.API.WebServer.Data.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Amica.API.WebServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FollowersController : ControllerBase {
        private readonly ILogger<FollowersController> logger;
        private readonly IFollowersRepository followers;

        public FollowersController(ILogger<FollowersController> logger, IFollowersRepository followers) {
            this.logger = logger;
            this.followers = followers;
        }

        #region Controller Services
        private long GetProfileByJwt(IEnumerable<Claim> claims) {
            var userId = long.Parse(claims.Single(c => {
                return c.Type == JwtClaims.TokenIdentity;
            })?.Value ?? null!);
            return userId;
        }
        #endregion

        /// <summary>
        /// Follow by profile_id and identity token
        /// </summary>
        /// <param name="profile_id"></param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="403">Any user problems with request (Already followed, profile not found, etc...)</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpPost]
        [Authorize(Roles = AmicaRolesSettings.User)]
        [Route("{profile_id}/Follow")]
        public async Task<IActionResult> Follow([FromRoute] long profile_id) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var isSuccess = await followers.Follow(userId, profile_id, true);

                if ( isSuccess )
                    return Ok();
                else
                    return Conflict();
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// UnFollow by profile_id and identity token
        /// </summary>
        /// <param name="profile_id"></param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="403">Any user problems with request (Not followed, profile not found, etc...)</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpPost]
        [Authorize(Roles = AmicaRolesSettings.User)]
        [Route("{profile_id}/UnFollow")]
        public async Task<IActionResult> UnFollow([FromRoute] long profile_id) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var isSuccess = await followers.UnFollow(userId, profile_id, true);

                if ( isSuccess )
                    return Ok();
                else
                    return Conflict();
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Get followers by profile id with pagination
        /// </summary>
        /// <param name="profile_id"></param>
        /// <param name="request"></param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="404">Not found problems (profile not found, etc...)</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpGet]
        [Authorize(Roles = AmicaRolesSettings.User)]
        [Route("{profile_id}/Followers")]
        public async Task<IActionResult> GetFollowers([FromRoute] long profile_id, [FromQuery] PaginationGetRequest request) {
            try {
                var result = await followers.GetFollowers(profile_id, request);

                if ( result is not null )
                    return Ok(result);
                else
                    return NotFound();
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Get followings by profile id with pagination
        /// </summary>
        /// <param name="profile_id"></param>
        /// <param name="request"></param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthenticated request (Wrong, Missing or Expired token)</response>
        /// <response code="404">Not found problems (profile not found, etc...)</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpGet]
        [Authorize(Roles = AmicaRolesSettings.User)]
        [Route("{profile_id}/Followings")]
        public async Task<IActionResult> GetFollowings([FromRoute] long profile_id, [FromQuery] PaginationGetRequest request) {
            try {
                var result = await followers.GetFollowings(profile_id, request);

                if ( result is not null )
                    return Ok(result);
                else
                    return NotFound();
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }
    }
}
