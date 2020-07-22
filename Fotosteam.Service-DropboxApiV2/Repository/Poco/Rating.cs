using System;
using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Kapselt die Bewertung zu einem Foto
    /// </summary>
    public class Rating
    {
        public Rating()
        {
            Date = DateTime.Now;
        }

        /// <summary>
        ///     Die Id des Fotos
        /// </summary>
        [Required]
        public int PhotoId { get; set; } // PhotoId

        /// <summary>
        ///     Der Name des Benutzers, der die Bewertung abgegeben hat
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        ///     Der Alias des Benutzers, der die Bewertung abgegeben hat
        /// </summary>
        public string UserAlias { get; set; } // UserAlias

        /// <summary>
        ///     Der Link zu dem Profilbild des Benutzers
        /// </summary>
        public string UserAvatarLink { get; set; } // UserAvatarLink

        /// <summary>
        ///     Der absolute Wert der Bewertung
        /// </summary>
        [Required]
        public int Value { get; set; }

        /// <summary>
        ///     Durchschnittliche Bewertung
        /// </summary>
        public double AverageRating { get; set; }

        public int RatingSum { get; set; }

        /// <summary>
        ///     Datum des Eintrags
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        ///     Dient zur Vereinfachung der Auswertung, falls die Bewertung von einem
        ///     angemeldenten Benutzer durchgeführt wurde
        /// </summary>
        internal int? MemberId { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return
                string.Format(
                    "PhotoId: {0}, UserName: {1}, UserAlias: {2}, UserAvatarLink: {3}, Value: {4}, AverageRating: {5}, Date: {6}",
                    PhotoId, UserName, UserAlias, UserAvatarLink, Value, AverageRating, Date);
        }
    }
}