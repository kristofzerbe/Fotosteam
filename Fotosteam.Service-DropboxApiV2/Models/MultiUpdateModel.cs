using System.Collections.Generic;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Models
{
    /// <summary>
    ///     Kapselt die Informationen für das Aktualisieren von mehreren Fotos
    /// </summary>
    public class MultiUpdateModel
    {
        //internal static Dictionary<MultiEditField, string> FieldNames = new Dictionary<MultiEditField, string>
        //{
        //    {MultiEditField.AllowCommenting, MultiEditField.AllowCommenting.ToString()},
        //    {MultiEditField.AllowFullSizeDownload, MultiEditField.AllowFullSizeDownload.ToString()},
        //    {MultiEditField.AllowPromoting, MultiEditField.AllowPromoting.ToString()},
        //    {MultiEditField.AllowRating, MultiEditField.AllowRating.ToString()},
        //    {MultiEditField.AllowSharing, MultiEditField.AllowSharing.ToString()},
        //    {MultiEditField.Category, MultiEditField.Category.ToString()},
        //    {MultiEditField.EventId, MultiEditField.EventId.ToString()},
        //    {MultiEditField.LocationId, MultiEditField.LocationId.ToString()},
        //    {MultiEditField.IsPrivate, MultiEditField.IsPrivate.ToString()},
        //    {MultiEditField.IsForStoryOnly, MultiEditField.IsForStoryOnly.ToString()},
        //    {MultiEditField.Topics, MultiEditField.Topics.ToString()},
        //    {MultiEditField.License, MultiEditField.License.ToString()}
        //};
        /// <summary>
        ///     Gibt an ob das Foto nicht für alle sichtbar ist
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        ///     Gibt an, ob das Foto in original heruntergeladen werden darf
        /// </summary>
        public bool AllowFullSizeDownload { get; set; }

        /// <summary>
        ///     Gibt an, ob das Foto kommentiert werden darf.
        ///     Dies will man eventuell auschalten, wenn zuviele Kommentare eingehen
        /// </summary>
        public bool AllowCommenting { get; set; }

        /// <summary>
        ///     Gibt an ob das Foto bewertet werden darf
        /// </summary>
        public bool AllowRating { get; set; }

        /// <summary>
        ///     Gibt an, ob das Foto getteilt werden darf.
        /// </summary>
        public bool AllowSharing { get; set; }

        /// <summary>
        ///     Definiert, ob das Foto nur innherhalb einer Geschichte angezeigt werden soll
        /// </summary>
        public bool IsForStoryOnly { get; set; }

        /// <summary>
        ///     Gib an, ob ein Bild beworben werden darf
        /// </summary>
        public bool AllowPromoting { get; set; }

        /// <summary>
        ///     Die Kategorien des Fotos als Enum
        /// </summary>
        public CategoryType Category { get; set; }

        /// <summary>
        ///     Eine Liste der Ids von Themen, die den Fotos zugeordnet werden sollen
        /// </summary>
        public List<int> Topics { get; set; }

        /// <summary>
        ///     Id des Ortes, das dem Foto zugeordnet werden soll
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        ///     Id des Ereignisses, das den Fotos zugeordnet werden soll
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        ///     Art der Lizenz. Ein Foto kann mehrere Lizenzen unterstützen
        /// </summary>
        public LicenseType License { get; set; }

        /// <summary>
        /// Gibt an, dass die angegebnen Fotos gelöscht werden sollen
        /// </summary>
        public bool ToBeDeleted { get; set; }

        /// <summary>
        ///     Liste der Photos, für die das Update gelten soll
        /// </summary>
        public List<int> PhotoIds { get; set; }

        /// <summary>
        /// Gibt an, ob existierende Werte überschrieben werden sollen
        /// Das gilt nur für Topics, License und Category
        /// </summary>
        public bool ReplaceExistingValues { get; set; }
        
        /// <summary>
        /// Gibt an welche Felder aktualisiert werden sollen
        /// </summary>
        public MultiEditField Fields { get; set; }
    }
}