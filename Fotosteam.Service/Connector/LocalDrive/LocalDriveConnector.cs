using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Connector.LocalDrive
{
    /// <summary>
    /// Kapselt den Zugriff auf die locale Festplatte.
    /// DIeser Connector eignet sich gut zu Testzwecken
    /// </summary>
    public class LocalDrive : ConnectorBase<List<FileInfo>>
    {

        private string _rootUrl;
        private string _alias;

        internal LocalDrive(IDataRepository repository, string rootUrl, string alias)
        {
            _rootUrl = string.Format("{0}/Members/{1}", rootUrl, alias);
            _alias = alias;
            Repository = repository;
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            CheckFolderStructure();
        }

        private string _appRootFolder;
        private string AppRootFolder
        {
            get
            {
                if (!string.IsNullOrEmpty(_appRootFolder))
                    return _appRootFolder;

                if (HttpContext.Current != null)
                {
                    var path = HttpContext.Current.Server.MapPath("/");
                    _appRootFolder = string.Format("{0}Members\\{1}", path, _alias);
                }
                else
                {
                    _appRootFolder = string.Format("Members\\{0}", _alias);
                }
                return _appRootFolder;
            }
        }

        internal string GetAuthorizeUrl(string callerUrl, string redirectUrl, int memberId)
        {
            return string.Empty;
        }

        [Obsolete("This methode ist not supported by local drive")]
        internal bool Authorize(string code, int memberId, string uri)
        {
            return true;
        }

        internal bool Authorize(HttpContext context, int memberId)
        {
            return true;
        }


        protected override void CheckFolderStructure()
        {

            if (!Directory.Exists(AppRootFolder))
            {
                Directory.CreateDirectory(AppRootFolder);
                var internalPath = string.Format("{0}\\{1}", AppRootFolder, InternalPath);
                _memberPathOrId = string.Format("{0}\\{1}", internalPath, MemberPath);
                Directory.CreateDirectory(_memberPathOrId);
                _photosFolderId = string.Format("{0}\\{1}", internalPath, PhotoPath);
                Directory.CreateDirectory(_photosFolderId);
                _toPublishFolderId = string.Format("{0}\\{1}", AppRootFolder, ToPublishPath);
                Directory.CreateDirectory(_toPublishFolderId);

                var readMeStream = GetReadmeStream();
                UploadFile(AppRootFolder, "readme.txt", readMeStream);
                readMeStream.Close();
            }
            else
            {
                var internalPath = string.Format("{0}\\{1}", AppRootFolder, InternalPath);
                _memberPathOrId = string.Format("{0}\\{1}", internalPath, MemberPath);
                _photosFolderId = string.Format("{0}\\{1}", internalPath, PhotoPath);
                _toPublishFolderId = string.Format("{0}\\{1}", AppRootFolder, ToPublishPath);
            }
        }

        protected override void CheckForGlitchFolder()
        {
            var fullPath = string.Format("{0}\\{1}\\{2}", AppRootFolder, InternalPath, GlitchPath);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        private string _toPublishFolderId;
        private string _photosFolderId;

        public override void DeleteNotExistingPhotos()
        {
        }

        internal override List<FileInfo> GetFileList(string folder = "")
        {
            if (string.IsNullOrEmpty(folder))
            {
                folder = _toPublishFolderId;
            }
            var files = Directory.GetFiles(folder);

            TotalFileCount = files.Length;
            return files.Select(file => new FileInfo(file)).ToList();
        }

        protected override void BackupExistingFiles(string folderId)
        {
            var files = GetFileList(folderId);
            foreach (var file in files)
            {
                MoveFile(new List<FileInfo>() { file }, folderId, "_" + file.Name);
            }
        }
        protected override void DeleteBackupedFiles(string folderId)
        {
            var files = GetFileList(folderId);
            foreach (var file in files)
            {
                if (file.Name.StartsWith("_"))
                    File.Delete(file.FullName);
            }
        }

        protected override void RollbackBackupedFiles(string folderName)
        {
            var folderId = string.Format(@"{0}\{1}", PhotoPath, folderName);
            var files = GetFileList(folderId);
            foreach (var file in files)
            {
                if (!file.Name.StartsWith("_"))
                    File.Delete(file.FullName);
            }

            files = GetFileList(folderId);
            foreach (var file in files)
            {
                MoveFile(new List<FileInfo>() { file }, folderId, file.Name.Substring(1));
            }
        }


        protected override void DeleteFile(List<FileInfo> fileInfo)
        {
            File.Delete(fileInfo[0].FullName);
        }

        protected override void DeletePhotoFolder(string folderName)
        {
            folderName = string.Format(@"{0}\{1}", PhotoPath, folderName);
            Directory.Delete(folderName, true);
        }


        protected override IEnumerable<SynchProgress> UploadPhotos(List<FileInfo> fileList)
        {
            var firstProgess = new SynchProgress() { Photo = null, Index = Index, TotalFileCount = TotalFileCount };
            if (fileList == null)
            {
                yield return firstProgess;
                yield break;
            }

            TotalFileCount = fileList.Count;
            yield return firstProgess;

            Index = 1;
            foreach (var file in fileList)
            {
                if (file.Name.ToLower().EndsWith("jpg"))
                {
                    MemoryStream ms = new MemoryStream();
                    using (FileStream fs = File.OpenRead(file.FullName))
                    {
                        fs.CopyTo(ms);
                    }
                    var photo = UploadPhoto(ms, new List<FileInfo>() { file });
                    yield return new SynchProgress() { Photo = photo, Index = Index, TotalFileCount = TotalFileCount };
                    Index++;
                }
            }
        }


        protected override string UploadFile(string folderId, string fileName, Stream stream, bool overrideFile = false)
        {
            var destination = string.Format("{0}\\{1}", folderId, fileName);

            stream.Position = 0;
            var fs = new FileStream(destination, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fs);
            fs.Close();

            return destination;
        }

        protected override string UploadFileToGlitch(string fileName, Stream stream, bool overrideFile = false)
        {
            var destination = string.Format("{0}\\{1}\\{2}", InternalPath,PhotoPath,GlitchPath );
            return UploadFile(destination, fileName , stream, true);
        }

        protected override string MoveFile(List<FileInfo> fileItem, string folderId, string newName)
        {
            var sourceFile = fileItem[0];
            var destination = string.Format("{0}\\{1}", folderId, newName);
            File.Move(sourceFile.FullName, destination);
            return destination;
        }

        protected override string GetFileName(List<FileInfo> source)
        {
            return source[0].Name;
        }

        protected override string GetContentLink(string fileId = null)
        {
            if (fileId == null)
                return "Unknown";

            var pathParts = fileId.Split('\\');
            var indexCount = pathParts.Length - 1;
            var foloderWIthFile = string.Format("{0}/{1}", pathParts[indexCount - 1], pathParts[indexCount]);

            if (fileId.Contains("\\Internal\\Member\\"))
            {
                return string.Format("{0}/{1}/{2}", _rootUrl, InternalPath, foloderWIthFile);
            }

            return string.Format("{0}/{1}/{2}/{3}", _rootUrl, InternalPath, PhotoPath, foloderWIthFile);
        }

        public override StorageProviderType ProviderType
        {
            get { return StorageProviderType.OneDrive; }
        }

        private string _memberPathOrId;
        public override string MemberPathOrId { get { return _memberPathOrId; } }


        public override void Connect(MemberStorageAccess access)
        { }


        public override string CreateNewPhotoFolder(ref string folderToCreate)
        {
            var newFolder = string.Format("{0}\\{1}", _photosFolderId, folderToCreate);

            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }
            else
            {
                var index = 0;
                var newFolderWithCounter = newFolder;
                while (Directory.Exists(newFolderWithCounter))
                {
                    index++;
                    newFolderWithCounter = string.Format("{0}_{1}", newFolder, index);
                }
                newFolder = newFolderWithCounter;
                Directory.CreateDirectory(newFolder);
            }

            return newFolder;
        }
    }
}