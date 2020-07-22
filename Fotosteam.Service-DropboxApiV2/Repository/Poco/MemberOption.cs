using Fotosteam.Service.Models;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Kapselt die Einstellung des Benutzer
    /// </summary>
    public class MemberOption
    {
        public int MemberId { get; set; }

        /// <summary>
        ///     Gibt an, ob der Benutzer für die Durchführung von Fotoprojekten
        /// </summary>
        public bool IsAvailableForProjects { get; set; }

        /// <summary>
        ///     Gibt an, ob es ein professioneller Fotograf ist
        /// </summary>
        public bool IsProfessional { get; set; }

        /// <summary>
        ///     Gibt an, ob die Emailadresse öffentlich dargestellt werden soll
        /// </summary>
        public bool DisplayEmailAddress { get; set; }

        /// <summary>
        ///     Gibt an, ob der Benutzer Benachrichtigungen angezeigt bekommen möchte
        /// </summary>
        public bool DisplayNotifications { get; set; }

        /// <summary>
        ///     Gibt an, ob der Benutzer über neue Kommentare per Email benachrichtigt werden möchte
        /// </summary>
        public bool NotifyByEmailOnComment { get; set; }

        /// <summary>
        ///     Gibt an, ob der Benutzer über neue Bewertungen per Email benachrichtigt werden möchte
        /// </summary>
        public bool NotifyByEmailOnRating { get; set; }

        /// <summary>
        ///     Gibt an, ob der Benutzer benachrichtigt werden möchte, wenn er als Buddy hinzugefügt wurde
        /// </summary>
        public bool NotifyByEmailOnBuddyAdd { get; set; }

        /// <summary>
        ///     Gibt an, ob der Benutzer benachrichtigt werden möchte, wenn er als Buddy bestätigt wurde
        /// </summary>
        public bool NotifyByEmailOnBuddyConfirmation { get; set; }

        /// <summary>
        ///     Gibt an, ob der Benutzer benachrichtigt werden möchte, wenn ein Buddy ein Foto veröffentlicht hat
        /// </summary>
        public bool NotifyByEmailOnBuddyAddedPhoto { get; set; }

        /// <summary>
        ///     Gibt an, ob der Benutzer über Neuigkeiten informiert werden möchte
        /// </summary>
        public bool NotifyByEmailOnNews { get; set; }

        /// <summary>
        ///     Gibt an, ob der Benutzer Kommentare zuläßt
        /// </summary>
        public bool AllowComments { get; set; }

        /// <summary>
        ///     Gibt an, ob der Benutzer Bewertunge zuläßt
        /// </summary>
        public bool AllowRating { get; set; }

        /// <summary>
        ///     Gibt an, ob das Foto getteilt werden darf.
        /// </summary>
        public bool AllowSharing { get; set; }

        /// <summary>
        ///     Sprache des Benutzers
        /// </summary>
        public MemberLanguage Language { get; set; }

        /// <summary>
        ///     Gib an, ob der Benutzer den Dropbox-Webhook benutzen möchte
        /// </summary>
        public bool UseDropboxWebhook { get; set; }

        /// <summary>
        ///     Gibt an ob Fotos beim Hochladen als privat definiert werden sollen
        /// </summary>
        public bool DefaultIsPrivate { get; set; }

        /// <summary>
        ///     Definiert die Standardlizez für Fotos
        /// </summary>
        public LicenseType DefaultLicense { get; set; }

        /// <summary>
        ///     Gibt an, ob ein Bild von Fotosteam geteilt werden
        /// </summary>
        public bool DefaultAllowPromoting { get; set; }

        /// <summary>
        ///     Gibt den Standardwert für ein Bild an ,es geteilt werden darf
        /// </summary>
        /// <remarks>AllowSharing gewinnt, wenn der Wert auf Falch steht</remarks>
        public bool DefaultAllowSharing { get; set; }

        /// <summary>
        ///     Gibt an, ob ein Bild in voller Größer heruntergeladen werden darf
        /// </summary>
        public bool DefaultAllowFullSizeDownload { get; set; }

        /// <summary>
        ///     Der Standardwert ob ein Bild bewertet werden darf
        /// </summary>
        public bool DefaultAllowRating { get; set; }

        /// <summary>
        ///     Der Standardwert ob ein Bild kommentiert werden darf
        /// </summary>
        public bool DefaultAllowCommenting { get; set; }

        /// <summary>
        ///     Legt fest ob beim Hochladen oder Webhook ein bestehendes Bild mit dem gleichen Namen
        ///     überschrieben werden soll
        /// </summary>
        public bool OverwriteExistingPhoto { get; set; }

        /// <summary>
        ///     Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>
        public override string ToString()
        {
            return
                string.Format(
                    "MemberId: {0}, IsAvailableForProjects: {1}, IsProfessional: {2}, DisplayEmailAddress: {3}, DisplayNotifications: {4}, NotifyByEmailOnComment: {5}, NotifyByEmailOnRating: {6}, NotifyByEmailOnBuddyAdd: {7}, NotifyByEmailOnBuddyConfirmation: {8}, NotifyByEmailOnBuddyAddedPhoto: {9}, NotifyByEmailOnNews: {10}, AllowComments: {11}, AllowRating: {12}, AllowSharing: {13}, Language: {14}, UseDropboxWebhook: {15}, DefaultIsPrivate: {16}, DefaultLicense: {17}, DefaultAllowPromoting: {18}, DefaultAllowSharing: {19}, DefaultAllowFullSizeDownload: {20}, DefaultAllowRating: {21}, DefaultAllowCommenting: {22}, OverwriteExistingPhoto: {23}",
                    MemberId, IsAvailableForProjects, IsProfessional, DisplayEmailAddress, DisplayNotifications,
                    NotifyByEmailOnComment, NotifyByEmailOnRating, NotifyByEmailOnBuddyAdd,
                    NotifyByEmailOnBuddyConfirmation, NotifyByEmailOnBuddyAddedPhoto, NotifyByEmailOnNews, AllowComments,
                    AllowRating, AllowSharing, Language, UseDropboxWebhook, DefaultIsPrivate, DefaultLicense,
                    DefaultAllowPromoting, DefaultAllowSharing, DefaultAllowFullSizeDownload, DefaultAllowRating,
                    DefaultAllowCommenting, OverwriteExistingPhoto);
        }
    }
}