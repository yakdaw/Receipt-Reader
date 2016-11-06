namespace Receipt.API.Controllers
{
    using Authentication;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OAuth;
    using Models;
    using Services;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private AuthRepository repository = null;
        private ResponseService responseService = null;

        public AccountController()
        {
            repository = new AuthRepository();
            responseService = new ResponseService();
        }

        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterModel user)
        {
            if (!ModelState.IsValid)
            {
                var message = responseService.ModelStateErrorsToString(ModelState);
                return BadRequest(message);
            }

            IdentityResult result = await repository.RegisterUser(user);

            if (!result.Succeeded)
            {
                var message = responseService.IdentityResultErrorsToString(result.Errors);
                return BadRequest(message);
            }

            return Ok();
        }

        [AllowAnonymous]
        [Route("LostPassword")]
        public async Task<IHttpActionResult> LostPassword(string userName)
        {
            if (!ModelState.IsValid)
            {
                var message = responseService.ModelStateErrorsToString(ModelState);
                return BadRequest(message);
            }

            IdentityUser user = await repository.FindUserByName(userName);

            if (user == null)
            {
                return BadRequest();
            }

            var tokenExpiration = TimeSpan.FromHours(6);
            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);
            identity.AddClaim(new Claim("name", userName));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthServerOptions.AccessTokenFormat.Protect(ticket);

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
