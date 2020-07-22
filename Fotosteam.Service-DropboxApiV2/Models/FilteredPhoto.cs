using System.Collections.Generic;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Models
{
    /// <summary>
    /// Repräsentiert ein Photo mit mininmalen Daten, die für die Anzeige nach einer Suche erforderlich sind
    /// </summary>
    public class FilteredPhoto : NewPhoto
    {
        /// <summary>
        ///     Die Kategorien des Fotos als Enum
        /// </summary>
        public CategoryType Category { get; set; }

        /// <summary>
        ///     Liste der Kategorien des Fotos
        /// </summary>
        public List<Category> Categories { get; set; }

        /// <summary>
        ///     Liste aller Themeen
        /// </summary>
        public ICollection<Topic> Topics { get; set; }

    }
}
