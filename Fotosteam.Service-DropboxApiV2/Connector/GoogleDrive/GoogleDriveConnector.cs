using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Microsoft.Win32;
using File = Google.Apis.Drive.v2.Data.File;

namespace Fotosteam.Service.Connector.GoogleDrive
{
    /// <summary>
    /// Kapselt den Zugriff auf ein Google-Driove.
    /// Im Gegensatz zu Dropbox und One-Drive gibt es nicht das Konzept der Anwendungen.
    /// Wir haben den Zugriff auf das komplette Laufwerk. Das wird sich wahrscheinlich bald ändern und diees Klasse muss angepasst werden
    /// </summary>
    internal class GoogleDrive : ConnectorBase<FileList>
    {
        private DriveService _service;

        internal GoogleDrive(IDataRepository repository)
        {
            Repository = repository;
        }

        private DriveService Service
        {
            get
            {
                if (_service != null)
                    return _service;

                var store = new CustomGoogleDataStore(Repository);
                var secret = new ClientSecrets
                {
                    ClientId = Startup.GoogleClientId,
                    ClientSecret = Startup.GoogleClientSecret
                };

                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(secret,
                    new[] { DriveService.Scope.Drive }, CurrentMemberId.ToString(), CancellationToken.None, store).Result;
                _service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Fotosteam"
                });
                GetFolderIds();
                return _service;
            }
        }

        /// <summary>
        ///     Gibt den Pfad oder die OrdnerId für den MemberOrdner zurück
        /// </summary>
        public override string MemberPathOrId
        {
            get { return _memberFolderId; }
        }

        /// <summary>
        ///     Erzeugt einen neuen Ordner und gibt den Pfad oder Id zurück
        /// </summary>
        public override string CreateNewPhotoFolder(ref string folder)
        {
            var startFolder = folder;
            var index = 0;
            var id = GetFolderId(_fotosFolderId, folder);
            while (id != string.Empty)
            {
                index++;
                folder = string.Format("{0}_{1}", startFolder, index);
                id = GetFolderId(_fotosFolderId, folder);
            }

            return CreateDirectory(folder, folder, _fotosFolderId).Id;
        }

        internal string GetAuthorizeUrl(string callerUrl, string redirectUrl, int memberId)
        {
            var flow = GetFlow();
            var result =
                new AuthorizationCodeWebApp(flow, redirectUrl, memberId.ToString()).AuthorizeAsync(memberId.ToString(),
                    CancellationToken.None).Result;

            return result.RedirectUri;
        }

        internal bool Authorize(string code, int memberId, string uri)
        {
            try
            {
                var flow = GetFlow();
                var memberString = memberId.ToString();
                var urlWithoutQueryString = uri.Substring(0, uri.IndexOf("?", StringComparison.InvariantCultureIgnoreCase
                    ));
                var token =
                    flow.ExchangeCodeForTokenAsync(memberString, code, urlWithoutQueryString, CancellationToken.None)
                        .Result;

                if (token == null)
                    return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }

            return true;
        }

        protected override void CheckFolderStructure()
        {
            var request = Service.Files.List();
            request.Q = "mimeType='application/vnd.google-apps.folder' and trashed=false and title contains 'fotosteam'";
            var result = new List<File>();
            do
            {
                try
                {
                    var files = request.Execute();
                    result.AddRange(files.Items);
                    request.PageToken = files.NextPageToken;
                }
                catch (Exception e)
                {
                    request.PageToken = null;
                    Log.Error(e);
                }
            } while (!String.IsNullOrEmpty(request.PageToken));


            if (result.Count == 0)
            {
                var fotosteam = CreateDirectory(AppPath, "Main folder for the Fotosteam application", "root");
                CreateDirectory(ToPublishPath, "Folder to place your photos for upload to fotosteam", fotosteam.Id);

                var internalDir = CreateDirectory(InternalPath, "Folder required by Fotosteam, DO NOT CHANGE", fotosteam.Id);
                CreateDirectory(PhotoPath, "Folder required by fotosteam, DO NOT CHANGE", internalDir.Id);
                CreateDirectory(MemberPath, "Folder required by fotosteam, DO NOT CHANGE", internalDir.Id);

                var readMeStream = GetReadmeStream();
                UploadFile(fotosteam.Id, "readme.txt", readMeStream);
                readMeStream.Close();
            }
        }

        private string _glitchPathId = string.Empty; 
        protected override void CheckForGlitchFolder()
        {
            if (!string.IsNullOrEmpty(_glitchPathId))
                return;
            var internalFolderId = GetFolderId(_fotosteamFolderId, InternalPath);
            _glitchPathId = GetFolderId(internalFolderId, GlitchPath);
           
            if(string.IsNullOrEmpty(  _glitchPathId))
                _glitchPathId = CreateDirectory(GlitchPath, "Folder required by fotosteam, DO NOT CHANGE", internalFolderId).Id;
        }

        /// <summary>
        ///     Create a new Directory.
        ///     Documentation:<![CDATA[ https://developers.google.com/drive/v2/reference/files/insert ]]>
        /// </summary>
        /// <param name="title">The title of the file. Used to identify file or folder name.</param>
        /// <param name="description">A short description of the file.</param>
        /// <param name="parent">
        ///     Collection of parent folders which contain this file.
        ///     Setting this field will put the file in all of the provided folders. root folder.
        /// </param>
        /// <returns></returns>
        private File CreateDirectory(string title, string description, string parent)
        {
            File newDirectory = null;

            // Create metaData for a new Directory
            var body = new File
            {
                Title = title,
                Description = description,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<ParentReference> { new ParentReference { Id = parent } }
            };
            try
            {
                var request = Service.Files.Insert(body);
                newDirectory = request.Execute();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return newDirectory;
        }

        private ForceOfflineGoogleAuthorizationCodeFlow GetFlow()
        {
            var scopes = new[]
            {
                "https://www.googleapis.com/auth/drive",
                "https://www.googleapis.com/auth/userinfo.email",
                "https://www.googleapis.com/auth/userinfo.profile",
                "https://www.googleapis.com/auth/drive.install"
            };
            var flow = new ForceOfflineGoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                //use custom own datastore
                DataStore = new CustomGoogleDataStore(Repository),
                ClientSecrets =
                    new ClientSecrets { ClientId = Startup.GoogleClientId, ClientSecret = Startup.GoogleClientSecret },
                Scopes = scopes
            });
            return flow;
        }

        #region Connector-Implementierungen

        private string _fotosteamFolderId;
        private string _toPublishFolderId;
        private string _fotosFolderId;
        private string _memberFolderId;

        /// <summary>
        /// Löscht Fotos aus der Datenbank, die nicht mehr auf Google-Drive im Foto-Ordner vorhanden sind
        /// </summary>
        public override void DeleteNotExistingPhotos()
        {
            var currentPhotoFolders = GetPhotoFolders();
            if (currentPhotoFolders.Count == 0)
                return;

            var request = _service.Children.List(_fotosFolderId);
            request.Q = "mimeType='application/vnd.google-apps.folder' and trashed=false";
            
            var list = request.Execute();

            if (list != null && list.Items != null)
            {
                var existingFolders = list.Items.Select(i => i.Id.ToLower()).ToList();

                var orphans = currentPhotoFolders.Except(existingFolders);
                foreach (var item in orphans)
                {
                    Repository.DeletePhotoByFolder(item);
                }
            }
        }


        internal override FileList GetFileList(string folder = "")
        {
            var request = Service.Files.List();

            if (string.IsNullOrEmpty(folder))
                folder = _toPublishFolderId;

            request.Q = "'" + folder + "' in parents and mimeType = 'image/jpeg'";
            var list = request.Execute();
            if (list != null)
            {
                TotalFileCount = list.Items.Count;
            }
            return list;
        }

        protected override void BackupExistingFiles(string folderId)
        {
            var files = GetFileList(folderId);
            foreach (var file in files.Items)
            {
                file.OriginalFilename = "_" + file.OriginalFilename;
                file.Title = file.OriginalFilename;
                Service.Files.Patch(file, file.Id).Execute();                
            }
        }

        protected override void DeleteBackupedFiles(string folderId)
        {
            var files = GetFileList(folderId);
            foreach (var file in files.Items)
            {
                if (file.Title.StartsWith("_"))
                {
                    DeleteFile(file.Id);
                }
            }
        }

        protected override void RollbackBackupedFiles(string folderName)
        {
            var folderId = GetFolderId(_fotosFolderId, folderName);
            var files = GetFileList(folderId);
            foreach (var file in files.Items)
            {
                if (!file.Title.StartsWith("_"))
                {
                    DeleteFile(file.Id);
                }
            }
            files = GetFileList(folderId);
            foreach (var file in files.Items)
            {
                file.Title = file.Title.Substring(1);
                file.OriginalFilename = file.Title;                
                Service.Files.Patch(file, file.Id).Execute();  
            }
        }

        private bool _folderIdsLoader;
        private void GetFolderIds()
        {
            if (_folderIdsLoader) return;

            //Sicherstellen, dass die Ordner angelegt werden
            CheckFolderStructure();

            _fotosteamFolderId = GetFolderId("root", AppPath);
            _toPublishFolderId = GetFolderId(_fotosteamFolderId, ToPublishPath);
            var internalFolderId = GetFolderId(_fotosteamFolderId, InternalPath);
            _fotosFolderId = GetFolderId(internalFolderId, PhotoPath);
            _memberFolderId = GetFolderId(internalFolderId, MemberPath);
            _folderIdsLoader = true;

            var readMeStream = GetReadmeStream();
            UploadFile(_fotosteamFolderId, "readme.txt", readMeStream);
            readMeStream.Close();

        }


        protected override IEnumerable<SynchProgress> UploadPhotos(FileList fileList)
        {
            if (fileList != null)
            {
                TotalFileCount = fileList.Items.Count;
                Index = 1;
                foreach (var file in fileList.Items)
                {
                    var imageBytes = Service.HttpClient.GetByteArrayAsync(file.DownloadUrl).Result;
                    var sourceStream = new MemoryStream(imageBytes);
                    var list = new FileList { Items = new List<File> { file } };
                    var photo = UploadPhoto(sourceStream, list);
                    yield return new SynchProgress() { Photo = photo, Index = Index, TotalFileCount = TotalFileCount };
                    Index++;
                }
            }
            else
            {
                yield return new SynchProgress() { Photo = null, Index = Index, TotalFileCount = TotalFileCount };
            }
        }

        protected override string GetContentLink(string fileId = null)
        {
            return ShareFile(fileId);
        }

        /// <summary>
        /// Gibt den Type des Providers zurück
        /// </summary>
        public override StorageProviderType ProviderType
        {
            get { return StorageProviderType.GoogleDrive; }
        }



        /// <summary>
        ///     Uploads a file
        ///     Documentation: https://developers.google.com/drive/v2/reference/files/insert
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fileName">path to the file to upload</param>
        /// <param name="folderId">
        ///     Collection of parent folders which contain this file.
        ///     Setting this field will put the file in all of the provided folders. root folder.
        /// </param>
        /// <returns>
        ///     If upload succeeded returns the File resource of the uploaded file
        ///     If the upload fails returns null
        /// </returns>
        private string UploadFile(Stream fileStream, string fileName, string folderId)
        {
            var currentFile = new File
            {
                Title = Path.GetFileName(fileName),
                MimeType = GetMimeType(fileName),
                Parents = new List<ParentReference> { new ParentReference { Id = folderId } }
            };

            try
            {
                var request = Service.Files.Insert(currentFile, fileStream, currentFile.MimeType);
                request.Upload();
                currentFile = request.ResponseBody;
                return currentFile.Id;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            return fileName;
        }

        private string UpdateFile(Stream fileStream, string fileName, string folderId)
        {
            var request = Service.Files.List();
            request.Q = "'" + folderId + "' in parents and mimeType = 'image/jpeg' and title='" + fileName + "'";
            var files = request.Execute().Items;
            if (files != null && files.Count > 0)
            {
                DeleteFile(files[0].Id);
            }
            fileName = UploadFile(fileStream, fileName, folderId);
            return fileName;
        }

        protected override string UploadFile(string path, string fileName, Stream stream, bool overrideFile = false)
        {
            return UpdateFile(stream, fileName, path);
        }

        protected override string UploadFileToGlitch(string fileName, Stream stream, bool overrideFile = false)
        {
            return UpdateFile(stream, fileName , _glitchPathId);
        }

        protected override void DeletePhotoFolder(string folderName)
        {
            var folderId = GetFolderId(_fotosFolderId, folderName);
            var deleteRequest = Service.Children.Delete(_fotosFolderId, folderId);
            deleteRequest.Execute();
        }

        protected override void DeleteFile(FileList fileInfo)
        {
            DeleteFile(fileInfo.Items[0].Id);
        }

        private void DeleteFile(string fileId)
        {
            var deleteRequest = Service.Files.Delete(fileId);             
            deleteRequest.Execute();
        }
        
        private string ShareFile(string fileId)
        {
            var newPermission = new Permission { Type = "anyone", Role = "reader" };
            try
            {
                Service.Permissions.Insert(newPermission, fileId).Execute();
                return Service.Files.Get(fileId).Execute().WebContentLink;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            return string.Empty;
        }

        private static string GetMimeType(string fileName)
        {
            var mimeType = "application/unknown";
            var ext = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext)) ext = "jpg";
            ext = ext.ToLower();
            var regKey = Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        private string GetFolderId(string root, string folder)
        {
            var request = _service.Children.List(root);
            request.Q = "mimeType='application/vnd.google-apps.folder' and trashed=false and title contains '" + folder +
                        "'";
            var result = new List<ChildReference>();
            do
            {
                try
                {
                    var children = request.Execute();
                    result.AddRange(children.Items);
                    request.PageToken = children.NextPageToken;
                }
                catch (Exception e)
                {
                    request.PageToken = null;
                    Log.Error(e);
                    return string.Empty;
                }
            } while (!String.IsNullOrEmpty(request.PageToken));

            if (result.Count == 0)
                return string.Empty;

            return result[0].Id;
        }

        /// <summary>
        ///     Diese Methode wird nur zur Kompatibilität des Interface implementiert.
        ///     Für Google-Drive wird die Authorizierung über die <see cref="CustomGoogleDataStore" /> sichergestellt
        /// </summary>
        /// <param name="access">Wird nicht benötigt</param>
        public override void Connect(MemberStorageAccess access)
        {
            if (Service == null)
                throw new InvalidOperationException("No connection could be established");
        }

        protected override string MoveFile(FileList source, string destination, string newFileName)
        {
            var file = source.Items[0];
            var targetFolder = Service.Files.Get(destination).Execute();
            var newParent = new ParentReference();
            newParent.SelfLink = targetFolder.SelfLink;
            newParent.ParentLink = targetFolder.Parents[0].SelfLink;
            newParent.Id = destination;
            newParent.Kind = targetFolder.Kind;
            newParent.IsRoot = false;
            file.Parents = new List<ParentReference>() { newParent };
            file.OriginalFilename = newFileName;
            file.Title = newFileName;
            
            return Service.Files.Update(file, file.Id).Execute().Id;
        }

        #endregion

        protected override string GetFileName(FileList source)
        {
            return source.Items[0].OriginalFilename;
        }
    }
}