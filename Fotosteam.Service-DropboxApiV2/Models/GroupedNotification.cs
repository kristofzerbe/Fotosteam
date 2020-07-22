using System.Collections.Generic;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Models
{
    /// <summary>
    /// Gruppiert Benachrichtungen
    /// </summary>
    public class GroupedNotification
    {
        /// <summary>
        /// Art der Benachrichtung
        /// </summary>
        public NotificationType Type { get; set; }
        /// <summary>
        /// Liste der Benachrichtigungen, die den gleichen Typ haben und zu dem gleichen Bild gehören
        /// </summary>
        public List<Notification> Notifications { get; set; }
        /// <summary>
        /// Anzahl der Benachrichtungen
        /// </summary>
        public int Count { get { return Notifications == null ? 0 : Notifications.Count; } }
        /// <summary>
        /// Die Url zu dem 100er Foto
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Der Titel des Fotos
        /// </summary>
        public string PhotoTitle { get; set; }
    }
}