using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Fotosteam.Service;
using Fotosteam.Service.Repository;
using Fotosteam.Tests.Service.Fake;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Fotosteam.Tests.Service
{
    internal class RequestHelper
    {
        internal static IDataRepository CurrentDataRepository = new DataRepository(new FakeFotosteamDbContext());

        internal const int TestMemmberId = 3;

        private static TestServer _testServer;
        private static readonly Object Lock = new Object();
        
        internal static void CreateServer()
        {
            _testServer = TestServer.Create(app =>
            {
                var startup = new Startup();
                Startup.SerializeTypenamesInJson = true;
                Startup.CreateAuthFilter = () => new TestAuthFilter();
                Startup.DependencyResolver = new DependencyResolver();
                startup.Configuration(app);
            });
        }

        internal static void ResetRepository()
        {
            CurrentDataRepository = new DataRepository(new FakeFotosteamDbContext());
        }

        internal const string LoginToken = "login_token";
        internal static void SetupClaim(bool isAuthenticated)
        {
            TestAuthFilter.UseAuthenticatedUser = isAuthenticated;

            if (isAuthenticated)
            {
                var response = Server.HttpClient.GetAsync("/api/authorize/login?provider=Google&token=" + LoginToken 
                    + "&redirect_uri=http://dummy/dummy.html").Result;

                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Found)
                {
                    Assert.Fail("Benutzer konnte nicht gesetzt werden");
                }

            }
        }

        internal static TestServer Server
        {
            get
            {
                lock (Lock)
                {
                    if(_testServer == null)
                        CreateServer();

                    return _testServer;
                }
            }
        }
        
        private static T Deserialize<T>(HttpResponseMessage response) where T : class
        {
            var content = response.Content.ReadAsStringAsync().Result;
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto

            };
            return JsonConvert.DeserializeObject<T>(content, settings);
        }
        private static T DeserializePrimitve<T>(HttpResponseMessage response) 
        {
            var content = response.Content.ReadAsStringAsync().Result;
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto

            };
            return JsonConvert.DeserializeObject<T>(content, settings);
        }

        internal static T DeleteResult<T>(string resource) where T : class
        {
            var response = Server.HttpClient.DeleteAsync(resource).Result;
            var result = Deserialize<T>(response);
            return result;
        }

        internal static T GetResult<T>(string resource) where T : class
        {
            var response = Server.CreateRequest(resource).GetAsync().Result;
            var result = Deserialize<T>(response);
            return result;
        }
        
        internal static  T PostResult<T>(string resource, object data) where T : class
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = Server.HttpClient.PostAsync(resource, content).Result;
            var result = Deserialize<T>(response);
            return result;
        }

        internal static T PostResultForJsonString<T>(string resource, string data)  
        {
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = Server.HttpClient.PostAsync(resource, content).Result;
            var result = DeserializePrimitve<T>(response);
            return result;
        }

        internal static T PostResultForSingleValue<T>(string resource, object data) 
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = Server.HttpClient.PostAsync(resource, content).Result;
            var result = DeserializePrimitve<T>(response);
            return result;
        }
        internal static T PostResultForDynamic<T>(string resource, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = Server.HttpClient.PostAsync(resource, content).Result;
            var result = DeserializePrimitve<T>(response);
            return result;
        }
        internal static T PostImageResult<T>(string resource, byte[] image) where T : class
        {
            
            var content = new MultipartFormDataContent();
            
            var imageContent = new ByteArrayContent( image);
            imageContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");

            content.Add(imageContent, "image", "image.jpg");
            var response = Server.HttpClient.PostAsync(resource, content).Result;
            var result = Deserialize<T>(response);
            return result;
        }

        internal static T PutResult<T>(string resource, object data) where T : class
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = Server.HttpClient.PutAsync(resource, content).Result;
            var result = Deserialize<T>(response);
            return result;
        }
   
    }

}
