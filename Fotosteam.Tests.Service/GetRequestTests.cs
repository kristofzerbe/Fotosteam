using System.Collections.Generic;
using System.Linq;
using System.Net;
using DropNet.Helpers;
using DropNet.Models;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository.Poco;
using Fotosteam.Tests.Service.Fake;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace Fotosteam.Tests.Service
{
    /// <summary>
    ///     Diese Testklasse wird benutzt, um die API über http zu testen
    /// </summary>
    [TestClass]
    public class GetRequestTests
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

        private RestClient GetClient(string baseUrl)
        {
            var restClient = new RestClient(baseUrl);
            var deserializer = new JsonContentDeserializer();
            restClient.ClearHandlers();
            restClient.AddHandler(deserializer.ContentType, new JsonContentDeserializer());
            return restClient;
        }

        #endregion

        //[TestMethod]
        public void Getting_location_by_latitude_and_longitude_will_return_correct_information()
        {
            var helper = new DropNet.Helpers.RequestHelper(string.Empty);
            var latitude = 6.4545366666666668;
            var longitude = 80.896691666666669;
            var request = helper.CreateGeoLocationRequest(longitude, latitude);
            var restClient = GetClient("http://maps.googleapis.com");

            var response = restClient.Execute<GoogleGeoCodeResponse>(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Assert.IsTrue(response.Data.results.Count() == 4,
                    "Es wurde nicht die richtige Anzahl an Ergebnissen zurückgegben");

                var componenents = response.Data.results[0].address_components;
                var location = new Location
                {
                    Country =
                        componenents.Where(x => x.types.Contains("country"))
                            .DefaultIfEmpty(new address_component())
                            .First()
                            .long_name,
                    State =
                        componenents.Where(x => x.types.Contains("administrative_area_level_1"))
                            .DefaultIfEmpty(new address_component())
                            .First()
                            .long_name,
                    County =
                        componenents.Where(x => x.types.Contains("administrative_area_level_2"))
                            .DefaultIfEmpty(new address_component())
                            .First()
                            .long_name,
                    City =
                        componenents.Where(x => x.types.Contains("locality"))
                            .DefaultIfEmpty(new address_component())
                            .First()
                            .long_name
                };
                location.Name = string.Join(",",
                    new[] { location.City, location.County, location.State, location.Country }
                        .Where(x => !string.IsNullOrEmpty(x)));

                Assert.IsTrue(location.Country == "Sri Lanka", "Der Ort wurde nicht richtig erkannt");
            }
            else
            {
                Assert.Fail("Status hatte nicht den richtigen Wert");
            }
        }

        private Result<List<Photo>> GetPhotos(bool isAuthenticated, string resource)
        {
            RequestHelper.SetupClaim(isAuthenticated);
            var result = RequestHelper.GetResult<Result<List<Photo>>>(resource);
            Assert.IsTrue(result.Status.Code == StatusCode.Success, "Der Aufruf ist fehlgeschlagen");
            Assert.IsTrue(result.Data.Count > 0, "Es wurden keine Fotos zurückgegeben");
            Assert.IsTrue(result.Data.Any(p => p.IsPrivate) == isAuthenticated, string.Format("Es wurden {0} private Fotos zurückgegeben", isAuthenticated ? "keine" : string.Empty));
            return result;
        }

        [TestMethod]
        public void Getting_photos_for_unauthorized_user_will_return_photos()
        {
            GetPhotos(false, "/api/data/robert/photos");

        }
        [TestMethod]
        public void Getting_photos_for_authorized_user_will_return_photos()
        {
            GetPhotos(true, "/api/data/robert/photos");
        }

        //[TestMethod]
        public void Getting_photos_lastest_10_Photos_will_return_photos()
        {
            //TODO: Der Test funktioniert nicht, da die Testdaten nicht stimmen
            GetPhotos(true, "/api/data/all/newphotos/0/10");
        }

        [TestMethod]
        public void Getting_photos_for_unauthorized_user_by_location_will_return_photos()
        {
            GetPhotosByLocations(false);
        }
        [TestMethod]
        public void Getting_photos_for_authorized_user_by_location_will_return_photos()
        {
            GetPhotosByLocations(true);
        }
        [TestMethod]
        public void Getting_photos_for_unauthorized_user_by_city_will_return_photos()
        {
            GetPhotosByCity(false);
        }
        [TestMethod]
        public void Getting_photos_for_authorized_user_by_city_will_return_photos()
        {
            GetPhotosByCity(true);
        }
        [TestMethod]
        public void Getting_photos_for_unauthorized_user_by_country_will_return_photos()
        {
            GetPhotosByCountry(false);
        }
        [TestMethod]
        public void Getting_photos_for_authorized_user_by_country_will_return_photos()
        {
            GetPhotosByCountry(true);
        }

        private void GetPhotosByLocations(bool isAuthorized)
        {
            var location = "sri lanka";
            var result = GetPhotos(isAuthorized, "/api/data/robert/location/" + location);
            Assert.IsTrue(result.Data.All(p => p.Location.Name.ToLower().Contains(location.ToLower())), "Es wurden keine Fotos oder mit falschem Ort zurückgeliefert");
        }

        private void GetPhotosByCity(bool isAuthorized)
        {
            var city = "Rambukkana";
            var result = GetPhotos(isAuthorized, "/api/data/robert/city/" + city);
            Assert.IsTrue(result.Data.All(p => p.Location.City.ToLower().Contains(city.ToLower())), "Es wurden keine Fotos oder mit falschem Ort zurückgeliefert");
        }

        private void GetPhotosByCountry(bool isAuthorized)
        {
            var country = "sri lanka";
            var result = GetPhotos(isAuthorized, "/api/data/robert/country/" + country);
            Assert.IsTrue(result.Data.All(p => p.Location.Country.ToLower().Equals(country)), "Es wurden keine Fotos oder mit falschem Ort zurückgeliefert");
        }

        [TestMethod]
        public void Getting_location_group_by_country_will_return_list_of_groups()
        {
            GetList<LocationGroup>("/api/data/robert/locationgroups/Country", "Locationgroup by Country");
        }

        [TestMethod]
        public void Getting_location_group_by_county_will_return_list_of_groups()
        {
            GetList<LocationGroup>("/api/data/robert/locationgroups/County", "Locationgroup by County");
        }

        [TestMethod]
        public void Getting_location_group_by_citry_will_return_list_of_groups()
        {
            GetList<LocationGroup>("/api/data/robert/locationgroups/city", "Locationgroup by City");
        }

        [TestMethod]
        public void Getting_photos_for_unauthorized_user_by_event_will_return_photos()
        {
            GetPhotosByEvent(false);
        }
        [TestMethod]
        public void Getting_photos_for_authorized_user_by_event_will_return_photos()
        {
            GetPhotosByEvent(true);
        }
        private void GetPhotosByEvent(bool isAuthorized)
        {
            var eventName = "Sri Lanka 2014";
            var result = GetPhotos(isAuthorized, "/api/data/robert/event/" + eventName);
            Assert.IsTrue(result.Data.All(p => p.Event.Name.ToLower().Contains(eventName.ToLower())), "Es wurden keine Fotos oder mit falschem Ereignis zurückgeliefert");
        }

        [TestMethod]
        public void Getting_photos_for_unauthorized_user_by_category_will_return_photos()
        {
            GetPhotosByCategory(false);
        }
        [TestMethod]
        public void Getting_photos_for_authorized_user_by_category_will_return_photos()
        {
            GetPhotosByCategory(true);
        }
        private void GetPhotosByCategory(bool isAuthorized)
        {
            var category = CategoryType.Architecture.ToString();
            var result = GetPhotos(isAuthorized, "/api/data/robert/category/" + category);
            Assert.IsTrue(result.Data.All(p => p.Categories.Any(c => c.Type.Equals(category))), "Es wurden keine zur gesuchtem Kategorie zurückgeliefert");
        }

        [TestMethod]
        public void Getting_photos_by_category_for_all_users_will_return_photos()
        {
            var category = CategoryType.Still.ToString();
            var result = GetPhotos(false, "/api/data/all/category/" + category +"/0/3");
            Assert.IsTrue(result.Data.All(p => p.Categories.Any(c => c.Type.Equals(category))), "Es wurden keine zur gesuchtem Kategorie zurückgeliefert");
        }
        
        [TestMethod]
        public void Getting_photos_for_unauthorized_user_by_topic_will_return_photos()
        {
            GetPhotosByTopic(false);
        }
        [TestMethod]
        public void Getting_photos_for_authorized_user_by_topic_will_return_photos()
        {
            GetPhotosByTopic(true);
        }
        private void GetPhotosByTopic(bool isAuthorized)
        {
            var topic = "Travel";
            var result = GetPhotos(isAuthorized, "/api/data/robert/topic/" + topic);
            Assert.IsTrue(result.Data.All(p => p.Topics.Any(t => t.Name.ToLower().Equals(topic.ToLower()))), "Es wurden keine Fotos zum gesuchtem Thema zurückgeliefert");
        }

        [TestMethod]
        public void Getting_categories_for_member_will_return_categories()
        {
            GetList<Category>("/api/data/robert/categories", "Kategorien");
        }

        [TestMethod]
        public void Getting_free_photos_without_any_criterias_will_return_photos()
        {
            RequestHelper.SetupClaim(false);
            var result = RequestHelper.GetResult<Result<List<NewPhoto>>>("/api/data/all/cc0/0/0");
            Assert.IsTrue(result.Data.Count > 0, "Es wurden keine Fotos gefunden");
        }

        [TestMethod]
        public void Getting_free_photos_with_criterias_will_return_photos()
        {
            RequestHelper.SetupClaim(false);
            var result = RequestHelper.GetResult<Result<List<NewPhoto>>>(
                string.Format(
                "/api/data/all/cc0/0/0?category={0}|{1}&topic=best", (int)CategoryType.Abstract, (int)CategoryType.Still));

            Assert.IsTrue(result.Data.Count > 0, "Es wurden keine Fotos gefunden");
        }

        private void GetList<T>(string resource, string itemName)
        {
            RequestHelper.SetupClaim(true);
            var authorizedResult = RequestHelper.GetResult<Result<List<T>>>(resource);
            Assert.IsTrue(authorizedResult.Data.Count > 0, "Es wurden keine " + itemName + " gefunden");
        }

        [TestMethod]
        public void Getting_events_for_member_will_return_events()
        {
            GetList<Event>("/api/data/robert/events", "Ereignisse");
        }

        [TestMethod]
        public void Getting_locations_for_member_will_return_locations()
        {
            GetList<Location>("/api/data/robert/locations", "Orte");
        }
        [TestMethod]
        public void Getting_topics_for_member_will_return_topics()
        {
            GetList<Topic>("/api/data/robert/topics", "Themen");
        }

        [TestMethod]
        public void Getting_comments_for_a_photo_will_return_list_of_comments()
        {
            GetList<Comment>("/api/data/commentsForPhoto/22/1/1", "Kommentare");
        }


        [TestMethod]
        public void Getting_stories_for_member_will_return_stories()
        {
            RequestHelper.SetupClaim(false);
            var result = RequestHelper.GetResult<Result<List<Story>>>("/api/data/robert/stories");
            Assert.IsTrue(result.Data.Count > 0, "Es wurden keine Geschichten gefunden");
        }

        [TestMethod]
        public void Getting_member_for_alias_will_return_member()
        {
            RequestHelper.SetupClaim(false);
            var result = RequestHelper.GetResult<Result<Member>>("/api/account/member/robert");
            FakeFotosteamDbContext.ReloadMembers();

            Assert.IsTrue(result.Status.Code == StatusCode.Success, "Die Abfrage war nicht erfolgreich");
            Assert.IsTrue(result.Data.Alias.ToLower().Equals("robert"), "Es wurde nicht der richtige Benutzer zurück gegeben");
        }

        [TestMethod]
        public void Getting_members_for_alias_will_return_list_of_members()
        {
            RequestHelper.SetupClaim(false);
            var result = RequestHelper.GetResult<Result<List<Member>>>("/api/account/members");
            FakeFotosteamDbContext.ReloadMembers();

            Assert.IsTrue(result.Status.Code == StatusCode.Success, "Die Abfrage war nicht erfolgreich");
            Assert.IsTrue(result.Data.Count > 0, "Es wurde keine Benutzer zurückgegeben");
        }

        [TestMethod]
        public void Getting_random_members_for_alias_will_return_list_of_members()
        {
            RequestHelper.SetupClaim(false);
            var result = RequestHelper.GetResult<Result<List<Member>>>("/api/account/membersrandom/2");
            FakeFotosteamDbContext.ReloadMembers();

            Assert.IsTrue(result.Status.Code == StatusCode.Success, "Die Abfrage war nicht erfolgreich");
            Assert.IsTrue(result.Data.Count == 2, "Es wurde keine 2 Benutzer zurückgegeben");
        }


        [TestMethod]
        public void Verifying_the_login_with_token_will_return_member()
        {
            RequestHelper.SetupClaim(true);
            var result = RequestHelper.GetResult<Result<Member>>("/api/authorize/verifylogin/" + RequestHelper.LoginToken);
            FakeFotosteamDbContext.ReloadMembers();

            Assert.IsTrue(result.Status.Code == StatusCode.Success, "Die Abfrage war nicht erfolgreich");
            Assert.IsTrue(result.Data != null, "Es wurde kein Benutzer zurück gegeben");
        }

        [TestMethod]
        public void Refreshing_content_from_dropbox_will_return_list_of_photos()
        {
            //account/RefreshUserContent
            Assert.Inconclusive("NotImplemented, da hier zu viel simuliert werden müßte");
        }

        [TestMethod]
        public void Verifying_the_dropbox_login_with_token_will_return_member()
        {
            //account/dropBoxVerify/LoginToken
            Assert.Inconclusive("NotImplemented, da hier zu viel simuliert werden müßte");
        }

        [TestMethod]
        public void Getting_memberinfo_for_authenticate_user_will_return_member()
        {
            RequestHelper.SetupClaim(true);

            var result = RequestHelper.GetResult<Result<Member>>("/api/account/MemberInfo");
            FakeFotosteamDbContext.ReloadMembers();

            Assert.IsTrue(result.Status.Code == StatusCode.Success, "Die Abfrage war nicht erfolgreich");
            Assert.IsTrue(result.Data != null, "Es wurde kein Benutzer zurück gegeben");
        }

        [TestMethod]
        public void Getting_memberinfo_for_unauthenticate_user_will_return_statusCode_NotAuthorized()
        {
            RequestHelper.SetupClaim(false);
            var result = RequestHelper.GetResult<Result<Member>>("/api/account/MemberInfo");
            FakeFotosteamDbContext.ReloadMembers();

            Assert.IsTrue(result.Status.Code == StatusCode.NotAuthorized, "Es wurde der falsche Status zurückgegeben");
            Assert.IsTrue(result.Data == null, "Es wurde ein Benutzer zurück gegeben");
        }

        [TestMethod]
        public void Getting_members_will_include_buddies_and_social_medias()
        {
            RequestHelper.SetupClaim(false);

            var result = RequestHelper.GetResult<Result<List<Member>>>("/api/account/Members");
            FakeFotosteamDbContext.ReloadMembers();

            Assert.IsTrue(result.Status.Code == StatusCode.Success, "Die Abfrage war nicht erfolgreich");
            Assert.IsTrue(result.Data.Any(m => m.Buddies.Count > 0 || m.SocialMedias.Count > 0), "Es wurde kein Benutzer zurück gegeben");
        }

        [TestMethod]
        public void Getting_Buddies_by_alias_will_return_list_of_member()
        {
            RequestHelper.SetupClaim(false);

            var result = RequestHelper.GetResult<Result<List<Member>>>("/api/account/Buddies/robert");
            Assert.IsTrue(result.Status.Code == StatusCode.Success, "Die Abfrage war nicht erfolgreich");
            Assert.IsTrue(result.Data.Any(), "Es wurden keine Daten zurück geliefert");
        }

        [TestMethod]
        public void Getting_Buddies_by_wrong_alias_will_return_no_data_found()
        {
            RequestHelper.SetupClaim(false);

            var result = RequestHelper.GetResult<Result<List<Member>>>("/api/account/Buddies/robert1");
            Assert.IsTrue(result.Status.Code == StatusCode.NoData, "Es wurde der falsche Statuscode zurückgeliefert");
        }


        [TestMethod]
        public void Getting_invite_codes_for_robert_will_return_codes()
        {
            RequestHelper.SetupClaim(true);
            var result = RequestHelper.GetResult<Result<List<string>>>("/api/account/InviteCodes/");
            Assert.IsTrue(result.Data.Count > 0, "Es wurden keine Invitecode zurückgeliefert");
        }
        #region sonstige methoden
        [TestMethod]
        public void SignOut_Will_block_user_from_unauthorized_access()
        {
            RequestHelper.SetupClaim(true);
            var result = RequestHelper.GetResult<Result<List<Photo>>>("/api/data/robert/photos");
            Assert.IsTrue(result.Data.Any(p => p.IsPrivate), "Es wurden keine privaten Fotos gefunden");
            RequestHelper.SetupClaim(false);
            var signOutresult = RequestHelper.GetResult<Result<bool>>("/api/authorize/signout");
            result = RequestHelper.GetResult<Result<List<Photo>>>("/api/data/robert/photos");
            Assert.IsFalse(result.Data.Any(p => p.IsPrivate), "Es wurden privaten Fotos gefunden");
        }
        #endregion

    }
}