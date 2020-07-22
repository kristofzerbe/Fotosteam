using System;
using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Ein Kommentar
    /// </summary>
    public class Comment
    {
        public Comment()
        {
            Date = DateTime.Now;
        }

        /// <summary>
        ///     Eindeutige Identifikation des Kommentars
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        ///     Referenz zu einem übergeordneten Kommentar
        /// </summary>
        public int? ParentCommentId { get; set; }

        /// <summary>
        ///     Der übergeordnete Kommentar
        /// </summary>
        public Comment Parent { get; set; }

        /// <summary>
        ///     Datum des Eintrags
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        ///     Der Text des Kommentars
        /// </summary>
        [Required]
        [MinLength(2)]
        public string Text { get; set; }

        /// <summary>
        ///     Der Name des Verfassers
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        ///     Der Alias des Verfassers
        /// </summary>
        public string UserAlias { get; set; }

        /// <summary>
        ///     Der Link zu dem Bild des Verfassers
        /// </summary>
        public string UserAvatarLink { get; set; }

        /// <summary>
        ///     Id des Fotos zu dem der Kommentar verfasst wurde
        /// </summary>
        public int PhotoId { get; set; }

        /// <summary>
        ///     Gesamtanzahl der Kommentare zu dem Foto
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return
                string.Format(
                    "CommentId: {0}, ParentCommentId: {1}, Parent: {2}, Date: {3}, Text: {4}, UserName: {5}, UserAlias: {6}, UserAvatarLink: {7}, PhotoId: {8}, TotalCount: {9}",
                    CommentId, ParentCommentId, Parent, Date, Text, UserName, UserAlias, UserAvatarLink, PhotoId,
                    TotalCount);
        }
    }
}