using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Fotosteam.Tests.Service.Fake;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fotosteam.Tests.Service
{
    [TestClass]
    public class DataRepositoryTests
    {
        DataRepository _repository;
        [TestInitialize]
        public void Initialize()
        {
            _repository = new DataRepository(new FakeFotosteamDbContext());
        }


        [TestMethod]
        public void Fetching_a_member_will_return_a_member_object()
        {
            var member = _repository.Queryable<Member>().FirstOrDefault(x => x.Email.ToLower() == "bobbakos@gmail.com");

            Assert.IsTrue(member != null && member.Id != 0, "Es wurde kein  Benutzer gefunden");
        }



        [TestMethod]
        public void Adding_and_deleting_a_member_will_succeed()
        {
            var userId = Guid.NewGuid().ToString();
            var memeber = new Member
            {
                Alias = "newAlias",
                AspNetUserId = userId,
                Email = "test@test.com",
                PlainName = "Test User",
                Avatar100Url = "http://bla.com/avat/asdasd.jpb"
            };

            try
            {
                _repository.Add(memeber);
                Assert.IsTrue(memeber.Id != 0, "Datensatz wurde nicht hinzugefügt");
                _repository.Delete(memeber);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
        [TestMethod]
        public void Find_StorageAccess_by_alias_will_return_storageaccess_object()
        {
            var access =
                _repository.Queryable<Member>().Include(x => x.StorageAccesses)
                    .FirstOrDefault(x => x.Alias == "robert" && x.StorageAccessType == StorageProviderType.Dropbox);

            if (access != null)
            {
                Assert.IsNotNull(access.StorageAccesses.ToList()[0], "Es wurde kein Benutzer mit einem Dropboxzugriff gefunden");
            }
            else
            {
                Assert.Fail("Es wurde kein Benutzer mit einem Dropboxzugriff gefunden");
            }
        }


        [TestMethod]
        public void Seach_for_photos_by_topic_will_return_a_list()
        {
            var list = _repository.GetPhotosByTopic("robert", "Black and white", true, 0, 10);

            Assert.IsTrue(list.Count() > 0, "Es wurden keine Photos gefunden");
        }


        [TestMethod]
        public void Search_for_photos_by_location_will_return_a_list()
        {
            var list = _repository.GetPhotosByLocation("robert", "Sri Lanka", true, GetAction.ByLocation, 0, 10);
            Assert.IsTrue(list.Count() > 0, "Es wurden keine Photos gefunden");

        }
        private const int TestPhotoId = 53;
        [TestMethod]
        public void Adding_a_comment_will_add_an_Entry_and_increase_counter()
        {
            var commentCount = _repository.Queryable<Comment>().Count(x => x.PhotoId == TestPhotoId);

            var comment = new Comment() { UserName = "Robert Bakos", UserAlias = "bobbakos", PhotoId = TestPhotoId, Text = "TestKommentar" };
            comment = _repository.AddComment(comment);
            var photocommentCount = _repository.Queryable<Photo>().First(x => x.Id == TestPhotoId).CommentCount;

            _repository.Execute("Update Photo Set CommentCount =0 where id={0}", TestPhotoId);
            _repository.Execute("Delete from Comment where photoid={0}", TestPhotoId);

            Assert.IsTrue(comment.TotalCount > commentCount, "Der Zähler wurde nicht hochgezählt");
            Assert.IsTrue(photocommentCount > 0, "Der Zähler wurde nicht hochgezählt");
        }

        //kristof: obsolet, da nun summiert wird
        //[TestMethod]
        //public void Adding_a_rating_will_calculate_the_average()
        //{
        //    var rating = new Rating() { UserName = "Robert Bakos", PhotoId = TestPhotoId, Value = 1 };
        //    rating = _repository.AddRating(rating);

        //    Assert.IsTrue(rating.AverageRating == 1, "Es wurde nicht der korrekte Mittelwert berechnet");

        //    rating= new Rating() { UserName = "Robert Bakos", PhotoId = TestPhotoId, Value = 3 };
        //    rating = _repository.AddRating(rating);
        //    Assert.IsTrue(rating.AverageRating == 2, "Es wurde nicht der korrekte Mittelwert berechnet");
        //}

        [TestMethod]
        public void Adding_a_location_will_succeed()
        {
            var location = new Location
            {
                CountryIsoCode = "DE",
                Longitude = 0.1234,
                Name = "Berlin"
            };
            _repository.Add(location);
            Assert.IsTrue(location.Id != 0, "Der Ort wurde nicht gespeichert");
            _repository.Delete(location);

        }

        [TestMethod]
        public void Adding_an_event_will_succeed()
        {
            var newEvent = new Event
            {
                MemberId = 1,
                Description = "Trip durch Indochina",
                Name = "Indochina 2011"
            };
            _repository.Add(newEvent);
            Assert.IsTrue(newEvent.Id != 0, "Das Ereignis wurde nicht gespeichert");
            _repository.Delete(newEvent);
        }

        [TestMethod]
        public void Adding_a_story_will_succeed()
        {
            var story = new Story
            {
                Name = "Test",
                HeaderPhotoId = 54,
                Chapters = new List<Chapter>(),
                MemberId = 3
            };

            var chapter = new Chapter
            {
                Name = "Neues Kapitel",
                Order = 1,
                Ledges = new List<Ledge>()
            };

            var ledge = new Ledge
            {
                Order = 1,
                Template = "LL",
                Bricks = new List<Brick>()
            };

            var brick1 = new TextBrick
            {

                Order = 1,
                Type = "text",
                Text = "Neuer Text"
            };

            var brick2 = new MapBrick
            {
                Order = 2,
                Type = "map",
                Longitude = 51.210581,
                Latitude = 3.222145,
                Zoom = 0
            };

            var brick3 = new PhotoBrick
            {
                Order = 3,
                Type = "photo",
                PhotoId = 54

            };
            ledge.Bricks = new List<Brick> { brick1, brick2, brick3 };
            chapter.Ledges = new List<Ledge> { ledge };
            story.Chapters = new List<Chapter> { chapter };

            _repository.Add(story);
            Assert.IsTrue(story.Id != 0, "Story wurde nicht gespeichert");
            _repository.Delete(story);
        }


        [TestMethod]
        public void Getting_Member_with_budies_will_succeed()
        {
            var member = _repository.Queryable<Member>().Include(m => m.Buddies).FirstOrDefault(m => m.Id == 3);
            Assert.IsTrue(member.Buddies != null && member.Buddies.Count > 0, "Keine Buddies gefunden");
        }

        [TestMethod]
        public void Building_Update_Statement_will_return_correct_statement()
        {
            var model = new MultiUpdateModel()
            {
                PhotoIds = new List<int>() { 1, 2 },
                AllowFullSizeDownload = true,
                EventId = 10,
                LocationId = 0,
                ReplaceExistingValues = true,
                Category = CategoryType.Nature | CategoryType.Animals,
                Fields = MultiEditField.AllowFullSizeDownload | MultiEditField.EventId | MultiEditField.Category | MultiEditField.LocationId | MultiEditField.EventId
            };

            var fieldsAndValues = new Dictionary<string, object>();
            var repository = new DataRepository();

            var actual = repository.BuildUpdateStatement(model, ref fieldsAndValues);
            var expected =
                "UPDATE PHOTO AllowFullSizeDownload={0},Category={1},LocationId={2},EventId={3} WHERE ID IN (1,2)";

            Assert.IsTrue(actual.Equals(expected), "Der String ist ungültig");
        }


    }




}
