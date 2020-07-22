using System.Data.Entity;
using System.Linq;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository.Poco;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp.Contrib;

namespace Fotosteam.Tests.Service
{
    [TestClass]
    public class PutRequestTests
    {
        private const int TestMemberId = 3;
        #region setup
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

        [TestMethod]
        public void Modifiying_a_member_with_home_location_will_return_home_location_with_Id()
        {
            var member = new Member { Id = TestMemberId };
            var location = new Location { Latitude = 50.078218, Longitude = 8.239761 };
            member.HomeLocation = location;

            RequestHelper.SetupClaim(true);
            var result = RequestHelper.PutResult<Result<Member>>("/api/account/member/", member);

            Assert.IsTrue(result.Data.HomeLocation != null && result.Data.HomeLocation.Id != Constants.NotSetId,
                "Die Location wurde nicht richtig gesetzt");
        }


        [TestMethod]
        public void Modifiying_a_member_to_delete_home_location_will_delete_location()
        {
            var member = new Member { Id = TestMemberId };
            var location =
                RequestHelper.CurrentDataRepository.Queryable<Member>()
                    .Include(m => m.HomeLocation)
                    .First(m => m.Id == TestMemberId).HomeLocation;

            RequestHelper.SetupClaim(true);
            var result = RequestHelper.PutResult<Result<Member>>("/api/account/member/", member);
            var noLocation = RequestHelper.CurrentDataRepository.Queryable<Location>().Any(l => l.Id == location.Id);

            Assert.IsTrue(result.Data.HomeLocation == null && !noLocation, "Die Location wurde nicht entfernt");
        }

        private void ModifyPoco<T>(string elementType, T poco, bool expectFailure = false)
        {
            RequestHelper.SetupClaim(true);
            var result = RequestHelper.PutResult<Result<T>>("/api/data/" + elementType, poco);
            if (expectFailure)
                Assert.IsTrue(result.Status.Code != StatusCode.Success, "Aktualisierung von fehlerhaftem " + elementType + " war erfolgreich:");
            else
                Assert.IsTrue(result.Status.Code == StatusCode.Success, "Aktualisierung von " + elementType + " war nicht erfolgreich:" + result.Status.Message);
        }

        #endregion


        [TestMethod]
        public void Modifiying_a_location_will_return_location_with_Id()
        {
            RequestHelper.ResetRepository();
            var poco = RequestHelper.CurrentDataRepository.Queryable<Location>().First(l => l.MemberId == TestMemberId);
            poco.State = "State";
            ModifyPoco(PostOrPutAction.Location, poco);
        }

        [TestMethod]
        public void Modifiying_a_location_with_invalid_data_will_not_succeed()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<Location>().First(l => l.MemberId == TestMemberId);
            poco.Name = string.Empty;
            ModifyPoco(PostOrPutAction.Location, poco, true);
        }

