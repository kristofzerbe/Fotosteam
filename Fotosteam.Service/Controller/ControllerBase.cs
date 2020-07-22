using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using Fotosteam.Service.Connector;
using Fotosteam.Service.Connector.Dropbox;
using Fotosteam.Service.Connector.GoogleDrive;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Constants = Fotosteam.Service.Models.Constants;

namespace Fotosteam.Service.Controller
{
    /// <summary>
    /// Definiert die gemeinsame Funktion für alle Controller
    /// </summary>
    public class ControllerBase : ApiController
    {
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected internal IDataRepository DataRepository { get; set; }
        protected internal IAuthRepository AuthenticationRepository { get; set; }

        /// <summary>
        /// Schreibt den Fehler in die Log-Datei und setzt der Result-Objekt auf einen Fehler
        /// </summary>
        /// <typeparam name="T">Der Typ, der im Resukt enthalten ist</typeparam>
        /// <param name="ex">Das Fehlerobjekt</param>
        /// <param name="result">Das Result-Objekt, das angepasst werden soll</param>
        /// <param name="sendEmail">Gibt an, ob eine Email versandt werden soll</param>
        protected void LogExceptionAndSetResult<T>(Exception ex, Result<T> result, bool sendEmail = true)
        {
            Log.Error(ex);
            result.Status.Code = Controller.StatusCode.InternalException;
            result.Status.Message = ResultMessages.UnhandledException;
            var comController = new CommunicationController(DataRepository, AuthenticationRepository);

            if (sendEmail)
                comController.Trello(new Message { Title = "Auto exception", SenderName = "fotosteam", SenderEmail = "info@fotosteam.com", Body = FormatException(ex) });
        }

        private string FormatException(Exception ex)
        {
            var body = new StringBuilder();
            body.Append(ex.Message);
            var innerException = ex.InnerException;
            while (innerException != null)
            {
                body.AppendLine();
                body.Append(innerException.Message);
                innerException = innerException.InnerException;
            }
            body.AppendLine();
            body.Append(ex.StackTrace);
            return body.ToString();
        }

        protected Result<string> LogExceptionAndCreateEmptyResult(Exception ex)
        {
            var result = new Result<string>();
            LogExceptionAndSetResult(ex, result);
            return result;
        }

        protected static Result<T> SetNotAuthorizedResult<T>(Result<T> result)
        {
            result.Status.Code = Controller.StatusCode.NotAuthorized;
            result.Status.Message = ResultMessages.NotAuthorized;
            return result;
        }

        internal bool DoesAliasMatchAuthenticatedUser(string alias)
        {
            var member = GetMemberFromAuthenticatedUser();
            return member != null && member.Alias.ToLower().Equals(alias.ToLower());
        }


        internal Member GetMemberFromAuthenticatedUser(bool includeDetails = false)
        {
            var id = GetUserIdForAuthenticatedUser();
            
            var loginData = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (string.IsNullOrEmpty(id))
                return new Member
                       {
                           Alias = Constants.NotSet, 
                           PlainName = User.Identity.Name,
                           ProviderName = (loginData != null) ? loginData.LoginProvider : "",
                           ProviderUserName = (loginData != null) ? loginData.UserName : "",
                           ProviderUserMail = (loginData != null) ? loginData.EmailAddress : ""
                       };

            var query = DataRepository.Queryable<Member>();
            if (includeDetails)
            {
                query = query.Include(m => m.HomeLocation)
                    .Include(m => m.SocialMedias)
                    .Include(m => m.Buddies)
                    .Include(m => m.Options);
            }

            var result = query.FirstOrDefault(m => m.AspNetUserId == id);
            if (result == null)
            {
                Log.Info("No data found for User with Id" + id);
                result = new Member { Alias = Constants.NotSet, PlainName = User.Identity.Name};
            } 
            result.ProviderName = loginData.LoginProvider;
            result.ProviderUserName = loginData.UserName;
            result.ProviderUserMail = loginData.EmailAddress;
            
            if (includeDetails)
            {
                var buddies = DataRepository.Queryable<Buddy>().Where(b => b.BuddyMemberId == result.Id).ToList();
                if (buddies.Any())
                {
                    foreach (var buddy in buddies)
                    {
                        result.Buddies.Add(buddy);
                    }
                }
                if(result.Options == null)
                    result.Options = new MemberOption() { MemberId = result.Id };
            }
            return result;
        }

