using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Microsoft.Live;
using Newtonsoft.Json;
using OneDrive;
using RestSharp;

namespace Fotosteam.Service.Connector.OneDrive
{
    /// <summary>
    ///     Kapselt den Zugriff auf OneDrive von Microsoft für die Verwaltung der Bilder
    /// </summary>
    public class OneDrive : ConnectorBase<List<ODItem>>
    {
        private const string AppRootFolder = "/drive/special/approot";
        private LiveConnectClient _client;

        private LiveAuthClient _liveAuthClient = new LiveAuthClient(Startup.MicrosoftClientId,
            Startup.MicrosoftClientSecret, null);

        private LiveConnectSession _session;

        internal OneDrive(IDataRepository repository)
        {
            Repository = repository;
        }

        private LiveConnectClient Client
        {
            get
            {
                if (_client != null)
                    return _client;

                _client = new LiveConnectClient(_session) { UseOneDrive = true };
                return _client;
            }
        }


        internal string GetAuthorizeUrl(string callerUrl, string redirectUrl, int memberId)
        {
            var scopes = new[] { "wl.signin", "wl.offline_access", "onedrive.appfolder" };
            return _liveAuthClient.GetLoginUrl(scopes, redirectUrl);
        }

        [Obsolete("This methode ist not supported by one drive")]
        internal bool Authorize(string code, int memberId, string uri)
        {
            return Authorize(HttpContext.Current, memberId);
        }

        internal bool Authorize(HttpContext context, int memberId)
        {
            try
            {
                var abstractContext = new HttpContextWrapper(context);
                var result = _liveAuthClient.ExchangeAuthCodeAsync(abstractContext).Result;

                _session = result.Session;
                var access =
                    Repository.Queryable<MemberStorageAccess>()
                        .FirstOrDefault(
                            s => s.MemberId == memberId && s.Type == StorageProviderType.OneDrive);
                if (access == null)
                {
                    access = new MemberStorageAccess
                    {
                        MemberId = memberId,
                        Token = _session.RefreshToken,
                        Secret = "onedrive",
                        Type = StorageProviderType.OneDrive
                    };
                    Repository.Add(access);
                }
                else
                {
                    access.Token = _session.RefreshToken;
                    Repository.Update(access);
                }

                CheckFolderStructure();
                return true;
            }
            catch (Exception ex)
            {                
                Log.Error(ex);
            }

            return false;
        }

        protected override void CheckFolderStructure()
        {
            CheckForSession();
            var folderInfo = GetFolderInformation(AppRootFolder);
            if (!folderInfo.Collection.Any())
            {
                var item = CreateFolder(AppRootFolder, InternalPath);
                CreateFolder("drive/items/" + item.Id, MemberPath);
                CreateFolder("drive/items/" + item.Id, PhotoPath);
                CreateFolder(AppRootFolder, ToPublishPath);

                var readMeStream = GetReadmeStream();
                UploadFile(item.ParentReference.Id , "readme.txt", readMeStream);
                readMeStream.Close();
            }
        }

        private string _glitchPathId = String.Empty;
        protected override void CheckForGlitchFolder()
        {
            if (!string.IsNullOrEmpty(_glitchPathId))
                return;

            var folderItems = GetFolderInformation(AppRootFolder);

            if (!folderItems.Collection.Any())
                return;

            var internPathId = folderItems.Collection.First(f => f.Folder != null && f.Name.ToLower().Equals( InternalPath.ToLower())).Id;
            var folders = GetFolderInformation("/drive/items/" + internPathId);
            var folder = folders.Collection.FirstOrDefault(f => f.Folder != null && f.Name.ToLower().Equals(GlitchPath.ToLower()));

            _glitchPathId = folder == null ? CreateFolder("drive/items/" + internPathId, GlitchPath).Id : folder.Id;
        }

