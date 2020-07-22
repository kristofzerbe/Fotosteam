using System.Threading.Tasks;
using Fotosteam.Service.Repository.Poco;
using OneDrive;

namespace Fotosteam.Service.Connector.OneDrive
{
    public static class OAuthAuthenticator
    {
        public static async Task<ODConnection> SignInToMicrosoftAccount(string oldRefreshToken,
            MemberStorageAccess access)
        {
            AppTokenResult appToken = null;
            if (!string.IsNullOrEmpty(oldRefreshToken))
            {
                //  appToken = await MicrosoftAccountOAuth.RedeemRefreshTokenAsync(msa_client_id, msa_client_secret, oldRefreshToken);
            }

            if (null == appToken)
            {
                //appToken = await MicrosoftAccountOAuth.LoginAuthorizationCodeFlowAsync(msa_client_id,
                //   msa_client_secret,
                //  new[] { "wl.offline_access", "wl.basic", "wl.signin", "onedrive.readwrite" });
            }

            if (null != appToken)
            {
                SaveRefreshToken(appToken.RefreshToken);

                return new ODConnection("https://api.onedrive.com/v1.0", new OAuthTicket(appToken));
            }

            return null;
        }

        private static void SaveRefreshToken(string refreshToken)
        {
            if (!string.IsNullOrEmpty(refreshToken))
            {
                // var settings = Properties.Settings.Default;
                // settings.RefreshToken = refreshToken;
                // settings.Save();
            }
        }

        public static async Task<AppTokenResult> RenewAccessTokenAsync(OAuthTicket ticket)
        {
            var oldRefreshToken = ticket.RefreshToken;
            AppTokenResult appToken = null;

            if (!string.IsNullOrEmpty(oldRefreshToken))
            {
                //appToken = await MicrosoftAccountOAuth.RedeemRefreshTokenAsync(msa_client_id, msa_client_secret, oldRefreshToken);
                SaveRefreshToken(appToken.RefreshToken);
            }

            return appToken;
        }
    }
}