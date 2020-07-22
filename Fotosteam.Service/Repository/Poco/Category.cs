using Fotosteam.Service.Models;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Zurdnung zu dem Fotos. Ein Foto kann mehreren Kategorien zugeordnet sein
    /// </summary>
    public class Category
    {
        /// <summary>
        ///     Textuelle Repräsentation von <see cref="CategoryType" />
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Der Wert der Aufzählung
        /// </summary>
        public int TypeValue { get; set; }

        /// <summary>
        ///     Anzahl der Fotos,die zu der Kategorie gefunden wurden.
        ///     Dabei ist auf den Kontext zu achten, da private Fotos nur für den jeweiligen Besitzer mitgezählt werden
        /// </summary>
        public int PhotoCount { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return string.Format("Type: {0}, TypeValue: {1}, PhotoCount: {2}", Type, TypeValue, PhotoCount);
        }
    }
}