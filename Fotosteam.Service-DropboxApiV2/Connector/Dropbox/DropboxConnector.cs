using System;
using Dropbox.Api.Files;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Api;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using RestSharp;

namespace Fotosteam.Service.Connector.Dropbox
{
    /// <summary>
    ///     Diese Klasse stellt die intere Api zur Kommunikation mit der Dropbox her.
    ///     Sie ist dafür zuständig den Inbox-Ordner zu synchronisieren.
    /// </summary>
    internal class DropboxConnector : ConnectorBase<ListFolderResult>
    {
        private DropboxClient _dropnetClient;
        public DropboxConnector(IDataRepository repository)
        {
            Repository = repository;
        }

        public DropboxConnector() { }

        private DropboxClient Client
        {
            get
            {
                if (_dropnetClient != null)
                    return _dropnetClient;

                var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
                {
                    Timeout = TimeSpan.FromMinutes(20)
                };
                _dropnetClient = new DropboxClient(_access.Token);//, userAgent: "Fotosteaam", httpClient: httpClient);
                
                return _dropnetClient;
            }
        }

        /// <summary>
        ///     Gibt den Pfad oder die OrdnerId für den MemberOrdner zurück
        /// </summary>
        public override string MemberPathOrId
        {
            get { return string.Format("/{0}/{1}", InternalPath, MemberPath); }
        }

        /// <summary>
        /// Definiert, ob schon eine Synchronisierung läuft.
        /// Dies ist wichtig, da der Webhook für jede Änderung feuert, wir aber aber
        /// nicht unabhängige Threads mit jeweils einer Datei laufen lassen möchten.
        /// </summary>
        public override bool IsSynchInProgress
        {
            get
            {
                return base.IsSynchInProgress;
            }
            set
            {
                if (value)
                {
                    if (!CurrentSynchs.Contains(_userId))
                    {
                        CurrentSynchs.Add(_userId);
                        WebHookSession.Instance.AddUserToSession(_userId);
                    }
                }
                else
                {
                    if (CurrentSynchs.Contains(_userId))
                    {
                        CurrentSynchs.Remove(_userId);
                        WebHookSession.Instance.RemoveUserFromSession(_userId);
                    }
                }
                base.IsSynchInProgress = value;
            }
        }
        private string _userId;
        private MemberStorageAccess _access;
        /// <summary>
        ///     Führt die Authorizierung des Connectors durch
        /// </summary>
        /// <param name="access">Die notwendigen Zugriffdaten</param>
        public override void Connect(MemberStorageAccess access)
        {
            _userId = access.UserId;
            _access = access;
        }

        protected override string MoveFile(ListFolderResult source, string destination, string newFileName)
        {
            var destinationPath = string.Format(@"{0}/{1}", destination, newFileName);
            return Task.Run( () => Client.Files.MoveAsync(source.Entries[0].PathLower, destinationPath)).Result.PathLower;
        }

        /// <summary>
        ///     Erzeugt einen neuen Ordner und gibt den Pfad oder Id zurück
        /// </summary>
        public override string CreateNewPhotoFolder(ref string folder)
        {
            var path = string.Format("/{0}/{1}", InternalPath, PhotoPath);
            var data = Task.Run( () =>Client.Files.ListFolderAsync(path)).Result;
            var toCheck = folder;
            if (data.Entries.Any(e => e.IsFolder && e.Name.Contains(toCheck.ToLower())))
            {
                var count = data.Entries.Count(x => x.IsFolder && x.Name.ToLower().StartsWith(toCheck.ToLower()));
                folder = string.Format("{0}_{1}", folder, count);
            }
            path = string.Format("/{0}/{1}/{2}", InternalPath, PhotoPath, folder);
            Task.Run( () =>Client.Files.CreateFolderAsync(path)).RunSynchronously();
            return path;
        }

        /// <summary>
        ///     Liefert die URL für die Authorisierung der Anwendung zurück
        ///     Dies ist der 1. Schritt
        /// </summary>
        /// <param name="callbackUrl">Die Url, die nach der Authorisierung aufgerufen werden soll</param>
        /// <param name="state">GUID zur Wiedererkennung des Requests</param>
        /// <returns>Die Url für die Authorisierung</returns>
        public string GetAuthorizeUrl(string callbackUrl, string state)
        {
            return DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, Startup.DropboxAppKey, new Uri(callbackUrl), state).ToString();
        }

