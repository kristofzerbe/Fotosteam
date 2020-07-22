using System;
using System.Collections.Generic;
using System.Web.Http;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Newtonsoft.Json;

namespace Fotosteam.Service.Controller
{
    /// <summary>
    /// Dies ist der Admin-Controller, der nur von Kristof und Robert genutzt werden kann.
    /// Die Ids sind hardcodiert und müssen es auch bleiben, damit man möglichst wenig von außen ändern kann
    /// </summary>
    [AdminAuthorize]
    public class RoKrController : ControllerBase
    {

        
        /// <summary>
        /// Standardkonstruktor
        /// </summary>
        public RoKrController()
            : this(new DataRepository(),new AuthRepository())
        {}

        /// <summary>
        /// Initialisiert die Klasse mit spezifischen Repositories 
        /// </summary>
        /// <param name="repository">Das Datenrepository</param>
        /// <param name="authRepository">Das Authentifizierungsrepository</param>
        public RoKrController(IDataRepository repository, IAuthRepository authRepository)
        {
            DataRepository = repository;
            AuthenticationRepository = authRepository;
            AdminAuthorizeAttribute.Controller = this;
        }


        [HttpPost]
        [ActionName("CreateInviteCodes")]
        public Result<List<string>> CreateInviteCodes()
        {
            var result = new Result<List<string>>();            
            return result;
        }
    }
}
