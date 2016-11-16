namespace Receipt.API.Providers
{
    using Authentication;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security.OAuth;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            using (AuthRepository repository = new AuthRepository())
            {
                IdentityUser user = await repository.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                identity.AddClaim(new Claim("id", user.Id));
                identity.AddClaim(new Claim("name", user.UserName));
            }

            context.Validated(identity);
        }
    }
}