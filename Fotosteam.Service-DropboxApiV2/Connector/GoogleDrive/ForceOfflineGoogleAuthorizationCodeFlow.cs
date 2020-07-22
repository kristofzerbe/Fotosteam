using System;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;

namespace Fotosteam.Service.Connector.GoogleDrive
{
    /// <summary>
    /// Definiert die Flow, um eine Offline-Authorizierung zu ermöglich,
    /// so dass die Anwendung auch ohne dass der Anwender jedesmal den Zugriff bestätigen muss
    /// </summary>
    internal class ForceOfflineGoogleAuthorizationCodeFlow : GoogleAuthorizationCodeFlow
    {
        /// <summary>
        /// Standardkonstruktor
        /// </summary>
        /// <param name="initializer"></param>
        public ForceOfflineGoogleAuthorizationCodeFlow(Initializer initializer) : base(initializer){}

        /// <summary>
        /// Bereitet die Anfrage für den Offlinezugriff vor
        /// </summary>
        /// <param name="redirectUri">Die Url, die von Google nach der Authorizierung aufgerufen werden soll</param>
        /// <returns>Der Request, der an Google geschickt wird</returns>
        public override AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri)
        {
            var requestUrl = new GoogleAuthorizationCodeRequestUrl(new Uri(AuthorizationServerUrl));
            requestUrl.AccessType = "offline";
            requestUrl.ApprovalPrompt = "force";
            requestUrl.ClientId = ClientSecrets.ClientId;
            requestUrl.Scope = string.Join(" ", Scopes);
            requestUrl.RedirectUri = redirectUri;
            return requestUrl;
        }
    };
}