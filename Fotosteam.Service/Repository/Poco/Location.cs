using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Die Klasse definiert einen Ort, der zu einem Foto, einem Ereignis oder einem Benutzer gehören kann
    /// </summary>
    public class Location : PocoBase
    {
        /// <summary>
        ///     Referenz zum Benutzer, darf nicht null sein!
        /// </summary>
        [Required]
        public int MemberId { get; set; }

        /// <summary>
        ///     Name muss eindeutig sein
        /// </summary>
        [Required]
        public string Name { get; set; } // Name

        /// <summary>
        ///     Dreistelliger ISO-Code des Landes
        /// </summary>
        [MaxLength(3)]
        public string CountryIsoCode { get; set; } // CountryIsoCode

        /// <summary>
        ///     Name des Landes
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        ///     Straße
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        ///     Stadt
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///     Bundesstaat / -land
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///     Landkreis
        /// </summary>
        public string County { get; set; }

        /// <summary>
        ///     Längegrad
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        ///     Breitengrad
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        ///     Berechneter Wert, wieviele Bilder dieser Location aktuell zugeordnet sind
        /// </summary>
        public int PhotoCount { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return
                string.Format(
                    "MemberId: {0}, Name: {1}, CountryIsoCode: {2}, Country: {3}, Street: {4}, City: {5}, State: {6}, County: {7}, Longitude: {8}, Latitude: {9}, PhotoCount: {10}",
                    MemberId, Name, CountryIsoCode, Country, Street, City, State, County, Longitude, Latitude,
                    PhotoCount);
        }
    }
}