using System;

namespace Fotosteam.Service.Models
{
    /// <summary>
    /// Allgemeine Konstanten
    /// </summary>
    internal class Constants
    {
        internal const int NotFound = -1;
        internal const int NotSetId = 0;
        internal const string NotSet = "NOT SET";
        internal const string ForAll = "ALL";
    }

    /// <summary>
    /// Platzhalter für die Ersetzung in Emailvorlagen
    /// </summary>
    internal class ReplaceToken
    {
        internal const string Salutation = "##salutation##";
        internal const string ComplimentaryClose1 = "##complimentaryClose1##";
        internal const string ComplimentaryClose2 = "##complimentaryClose2##";
        internal const string Disclaimer = "##disclaimer##";
        internal const string BetaDisclaimer = "##betaDisclaimer##";
        internal const string Title = "##title##";
        internal const string Body = "##body##";
        //--
        internal const string Alias = "##alias##";
        internal const string UserAlias = "##userAlias##";
        internal const string UserUrl = "##userUrl##";
        internal const string PhotoName = "##photoname##";
        internal const string PhotoTitle = "##phototitle##";
        internal const string PhotoUrl = "##photourl##";
        internal const string Rating = "##rating##";
        internal const string Comment = "##comment##";

    }

    /// <summary>
    ///     Gibt die Art der Anwendung für die Authentifizierung zurück
    /// </summary>
    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1
    };

    /// <summary>
    ///     Definiert die möglichen Lizenztypen für ein Bild.
    ///     Es können mehrere Lizenzen zugewiesen werden
    /// </summary>
    [Flags]
    public enum LicenseType
    {
        None = 0,           // = 0, nicht freigegeben
        CcBy = 1 << 0,      // = 1, Namensnennung (Attribution)
        CcSa = 1 << 1,      // = 2, Weitergabe unter gleichen Bedingungen (Share Alike)
        CcNc = 1 << 2,      // = 4, Nicht kommerziell (Non-Commercial)
        CcNd = 1 << 3,      // = 8, Keine Bearbeitung (No Derivatives)
        CcZero = 1 << 4,    // = 16, frei (Public Domain), nur Version 1.0
        Cc30 = 1 << 5,      // = 32, Version 3.0 (ported)
        Cc40 = 1 << 6       // = 64, Version 4.0 (unported)

        /* gültige Kombinationen
         *                     1.0  3.0  4.0
         *                     ---  ---  ---
         * CC-BY (1)        =       33   65     
         * CC-BY-SA (3)     =       35   67
         * CC-BY-NC (5)     =       37   69
         * CC-BY-ND (9)     =       41   73
         * CC-BY-NC-SA (7)  =       39   71
         * CC-BY-NC-ND (13) =       45   77
         * CC-ZERO (16)     =  16   --  --
         */
    }

    /// <summary>
    /// Definiert Felder die über multi edit aktualisiert werden können
    /// </summary>
    [Flags]
    public enum MultiEditField
    {
        IsPrivate = 1 << 0,                 //=1
        AllowFullSizeDownload = 1 << 1,     //=2
        AllowCommenting = 1 << 2,           //=4
        AllowRating = 1 << 3,               //=8
        AllowSharing = 1 << 4,              //=16
        IsForStoryOnly = 1 << 5,            //=32
        AllowPromoting = 1 << 6,            //=64
        Category = 1 << 7,                  //=128
        Topics = 1 << 8,                    //=256
        LocationId = 1 << 9,                //=512
        EventId = 1 << 10,                  //=1024
        License = 1 << 11                   //=2048
    }

    /// <summary>
    ///     Definiert die Kategorie eines Fotos. Ein Foto kann in mehrere Kategorien fallen, deshalb das Flag-Attribut
    /// </summary>
    [Flags]
    public enum CategoryType
    {
        NotSet = 0,                 //=0
        People = 1 << 0,            //=1
        Architecture = 1 << 1,      //=2
        Urban = 1 << 2,             //=4
        Landscape = 1 << 3,         //=8
        Nature = 1 << 4,            //=16
        Still = 1 << 5,             //=32
        Night = 1 << 6,             //=64
        Abstract = 1 << 7,          //=128
        Nude = 1 << 8,              //=256
        Animals = 1 << 9,           //=512
        Plants = 1 << 10,           //=1024
        Sports = 1 << 11,           //=2048
        Technics = 1 << 12          //=4096
    }

    /// <summary>
    ///     Definiert die möglichen Cloud-Dienste
    /// </summary>
    public enum StorageProviderType
    {
        None = 0,
        Dropbox = 1,
        GoogleDrive = 2,
        OneDrive = 3,
        LocalDrive = 4,
    }

    /// <summary>
    ///     Definiert die möglichen sozialen Plattformen, mit den verlinkt werden kann
    /// </summary>
    public enum MediaType
    {
        Website = 0,
        Facebook = 1,
        Google = 2,
        Twitter = 3,
        FiveHundredPx = 4,
        Flickr = 5,
        Instagram = 6,
        Tumblr = 7,
        Pinterest = 8
    }


    /// <summary>
    ///     Definiert die Möglichen Bricktypen, die innerhalb einer Story verwendet werden können
    /// </summary>
    public enum BrickType
    {
        Text,
        Map,
        Photo
    }

    /// <summary>
    ///     Definiert die möglichen Aktionen, die für HTTP-Get zugelassen sind
    /// </summary>
    public static class GetAction
    {
        public const string ByTopic = "topic";
        public const string ByLocation = "location";
        public const string ByCity = "city";
        public const string ByCountry = "country";
        public const string ByEvent = "event";
        public const string ByCatergory = "category";
        public const string ByJournal = "journal";
        public const string CommentsForPhoto = "commentsforphoto";
        public const string RatingsForPhoto = "ratingsforphoto";
        public const string NewComments = "newcomments";
        public const string Topics = "topics";
        public const string Locations = "locations";
        public const string LocationGroups = "locationgroups";
        public const string Events = "events";
        public const string Categories = "categories";
        public const string Photos = "photos";
        public const string NewPhotos = "newphotos";
        public const string LatestPhotosForUser = "latestphotosforuser";
        public const string TopRatedPhotos = "topratedphotos";
        public const string MyTopRatedPhotos = "mytopratedphotos";
        public const string Stories = "stories";
        public const string UnReadNotifications = "unreadnotifications";
        public const string AllNotifications = "allnotifications";
        public const string RandomPhotos= "randomphotos";
        public const string Find = "find";
        public const string CC0Photos = "cc0";
    }

    /// <summary>
    ///     Definiert die möglichen Aktionen, die für HTTP-Put oder HTTP-Post zugelassen sind
    /// </summary>
    public static class PostOrPutAction
    {
        public const string Comment = "comment";
        public const string Rate = "rate";
        public const string Photo = "photo";
        public const string PhotoTopic = "phototopic";
        public const string Exif = "exif";
        public const string Location = "location";
        public const string Event = "event";
        public const string Topic = "topic";
        public const string Story = "story";
        public const string Chapter = "chapter";
        public const string Ledge = "ledge";
        public const string Brick = "brick";
        public const string Thumbs = "thumbs";
        public const string Member = "member";
        public const string MemberOptions = "memberoptions";
        public const string SocialMedia = "socialmedia";
        public const string ColorReset = "colorreset";
        public const string ColorResetheader = "colorresetheader";
        public const string ColorResetAvatar = "colorresetavatar";
        public const string Update = "update";
        public const string MergeLocations = "mergelocations";
        public const string Buddy = "addbuddy";
        public const string ConfirmBuddy = "confirmbuddy";
        public const string MultiUpdate = "multiupdate";
        public const string UpdatePhoto = "updatephoto";
        public const string UpdateOriginalPhoto = "updateoriginalphoto";
    }

    /// <summary>
    /// Aufzählung zum Koppeln von Objekttypen in einem Dictionary.
    /// Wird für die Aktualisierung von Objekten auf Eigenscahftsebene benötigt
    /// </summary>
    public enum ModelType
    {
        Comment,
        Rate,
        Photo,
        Exif,
        Location,
        Event,
        Topic,
        Story,
        Chapter,
        Ledge,
        TextBrick,
        PhotoBrick,
        MapBrick,
        Member,
        SocialMedia,
        MemberOption,
        Notification
    }

    /// <summary>
    ///     Definiert die möglichen Aktionen, die für HTTP-Delete zugelassen sind
    /// </summary>
    public static class DeleteAction
    {
        public const string Topic = "topic";
        public const string Location = "location";
        public const string Event = "event";
        public const string Photo = "photo";
        public const string Story = "story";
        public const string Chapter = "chapter";
        public const string TextBrick = "textbrick";
        public const string PhotoBrick = "photobrick";
        public const string MapBrick = "mapbrick";
        public const string Ledge = "ledge";
        public const string Member = "member";
        public const string Buddy = "buddy";
        public const string Exif = "exif";
        public const string SocialMedia = "socialmedia";
        public const string Comment = "comment";
    }

    /// <summary>
    /// Definiert die Groupierungsebene für Locationen
    /// </summary>
    public enum LocationGrouping
    {
        Country,
        County,
        City
    }

    /// <summary>
    /// Definiert die verschiedenen Type von Benachrichtigungen
    /// auf die in der Anwendung reagiert werden muss
    /// </summary>
    public enum NotificationType
    {
        General = 0,
        Comment = 1,
        Rating = 2,
        BuddyRequest = 3,
        BuddyConfirmation = 4,
        BuddyNewPhoto = 5,
        DropboxSynchronization = 6,
        PhotoSynch =7
    }

    /// <summary>
    /// Definiert die von der Anwendung unterstützen Sprachen
    /// </summary>
    public enum MemberLanguage
    {
        de = 0,
        en = 1
    }
}