        /// <summary>
        ///     Wird aufgerufen nachdem der Benutzer den Zugriff authorisiert hat
        ///     Die ist der 2. Schritt
        /// </summary>
        /// <returns></returns>
        public Member GetUserAuthentication(Uri requestUri)
        {
            var result = DropboxOAuth2Helper.ParseTokenFragment(requestUri);

            var isNew = false;
            var access = CurrentMember.StorageAccesses.FirstOrDefault(
                    s => s.Type == StorageProviderType.Dropbox && s.MemberId == CurrentMember.Id);

            if (access == null)
            {
                access = new MemberStorageAccess
                {
                    Type = StorageProviderType.Dropbox,
                    MemberId = CurrentMember.Id,
                };

                isNew = true;
            }

            access.Token = result.AccessToken;
            access.UserId = result.Uid;

            if (isNew)
            {
                Repository.Add(access);
                CurrentMember.StorageAccessType = StorageProviderType.Dropbox;
                Repository.Update(CurrentMember);
            }
            else
            {
                Repository.Update(access);
            }
            _access = access;
            CheckFolderStructure();

            CurrentMember.StorageAccesses = new List<MemberStorageAccess> { access };
            return CurrentMember;
        }

        internal override ListFolderResult GetFileList(string folder = "")
        {
            if (string.IsNullOrEmpty(folder))
                folder = ToPublishPath;

            var list = Task.Run(() => Client.Files.ListFolderAsync(folder)).Result;
            if (list != null && list.Entries != null)
            {
                TotalFileCount = list.Entries.Count(x => x.IsFile && x.PathLower.EndsWith(".jpg"));
            }
            return list;
        }

        protected override void BackupExistingFiles(string folderId)
        {
            var files = GetFileList(folderId);
            foreach (var fileData in files.Entries)
            {
                var list = new ListFolderResult();
                list.Entries.Add(fileData);
                MoveFile(list, folderId, "_" + fileData.Name);
            }
        }

        protected override void DeleteBackupedFiles(string folderId)
        {
            var files = GetFileList(folderId);
            foreach (var fileData in files.Entries)
            {
                if (fileData.Name.StartsWith("_"))
                    Task.Run( () =>Client.Files.DeleteAsync(fileData.PathLower)).RunSynchronously();
            }
        }

        protected override void DeleteFile(ListFolderResult fileInfo)
        {
            Task.Run(() => Client.Files.DeleteAsync(fileInfo.Entries[0].PathLower)).RunSynchronously();
        }
        protected override void DeletePhotoFolder(string folderName)
        {
            var path = string.Format(@"/{0}/{1}/{2}", InternalPath, PhotoPath, folderName);
            Task.Run(() => Client.Files.DeleteAsync(path)).RunSynchronously();
        }

        protected override void RollbackBackupedFiles(string folderName)
        {
            var folderId = string.Format(@"/{0}/{1}/{2}", InternalPath, PhotoPath, folderName);
            var files = GetFileList(folderId);
            foreach (var fileData in files.Entries)
            {
                if (!fileData.PathLower.StartsWith("_"))
                    Task.Run( () =>Client.Files.DeleteAsync(fileData.PathLower)).RunSynchronously();
            }

            files = GetFileList(folderId);
            foreach (var fileData in files.Entries)
            {
                var list = new ListFolderResult();
                list.Entries.Add(fileData);
                MoveFile(list, folderId, fileData.Name.Substring(1));
            }
        }

        public override void DeleteNotExistingPhotos()
        {
            var currentPhotoFolders = GetPhotoFolders();
            if (currentPhotoFolders.Count == 0)
                return;

            var list = GetFileList(string.Format("{0}/{1}", InternalPath, PhotoPath));
            if (list != null && list.Entries != null)
            {
                var existingFolders = list.Entries.Where(c => c.IsFolder).Select(c => c.PathLower).ToList();
                var orphans = currentPhotoFolders.Except(existingFolders).ToList();
                foreach (var item in orphans)
                {
                    Repository.DeletePhotoByFolder(item);
                }
            }

        }

        protected override string UploadFile(string path, string fileName, Stream stream, bool overrideFile = false)
        {

            var mode = overrideFile ? (WriteMode)WriteMode.Overwrite.Instance : WriteMode.Add.Instance;

            Task.Run( () =>Client.Files.UploadAsync(path + "//" + fileName, mode, body: stream)).RunSynchronously();
            return string.Format("{0}/{1}", path, fileName);
        }

        protected override string UploadFileToGlitch(string fileName, Stream stream, bool overrideFile = false)
        {
            var newPath = string.Format(@"/{0}/{1}", InternalPath, GlitchPath);
            return UploadFile(newPath, fileName, stream, overrideFile);
        }

