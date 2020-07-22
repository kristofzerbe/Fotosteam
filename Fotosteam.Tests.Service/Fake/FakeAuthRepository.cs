using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fotosteam.Tests.Service.Fake
{
    public class FakeAuthRepository : IAuthRepository
    {

        //private static readonly Claim CurrentClaim = new Claim(ClaimTypes.NameIdentifier, "114664602030178284885", ClaimValueTypes.String, "Google");
        private static IdentityUser _currentIdentity;
        private static IdentityUser CurrentIdentity
        {
            get
            {

                if (_currentIdentity == null)
                {
                    _currentIdentity = new IdentityUser
                    {
                        Id = "c3d71ec7-e492-4b1d-85e1-55e1d16f7c00",
                        UserName = "bobbakos@gmail.com",
                        Email = "bobbakos@gmail.com",

                    };

                }
                return _currentIdentity;

            }
        }


        public Task<bool> AddRefreshToken(RefreshToken token)
        {
            return Task.FromResult(true);
        }

        public Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            return Task.FromResult(true);
        }

        public Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            return Task.FromResult(true);
        }

        public Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            return Task.FromResult(new RefreshToken());
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return new List<RefreshToken>();
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            return Task.FromResult(CurrentIdentity);
        }

        public IdentityUser Find(UserLoginInfo loginInfo)
        {
            return TestAuthFilter.UseAuthenticatedUser ? CurrentIdentity : null;
        }

        public IdentityResult Delete(IdentityUser user)
        {
            return new IdentityResult();
        }

        public IdentityUser FindByName(string userName)
        {
            return TestAuthFilter.UseAuthenticatedUser ? CurrentIdentity : null;
        }

        public IdentityUser FindByEmail(string email)
        {
            return TestAuthFilter.UseAuthenticatedUser ? CurrentIdentity : null;
        }

        public Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            return Task.FromResult(new IdentityResult());
        }

        public Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string authenticationType)
        {
            return Task.FromResult(new ClaimsIdentity());
        }

        public Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            return Task.FromResult(new IdentityResult());
        }

        public void Dispose()
        { }
    }
}
