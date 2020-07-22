using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fotosteam.Service.Repository.Poco;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fotosteam.Service.Repository
{
    public interface IAuthRepository
    {
        Task<bool> AddRefreshToken(RefreshToken token);
        Task<bool> RemoveRefreshToken(string refreshTokenId);
        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> FindRefreshToken(string refreshTokenId);
        List<RefreshToken> GetAllRefreshTokens();
        Task<IdentityUser> FindAsync(UserLoginInfo loginInfo);
        IdentityUser Find(UserLoginInfo loginInfo);
        IdentityResult Delete(IdentityUser user);
        IdentityUser FindByName(string userName);
        IdentityUser FindByEmail(string email);
        Task<IdentityResult> CreateAsync(IdentityUser user);
        Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string authenticationType);
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
        void Dispose();
    }
}