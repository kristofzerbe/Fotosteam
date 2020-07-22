namespace Fotosteam.Service.Models
{
    /// <summary>
    ///     Modell zur Aktualisierung der Thumbnails eines Fotos
    /// </summary>
    public class ThumbModel
    {
        /// <summary>
        ///     Id des Photos, für dass die Thumbnails aktualisiert werden sollen
        /// </summary>
        public int PhotoId { get; set; }

        /// <summary>
        ///     Der Abstand vom linken Rand in Prozent
        /// </summary>
        public float XPercentage { get; set; }

        /// <summary>
        ///     Der Abstand vom unteren Rand in Prozent
        /// </summary>
        public float YPercentage { get; set; }
    }
}