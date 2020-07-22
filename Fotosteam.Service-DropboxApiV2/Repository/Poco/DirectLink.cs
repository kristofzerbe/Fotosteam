using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Die Klasse repräsentiert den Link zu einem Fotos für den direkten Zugriff
    /// </summary>
    public class DirectLink
    {
        /// <summary>
        ///     Id des Fotos
        /// </summary>
        [Required]
        public int PhotoId { get; set; } // PhotoId

        /// <summary>
        ///     Die Größe des Fotos,
        ///     dabei gelten für Quadratische Formate: 100,200 und 400px
        ///     und sonst 480, 640, 1024, 1440, 1920
        ///     0 = Originalgröße
        /// </summary>
        [Required]
        public int Size { get; set; } // Size

        /// <summary>
        ///     Der Link zu dem Foto
        /// </summary>
        [Required]
        public string Url { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("PhotoId: {0}, Size: {1}, Url: {2}", PhotoId, Size, Url);
        }
    }
}