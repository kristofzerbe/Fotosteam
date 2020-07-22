using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Repräsentiert einen Teil der Geschichte
    /// </summary>
    public class Chapter : PocoBase
    {
        /// <summary>
        ///     Ist die Überschrift des Kapitels
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        /// <summary>
        ///     Gibt die Reihenfolge innerhalb der Story wieder
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     Definiert die Ledges zum Kapitel
        /// </summary>
        public ICollection<Ledge> Ledges { get; set; }

        /// <summary>
        ///     Definiert die ID der zugehörigen <see cref="Story" />
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int StoryId { get; set; }

        /// <summary>
        ///     Gibt an ob das Foto nicht für alle sichtbar ist
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("Name: {0}, Order: {1}, Ledges: {2}, StoryId: {3}, IsPrivate: {4}", Name, Order, Ledges,
                StoryId, IsPrivate);
        }
    }
}