using System.ComponentModel.DataAnnotations;
using Fotosteam.Service.Models;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Kapselt die die Information zu einer Karte, die in einem Ledge eingebuden wird
    /// </summary>
    public class MapBrick : Brick
    {
        /// <summary>
        ///     Initialisiert den Typ
        /// </summary>
        /// <remarks>Der Typ ist für die JSON-(DE)Serialisierung wichtig</remarks>
        public MapBrick()
        {
            Type = BrickType.Map.ToString().ToLower();
        }

        /// <summary>
        ///     Der Breitengrad des Ortes
        /// </summary>
        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }

        /// <summary>
        ///     Der Längengrad des Ortes
        /// </summary>
        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; }

        /// <summary>
        ///     Ein mögliche Vergrößerung für die Anzeige
        /// </summary>
        public int Zoom { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0}, Latitude: {1}, Longitude: {2}, Zoom: {3}", base.ToString(), Latitude, Longitude,
                Zoom);
        }
    }
}