        internal int GetMemberId()
        {
            var id = GetUserIdForAuthenticatedUser();
            if (string.IsNullOrEmpty(id))
                return Constants.NotSetId ;
            return DataRepository.Queryable<Member>().Where(m => m.AspNetUserId == id).Select(m => m.Id).SingleOrDefault(
                );
        }

        static internal string GetUrlRoot()
        {
            return string.Format("{0}://{1}/", 
                HttpContext.Current.Request.Url.Scheme, 
                HttpContext.Current.Request.Url.Authority);
        }

        protected internal static readonly Dictionary<string, IdentityUser> AuthorizedUsers =
new Dictionary<string, IdentityUser>();

        protected internal string RedirectUrl
        {
            get
            {
                return
                    string.Format("{0}://{1}/AuthenticationReady.html", Request.RequestUri.Scheme,
                        Request.RequestUri.Authority);
            }
        }
        internal string GetUserIdForAuthenticatedUser()
        {
            var claimIdentity = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (claimIdentity == null)
            {
                return string.Empty;
            }

            var user = GetUser();

            if (!AuthorizedUsers.ContainsKey(claimIdentity.ProviderKey) && user != null)
            {
                AuthorizedUsers.Add(claimIdentity.ProviderKey, user);
            }
            return user == null ? string.Empty : user.Id;
        }
        protected internal IdentityUser GetUser()
        {
            var claimIdentity = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (claimIdentity == null)
            {
                return null;
            }

            var user = AuthenticationRepository.Find(new UserLoginInfo(claimIdentity.LoginProvider,
                    claimIdentity.ProviderKey));

            return user;
        }

        protected Stream GetImageStreamFromPost()
        {
            Request.Content.LoadIntoBufferAsync().Wait();
            Stream imageStream = null;
            Request.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider()).ContinueWith(task =>
            {
                var provider = task.Result;
                foreach (var content in provider.Contents)
                {
                    imageStream = content.ReadAsStreamAsync().Result;
                    return imageStream;
                }
                return null;
            }).Wait();

            return imageStream;
        }

        internal IConnector CurrentConnector(Member member)
        {
            if (member == null || member.StorageAccesses.Count == 0)
            {
                return null;
            }

            IConnector connector;
            switch (member.StorageAccessType)
            {
                case StorageProviderType.Dropbox:
                    connector = new DropboxConnector(DataRepository);
                    break;
                case StorageProviderType.GoogleDrive:
                    connector = new GoogleDrive(DataRepository);
                    break;
                case StorageProviderType.OneDrive:
                    connector = new Connector.OneDrive.OneDrive(DataRepository);
                    break;
                case StorageProviderType.LocalDrive:
                    connector = new Connector.LocalDrive.LocalDrive(DataRepository, string.Format("{0}://{1}", Request.RequestUri.Scheme, Request.RequestUri.Authority), member.Alias);
                    break;
                default:
                    throw new InvalidEnumArgumentException(string.Format("Der Connector {0} ist unbekannt", member.StorageAccessType));
            }
            var access = member.StorageAccesses.First(s=>s.Type == member.StorageAccessType );
            connector.RedirectUrl = RedirectUrl;
            connector.CurrentMember = member;
            connector.Connect(access);

            return connector;
        }

        protected Member GetMemberWithStorageAccesses()
        {
            var id = GetMemberFromAuthenticatedUser().AspNetUserId;
            var member = DataRepository.Queryable<Member>()
                .Include(x => x.StorageAccesses)
                .Include(x => x.Options )
                .FirstOrDefault(x => x.AspNetUserId == id);

            if (member != null && member.Options == null)
            {
                member.Options = new MemberOption() {MemberId = member.Id};
            }

            return member;
        }

        internal protected bool IsUserAuthorized
        {
            get
            {
                if (User == null || User.Identity == null || !User.Identity.IsAuthenticated)
                {
                    return false;
                }

                return AuthorizeController.IsAuthorized(User.Identity);
            }
        }

        internal protected void ClearSensitiveData(Member member)
        {
            if (member == null)
                return;

            member.StorageAccesses = null;
            if (member.Options == null || !member.Options.DisplayEmailAddress)
                member.Email = null;

            member.ProviderKey = null;
            member.AspNetUserId = null;

            if (member.HomeLocation != null)
            {
                member.HomeLocation.Street = string.Empty;
                member.HomeLocation.County = string.Empty;
            }
        }
    }
}