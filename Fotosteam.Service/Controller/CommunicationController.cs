using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Web.Http;
using Fotosteam.Service.Hub;
using Fotosteam.Service.Models;
using Fotosteam.Service.Properties;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Newtonsoft.Json;

namespace Fotosteam.Service.Controller
{
    /// <summary>
    /// Der Kontroller ermöglicht die Kommunikation mit Fotosteam, Trello und einem Mitglied
    /// </summary>
    public class CommunicationController : ControllerBase
    {
        private readonly string _trelloEmailAddress = ConfigurationManager.AppSettings["NotificationTrelloRecipient"];
        private readonly string _fotoSteamAddress = ConfigurationManager.AppSettings["NotificationFotoSteamRecipient"];

        /// <summary>
        /// Standardkonstruktor
        /// </summary>
        public CommunicationController()
        {
            DataRepository = new DataRepository();
            AuthenticationRepository = new AuthRepository();
        }

        /// <summary>
        /// Initialisiert die Klasse mit spezifischen Repositories 
        /// </summary>
        /// <param name="repository">Das Datenrepository</param>
        /// <param name="authRepository">Das Authentifizierungsrepository</param>
        public CommunicationController(IDataRepository repository, IAuthRepository authRepository)
        {
            DataRepository = repository;
            AuthenticationRepository = authRepository;
        }

