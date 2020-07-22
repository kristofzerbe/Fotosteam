using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Imaging;
using Fotosteam.Service.Models;
using Fotosteam.Service.Properties;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using log4net;
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable PossibleLossOfFraction

namespace Fotosteam.Service.Connector
{
    /// <summary>
    ///     Die Klasse kapselt die gemeinsamen Implementierungen für die unterschiedlichen Cloud-Connectoren
    /// </summary>
    /// <typeparam name="T">Der Typ, der die Liste der Dateien definiert</typeparam>
    public abstract class ConnectorBase<T> : IConnector
    {
        protected const string AppPath = "Fotosteam";
        protected const string PhotoPath = "Fotos";
        protected const string InternalPath = "Internal";
        protected const string ToPublishPath = "ToPublish";
        protected const string GlitchPath = "Glitch";
        protected const string MemberPath = "Member";
        protected const string FotosJsonFilename = "fotos.json";
        protected const string MetaJsonFilename = "meta.json";
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected int Index;
        protected int TotalFileCount;

        protected static readonly HashSet<string> CurrentSynchs = new HashSet<string>();
        internal static bool IsSynchInProgressForUser(string userId)
        {
            return CurrentSynchs.Contains(userId);
        }
        /// <summary>
        /// Gibt an, ob eine Synchronisierung schon gestartet ist.
        /// Dies ist für den Webhook wichtig, aber verhindert auch, dass der Benutzer 
        /// mehrfach die Synchronisierung über die Webseite anstößt
        /// </summary>
        public virtual bool IsSynchInProgress { get; set; }


        protected IDataRepository Repository;

