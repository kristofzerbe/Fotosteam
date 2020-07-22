using System.Linq;
using System.Security.Principal;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Fotosteam.Tests.Service.Fake;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fotosteam.Tests.Service
{

    [TestClass]
    public class AccountControllerTests
    {
        private readonly DataRepository Rep = new DataRepository(new FakeFotosteamDbContext());

        private AccountController GetControllerWithContext(bool isAuthenticated = false)
        {
            var controller = new AccountController(Rep)
            {
                User = new GenericPrincipal(new TestIdentity("bobbakos@gmail.com", isAuthenticated), null)
            };
            return controller;
        }

        private const string RobertAlias = "robert";
        [TestMethod]
        public void Getting_Member_by_alias_will_return_member()
        {
            var controller = GetControllerWithContext();
            var result = controller.Member(RobertAlias);

            Assert.IsTrue(result.Data.Alias.ToLower().Equals(RobertAlias.ToLower()), "Es wurde nicht das richtige Mittglied zurückgeben");
        }

        [TestMethod]
        public void Getting_a_random_member_will_return_a_member()
        {
            var controller = GetControllerWithContext();
            const int expectedCount = 2;
            var result = controller.MembersRandom(expectedCount);

            Assert.IsTrue(result.Data.Count == expectedCount, "Es wurde nicht die gewüschte Anzahl von Mitgliedern zurückgegeben");
        }

        [TestMethod]
        public void Getting_Buddies_by_alias_will_return_list_of_member()
        {
            var controller = GetControllerWithContext();
            var result = controller.Buddies(RobertAlias);

            Assert.IsTrue(result.Data.Any(), "Es wurde keine Mittglieder zurückgeben");
        }

        [TestMethod]
        public void Getting_Buddies_by_id_will_return_list_of_member()
        {
            var controller = GetControllerWithContext();
            var result = controller.Buddies("3");

            Assert.IsTrue(result.Data.Any(), "Es wurde keine Mittglieder zurückgeben");
        }

        [TestMethod]
        public void Modifiying_a_member_with_home_location_will_return_home_location_with_Id()
        {
            var member = new Member { Id = 3 };
            //Welllington: lat -41.286460,  long 174.776236
            //Wiesbaden: lat 50.078218, long 8.239761
            var location = new Location { Latitude = -41.286460, Longitude = 174.776236 };
            member.HomeLocation = location;

            var controller = GetControllerWithContext(true);
            var result = controller.Member(member);

            Assert.IsTrue(result.Data.HomeLocation != null && result.Data.HomeLocation.Id != Constants.NotSetId,
    "Die Location wurde nicht richtig gesetzt");
        }

    }
}
