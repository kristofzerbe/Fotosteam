using System;
using Fotosteam.Service.Models;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Definiert die Benachrichtung
    /// </summary>
    public class Notification : PocoBase
    {
        public Notification()
        {
            Date = DateTime.Now;
        }

        /// <summary>
        ///     Datum und Uhrzeit der Aktion
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Typ der Benachrichtung
        /// </summary>
        public NotificationType Type { get; set; }

        /// <summary>
        ///     Gibt an, ob die Benachrichtigung schon gelesen wurde
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        ///     Alias desjenigen, der die Notification verursacht hat
        /// </summary>
        public string UserAlias { get; set; }

        /// <summary>
        ///     AvatarLink desjenigen, der die Notification verursacht hat
        /// </summary>
        public string UserAvatarLink { get; set; }

        /// <summary>
        ///     Gibt an, ob der Verursacher der Benachrichtung ein Member ist,
        ///     dann kann auch auf die Benutzerseite verlinkt werden
        /// </summary>
        public bool IsUserAmember { get; set; }

        /// <summary>
        ///     für Comment, Rating; keine Id, um ohne Roundtrip einen Link zum betreffenden Foto bauen zu können
        ///     .../MemberAlias/PhotoName
        /// </summary>
        public string PhotoName { get; set; }

        /// <summary>
        ///     Die Id des Benutzers der benachrichtigt wird
        /// </summary>
        internal int MemberId { get; set; }

        /// <summary>
        ///     Zusaätzliche Information, die zum Client geschickt werden kann
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        ///     Die Url zu dem 100er Foto
        /// </summary>
        public string PhotoUrl { get; internal set; }

        /// <summary>
        ///     Der Titel des Fotos
        /// </summary>
        public string PhotoTitle { get; set; }
    }
}