using System.Linq;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Repository.Poco;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fotosteam.Tests.Service
{
    [TestClass]
    public class DeleteRequestTests
    {
        #region Setup

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            RequestHelper.CreateServer();
        }

        [ClassCleanup]
        public static void TearDown()
        {
            RequestHelper.Server.Dispose();
        }
        #endregion


        #region account methods

  //      [TestMethod]
        public void Deleting_a_member_will_remove_it_from_the_collection()
        {
            Assert.Fail("Not implemented");
        }

        
//[TestMetod]
        public void Deleting_a_storage_access_will_remove_it_from_the_collection()
        {
            Assert.Fail("Not implemented");
        }

       // [TestMethod]
        public void Deleting_a_login_access_will_remove_it_from_the_collection()
        {
            Assert.Fail("Not implemented");
        }

        #endregion

        #region data methods

        private void ExecuteDeleteRequest<T>(string resource, string resources, string id) where T : class
        {
            RequestHelper.SetupClaim(true);

            var resourceUrl = "/api/data/" + resource + "/" + id;

            var count = RequestHelper.CurrentDataRepository.Queryable<T>().Count();
            var result = RequestHelper.DeleteResult<Result<bool>>(resourceUrl);
            var newCount = RequestHelper.CurrentDataRepository.Queryable<T>().Count();

            Assert.IsTrue(result.Data, resource + " konnte nicht gelöscht werden");
            Assert.IsTrue(newCount < count, "Die Anzahl der " + resources + "hat sich nicht geändert");
        }

        [TestMethod]
        public void Deleting_a_photo_will_remove_it_from_the_collection()
        {
            ExecuteDeleteRequest<Photo>("photo", "photos", "53");
        }

        [TestMethod]
        public void Deleting_a_topic_will_remove_it_from_the_collection()
        {
            var member = RequestHelper.CurrentDataRepository.Queryable<Member>().FirstOrDefault(m => m.Id == 3);
            var topic = RequestHelper.CurrentDataRepository.Queryable<Topic>().FirstOrDefault(t => t.Id == 1);
            topic.Member = member;
            topic.MemberId = member.Id;
            ExecuteDeleteRequest<Topic>("topic", "topics", "1");
        }
        [TestMethod]
        public void Deleting_a_event_access_will_remove_it_from_the_collection()
        {
            ExecuteDeleteRequest<Event>("event", "events", "2");
        }
        
        [TestMethod]
        public void Deleting_a_story_access_will_remove_it_from_the_collection()
        {
            RequestHelper.ResetRepository();
            ExecuteDeleteRequest<Story>("story", "stories", "4");
        }
        [TestMethod]
        public void Deleting_a_chapter_access_will_remove_it_from_the_collection()
        {
            RequestHelper.ResetRepository();
            RequestHelper.SetupClaim(true);
       
            const string resourcesUrl = "/api/data/robert/stories";
            const string resourceUrl = "/api/data/chapter/3";

            var count = RequestHelper.CurrentDataRepository.Queryable<Chapter>().Count();

            var result = RequestHelper.DeleteResult<Result<bool>>(resourceUrl);
            var newCount = RequestHelper.CurrentDataRepository.Queryable<Chapter>().Count();

            Assert.IsTrue(result.Data, "Ledge konnte nicht gelöscht werden");
            Assert.IsTrue(newCount < count, "Die Anzahl der Ledges hat sich nicht geändert");
        }
        [TestMethod]
        public void Deleting_a_ledge_will_remove_it_from_the_collection()
        {
            RequestHelper.ResetRepository();
            RequestHelper.SetupClaim(true);
            
            const string resourceUrl = "/api/data/ledge/3";

            var count = RequestHelper.CurrentDataRepository.Queryable<Ledge>().Count();
            var result = RequestHelper.DeleteResult<Result<bool>>(resourceUrl);
            var newCount = RequestHelper.CurrentDataRepository.Queryable<Ledge>().Count();

            Assert.IsTrue(result.Data, "Ledge konnte nicht gelöscht werden");
            Assert.IsTrue(newCount < count, "Die Anzahl der Ledges hat sich nicht geändert");
        }

        private void ExecuteDeleteRequestForBrick<T>(string resource, string id)  where T:class
        {
            RequestHelper.ResetRepository();
            RequestHelper.SetupClaim(true);

            var resourceUrl = "/api/data/" + resource + "/" + id;

            var count = RequestHelper.CurrentDataRepository.Queryable<T>().Count();
            var result = RequestHelper.DeleteResult<Result<bool>>(resourceUrl);
            var newCount = RequestHelper.CurrentDataRepository.Queryable<T>().Count();

            Assert.IsTrue(result.Data, resource + " konnte nicht gelöscht werden");
            Assert.IsTrue(newCount < count, "Die Anzahl der " + resource + " hat sich nicht geändert");
        }

        [TestMethod]
        public void Deleting_a_textbrick_access_will_remove_it_from_the_collection()
        {
            RequestHelper.ResetRepository();
            ExecuteDeleteRequestForBrick<TextBrick>("textbrick",  "4");
        }

        [TestMethod]
        public void Deleting_a_photobrick_access_will_remove_it_from_the_collection()
        {
            RequestHelper.ResetRepository();
            ExecuteDeleteRequestForBrick<PhotoBrick>("photobrick",  "3");
        }
        [TestMethod]
        public void Deleting_a_mapbrick_access_will_remove_it_from_the_collection()
        {
            RequestHelper.ResetRepository();
            ExecuteDeleteRequestForBrick<MapBrick>("mapbrick", "2");
        }

        [TestMethod]
        public void Deleting_a_buddy_will_remove_it_from_the_collection()
        {
            ExecuteDeleteRequest<Buddy>("buddy", "buddy", "2");
        }

        #endregion
    }
}
