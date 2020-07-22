using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository.Poco;
using Fotosteam.Tests.Service.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Fotosteam.Tests.Service
{
    [TestClass]
    public class PostRequestTests
    {
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

        private void AddPoco<T>(string elementType, T poco, bool expectFailure = false)
        {
            RequestHelper.SetupClaim(true);
            var result = RequestHelper.PostResult<Result<T>>("/api/data/" + elementType, poco);
            if (expectFailure)
                Assert.IsTrue(result.Status.Code != StatusCode.Success, "Hinzufügen von ungültigem " + elementType + "war erfolgreich! Fehler erwartet");
            else
                Assert.IsTrue(result.Status.Code == StatusCode.Success, "Hinzufügen " + elementType + "war nicht erfolgreich:" + result.Status.Message);
        }

        #endregion

        [TestMethod]
        public void Adding_a_new_location_will_return_location_with_Id()
        {
            var poco = new Location
            {
                Name = "Ein toller Ort",
                City = "City",
                County = "Country",
                CountryIsoCode = "DE"
            };
            AddPoco(PostOrPutAction.Location, poco);
        }

        public void Adding_a_new_invalid_location_will_not_succeed()
        {
            var poco = new Location
            {
                City = "City",
                County = "Country",
                CountryIsoCode = "DE"
            };
            AddPoco(PostOrPutAction.Location, poco, true);
        }

        [TestMethod]
        public void Adding_a_new_event_will_return_event_with_Id()
        {
            var poco = new Event
            {
                Name = "Ein neues Ereignis"
            };
            AddPoco(PostOrPutAction.Event, poco);
        }

        [TestMethod]
        public void Adding_a_new_invalid_event_will_not_succeed()
        {
            var poco = new Event
            {
                Description = "Ein Beschreibung"
            };
            AddPoco(PostOrPutAction.Event, poco, true);
        }

        [TestMethod]
        public void Adding_a_new_photo_will_return_photo_with_Id()
        {
            var poco = new Photo
            {
                Title = "Title",
                Name = "tolleBild",
                OriginalName ="tollesBild.jpg", 
                MemberId = RequestHelper.TestMemmberId,
                Width = 1920,
                Height = 1024,
                AspectRation = 1.5,
                Orientation = "landscape",
                Folder = "Test"
            };
            AddPoco(PostOrPutAction.Photo, poco);
        }

        [TestMethod]
        public void Adding_a_new_invalid_photo_will_not_succeed()
        {
            var poco = new Photo
            {
                Name = "tolleBild.jpg"
            };
            AddPoco(PostOrPutAction.Photo, poco, true);
        }

        [TestMethod]
        public void Adding_a_new_textbrick_will_return_textbrick_with_Id()
        {
            var poco = new TextBrick
            {
                LedgeId = 1,
                Text = "Das ist ein kurzer Text für diesen Test"

            };
            AddPoco(PostOrPutAction.Brick, poco);
        }

        [TestMethod]
        public void Adding_a_new_invalid_textbrick_will_not_succeed()
        {
            var poco = new TextBrick
            {
                LedgeId = 1,
                Text = ""
            };
            AddPoco(PostOrPutAction.Brick, poco, true);
        }

        [TestMethod]
        public void Adding_a_new_mapbrick_will_return_mapbrick_with_Id()
        {
            var poco = new MapBrick
            {
                LedgeId = 1,
                Latitude = 83.92,
                Longitude = 32.11,
                Type = BrickType.Map.ToString()
            };
            AddPoco(PostOrPutAction.Brick, poco);
        }

        [TestMethod]
        public void Adding_a_new_invalid_mapbrick_will_not_succeed()
        {
            var poco = new MapBrick
            {
                LedgeId = 1,
                Latitude = -400,
                Longitude = 32.11
            };
            AddPoco(PostOrPutAction.Brick, poco, true);
        }

        [TestMethod]
        public void Adding_a_new_photobrick_will_return_photobrick_with_Id()
        {
            var poco = new PhotoBrick
            {
                LedgeId = 1,
                PhotoId = 54
            };
            AddPoco(PostOrPutAction.Brick, poco);
        }


        [TestMethod]
        public void Adding_a_new_chapter_will_return_chapter_with_Id()
        {
            var poco = new Chapter
            {
                StoryId = 1,
                Name = "Kapitel 1"
            };
            AddPoco(PostOrPutAction.Chapter, poco);
        }

        [TestMethod]
        public void Adding_a_new_invalid_chapter_will_not_succeed()
        {
            var poco = new Chapter { StoryId = 1, };

            AddPoco(PostOrPutAction.Chapter, poco, true);
        }

        [TestMethod]
        public void Adding_a_new_ledge_will_return_ledge_with_Id()
        {
            var poco = new Ledge
            {
                ChapterId = 1,
                Template = "bla" //TODO: Gültige Templates definieren
            };
            AddPoco(PostOrPutAction.Ledge, poco);
        }

        [TestMethod]
        public void Adding_a_new_invalid_ledge_will_not_succeed()
        {
            var poco = new Ledge();
            AddPoco(PostOrPutAction.Ledge, poco, true);
        }

        [TestMethod]
        public void Adding_a_new_story_will_return_story_with_Id()
        {
            var poco = new Story
            {
                Name = "Ein tolle Geschichte"
            };
            AddPoco(PostOrPutAction.Story, poco);
        }

        [TestMethod]
        public void Adding_a_new_invalid_story_will_not_succeed()
        {
            var poco = new Story();
            AddPoco(PostOrPutAction.Story, poco, true);
        }

        [TestMethod]
        public void Adding_a_new_topic_will_return_topic_with_Id()
        {
            var poco = new Topic
            {
                Name = "Ein neues Thema"
            };
            AddPoco(PostOrPutAction.Topic, poco);
        }

        [TestMethod]
        public void Adding_a_new_invalid_topic_will_not_succeed()
        {
            var poco = new Topic();
            AddPoco(PostOrPutAction.Topic, poco, true);
        }

        [TestMethod]
        public void Adding_a_new_valid_socialMedia_will_not_succeed()
        {
            var poco = new SocialMedia()
            {
                MemberId = RequestHelper.TestMemmberId,
                Type = MediaType.Facebook,
                Url ="http://www.facebook.com/bla"
            };
            AddPoco(PostOrPutAction.SocialMedia, poco, false);
        }

        [TestMethod]
        public void Adding_a_new_invalid_socialMedia_will_not_succeed()
        {
            var poco = new SocialMedia();
            AddPoco(PostOrPutAction.SocialMedia, poco, true);
        }



        [TestMethod]
        public void Deserializing_json_will_create_model_with_correct_type()
        {
            var model = "{\"Id\":\"1\",\"Type\":\"photo\",\"PropertyName\":\"Title\",\"Value\":\"New Title\"}";
            var photoModel = JsonConvert.DeserializeObject<UpdateModel>(model);
            Assert.IsTrue(photoModel.Type == ModelType.Photo, "Typ wurde nicht richtig deserialisiert");

        }

        [TestMethod]
        public void Changing_the_title_of_a_photo_will_succeed()
        {
            var photo = RequestHelper.CurrentDataRepository.Queryable<Photo>().FirstOrDefault(p => p.MemberId == 3);
            var newTitle = photo.Title + " Changed";
            photo.IsNew = false;
            var model = new UpdateModel
            {
                Id = photo.Id,
                Type = ModelType.Photo,
                PropertyName = "Title",
                Value = newTitle
            };

            RequestHelper.SetupClaim(true);
            var result = RequestHelper.PostResult<Result<bool>>("/api/data/update", model);
            Assert.IsTrue(result.Data, "Es wurde nicht Wahr zurückgebren");
            var changedPhoto = RequestHelper.CurrentDataRepository.Queryable<Photo>().FirstOrDefault(p => p.Id == photo.Id);
            Assert.IsTrue(changedPhoto.Title.Equals(newTitle), "Der Title wurde nicht aktualisiert");
        }

        [TestMethod]
        public void Changing_the_capture_date_of_a_photo_will_succeed()
        {
            //CaptureDate
            var photo = RequestHelper.CurrentDataRepository.Queryable<Photo>().FirstOrDefault(p => p.MemberId == 3);
            
            var model = new UpdateModel
            {
                Id = photo.Id,
                Type = ModelType.Photo,
                PropertyName = "CaptureDate",
                Value = "2015-05-17"
            };

            RequestHelper.SetupClaim(true);
            var result = RequestHelper.PostResult<Result<bool>>("/api/data/update", model);
            Assert.IsTrue(result.Data, "Es wurde nicht Wahr zurückgebren");
            var changedPhoto = RequestHelper.CurrentDataRepository.Queryable<Photo>().FirstOrDefault(p => p.Id == photo.Id);
            Assert.IsTrue(changedPhoto.CaptureDate.Equals(new DateTime(2015,5,17)), "Der Title wurde nicht aktualisiert");
        }
        
        [TestMethod]
        public void Changing_the_location_of_a_photo_to_not_set_will_succeed()
        {
            var photo = RequestHelper.CurrentDataRepository.Queryable<Photo>().FirstOrDefault(p => p.MemberId == 3 && p.LocationId != null);

            var model = new UpdateModel
            {
                Id = photo.Id,
                Type = ModelType.Photo,
                PropertyName = "LocationId",
                Value = "0"
            };

            RequestHelper.SetupClaim(true);
            var result = RequestHelper.PostResult<Result<bool>>("/api/data/update", model);
            Assert.IsTrue(result.Data, "Es wurde nicht Wahr zurückgebren");
            var changedPhoto = RequestHelper.CurrentDataRepository.Queryable<Photo>().FirstOrDefault(p => p.Id == photo.Id);
            Assert.IsTrue(changedPhoto.LocationId ==null, "Der Ort wurde nicht aktualisiert");
        }
        [TestMethod]
        public void Changing_the_home_location_for_robert_will_succeed()
        {
            var member = RequestHelper.CurrentDataRepository.Queryable<Member>().FirstOrDefault(p => p.Id == 3);
            var model = new UpdateModel
            {
                Id = member.Id,
                Type = ModelType.Member,
                PropertyName = "HomeLocation_Id",
                Value = "71"
            };

            RequestHelper.SetupClaim(true);
            var result = RequestHelper.PostResult<Result<bool>>("/api/data/update", model);
            Assert.IsTrue(result.Data, "Es wurde nicht Wahr zurückgebren");
        }

        [TestMethod]
        public void Checking_for_a_none_existing_alias_being_avaiable_will_return_true()
        {
            RequestHelper.SetupClaim(false);
            var member = RequestHelper.CurrentDataRepository.Queryable<Member>().FirstOrDefault();
            var result = RequestHelper.PostResultForSingleValue<bool>("/api/account/IsAliasAvailable", "Dasgibteseinfachnicht");
            Assert.IsTrue(result, "Es wurde nicht wahr zurückgegeben");
        }

        [TestMethod]
        public void Checking_for_an_existing_alias_being_avaiable_will_return_false()
        {
            RequestHelper.SetupClaim(false);
            var member = RequestHelper.CurrentDataRepository.Queryable<Member>().FirstOrDefault();
            var result = RequestHelper.PostResultForSingleValue<bool>("/api/account/IsAliasAvailable", member.Alias);
            Assert.IsFalse(result, "Es wurde nicht falsch zurückgegeben");
        }

               
        [TestMethod]
        public void Adding_a_buddy_will_succeed()
        {
            RequestHelper.SetupClaim(true);
            
            var body = "{\"BuddyMemberId\":\"44\"}";
            var result = RequestHelper.PostResultForJsonString<Result<Buddy>>("/api/data/addbuddy", body);
            Assert.IsTrue( result.Status.Code == StatusCode.Success , "Die Operation war nicht erfolgreich");
            var buddy =
                RequestHelper.CurrentDataRepository.Queryable<Buddy>()
                    .FirstOrDefault(b => b.MemberId == 3 && b.BuddyMemberId == 44);
            Assert.IsTrue(buddy != null, "Es wurde kein Buddyeintrag erzeugt");
            var notification =
                RequestHelper.CurrentDataRepository.Queryable<Notification>()
                    .FirstOrDefault(n => n.MemberId == 44 && n.Type == NotificationType.BuddyRequest);
            Assert.IsTrue(notification != null, "Es wurde kein Benachrichtung erzeugt");
        }

        /*
        Dafür ist kein Fake vorhanden. Kann nur mit Dummy SMTP getestet werden
         * [TestMethod]
        public void Sending_an_email_to_info_will_return_true()
        {
            RequestHelper.SetupClaim(true);
            var message = new CommunicationController.Message { Body = "Body", SenderEmail = "robertbakos@hotmail.com", SenderName = "Test", Title = "Test Title" };
            var result = RequestHelper.PostResult<Result<bool>>("/api/communication/contact", message);
            Assert.IsTrue(result.Data, "Es wurde nicht wahr zurückgegeben");

        }

        [TestMethod]
        public void Sending_an_email_to_trello_will_return_true()
        {
            RequestHelper.SetupClaim(true);
            var message = new CommunicationController.Message { Body = "Body", SenderEmail = "robertbakos@hotmail.com", SenderName = "Test", Title = "Test Title" };
            var result = RequestHelper.PostResult<Result<bool>>("/api/communication/trello", message);
            Assert.IsTrue(result.Data, "Es wurde nicht wahr zurückgegeben");

        }


        [TestMethod]
        public void Sending_an_email_to_robert_will_return_true()
        {
            RequestHelper.SetupClaim(true);
            var message = new CommunicationController.Message { Body = "Body", SenderEmail = "robertbakos@hotmail.com", SenderName = "Test", Title = "Test Title" };
            var result = RequestHelper.PostResult<Result<bool>>("/api/communication/member/robert", message);
            Assert.IsTrue(result.Data, "Es wurde nicht wahr zurückgegeben");

        }
        */

        private const int PhotoId = 66;
        [TestMethod]
        public void Adding_a_comment_will_increase_comment_counter()
        {
            RequestHelper.SetupClaim(false);
            var member =RequestHelper.CurrentDataRepository.Queryable<Member>().Include(m => m.Options).First(m => m.Id == 3);
            member.Options.AllowComments = true;
            var newComment = new Comment() { PhotoId = PhotoId, Text = "This is a comment", UserName = "Benutzer", UserAlias = "User"};
            var result = RequestHelper.PostResult<Result<Comment>>("/api/communication/addComment", newComment);

            Assert.IsTrue(result.Data.TotalCount > 0, "Der Zähler wurde nicht hochgesetzt");            
        }

        [TestMethod]
        public void Adding_a_comment_will_add_a_notification()
        {
            RequestHelper.SetupClaim(false);
            var newComment = new Comment() { PhotoId = PhotoId, Text = "This is a comment", UserName = "Benutzer", UserAlias = "User" };
            var member =
                RequestHelper.CurrentDataRepository.Queryable<Member>().Include(m => m.Options).First(m => m.Id == 3);
            member.Options.AllowComments = true;
            var oldCount = RequestHelper.CurrentDataRepository.Queryable<Notification>().Count();
            var result = RequestHelper.PostResult<Result<Comment>>("/api/communication/addComment", newComment);
            var newCount = RequestHelper.CurrentDataRepository.Queryable<Notification>().Count();
            Assert.IsTrue(oldCount<newCount, "Es wurde kein Benachrichtungsobjekt erstellt");
        }

        [TestMethod]
        public void Adding_a_rating_will_change_the_overall_rating()
        {
            RequestHelper.SetupClaim(false);
            var newRating = new Rating() {PhotoId = PhotoId,Value= 3, UserAlias ="User"};
            var member =
    RequestHelper.CurrentDataRepository.Queryable<Member>().Include(m => m.Options).First(m => m.Id == 3);
            member.Options.AllowRating  = true;
            var photo = RequestHelper.CurrentDataRepository.Queryable<Photo>().FirstOrDefault(p => p.Id == PhotoId);
            var oldValue = photo.RatingSum;
            var result = RequestHelper.PostResult<Result<Rating>>("/api/communication/addRating", newRating);
            var newValue = result.Data.RatingSum;
            Assert.IsTrue(newValue != oldValue, "Die Summe des Ratings wurde nicht geändert");

        }

        

        [TestMethod]
        public void Merging_location_will_delete_slave_and_update_all_photos()
        {
            RequestHelper.SetupClaim(true);
            var photo = RequestHelper.CurrentDataRepository.Queryable<Photo>().FirstOrDefault(p => p.Id == PhotoId);
            object value = new {NewId = 45, OldId= 46};
            var result = RequestHelper.PostResult<Result<bool>>("/api/data/mergelocations", value);
            Assert.IsTrue(result.Data,"Das Zusammenführen war nicht erfolgreich");
        }
    [TestMethod]
        public void Adding_a_topic_to_a_photo_will_succeed()
        {
            RequestHelper.SetupClaim(true);
            var photo = RequestHelper.CurrentDataRepository.Queryable<Photo>().FirstOrDefault(p => p.MemberId == 3 && p.Topics.Count >0 );
            var topic = RequestHelper.CurrentDataRepository.Queryable<Topic>().FirstOrDefault(t => t.MemberId == 3);
            object value = new { PhotoId = photo.Id, TopicId= topic.Id,  Action="add"};
            var result = RequestHelper.PostResult<Result<bool>>("/api/data/phototopic", value);
            Assert.IsTrue(result.Data, "Die Aktualisierung war nicht erfolgreich");
        }
        #region Manuelle Tests
        //[TestMethod]
        public void Uploading_a_new_avatar_image_will_update_links()
        {
            RequestHelper.SetupClaim(true);

            var converter = new ImageConverter();
            var imgArray = (byte[])converter.ConvertTo(Resources.avatar_400, typeof(byte[]));
            var member = RequestHelper.CurrentDataRepository.Queryable<Member>().FirstOrDefault(m => m.Id == 3);
            var url = member.Avatar100Url;
            var result = RequestHelper.PostImageResult<Result<Member>>("/api/account/avatarimage", imgArray);

            Assert.IsTrue(url != result.Data.Avatar100Url, "Die Urls sind gleich");
        }

        //[TestMethod]
        public void Uploading_a_new_header_image_will_update_links()
        {
            RequestHelper.SetupClaim(true);

            var converter = new ImageConverter();
            var imgArray = (byte[])converter.ConvertTo(Resources.chess, typeof(byte[]));
            var member = RequestHelper.CurrentDataRepository.Queryable<Member>().FirstOrDefault(m => m.Id == 3);
            var url = member.Header1024Url;
            var result = RequestHelper.PostImageResult<Result<Member>>("/api/account/header", imgArray);

            Assert.IsTrue(url != result.Data.Header1024Url, "Die Urls sind gleich");
        }

        //[TestMethod]
        public void Modifiying_the_header_for_a_member_by_photoid_will_succeed()
        {
            RequestHelper.SetupClaim(true);

            var member = RequestHelper.CurrentDataRepository.Queryable<Member>().FirstOrDefault(m => m.Id == 3);
            var url = member.Header1024Url;
            const int photoId = 63; //63 valud, not valid 35
            var result = RequestHelper.PostResult<Result<Member>>("/api/account/header", photoId);

            Assert.IsTrue(url != result.Data.Header1024Url, "Die Urls sind gleich");
        }

        // [TestMethod]
        public void Uploading_a_new_image_will_return_photo_obejct()
        {
            RequestHelper.SetupClaim(true);

            var converter = new ImageConverter();
            var imgArray = (byte[])converter.ConvertTo(Resources.chess, typeof(byte[]));

            var result = RequestHelper.PostImageResult<Result<Photo>>("/api/data/photo", imgArray);

            Assert.IsTrue(result.Data != null, "Es wurde kein Photo-Objekt zurückgegeben");
        }

        [TestMethod]
        public void Getting_drive_authorization_url_will_return_url()
        {
            RequestHelper.SetupClaim(true);
            var result = RequestHelper.PostResult<Result<string>>("/api/authorize/GetDriveAuthorizationUrl", "https://localhost:44300/api/authorize/authorizedrive/");
            Assert.IsFalse(string.IsNullOrEmpty(result.Data), "Es wurde keine Url zurückgeliefert");
        }
        #endregion
    }
}