        protected override IEnumerable<SynchProgress> UploadPhotos(ListFolderResult folderInformation)
        {
            var files = folderInformation.Entries.Where(x => x.IsFile && x.PathLower.EndsWith(".jpg")).ToList();

            var currentCount = files.Count;
            TotalFileCount = currentCount;

            Index = 1;
            foreach (var fileData in files)
            {
                Log.Info(fileData.PathLower);
                var sourceStream = new MemoryStream(Task.Run( () =>Client.Files.DownloadAsync(fileData.PathLower)).Result.GetContentAsByteArrayAsync().Result);
                if (sourceStream.Length == 0 || !sourceStream.CanRead)
                {
                    Log.Info("Der Stream ist leer:" + fileData.PathLower);
                    Thread.Sleep(2000); //Warten bis die Datei aktualisiert ist
                    continue;
                }
                var list = new ListFolderResult();
                list.Entries.Add(fileData);
                var photo = UploadPhoto(sourceStream, list);
                yield return new SynchProgress() { Photo = photo, Index = Index, TotalFileCount = currentCount };
                Index++;
            }
        }


        /// <summary>
        ///     Ändert den Link, für den direkten Download für bessere Performance
        ///     https://www.dropbox.com/s/a80rykcfrcz09ms/1440.jpg?dl=1  wird zu
        ///     https://dl.dropboxusercontent.com/s/a80rykcfrcz09ms/1440.jpg
        /// </summary>
        /// <param name="fileName">Der Name der Datei</param>
        /// <returns>Liefert die geänderte Url</returns>
        protected override string GetContentLink(string fileName = null)
        {
            var url = Task.Run( () =>Client.Sharing.CreateSharedLinkAsync(fileName)).Result.Url;
            var link = url.Replace("?dl=0", "").Replace("www.dropbox.com", "dl.dropboxusercontent.com");
            return link;
        }

        public override StorageProviderType ProviderType
        {
            get { return StorageProviderType.Dropbox; }
        }

        protected override void CheckFolderStructure()
        {
            var data = Task.Run( () =>Client.Files.ListFolderAsync(string.Empty)).Result;

            if (!data.Entries.Any(x => x.IsFolder && x.PathLower == InternalPath.ToLower()))
            {
                var newPath = string.Format(@"/{0}", InternalPath);
                Task.Run( () =>Client.Files.CreateFolderAsync(newPath)).RunSynchronously();
                Task.Run( () =>Client.Files.CreateFolderAsync(string.Format(@"{0}/{1}", newPath, PhotoPath))).RunSynchronously();
                Task.Run( () =>Client.Files.CreateFolderAsync(string.Format(@"{0}/{1}", newPath, MemberPath))).RunSynchronously();
                var readMeStream = GetReadmeStream();
                UploadFile("/", "readme.txt", readMeStream);
                readMeStream.Close();
            }

            if (!data.Entries.Any(x => x.IsFolder && x.PathLower == ToPublishPath.ToLower()))
            {
                Task.Run( () =>Client.Files.CreateFolderAsync(string.Format(@"/{0}", ToPublishPath)).RunSynchronously());
            }
        }

        protected override void CheckForGlitchFolder()
        {
            var data = Task.Run(() => Client.Files.ListFolderAsync(string.Format(@"/{0}/", InternalPath))).Result;
            if (data.Entries.Any(x => x.IsFolder && x.PathLower == GlitchPath.ToLower())) return;
            var newPath = string.Format(@"/{0}/{1}", InternalPath, GlitchPath);
            Task.Run( () =>Client.Files.CreateFolderAsync(newPath)).RunSynchronously();
        }

        protected override string GetFileName(ListFolderResult source)
        {
            return source.Entries[0].PathLower;
        }


        /// <summary>
        /// Creates a request for the google api to get location information
        /// </summary>
        /// <param name="longitude">The double value representation of the longitude for location</param>
        /// <param name="latitude">The double value representation of the latitude for location</param>
        /// <returns>The request object that needs to be extended by the base url (http://maps.googleapis.com)</returns>
        public static RestRequest CreateGeoLocationRequest(double longitude, double latitude)
        {
            //http://maps.googleapis.com/maps/api/geocode/json?latlng=17.49266,78.54620&sensor=true",
            var englishCulture = new CultureInfo("en");
            var request = new RestRequest(Method.GET)
            {
                Resource = "/maps/api/geocode/json?latlng={latitude},{longitude}&sensor=true"
            };

            request.AddParameter("latitude", latitude.ToString(englishCulture), ParameterType.UrlSegment);
            request.AddParameter("longitude", longitude.ToString(englishCulture), ParameterType.UrlSegment);

            return request;
        }
    }
}