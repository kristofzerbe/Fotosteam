using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fotosteam.Service.Models
{
    /// <summary>
    /// Kapselt die Informationen für die Aktualisierung eines Objektes auf Eingeschaftebene
    /// </summary>
    public class UpdateModel
    {
        /// <summary>
        /// Die Id des Objekts, das aktualisert werden soll
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Der Typ des Objekts, dass aktualisiert werden soll.
        /// Ist notwendig, um ein entsprechende Objekt aus der DB zu laden
        /// </summary>
        [JsonConverter(typeof (StringEnumConverter))]
        public ModelType Type { get; set; }

        /// <summary>
        /// Der Name der Eigenschaft, die geändert werden soll.
        /// </summary>
        /// <remarks>Für Referenzobjekte ist es notwendig speziell Aktuaisierungmethoden zu schreiben</remarks>
        public string PropertyName { get; set; }
        /// <summary>
        /// Der Wert, der der Eigenschaft zugeordnet werden soll
        /// </summary>
        public string Value { get; set; }
    }
}