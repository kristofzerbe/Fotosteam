using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Fotosteam.Tests.Service.Fake;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Fotosteam.Tests.Service
{
    [TestClass]
    public class DataControllerTests
    {
        /// <summary>
        /// Das ist kein richtiger Unittest, ist nur hier um ohne GUI eine Aktualisierung der Daten aus der Dropbox zu ermöglichen
        /// </summary>
        /// <remarks>
        /// Die Aktualisierung kann nun auch über die Demoseite aufgerufen werden
        /// </remarks>
        //[TestMethod]
        public void Uploading_Pictures_Will_Add_Photos_To_Database()
        {
            var controller = new AccountController();
            controller.User = new GenericPrincipal(new GenericIdentity("bobbakos@gmail.com"), null);
            controller.RefreshUserContent();
        }

        

        readonly DataRepository Rep = new DataRepository(new FakeFotosteamDbContext()); //RepositoryHelper.MockRepository

        private DataController GetControllerWithContext(bool isAuthenticated = false)
        {            
            var controller = new DataController(Rep, new FakeAuthRepository());
            TestAuthFilter.UseAuthenticatedUser = isAuthenticated;
            controller.User = new ClaimsPrincipal(TestAuthFilter.Identity);
            if (isAuthenticated)
            {
                var authController = new AccountController(Rep, new FakeAuthRepository());
                authController.User = controller.User;
                authController.GetUserIdForAuthenticatedUser();
            }            
            return controller;
        }

        private const string RobertAlias = "robert";
        [TestMethod]
        public void Getting_photos_for_the_journal_will_return_photos()
        {
            var controller = GetControllerWithContext();
            var result = (Result<List<Photo>>)controller.Get(RobertAlias, GetAction.ByJournal, 0, 10);

            Assert.IsTrue(result.Data.Any(), "Es wurden keine Fotos gefunden");
        }
        [TestMethod ]
        public void Test()
        {
            var text ="{\"BuddyMemberId\":\"44\"}";
            var controller = GetControllerWithContext();
            controller.AddBuddy(text);
        }

        [TestMethod]
        public void Requesting_photos_for_alias_robert_as_authenticated_user_will_return_photos()
        {
            var controller = GetControllerWithContext(true);
            var result = (Result<List<Photo>>) controller.Get(RobertAlias, GetAction.ByJournal, 0, 100);

            bool any = result.Data.Any(x => x.IsPrivate);
            Assert.IsTrue(any, "Es wurden keine Fotos gefunden");
        }

        [TestMethod]
        public void Getting_photos_by_topic_will_return_photos()
        {
            var controller = GetControllerWithContext();
            const string topic = "New Zealand";
            var result = (Result<List<Photo>>) controller.Get(RobertAlias, GetAction.ByTopic, topic);

            Assert.IsTrue(result.Data.All(p=>p.Topics.Any(t=>t.Name.ToLower() == topic.ToLower())), "Es wurden keine Fotos gefunden");
        }

        [TestMethod]
        public void Getting_photos_by_location_will_return_photos()
        {
            var controller = GetControllerWithContext();
            const string location = "New Zealand";
            var result = (Result<List<Photo>>)controller.Get(RobertAlias, GetAction.ByLocation, location);
            Assert.IsTrue(result.Data.All(p => p.Location.Name.ToLower().Contains( location.ToLower())), "Es wurden keine Fotos gefunden");
        }

        [TestMethod]
        public void Getting_photos_by_event_will_return_photos()
        {
            var controller = GetControllerWithContext();
            const string currentEvent = "DemoEvent";
            var result = (Result<List<Photo>>)controller.Get(RobertAlias, GetAction.ByEvent, currentEvent);

            Assert.IsTrue(result.Data.All(p=>p.Event.Name.ToLower() == currentEvent.ToLower()), "Es wurden keine Fotos gefunden");
        }

        [TestMethod]
        public void Getting_photos_by_category_will_return_photos()
        {
            var controller = GetControllerWithContext();
            var category = CategoryType.Nature.ToString();
            var result = (Result<List<Photo>>)controller.Get(RobertAlias, GetAction.ByCatergory, category);

            Assert.IsTrue(result.Data.All(p=>p.Categories.Any(c=>c.Type.ToLower( ) == category.ToLower( ))), "Es wurden keine Fotos gefunden");
        }

        [TestMethod]
        public void Getting_categories_for_member_will_return_categories()
        {
            var controller = GetControllerWithContext();
            var result = (Result<List<Category>>) controller.Get(RobertAlias, GetAction.Categories);
            Assert.IsTrue(result.Data.Any(), "Es wurden keine Kategorien zurückgegeben");
        }


        [TestMethod]
        public void Getting_locations_for_member_will_return_locations()
        {
            var controller = GetControllerWithContext();
            var result = (Result<List<Location>>)controller.Get(RobertAlias, GetAction.Locations);
            Assert.IsTrue(result.Data.Any(), "Es wurden keine Orte zurückgegeben");
        }

        [TestMethod]
        public void Getting_events_for_member_will_return_events()
        {
            var controller = GetControllerWithContext();
            var result = (Result<List<Event>>)controller.Get(RobertAlias, GetAction.Events);
            Assert.IsTrue(result.Data.Any(), "Es wurden keine Ereignisse zurückgegeben");
        }

        [TestMethod]
        public void Getting_topics_for_member_will_return_topics()
        {
            var controller = GetControllerWithContext();
            var result = (Result<List<Topic>>)controller.Get(RobertAlias, GetAction.Topics);
            Assert.IsTrue(result.Data.Any(), "Es wurden keine Themen zurückgegeben");
        }

        private const int robertMemberId = 3;
        [TestMethod]
        public void Getting_photos_for_member_will_return_photos()
        {
            var controller = GetControllerWithContext();
            var result = (Result<List<Photo>>)controller.Get(RobertAlias, GetAction.Photos);
            Assert.IsTrue(result.Data.Any(p=>p.MemberId == robertMemberId), "Es wurden keine Fotos zurückgegeben");
        }

        [TestMethod]
        public void Getting_stories_for_member_will_return_stories()
        {
            var controller = GetControllerWithContext();
            var result = (Result<List<Story>>)controller.Get(RobertAlias, GetAction.Stories);
            Assert.IsTrue(result.Data.Any(s => s.MemberId == robertMemberId), "Es wurden keine Geschichten zurückgegeben");
        }

        [TestMethod]
        public void Concatinating_null_values_will_succeed()
        {
            var location = new Location() {State = "Bundestaat", Country = "Land",County ="Region"};
            var arrayToConcat = new[] { location.City ?? location.County ?? location.State, location.Country };
            location.Name = string.Join(", ", arrayToConcat
                .Where(x => !string.IsNullOrEmpty(x)));

            Assert.IsTrue(location.Name == string.Format("{0}, {1}", location.County, location.Country));
            location.City = "Stadt";
            
            arrayToConcat = new[] { location.City ?? location.County ?? location.State, location.Country };
            location.Name = string.Join(", ", arrayToConcat
                .Where(x => !string.IsNullOrEmpty(x)));

            Assert.IsTrue(location.Name == string.Format("{0}, {1}", location.City , location.Country));

        }
        [TestMethod]
        public void Test_Yield_will_contain_item_outside_of_loop()
        {
            var list = new List<int>();
            foreach (var i in GetValues())
            {
                list.Add(i);
            }

            Assert.IsTrue(list.Any(item => item == -1));
        }

        private IEnumerable<int> GetValues()
        {
            yield return -1;
            for (var i = 0; i < 10; i++)
            {
                yield return i;
            }
        }

            /// <summary>
        /// Dies ist keine wirkliche Testmethode. Ist nur vorhanden, um Dummydaten zu erzeugen
        /// </summary>
        [TestMethod]
        public void ExtractData()
        {
            
            var rep = new DataRepository();
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };

            File.WriteAllText("multiUpdateModel.json", JsonConvert.SerializeObject(new MultiUpdateModel()));

            File.WriteAllText("members.json", JsonConvert.SerializeObject(rep.Queryable<Member>().Include(m=>m.Options).ToList()));
            return;
            var list = rep.Queryable<Story>()
                .Include(s => s.HeaderPhoto.DirectLinks)
                .Include(s => s.Chapters.Select( c => c.Ledges.Select(l => l.Bricks))).ToList();

            var photoBricksWithPhotos = rep.Queryable<Story>().
                SelectMany(s => s.Chapters.SelectMany(c => c.Ledges.SelectMany(l => l.Bricks))).OfType<PhotoBrick>()
                .Include(b=>b.Photo.DirectLinks).OrderBy(b=>b.Id).ToList();

            var photoBricks = list.SelectMany(s => s.Chapters.SelectMany(c => c.Ledges.SelectMany(l => l.Bricks))).OfType<PhotoBrick>()
                .OrderBy(b=>b.Id).ToList();

            for (var i = 0; i < photoBricks.Count; i++)
            {
                photoBricks[i].Photo = photoBricksWithPhotos[i].Photo;
            }
           
            File.WriteAllText("stories.json", JsonConvert.SerializeObject(
                list, settings));

            return;
            File.WriteAllText("members.json", JsonConvert.SerializeObject(rep.Queryable<Member>()
                .Include(m => m.StorageAccesses)
                .Include(m=>m.HomeLocation)
                .Include(m=>m.SocialMedias)
                .Include(m=>m.Buddies)
                .ToList()));
            File.WriteAllText("membersocialMedia.json", JsonConvert.SerializeObject(rep.Queryable<SocialMedia>().ToList()));
            File.WriteAllText("memberBuddies.json", JsonConvert.SerializeObject(rep.Queryable<Buddy>().ToList()));

            
            
            File.WriteAllText("photos.json", JsonConvert.SerializeObject(rep.Queryable<Photo>()
    .Include(p => p.Location)
    .Include(p => p.Event)
    .Include(p => p.Topics)
    .Include(p => p.DirectLinks)
    .Include(p => p.Exif)
    .ToList(),settings ));
            File.WriteAllText("locations.json", JsonConvert.SerializeObject(rep.Queryable<Location>().ToList()));
            File.WriteAllText("direktLinks.json", JsonConvert.SerializeObject(rep.Queryable<DirectLink>().ToList()));
            return;

            File.WriteAllText("ledges.json", JsonConvert.SerializeObject(rep.Queryable<Ledge>().ToList()));
            File.WriteAllText("chapters.json", JsonConvert.SerializeObject(rep.Queryable<Chapter>().ToList()));            
            File.WriteAllText("ratings.json", JsonConvert.SerializeObject(rep.Queryable<Rating>().ToList()));
            File.WriteAllText("storageProviders.json", JsonConvert.SerializeObject(rep.Queryable<StorageProvider>().ToList()));
            File.WriteAllText("direktLinks.json", JsonConvert.SerializeObject(rep.Queryable<DirectLink>().ToList()));
            File.WriteAllText("members.json", JsonConvert.SerializeObject(rep.Queryable<Member>().Include( m => m.StorageAccesses).ToList()));
            File.WriteAllText("locations.json", JsonConvert.SerializeObject(rep.Queryable<Location>().ToList()));
            File.WriteAllText("events.json", JsonConvert.SerializeObject(rep.Queryable<Event>().ToList()));
            File.WriteAllText("topic.json", JsonConvert.SerializeObject(rep.Queryable<Topic>().ToList()));
            File.WriteAllText("exifs.json", JsonConvert.SerializeObject(rep.Queryable<ExifData>().ToList()));
            File.WriteAllText("stories.json", JsonConvert.SerializeObject(
                rep.Queryable<Story>().Include(s => s.Chapters.Select(c => c.Ledges.Select(l => l.Bricks))).ToList(), settings));
            File.WriteAllText("memberStorageAccesses.json", JsonConvert.SerializeObject(rep.Queryable<MemberStorageAccess>().ToList()));
            
        }
    }
}
