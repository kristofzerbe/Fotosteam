using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Fotosteam.Service.Connector;
using Fotosteam.Service.Hub;
using Fotosteam.Service.Imaging;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Newtonsoft.Json;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Fotosteam.Service.Controller
{
    /// <summary>
    ///     Definiert die Rest-API, die für die Authentifizierung zuständig ist
    ///     Wird über api/authorisation aufgerufen (Das wird jedoch im Startup von Owin definiert und hier wird der
    ///     Standardwert angenommen.
    /// </summary>
    /// <remarks>
    ///     Paramenternamen richten sich nach der Definition im <see cref="WebApiConfig" />.
    ///     Aus diesem Grund taucht der Parameter data häufig auf, auch wenn ein anderer Name passender wäre
    /// </remarks>
    [RoutePrefix("api/Account")]
    public class AccountController : ControllerBase
    {

        /// <summary>
        ///     Überladener Konstruktor, der die Überbabe eines Repositories erlaubt
        /// </summary>
        /// <param name="dataRepository">Repositiory-Objekt zur Interaktion mit persistierbaren Daten</param>
        public AccountController(IDataRepository dataRepository)
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
        public AccountController(IDataRepository dataRepository, IAuthRepository authRepository)
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
        public AccountController()
        {
            AuthenticationRepository = new AuthRepository();
            DataRepository = new DataRepository();
        }


        /// <summary>
        ///     Liefert die Benutzerinformationen zurück
        /// </summary>
        /// <returns>Ein <see cref="Result{T}" /> mit Member-Objekt</returns>
        [HttpGet]
        public Result<Member> MemberInfo()
        {
            var result = new Result<Member>();
            try
            {
                if (User == null || !User.Identity.IsAuthenticated)
                {
                    result.Status.Code = Controller.StatusCode.NotAuthorized;
                    result.Status.Message = ResultMessages.NotAuthorized;
                    return result;
                }
                result.Data = GetMemberFromAuthenticatedUser(true);
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return result;
        }

        /// <summary>
        ///     Liefert ein Member-Objekt für den gewünschte Alias zurück
        /// </summary>
        /// <param name="data">Der Alias des Benutzers</param>
        /// <returns>Ein Member-Objekt ohne Zugriffsdaten</returns>
        [HttpGet]
        public Result<Member> Member(string data)
        {
            return GetMember(data, Constants.NotSetId);
        }

        private Result<Member> GetMember(string alias, int id)
        {
            var result = new Result<Member>();
            try
            {
                Member member;
                if (!string.IsNullOrEmpty(alias))
                    member = DataRepository.GetMemberByAlias(alias, true);
                else
                    member = DataRepository.GetMemberById(id, true);

                if (member != null)
                {
                    ClearSensitiveData(member);
                }
                result.Data = member;
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return result;
        }

        /// <summary>
        ///     Liefert ein Member-Objekt für die gewünschte ID zurück
        /// </summary>
        /// <param name="data">Die ID des Benutzers</param>
        /// <returns>Ein Member-Objekt ohne Zugriffsdaten</returns>
        [HttpGet]
        public Result<Member> MemberById(int data)
        {
            return GetMember(string.Empty, data);

        }

        /// <summary>
        ///     Liefert eine Liste von Members anhand der Buddies des übergebenen Benutzers zurück
        /// </summary>
        /// <param name="data">Alias oder Id des Benutzer</param>
        /// <returns>Liste von Members</returns>
        [HttpGet]
        public Result<List<Member>> Buddies(string data)
        {
            var result = new Result<List<Member>>();
            if (data.ToLower() == Constants.NotSet.ToLower())
                return result;

            try
            {
                int id;
                result.Data = int.TryParse(data, out id)
                    ? DataRepository.GetBuddiesById(id)
                    : DataRepository.GetBuddiesByAlias(data);
            }
            catch (NullReferenceException)
            {
                result.Data = null;
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            if (result.Data == null || !result.Data.Any())
            {
                result.Status.Code = Controller.StatusCode.NoData;
                result.Status.Message = ResultMessages.NoMatchingData;
            }
            else
            {
                foreach (var member in result.Data)
                {
                    ClearSensitiveData(member);
                }
            }
            return result;
        }

        /// <summary>
        /// Liefert die Liste der Invitecodes für den angemeldeten Benutzer zurück
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Obsolete("Der Einladungscode wird nicht mehr genutzt")]
        public Result<List<string>> InviteCodes()
        {
            return new Result<List<string>>();
        }

        
        /// <summary>
        ///     Liefert zurück, ob ein InviteCode verfügbar ist und noch nicht verwendet wurde
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Bool</returns>
        /// <remarks>http://encosia.com/using-jquery-to-post-frombody-parameters-to-web-api/</remarks>
        [HttpPost]
        [AllowAnonymous]
        [Obsolete("Der Einladungscode wird nicht mehr verwendet")]
        public bool IsInviteCodeAvailable([FromBody] string data)
        {
            return true;
        }

        /// <summary>
        ///     Überprüft, ob ein Alias schon vergeben ist
        /// </summary>
        /// <param name="data">Der zu überprüfende Alias</param>
        /// <returns>Bool</returns>
        /// <remarks>http://encosia.com/using-jquery-to-post-frombody-parameters-to-web-api/</remarks>
        [HttpPost]
        [AllowAnonymous]
        public bool IsAliasAvailable([FromBody] string data)
        {
            try
            {
                var isAvailable =
                    !DataRepository.Queryable<Member>()
                        .Any(m => m.Alias.ToLower().Equals(data.ToLower()));
                return isAvailable;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        /// <summary>
        ///     Ermöglicht die Aktualisierung des Wohnorts, Name, Motto und Beschreibung eines Benutzer
        /// </summary>
        /// <param name="currentMember"></param>
        /// <returns>Ein Memberobjekt</returns>
        /// <remarks>Der Name kann nicht als Leer gesetzt werden. Wird er nicht mitgeliefert, bleibt der alte Name bestehen</remarks>
        [HttpPut]
        [Authorize]
        public Result<Member> Member(Member currentMember)
        {
            var result = new Result<Member>();
            Member member = null;
            try
            {
                member = GetMemberFromAuthenticatedUser();
                if (currentMember.Id == 0 || member.Id != currentMember.Id)
                {
                    result.Status.Code = Controller.StatusCode.NotAuthorized;
                    result.Status.Message = ResultMessages.NotAuthorized;
                }
                if (member.HomeLocation != null && currentMember.HomeLocation != null)
                {
                    member.HomeLocation.Longitude = currentMember.HomeLocation.Longitude;
                    member.HomeLocation.Latitude = currentMember.HomeLocation.Latitude;
                }
                if (member.HomeLocation == null && currentMember.HomeLocation != null)
                {
                    member.HomeLocation = currentMember.HomeLocation;
                }

                if (currentMember.HomeLocation == null)
                {
                    if (member.HomeLocation != null)
                    {
                        DataRepository.Delete(member.HomeLocation);
                    }
                    member.HomeLocation = null;
                }
                else
                {
                    // ReSharper disable once PossibleNullReferenceException
                    member.HomeLocation.MemberId = member.Id;
                    ExtendHomeLocation(member.HomeLocation);

                    if (member.HomeLocation.Id == Constants.NotSetId)
                    {
                        DataRepository.Add(member.HomeLocation);
                    }
                    else
                    {
                        DataRepository.Update(member.HomeLocation);
                    }
                }
                DataRepository.Update(member);
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }

            result.Data = member;
            return result;
        }

        private static void ExtendHomeLocation(Location homeLocation)
        {
            if (homeLocation == null)
                return;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (homeLocation.Longitude == 0 || homeLocation.Latitude == 0)
                return;

            var location = Processing.SetLocationDetails(homeLocation.Latitude, homeLocation.Longitude);

            homeLocation.Name = location.Name;
            homeLocation.City = location.City;
            homeLocation.Country = location.Country;
            homeLocation.CountryIsoCode = location.CountryIsoCode;
            homeLocation.County = location.County;
        }



        /// <summary>
        ///     Liefert eine Liste alle aller Mitglieder zurück
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Result<List<Member>> Members()
        {
            var result = new Result<List<Member>>();
            try
            {
                result.Data = DataRepository.Queryable<Member>()
                    .Include(m => m.HomeLocation)
                    .Include(m => m.Buddies)
                    .Include(m => m.SocialMedias).Where(m=>m.Photos.Any()).ToList();

                foreach (var member in result.Data)
                {
                    ClearSensitiveData(member);
                }
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return result;
        }

        /// <summary>
        ///     Liefert eine zufällige Liste von Mitgliedern
        /// </summary>
        /// <param name="data">Anzahl der Mitglieder</param>
        /// <returns></returns>
        [HttpGet]
        public Result<List<Member>> MembersRandom(int data)
        {
            var result = new Result<List<Member>>();
            try
            {
                result.Data = DataRepository.Queryable<Member>().Where(x => x.Avatar200Url != null && x.Photos.Any())
                    .OrderBy(m => Guid.NewGuid())
                    .Take(data)
                    .ToList();

                foreach (var member in result.Data)
                {
                    ClearSensitiveData(member);
                }
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return result;
        }

        /// <summary>
        /// Dies Methode hält die Kommuniktion mit dem Client aufrecht, um den Synchronisierungsfortschritt
        /// pro Foto in den Ausgabefluss zu schreiben
        /// </summary>
        /// <param name="stream">Der Stream mit dem Client</param>
        /// <param name="content">Der Inhalt des Streams. Wird nicht verwendet, ist aber für den Aufruf erfordern</param>
        /// <param name="context">Der Context der Verbindung. Wird nicht verwendet, ist aber für den Aufruf erfordern</param>
        /// <remarks>Es ist nicht sichergestellt, dass die Verbindung offen bleibt. Außerdem ist unklar wie die Browser auf den
        /// Flush reaagieren, so dass das Parsen der einzelnen Fortschirttinformatioen <see cref="SynchProgress"/> nicht klar ist</remarks>
        public void OnStreamAvailable(Stream stream, HttpContent content, TransportContext context)
        {
            try
            {
                var serializer = new JsonSerializer();
                var memberId = GetMemberId();
                bool streamIsOpen = true;
                foreach (var currentStep in _connector.RefreshFolderContent())
                {
                    try
                    {
                        NotificationHub.PushNotification(new Notification()
                        {
                            Data = currentStep,
                            Type = NotificationType.PhotoSynch,
                            Date = DateTime.Now,
                            MemberId = memberId,
                            PhotoName = currentStep.Photo == null ? string.Empty : currentStep.Photo.Name,
                            PhotoTitle  = currentStep.Photo == null ? string.Empty : currentStep.Photo.Title 
                        });

                        if (currentStep.Photo != null)
                        {
                            SavePhotosToDatabase(currentStep.Photo);
                        }

                        if (!streamIsOpen) continue;

                        using (var writer = new StreamWriter(stream))
                        {
                            if (currentStep.Photo != null)
                            {
                                dynamic currentPhoto = new
                                {
                                    currentStep.Photo.Id,
                                    currentStep.Photo.Title,
                                    currentStep.Photo.Name,
                                    currentStep.Photo.OriginalName,
                                    currentStep.Photo.MemberId,
                                    currentStep.Photo.IsPrivate,
                                    DirectLinks = currentStep.Photo.DirectLinks.Where(l => l.Size <= 400).ToList(),
                                    currentStep.Photo.Color
                                };
                                dynamic progress = new
                                {
                                    currentStep.TotalFileCount,
                                    currentStep.Index,
                                    Photo = currentPhoto
                                };
                                var returnValue = new Result<object>() { Data = progress };
                                serializer.Serialize(writer, returnValue);
                            }
                            else
                            {
                                var returnValue = new Result<SynchProgress> { Data = currentStep };
                                serializer.Serialize(writer, returnValue);
                            }
                            stream.Flush();
                        }
                    }

                    catch (HttpException ex)
                    {
                        // Die Verbindung zum Client wurde unterbrochen, die Synchronisierung läuft jedoch weiter
                        if (ex.ErrorCode == -2147023667)
                        {
                            streamIsOpen = false;
                        }
                    }
                }
                var notification = new Notification()
                {
                    Type = NotificationType.PhotoSynch,
                    MemberId = memberId,
                    UserAlias = "fotosteam"
                };
                DataRepository.Add(notification);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                var error = new Result<SynchProgress> { Data = new SynchProgress() };
                error.Status.Code = Controller.StatusCode.Failure;
                error.Status.Message = ResultMessages.UnhandledException;
                try
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(writer, error);

                    }
                }
                catch (Exception ex1)
                {
                    //Da können wir jetzt auch nicht mehr machen.
                    Log.Error(ex1);
                }

            }
            finally
            {
                // Close output stream as we are done
                stream.Close();
            }
        }

        private IConnector _connector;
        /// <summary>
        ///     Aktualisiert den Inhalt aus der Dropbbox
        /// </summary>
        /// <returns>Result mit der Liste der neuen Fotos</returns>
        [Authorize]
        [HttpGet]
        public HttpResponseMessage RefreshUserContent()
        {
            var result = new Result<SynchProgress>();
            try
            {
                var member = GetMemberWithStorageAccesses();
                if (member.StorageAccessType == StorageProviderType.Dropbox)
                {
                    var access = member.StorageAccesses.FirstOrDefault(s => s.Type == StorageProviderType.Dropbox);
                    if (access != null)
                    {
                        if (SynchController.IsSynchInProgress(access.UserId))
                        {
                            result.Status.Code = Controller.StatusCode.SynchProcessIsInProgress;
                            result.Data = new SynchProgress();
                            return new HttpResponseMessage()
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(result)),
                                StatusCode = HttpStatusCode.OK
                            };
                        }
                    }
                }

                _connector = CurrentConnector(member);
                if (_connector == null)
                {
                    result.Status.Code = Controller.StatusCode.NoStorageAccessDefined;
                    result.Status.Message = ResultMessages.NoDataAccess;
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(result)),
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
                _connector.IsSynchInProgress = true;
                var response = Request.CreateResponse();
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Cache-Control", "no-cache, must-revalidate");
                response.Content = new PushStreamContent(new Action<Stream, HttpContent, TransportContext>(OnStreamAvailable), "application/json");
                return response;

            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(result)),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            finally
            {
                if (_connector != null)
                    _connector.IsSynchInProgress = false;
            }

        }

        private void SavePhotosToDatabase(Photo photo)
        {
            if (string.IsNullOrEmpty(photo.Title))
            {
                photo.Title = "Untitled";
            }
            if (photo.Location != null && photo.Location.Id != Constants.NotSetId)
            {
                DataRepository.Update(photo.Location);
            }
            if (photo.Id == Constants.NotSetId)
            {
                DataRepository.Add(photo);
                
            }
            else
            {
                DataRepository.Update(photo);                
            }
        }

        /// <summary>
        ///     Aktualisiert das Avatarbild
        /// </summary>
        /// <returns>Ein aktualisiertes Benutzerobjekt mit den enstrepchenden URLs</returns>
        [Authorize]
        [HttpPost]
        public Result<Member> AvatarImage()
        {
            return UploadImageForMember(false);
        }

        /// <summary>
        /// Erlaubt das Hochladen eines Kopfbildes für den Benutzer
        /// </summary>
        /// <returns>Ein aktualisiertes Benutzerobjekt mit den enstrepchenden URLs</returns>
        [Authorize]
        [HttpPost]
        public Result<Member> Header()
        {
            return UploadImageForMember(true);
        }

        private Result<Member> UploadImageForMember(bool isHeader)
        {
            var result = new Result<Member>();
            try
            {
                var member = GetMemberWithStorageAccesses();
                var connector = CurrentConnector(member);
                if (connector == null)
                {
                    result.Status.Code = Controller.StatusCode.NoStorageAccessDefined;
                    result.Status.Message = ResultMessages.NoDataAccess;
                    return result;
                }

                if (Request.Content.IsMimeMultipartContent())
                {
                    var stream = GetImageStreamFromPost();
                    if (isHeader)
                    {
                        UploadHeader(member, connector, stream);
                    }
                    else
                    {
                        UploadAvatar(member, connector, stream);
                    }

                    DataRepository.Update(member);

                    //kristof: Avatar100Url in Comments und Ratings aktualisieren
                    DataRepository.UpdateAvatarInAllRelations(member.Alias, member.Avatar100Url);

                    ClearSensitiveData(member);
                    result.Data = member;
                }
                else
                {
                    var id = Request.Content.ReadAsAsync<int>().Result;
                    return Header(id, member, connector);
                }
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return result;
        }

        private void UploadAvatar(Member member, IConnector connector, Stream stream)
        {
            var info = connector.UploadAvatarImage(stream);
            member.Avatar100Url = info.Avatar100Url;
            member.Avatar200Url = info.Avatar200Url;
            member.AvatarColor = info.AvatarColor;
        }

        private void UploadHeader(Member member, IConnector connector, Stream stream)
        {
            var info = connector.UploadHeaderImage(stream);
            member.Header640Url = info.Header640Url;
            member.Header1024Url = info.Header1024Url;
            member.Header1440Url = info.Header1440Url;
            member.Header1920Url = info.Header1920Url;
            member.HeaderColor = info.HeaderColor;
        }

        private Result<Member> Header(int photoId, Member member, IConnector connector)
        {
            var result = new Result<Member>();
            try
            {
                if (connector == null)
                {
                    result.Status.Code = Controller.StatusCode.NoStorageAccessDefined;
                    result.Status.Message = ResultMessages.NoDataAccess;
                    return result;
                }
                var link =
                    DataRepository.Queryable<Photo>()
                        .Include(p => p.DirectLinks)
                        .Where(p => p.MemberId == member.Id && p.Id == photoId)
                        .Select(p => p.DirectLinks.FirstOrDefault(l => l.Size == 0))
                        .FirstOrDefault();
                if (link == null || string.IsNullOrEmpty(link.Url))
                {
                    result.Status.Code = Controller.StatusCode.NoData;
                    result.Status.Message = ResultMessages.NoMatchingData;
                    return result;
                }

                var client = new HttpClient();
                var uri = new Uri(link.Url);
                var response = client.GetAsync(uri).Result;
                var stream = response.Content.ReadAsStreamAsync().Result;
                UploadHeader(member, connector, stream);
                result.Data = member;
            }
            catch (Exception ex)
            {
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;
                Log.Error(ex);
            }
            return result;
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AuthenticationRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}