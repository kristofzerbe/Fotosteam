using System.Collections.Generic;
using System.IO;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Connector
{
    /// <summary>
    /// Definiert die gemeinsamen Methoden f�r einen Connector
    /// </summary>
    public interface IConnector
    {        
        /// <summary>
        /// Der aktuelle Benutzer
        /// </summary>
        Member CurrentMember { get; set; }
        /// <summary>
        ///     Gibt den Pfad oder die OrdnerId f�r den MemberOrdner zur�ck
        /// </summary>
        string MemberPathOrId { get; }

        /// <summary>
        ///     F�hrt die Authorizierung des Connectors durch
        /// </summary>
        /// <param name="access">Die notwendingen Zugruffsdaten</param>
        /// <remarks>Nicht alle Connectoren ben�tigen diese Daten</remarks>
        void Connect(MemberStorageAccess access);

        /// <summary>
        ///     Aktualisiert die Bilder aus dem ToPublish-Ordner
        /// </summary>
        /// <returns>Liste mit den gefundenen Fotos</returns>
        IEnumerable<SynchProgress> RefreshFolderContent();

        /// <summary>
        ///     L�dt das Avatarbild f�r einen Benutzer hoch
        /// </summary>
        /// <param name="imageStream">Das Bild als Stream</param>
        /// <returns>Array mit den Links zu dem Avatarbilder </returns>
        AvatarInformation UploadAvatarImage(Stream imageStream);

        /// <summary>
        ///     L�dt das Hintergrundbild f�r einen Benutzer hoch
        /// </summary>
        /// <param name="imageStream">Das Bild als Stream</param>
        /// <returns>Array mit den Links zu den Headerbildern</returns>
        HeaderInformation UploadHeaderImage(Stream imageStream);

        /// <summary>
        /// L�dt ein neues Foto zu dem Provider hoch
        /// </summary>
        /// <param name="imageStream">Stream des Fotos</param>
        /// <param name="fileName">Name des Fotos</param>
        /// <returns></returns>
        Photo UploadPhoto(Stream imageStream, string fileName);


        /// <summary>
        /// Aktualisiert ein vorhandenes Foto mit dem �bergebenen
        /// </summary>
        /// <param name="imageStream">Stream des Fotos</param>
        /// <param name="existingPhoto">Daten es existierenden Fotos</param>        
        /// <returns></returns>
        Photo UpdatePhoto(Stream imageStream, Photo existingPhoto);

        /// <summary>
        /// Aktualisiert das Original zu einem Foto. Das Foto wird auf 640px reduziert
        /// </summary>
        /// <param name="imageStream">Stream des Fotos</param>
        /// <param name="existingPhoto">Daten es existierenden Fotos</param>
        /// <returns></returns>
        Photo UpdateOrignalPhoto(Stream imageStream, Photo existingPhoto);
        

        /// <summary>
        ///     Aktualisiert die quadratischen Bilder mit dem neuen Bereich
        /// </summary>
        /// <param name="imageStream">Bild as Stream</param>
        /// <param name="folder">Ordner in das originale Bild gespeichert ist</param>
        /// <param name="xPercentage">Prozentueller Abstand vom linken Rand</param>
        /// <param name="yPercentage">Prozentueller Abstand vom unteren Rand</param>
        /// <param name="forceOffset">Gibt an ob der Abstand �berschrieben werden soll</param>
        /// <returns>Array mit den Links zu den quadratischen Bildern</returns>
        string[] ChangeSquareImages(Stream imageStream, string folder, float xPercentage, float yPercentage, bool forceOffset);

        /// <summary>
        /// Die Redirect Url, die beim Provider hinterlegt ist
        /// </summary>
        string RedirectUrl { get; set; }
        /// <summary>
        /// Liefert den Type des Providers zur�ck
        /// </summary>
        StorageProviderType ProviderType { get; }

        /// <summary>
        /// Gibt an, ob die Synchronisierung l�uft
        /// </summary>
        bool IsSynchInProgress { get; set; }
    }
}