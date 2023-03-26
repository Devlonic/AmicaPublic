using Amica.API.Data.DTO;
using Amica.API.Data.Models;
using Amica.API.Data.Repositories;
using Amica.API.WebServer.Data.Services;
using Amica.API.WebServer.Data.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amica.API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase {
        private IAccountsRepository accounts;
        private readonly ICaptchaValidatorService captcha;

        public AccountsController(IAccountsRepository accounts, ICaptchaValidatorService captcha) {
            this.accounts = accounts;
            this.captcha = captcha;
        }

        /// <summary>
        /// Transform (login and password) to (identity token)
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">User successfully authenticated and token generated</response>
        /// <response code="422">Authentication failed by user. Return SignUpResponce with explanation</response>
        /// <response code="500">Authentication failed by server. Until release returns Exception text</response>
        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] SignInRequest request) {
            try {
                var responce = await accounts.SignInAccount(request);
                if ( responce.IsSignedIn == false )
                    return UnprocessableEntity(responce);

                return Ok(responce);
            }
            catch ( Exception e ) {
                return Problem(e.Message, null, 500);
            }
        }

        /// <summary>
        /// Register new user (Create account and linked profile)
        /// </summary>
        /// <param name="request"></param>
        /// <response code="200">User successfully registered</response>
        /// <response code="401">Missing captcha token. Return SignUpResponce with explanation</response>
        /// <response code="422">Registration failed by user. Return SignUpResponce with explanation</response>
        /// <response code="500">Registration failed by server. Until release returns Exception text</response>
        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] SignUpRequest request) {
            try {
                var responce = await accounts.SignUpAccount(request, AmicaRolesSettings.RegUser);

                // any user-side problems
                if ( responce.IsSignedUp == false )
                    return UnprocessableEntity(responce);

                // user signed up successfully
                return Ok(responce);
            }
            catch ( Exception e ) {
                // any server-side problems
                return Problem(e.Message, null, 500);
            }

        }
    }
}