        protected Stream GetReadmeStream()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(Resources.readme)) { Position = 0 };
        }

        /// <summary>
        /// Die zur Authentifizierung notwendige URL. Ist Providerspezifisch
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Verbinden den Connector mit den Provider
        /// </summary>
        /// <param name="access">Die Informationen, die zur Anmeldung notwendig sind</param>
        public abstract void Connect(MemberStorageAccess access);

        /// <summary>
        /// Sorgt dafür, dass der implementierende Connector die Fotos aus der Datenbank löscht,
        /// die nicht mehr bei dem Provider vorhanden sind
        /// </summary>
        public abstract void DeleteNotExistingPhotos();

        /// <summary>
        /// Der Type des implementierenden Connectors
        /// </summary>
        public abstract StorageProviderType ProviderType { get; }

        internal protected List<string> GetPhotoFolders()
        {
            return Repository.Queryable<Photo>().Where(p => p.MemberId == CurrentMemberId && p.StorageAccessType == ProviderType).Select(p => p.Folder.ToLower()).ToList();
        }

        /// <summary>
        /// Führt die Synchronsierung durch. Dabei wird die Struktur überprüft,
        /// nicht vorhandene Bilder gelöscht und dann die Fotos verarbeitet.
        /// Dies ist eine Template-Method. Die konkreten Aktionen werden von
        /// den implementierenden Connectoren durchgeführt
        /// </summary>
        /// <returns>Nach dem Auslesen der Gesamtanzahl der Fotos wird ein SynchProgess-Objekt zurückgegeben
        /// Danach pro Foto ein entsprechendes Objekt</returns>
        public IEnumerable<SynchProgress> RefreshFolderContent()
        {
            CheckFolderStructure();
            DeleteNotExistingPhotos();
            var fileList = GetFileList();
            yield return new SynchProgress() { TotalFileCount = TotalFileCount, Index = 0, Photo = null };

            if (TotalFileCount == 0)
                yield break;

            foreach (var progress in UploadPhotos(fileList))
            {
                yield return progress;
            }
        }

        /// <summary>
        /// Aktualisiert das Avatar-Bild des Benutzer
        /// </summary>
        /// <param name="imageStream">Stream mit den Daten des Bildes</param>
        /// <returns><see cref="AvatarInformation"/></returns>
        public AvatarInformation UploadAvatarImage(Stream imageStream)
        {
            var newPath = MemberPathOrId;

            var sizes = new[] { 200, 100 };
            var urls = new string[sizes.Length + 1];
            for (var i = 0; i < sizes.Length; i++)
            {
                var fileName = string.Format(@"avatar-{0}.jpg", sizes[i]);
                urls[i] = CreateThumbnailAndUpload(ref imageStream, newPath, fileName, sizes[i], true);
            }
            urls[urls.Length - 1] = Processing.GetDominantColor(imageStream);

            return new AvatarInformation
             {
                 Avatar200Url = urls[0],
                 Avatar100Url = urls[1],
                 AvatarColor = urls[2]
             };
        }

        /// <summary>
        /// Aktualisiert das Kopf-Bild des Benutzer
        /// </summary>
        /// <param name="imageStream">Stream mit den Daten des Bildes</param>
        /// <returns><see cref="HeaderInformation"/></returns>
        public HeaderInformation UploadHeaderImage(Stream imageStream)
        {
            var newPath = MemberPathOrId;
            var sizes = new[] { 1920, 1440, 1024, 640 };
            var urls = new string[sizes.Length + 1];
            for (var i = 0; i < sizes.Length; i++)
            {
                imageStream.Seek(0, SeekOrigin.Begin);
                var fileName = string.Format(@"{0}.jpg", sizes[i]);
                urls[i] = CreateThumbnailAndUpload(ref imageStream, newPath, fileName, sizes[i]);
            }
            urls[urls.Length - 1] = Processing.GetDominantColor(imageStream);

            return new HeaderInformation
            {
                Header640Url = urls[3],
                Header1024Url = urls[2],
                Header1440Url = urls[1],
                Header1920Url = urls[0],
                HeaderColor = urls[4]
            };
        }

        protected abstract void BackupExistingFiles(string folderId);
        protected abstract void DeleteBackupedFiles(string folderId);
        protected abstract void RollbackBackupedFiles(string folderName);

        /// <summary>
        /// Aktualisiert ein vorhandenes Foto mit dem übergebenen
        /// </summary>
        /// <param name="imageStream">Stream des Fotos</param>
        /// <param name="existingPhoto">Daten es existierenden Fotos</param>
        /// <returns></returns>
        public Photo UpdatePhoto(Stream imageStream, Photo existingPhoto)
        {
            try
            {
                var folderId = existingPhoto.Folder;
                existingPhoto.DirectLinks.Clear();
                BackupExistingFiles(folderId);

                ReadMetaInformation(imageStream, existingPhoto);

                if (Processing.CorrectImageRotation(ref imageStream, existingPhoto.Exif) !=
                    RotateFlipType.RotateNoneFlipNone)
                {
                    CorrectPhotoInformation(existingPhoto);
                }

                imageStream.Seek(0, SeekOrigin.Begin);
                var fileId = UploadFile(folderId, FullsizeFileName, imageStream, true);
                var link = GetContentLink(fileId);

                CreateThumbNails(imageStream, folderId, existingPhoto);

                var domColor = Processing.GetDominantColor(imageStream);
                existingPhoto.Color = domColor;

                imageStream.Close();
                imageStream.Dispose();

                existingPhoto.DirectLinks.Add(new DirectLink { Size = 0, Url = link });
                CheckForExistingLocation(existingPhoto);
                ExtractCategoriesAndTopics(existingPhoto);

                DeleteBackupedFiles(folderId);

                return existingPhoto;
            }
            catch
            {
                ProcessGlitch(imageStream, existingPhoto.Name, true);
                //Der Fehler muss nach oben gereicht werden, damit er in der obersten Ebene behandelt wird
                throw;
            }
        }

        /// <summary>
        /// Aktualisiert das Original zu einem Foto. Das Foto wird auf 640px reduziert
        /// </summary>
        /// <param name="imageStream">Stream des Fotos</param>
        /// <param name="existingPhoto">Daten es existierenden Fotos</param>
        /// <returns></returns>
        public Photo UpdateOrignalPhoto(Stream imageStream, Photo existingPhoto)
        {
            try
            {
                var folderId = existingPhoto.Folder;
                
                if (Processing.CorrectImageRotation(ref imageStream, existingPhoto.Exif) !=
                    RotateFlipType.RotateNoneFlipNone)
                {
                    CorrectPhotoInformation(existingPhoto);
                }

                imageStream.Seek(0, SeekOrigin.Begin);

                var url = CreateThumbnailAndUpload(ref imageStream, folderId, string.Format(OriginalFileName, Photo.Orignal640UrlSize-1), Photo.Orignal640UrlSize);
                var link = existingPhoto.DirectLinks.FirstOrDefault(l => l.Size == Photo.Orignal640UrlSize);
                if (link != null)
                {
                    existingPhoto.DirectLinks.Remove(link);
                }
                
                existingPhoto.DirectLinks.Add(new DirectLink { Size = Photo.Orignal640UrlSize, Url = url });

                imageStream.Close();
                imageStream.Dispose();
                
                return existingPhoto;
            }
            catch
            {
                ProcessGlitch(imageStream, existingPhoto.Name, true);
                //Der Fehler muss nach oben gereicht werden, damit er in der obersten Ebene behandelt wird
                throw;
            }
        }

        private void ProcessGlitch(Stream imageStream, string folderName, bool isExisting)
        {
            try
            {
                //Falls es noch keinen Namen gibt, dann können wir nicht aufräumen
                if (string.IsNullOrEmpty(folderName))
                    return;

                //Ohne Stream kann keine Datei erzeugt werden
                if (imageStream == null)
                    return;

                CheckForGlitchFolder();

                imageStream.Seek(0, SeekOrigin.Begin);
                UploadFileToGlitch(folderName + ".jpg", imageStream, true);

                if (isExisting)
                {
                    RollbackBackupedFiles(folderName);
                    return;
                }

                DeletePhotoFolder(folderName);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        /// <summary>
        /// Erzeugt ein quadratische Bild von einer bestimmten Stelle eines Bildes
        /// </summary>
        /// <param name="imageStream">Stream mit den Bilddaten</param>
        /// <param name="folder">Name oder Id des Ordners wo die Bilder ersetzt werden sollen</param>
        /// <param name="xPercentage">Der horizontale Offset wo das Quadrat anfangen soll</param>
        /// <param name="yPercentage">Der vertikale Offset wo das Quadrat anfangen soll</param>
        /// <param name="forceOffset">Gibt an, dass der Offset überschrieben werden soll, selbst wenn x und y auf 0 stehen</param>
        /// <returns>Array mit den DirectLinks in den Größen 400,200,100</returns>
        public string[] ChangeSquareImages(Stream imageStream, string folder, float xPercentage, float yPercentage, bool forceOffset =false)
        {
            var sizes = new[] { 400, 200, 100 };
            var urls = new string[sizes.Length];
            for (var i = 0; i < sizes.Length; i++)
            {
                var fileName = string.Format(@"{0}.jpg", sizes[i]);
                urls[i] = CreateThumbnailAndUpload(ref imageStream, folder, fileName, sizes[i], true,
                    xPercentage , yPercentage, forceOffset );
            }
            
            if (imageStream != Stream.Null)
            {
                imageStream.Close();
                imageStream.Dispose();
            }

            return urls;
        }

        const string FullsizeFileName = "full.jpg";
        private const string OriginalFileName = "original{0}.jpg";

        protected abstract string MoveFile(T source, string destination, string newFileName);
        protected abstract void DeleteFile(T source);
        protected abstract void DeletePhotoFolder(string folderName);

        /// <summary>
        ///     Lädt ein Foto für einen Benutzer hoch
        /// </summary>
        /// <param name="imageStream">Das Bild als Stream</param>
        /// <param name="fileInfo">Repräsenitiert die Information zu dem Bild für den jeweiligen Provider</param>
        /// <returns>Ein Photo-Objekt mit allen Links</returns>
        public Photo UploadPhoto(Stream imageStream, T fileInfo)
        {
            var isUpdate = false;
            var newFolderName = string.Empty;
            var currentFile = fileInfo;
            try
            {
                var fileName = GetFileName(fileInfo);
                newFolderName = MakeFileSystemSafeFolderName(fileName);

                if (CurrentMember.Options.OverwriteExistingPhoto)
                {
                    var existingPhoto = GetExistingPhoto(newFolderName);

                    if (existingPhoto != null)
                    {
                        isUpdate = true;
                        existingPhoto = UpdatePhoto(imageStream, existingPhoto);
                        DeleteFile(fileInfo);
                        return existingPhoto;
                    }
                }

                var folderId = CreateNewPhotoFolder(ref newFolderName);

                var photo = CreatePhoto(fileName, newFolderName, folderId);

                var fileId = MoveFile(fileInfo, folderId, FullsizeFileName);

                ReadMetaInformation(imageStream, photo);

                if (Processing.CorrectImageRotation(ref imageStream, photo.Exif) !=
                    RotateFlipType.RotateNoneFlipNone)
                {
                    imageStream.Seek(0, SeekOrigin.Begin);
                    fileId = UploadFile(folderId, FullsizeFileName, imageStream, true);
                    CorrectPhotoInformation(photo);
                }

                var link = GetContentLink(fileId);

                if (string.IsNullOrEmpty(photo.Title) || string.IsNullOrEmpty(photo.Title.Trim()))
                    photo.Title = "Untitled";

                CreateThumbNails(imageStream, folderId, photo);

                var domColor = Processing.GetDominantColor(imageStream);
                photo.Color = domColor;

                imageStream.Close();
                imageStream.Dispose();

                photo.DirectLinks.Add(new DirectLink { Size = 0, Url = link });
                CheckForExistingLocation(photo);
                ExtractCategoriesAndTopics(photo);

                return photo;
            }
            catch
            {
                if (!isUpdate)
                    ProcessGlitch(imageStream, newFolderName, false);
                
                DeleteFile(currentFile);
                
                throw;
            }
        }

        private Photo GetExistingPhoto(string folder)
        {
            var existingPhoto =
                Repository.Queryable<Photo>()
                    .Include(p => p.Topics)
                    .Include(p => p.Exif)
                    .Include(p => p.DirectLinks)
                    .FirstOrDefault(
                        p => p.Name.ToLower().Equals(folder.ToLower()) && p.MemberId == CurrentMemberId);

            return existingPhoto;
        }

        internal void ExtractCategoriesAndTopics(Photo photo)
        {
            if (photo.Exif == null || photo.Exif.Keywords == null || !photo.Exif.Keywords.Any())
                return;
            foreach (var keyword in photo.Exif.Keywords)
            {
                CategoryType category;

                int categoryValue;
                if (!int.TryParse(keyword, out categoryValue) && Enum.TryParse(keyword, true, out category))
                {
                    photo.Category = photo.Category | category;
                }
                else
                {
                    var topic =
                        Repository.Queryable<Topic>()
                            .FirstOrDefault(
                                t => t.MemberId == CurrentMemberId && t.Name.ToLower().Equals(keyword.ToLower()));

                    if (topic == null) continue;

                    if (photo.Topics == null)
                        photo.Topics = new List<Topic>();

                    if (!photo.Topics.Any(t => t.Name.ToLower().Equals(keyword.ToLower())))
                        photo.Topics.Add(topic);
                }
            }
        }
        private static void CorrectPhotoInformation(Photo photo)
        {
            photo.Width = photo.Exif.Width;
            photo.Height = photo.Exif.Height;
            photo.Orientation = photo.Exif.Orientation;
            photo.AspectRation = photo.Exif.AspectRatio;
        }

        private Photo CreatePhoto(string fileName, string folder, string folderId)
        {
            var photo = new Photo
            {
                DirectLinks = new List<DirectLink>(),
                StorageAccessType = ProviderType,
                OriginalName = fileName,
                Name = folder,
                Folder = folderId,
                Member = null,
                MemberId = CurrentMemberId,
                AllowFullSizeDownload = CurrentMember.Options.DefaultAllowFullSizeDownload,
                IsPrivate = CurrentMember.Options.DefaultIsPrivate,
                AllowPromoting = CurrentMember.Options.DefaultAllowPromoting,
                AllowSharing = CurrentMember.Options.DefaultAllowSharing,
                AllowCommenting = CurrentMember.Options.DefaultAllowCommenting,
                AllowRating = CurrentMember.Options.AllowRating,
                License = CurrentMember.Options.DefaultLicense
            };
            return photo;
        }

        private void CheckForExistingLocation(Photo photo)
        {
            var currentLocation = photo.Exif.Location;

            if (currentLocation == null)
            {
                return;
            }

            var existingLocation =
                Repository.Queryable<Location>().FirstOrDefault(
                l => l.Longitude == currentLocation.Longitude
                    && l.Latitude == currentLocation.Latitude
                    && l.MemberId == CurrentMemberId);

            if (existingLocation != null)
            {
                if (string.IsNullOrEmpty(existingLocation.City))
                {
                    existingLocation.City = currentLocation.City;
                }

                if (string.IsNullOrEmpty(existingLocation.Country))
                {
                    existingLocation.Country = currentLocation.Country;
                }

                if (string.IsNullOrEmpty(existingLocation.State))
                {
                    existingLocation.State = currentLocation.State;
                }

                if (string.IsNullOrEmpty(existingLocation.Street))
                {
                    existingLocation.Street = currentLocation.Street;
                }

                existingLocation.PhotoCount++;
                photo.Location = existingLocation;
            }
            else
            {
                currentLocation.MemberId = CurrentMemberId;
                currentLocation.PhotoCount = 1;
                photo.Location = currentLocation;
            }
        }

        /// <summary>
        /// Lädt ein einzelnes Foto hoch
        /// </summary>
        /// <param name="imageStream">Stream mit den Bilddaten</param>
        /// <param name="fileName">Name des Bildes</param>
        /// <returns>Ein <see cref="Photo"/>Objekt</returns>
        public Photo UploadPhoto(Stream imageStream, string fileName)
        {
            var isUpating = false;
            var newFolderName = fileName;
            try
            {
                newFolderName = MakeFileSystemSafeFolderName(fileName);

                if (CurrentMember.Options.OverwriteExistingPhoto)
                {
                    var existingPhoto = GetExistingPhoto(newFolderName);

                    if (existingPhoto != null)
                    {
                        isUpating = true;
                        return UpdatePhoto(imageStream, existingPhoto);
                    }
                }

                var folderId = CreateNewPhotoFolder(ref newFolderName);
                
                var photo = CreatePhoto(fileName, newFolderName, folderId);
                
                ReadMetaInformation(imageStream, photo);

                if (Processing.CorrectImageRotation(ref imageStream, photo.Exif) != RotateFlipType.RotateNoneFlipNone)
                {
                    CorrectPhotoInformation(photo);
                }

                imageStream.Seek(0, SeekOrigin.Begin);
                var fileId = UploadFile(folderId, FullsizeFileName, imageStream, true);
                var link = GetContentLink(fileId);

                if (string.IsNullOrEmpty(photo.Title) || string.IsNullOrEmpty(photo.Title.Trim()))
                    photo.Title = "Untitled";
                if (string.IsNullOrEmpty(photo.Name)) photo.Name = fileName;

                CreateThumbNails(imageStream, folderId, photo);

                var domColor = Processing.GetDominantColor(imageStream);
                photo.Color = domColor;

                imageStream.Close();
                imageStream.Dispose();

                photo.DirectLinks.Add(new DirectLink { Size = 0, Url = link });
                CheckForExistingLocation(photo);
                ExtractCategoriesAndTopics(photo);

                return photo;
            }
            catch
            {
                //Wenn ein Foto aktualisiert wird, dann wird in der Methode schon aufgeräumt, deshalb gibt es hier nichts zu tun
                if (!isUpating)
                    ProcessGlitch(imageStream, newFolderName, false);
                //Der Fehler muss nach oben gereicht werden, damit er in der obersten Ebene behandelt wird
                throw;
            }
        }

        protected abstract string GetFileName(T source);

        /// <summary>
        /// Der aktuelle Benutzer, der die Aktion angestoßen hat
        /// </summary>
        public Member CurrentMember { get; set; }
        protected virtual int CurrentMemberId { get { return CurrentMember.Id; } }

        /// <summary>
        /// Der Pfad oder die ID zu Internal/Member-Ordner
        /// </summary>
        public abstract string MemberPathOrId { get; }

        /// <summary>
        ///     Erzeugt einen neuen Ordner und gibt den Pfad oder Id zurück
        /// </summary>
        public abstract string CreateNewPhotoFolder(ref string filename);


        internal protected static string MakeFileSystemSafeFolderName(string fileName)
        {
            var name = Path.GetFileNameWithoutExtension(fileName) ?? fileName;

            return new string(name.Where(IsFileSystemSafe).ToArray())
                .Replace("-", "_")
                .Replace("&", "_AND_")
                .Replace("#", "_")
                .Replace("%", "")
                .Replace(" ", "");
        }


        private static bool IsFileSystemSafe(char c)
        {
            return !Path.GetInvalidFileNameChars().Contains(c);
        }


        protected abstract string UploadFile(string path, string fileName, Stream stream, bool overrideFile = false);
        protected abstract string UploadFileToGlitch(string fileName, Stream stream, bool overrideFile = false);

        protected abstract void CheckFolderStructure();
        protected abstract void CheckForGlitchFolder();

        internal abstract T GetFileList(string folder = "");

        protected abstract IEnumerable<SynchProgress> UploadPhotos(T fileList);

        protected ExifData ReadMetaInformation(Stream sourceStream, Photo currentPhoto)
        {
            var meta = Processing.ReadExifData(sourceStream);
            if (meta == null)
                return null;

            currentPhoto.Width = meta.Width;
            currentPhoto.Height = meta.Height;
            currentPhoto.Orientation = meta.Orientation;
            currentPhoto.AspectRation = meta.AspectRatio;
            currentPhoto.CaptureDate = meta.CaptureDate;
            currentPhoto.PublishDate = DateTime.Now;
            currentPhoto.Exif = meta;
            currentPhoto.Title = meta.Title;
            currentPhoto.Description = meta.Description;
            if (currentPhoto.Width > currentPhoto.Height)
            {
                currentPhoto.Left = ((currentPhoto.Width - currentPhoto.Height)/ 2.0d)/currentPhoto.Width;
            }
            else
            {
                currentPhoto.Top = ((currentPhoto.Height - currentPhoto.Width) / 2.0d) / currentPhoto.Height;
            }
            return meta;
        }

        protected void CreateThumbNails(Stream sourceStream, string newPath, Photo photo)
        {

            var imageSizes = new[] { 1920, 1440, 1024, 640, 480, 400, 200, 100 };

            sourceStream.Seek(0, SeekOrigin.Begin);
            Stream imageStream = new MemoryStream();
            sourceStream.CopyTo(imageStream);

            var watch = Stopwatch.StartNew();
            foreach (var size in imageSizes)
            {

                var fileName = string.Format(@"{0}.jpg", size);
                var url = CreateThumbnailAndUpload(ref imageStream, newPath, fileName, size, size <= 400);
                photo.DirectLinks.Add(new DirectLink { Size = size, Url = url });
                imageStream.Seek(0, SeekOrigin.Begin);
                Debug.Print("{0}:{1}", size, watch.Elapsed);
                watch.Restart();
            }

            imageStream.Close();
            imageStream.Dispose();

            watch.Stop();
        }

        protected string CreateThumbnailAndUpload(ref Stream stream, string path, string fileName, int size,
            bool crop = false, float xPercentage = 0, float yPercentage = 0, bool forceOffset = false)
        {
            var processing = new Processing();
            var newImageStream = processing.ResizeImage(stream, size, crop, xPercentage, yPercentage, forceOffset);
            newImageStream.Seek(0, SeekOrigin.Begin);
            
            fileName = UploadFile(path, fileName, newImageStream, true);
            var link = GetContentLink(fileName);
            if (crop)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                stream.Close();
                stream = newImageStream;
            }
            return link;
        }

        protected abstract string GetContentLink(string url = null);

    }
}