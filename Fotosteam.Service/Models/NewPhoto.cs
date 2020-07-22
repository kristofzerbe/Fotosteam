using System;
using System.Collections.Generic;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Models
{
    /// <summary>
    /// Repräsentiert ein Foto, das neu veröffentlicht wurde
    /// </summary>
    public class NewPhoto
    {
        /// <summary>
        /// Id des Photos
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Der Name des Fotos
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Der Title des Fotos
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Liefert die druchschnittliche Bewertung des Fotos
        /// </summary>
        //Kristof: obsolet, da nun die Bewertungen summiert werden
        
        //public double AverageRating { get; set; }
        /// <summary>
        ///     Liefert die Summe aller Bewertungungen des Fotos
        /// </summary>
        public int RatingSum { get; set; }
        
        /// <summary>
        /// Eine Zusammenfassung des Ortes des Fots
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Datum der Veröffentlichung
        /// </summary>
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// Die Links für den direkten Zugriff auf die verschiedenen Formate des Fotos
        /// </summary>
        public ICollection<DirectLink> DirectLinks { get; set; }
        /// <summary>
        ///     Dominante Farbe des Bildes
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// Id des Benutzers, dem das Foto gehört
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// Der Alias des Benutzers, dem das Foto gehört
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// Der volle Name des Benutzers, dem das Foto gehört
        /// </summary>
        public string PlainName { get; set; }
        /// <summary>
        ///     Der direkte Link zum Profilbild mit 100px
        /// </summary>
        public string Avatar100Url { get; set; }

        /// <summary>
        ///     Der direkte Link zum Profilbild mit 200px
        /// </summary>
        public string Avatar200Url { get; set; }

        /// <summary>
        /// Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Title: {2}, RatingSum: {3}, Location: {4}, PublishDate: {5}, DirectLinks: {6}, Color: {7}, MemberId: {8}, Alias: {9}, Avatar100Url: {10}, Avatar200Url: {11}", Id, Name, Title, RatingSum, Location, PublishDate, DirectLinks, Color, MemberId, Alias, Avatar100Url, Avatar200Url);
        }
    }
}
