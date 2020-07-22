using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Http;
using Fotosteam.Service.Connector.Dropbox;
using Fotosteam.Service.Hub;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Newtonsoft.Json;

namespace Fotosteam.Service.Controller
{
    /// <summary>
    ///     Definiert die Rest-API, die für die Synchronisation der Dropboxk zuständig ist
    ///     Wird über api/synch aufgerufen (Das wird jedoch im Startup von Owin definiert und hier wird der Standardwert
    ///     angenommen.
    /// </summary>
    /// <remarks>Dank an Jerrie Pelser (http://www.jerriepelser.com/blog/creating-a-dropbox-webhook-in-aspnet)</remarks>
    public class SynchController : ControllerBase
    {
        /// <summary>
        /// Standardkonstruktor
        /// </summary>
        public SynchController()
        {
            DataRepository = new DataRepository();
        }

        /// <summary>
        /// Konstruktor der das Festelegen eines Repositories ermöglicht
        /// </summary>
        /// <param name="repository"></param>
        public SynchController(IDataRepository repository)
        {
            DataRepository = repository;
        }

        /// <summary>
        ///     Dient lediglich zur Überprüfung des Webhooks
        /// </summary>
        /// <param name="challenge">Die Frage von Dropbox</param>
        /// <returns>Liefert die Frage zurück</returns>
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage Notify([FromUri] string challenge)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(challenge)

            };
        }

        internal string GetSha256Hash(HMACSHA256 sha256Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            var data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            var stringBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            foreach (var t in data)
            {
                stringBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string. 
            return stringBuilder.ToString();
        }

  

        /// <summary>
        ///     Reagiert auf den Webhook der Dropbox, um dann eine Aktion durchzuführen
        /// </summary>
        /// <returns>Der aynchrone Task für das Ergebnis des Webhook </returns>
        [HttpPost]
        public IHttpActionResult Notify()
        {
            try
            {
                // Get the request signature
                var signature = Request.Headers.GetValues("X-Dropbox-Signature").FirstOrDefault();
                if (signature == null)
                {
                    lock (_lock)
                    {
                        Log.Error("Notify:Header incorrect");
                    }
                    return BadRequest("Header incorrect");
                }

                // Extract the raw body of the request
                var body = Request.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<dynamic>(body);

                foreach (string userId in result.delta.users)
                {
                    if (WebHookSession.Instance.IsUserInSession(userId))
                    {
                        lock (_lock)
                        {
                            Log.Info(string.Format("User {0} has already an active session", userId));
                        }
                        continue;
                    }
                    WebHookSession.Instance.AddUserToSession(userId);
                    //Der ThreadPool erzeugt eine eigene Instant der WebHookSession,
                    //deshalb übergeben wir diese von diesem Thread aus, der im ApplicationPool des IIS läuft
                    //und somit für alle Benutzer gültig sein sollte
                    ThreadPool.QueueUserWorkItem(StartSynch, new {UserId = userId, Session = WebHookSession.Instance});
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return Ok();
        }


        internal static bool IsSynchInProgress(string userId)
        {
            return WebHookSession.Instance.IsUserInSession(userId);
        }

        private static object _lock = new object();
        internal static void StartSynch(dynamic stateInfo)
        {
            string userId = stateInfo.UserId.ToString();

            lock (_lock)
            {
                try
                {
                    if (DropboxConnector.IsSynchInProgressForUser(userId))
                        return;

                    var repository = new DataRepository();

                    var memberId =
                        repository.Queryable<MemberStorageAccess>()
                            .Where(m => m.UserId == userId)
                            .Select(m => m.MemberId)
                            .SingleOrDefault();

                    if (memberId == Constants.NotSetId)
                    {
                        Log.Error("Member not found for user" + userId);
                        return;
                    }

                    var connector = new DropboxConnector(repository);
                    var member = repository.Queryable<Member>().Include(m => m.StorageAccesses)
                        .Include(m => m.Options).FirstOrDefault(m => m.Id == memberId);

                    if (member == null || !member.Options.UseDropboxWebhook)
                    {
                        Log.Info("Member not found or Webhook not enabled");
                        return;
                    }

                    var access = member.StorageAccesses.FirstOrDefault(s => s.Type == StorageProviderType.Dropbox);
                    connector.CurrentMember = member;
                    connector.Connect(access);

                    Log.Info("Deleting missing photos for " + member.Id);
                    connector.DeleteNotExistingPhotos();

                    var count = connector.GetFileList().Entries.Count;

                    if (count > 0)
                    {
                        Log.Info(count + " files to synch to begin with");
                        var info = new Notification()
                        {
                            Type = NotificationType.DropboxSynchronization,
                            MemberId = member.Id,
                            Date = DateTime.Now,
                            Data = new SynchProgress() { Index = 0, TotalFileCount = count }
                        };

                        if (count > 0)
                            NotificationHub.PushNotification(info);

                        while (count > 0)
                        {
                            SynchFiles(member, connector, repository);
                            count = connector.GetFileList().Entries.Count;
                            Log.Info(count + " files to synch left");
                        }

                        var notification = new Notification()
                        {
                            Type = NotificationType.DropboxSynchronization,
                            MemberId = member.Id,
                            UserAlias = "fotosteam"
                        };
                        repository.Add(notification);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
                finally
                {
                    lock (_lock)
                    {
                        stateInfo.Session.RemoveUserFromSession(userId);
                    }
                }
            }
        }

        private static void SynchFiles(Member member, DropboxConnector connector, DataRepository repository)
        {
            var info = new Notification()
            {
                Type = NotificationType.DropboxSynchronization,
                MemberId = member.Id,
            };

            foreach (var currentStep in connector.RefreshFolderContent())
            {
                info.Date = DateTime.Now;
                info.Data = currentStep;
                if (currentStep.Photo != null)
                {
                    info.PhotoTitle = currentStep.Photo.Name;
                    NotificationHub.PushNotification(info);
                }
                if (currentStep.Photo != null)
                    SavePhotosToDatabase(repository, currentStep.Photo);
            }
        }

        private static void SavePhotosToDatabase(IDataRepository repository, Photo photo)
        {
            if (string.IsNullOrEmpty(photo.Title))
            {
                photo.Title = "Untitled";
            }
            if (photo.Location != null && photo.Location.Id != Constants.NotSetId)
            {
                repository.Update(photo.Location);
            }
            if (photo.Id == Constants.NotSetId)
            {
                repository.Add(photo);
                            }
            else
            {
                repository.Update(photo);
            }
        }
    }
}