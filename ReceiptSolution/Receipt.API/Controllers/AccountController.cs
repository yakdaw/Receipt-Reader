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
        private EmailService emailService = null;
        private ResponseService responseService = null;
        private TokenService tokenService = null;

        public AccountController()
        {
            repository = new AuthRepository();
            emailService = new EmailService();
            responseService = new ResponseService();
            tokenService = new TokenService();
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

            var accessToken = tokenService.CreateAccessToken(userName, 6);

            emailService.SendLostPasswordMail(user.Email, accessToken);

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
