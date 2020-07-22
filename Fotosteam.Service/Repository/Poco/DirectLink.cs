using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Die Klasse repr�sentiert den Link zu einem Fotos f�r den direkten Zugriff
    /// </summary>
    public class DirectLink
    {
        /// <summary>
        ///     Id des Fotos
        /// </summary>
        [Required]
        public int PhotoId { get; set; } // PhotoId

        /// <summary>
        ///     Die Gr��e des Fotos,
        ///     dabei gelten f�r Quadratische Formate: 100,200 und 400px
        ///     und sonst 480, 640, 1024, 1440, 1920
        ///     0 = Originalgr��e
        /// </summary>
        [Required]
        public int Size { get; set; } // Size

        /// <summary>
        ///     Der Link zu dem Foto
        /// </summary>
        [Required]
        public string Url { get; set; }

        /// <summary>
        ///     Formattiert das Objekt f�r ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("PhotoId: {0}, Size: {1}, Url: {2}", PhotoId, Size, Url);
        }
    }
}