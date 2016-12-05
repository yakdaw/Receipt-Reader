namespace Receipt.API.Authentication
{
    using Entities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    public class AuthRepository : IDisposable
    {
        private AuthContext context;
        private UserManager<IdentityUser> userManager;

        public AuthRepository()
        {
            context = new AuthContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));

            userManager.UserValidator = new UserValidator<IdentityUser>(userManager)
            {
                RequireUniqueEmail = true
            };

            userManager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(Startup.dataProtectionProvider.Create("UserToken"))
            {
                TokenLifespan = TimeSpan.FromHours(3)
            };
        }

        public async Task<IdentityResult> RegisterUser(RegisterModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.Name,
                Email = userModel.Email
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

        public async Task<IdentityUser> FindUserByEmail(string userEmail)
        {
            IdentityUser user = await userManager.FindByEmailAsync(userEmail);
            return user;
        }

        public async Task<string> GeneratePasswordResetToken(IdentityUser user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user.Id);
            return HttpUtility.UrlEncode(token);
        }

        public async Task<IdentityResult> ResetPassword(string id, string token, string password)
        {
            var result = await userManager.ResetPasswordAsync(id, HttpUtility.UrlDecode(token), password);
            return result;
        }

        public async Task<IdentityUser> FindUserByLoginInfo(string userLoginInfo)
        {
            var user = new IdentityUser();

            if (!userLoginInfo.Contains("@"))
            {
                user = await userManager.FindByNameAsync(userLoginInfo);
            }
            else
            {
                user = await userManager.FindByEmailAsync(userLoginInfo);
            }

            return user;
        }

        public Client FindClient(string clientId)
        {
            var client = context.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = context.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            context.RefreshTokens.Add(token);

            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await context.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                context.RefreshTokens.Remove(refreshToken);
                return await context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            context.RefreshTokens.Remove(refreshToken);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await context.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return context.RefreshTokens.ToList();
        }

        public void Dispose()
        {
            context.Dispose();
            userManager.Dispose();
        }
    }
}