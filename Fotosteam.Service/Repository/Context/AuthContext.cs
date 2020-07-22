using System.Data.Entity;
using Fotosteam.Service.Repository.Poco;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fotosteam.Service.Repository.Context
{
    /// <summary>
    /// Definiert den Kontext für die Authentifizierung über das Entityframework
    /// </summary>
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("FotosteamDbContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AuthContext>());
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}