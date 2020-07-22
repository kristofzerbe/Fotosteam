using System.Collections.Generic;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Die Klasse definiert einen Zusammenfassung von Orten, so dass alle Orte eines Landes/Bundeslandes/Stadt
    ///     zusammengefasst werden können
    /// </summary>
    public class LocationGroup
    {
        /// <summary>
        ///     Referenz zum Benutzer, darf nicht null sein!
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        ///     Gibt den Namen der Gruppe an
        ///     Bei Country => Name des Landes
        ///     Bei County => Name des Landes, Name des Bundeslandes
        ///     Bei City => Name des Landes, Name des Bundeslandes, Name der Stadt
        /// </summary>
        public string Name { get; set; } // Name

        /// <summary>
        ///     Liefert die in der Gruppe enthaltenen Lokationen
        /// </summary>
        public List<Location> Locations { get; set; }

        /// <summary>
        ///     Längegrad des ersten Ortes in der Gruppe
        /// </summary>
        public double FirstLongitude { get; set; }

        /// <summary>
        ///     Breitengrad des ersten Ortes in der Gruppe
        /// </summary>
        public double FirstLatitude { get; set; }

        /// <summary>
        ///     Berechneter Wert, wieviele Bilder dieser Gruppe sind
        /// </summary>
        public int PhotoCount { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return
                string.Format(
                    "MemberId: {0}, Name: {1}, Locations: {2}, FirstLongitude: {3}, FirstLatitude: {4}, PhotoCount: {5}",
                    MemberId, Name, Locations, FirstLongitude, FirstLatitude, PhotoCount);
        }
    }
}