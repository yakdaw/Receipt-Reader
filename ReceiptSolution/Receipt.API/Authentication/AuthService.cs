namespace Receipt.API.Authentication
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;

    public class AuthService
    {
        public string GetUserName(IPrincipal user)
        {
            var identity = (ClaimsIdentity)user.Identity;
            IEnumerable<Claim> claims = identity.Claims;

            return claims.FirstOrDefault(x => x.Type == "name").Value;
        }
    }
}