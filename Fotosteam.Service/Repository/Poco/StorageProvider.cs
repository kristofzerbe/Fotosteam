using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Definiert den Cloud-Dienst
    /// </summary>
    /// <remarks>Die Klasse wird eventuell in �ber eine Enum aufgel�st </remarks>
    public class StorageProvider : PocoBase
    {
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Formattiert das Objekt f�r ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("Name: {0}", Name);
        }
    }
}