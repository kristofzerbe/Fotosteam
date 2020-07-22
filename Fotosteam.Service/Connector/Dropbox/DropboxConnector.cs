using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using DropNet;
using DropNet.Models;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Connector.Dropbox
{
    /// <summary>
    ///     Diese Klasse stellt die intere Api zur Kommunikation mit der Dropbox her.
    ///     Sie ist dafür zuständig den Inbox-Ordner zu synchronisieren.
    /// </summary>
    public class DropboxConnector : ConnectorBase<MetaData>
    {
        private DropNetClient _dropnetClient;
        public DropboxConnector(IDataRepository repository)
        {
            Repository = repository;
        }

        public DropboxConnector() { }

        private DropNetClient Client
        {
            get
            {
                if (_dropnetClient == null)
                {
                    _dropnetClient = new DropNetClient(Startup.DropboxAppKey, Startup.DropboxAppSecret);
                    _dropnetClient.UseSandbox = true;
                }

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
        /// <summary>
        ///     Führt die Authorizierung des Connectors durch
        /// </summary>
        /// <param name="access">Die notwendigen Zugriffdaten</param>
        public override void Connect(MemberStorageAccess access)
        {
            _userId = access.UserId;
            Client.UserLogin = new UserLogin { Secret = access.Secret, Token = access.Token };
        }

        protected override string MoveFile(MetaData source, string destination, string newFileName)
        {
            var destinationPath = string.Format(@"{0}/{1}", destination,newFileName );
            return Client.Move(source.Path, destinationPath).Path;
        }

        /// <summary>
        ///     Erzeugt einen neuen Ordner und gibt den Pfad oder Id zurück
        /// </summary>
        public override string CreateNewPhotoFolder(ref string folder)
        {
            var path = string.Format("/{0}/{1}", InternalPath, PhotoPath);
            var data = Client.GetMetaData(path);
            var toCheck = folder;
            if (data.Contents.Any(x => x.Is_Dir && x.Name.ToLower() == toCheck.ToLower()))
            {
                var count = data.Contents.Count(x => x.Is_Dir && x.Name.ToLower().StartsWith(toCheck.ToLower()));
                folder = string.Format("{0}_{1}", folder, count);
            }
            path = string.Format("/{0}/{1}/{2}", InternalPath, PhotoPath, folder);
            Client.CreateFolder(path);
            return path;
        }

        /// <summary>
        ///     Liefert die URL für die Authorisierung der Anwendung zurück
        ///     Dies ist der 1. Schritt
        /// </summary>
        /// <param name="callbackUrl">Die Url, die nach der Authorisierung aufgerufen werden soll</param>
        /// <returns>Die Url für die Authorisierung</returns>
        public string GetAuthorizeUrl(string callbackUrl)
        {
            Client.GetToken();
            return Client.BuildAuthorizeUrl(callbackUrl);
        }

        /// <summary>
        ///     Wird aufgerufen nachdem der Benutzer den Zugriff authorisiert hat
        ///     Die ist der 2. Schritt
        /// </summary>
        /// <returns></returns>
        public Member GetUserAuthentication()
        {
            var userLogin = Client.GetAccessToken();
            var info = Client.AccountInfo();
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

            access.Secret = userLogin.Secret;
            access.Token = userLogin.Token;
            access.UserId = info.uid.ToString();

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

            CheckFolderStructure();

            CurrentMember.StorageAccesses = new List<MemberStorageAccess> { access };
            return CurrentMember;
        }

        internal override MetaData GetFileList(string folder = "")
        {
            if (string.IsNullOrEmpty(folder))
                folder = ToPublishPath;

            var list = Client.GetMetaData(folder);
            if (list != null && list.Contents != null)
            {
                TotalFileCount = list.Contents.Count(x => x.Extension.ToLower() == ".jpg");
            }
            return list;
        }

        protected override void BackupExistingFiles(string folderId)
        {
            var files = GetFileList(folderId);
            foreach (var fileData in files.Contents )
            {
                MoveFile( fileData, folderId, "_" + fileData.Name);
            }
        }

        protected override void DeleteBackupedFiles(string folderId)
        {
            var files = GetFileList(folderId);
            foreach (var fileData in files.Contents)
            {
                if (fileData.Name.StartsWith("_"))
                    Client.Delete(fileData.Path);
            } 
        }

        protected override void DeleteFile(MetaData fileInfo)
        {
            Client.Delete(fileInfo.Path);
        }
        protected override void DeletePhotoFolder(string folderName)
        {
            var path = string.Format(@"/{0}/{1}/{2}", InternalPath, PhotoPath, folderName);
            Client.Delete(path);
        }

        protected override void RollbackBackupedFiles(string folderName)
        {
            var folderId = string.Format(@"/{0}/{1}/{2}",InternalPath,PhotoPath,folderName );
            var files = GetFileList(folderId);
            foreach (var fileData in files.Contents)
            {
                if(!fileData.Name.StartsWith("_"))
                    Client.Delete(fileData.Path);                
            }

            files = GetFileList(folderId);
            foreach (var fileData in files.Contents)
            {
                MoveFile(fileData, folderId, fileData.Name.Substring(1));
            }
        }

        public override void DeleteNotExistingPhotos()
        {
            var currentPhotoFolders = GetPhotoFolders();
            if (currentPhotoFolders.Count == 0)
                return;

            var list = Client.GetMetaData(string.Format("{0}/{1}", InternalPath, PhotoPath));
            if (list != null && list.Contents != null)
            {
                var existingFolders = list.Contents.Where(c => c.Is_Dir).Select(c => c.Path.ToLower()).ToList();
                var orphans = currentPhotoFolders.Except(existingFolders).ToList();                
                foreach (var item in orphans)
                {
                    Repository.DeletePhotoByFolder(item);
                }
            }
            
        }

        protected override string UploadFile(string path, string fileName, Stream stream, bool overrideFile = false)
        {
            Client.UploadFile(path, fileName, stream, overrideFile);
            return string.Format("{0}/{1}", path, fileName);
        }

        protected override string UploadFileToGlitch(string fileName, Stream stream, bool overrideFile = false)
        {
            var newPath = string.Format(@"/{0}/{1}", InternalPath, GlitchPath);
            return UploadFile(newPath, fileName, stream, overrideFile);
        }

        protected override IEnumerable<SynchProgress> UploadPhotos(MetaData folderInformation)
        {
            var files = folderInformation.Contents.Where(x => x.Extension.ToLower() == ".jpg").ToList();
            
            var currentCount = files.Count;
            TotalFileCount = currentCount;

            Index = 1;
            foreach (var fileData in files)
            {
                Log.Info(fileData.Path);
                var sourceStream = new MemoryStream(Client.GetFile(fileData.Path));
                if (sourceStream.Length == 0 || !sourceStream.CanRead)
                {
                    Log.Info("Der Stream ist leer:" + fileData.Path);
                    Thread.Sleep(2000); //Warten bis die Datei aktualisiert ist
                    continue;
                }
                var photo = UploadPhoto(sourceStream, fileData);
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
            var url = Client.GetShare(fileName, false).Url;
            var link = url.Replace("?dl=0", "").Replace("www.dropbox.com", "dl.dropboxusercontent.com");
            return link;
        }

        public override StorageProviderType ProviderType
        {
            get { return StorageProviderType.Dropbox; }
        }

        protected override void CheckFolderStructure()
        {
            var data = Client.GetMetaData("/");
            if (!data.Contents.Any(x => x.Is_Dir && x.Name.ToLower() == InternalPath.ToLower()))
            {
                var newPath = string.Format(@"/{0}", InternalPath);
                Client.CreateFolder(newPath);
                Client.CreateFolder(string.Format(@"{0}/{1}", newPath, PhotoPath));
                Client.CreateFolder(string.Format(@"{0}/{1}", newPath, MemberPath));
                var readMeStream = GetReadmeStream();
                UploadFile("/", "readme.txt", readMeStream);
                readMeStream.Close();
            }

            if (!data.Contents.Any(x => x.Is_Dir && x.Name.ToLower() == ToPublishPath.ToLower()))
            {
                Client.CreateFolder(string.Format(@"/{0}", ToPublishPath));
            }
        }

        protected override void CheckForGlitchFolder()
        {
            var data = Client.GetMetaData(string.Format( @"/{0}/",InternalPath) );
            if (!data.Contents.Any(x => x.Is_Dir && x.Name.ToLower() == GlitchPath.ToLower()))
            {
                var newPath = string.Format(@"/{0}/{1}", InternalPath,GlitchPath);
                Client.CreateFolder(newPath);
            }
        }

        protected override string GetFileName(MetaData source)
        {
            return source.Name;
        }

    }
}