        /// <summary>
        /// Fügt einen neuen Kommentar zu ein Bild hinzu.
        /// Dabei wird überprüft, ob der Besitzer Kommatare zu läßt und Kommentare für das Foto zugelasssen sind
        /// </summary>
        /// <param name="newComment">Das Objekt mit dem Komentar</param>
        /// <returns>Ein Result-Objekt mit dem gespeicherten Kommantar</returns>
        /// <remarks>Es gibt noch keine Überprüfung für Kommentare zu Kommentare. Das ist noch nicht getestet</remarks>
        [HttpPost]
        [ActionName("AddComment")]
        [AllowAnonymous]
        public Result<Comment> AddComment([FromBody]Comment newComment)
        {
            var result = new Result<Comment>();
            try
            {
                if (string.IsNullOrEmpty(newComment.Text) || newComment.Text.Length == 1)
                {
                    result.Status.Code = Controller.StatusCode.NoData;
                    return result;
                }

                var photoInfo = GetPhotoInfoFromId(newComment.PhotoId);

                var allowCommentOnPhoto =
                    DataRepository.Queryable<Photo>()
                        .Where(p => p.Id == newComment.PhotoId)
                        .Select(p => p.AllowCommenting)
                        .SingleOrDefault();

                if (!photoInfo.Member.Options.AllowComments || !allowCommentOnPhoto)
                {
                    result.Status.Code = Controller.StatusCode.NotAuthorized;
                    result.Status.Message = ResultMessages.NotAuthorized;
                    return result;
                }

                if (string.IsNullOrEmpty(newComment.UserAlias))
                    newComment.UserAlias = newComment.UserName;
                result.Data = DataRepository.AddComment(newComment);
                var authenticateUser = GetMemberFromAuthenticatedUser();

                if (authenticateUser != null && !photoInfo.Member.Id.Equals(authenticateUser.Id))
                {
                    if (photoInfo.Member.Options.DisplayNotifications)
                    {
                        var nofitication = CreateNotification(newComment, photoInfo);
                        DataRepository.Add(nofitication);
                        NotificationHub.PushNotification(nofitication);
                    }
                    if (photoInfo.Member.Options.NotifyByEmailOnComment)
                    {
                        SendNotificationEmail(newComment, photoInfo);
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                Log.Error(ex);
                result.Status.Code = Controller.StatusCode.NotValidEntity;
                result.Status.Message = ResultMessages.NotValidEntity;
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return result;
        }

        /// <summary>
        /// Fügt eine Bewertung zu einem Foto hinzu. Dabei wird überprüft ob der Benutzer Bewertungen generell
        /// und für das Fotos im speziellen zuläßt
        /// </summary>
        /// <param name="newRating">Objekt mit der Bewertung</param>
        /// <returns>Result mit dem gespeicherten Bewertungsobjekts</returns>
        [HttpPost]
        [ActionName("AddRating")]
        [AllowAnonymous]
        public Result<Rating> AddRating([FromBody] Rating newRating)
        {
            var result = new Result<Rating>();
            try
            {
                var photoInfo = GetPhotoInfoFromId(newRating.PhotoId);

                var allowRating =
                    DataRepository.Queryable<Photo>()
                        .Where(p => p.Id == newRating.PhotoId)
                        .Select(p => p.AllowRating)
                        .SingleOrDefault();

                if (!photoInfo.Member.Options.AllowRating || !allowRating)
                {
                    result.Status.Code = Controller.StatusCode.NotAuthorized;
                    result.Status.Message = ResultMessages.NotAuthorized;
                    return result;
                }

                if (string.IsNullOrEmpty(newRating.UserAlias))
                    newRating.UserAlias = newRating.UserName;

                var memberid = GetMemberId();
                if (memberid != Constants.NotSetId)
                    newRating.MemberId = memberid;

                result.Data = DataRepository.AddRating(newRating);

                if (photoInfo.Member.Options.DisplayNotifications)
                {
                    var notification = CreateNotification(newRating, photoInfo);
                    DataRepository.Add(notification);
                    NotificationHub.PushNotification(notification);
                }

                if (photoInfo.Member.Options.NotifyByEmailOnRating)
                {
                    SendNotificationEmail(newRating, photoInfo);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("PRIMARY KEY"))
                {
                    result.Status.Code = Controller.StatusCode.UserHasAlreadyRated;
                    result.Status.Message = ResultMessages.UnhandledException;
                }
                else
                {
                    LogExceptionAndSetResult(ex, result);
                }
            }
            return result;
        }

        private MinimalPhotoInfo GetPhotoInfoFromId(int photoId)
        {
            if (photoId == 0) return null;
            var info = DataRepository.GetMinimalPhotoInfo(photoId);
            return info;
        }


        private Notification CreateNotification(Comment comment, MinimalPhotoInfo photoInfo)
        {

            var notification = CreateBaseNotification(photoInfo);
            notification.Type = NotificationType.Comment;
            notification.UserAlias = comment.UserAlias;
            notification.UserAvatarLink = comment.UserAvatarLink;
            return notification;
        }
        private Notification CreateNotification(Rating rating, MinimalPhotoInfo photoInfo)
        {
            var notification = CreateBaseNotification(photoInfo);
            notification.Type = NotificationType.Rating;
            notification.UserAlias = rating.UserAlias;
            notification.UserAvatarLink = rating.UserAvatarLink;
            return notification;
        }

        private Notification CreateBaseNotification(MinimalPhotoInfo photoInfo)
        {
            var currentMember = GetMemberFromAuthenticatedUser();
            var notifcation = new Notification()
            {
                Date = DateTime.Now,
                IsUserAmember = currentMember.Id != Constants.NotSetId,
                PhotoName = photoInfo.Name,
                MemberId = photoInfo.Member.Id,
            };
            return notifcation;
        }

        private const string DoNotReplyAdddress = "noreply@fotosteam.com";
        private const string DoNotReplyName = "fotosteam service";

        private void SendNotificationEmail(Rating rating, MinimalPhotoInfo info)
        {
            var lang = Enum.GetName(typeof(MemberLanguage), info.Member.Options.Language);
            var langResource = Resources.ResourceManager.GetString("mail_" + lang + "_json");
            var dicResource = JsonConvert.DeserializeObject<Dictionary<string, string>>(langResource);

            var body = Resources.mail_template_inline;
            var content = Resources.ResourceManager.GetString("NewRating_" + lang, Resources.Culture);
            var title = dicResource["rating-title"];

            var ratingTitle = rating.Value == 1 ? dicResource["rating-value-star"] : dicResource["rating-value-stars"].Replace("%1", rating.Value.ToString());

            if (!string.IsNullOrEmpty(content))
                content = content
                    .Replace(ReplaceToken.Alias, info.Member.Alias)
                    .Replace(ReplaceToken.UserAlias, rating.UserAlias)
                    .Replace(ReplaceToken.PhotoName, info.Name)
                    .Replace(ReplaceToken.PhotoTitle, info.Title)
                    .Replace(ReplaceToken.PhotoUrl, info.Url640)
                    .Replace(ReplaceToken.Rating, ratingTitle);

            body = body
                .Replace(ReplaceToken.Title, title)
                .Replace(ReplaceToken.Body, content)
                .Replace(ReplaceToken.Salutation, dicResource["salutation"].Replace("%1", info.Member.Alias))
                .Replace(ReplaceToken.ComplimentaryClose1, dicResource["complimentary-close-1"])
                .Replace(ReplaceToken.ComplimentaryClose2, dicResource["complimentary-close-2"])
                .Replace(ReplaceToken.Disclaimer, dicResource["disclaimer"])
                .Replace(ReplaceToken.BetaDisclaimer, dicResource["beta-disclaimer"]);

            var message = new Message
            {
                Title = title,
                Body = body,
                SenderEmail = DoNotReplyAdddress,
                SenderName = DoNotReplyName
            };
            SendMessage(message, info.Member.Email);
        }

        private void SendNotificationEmail(Comment comment, MinimalPhotoInfo info)
        {
            var lang = Enum.GetName(typeof(MemberLanguage), info.Member.Options.Language);
            var langResource = Resources.ResourceManager.GetString("mail_" + lang + "_json");
            var dicResource = JsonConvert.DeserializeObject<Dictionary<string, string>>(langResource);

            var body = Resources.mail_template_inline;
            var content = Resources.ResourceManager.GetString("NewComment_" + lang, Resources.Culture);
            var title = dicResource["comment-title"];

            if (!string.IsNullOrEmpty(content))
                content = content
                    .Replace(ReplaceToken.Alias, info.Member.Alias)
                    .Replace(ReplaceToken.UserAlias, comment.UserAlias)
                    .Replace(ReplaceToken.PhotoName, info.Name)
                    .Replace(ReplaceToken.PhotoTitle, info.Title)
                    .Replace(ReplaceToken.PhotoUrl, info.Url640)
                    .Replace(ReplaceToken.Comment, comment.Text);

            body = body
                .Replace(ReplaceToken.Title, title)
                .Replace(ReplaceToken.Body, content)
                .Replace(ReplaceToken.Salutation, dicResource["salutation"].Replace("%1", info.Member.Alias))
                .Replace(ReplaceToken.ComplimentaryClose1, dicResource["complimentary-close-1"])
                .Replace(ReplaceToken.ComplimentaryClose2, dicResource["complimentary-close-2"])
                .Replace(ReplaceToken.Disclaimer, dicResource["disclaimer"])
                .Replace(ReplaceToken.BetaDisclaimer, dicResource["beta-disclaimer"]);

            var message = new Message
            {
                Title = title,
                Body = body,
                SenderEmail = DoNotReplyAdddress,
                SenderName = DoNotReplyName
            };
            SendMessage(message, info.Member.Email);
        }

        internal static void SendNewBuddyNotificationEmail(Member memberToNotify, Member newbuddy)
        {
            var lang = Enum.GetName(typeof(MemberLanguage), memberToNotify.Options.Language);
            var langResource = Resources.ResourceManager.GetString("mail_" + lang + "_json");
            var dicResource = JsonConvert.DeserializeObject<Dictionary<string, string>>(langResource);

            var body = Resources.mail_template_inline;
            var content = Resources.ResourceManager.GetString("BuddyAdd_" + lang, Resources.Culture);
            var title = dicResource["buddy-add-title"];

            if (!string.IsNullOrEmpty(content))
                content = content
                .Replace(ReplaceToken.UserAlias, newbuddy.Alias)
                .Replace(ReplaceToken.UserUrl, string.Concat(GetUrlRoot(), newbuddy.Alias, "/about"));

            body = body
                .Replace(ReplaceToken.Title, title)
                .Replace(ReplaceToken.Body, content)
                .Replace(ReplaceToken.Salutation, dicResource["salutation"].Replace("%1", memberToNotify.Alias))
                .Replace(ReplaceToken.ComplimentaryClose1, dicResource["complimentary-close-1"])
                .Replace(ReplaceToken.ComplimentaryClose2, dicResource["complimentary-close-2"])
                .Replace(ReplaceToken.Disclaimer, dicResource["disclaimer"])
                .Replace(ReplaceToken.BetaDisclaimer, dicResource["beta-disclaimer"]);

            var message = new Message
            {
                Title = title,
                Body = body,
                SenderEmail = DoNotReplyAdddress,
                SenderName = DoNotReplyName
            };
            SendMessage(message, memberToNotify.Email);
        }

        internal static void SendBuddyConfirmationNotificationEmail(Member memberToNotify, Member confirmingBuddy)
        {
            var lang = Enum.GetName(typeof(MemberLanguage), memberToNotify.Options.Language);
            var langResource = Resources.ResourceManager.GetString("mail_" + lang + "_json");
            var dicResource = JsonConvert.DeserializeObject<Dictionary<string, string>>(langResource);

            var body = Resources.mail_template_inline;
            var content = Resources.ResourceManager.GetString("BuddyConfirmation_" + lang, Resources.Culture);
            var title = dicResource["buddy-confirmation-title"];

            if (!string.IsNullOrEmpty(content))
                content = content
                .Replace(ReplaceToken.UserAlias, confirmingBuddy.Alias);

            body = body
                .Replace(ReplaceToken.Title, title)
                .Replace(ReplaceToken.Body, content)
                .Replace(ReplaceToken.Salutation, dicResource["salutation"].Replace("%1", memberToNotify.Alias))
                .Replace(ReplaceToken.ComplimentaryClose1, dicResource["complimentary-close-1"])
                .Replace(ReplaceToken.ComplimentaryClose2, dicResource["complimentary-close-2"])
                .Replace(ReplaceToken.Disclaimer, dicResource["disclaimer"])
                .Replace(ReplaceToken.BetaDisclaimer, dicResource["beta-disclaimer"]);

            var message = new Message
            {
                Title = title,
                Body = body,
                SenderEmail = DoNotReplyAdddress,
                SenderName = DoNotReplyName
            };
            SendMessage(message, memberToNotify.Email);
        }

        internal static void SendNewPhotoNotificationEmail(MinimalPhotoInfo info, List<Member> members)
        {
            foreach (var member in members)
            {
                var lang = Enum.GetName(typeof(MemberLanguage), member.Options.Language);
                var langResource = Resources.ResourceManager.GetString("mail_" + lang + "_json");
                var dicResource = JsonConvert.DeserializeObject<Dictionary<string, string>>(langResource);

                var body = Resources.mail_template_inline;
                var content = Resources.ResourceManager.GetString("NewPhoto_" + lang, Resources.Culture);
                var title = dicResource["new-foto-title"].Replace("%1", info.Member.Alias);

                if (!string.IsNullOrEmpty(content))
                    content = content
                    .Replace(ReplaceToken.Alias, member.Alias)
                    .Replace(ReplaceToken.UserAlias, info.Member.Alias)
                    .Replace(ReplaceToken.PhotoName, info.Name)
                    .Replace(ReplaceToken.PhotoTitle, info.Title)
                    .Replace(ReplaceToken.PhotoUrl, info.Url640);

                body = body
                    .Replace(ReplaceToken.Title, title)
                    .Replace(ReplaceToken.Body, content)
                    .Replace(ReplaceToken.Salutation, dicResource["salutation"].Replace("%1", member.Alias))
                    .Replace(ReplaceToken.ComplimentaryClose1, dicResource["complimentary-close-1"])
                    .Replace(ReplaceToken.ComplimentaryClose2, dicResource["complimentary-close-2"])
                    .Replace(ReplaceToken.Disclaimer, dicResource["disclaimer"])
                    .Replace(ReplaceToken.BetaDisclaimer, dicResource["beta-disclaimer"]);

                var message = new Message
                {
                    Title = title,
                    Body = body,
                    SenderEmail = DoNotReplyAdddress,
                    SenderName = DoNotReplyName
                };
                SendMessage(message, member.Email);
            }
        }

        /// <summary>
        /// Sendet eine Email an info@fotosteam.com
        /// </summary>
        /// <returns>Result mit Boolean</returns>
        [HttpPost]
        public Result<bool> Contact([FromBody] Message message)
        {
            return SendEmail(message, Recipient.Contact);
        }

        /// <summary>
        /// Sendet eine Email an Trello
        /// </summary>
        /// <returns>Result mit Boolean</returns>

        [HttpPost]
        public Result<bool> Trello([FromBody] Message message)
        {
            return SendEmail(message, Recipient.Trello);
        }

        /// <summary>
        /// Sendet eine Email an ein Mitglied
        /// </summary>
        /// <returns>Result mit Boolean</returns>
        [HttpPost]
        public Result<bool> Member(string data, [FromBody] Message message)
        {
            return SendEmail(message, Recipient.Member, data);
        }

        private enum Recipient
        {
            Contact,
            Trello,
            Member
        }

        private Result<bool> SendEmail(Message message, Recipient recipient, string alias = null)
        {
            var result = new Result<bool>();
            try
            {
                var recipientEmail = string.Empty;
                switch (recipient)
                {
                    case Recipient.Contact:
                        recipientEmail = _fotoSteamAddress;
                        message.Body = message.SenderName + " am " + DateTime.Now + "\n\n" + message.Body + "\n\n" + message.SenderEmail;
                        break;
                    case Recipient.Trello:
                        recipientEmail = _trelloEmailAddress;
                        message.Title = message.SenderName + ": " + message.Title;
                        message.Body = DateTime.Now + "\n\n" + message.Body + "\n\n" + message.SenderEmail;
                        break;
                    case Recipient.Member:
                        {
                            Log.Warn(string.Format("Attempt to send message to {0}", alias));
                            result.Status.Code = Controller.StatusCode.NotAuthorized;
                            return result;
                        }
                }
                SendMessage(message, recipientEmail);
                result.Data = true;
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result, false);
            }
            return result;
        }



        private static void SendMessage(Message message, string recipientEmail)
        {
            try
            {
                var mailMessage = new MailMessage()
                {
                    Subject = message.Title,
                    Sender = new MailAddress(DoNotReplyAdddress),
                    Body = message.Body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(new MailAddress(recipientEmail));

                var client = new SmtpClient(); //Der Host wird über die mailing            
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
