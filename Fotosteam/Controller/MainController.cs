using System.Configuration;
using System.Web.Mvc;
using Fotosteam.Models;
using Fotosteam.Service.Controller;

namespace Fotosteam.Controller
{
    public class MainController : System.Web.Mvc.Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            if (ConfigurationManager.AppSettings["StartingPoint"] == "index.dev.html")
            {
                return View("IndexDev");                
            } else
            {
                return View("Index");                
            }
        }

        public ActionResult Foto(string alias, string name)
        {
            if (ConfigurationManager.AppSettings["StartingPoint"] == "index.dev.html")
            {
                return View("IndexDev");
            } else
            {
                var controller = new WebController();
                var photo = controller.GetPhoto(alias, name);
                
                var model = new FotoMetaModel(photo);

                return View("Foto", model);                
            }
        }

        public ActionResult Story(string alias, string name)
        {
            if (ConfigurationManager.AppSettings["StartingPoint"] == "index.dev.html")
            {
                return View("IndexDev");
            }
            else
            {
                var controller = new WebController();
                var story = controller.GetStory(alias, name);
                var model = new StoryMetaModel(story);

                return View("Story", model);
            }
        }
    }
}