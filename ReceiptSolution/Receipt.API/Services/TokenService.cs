namespace Receipt.API.Services
{
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OAuth;
    using System;
    using System.Security.Claims;

    public class TokenService
    {
        public string CreateAccessToken(string userName, int time)
        {
            var tokenExpiration = TimeSpan.FromHours(time);
            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);
            identity.AddClaim(new Claim("name", userName));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthServerOptions.AccessTokenFormat.Protect(ticket);

            return accessToken;
        }
    }
}