        private void CheckForSession()
        {
            if (_session != null)
                return;
            var access = Repository.Queryable<MemberStorageAccess>()
                .FirstOrDefault(
                    s => s.MemberId == CurrentMemberId && (int)s.Type == (int)StorageProviderType.OneDrive);
            if (access == null)
                throw new InvalidCredentialException("Accesstoken could not be found");

            var restClient = new RestClient("https://login.live.com/oauth20_token.srf");
            var request = new RestRequest();
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            var body =
                string.Format(
                    "client_id={0}&redirect_uri={1}&client_secret={2}&refresh_token={3}&grant_type=refresh_token"
                    , Startup.MicrosoftClientId, RedirectUrl, Startup.MicrosoftClientSecret, access.Token);
            request.AddParameter("application/x-www-form-urlencoded", body, ParameterType.RequestBody);

            var result = restClient.Execute(request);
            dynamic token = JsonConvert.DeserializeObject(result.Content);
            access.Token = token.refresh_token;
            string userId = token.user_id;
            Repository.Update(access);

            //TODO: Hier muss noch überprüft werden, wie mit eventuellen Timeouts umgegangen wird
            //falls der Benutzer sich tage/wochenlange nicht anmeldet und Synchronisiert
            _liveAuthClient = new LiveAuthClient(Startup.MicrosoftClientId, Startup.MicrosoftClientSecret, RedirectUrl,
                new TokenHandler(access.Token, userId));

            var authorizeResult = _liveAuthClient.InitializeSessionAsync(RedirectUrl).Result;
            if (authorizeResult.Status != LiveConnectSessionStatus.Connected)
                throw new InvalidCredentialException("One Drive cannot be authorized");

            _session = _liveAuthClient.Session;

            GetFolderIds();
        }

        /// <summary>
        /// Diese Klasse wird für das Reauthentifizieren ohne Einwirkung des Benutzer benötigt
        /// </summary>
        private class TokenHandler : IRefreshTokenHandler
        {
            private readonly string _refreshToken;
            private readonly string _userId;

            /// <summary>
            /// Initialisiert die Klasse
            /// </summary>
            /// <param name="refreshToken">Der Refreshtoken</param>
            /// <param name="userId">Id des Bentuzers bei Microsoft</param>
            public TokenHandler(string refreshToken, string userId)
            {
                _refreshToken = refreshToken;
                _userId = userId;
            }

            /// <summary>
            /// Ermittelt einen neuen Refreshtoken. 
            /// </summary>
            public Task<RefreshTokenInfo> RetrieveRefreshTokenAsync()
            {
                return Task.FromResult(new RefreshTokenInfo(_refreshToken, _userId));
            }
            
            /// <summary>
            /// Wir speichern an dieser Stelle den Token nicht
            /// </summary>
            /// <param name="tokenInfo"></param>
            /// <returns></returns>

            public async Task SaveRefreshTokenAsync(RefreshTokenInfo tokenInfo)
            {
                await Task.Delay(0);
            }
        }

        private string _toPublishFolderId;
        private string _photosFolderId;

        private void GetFolderIds()
        {
            var folderItems = GetFolderInformation(AppRootFolder);

            if (!folderItems.Collection.Any())
                return;

            _toPublishFolderId = folderItems.Collection.First(f => f.Folder != null && f.Name.ToLower() == ToPublishPath.ToLower()).Id;
            var internPathId = folderItems.Collection.First(f => f.Folder != null && f.Name.ToLower() == InternalPath.ToLower()).Id;
            var folders = GetFolderInformation("/drive/items/" + internPathId);
            _photosFolderId = folders.Collection.First(f => f.Folder != null && f.Name.ToLower() == PhotoPath.ToLower()).Id;
            _memberPathOrId = folders.Collection.First(f => f.Folder != null && f.Name.ToLower() == MemberPath.ToLower()).Id;
        }

        private ODItemCollection GetFolderInformation(string pathWithOutChildren)
        {
            var result = Client.GetAsync(pathWithOutChildren + "/children").Result;
            return JsonConvert.DeserializeObject<ODItemCollection>(result.RawResult);            
        }

