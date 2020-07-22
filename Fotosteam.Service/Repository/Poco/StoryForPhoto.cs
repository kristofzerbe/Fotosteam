using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Kapselt eine Geschichte von einem Benutzer
    /// </summary>
    public class StoryForPhoto : PocoBase
    {
        /// <summary>
        ///     Der Titel der Geschichte
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Url-sicherer Schlüssel
        /// </summary>
        public string Key
        {
            get { return Name.MakeUrlSafe(); }
        }

        /// <summary>
        ///     Anzahl der Kapitel
        /// </summary>
        public int ChapterCount { get; set; }

        /// <summary>
        ///     Anzahl der Fotos in der Gechichte
        /// </summary>
        public int PhotoCount { get; set; }

        /// <summary>
        ///     Auflistung der Kapitel
        /// </summary>
        public ICollection<Chapter> Chapters { get; set; }

        /// <summary>
        ///     Id des Benutzers
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int MemberId { get; set; }

        /// <summary>
        ///     Das zugehörige Member-Objekt
        /// </summary>
        public Member Member { get; set; }

        /// <summary>
        ///     Liefert die Id zu dem HeaderPhoto
        /// </summary>
        public int? HeaderPhotoId { get; set; }

        /// <summary>
        ///     Liefert das Headerphoto zurück
        /// </summary>
        public Photo HeaderPhoto { get; set; }

        /// <summary>
        ///     Gibt an ob das Foto nicht für alle sichtbar ist
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        ///     Liefert die Id zu dem Foto
        /// </summary>
        public int PhotoId { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return
                string.Format(
                    "PhotoId: {10},Name: {0}, Key: {1}, ChapterCount: {2}, PhotoCount: {3}, Chapters: {4}, MemberId: {5}, Member: {6}, HeaderPhotoId: {7}, HeaderPhoto: {8}, IsPrivate: {9}",
                    Name, Key, ChapterCount, PhotoCount, Chapters, MemberId, Member, HeaderPhotoId, HeaderPhoto,
                    IsPrivate, PhotoId);
        }
    }
}