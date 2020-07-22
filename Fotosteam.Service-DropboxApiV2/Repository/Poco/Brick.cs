using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Repräsentiert einen Teil unter einem Ledge
    /// </summary>
    public abstract class Brick : PocoBase
    {
        /// <summary>
        ///     Definiert die Reihenfolge für die Anzeige
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     Definiert die Art des Bricks. Das ist notwendig für die Serializierung,
        ///     da sich die verschiedenen Type von Brick ableiten, aber die komplette Typ-Information nicht nach aussen geben wird,
        ///     um das JSON klein zu halten
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Type { get; set; }

        /// <summary>
        ///     Die Id des Ledge zu dem der Brick gehört
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int LedgeId { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("Order: {0}, Type: {1}, LedgeId: {2}", Order, Type, LedgeId);
        }
    }
}