using System;
using System.Configuration;
using Fotosteam.Service.Providers;
using Google.Apis.Json;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Twitter;
using Newtonsoft.Json;
using Owin;

namespace Fotosteam.Service
{
    /// <summary>
    ///     Die Klasse initialisiert die WebAPI mit den notwendigen Informationen für Routing und Authentifzierung
    /// </summary>
    public partial class Startup
    {
        internal static string MicrosoftClientId;
        internal static string MicrosoftClientSecret;
        internal static string TwitterClientId;
        internal static string TwitterClientSecret;
        internal static string FacebookClientId;
        internal static string FacebookClientSecret;
        internal static string GoogleClientId;
        internal static string GoogleClientSecret;
        internal static string DropboxAppKey;
        internal static string DropboxAppSecret;
        internal static int NumberOfInviteCodesForNewMembers = 5;

        // Enable the application to use OAuthAuthorization. You can then secure your Web APIs
        static Startup()
        {
            SetupIds();

        }

        private static void SetupIds()
        {
            if (!string.IsNullOrEmpty(MicrosoftClientId))
                return;

            PublicClientId = "fotosteam";
            ReadConfiguration();
        }

        private static void ReadConfiguration()
        {
            MicrosoftClientId = ConfigurationManager.AppSettings["MicrosoftClientId"];
            MicrosoftClientSecret = ConfigurationManager.AppSettings["MicrosoftClientSecret"];
            TwitterClientId = ConfigurationManager.AppSettings["TwitterClientId"];
            TwitterClientSecret = ConfigurationManager.AppSettings["TwitterSecret"];
            FacebookClientId = ConfigurationManager.AppSettings["FacebookClientId"];
            FacebookClientSecret = ConfigurationManager.AppSettings["FacebookClientSecret"];
            GoogleClientId = ConfigurationManager.AppSettings["GoogleClientId"];
            GoogleClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"];
            DropboxAppKey = ConfigurationManager.AppSettings["DropboxAppKey"];
            DropboxAppSecret = ConfigurationManager.AppSettings["DropboxAppSecret"];
            NumberOfInviteCodesForNewMembers = Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfInviteCodesForNewMembers"]);
        }

        /// <summary>
        /// Definiert die ID für die Anwendung
        /// </summary>
        /// <remarks>
        /// Wird extern nicht verwendet
        /// </remarks>
        public static string PublicClientId { get; private set; }

        /// <summary>
        /// Fügt die Authentifzierungsmöglichkeiten der Anwendung hinzu
        /// </summary>
        /// <param name="app">Das Appbuilder-Objekt, dass der Anwendung zugrunde liegt</param>
        /// <remarks>For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864</remarks>
        public void ConfigureAuth(IAppBuilder app)
        {

            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ApplicationCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            var OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60.0),
                AuthorizationCodeExpireTimeSpan = TimeSpan.FromMinutes(60.0),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider(),
                SystemClock = new SystemClock(),
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            //Cookie
            //Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                AuthenticationMode = AuthenticationMode.Active,
                ExpireTimeSpan = TimeSpan.FromHours(2.0),
                SlidingExpiration = true,
                SystemClock = new SystemClock(),
                CookieName = "Fotosteam_session",
                LoginPath = new PathString("/start") //INFO:Das eventuell ändnern, wenn weiterhin ein Anmeldungsdialog erscheint

            });


            app.UseMicrosoftAccountAuthentication(MicrosoftClientId, MicrosoftClientSecret);
            app.UseTwitterAuthentication(new TwitterAuthenticationOptions
            {
                ConsumerKey = TwitterClientId,
                ConsumerSecret = TwitterClientSecret,
                BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(
                new[]
                {
                "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
                "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
                "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
                "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
                "4eb6d578499b1ccf5f581ead56be3d9b6744a5e5", // VeriSign Class 3 Primary CA - G5
                "5168FF90AF0207753CCCD9656462A212B859723B", // DigiCert SHA2 High Assurance Server C‎A 
                "B13EC36903F8BF4701D498261A0802EF63642BC3" // DigiCert High Assurance EV Root CA
                })
            });




            app.UseFacebookAuthentication(FacebookClientId, FacebookClientSecret);

            var googlePlusOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = GoogleClientId,
                ClientSecret = GoogleClientSecret
            };

            app.UseGoogleAuthentication(googlePlusOptions);
        }
    }
}