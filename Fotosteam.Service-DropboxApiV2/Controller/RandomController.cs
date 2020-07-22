using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using log4net;

namespace Fotosteam.Service.Controller
{
    /// <summary>
    ///     Der Controller kapselt Methoden zur Rückgabe von zufälligen Informationen,
    ///     wie z.B. zufälligem Text
    /// </summary>
    public class RandomController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (AccountController));

        /// <summary>
        ///     Liefert einen zufälligen deutschen Namen zurück
        /// </summary>
        /// <returns>Den zufälligen Namen</returns>
        [Route("api/random/name")]
        public string GetRandomGermanName()
        {
            try
            {
                var client = new WebClient();
                var reply = client.DownloadString("http://realnamecreator.alexjonas.de/js.php");

                reply =
                    reply.Replace("document.write('<a class=\"rnc\" href=\"http://realnamecreator.alexjonas.de/?js\">",
                        "");
                reply = reply.Replace("</a>');", "");
                Log.Debug(reply);

                return reply;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return "No Random name";
            }
        }

        /// <summary>
        ///     Liefert einen zufälligen Lorem Ipsum Text
        /// </summary>
        /// <param name="paragraphCount">
        ///     Anzahl der Paragrafen des Textes, wenn 0 übergeben wird, dann wir der Wert auf 1 gesetzt,
        ///     um einen Paragrafen zu erhalten
        /// </param>
        /// <returns>Den Zufallstext</returns>
        [Route("api/random/text/{paragraphCount}")]
        public string GetRandomText(int paragraphCount)
        {
            if (paragraphCount == 0)
                paragraphCount = 1;

            try
            {
                var client = new WebClient();
                var reply = client.DownloadString("http://loripsum.net/api/" + paragraphCount + "/short/");
                Log.Debug(reply);
                return reply;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return "No Random Text";
            }
        }

        /// <summary>
        ///     Liefert eine zufälliges Bild zurück
        /// </summary>
        /// <returns>HTTPResponseMessage mit dem Bild in einer Größe von 32x32 Pixeln</returns>
        [Route("api/random/avatar")]
        public HttpResponseMessage GetRandomAvatar()
        {
            try
            {
                var client = new HttpClient();
                var img = client.GetByteArrayAsync("http://lorempixel.com/32/32/people/").Result;

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(img);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                Log.Debug(response.ToString());
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new HttpResponseMessage(HttpStatusCode.BadRequest); 
            }
        }
    }
}