using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Definiert die Struktur innerhalb eines Kapitels
    /// </summary>
    public class Ledge : PocoBase
    {
        /// <summary>
        ///     Reihenfolge für die Anzeige
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     Die Vorlage, auf der das Unterkapitel sich bezieht
        /// </summary>
        [Required]
        public string Template { get; set; }

        /// <summary>
        ///     Die einzelnen Elemente des des Unterkapitels
        /// </summary>
        public ICollection<Brick> Bricks { get; set; }

        /// <summary>
        ///     Die Id des zugehörigen Kapitels
        /// </summary>
        [Required]
        public int ChapterId { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("Order: {0}, Template: {1}, Bricks: {2}, ChapterId: {3}", Order, Template, Bricks,
                ChapterId);
        }
    }
}