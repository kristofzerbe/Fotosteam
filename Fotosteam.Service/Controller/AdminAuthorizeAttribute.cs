using System.Web.Http;
using System.Web.Http.Controllers;

namespace Fotosteam.Service.Controller
{
    /// <summary>
    /// Es ist sinnvoll, dass wir übers Web Änderungen vornehmen können.
    /// Entsprechende Methoden müssen mit diesem Attribute versehen werden, um eine versehntliche
    /// Ausführung durch einen authorisieren Benutzer zu verhindern.
    /// Die Methoden dürfen ausschließlich im <see cref="RoKrController"/> implementiert werden,
    /// der mit diesem Attribute dekoriert ist
    /// </summary>
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Verweis zu dem Controller auf dem das Attribut angewendet wird.
        /// Muss vom Constructor des Controllers gesetzt werden
        /// </summary>
        public static ControllerBase Controller;

        //Nur die nachfolgenden Benutzer sind authorisiziert
        private const int KristofId = 2;
        private const int RobertId = 33;

        private static bool IsAuthorized()
        {
            var currentMember = Controller.GetMemberFromAuthenticatedUser();
            var id = currentMember.Id;
            if (id != KristofId && id != RobertId)
            {
                return false;
            }
            return true;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var authorized = base.IsAuthorized(actionContext);
            if (!authorized)
            {
                // The user is not authenticated
                return false;
            }
            return IsAuthorized();
        }
    }
}
