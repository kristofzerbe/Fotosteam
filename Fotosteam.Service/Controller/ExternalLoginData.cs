using System;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Fotosteam.Service.Controller
{
    /// <summary>
    ///     Definiert die Daten für die Authentifzierung über einen externen Provider wie z.B. Google
    /// </summary>
    public class ExternalLoginData
    {
        /// <summary>
        ///     Name des Providers
        ///     gültig sind Google, Facebook, Twitter und Microsoft
        /// </summary>
        public string LoginProvider { get; private set; }

        /// <summary>
        ///     Das ist die ID des Benutzers beim Provider
        /// </summary>
        public string ProviderKey { get; private set; }

        /// <summary>
        ///     Definiert den Namen des Benutzers
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        ///     Definiert die Emailadresse des Benutzers.
        ///     Die Emailadresse wird intern als Benutzername genutzt, da sie eindeutig einen Benutzer beschreibt
        /// </summary>
        public string EmailAddress { get; private set; }

        /// <summary>
        ///     Erzeugt ein <see cref="ExternalLoginData" /> anhand des Claims, das von dem Provider übergeben wird
        /// </summary>
        /// <param name="identity">Die Claims-Informationen des Providers für den Benutzer</param>
        /// <returns></returns>
        public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
        {
            if (identity == null)
            {
                return null;
            }

            var providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) ||
                String.IsNullOrEmpty(providerKeyClaim.Value))
            {
                return null;
            }

            if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
            {
                return null;
            }

            return new ExternalLoginData
            {
                LoginProvider = providerKeyClaim.Issuer,
                ProviderKey = providerKeyClaim.Value,
                UserName = identity.FindFirstValue(ClaimTypes.Name),
                EmailAddress = identity.FindFirstValue(ClaimTypes.Email)
            };
        }
    }
}