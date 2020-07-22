
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Controller
{
    /// <summary>
    ///     Ergebnis eines Serviceaufrufs
    /// </summary>
    public class Result<T>
    {
        /// <summary>
        ///     Standardkonstruktor, der den Status auf Erfolg setzt
        /// </summary>
        public Result()
        {
            Status = new Status {Code = StatusCode.Success, Message = ResultMessages.Success};
        }

        /// <summary>
        ///     Liefert das typsierte Datenobjekt zurück, das bei der Definition angeben wurde
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        ///     Kapselt den Status der Aktion, die zu dem Ergbnis geführt hat
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        ///     Liefert ein Standardergbnis für eine unbekannte Aktion, so dass sie nur einmal definiert werden muss
        /// </summary>
        /// <returns></returns>
        public static Result<T> GetUnkownActionResult()
        {
            return new Result<T>
            {
                Status = new Status {Code = StatusCode.UnknownAction, Message = ResultMessages.UnkownAction}
            };
        }
    }

    public class SynchProgress
    {

        /// <summary>
        /// Das aktuelle verarbeitete Fotos
        /// </summary>
        public Photo Photo { get; set; }
        /// <summary>
        /// Der aktuelle Index innerhalb der Schleife der zu verarbeitenden Fotos
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Die Gesamtanzahl aller zu verarbeiten Fotos
        /// </summary>
        public int TotalFileCount { get; set; }

        /// <summary>
        /// Formattiert das Objekt für ein textuelle Ausgabe
        /// </summary>       
        public override string ToString()
        {
            return string.Format("Photo: {0}, Index: {1}, TotalFileCount: {2}", Photo, Index, TotalFileCount);
        }
    }

    internal static class ResultMessages
    {
        internal const string Success = "Success";
        internal const string Failure = "Failure";
        internal const string NoDataAccess = "No cloud access found";
        internal const string UnkownAction = "Action is not defined";
        internal const string InvalidMessageFormat = "Invalid message format";
        internal const string UnhandledException = "Unhandled exception";
        internal const string NotAuthorized = "User is not authorized";
        internal const string NoMatchingData = "No matching data found";
        internal const string NotValidEntity = "Entity has invalid entries";
    }

    /// <summary>
    ///     Definiert die verschiedenen Statuswerte für eine Aktion
    /// </summary>
    public enum StatusCode
    {
        /// <summary> Aktion war erfolgreich </summary>
        Success = 0,

        /// <summary> Aktion ist fehlgeschlagen</summary>
        Failure = 1,

        /// <summary> Aktion hat zu lange gedauert </summary>
        Timeout = 2,

        /// <summary> Es konnte nicht ermittelt werden, was zu tun ist</summary>
        UnknownAction = 3,

        /// <summary> Aktion lief auf einen nicht erwarteten Fehler </summary>
        InternalException = 4,

        /// <summary> Der Zugriff auf den Cloud-Speicher war nicht möglich, da nicht konfiguriert </summary>
        NoStorageAccessDefined = 5,

        /// <summary> Aktion konnte nicht durchgeführt werden, da der Benutzer nicht berechtigt ist </summary>
        NotAuthorized = 6,

        /// <summary> Aktion lieferte keine Daten</summary>
        NoData = 7,

        /// <summary> Aktion war nicht erfolgreich, da die übergebenen JSON-Daten nicht gelesen werden konnten </summary>
        CannotDeserializer = 8,

        /// <summary> Aktion war nicht erfolgreich, da die Daten nicht korrekt waren </summary>
        NotValidEntity = 9,

        /// <summary> Der Benutzer hat den Zugriff für GoogleDrive schon authoriziert </summary>
        GoogleDriverAlreadyAuthorized = 10,

        /// <summary> Fehler, wenn der PRIMARY KEY in Rating verletzt wird, d.h. User hat schon bewertet</summary>
        UserHasAlreadyRated = 11,

        /// <summary>Die Synchronisierung läuft schon über den Webhook </summary>
        SynchProcessIsInProgress = 12
    }

    /// <summary>
    ///     Die Klasse kapselt die Statusinformation in einen Code und eine Meldung
    /// </summary>
    public class Status
    {
        /// <summary>
        ///     Beschreibt den Status der Aktion
        /// </summary>
        public StatusCode Code { get; set; }

        /// <summary>
        ///     Bescheibt textuell den Status der Aktion
        /// </summary>
        public string Message { get; set; }
    }
}