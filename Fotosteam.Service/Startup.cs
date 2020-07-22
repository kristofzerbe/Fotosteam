using System;
using System.Web.Http;
using System.Web.Http.Filters;
using Fotosteam.Service.Controller;
using Google.Apis.Json;
using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Owin;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace Fotosteam.Service
{
    /// <summary>
    /// Die Klasse ist der Einstiegspunkt für OWIN
    /// </summary>
    public partial class Startup
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (DataController));

        /// <summary>
        ///     Delegate um einen Authentifizierungsfilter zum Testen einschleusen zu können
        /// </summary>
        public static Func<IFilter> CreateAuthFilter = () => null;

        /// <summary>
        /// Die Optioen,die für die extere Authenifizeru verwendet werden sollen
        /// </summary>
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        /// <summary>
        /// Wird verwendet, um z.B. für die Unittests die Controller mit Fake-Repositories zu bestücken
        /// </summary>
        public static IDependencyResolver DependencyResolver { get; set; }

        /// <summary>
        /// Kann verwendet werden, um Referenzen im JSON zu erzeugen, so könnte die notwendige Bandbreite reduziert werden,
        /// da bei mehrfacher Verwendungen eines Objektes dieses nur einmal serialisiert wird
        /// </summary>
        public static bool SerializeTypenamesInJson { get; set; }

        /// <summary>
        /// Führt die Konfiguration der Anwendung durch
        /// </summary>
        /// <param name="app">Der OwinAppBuilder</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureExceptionHandling();
            ConfigureAuth(app);

            var config = new HttpConfiguration();

            //Dependency Injection
            if (DependencyResolver != null)
            {
                config.DependencyResolver = DependencyResolver;
            }

            WebApiConfig.Register(config);
            app.UseWebApi(config);

            //Dieser Teil ist nur das Testen notwendig. Man könnte eventuell so auch die Authentifzierung weiter aufbohren
            var authFilter = CreateAuthFilter();
            if (authFilter != null)
            {
                config.Filters.Add(authFilter);
            }

            //Add SignalR 
            app.MapSignalR();
            GlobalHost.HubPipeline.RequireAuthentication(); //Notifications gelten nur für angelmeldete Benutzer   
            var json = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            GlobalHost.DependencyResolver.Register(typeof(IJsonSerializer), () => JsonSerializer.Create(json));
           
        }

        private void ConfigureExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
        }

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error("Unhandled exception in " + sender, e.ExceptionObject as Exception);
        }
    }
}