        [TestMethod]
        public void Modifiying_a_event_will_return_event_with_Id()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<Event>().First(e => e.MemberId == TestMemberId);
            poco.Description = "Neue Beschreibung";
            ModifyPoco(PostOrPutAction.Event, poco);
        }

        [TestMethod]
        public void Modifiying_a_event_with_invalid_data_will_not_succeed()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<Event>().First(e => e.MemberId == TestMemberId);
            poco.Name = string.Empty;
            ModifyPoco(PostOrPutAction.Event, poco, true);
        }

        [TestMethod]
        public void Modifiying_a_photo_will_return_photo_with_Id()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<Photo>().First(p => p.MemberId == TestMemberId);
            poco.Title = "Neuer Titel";
            ModifyPoco(PostOrPutAction.Photo, poco);
        }

        [TestMethod]
        public void Modifiying_a_photo_will_with_invalid_data_will_not_succeed()
        {
            RequestHelper.ResetRepository();
            var poco = RequestHelper.CurrentDataRepository.Queryable<Photo>().First(p => p.MemberId == TestMemberId);
            poco.Title = string.Empty;
            ModifyPoco(PostOrPutAction.Photo, poco, true);
        }

        [TestMethod]
        public void Modifiying_a_textbrick_will_return_textbrick_with_Id()
        {
            RequestHelper.ResetRepository();
            var poco = RequestHelper.CurrentDataRepository.Queryable<TextBrick>().Take(1).First();
            poco.Text = "Das ist ein neuer Text";
            ModifyPoco(PostOrPutAction.Brick, poco);
        }

        [TestMethod]
        public void Modifiying_a_textbrick_with_invalid_data_will_not_succeed()
        {
            RequestHelper.ResetRepository();
            var poco = RequestHelper.CurrentDataRepository.Queryable<TextBrick>().Take(1).First();
            poco.Text = string.Empty;
            ModifyPoco(PostOrPutAction.Brick, poco, true);
        }

        [TestMethod]
        public void Modifiying_a_mapbrick_will_return_mapbrick_with_Id()
        {
            RequestHelper.ResetRepository();
            var poco = RequestHelper.CurrentDataRepository.Queryable<MapBrick>().Take(1).First();
            poco.Latitude = 45.23;
            ModifyPoco(PostOrPutAction.Brick, poco);
        }

        [TestMethod]
        public void Modifiying_a_mapbrick_with_invalid_data_will_not_succeed()
        {
            RequestHelper.ResetRepository();
            var poco = RequestHelper.CurrentDataRepository.Queryable<MapBrick>().Take(1).First();
            poco.Longitude = 200;
            ModifyPoco(PostOrPutAction.Brick, poco, true);
        }

        [TestMethod]
        public void Modifiying_a_photobrick_will_return_photobrick_with_Id()
        {
            RequestHelper.ResetRepository();
            var poco = RequestHelper.CurrentDataRepository.Queryable<PhotoBrick>().Take(1).First();
            poco.PhotoId = 54;

            ModifyPoco(PostOrPutAction.Brick, poco);
        }

        [TestMethod]
        public void Modifiying_a_photobrick_with_invalid_data_will_not_succeed()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<PhotoBrick>().Take(1).First();
            poco.LedgeId = 0;
            poco.PhotoId = 54;

            ModifyPoco(PostOrPutAction.Brick, poco, true);
        }

        [TestMethod]
        public void Modifiying_a_chapter_will_return_chapter_with_Id()
        {
            RequestHelper.ResetRepository();
            var poco = RequestHelper.CurrentDataRepository.Queryable<Chapter>().Take(1).First();
            poco.Name = "Das ist ein neues Kaptiel";
            ModifyPoco(PostOrPutAction.Chapter, poco);
        }

        [TestMethod]
        public void Modifiying_a_chapter_with_invalid_data_will_not_succeed()
        {
            RequestHelper.ResetRepository();
            var poco = RequestHelper.CurrentDataRepository.Queryable<Chapter>().Take(1).First();
            poco.Name = string.Empty;
            ModifyPoco(PostOrPutAction.Chapter, poco, true);
        }

        [TestMethod]
        public void Modifiying_a_ledge_will_return_ledge_with_Id()
        {
            RequestHelper.ResetRepository();
            var poco = RequestHelper.CurrentDataRepository.Queryable<Ledge>().Take(1).First();
            poco.Order = 2;
            ModifyPoco(PostOrPutAction.Ledge, poco);
        }

        [TestMethod]
        public void Modifiying_a_ledge_with_invalid_data_will_not_succeed()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<Ledge>().Take(1).First();
            poco.Template = string.Empty;
            ModifyPoco(PostOrPutAction.Ledge, poco, true);
        }

        [TestMethod]
        public void Modifiying_a_story_will_return_story_with_Id()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<Story>().Take(1).First();
            poco.Name = "Neuer name für die Geschichte";
            poco.Chapters.Clear();
            ModifyPoco(PostOrPutAction.Story, poco);
        }

        [TestMethod]
        public void Modifiying_a_story_with_invalid_data_will_not_succeed()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<Story>().Take(1).First();
            poco.Name = string.Empty;
            ModifyPoco(PostOrPutAction.Story, poco, true);
        }

        [TestMethod]
        public void Modifiying_a_topic_will_return_topic_with_Id()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<Topic>().First(t => t.MemberId == TestMemberId);
            poco.Description = "Erweiterung der Beschreibung";
            poco.MemberId = RequestHelper.TestMemmberId;
            ModifyPoco(PostOrPutAction.Topic, poco);
        }

        [TestMethod]
        public void Modifiying_a_topic_with_invalid_data_will_not_succeed()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<Topic>().First(t => t.MemberId == TestMemberId);
            poco.MemberId = RequestHelper.TestMemmberId;
            poco.Name = string.Empty;
            ModifyPoco(PostOrPutAction.Story, poco, true);
        }

        [TestMethod]
        public void Modifiying_display_section_of_photo_will_change_urls()
        {
            var photo = RequestHelper.CurrentDataRepository.Queryable<Photo>().FirstOrDefault(p => p.Id == 63);
            RequestHelper.SetupClaim(true);
            var model = new ThumbModel { PhotoId = 63, XPercentage = .25f, YPercentage = .25f };
            var result = RequestHelper.PutResult<Result<Photo>>("/api/data/thumbs/", model);
        }

        [TestMethod]
        public void Modifiying_a_socialMedia_will_return_socialMedia_with_Id()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<SocialMedia>().First(p => p.MemberId == TestMemberId);
            poco.Url = HttpUtility.HtmlEncode("http://newurl.com");
            ModifyPoco(PostOrPutAction.SocialMedia, poco);
        }

        [TestMethod]
        public void Modifiying_a_invalid_socialMedia_will_fail()
        {
            var poco = RequestHelper.CurrentDataRepository.Queryable<SocialMedia>().First(p => p.MemberId == TestMemberId);
            poco.Url = "";
            ModifyPoco(PostOrPutAction.SocialMedia, poco, true);
        }

        //Nur manuelll testen
        // [TestMethod]
        public void Changing_the_square_format_of_a_picture_will_change_ulr()
        {
            var photo = RequestHelper.CurrentDataRepository.Queryable<Photo>().Include(p => p.DirectLinks).FirstOrDefault(p => p.Id == 63);
            var url = photo.DirectLinks.First(l => l.Size == 100);

            RequestHelper.SetupClaim(true);
            var model = new ThumbModel { PhotoId = 63, XPercentage = .25f, YPercentage = .25f };

            var result = RequestHelper.PutResult<Result<Photo>>("/api/data/thumbs/", model);
            var newUrl = result.Data.DirectLinks.First(l => l.Size == 100);

            Assert.IsTrue(url != newUrl, "Urls sind nicht unterschiedlich");
        }

        //Nur manuelll testen
        // [TestMethod]
        public void Resting_the_dominant_color_will_return_a_color_value()
        {
            RequestHelper.SetupClaim(true);
            var result = RequestHelper.PutResult<Result<string>>("/api/data/ColorReset/", 63);
            Assert.IsTrue(!string.IsNullOrEmpty(result.Data), "Es wurde kein Wert zurückgeliefert");
        }

        [TestMethod]
        public void Updating_multiple_photos_will_succeed_and_change_the_values()
        {
            var photos =
                RequestHelper.CurrentDataRepository.Queryable<Photo>()
                    .Where(p => p.MemberId == TestMemberId)
                    .Take(3)
                    .ToList();

            RequestHelper.SetupClaim(true);
            var model = new MultiUpdateModel();
            model.PhotoIds = photos.Select(p => p.Id).ToList();
            model.AllowCommenting = true;

            var topics =
                RequestHelper.CurrentDataRepository.Queryable<Topic>()
                    .Where(t => t.MemberId == TestMemberId)
                    .Take(3)
                    .ToList();

            model.Topics = topics.Select(t => t.Id).ToList();

            var testEvent = RequestHelper.CurrentDataRepository.Queryable<Event>().FirstOrDefault(e => e.MemberId == TestMemberId);
            if (testEvent != null)
                model.EventId = testEvent.Id;

            var testLocation = RequestHelper.CurrentDataRepository.Queryable<Location>().FirstOrDefault(e => e.MemberId == TestMemberId);
            if (testLocation != null)
                model.LocationId = testLocation.Id;

            model.License = LicenseType.CcZero;

            model.Fields = MultiEditField.License | MultiEditField.EventId | MultiEditField.Topics | MultiEditField.AllowCommenting ;

            var result = RequestHelper.PutResult<Result<bool>>("/api/data/" + PostOrPutAction.MultiUpdate, model);

            Assert.IsTrue(result.Data, "Es wurde nicht Wahr zurückgegeben");
        }


        [TestMethod]
        public void Updating_multiple_photos_with_incorrect_ids_will_fail()
        {
            var photos =
                RequestHelper.CurrentDataRepository.Queryable<Photo>()
                    .Where(p => p.MemberId == TestMemberId)
                    .Take(3)
                    .ToList();

            RequestHelper.SetupClaim(true);
            var model = new MultiUpdateModel();
            model.PhotoIds = photos.Select(p => p.Id).ToList();
            model.PhotoIds.Add(666);

            var result = RequestHelper.PutResult<Result<bool>>("/api/data/" + PostOrPutAction.MultiUpdate, model);

            Assert.IsTrue(!result.Data, "Es wurde nicht Falsch zurückgegeben");
        }
    }
}