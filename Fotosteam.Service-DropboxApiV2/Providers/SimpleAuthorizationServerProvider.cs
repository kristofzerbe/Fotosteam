using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Fotosteam.Service.Providers
{
    /// <summary>
    ///  Kaspelt die Methoden für den OAuth2-Zugriff
    ///  Das kommt aus https://github.com/tjoudeh/AngularJSAuthentication
    /// </summary>
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        ///     Aktualisiert einen Refreshtoken
        /// </summary>
        /// <param name="context">Der Authentifizierungskontext</param>
        /// <returns>Task mit NULL</returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}