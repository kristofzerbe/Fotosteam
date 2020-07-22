using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Fotosteam.Service.Connector.Dropbox;
using Fotosteam.Service.Connector.GoogleDrive;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Fotosteam.Service.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Constants = Fotosteam.Service.Models.Constants;

namespace Fotosteam.Service.Controller
{
    /// <summary>
    /// Definiert Methoden, die für
    /// </summary>
    [RoutePrefix("api/authorize")]
    public class AuthorizeController : ControllerBase
    {

        /// <summary>
        ///     Überladener Konstruktor, der die Überbabe eines Repositories erlaubt
        /// </summary>
        /// <param name="dataRepository">Repositiory-Objekt zur Interaktion mit persistierbaren Daten</param>
        public AuthorizeController(IDataRepository dataRepository)
        {
            Log.Debug(string.Format("Intialisiere AccountController mit externem Datarepository {0}",
                dataRepository.GetType()));
            DataRepository = dataRepository;
            AuthenticationRepository = new AuthRepository();
        }

        /// <summary>
        ///     Überladener Konstruktor, der die Übergabe von mehreren Repositories erlaubbt
        /// </summary>
        /// <param name="dataRepository">Repositiory-Objekt zur Interaktion mit persistierbaren Daten</param>
        /// <param name="authRepository">
        ///     Repositiory-Objekt zur Interaktion mit benutzerspezifischen Daten für die
        ///     Authentifizierung
        /// </param>
        public AuthorizeController(IDataRepository dataRepository, IAuthRepository authRepository)
        {
            Log.Debug(
                string.Format("Intialisiere AccountController mit externem Datarepository {0} und AuthRepository {1}",
                    dataRepository.GetType(), authRepository.GetType()));
            DataRepository = dataRepository;
            AuthenticationRepository = authRepository;
        }

        /// <summary>
        ///     Standarkonstruktor, der sowohl ein Daten- als auch ein Authentifzierungsobjekt initalisiert
        /// </summary>
        public AuthorizeController()
        {
            AuthenticationRepository = new AuthRepository();
            DataRepository = new DataRepository();
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        /// <summary>
        ///     Registriert einen neues Mitglied mit dem entsprechenden Provider
        /// </summary>
        /// <param name="model">RegisterExternalBindingModel, das alle notwendigen Daten enthält</param>
        /// <returns>Einen HTTPResponse mit Erfolg oder Misserfolg</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> RegisterExternal([FromBody] RegisterExternalBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
                if (verifiedAccessToken == null)
                {
                    return BadRequest("Invalid Provider or External Access Token");
                }

                var user =
                    await
                        AuthenticationRepository.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.UserId));

                var hasRegistered = user != null;

                if (hasRegistered)
                {
                    return BadRequest("External user is already registered");
                }

                var alias = model.Alias.Replace(" ", string.Empty);

                user = new IdentityUser { UserName = model.Email, Email = model.Email };

                var result = await AuthenticationRepository.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                var newAlias = alias;
                if (DataRepository.Find<Member>(m => m.Alias == alias) != null)
                {
                    newAlias = newAlias + model.Email.GetHashCode();
                }

                var info = new ExternalLoginInfo
                {
                    DefaultUserName = newAlias,
                    Email = model.Email,
                    Login = new UserLoginInfo(model.Provider, model.ExternalAccessToken)
                };

                result = await AuthenticationRepository.AddLoginAsync(user.Id, info.Login);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                var member = CreateNewMember(model, newAlias, user);
                
