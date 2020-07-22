using System.Data.Entity;
using System.IO;
using System.Linq;
using Fotosteam.Service.Connector.LocalDrive;
using Fotosteam.Service.Repository.Poco;
using Fotosteam.Tests.Service.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fotosteam.Tests.Service
{

    /// <summary>
    /// DIese Klasse beinhaltet manuell Testes
    /// </summary>
    [TestClass]
    public class LocalDriveTests
    {
        private const string RootFolder = "https://wwww.fotosteamdebug.com:44300";
        private const string MemberAlias = "robert";
        private const int MemderId = 3;

        [TestMethod]
        public void Calling_refreshusercontent_will_create_folder_structure()
        {
            var connector = new LocalDrive(RequestHelper.CurrentDataRepository, RootFolder, MemberAlias);
            connector.CurrentMember =
                RequestHelper.CurrentDataRepository.Queryable<Member>().FirstOrDefault(m => m.Id == 3);
            var result = connector.RefreshFolderContent().ToList();
            var folderName = string.Format("Members/{0}/Internal/Fotos", MemberAlias);
            var folderExists = Directory.Exists(folderName);
            Assert.IsTrue(folderExists, "Der Ordner wurde nicht erzeugt");
            Directory.Delete("Members", true);
        }

        [TestMethod]
        public void Calling_refreshusercontent_with_existing_foto_will_create_folder_all_pitures()
        {
            var connector = new LocalDrive(RequestHelper.CurrentDataRepository, RootFolder, MemberAlias);
            connector.CurrentMember =
                RequestHelper.CurrentDataRepository.Queryable<Member>().Include(m=>m.Options).FirstOrDefault(m => m.Id == 3);

            var result = connector.RefreshFolderContent().ToList();


            var image = Resources.full;
            var folderName = string.Format("Members/{0}/ToPublish", MemberAlias);
            image.Save(folderName + "//newFoto.jpg");

            result = connector.RefreshFolderContent().ToList();

            Assert.IsTrue(result.Count > 0, "Es wurden keine Fotos erzeugt");
            Directory.Delete("Members", true);
        }

    }
}
