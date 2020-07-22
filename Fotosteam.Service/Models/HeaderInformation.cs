namespace Fotosteam.Service.Models
{
    /// <summary>
    /// Kapselt die Informationen, die beim Aktualisieren von Fotos übertragen werden müssen
    /// </summary>
    public class HeaderInformation
    {
        /// <summary>
        ///     Der direkte Link zum Hintergrundbild mit 640px
        /// </summary>
        public string Header640Url { get; set; }

        /// <summary>
        ///     Der direkte Link zum Hintergrundbild mit 1024px
        /// </summary>
        public string Header1024Url { get; set; }

        /// <summary>
        ///     Der direkte Link zum Hintergrundbild mit 1440px
        /// </summary>
        public string Header1440Url { get; set; }

        /// <summary>
        ///     Der direkte Link zum Hintergrundbild mit 1920px
        /// </summary>
        public string Header1920Url { get; set; }

        /// <summary>
        ///     Dominante Farbe des Header-Bildes
        /// </summary>
        public string HeaderColor { get; set; }
    }
}