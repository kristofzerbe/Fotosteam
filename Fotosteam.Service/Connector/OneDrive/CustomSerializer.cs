using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp.Serializers;

namespace Fotosteam.Service.Connector.OneDrive
{
    /// <summary>
    /// Für doe Nutzung von Restsharp mit Onedrive muss eine angepasster Serialisierer benutzt werden,
    /// da der Standardserialisierer nicht unbedingt alles in das geforderte Format umsetzt
    /// </summary>
    public class CustomSerializer : ISerializer
    {
        /// <summary>
        /// Standardkonstruktor, der den Contenttype auf Json festlegt
        /// </summary>
        public CustomSerializer()
        {
            ContentType = "application/json";
        }

        /// <summary>
        /// Führt die Serialiiserung mit dem StringConverter durch
        /// </summary>
        /// <param name="obj">Das Objekt, das serialisiert werden soll</param>
        /// <returns>Der Json-String des serialisierten Objekts</returns>
        public string Serialize(object obj)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            return JsonConvert.SerializeObject(obj, Formatting.None, settings);
        }

        string ISerializer.RootElement { get; set; }
        string ISerializer.Namespace { get; set; }
        string ISerializer.DateFormat { get; set; }
        public string ContentType { get; set; }
    }
}