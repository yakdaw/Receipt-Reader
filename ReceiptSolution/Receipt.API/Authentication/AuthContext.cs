namespace Receipt.API.Authentication
{
    using Entities;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;

    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("ReceiptReaderIdentityContext")
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}