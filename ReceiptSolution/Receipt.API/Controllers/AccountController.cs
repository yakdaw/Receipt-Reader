namespace Receipt.API.Controllers
{
    using Authentication;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Services;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private AuthRepository repository = null;
        private EmailService emailService = null;
        private ResponseService responseService = null;
        private TokenService tokenService = null;

        /// <summary>
        /// Account based operations
        /// </summary>
        public AccountController()
        {
            repository = new AuthRepository();
            emailService = new EmailService();
            responseService = new ResponseService();
            tokenService = new TokenService();
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user">User desired name and password.</param>
        /// <response code="200">User successfully registered in database.</response>
        /// <response code="400">Invalid data model. / User with that name is already in database.</response>
        [AllowAnonymous]
        [Route("register")]
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

        /// <summary>
        /// Send reset token
        /// </summary>
        /// <param name="userName">User name to reset its password.</param>
        /// <response code="200">Token successfully sent as response and to user's 
        /// associated mail. Expires after 3 hours.</response>
        /// <response code="400">Invalid data model. / User not found.</response>
        [AllowAnonymous]
        [Route("lostPassword")]
        public async Task<IHttpActionResult> LostPassword(LostPasswordModel lostPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                var message = responseService.ModelStateErrorsToString(ModelState);
                return BadRequest(message);
            }

            IdentityUser user = await repository.FindUserByName(lostPasswordModel.UserName);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var passwordResetToken = await repository.GeneratePasswordResetToken(user);

            emailService.SendLostPasswordMail(user.Email, passwordResetToken);

            return Ok(passwordResetToken);
        }

        /// <summary>
        /// Reset user's password
        /// </summary>
        /// <param name="passwordRecoveryModel">
        /// Model that contains user's name, desired password with its confirmation (same password) 
        /// and reset token from LostPassword method.
        /// </param>
        /// <response code="200">User password was successfully reset.</response>
        /// <response code="500">Invalid data model. / User not found. / Reset token denied.</response>
        [AllowAnonymous]
        [Route("resetPassword")]
        public async Task<IHttpActionResult> ResetPassword(PasswordRecoveryModel passwordRecoveryModel)
        {
            if (!ModelState.IsValid)
            {
                var message = responseService.ModelStateErrorsToString(ModelState);
                return BadRequest(message);
            }

            IdentityUser user = await repository.FindUserByName(passwordRecoveryModel.UserName);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var result = await repository.ResetPassword(user.Id, passwordRecoveryModel.Token, passwordRecoveryModel.Password);

            if (!result.Succeeded)
            {
                var message = responseService.IdentityResultErrorsToString(result.Errors);
                return BadRequest(message);
            }

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
