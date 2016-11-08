namespace Receipt.API.Authentication
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Models;
    using System;
    using System.Threading.Tasks;

    public class AuthRepository : IDisposable
    {
        private AuthContext context;
        private UserManager<IdentityUser> userManager;

        public AuthRepository()
        {
            context = new AuthContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));

            userManager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(Startup.dataProtectionProvider.Create("UserToken"))
            {
                TokenLifespan = TimeSpan.FromHours(3)
            };
        }

        public async Task<IdentityResult> RegisterUser(RegisterModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            var result = await userManager.CreateAsync(user, userModel.Password);
            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<IdentityUser> FindUserByName(string userName)
        {
            IdentityUser user = await userManager.FindByNameAsync(userName);
            return user;
        }

        public async Task<string> GeneratePasswordResetToken(IdentityUser user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            return token;
        }

        public async Task<IdentityResult> ResetPassword(string id, string token, string password)
        {
            var result = await userManager.ResetPasswordAsync(id, token, password);
            return result;
        }

        public void Dispose()
        {
            context.Dispose();
            userManager.Dispose();
        }
    }
}