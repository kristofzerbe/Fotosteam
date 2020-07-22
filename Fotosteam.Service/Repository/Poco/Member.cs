using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fotosteam.Service.Models;

namespace Fotosteam.Service.Repository.Poco
{
    /// <summary>
    ///     Das Objekt repräsentiert einen Anwender von Fotosteam.
    ///     Dieser muss intern mit einem AspNetUser verknüpft werden
    /// </summary>
    public class Member : PocoBase
    {
        /// <summary>
        ///     Der Spitzname des Anwenders. Dieser muss eindeutig sein
        /// </summary>
        [Required]
        public string Alias { get; set; }

        /// <summary>
        ///     Der vollständige Name
        /// </summary>
        [Required]
        public string PlainName { get; set; }

        /// <summary>
        ///     Die Emailadress. Diese dient zur authentifzierung des Benutzers
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        ///     Der direkte Link zum Profilbild mit 100px
        /// </summary>
        public string Avatar100Url { get; set; }

        /// <summary>
        ///     Der direkte Link zum Profilbild mit 200px
        /// </summary>
        public string Avatar200Url { get; set; }

        /// <summary>
        ///     Dominante Farbe des Avatar-Bildes
        /// </summary>
        public string AvatarColor { get; set; }

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

        /// <summary>
        ///     Die eindeutige Identifikation des externen Authenfizierungsdienst, z.B. von Google
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        ///     Name des Authentifizierungsdienstes
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        ///     Name des Users beim Authentifizierungsdienst
        /// </summary>
        public string ProviderUserName { get; set; }

        /// <summary>
        ///     Mail-Adresse des Users beim Authentifizierungsdienst
        /// </summary>
        public string ProviderUserMail { get; set; }

        /// <summary>
        ///     Die Id zum internen Anwendungsnutzer
        /// </summary>
        internal string AspNetUserId { get; set; }

        /// <summary>
        ///     Die Geschichten des Benutzers
        /// </summary>
        public ICollection<Story> Stories { get; set; }

        /// <summary>
        ///     Die Orte des Benutzers
        /// </summary>
        public ICollection<Location> Locations { get; set; }

        /// <summary>
        ///     Definiert den Standardzugriff für den Cloud-Speicher, z.B. Dropbox oder Google
        /// </summary>
        public StorageProviderType StorageAccessType { get; set; }

        /// <summary>
        ///     Die Ereignisse des Bentzers
        /// </summary>
        public ICollection<Event> Events { get; set; }

        /// <summary>
        ///     Die Informationen zum Cloud-Zugriff, z.B. Dropbox
        /// </summary>
        /// <remarks>Diese Informationen werden nicht über die Api nach aus gegeben</remarks>
        public ICollection<MemberStorageAccess> StorageAccesses { get; set; }

        /// <summary>
        ///     Der Wohnort
        /// </summary>
        public Location HomeLocation { get; set; }

        // ReSharper disable once InconsistentNaming
        public int HomeLocation_Id { get; set; }

        /// <summary>
        ///     Ein knackiges Mottp
        /// </summary>
        public string Motto { get; set; }

        /// <summary>
        ///     Eine kurze Beschreibung, die nicht mehr als 2000 Zeichen enthalten darf
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Liste der verknüpften Benutzer
        /// </summary>
        public ICollection<Buddy> Buddies { get; set; }

        /// <summary>
        ///     Informatioen zu Konaktdaten für Twitter, Facebook usw.
        /// </summary>
        public ICollection<SocialMedia> SocialMedias { get; set; }

        /// <summary>
        ///     Liste der Fotos des Benutzers.
        /// </summary>
        public ICollection<Photo> Photos { get; set; }

        /// <summary>
        ///     Die Einstellungen des Benutzers
        /// </summary>
        public MemberOption Options { get; set; }

        public override string ToString()
        {
            return
                string.Format(
                    "Alias: {0}, PlainName: {1}, Email: {2}, Avatar100Url: {3}, Avatar200Url: {4}, AvatarColor: {5}, Header640Url: {6}, Header1024Url: {7}, Header1440Url: {8}, Header1920Url: {9}, HeaderColor: {10}, ProviderKey: {11}, ProviderName: {12}, ProviderUserName: {13}, ProviderUserMail: {14}, AspNetUserId: {15}, Stories: {16}, Locations: {17}, StorageAccessType: {18}, Events: {19}, StorageAccesses: {20}, HomeLocation: {21}, HomeLocation_Id: {22}, Motto: {23}, Description: {24}, Buddies: {25}, SocialMedias: {26}, Photos: {27}, Options: {28}",
                    Alias, PlainName, Email, Avatar100Url, Avatar200Url, AvatarColor, Header640Url, Header1024Url,
                    Header1440Url, Header1920Url, HeaderColor, ProviderKey, ProviderName, ProviderUserName,
                    ProviderUserMail, AspNetUserId, Stories, Locations, StorageAccessType, Events, StorageAccesses,
                    HomeLocation, HomeLocation_Id, Motto, Description, Buddies, SocialMedias, Photos, Options);
        }
    }
}