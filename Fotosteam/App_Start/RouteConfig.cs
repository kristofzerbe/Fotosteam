using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fotosteam {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapPageRoute("Teaser", "teaser", "~/teaser/index.html");

            routes.MapRoute(
                name: "Foto",
                url: "{Alias}/foto/{Name}",
                defaults: new { controller = "Main", action = "Foto" }
            );

            routes.MapRoute(
                name: "Story",
                url: "{Alias}/story/{Name}",
                defaults: new { controller = "Main", action = "Story" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{*anything}",
                defaults: new { controller = "Main", action = "Index" }
            );

            //string startingPoint = ConfigurationManager.AppSettings["StartingPoint"];

            //stackoverflow.com/questions/19643001/how-to-route-everything-other-than-web-api-to-index-html'
            //routes.MapPageRoute("Default", "{*anything}", String.Concat("~/", startingPoint));                

        }
    }
}