        private ODItem CreateFolder(string root, string folder)
        {
            var url = root + "/children";
            var folderData = new Dictionary<string, object>();
            folderData.Add("name", folder);
            folderData.Add("folder", new object());
            folderData.Add("@name.conflictBehavior", "rename");

            try
            {
                var operationResult = Client.PostAsync(url, folderData).Result;
                return JsonConvert.DeserializeObject<ODItem>(operationResult.RawResult);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Löscht die Fotos aus der Datenbank, die nicht mehr im Internal/Fotos-Ordner sind
        /// </summary>
        public override void DeleteNotExistingPhotos()
        {
            var currentPhotoFolders = GetPhotoFolders();
            if (currentPhotoFolders.Count == 0)
                return;

            var folder = GetFolderInformation("/drive/items/" + _photosFolderId);
            var list = folder.Collection.Where(f => f.Folder != null).Select(f => f.Id.ToLower()).ToList();
            if (!list.Any()) return;
            var orphans = currentPhotoFolders.Except(list);
            foreach (var item in orphans)
            {
                Repository.DeletePhotoByFolder(item);
            }
        }

        internal override List<ODItem> GetFileList(string folderId="")
        {
            if (String.IsNullOrEmpty(_toPublishFolderId))
                return null;
            if (string.IsNullOrEmpty(folderId))
            {
                folderId = _toPublishFolderId;
            }

            var folder = GetFolderInformation("/drive/items/" + folderId);
            var list = folder.Collection.Where(f => f.Folder == null && f.Name.ToLower().EndsWith("jpg")).ToList();
            TotalFileCount = list.Count;

            return list;
        }

        protected override void BackupExistingFiles(string folderId)
        {
            var files = GetFileList(folderId);            
            foreach (var fileData in files)
            {
                MoveFile(new List<ODItem>() { fileData }, folderId, "_" + fileData.Name);
            }
        }

        protected override void DeleteBackupedFiles(string folderId)
        {
            var files = GetFileList(folderId);
            foreach (var fileData in files)
            {
                if (fileData.Name.StartsWith("_"))
                    DeleteFile(fileData.Id);
            }
        }

        protected override void RollbackBackupedFiles(string folderName)
        {
            var folders = GetFolderInformation("/drive/items/" + _photosFolderId);
            var folderId = folders.Collection.First(f => f.Folder != null && f.Name.ToLower().Equals(folderName.ToLower())).Id;
            var files = GetFileList(folderId);
            
            foreach (var fileData in files)
            {
                if (!fileData.Name.StartsWith("_"))
                    DeleteFile(fileData.Id);
            }

            files = GetFileList(folderId);
            foreach (var fileData in files)
            {
                MoveFile(new List<ODItem>() { fileData }, folderId, fileData.Name.Substring(1));
            }
        }

        protected override IEnumerable<SynchProgress> UploadPhotos(List<ODItem> fileList)
        {
            TotalFileCount = fileList.Count;            
            
            Index = 1;
            foreach (var file in fileList)
            {
                if (file.Name.ToLower().EndsWith("jpg"))
                {
                    var fileContent = Client.DownloadAsync("drive/items/" + file.Id + "/content").Result;
                    var photo = UploadPhoto(fileContent.Stream, new List<ODItem>() { file });
                    yield return new SynchProgress() { Photo = photo, Index = Index, TotalFileCount = TotalFileCount };
                    Index++;
                }
            }
        }

        private const string RootPath = "https://api.onedrive.com/v1.0/";

        protected override string UploadFile(string folderId, string fileName, Stream stream, bool overrideFile = false)
        {
            var path = string.Format("{0}drive/items/{1}/children/{2}/content", RootPath, folderId, fileName);
            var request = WebRequest.Create(path);
            request.Headers.Add("Authorization", "Bearer " + _session.AccessToken);
            request.Method = "PUT";
            request.ContentType = "application/octet-stream";
            var streamEnd = Convert.ToInt32(stream.Length);
            var buffer = new byte[streamEnd];
            stream.Read(buffer, 0, streamEnd);

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(buffer, 0, streamEnd);
                requestStream.Flush();
            }

            var response = (HttpWebResponse)request.GetResponse();
            string result;
            var responseStream = response.GetResponseStream();
            if (responseStream == null)
                throw new NullReferenceException("Responsestream could not be read");

            using (var reader = new StreamReader(responseStream))
            {
                result = reader.ReadToEnd();
            }
            var item = JsonConvert.DeserializeObject<ODItem>(result);
            return item.Id;
        }

        protected override string UploadFileToGlitch(string fileName, Stream stream, bool overrideFile = false)
        {
            return UploadFile(_glitchPathId, fileName , stream, true);
        }

        protected override string MoveFile(List<ODItem> fileItem, string folderId, string newName)
        {
            var fileId = fileItem[0].Id;
            var restClient = new RestClient(RootPath);
            var request = new RestRequest("/drive/items/" + fileId, Method.PATCH);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("Authorization", string.Format("Bearer {0}", _session.AccessToken),
                ParameterType.HttpHeader);
            request.AddHeader("Content-Type", "application/json");

            var item = new ODItem { Name = newName, ParentReference = new ODItemReference { Id = folderId } };
            request.JsonSerializer = new CustomSerializer();
            request.AddBody(item);
            restClient.Execute(request);
            return fileId;
        }

        protected override string GetFileName(List<ODItem> source)
        {
            return source[0].Name;
        }

        protected override void DeleteFile(List<ODItem> fileInfo)
        {
            DeleteFile(fileInfo[0].Id);
        }

        protected override void DeletePhotoFolder(string folderName)
        {
            var folders = GetFolderInformation("/drive/items/" + _photosFolderId );
            var folderId = folders.Collection.First(f => f.Folder != null && f.Name.ToLower().Equals(folderName.ToLower( ))).Id;
            DeleteFile(folderId);
        }

        private void DeleteFile(string fileId)
        {
            var restClient = new RestClient(RootPath);
            var request = new RestRequest("/drive/items/" + fileId, Method.DELETE);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("Authorization", string.Format("Bearer {0}", _session.AccessToken),
                ParameterType.HttpHeader);
            request.AddHeader("Content-Type", "application/json");
            restClient.Execute(request);
        }


        protected override string GetContentLink(string fileId = null)
        {
            if (string.IsNullOrEmpty(fileId) || fileId.StartsWith("http"))
                return fileId;

            var restClient = new RestClient(RootPath);
            var request = new RestRequest("/drive/items/" + fileId + "/action.createLink", Method.POST);

            request.AddParameter("Authorization", string.Format("Bearer {0}", _session.AccessToken),
                ParameterType.HttpHeader);
            request.RequestFormat = DataFormat.Json;
            var bodyParameters = new Dictionary<string, string> { { "type", "view" } };
            request.JsonSerializer = new CustomSerializer();
            request.AddBody(bodyParameters);
            var res = restClient.Execute(request);
            dynamic createdLink = JsonConvert.DeserializeObject(res.Content);

            return createdLink.link.webUrl.Value.ToString().Replace("redir?", "download?");
        }

        /// <summary>
        /// Der Typ des Connectors
        /// </summary>
        public override StorageProviderType ProviderType
        {
            get { return StorageProviderType.OneDrive; }
        }

        private string _memberPathOrId;
        /// <summary>
        /// Pfad bzw. Id zum Internal/Member-Ordner
        /// </summary>
        public override string MemberPathOrId { get { return _memberPathOrId; } }


        /// <summary>
        /// Stellt eine Verbindung zum Provider her
        /// </summary>
        /// <param name="access">Informationen mit denen der Benutzer angemeldet werden kann</param>
        public override void Connect(MemberStorageAccess access)
        {
            CheckForSession();
        }

        /// <summary>
        /// Erzeugt ein neues Verzeichnis im Internal/Foto-Ornder
        /// </summary>
        /// <param name="folderToCreate">Name des Orderns</param>
        /// <returns>Id des neuen Verzeichnisses</returns>
        public override string CreateNewPhotoFolder(ref string folderToCreate)
        {
            const string fixedPart = "/drive/items";
            var folderToCheck = string.Format("{0}/{1}", fixedPart, _photosFolderId);
            var folderInfo = GetFolderInformation(folderToCheck);
            var folder = folderToCreate;
            if (folderInfo.Collection.Any())
            {
                var count = folderInfo.Collection.Count(x => x.Name.ToLower().Contains(folder.ToLower()) && x.Folder != null);
                if (count > 0)
                {
                    folder = string.Format("{0}_{1}", folder, count);
                    folderToCreate = folder;
                }
            }

            return CreateFolder("/drive/items/" + _photosFolderId, folder).Id;
        }
    }
}