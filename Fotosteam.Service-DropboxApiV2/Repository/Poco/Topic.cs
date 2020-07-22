using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Benutzerdefinierte Kategorien, ein Foto kann mehreren Themen zugerodnet sein
    /// </summary>
    public class Topic : PocoBase
    {
        /// <summary>
        ///     Eindeutiger Name
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        ///     Berechneter Wert, wieviele Bilder diesem Topic aktuell zugeordnet sind
        /// </summary>
        public int PhotoCount { get; set; }

        /// <summary>
        ///     Id des Benutzers
        /// </summary>
        [Required]
        public int MemberId { get; set; }

        /// <summary>
        ///     Der zugehörige Benutzer
        /// </summary>
        public Member Member { get; internal set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("Name: {0}, Description: {1}, PhotoCount: {2}, MemberId: {3}, Member: {4}", Name,
                Description, PhotoCount, MemberId, Member);
        }
    }
}