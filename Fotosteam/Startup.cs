using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fotosteam.Startup))]
namespace Fotosteam
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var startup = new Fotosteam.Service.Startup();
            startup.Configuration(app);
        }
    }
}

