using System;
using System.Threading.Tasks;

namespace OneDrive
{
    public interface IAuthenticationInfo
    {
        string AccessToken { get; }

        string RefreshToken { get; }

        string TokenType { get; }

        DateTimeOffset TokenExpiration { get; }

        Task<bool> RefreshAccessTokenAsync();

        string AuthorizationHeaderValue { get; }
    }
}
