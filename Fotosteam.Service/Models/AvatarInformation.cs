namespace Fotosteam.Service.Models
{
    /// Kapselt die Informationen, die beim Aktualisieren von Fotos übertragen werden müssen
    public class AvatarInformation
    {
        /// <summary>
        ///     Der direkte Link zum Profilbild mit 100px
        /// </summary>
        public string Avatar100Url { get; set; }

        /// <summary>
        ///     Der direkte Link zum Profilbild mit 200px
        /// </summary>
        public string Avatar200Url { get; set; }

        /// <summary>
        ///     Dominante Farbe des Avatar-Bildes
        /// </summary>
        public string AvatarColor { get; set; }
    }
}