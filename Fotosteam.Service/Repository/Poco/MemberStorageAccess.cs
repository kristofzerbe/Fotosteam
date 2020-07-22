using System.ComponentModel.DataAnnotations;
using Fotosteam.Service.Models;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Kapselt die Daten, die f�r den Zugriff auf einen Cloud-Dienst notwendig sind
    /// </summary>
    /// <remarks>Die Daten werden bei der Authorisierung durch den Cloud-Dienst ermittelt und nicht wieder nach au�en geben</remarks>
    public class MemberStorageAccess
    {
        /// <summary>
        ///     Die Id des Benutzers
        /// </summary>
        [Required]
        public int MemberId { get; set; } // MemberId (Primary key)

        /// <summary>
        ///     Der Typ des Cloud-Dienste
        /// </summary>
        public StorageProviderType Type { get; set; }

        /// <summary>
        ///     Der notwendige Toke f�r die Authentifzierung
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        ///     Das Geheimnis f�r die Authentifzierung
        /// </summary>
        [Required]
        public string Secret { get; set; }

        /// <summary>
        ///     DIe Id des Users bei dem Provider
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Formattiert das Objekt f�r ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("MemberId: {0}, Type: {1}, Token: {2}, Secret: {3}, UserId: {4}", MemberId, Type, Token,
                Secret, UserId);
        }
    }
}