using System.ComponentModel.DataAnnotations;
using Fotosteam.Service.Models;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Kapselt die Informationen für einen Kontakt über eine Social Medial Plattform wie z.B. Facebook
    /// </summary>
    public class SocialMedia : PocoBase
    {
        /// <summary>
        ///     Der Typ der Plattform
        /// </summary>
        public MediaType Type { get; set; }

        /// <summary>
        ///     Der Link unter dem der Benutzer zu erreichen ist
        /// </summary>
        [Required]
        public string Url { get; set; }

        /// <summary>
        ///     Die Id des Benutzers
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("Type: {0}, Url: {1}, MemberId: {2}", Type, Url, MemberId);
        }
    }
}