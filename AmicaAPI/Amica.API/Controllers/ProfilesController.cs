using Amica.API.Data;
using Amica.API.Data.Repositories;
using Amica.API.WebServer.Data.Repositories.Profiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Amica.API.WebServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase {
        private readonly IProfilesRepository profiles;
        private readonly ILogger<ProfilesController> logger;

        public ProfilesController(IProfilesRepository profiles, ILogger<ProfilesController> logger) {
            this.profiles = profiles;
            this.logger = logger;
        }

        #region Controller Services
        private long? GetProfileByJwt(IEnumerable<Claim> claims) {
            var raw = claims.SingleOrDefault(c => {
                return c.Type == JwtClaims.TokenIdentity;
            })?.Value;

            if ( raw is null )
                return null;

            var userId = long.Parse(raw);
            return userId;
        }
        #endregion

        /// <summary>
        /// Get profile by profile_nickname
        /// </summary>
        /// <param name="profile_nickname"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Profile not found</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpGet]
        [Route("nickname/{profile_nickname}")]
        public async Task<IActionResult> GetProfileByNickName([FromRoute] string profile_nickname) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var res = await profiles.GetProfileByNickNameAsync(profile_nickname, userId ?? -1);
                if ( res is null )
                    return NotFound();
                return Ok(res);
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Match profiles by part
        /// </summary>
        /// <param name="part"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Profiles not found</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpGet]
        [Route("find/{part}")]
        public async Task<IActionResult> FindListByNickNameAsync([FromRoute] string part) {
            try {
                // get profile id from JWT token
                //var userId = GetProfileByJwt(User.Claims);

                var res = await profiles.MatchProfilesByNickNameAsync(part);
                if ( res is null )
                    return NotFound();
                return Ok(res);
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Get profile by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Success</response>
        /// <response code="404">Profile not found</response>
        /// <response code="500">Any server problem. Until release returns Exception text</response>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProfileById([FromRoute] long id) {
            try {
                // get profile id from JWT token
                var userId = GetProfileByJwt(User.Claims);

                var res = await profiles.GetProfileByIdAsync(id, userId ?? -1);
                if ( res is null )
                    return NotFound();
                return Ok(res);
            }
            catch ( Exception e ) {
                logger.LogError(e, "Unexpected server error");
                return Problem(e.Message, null, 500);
            }
        }
    }
}
