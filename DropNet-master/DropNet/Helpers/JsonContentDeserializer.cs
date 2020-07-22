using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace DropNet.Helpers
{
    /// <summary>
    /// Deserializer for the content of an response
    /// </summary>
    public class JsonContentDeserializer : IDeserializer 
    {
        /// <summary>
        /// Type the deserializer should be used for
        /// </summary>
        public string ContentType { get; private  set; }
        public JsonContentDeserializer()
        {
            ContentType = "application/json";
        }

        /// <summary>
        /// Deserializes only the content of the response
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <param name="response">The reponse containing the date to be converted</param>
        /// <returns>Deserialised object</returns>
        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public string RootElement { get; set; }
        
        public string Namespace { get; set; }        

        public string DateFormat{ get; set; }
        
    }
}