                lock (LockObject)
                {
                    if (!AuthorizedUsers.ContainsKey(model.ExternalAccessToken))
                    {
                        AuthorizedUsers.Add(model.ExternalAccessToken, user);
                    }
                }
                var memberResult = new Result<Member> { Data = member };
                return Ok(memberResult);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return BadRequest("Internal Failure");
            }
        }

        private Member CreateNewMember(RegisterExternalBindingModel model, string newAlias, IdentityUser user)
        {
            var member = CreateMember(model, newAlias, user);
            DataRepository.AddNewMember(member, model.Language);                        
            return member;
        }


        private static Member CreateMember(RegisterExternalBindingModel model, string newAlias, IdentityUser user)
        {
            var member = new Member
            {
                Alias = newAlias,
                AspNetUserId = user.Id,
                Email = model.Email,
                ProviderKey = model.ExternalAccessToken,
                PlainName = model.UserName,
            };
            return member;
        }

        /// <summary>
        ///     Authentifiziert den Benutzer über OAuth mit Google, Facebook, Twitter
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        // GET api/Account/ExternalLogin
        [AllowAnonymous]
        [Route("login", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            try
            {
                var redirectUri = "";

                if (error != null)
                {
                    if (error == "access_denied")
                    {
                        return Redirect(RedirectUrl);
                    }
                    return BadRequest(Uri.EscapeDataString(error));
                }

                if (!User.Identity.IsAuthenticated)
                {
                    return new ChallengeResult(provider, this);
                }

                var token = ValidateClientAndRedirectUri(ref redirectUri);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest(token);
                }

                var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

                if (externalLogin == null)
                {
                    return InternalServerError();
                }

                if (externalLogin.LoginProvider != provider)
                {
                    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    return new ChallengeResult(provider, this);
                }

                var member = new Member();
                var user =
                    await
                        AuthenticationRepository.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                            externalLogin.ProviderKey));
                if (user != null)
                {
                    member = DataRepository.Queryable<Member>().FirstOrDefault(x => x.AspNetUserId == user.Id);
                    if (member == null || member.Id == Constants.NotFound)
                    {
                        member = new Member {Email = externalLogin.EmailAddress, PlainName = externalLogin.UserName};
                    }
                    lock (LockObject)
                    {
                        if (!AuthorizedUsers.ContainsKey(externalLogin.ProviderKey))
                        {
                            AuthorizedUsers.Add(externalLogin.ProviderKey, user);
                        }
                    }
                    var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalLogin.ProviderKey);
                    if (verifiedAccessToken == null)
                    {
                        return BadRequest("Invalid Provider or External Access Token");
                    }
                    var currentUtc = new SystemClock().UtcNow;
                    var expiresUtc = currentUtc.Add(TimeSpan.FromHours(3.0));
                    Authentication.SignIn(
                        new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            ExpiresUtc = expiresUtc,
                            IssuedUtc = currentUtc,
                            IsPersistent = true
                        }, User.Identity as ClaimsIdentity);
                }
                else
                {
                    member.Alias = externalLogin.UserName;
                    member.Email = externalLogin.EmailAddress ?? Constants.NotSet;
                    member.PlainName = externalLogin.UserName;
                }
                member.ProviderKey = externalLogin.ProviderKey;

                lock (LockObject)
                {
                    if (!AuthenticatedMembers.ContainsKey(token))
                    {
                        AuthenticatedMembers.Add(token, member);
                    }
                }

                if (string.IsNullOrEmpty(redirectUri))
                {
                    return Ok(member);
                }

                return Redirect(redirectUri);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                var result = new Result<bool>();
                result.Status.Code = Controller.StatusCode.NotAuthorized;          
                return Ok(result);
            }
        }

        /// <summary>
        ///     Führt das Logout durch. Was aber aufgrund des AuthO2 mechanismus nur funktioniert,
        ///     wenn der Cookie auf dem Client gelöscht wird. Daher ist das hier nur eine Teillösung
        /// </summary>
        /// <returns>Wahr</returns>
        [HttpGet]
        public Result<bool> SignOut()
        {
            var result = new Result<bool>();
            try
            {
                var user = GetMemberFromAuthenticatedUser();
                if (user != null)
                {
                    foreach (var member in AuthenticatedMembers.Where(member => member.Value.Id == user.Id))
                    {
                        AuthenticatedMembers.Remove(member.Key);
                        break;
                    }
                }
                Authentication.SignOut("ExternalCookie", "ApplicationCookie");
                Authentication.SignOut();
                result.Data = true;
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return result;
        }

        /// <summary>
        ///     Überprüft ob die Authtifzierung erfolgreic war und liefert das entsprechende Member-Objekt zurück
        /// </summary>
        /// <param name="data">Der Token, der zur Authentifizierung genutzt wurde</param>
        /// <returns>Member-Objekt</returns>
        [HttpGet]
        public object VerifyLogin(string data)
        {
            try
            {
                var result = FindUserByToken(data);
                if (result.Data == null)
                {
                    result.Data = new Member();
                    result.Status.Code = Controller.StatusCode.NotAuthorized;
                    return result;                    
                }
                if (result.Data.Id == Constants.NotSetId)
                {
                    dynamic member = new
                    {
                        Id = Constants.NotSetId,
                        result.Data.PlainName,
                        result.Data.Email,
                        result.Data.ProviderKey,
                        result.Data.Alias
                    };
                    return new Result<object> {Data = member};
                }
                return result;
            }
            catch (Exception ex)
            {
                var result = new Result<object>(){Data = null};
                LogExceptionAndSetResult(ex, result);
                return result;
            }
        }


        internal static bool IsAuthorized(IIdentity user)
        {
            var externalLogin = ExternalLoginData.FromIdentity(user as ClaimsIdentity);
            return externalLogin != null && AuthorizedUsers.ContainsKey(externalLogin.ProviderKey);
        }

        #region Dropboxspezfische Funktioen

        private DropboxConnector _connector;

        private DropboxConnector Connector
        {
            get { return _connector ?? (_connector = new DropboxConnector(DataRepository)); }
        }

        private static readonly Dictionary<string, Member> AuthenticatedMembers = new Dictionary<string, Member>();

        private static readonly Dictionary<string, DropboxConnector> AuthorizeConnector = new Dictionary<string, DropboxConnector>();

        private static string _confirmationUrl;

        /// <summary>
        ///     Liefert die URL für die Authorisierung der Anwendung zurück
        /// </summary>
        /// <returns>Die Url für die Authorisierung</returns>
        /// <remarks>Der Benutzer muss sich vorher angemeldert haben</remarks>
        [Authorize]
        [HttpPost]
        public Result<string[]> DropboxAuthorisationUrl([FromBody] string url)
        {
            _confirmationUrl = RedirectUrl;
            var data = new string[2];
            var result = new Result<string[]>();
            data[1] = Guid.NewGuid().ToString();
            try
            {
                data[0] = Connector.GetAuthorizeUrl(_confirmationUrl,data[1]);
                lock (LockObject)
                {
                    AuthorizeConnector.Add(data[1], Connector);
                }

                result.Data = data;
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return result;
        }

        private static readonly Object LockObject = new Object();


        /// <summary>
        ///     Wird aufgerufen, um die Authorisierung abzuschließen
        /// </summary>
        /// <param name="data">Der Identifier, damit wir den richtigen Connector zurück bekommen</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public Result<Member> DropBoxVerify()
        {            
            lock (LockObject)
            {
                var result = new Result<Member>();
                try
                {
                    var data = Request.GetQueryNameValuePairs().First(x => x.Key == "state").Value;
                    var query = Request.RequestUri.Query.Substring(1);
                    var uri = new Uri(RedirectUrl + "#" + query);

                    var connector = AuthorizeConnector[data];
                    connector.CurrentMember = GetMemberWithStorageAccesses();
                    
                    var member = connector.GetUserAuthentication(uri);
                    AuthorizeConnector.Remove(data);
                    member.StorageAccesses.Clear();
                    result.Data = member;
                }
                catch (Exception ex)
                {
                    LogExceptionAndSetResult(ex, result);
                }
                return result;
            }
        }

        /// <summary>
        ///     Liefert die Url zur Authorizierung von Goolge-Drive zurück
        /// </summary>
        /// <param name="url">Die Redirect-Url, die von Google aufgerufen wird</param>
        /// <returns>Result mit der Url</returns>
        [HttpPost]
        [Authorize]
        public Result<string> GetDriveAuthorizationUrl([FromBody] string url)
        {
            var result = new Result<string>();
            try
            {
                var connector = new GoogleDrive(DataRepository);                
                var memberId = GetMemberFromAuthenticatedUser().Id;
                var access =
                    DataRepository.Queryable<MemberStorageAccess>()
                        .FirstOrDefault(s => s.MemberId == memberId && s.Type == StorageProviderType.GoogleDrive);
                
                if (access != null && !access.Token.StartsWith("{\"access_token\":"))
                    DataRepository.Delete(access);

                result.Data = connector.GetAuthorizeUrl(Request.RequestUri.ToString(), url, memberId);
                if (result.Data == null)
                {
                    result.Status.Code = Controller.StatusCode.GoogleDriverAlreadyAuthorized;
                }
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }

            return result;
        }


        /// <summary>
        ///     Liefert die Url zur Authorizierung von Goolge-Drive zurück
        /// </summary>
        /// <param name="url">Die Redirect-Url, die von Google aufgerufen wird</param>
        /// <returns>Result mit der Url</returns>
        [HttpPost]
        [Authorize]
        public Result<string> GetOneDriveAuthorizationUrl([FromBody] string url)
        {
            var result = new Result<string>();
            try
            {
                var connector = new Connector.OneDrive.OneDrive(DataRepository);
                var memberId = GetMemberFromAuthenticatedUser().Id;
                result.Data = connector.GetAuthorizeUrl(Request.RequestUri.ToString(), url, memberId);
                if (result.Data == null)
                {
                    result.Status.Code = Controller.StatusCode.GoogleDriverAlreadyAuthorized;
                }
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }

            return result;
        }


        /// <summary>
        ///     Führt die Authorizierung durch und speichert den Token für die spätere Verwendung
        /// </summary>
        /// <param name="code">Der Authorizierungscode von Google</param>
        /// <param name="state">Zusatzinformation, z.B. benutzerdefinierter Token, der vom Provider zurückgeliefert werden soll</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IHttpActionResult AuthorizeDrive([FromUri] string code, [FromUri] string state)
        {
            var url = RedirectUrl;
            try
            {
                var connector = new GoogleDrive(DataRepository);

                var member = GetMemberFromAuthenticatedUser();
                var memberId = member.Id;
                
                var isAuthorized = connector.Authorize(code, memberId, Request.RequestUri.ToString());

                if (isAuthorized && member.StorageAccessType == StorageProviderType.None)
                {
                    member.StorageAccessType = StorageProviderType.GoogleDrive;
                    DataRepository.Update(member);
                }
                connector.RefreshFolderContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return BadRequest("Cannot authorize");
            }
            return Redirect(url);
        }

        /// <summary>
        ///     Führt die Authorizierung durch und speichert den Token für die spätere Verwendung
        /// </summary>
        /// <param name="code">Der Authorizierungscode von Microsoft</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IHttpActionResult AuthorizeOneDrive([FromUri] string code)
        {
            var url = RedirectUrl;

            try
            {
                var connector = new Connector.OneDrive.OneDrive(DataRepository);
                var member = GetMemberFromAuthenticatedUser();

                var isAuthorized = connector.Authorize(HttpContext.Current, member.Id);

                if (isAuthorized && member.StorageAccessType == StorageProviderType.None)
                {
                    member.StorageAccessType = StorageProviderType.OneDrive;
                    DataRepository.Update(member);
                }
                connector.RefreshFolderContent();
            }

            catch (Exception ex)
            {
                Log.Error(ex);
                return BadRequest("Cannot authorize");
            }
            return Redirect(url);
        }

        private Result<Member> FindUserByToken(string data)
        {
            var result = new Result<Member>();
            try
            {
                Member member;
                if (!AuthenticatedMembers.TryGetValue(data, out member))
                {
                    result.Status.Code = Controller.StatusCode.NoData;
                    result.Status.Message = ResultMessages.NoMatchingData;
                }
                else
                {
                    result.Data = AuthenticatedMembers[data];
                }
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return result;
        }

        #region Helpers

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private string ValidateClientAndRedirectUri(ref string redirectUriOutput)
        {
            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            var validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            redirectUriOutput = redirectUri.AbsoluteUri;

            var token = GetQueryString(Request, "token");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "token is required";
            }

            return token;
        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null)
            {
                return null;
            }

            var match =
                queryStrings.FirstOrDefault(
                    keyValue => String.Compare(keyValue.Key, key, StringComparison.OrdinalIgnoreCase) == 0);

            if (string.IsNullOrEmpty(match.Value))
            {
                return null;
            }

            return match.Value;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            string verifyTokenEndPoint;

            if (provider.ToLower() == "facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                var appToken = "368446986668931|ayca71CIxnRnK64m2h0ZYeEQCX8";
                verifyTokenEndPoint =
                    string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken,
                        appToken);
            }
            else
            {
                parsedToken = new ParsedExternalAccessToken();
                var identity = User.Identity as ClaimsIdentity;
                var login = ExternalLoginData.FromIdentity(identity);
                parsedToken.UserId = login.ProviderKey;
                return parsedToken;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.UserId = jObj["data"]["user_id"];
                    parsedToken.AppId = jObj["data"]["app_id"];
                }
                else if (provider == "Google")
                {
                    parsedToken.UserId = jObj["user_id"];
                    parsedToken.AppId = jObj["audience"];
                }
            }

            return parsedToken;
        }


        /// <summary>
        ///     Entfernt einen Benutzer ohne Daten beim Cloud-Provider zu löschen
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public Result<bool> Remove()
        {
            var result = new Result<bool> { Data = true };
            try
            {
                var currentUser = GetUser();
                var member = DataRepository.Queryable<Member>().FirstOrDefault(m => m.AspNetUserId == currentUser.Id);
                var userDeleteResult = AuthenticationRepository.Delete(currentUser);
                if (!userDeleteResult.Succeeded)
                {
                    result.Data = false;
                    result.Status.Code = Controller.StatusCode.Failure;
                    result.Status.Message = ResultMessages.Failure;
                }
                else
                {
                    DataRepository.DeleteMember(member);
                    SignOut();
                }
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return result;
        }

        #endregion

        #endregion
    }
}