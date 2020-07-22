using System.Web.Http;
using Google.Apis.Json;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace Fotosteam.Service
{
    /// <summary>
    ///     Konfiguriert die Routen für die API
    ///     Die API teilt sich auf den Account- und DataController auf
    /// </summary>
    /// <remarks>
    ///     Der Controller für das Syncronisieren ist noch nicht implementiert
    /// </remarks>
    public static class WebApiConfig
    {
        /// <summary>
        ///     Fügt der Konfiguration das Routing hinzu und setzt die globalen Eigenschaften für das JSON-(DE)Serializing
        /// </summary>
        /// <param name="config">Das Konfigurationsobjekt der Anwendung</param>
        public static void Register(HttpConfiguration config)
        {
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            
                //Das wir benötig für die Unit-Test, da sonst die Stories nicht seralisiert werden können
            if (Startup.SerializeTypenamesInJson)
            {
                json.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            }
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/account/{action}/{data}",
                new { controller = "Account", data = RouteParameter.Optional });

            config.Routes.MapHttpRoute("DefaultAuthorizeApi", "api/authorize/{action}/{data}",
                new { controller = "Authorize", data = RouteParameter.Optional });

            config.Routes.MapHttpRoute("DefaultMailApi", "api/communication/{action}/{data}",
                new { controller = "Communication", data = RouteParameter.Optional });

            config.Routes.MapHttpRoute("UserApiWithAction", "api/data/{alias}/{elementType}/{criteria}",
                new { controller = "Data" });

            config.Routes.MapHttpRoute("Lists", "api/data/{elementType}/{criteria}",
                new { controller = "Data", criteria = RouteParameter.Optional });

            config.Routes.MapHttpRoute("ListWithRange", "api/data/{aliasOrCommand}/{criteria}/{skip}/{take}",
                new { controller = "Data" }, new { skip = @"\d+", take = @"\d+" });

            config.Routes.MapHttpRoute("ListWithRangeForUser", "api/data/{alias}/{elementType}/{criteria}/{skip}/{take}",
            new { controller = "Data" }, new { skip = @"\d+", take = @"\d+" });

            config.Routes.MapHttpRoute("RoKrApi", "api/RoKr/{action}/{data}",
                new { controller = "RoKr", data = RouteParameter.Optional });

            config.Routes.MapHttpRoute("SynchApi", "api/synch/{action}/{challenge}",
                new { controller = "Synch", challenge = RouteParameter.Optional });
        }
    }
}