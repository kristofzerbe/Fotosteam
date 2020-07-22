using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Fotosteam.Service.Results
{
    /// <summary>
    /// Das Objekt wird benötigt, um die Anfrage an den externen Provider weiterzuleiten
    ///  Das kommt aus https://github.com/tjoudeh/AngularJSAuthentication
    /// </summary>
    public class ChallengeResult : IHttpActionResult
    {
        public ChallengeResult(string loginProvider, ApiController controller)
        {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

        public string LoginProvider { get; set; }
        public HttpRequestMessage Request { get; set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            Request.GetOwinContext().Authentication.Challenge(LoginProvider);

            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
    }
}