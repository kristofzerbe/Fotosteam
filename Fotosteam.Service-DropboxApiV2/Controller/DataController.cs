using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Fotosteam.Service.Connector;
using Fotosteam.Service.Hub;
using Fotosteam.Service.Imaging;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Newtonsoft.Json;
// ReSharper disable PossibleNullReferenceException
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Fotosteam.Service.Controller
{
    /// <summary>
    ///     Der Controller beinhaltet die Funktionaliät zur Manipulation der Bilddateien im Fotostream-Ordner des Benutzers.
    ///     Wird über api/image aufgerufen (Das wird jedoch im Startup von Owin definiert und hier wird der Standardwert
    ///     angenommen.
    /// </summary>
    /// ///
    /// <remarks>
    ///     Paramenternamen richten sich nach der Definition im <see cref="WebApiConfig" />.
    ///     Aus diesem Grund taucht der Parameter data häufig auf, auch wenn ein anderer Name passender wäre
    /// </remarks>
    public class DataController : ControllerBase
    {
        private static Dictionary<string, int> _allCategories;
        private static readonly Object Lock = new Object();

        /// <summary>
        ///     Standardkonstruktor, der ein Datenrepository initalisiert
        /// </summary>
        public DataController()
        {
            DataRepository = new DataRepository();
            AuthenticationRepository = new AuthRepository();
        }

        /// <summary>
        ///     Überladener Konstruktor, der das Einfügen eines Datenrepositories erfordert
        /// </summary>
        /// <param name="repository"></param>
        public DataController(IDataRepository repository)
        {
            Log.Debug(string.Format("Intialisiere DataController mit externem Repository {0}", repository.GetType()));
            DataRepository = repository;
        }

        /// <summary>
        ///     Überladener Konstruktor, der das Einfügen eines Datenrepositories erfordert
        /// </summary>
        /// <param name="repository">Das zu verwendende Datenrepository</param>
        /// <param name="authRepository">Das zu verwendende Authentifzierungsrepsoitory</param>
        public DataController(IDataRepository repository, IAuthRepository authRepository)
        {
            Log.Debug(string.Format("Intialisiere DataController mit externem Repository {0}", repository.GetType()));
            DataRepository = repository;
            AuthenticationRepository = authRepository;
        }

        private IAuthRepository AuthRepository
        {
            get
            {
                if (AuthenticationRepository == null)
                    AuthenticationRepository = new AuthRepository();

                return AuthenticationRepository;
            }
        }

        #region Delete-Methoden

        /// <summary>
        ///     Löscht ein Element anhand des Kriterium == Id des Elements
        /// </summary>
        /// <param name="elementType">Art des Elements, dass gelöscht werden soll</param>
        /// <param name="criteria">Die Id des Elements</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        public Result<bool> Delete(string elementType, string criteria)
        {
            var result = new Result<bool>();
            var resultValue = false;

            int elmentId;
            if (!int.TryParse(criteria, out elmentId))
            {
                result.Status.Code = Controller.StatusCode.NoData;
                return result;
            }
            try
            {
                int storyId;
                switch (elementType.ToLower())
                {
                    case DeleteAction.Story:
                        resultValue = DeleteById<Story>(elmentId);
                        break;
                    case DeleteAction.Chapter:
                        storyId = DataRepository.GetStoryIdByChapterId(elmentId);
                        resultValue = DeleteById<Chapter>(elmentId);
                        DataRepository.UpdateCountsForStory(storyId);
                        break;
                    case DeleteAction.Ledge:
                        storyId = DataRepository.GetStoryIdByLedgeId(elmentId);
                        resultValue = DeleteById<Ledge>(elmentId);
                        DataRepository.UpdateCountsForStory(storyId);
                        break;
                    case DeleteAction.TextBrick:
                        resultValue = DeleteById<TextBrick>(elmentId);
                        break;
                    case DeleteAction.PhotoBrick:
                        storyId = DataRepository.GetStoryIdByPhotoBrickId(elmentId);
                        resultValue = DeleteById<PhotoBrick>(elmentId);
                        DataRepository.UpdateCountsForStory(storyId);
                        break;
                    case DeleteAction.MapBrick:
                        resultValue = DeleteById<MapBrick>(elmentId);
                        break;
                    case DeleteAction.Member:
                        resultValue = DeleteById<Member>(elmentId);
                        break;
                    case DeleteAction.Buddy:
                        resultValue = DeleteBuddy(elmentId);
                        break;
                    case DeleteAction.Comment:
                        resultValue = DeleteComment(elmentId);
                        break;
                    case DeleteAction.Event:
                        resultValue = DeleteById<Event>(elmentId);
                        break;
                    case DeleteAction.Photo:
                        resultValue = DeleteById<Photo>(elmentId);
                        break;
                    case DeleteAction.Location:
                        resultValue = DeleteById<Location>(elmentId);
                        break;
                    case DeleteAction.Topic:
                        resultValue = DeleteById<Topic>(elmentId);
                        break;
                    case DeleteAction.SocialMedia:
                        resultValue = DeleteById<SocialMedia>(elmentId);
                        break;
                    case DeleteAction.Exif:
                        resultValue = DeleteExifByPhotoId(elmentId);
                        break;
                    default:
                        {
                            result.Status.Code = Controller.StatusCode.UnknownAction;
                            break;
                        }
                }
                if (!resultValue)
                {
                    result.Status.Code = Controller.StatusCode.NoData;
                }
                result.Data = resultValue;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;
            }
            return result;
        }

        #endregion

        private bool DeleteById<T>(int elementId) where T : PocoBase
        {
            if (!CanExecute<T>(elementId))
            {
                return false;
            }
            var entity = Activator.CreateInstance<T>();

            entity.Id = elementId;
            DataRepository.Delete(entity);
            return true;
        }

        private bool DeleteComment(int commentId)
        {
            var comment = DataRepository.Queryable<Comment>().FirstOrDefault(c => c.CommentId == commentId);
            if (comment != null)
            {
                DataRepository.Delete(comment);
                return true;
            }
            return false;
        }

        private bool DeleteBuddy(int buddyIdToBeDeleted)
        {
            var currentMemberId = GetMemberId();
            var buddy = DataRepository.Queryable<Buddy>().FirstOrDefault(b => b.MemberId == currentMemberId && b.BuddyMemberId == buddyIdToBeDeleted);
            if (buddy != null)
            {
                DataRepository.Delete(buddy);
                return true;
            }

            buddy = DataRepository.Queryable<Buddy>().FirstOrDefault(b => b.BuddyMemberId == currentMemberId && b.MemberId == buddyIdToBeDeleted);
            if (buddy != null)
            {
                buddy.IsMutual = false;
                DataRepository.Update(buddy);
                return true;
            }
            return false;
        }

        private bool DeleteExifByPhotoId(int photoId)
        {
            if (!CanExecute<Photo>(photoId))
            {
                return false;
            }
            var exif = new ExifData { PhotoId = photoId };
            DataRepository.Delete(exif);
            return true;
        }

        private bool CanExecute<T>(int id) where T : PocoBase
        {
            if (!IsUserAuthorized)
            {
                return false;
            }

            var memberId = GetMemberId();
            return ElementBelongsToMember<T>(memberId, id);
        }
        private bool CanExecuteOptions(int id)
        {
            if (!IsUserAuthorized)
            {
                return false;
            }

            var memberId = GetMemberId();
            return DataRepository.Queryable<Member>().Any(m => m.Id == memberId && m.Id == id);
        }

        private bool ElementBelongsToMember<T>(int memberId, int elementId) where T : PocoBase
        {
            try
            {
                if (typeof(T) == typeof(Member) || typeof(T) == typeof(MemberOption))
                {
                    return DataRepository.Queryable<Member>().Any(m => m.Id == memberId && m.Id == elementId);
                }

                if (typeof(T) == typeof(Photo))
                {
                    return DataRepository.Queryable<Photo>().Any(p => p.MemberId == memberId && p.Id == elementId);
                }

                if (typeof(T) == typeof(Event))
                {
                    return DataRepository.Queryable<Event>().Any(e => e.MemberId == memberId && e.Id == elementId);
                }

                if (typeof(T) == typeof(Topic))
                {
                    return DataRepository.Queryable<Topic>().Any(t => t.MemberId == memberId && t.Id == elementId);
                }

                if (typeof(T) == typeof(Location))
                {
                    return DataRepository.Queryable<Location>().Any(l => l.MemberId == memberId && l.Id == elementId);
                }

                if (typeof(T) == typeof(Story))
                {
                    return DataRepository.Queryable<Story>().Any(s => s.Id == elementId && s.MemberId == memberId);
                }

                if (typeof(T) == typeof(SocialMedia))
                {
                    return DataRepository.Queryable<SocialMedia>().Any(s => s.Id == elementId && s.MemberId == memberId);
                }
                if (typeof(T) == typeof(Notification))
                {
                    return DataRepository.Queryable<Notification>().Any(s => s.Id == elementId && s.MemberId == memberId);
                }
                if (typeof(T) == typeof(Chapter))
                {
                    return
                        DataRepository.Queryable<Story>()
                            .Where(s => s.MemberId == memberId)
                            .Select(s => s.Chapters.Select(c => c.Id == elementId)).Any();
                }

                if (typeof(T) == typeof(Ledge))
                {
                    return DataRepository.Queryable<Story>()
                        .Where(s => s.MemberId == memberId)
                        .Select(s => s.Chapters.Select(c => c.Ledges.Select(l => l.Id == elementId))).Any();
                }

                if (typeof(T).BaseType == typeof(Brick))
                {
                    return DataRepository.Queryable<Story>()
                        .Where(s => s.MemberId == memberId)
                        .Select(
                            s => s.Chapters.Select(c => c.Ledges.Select(l => l.Bricks.Select(b => b.Id == elementId))))
                        .Any();
                }
            }
            catch (Exception ex)
            {
                Log.Info("Element nicht gefunden", ex);
            }
            return false;
        }

        #region Private Methoden


        private static Dictionary<string, int> AllCategories
        {
            get
            {
                lock (Lock)
                {
                    if (_allCategories == null)
                    {
                        _allCategories = new Dictionary<string, int>();
                        foreach (var value in Enum.GetValues(typeof(CategoryType)))
                        {
                            var category = (CategoryType)value;
                            _allCategories.Add(category.ToString(), (int)value);
                        }
                    }
                    return _allCategories;
                }
            }
        }

        private Result<T> CheckResult<T>(Result<T> result)
        {
            if (result.Data == null)
            {
                result.Status.Code = Controller.StatusCode.NoData;
                result.Status.Message = ResultMessages.NoMatchingData;
            }
            var photos = result.Data as List<Photo>;
            if (photos != null)
            {
                CheckFullDownload(photos);
            }
            return result;
        }

        private void CheckFullDownload(List<Photo> photos)
        {
            if (photos == null)
                return;

            var currentMemberId = User.Identity.IsAuthenticated ? GetMemberId() : Constants.NotSetId;

            if (photos.Any(p => p.MemberId == currentMemberId))
                return;

            foreach (var link in photos.Where(photo => !photo.AllowFullSizeDownload && photo.DirectLinks != null)
                .Select(photo => photo.DirectLinks.FirstOrDefault(d => d.Size == 0)).Where(link => link != null))
            {
                link.Url = string.Empty;
            }
        }

        #endregion

        #region Get-Methoden

        /// <summary>
        ///     * /api/data/&lt;Alias&gt;/categories
        ///     * /api/data/&lt;Alias&gt;/events
        ///     * /api/data/&lt;Alias&gt;/locations
        ///     * /api/data/&lt;Alias&gt;/topics
        /// </summary>
        /// <param name="elementType">Alias der Benutzers, z.B. bobbakos</param>
        /// <param name="criteria">Die Art der Information, die gewünscht ist. I.R. ist es eine Liste von Objektens</param>
        /// <returns></returns>
        [HttpGet]
        public object Get(string elementType, string criteria)
        {
            criteria = criteria.ToLower();
            try
            {
                switch (criteria)
                {
                    case GetAction.Categories:
                        return GetCategoriesForUser(elementType);
                    case GetAction.Events:
                        return GetEventsForUser(elementType);
                    case GetAction.Locations:
                        return GetLocationsForUser(elementType);
                    case GetAction.Topics:
                        return GetTopicsForUser(elementType);
                    case GetAction.Photos:
                        int id;
                        if (int.TryParse(elementType, out id))
                        {
                            return GetPhotoById(id);
                        }
                        return Get(elementType, GetAction.ByJournal, 0, 0);
                    case GetAction.Stories:
                        return GetStoriesForUser(elementType);
                    default:
                        {
                            return GetUnknownActionResult<List<Photo>>();
                        }
                }
            }
            catch (Exception ex)
            {
                var result = new Result<string>();
                LogExceptionAndSetResult(ex, result);
                return result;
            }
        }
        /// <summary>
        ///     /api/data/&lt;Alias&gt;/topic/&lt;Topic&gt;
        ///     /api/data/&lt;Alias&gt;/location/lt;location&gt;
        ///     /api/data/&lt;Alias&gt;/event/lt;event&gt;
        ///     /api/data/&lt;Alias&gt;/category/lt;category&gt;
        /// </summary>
        /// <param name="alias">Alias der Benutzers, z.B. bobbakos</param>
        /// <param name="elementType">Art des Kriteriums: topic, location, event, category</param>
        /// <param name="criteria">Das Suchkritierium, z.B. Berlin</param>
        /// <returns></returns>
        [HttpGet]
        public object Get(string alias, string elementType, string criteria)
        {
            if (criteria == "*" || criteria == "%")
                criteria = string.Empty;
            try
            {
                switch (elementType)
                {
                    case GetAction.RandomPhotos:
                        return GetRandomPhotos(alias, criteria);
                    case GetAction.ByTopic:
                        return GetPhotosByTopic(alias, criteria, 0, 0);
                    case GetAction.ByLocation:
                        return GetPhotosByLocation(alias, criteria, GetAction.ByLocation, 0, 0);
                    case GetAction.ByCity:
                        return GetPhotosByLocation(alias, criteria, GetAction.ByCity, 0, 0);
                    case GetAction.ByCountry:
                        return GetPhotosByLocation(alias, criteria, GetAction.ByCountry, 0, 0);
                    case GetAction.ByEvent:
                        return GetPhotosByEvent(alias, criteria, 0, 0);
                    case GetAction.ByCatergory:
                        return GetPhotosByCategory(alias, criteria, 0, 0);
                    case GetAction.LocationGroups:
                        {
                            LocationGrouping locationGroup;
                            return Enum.TryParse(criteria, true, out locationGroup)
                                ? GetLocationGroup(alias, locationGroup)
                                : Result<List<LocationGroup>>.GetUnkownActionResult();
                        }

                }

                return Result<List<Photo>>.GetUnkownActionResult();
            }
            catch (Exception ex)
            {
                var result = new Result<string>();
                LogExceptionAndSetResult(ex, result);
                return result;
            }
        }
        //api/data/&lt;Alias&gt;/journal/<Skip>/<Take>
        /// <summary>
        ///     Liefert alle Fotos zu einem Benutzer
        /// </summary>
        /// <param name="aliasOrCommand">Benutzername, oder für neue Kommentare = newcommentsforphoto</param>
        /// <param name="criteria">Ist fix = journal, wird für Api gebraucht, nicht ändern!</param>
        /// <param name="skip">Anzahl der Elemente, die übersprungen werden sollen</param>
        /// <param name="take">Anzahl der Fotos, die geholt werden sollen</param>
        /// <returns>Die Überladen des ersten Parameters ist subotimal, aber es kann kein Get mit einer anderen Signatur erstellt werden</returns>
        [HttpGet]
        public object Get(string aliasOrCommand, string criteria, int skip, int take)
        {
            try
            {
                if (aliasOrCommand.ToLower().Equals(GetAction.CommentsForPhoto.ToLower()))
                {
                    return GetCommentsForPhoto(criteria, skip, take);
                }
                if (aliasOrCommand.ToLower().Equals(GetAction.RatingsForPhoto.ToLower()))
                {
                    return GetRatingsForPhoto(criteria, skip, take);
                }
                if (aliasOrCommand.ToLower().Equals(GetAction.NewComments.ToLower()))
                {
                    return GetNewComments(skip, take);
                }

                if (aliasOrCommand.ToLower().Equals(GetAction.Find.ToLower()))
                    return Find(criteria, skip, take);

                if (criteria.ToLower().Equals(GetAction.NewPhotos.ToLower()))
                {
                    return GetNewPhotos(skip, take, Constants.NotSetId);
                }
                if (criteria.ToLower().Equals(GetAction.LatestPhotosForUser.ToLower()))
                {
                    var memberId = DataRepository.GetIdForAlias(aliasOrCommand);
                    return GetNewPhotos(skip, take, memberId);
                }
                if (criteria.ToLower().Equals(GetAction.TopRatedPhotos.ToLower()))
                {
                    return GetTopRatedPhotos(aliasOrCommand, skip, take, false);
                }
                if (criteria.ToLower().Equals(GetAction.MyTopRatedPhotos.ToLower()))
                {
                    return GetTopRatedPhotos(Constants.ForAll, skip, take, true);
                }
                if (criteria.ToLower().Equals(GetAction.ByJournal.ToLower()))
                {
                    return GetPhotosForUser(aliasOrCommand, skip, take);
                }
                if (criteria.ToLower().Equals(GetAction.CC0Photos))
                {
                    return GetCC0Photos(aliasOrCommand, skip, take, Request.GetQueryNameValuePairs());
                }
                if (criteria.ToLower().Equals(GetAction.UnReadNotifications.ToLower()))
                {
                    return GetNotifications(skip, take, true);
                }
                if (criteria.ToLower().Equals(GetAction.AllNotifications.ToLower()))
                {
                    return GetNotifications(skip, take, false);
                }
                return GetUnknownActionResult<bool>();
            }
            catch (Exception ex)
            {
                var result = new Result<string>();
                LogExceptionAndSetResult(ex, result);
                return result;
            }
        }

        [HttpGet]
        public object Get(string alias, string elementType, string criteria, int skip, int take)
        {
            try
            {
                switch (elementType)
                {
                    case GetAction.ByTopic:
                        return GetPhotosByTopic(alias, criteria, skip, take);
                    case GetAction.ByLocation:
                        return GetPhotosByLocation(alias, criteria, GetAction.ByLocation, skip, take);
                    case GetAction.ByCity:
                        return GetPhotosByLocation(alias, criteria, GetAction.ByCity, skip, take);
                    case GetAction.ByCountry:
                        return GetPhotosByLocation(alias, criteria, GetAction.ByCountry, skip, take);
                    case GetAction.ByEvent:
                        return GetPhotosByEvent(alias, criteria, skip, take);
                    case GetAction.ByCatergory:
                        return GetPhotosByCategory(alias, criteria, skip, take);
                }
                return GetUnknownActionResult<bool>();
            }
            catch (Exception ex)
            {
                var result = new Result<string>();
                LogExceptionAndSetResult(ex, result);
                return result;
            }
        }

        private object GetPhotosForUser(string alias, int skip, int take)
        {
            var result = new Result<List<Photo>>();
            try
            {
                result.Data = DataRepository.GetPhotos(alias, skip, take, DoesAliasMatchAuthenticatedUser(alias));
                foreach (var photo in result.Data)
                {
                    photo.Member = null;
                }
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }

        private object GetPhotoById(int id)
        {
            var result = new Result<MinimalPhotoInfo>();
            try
            {
                if (!CanExecute<Photo>(id))
                {
                    SetNotAuthorizedResult(result);
                    return result;
                }
                result.Data = DataRepository.GetMinimalPhotoInfo(id);
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }

        private object GetNewPhotos(int skip, int take, int memberId)
        {
            var photos = new Result<List<NewPhoto>>();
            try
            {
                photos.Data = DataRepository.GetLatestPhotos(skip, take, memberId);
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, photos);
            }
            return CheckResult(photos);
        }

        private object GetTopRatedPhotos(string alias, int skip, int take, bool forCurrentUser)
        {
            var photos = new Result<List<NewPhoto>>();
            try
            {
                int memberId = Constants.NotSetId;
                if (forCurrentUser)
                    memberId = GetMemberId();

                photos.Data = DataRepository.GetTopRatedPhotos(alias, skip, take, memberId);
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, photos);
            }
            return CheckResult(photos);
        }

        private object GetCC0Photos(string alias, int skip, int take, IEnumerable<KeyValuePair<string, string>> filter)
        {
            var photos = new Result<List<FilteredPhoto>>();
            try
            {
                photos.Data = DataRepository.GetCC0Photos(alias, skip, take, filter);
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, photos);
            }
            return CheckResult(photos);
        }
        private object GetNotifications(int skip, int take, bool onlyNotRead)
        {
            var notifications = new Result<List<GroupedNotification>>();
            try
            {
                notifications.Data = DataRepository.GetNotifications(skip, take, onlyNotRead, GetMemberId());
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, notifications);

            }
            return notifications;
        }


        /// <summary>
        ///     Gibt die Liste alle Fotos zurück, die der Benutzer noch nicht bearbeitet hat
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public Result<List<Photo>> NewPhotos()
        {
            var result = new Result<List<Photo>>();
            try
            {
                var memberId = GetMemberId();
                var photos = DataRepository.GetNewPhotos(memberId);
                result.Data = photos;
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }

            return result;
        }


        private Result<List<LocationGroup>> GetLocationGroup(string alias, LocationGrouping grouping)
        {
            var result = new Result<List<LocationGroup>>();
            var allLocations = GetLocationsForUser(alias).Data;

            if (allLocations == null || !allLocations.Any())
            {
                result.Status.Code = Controller.StatusCode.NoData;
                result.Status.Message = ResultMessages.NoMatchingData;
                return result;
            }

            switch (grouping)
            {
                case LocationGrouping.Country:
                    result.Data = GroupLocationsByCountry(allLocations);
                    break;
                case LocationGrouping.County:
                    result.Data = GroupLocationsByCounty(allLocations);
                    break;
                case LocationGrouping.City:
                    result.Data = GroupLocationsByCity(allLocations);
                    break;
                default:
                    result.Status.Code = Controller.StatusCode.UnknownAction;
                    result.Status.Message = ResultMessages.UnkownAction;
                    break;
            }
            return result;
        }


        private Result<List<Photo>> GetRandomPhotos(string alias, string criteria)
        {
            var result = new Result<List<Photo>>();
            try
            {
                result.Data = DataRepository.GetRandomPhotos(alias, int.Parse(criteria), DoesAliasMatchAuthenticatedUser(alias));
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }

        private Result<List<Photo>> GetPhotosByTopic(string alias, string criteria, int skip, int take)
        {
            var result = new Result<List<Photo>>();
            try
            {
                result.Data = DataRepository.GetPhotosByTopic(alias, criteria, DoesAliasMatchAuthenticatedUser(alias), skip, take);
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }

        private Result<List<Photo>> GetPhotosByLocation(string alias, string criteria, string type, int skip, int take)
        {
            var result = new Result<List<Photo>>();
            try
            {
                result.Data = DataRepository.GetPhotosByLocation(alias, criteria, DoesAliasMatchAuthenticatedUser(alias), type, skip, take);
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }


        private List<LocationGroup> GroupLocationsByCountry(List<Location> locations)
        {
            var result = locations.GroupBy(l => l.County).Select(g => new LocationGroup()
            {
                FirstLatitude = g.Where(x => x.Latitude != 0).DefaultIfEmpty(new Location() { Latitude = 0 }).FirstOrDefault().Latitude,
                FirstLongitude = g.Where(x => x.Longitude != 0).DefaultIfEmpty(new Location() { Longitude = 0 }).FirstOrDefault().Longitude,
                Name = g.Any(x => string.IsNullOrEmpty(x.County)) ? g.First(x => !string.IsNullOrEmpty(x.Name)).Name : g.First(x => !string.IsNullOrEmpty(x.County)).County,
                Locations = g.ToList(),
                PhotoCount = g.Sum(x => x.PhotoCount)
            }).ToList();


            return result;
        }

        private const string NotSet = "Not Set";
        private List<LocationGroup> GroupLocationsByCounty(List<Location> locations)
        {
            var result = locations.GroupBy(l => new { l.Country, l.County }).Select(g => new LocationGroup()
            {
                FirstLatitude = g.Where(x => x.Latitude != 0).DefaultIfEmpty(new Location() { Latitude = 0 }).FirstOrDefault().Latitude,
                FirstLongitude = g.Where(x => x.Longitude != 0).DefaultIfEmpty(new Location() { Longitude = 0 }).FirstOrDefault().Longitude,
                Name = g.Where(x => !string.IsNullOrEmpty(x.Country)).DefaultIfEmpty(new Location() { Country = NotSet }).FirstOrDefault().Country + ", "
                + (g.Any(x => string.IsNullOrEmpty(x.County)) ? NotSet : g.First(x => !string.IsNullOrEmpty(x.County)).County),
                Locations = g.ToList(),
                PhotoCount = g.Sum(x => x.PhotoCount)
            }).ToList();

            return result;

        }

        private List<LocationGroup> GroupLocationsByCity(List<Location> locations)
        {
            var result = locations.GroupBy(l => new { l.Country, l.City }).Select(g => new LocationGroup()
            {
                FirstLatitude = g.Where(x => x.Latitude != 0).DefaultIfEmpty(new Location() { Latitude = 0 }).FirstOrDefault().Latitude,
                FirstLongitude = g.Where(x => x.Longitude != 0).DefaultIfEmpty(new Location() { Longitude = 0 }).FirstOrDefault().Longitude,
                Name = g.Where(x => !string.IsNullOrEmpty(x.Name)).DefaultIfEmpty(new Location() { Name = NotSet }).FirstOrDefault().Name,
                Locations = g.ToList(),
                PhotoCount = g.Sum(x => x.PhotoCount)
            }).OrderBy(g=>g.Name).ToList();

            return result;
        }
  
        private Result<List<Photo>> GetPhotosByCategory(string alias, string criteria, int skip, int take)
        {
            var result = new Result<List<Photo>>();
            try
            {
                result.Data = DataRepository.GetPhotosByCategory(alias, criteria, DoesAliasMatchAuthenticatedUser(alias), skip, take);
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }

        private Result<List<Photo>> GetPhotosByEvent(string alias, string criteria, int skip, int take)
        {
            var result = new Result<List<Photo>>();
            try
            {
                result.Data = DataRepository.GetPhotosByEvent(alias, criteria, DoesAliasMatchAuthenticatedUser(alias), skip, take);
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }

        private Result<T> GetUnknownActionResult<T>()
        {
            return new Result<T>
            {
                Status = new Status
                {
                    Code = Controller.StatusCode.UnknownAction,
                    Message = "Unknown Request"
                }
            };
        }

        private object Find(string criteria, int skip, int take)
        {
            var byCategory = GetPhotosByCategory(string.Empty, criteria, skip, take);
            var byTopic = GetPhotosByTopic(string.Empty, criteria, skip, take);
            var byLocation = GetPhotosByLocation(string.Empty, criteria, GetAction.ByLocation, skip, take);
            var byEvent = GetPhotosByEvent(string.Empty, criteria, skip, take);
            var result = new List<object>()
            {
                new{ByCategory = byCategory.Data} ,
                new{ByTopics =  byTopic.Data} ,
                new{ByLocation  =byLocation.Data} ,
                new{ByEvent = byEvent.Data }
            };
            return new Result<object> { Data = result };
        }

        private Result<List<Comment>> GetCommentsForPhoto(string photoId, int skip, int take)
        {
            int id;
            var result = new Result<List<Comment>>();
            try
            {
                if (int.TryParse(photoId, out id))
                {
                    var query = DataRepository.Queryable<Comment>().Where(c => c.PhotoId == id);

                    if (skip > 0)
                        query = query.Skip(skip);

                    if (take > 0)
                        query = query.Take(take);

                    result.Data = query.OrderByDescending(c => c.CommentId).ThenBy(c => c.ParentCommentId).ThenBy(c => c.CommentId).ToList();
                }

            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }
        private Result<List<Rating>> GetRatingsForPhoto(string photoId, int skip, int take)
        {
            int id;
            var result = new Result<List<Rating>>();
            try
            {
                if (int.TryParse(photoId, out id))
                {

                    var query = DataRepository.Queryable<Rating>().Where(r => r.PhotoId == id);
                    if (skip > 0)
                        query = query.Skip(skip);

                    if (take > 0)
                        query = query.Take(take);

                    result.Data = query.OrderByDescending(r => r.Date).ThenBy(r => r.AverageRating).ThenBy(r => r.Value).ToList();
                }

            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }
        private Result<List<Comment>> GetNewComments(int skip, int take)
        {

            var result = new Result<List<Comment>>();
            try
            {
                var query = DataRepository.Queryable<Comment>();

                if (skip > 0)
                    query = query.Skip(skip);

                if (take > 0)
                    query = query.Take(take);

                result.Data = query.OrderBy(c => c.ParentCommentId).ThenBy(c => c.CommentId).ToList();
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }


        private Result<List<Category>> GetCategoriesForUser(string alias)
        {
            var result = new Result<List<Category>>();
            try
            {
                result.Data = DataRepository.GetCategories(alias, DoesAliasMatchAuthenticatedUser(alias));

                var categories = new Dictionary<string, int>(AllCategories);
                //Kategorien hinzufügen, die ohne Fotos sind
                if (result.Data == null)
                {
                    result.Data = new List<Category>();
                }

                foreach (var category in result.Data)
                {
                    categories.Remove(category.Type);
                }

                foreach (var category in categories)
                {
                    result.Data.Add(new Category { PhotoCount = 0, Type = category.Key, TypeValue = category.Value });
                }
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }

        private Result<List<Event>> GetEventsForUser(string alias)
        {
            var result = new Result<List<Event>>();
            try
            {
                result.Data = DataRepository.GetEvents(alias, DoesAliasMatchAuthenticatedUser(alias));
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }

        private Result<List<Location>> GetLocationsForUser(string alias)
        {
            var result = new Result<List<Location>>();
            try
            {
                result.Data = DataRepository.GetLocations(alias, DoesAliasMatchAuthenticatedUser(alias));
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }

        private Result<List<Topic>> GetTopicsForUser(string alias)
        {
            var result = new Result<List<Topic>>();
            try
            {
                result.Data = DataRepository.GetTopics(alias, DoesAliasMatchAuthenticatedUser(alias));
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }
            return CheckResult(result);
        }

        private Result<List<Story>> GetStoriesForUser(string alias)
        {
            var result = new Result<List<Story>>();
            try
            {
                result.Data = DataRepository.GetStories(alias, DoesAliasMatchAuthenticatedUser(alias));
                //Member entfernen, da die Informationen nicht zurückgegeben werden sollen
                if (result.Data != null)
                {
                    foreach (var story in result.Data)
                    {
                        story.Member = null;
                        if (story.HeaderPhoto != null)
                        {
                            story.HeaderPhoto.Member = null;
                            story.HeaderPhoto.Stories = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptionAndSetResult(ex, result);
            }

            return CheckResult(result);
        }

        #endregion

        #region Post-Methoden

        /// <summary>
        ///     Speichert ein Element in die Datenbank
        /// </summary>
        /// <param name="elementType">Der Typ des Elementes. Muss als Text <see cref="BrickType" /> entsprechen</param>
        /// <returns>Ein Result&lt;ElementTyp&gt;</returns>
        [Authorize]
        [HttpPost]
        public object Post([FromUri] string elementType)
        {
            try
            {
                if (Request.Content.IsMimeMultipartContent())
                {
                    var fileNameOrId = Request.RequestUri.Segments[Request.RequestUri.Segments.Length - 1];
                    if (elementType.ToLower() == PostOrPutAction.Photo)
                        return UploadPhoto(fileNameOrId);
                    if (elementType.ToLower() == PostOrPutAction.UpdatePhoto)
                        return UpdatePhotoWithNewImage(fileNameOrId);
                    if (elementType.ToLower() == PostOrPutAction.UpdateOriginalPhoto)
                        return UpdatePhotoWithOriginalPhoto(fileNameOrId);
                }

                dynamic pocoJson = Request.Content.ReadAsStringAsync().Result;

                switch (elementType.ToLower())
                {
                    case PostOrPutAction.Update:
                        return Update(pocoJson);
                    case PostOrPutAction.MergeLocations:
                        return MergeLocations(pocoJson);
                    case PostOrPutAction.PhotoTopic:
                        return UpdateTopicForPhoto(pocoJson);

                    case PostOrPutAction.Brick:
                        {
                            var value = (JsonConvert.DeserializeObject(pocoJson))["Type"].Value.ToLower();

                            if (value.Equals(BrickType.Text.ToString().ToLower()))
                            {
                                return AddPoco<TextBrick>(pocoJson);
                            }
                            if (value.Equals(BrickType.Map.ToString().ToLower()))
                            {
                                return AddPoco<MapBrick>(pocoJson);
                            }

                            if (value.Equals(BrickType.Photo.ToString().ToLower()))
                            {
                                Result<PhotoBrick> result = AddPoco<PhotoBrick>(pocoJson);
                                var storyId = DataRepository.GetStoryIdByPhotoBrickId(result.Data.Id);
                                DataRepository.UpdateCountsForStory(storyId);
                                return result;
                            }
                            return CreateNotDeSerializableResult<object>();
                        }
                    case PostOrPutAction.Chapter:
                        Result<Chapter> chapterResult = AddPoco<Chapter>(pocoJson);
                        DataRepository.UpdateCountsForStory(chapterResult.Data.StoryId);
                        return chapterResult;
                    case PostOrPutAction.Event:
                        return AddNewEvent(pocoJson);
                    case PostOrPutAction.Exif:
                        return AddPoco<ExifData>(pocoJson);
                    case PostOrPutAction.Ledge:
                        return AddPoco<Ledge>(pocoJson);
                    case PostOrPutAction.Location:
                        return AddPoco<Location>(pocoJson);
                    case PostOrPutAction.Photo:
                        return AddPoco<Photo>(pocoJson);
                    case PostOrPutAction.Story:
                        return AddPoco<Story>(pocoJson);
                    case PostOrPutAction.Topic:
                        return AddPoco<Topic>(pocoJson);
                    case PostOrPutAction.Member:
                        return AddPoco<Member>(pocoJson);
                    case PostOrPutAction.SocialMedia:
                        return AddPoco<SocialMedia>(pocoJson);
                    case PostOrPutAction.Buddy:
                        return AddBuddy(pocoJson);
                    case PostOrPutAction.ConfirmBuddy:
                        return ConfirmBuddy(pocoJson);
                    default:
                        return GetUnknownActionResult<object>();
                }
            }
            catch (Exception ex)
            {
                return LogExceptionAndCreateEmptyResult(ex);
            }
        }

        /// <summary>
        /// Beim Hinzufügen eines Ereignisses soll ein Location Objekt zurückgegeben werden.
        /// </summary>
        private Result<Event> AddNewEvent(object eventJson)
        {
            var eventResult = AddPoco<Event>(eventJson);
            eventResult.Data.Location = DataRepository.Queryable<Location>().FirstOrDefault(l => l.Id == eventResult.Data.LocationId);
            return eventResult;
        }


        private Result<T> CreateNotDeSerializableResult<T>() where T : class
        {
            var result = new Result<T>
            {
                Status =
                {
                    Code = Controller.StatusCode.CannotDeserializer,
                    Message = "Object cannot be deserialized"
                }
            };
            return result;
        }

        private static Dictionary<ModelType, Type> _modelTypes;

        private Dictionary<ModelType, Type> ModelTypes
        {
            get
            {
                lock (Lock)
                {
                    if (_modelTypes != null)
                        return _modelTypes;

                    _modelTypes = new Dictionary<ModelType, Type>();
                    _modelTypes.Add(ModelType.Comment, typeof(Comment));
                    _modelTypes.Add(ModelType.Chapter, typeof(Chapter));
                    _modelTypes.Add(ModelType.Event, typeof(Event));
                    _modelTypes.Add(ModelType.Exif, typeof(ExifData));
                    _modelTypes.Add(ModelType.Ledge, typeof(Ledge));
                    _modelTypes.Add(ModelType.Location, typeof(Location));
                    _modelTypes.Add(ModelType.MapBrick, typeof(MapBrick));
                    _modelTypes.Add(ModelType.Photo, typeof(Photo));
                    _modelTypes.Add(ModelType.PhotoBrick, typeof(PhotoBrick));
                    _modelTypes.Add(ModelType.Rate, typeof(Rating));
                    _modelTypes.Add(ModelType.Story, typeof(Story));
                    _modelTypes.Add(ModelType.TextBrick, typeof(TextBrick));
                    _modelTypes.Add(ModelType.Topic, typeof(Topic));
                    _modelTypes.Add(ModelType.Member, typeof(Member));
                    _modelTypes.Add(ModelType.MemberOption, typeof(MemberOption));
                    _modelTypes.Add(ModelType.SocialMedia, typeof(SocialMedia));
                    _modelTypes.Add(ModelType.Notification, typeof(Notification));

                    return _modelTypes;
                }
            }
        }

        private Result<bool> UpdateTopicForPhoto(object jsonModel)
        {
            var result = new Result<bool>();
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(jsonModel.ToString());
                int photoId = model.PhotoId;
                int topicId = model.TopicId;
                var canExecute = CanExecute<Photo>(photoId);
                canExecute = canExecute && CanExecute<Topic>(topicId);
                if (!canExecute)
                {
                    result.Status.Code = Controller.StatusCode.NotAuthorized;
                    return result;
                }
                bool isAdd = model.Action.ToString().ToLower() == "add";
                DataRepository.UpdatePhotoTopic(photoId, topicId, isAdd);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                result.Status.Code = Controller.StatusCode.CannotDeserializer;
                result.Status.Message = "Object cannot be deserialized";
            }

            result.Data = true;
            return result;
        }
        private Result<bool> MergeLocations(object jsonModel)
        {
            var result = new Result<bool>();
            try
            {
                var model = JsonConvert.DeserializeObject<dynamic>(jsonModel.ToString());
                int newId = model.NewId;
                int oldId = model.OldId;

                var canExecute = CanExecute<Location>(newId);
                canExecute = canExecute && CanExecute<Location>(oldId);
                if (!canExecute)
                {
                    result.Status.Code = Controller.StatusCode.NotAuthorized;
                    return result;
                }

                var photoCount = DataRepository.Queryable<Photo>().Count(x => x.LocationId == oldId);
                var newLocation = DataRepository.Queryable<Location>().FirstOrDefault(x => x.Id == newId);
                if (newLocation == null)
                {
                    result.Status.Code = Controller.StatusCode.NotValidEntity;
                    return result;
                }
                newLocation.PhotoCount += photoCount;
                DataRepository.UpdateLocationsInAllRelations(newId, oldId);
                DataRepository.Update(newLocation);
                DeleteById<Location>(oldId);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                result.Status.Code = Controller.StatusCode.CannotDeserializer;
                result.Status.Message = "Object cannot be deserialized";
            }

            result.Data = true;
            return result;
        }

        private Result<bool> UpdatePhoto(Photo photo, UpdateModel model, PropertyInfo property)
        {
            var result = new Result<bool>();
            object value = model.Value;

            if (model.PropertyName.ToLower() == "eventid" ||
                model.PropertyName.ToLower() == "locationid"
                )
            {
                if (value.ToString() == "0")
                {
                    if (model.PropertyName.ToLower() == "eventid")
                    {
                        photo.Event = null;
                        photo.EventId = null;
                    }
                    else
                    {
                        photo.Location = null;
                        photo.LocationId = null;
                    }

                }
                else
                    SetProperty(property, value, photo);
            }
            else
                SetProperty(property, value, photo);

            var photoIsNew = photo.IsNew;
            photo.IsNew = false;
            DataRepository.Update(photo);
            result.Data = true;

            if (photoIsNew && !photo.IsPrivate)
                NotifyBuddiesAboutNewPhoto(photo);

            return result;
        }

        private Result<bool> UpdateEvent(Event eventToUpdate, UpdateModel model, PropertyInfo property)
        {
            var result = new Result<bool>();
            object value = model.Value;

            if (model.PropertyName.ToLower() == "locationid")
            {
                if (value.ToString() == "0")
                {
                    eventToUpdate.Location = null;
                    eventToUpdate.LocationId = null;
                }
                else
                    SetProperty(property, value, eventToUpdate);
            }
            else
                SetProperty(property, value, eventToUpdate);

            DataRepository.Update(eventToUpdate);
            result.Data = true;

            return result;
        }
        private Result<bool> Update(object jsonModel)
        {
            var result = new Result<bool>();
            try
            {
                var model = JsonConvert.DeserializeObject<UpdateModel>(jsonModel.ToString());
                var type = ModelTypes[model.Type];
                var poco = GetPocoFromType(type, model);

                bool canExecute;
                if (type == typeof(MemberOption))
                {
                    canExecute = CanExecuteOptions(model.Id);
                }
                else
                {
                    // Ausgeschaltet, da Catch-Block vorhanden
                    // ReSharper disable once PossibleNullReferenceException
                    canExecute = (bool)GetType()
                        .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                        .FirstOrDefault(m => m.Name == "CanExecute")
                        .MakeGenericMethod(type).Invoke(this, new object[] { model.Id });
                }

                if (!canExecute)
                {
                    SetNotAuthorizedResult(result);
                    return result;
                }

                var property = poco.GetType()
                    .GetProperties()
                    .First(p => p.Name.ToLower() == model.PropertyName.ToLower());

                var photo = poco as Photo;
                if (photo != null)
                    return UpdatePhoto(photo, model, property);

                var eventToUpdate = poco as Event;
                if (eventToUpdate != null)
                {
                    return UpdateEvent(eventToUpdate, model, property);
                }

                SetProperty(property, model.Value, poco);

                DataRepository.GetType().GetMethod("Update").MakeGenericMethod(type).Invoke(DataRepository, new[] { poco });
                result.Data = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                result.Status.Code = Controller.StatusCode.CannotDeserializer;
                result.Status.Message = "Object cannot be deserialized";
            }

            return result;
        }

        private void NotifyBuddiesAboutNewPhoto(Photo newPhoto)
        {
            try
            {
                var info = DataRepository.GetMinimalPhotoInfo(newPhoto.Id);

                List<int> ids = DataRepository.GetBuddyIdListById(newPhoto.MemberId);

                var members = DataRepository.Queryable<Member>()
                    .Include(m => m.Options)
                    .Include(m => m.Buddies).Where(m => ids.Contains(m.Id) && m.Options.NotifyByEmailOnBuddyAddedPhoto).ToList();

                if (members.Any())
                {
                    CommunicationController.SendNewPhotoNotificationEmail(info, members);
                }
            }
            catch (NullReferenceException)
            {
                //Es gibt keine Buddies
            }
            catch (Exception ex)
            {
                Log.Error((ex));
            }
        }

        private void SetProperty(PropertyInfo property, object value, object poco)
        {


            if (property.PropertyType == typeof(long))
            {
                property.SetValue(poco, long.Parse(value.ToString()), null);
                return;
            }
            if (property.PropertyType == typeof(long?))
            {
                property.SetValue(poco, long.Parse(value.ToString()), null);
                return;
            }
            if (property.PropertyType == typeof(string))
            {
                property.SetValue(poco, value, null);
                return;
            }

            if (property.PropertyType == typeof(int))
            {
                property.SetValue(poco, int.Parse(value.ToString()), null);
                return;
            }

            if (property.PropertyType == typeof(int?))
            {
                if (value != null)
                {
                    property.SetValue(poco, int.Parse(value.ToString()), null);
                }
                else
                {
                    property.SetValue(poco, null, null);
                }
                return;
            }


            if (property.PropertyType == typeof(bool))
            {
                property.SetValue(poco, bool.Parse(value.ToString()), null);
                return;
            }

            if (property.PropertyType == typeof(DateTime?))
            {
                property.SetValue(poco, DateTime.Parse(value.ToString()), null);
                return;
            }

            if (property.PropertyType == typeof(DateTime))
            {
                property.SetValue(poco, DateTime.Parse(value.ToString()), null);
                return;
            }

            if (property.PropertyType.BaseType == typeof(Enum))
            {
                var type = property.PropertyType;
                property.SetValue(poco, Enum.Parse(type, value.ToString(), true), null);
            }

            var info = new CultureInfo("en-US");
            if (property.PropertyType == typeof(decimal))
            {
                property.SetValue(poco, decimal.Parse(value.ToString(), info), null);
                return;
            }

            
            if (property.PropertyType == typeof(decimal?))
            {
                if (value != null)
                {
                    property.SetValue(poco, decimal.Parse(value.ToString(),info),null);
                }
                else
                {
                    property.SetValue(poco, null, null);
                }
                return;
            }

            if (property.PropertyType == typeof(double))
            {
                property.SetValue(poco, double.Parse(value.ToString(), info), null);
                return;
            }

            if (property.PropertyType == typeof(double?))
            {
                property.SetValue(poco, double.Parse(value.ToString(), info), null);
            }
        }

        private object GetPocoFromType(Type type, UpdateModel model)
        {
            var queryable = DataRepository.GetType()
                .GetMethod("Queryable")
                .MakeGenericMethod(type)
                .Invoke(DataRepository, null);


            var method = typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(m => m.Name == "FirstOrDefault" && m.GetParameters().Count() == 2);

            // ReSharper disable once PossibleNullReferenceException
            var genericMethod = method.MakeGenericMethod(type);

            var param = Expression.Parameter(type, "t");
            MemberExpression len;
            len = Expression.PropertyOrField(param, type != typeof(MemberOption) ? "Id" : "MemberId");

            var body = Expression.Equal(len, Expression.Constant(model.Id));
            var lambda = Expression.Lambda(body, param);

            return genericMethod.Invoke(null, new[] { queryable, lambda });
        }

        internal Result<Buddy> AddBuddy(object pocoJson)
        {
            var result = new Result<Buddy>();
            try
            {
                var input = JsonConvert.DeserializeObject<dynamic>(pocoJson.ToString());
                var currentMember = GetMemberFromAuthenticatedUser();
                var currentMemberId = GetMemberId();
                int buddyId = input.BuddyMemberId;

                var existingBuddy =
                    DataRepository.Queryable<Buddy>().FirstOrDefault(
                    b => b.BuddyMemberId == buddyId && b.MemberId == currentMemberId);

                if (existingBuddy != null)
                {
                    result.Data = existingBuddy;
                    return result;
                }

                existingBuddy =
                    DataRepository.Queryable<Buddy>().FirstOrDefault(
                    b => b.BuddyMemberId == currentMemberId && b.MemberId == buddyId);

                var notification = new Notification();

                //kristof: Notification VOR dem Senden, weil es sonst zu Exceptions kommt
                if (existingBuddy != null)
                {
                    existingBuddy.IsMutual = true;
                    DataRepository.Update(existingBuddy);
                    result.Data = existingBuddy;

                    notification.IsUserAmember = true;
                    notification.MemberId = buddyId;
                    notification.Type = NotificationType.BuddyConfirmation;
                    notification.UserAlias = currentMember.Alias;
                    notification.UserAvatarLink = currentMember.Avatar100Url;
                    DataRepository.Add(notification);
                    NotificationHub.PushNotification(notification);

                    SendBuddyConfirmationNotificationEmail(existingBuddy);
                }
                else
                {
                    var buddy = new Buddy()
                    {
                        MemberId = currentMemberId,
                        BuddyMemberId = buddyId,
                        IsMutual = false
                    };

                    DataRepository.Add(buddy);
                    result.Data = buddy;

                    notification.IsUserAmember = true;
                    notification.MemberId = buddyId;
                    notification.Type = NotificationType.BuddyRequest;
                    notification.UserAlias = currentMember.Alias;
                    notification.UserAvatarLink = currentMember.Avatar100Url;
                    DataRepository.Add(notification);
                    NotificationHub.PushNotification(notification);

                    SendNewBuddyNotificationEmail(buddy);
                }


                return result;
            }
            catch (JsonReaderException ex)
            {
                Log.Info("Error on serializing:", ex);
                return CreateNotDeSerializableResult<Buddy>();
            }
            catch (Exception ex)
            {
                Log.Info("Error on save:", ex);
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;
                return result;
            }
        }

        private void SendNewBuddyNotificationEmail(Buddy newBuddy)
        {
            var options = DataRepository.Queryable<Member>().Include(m => m.Options)
                .Where(m => m.Id == newBuddy.BuddyMemberId)
                .Select(m => new { m.Options.NotifyByEmailOnBuddyAdd, m.Options.Language })
                .SingleOrDefault();

            if (options.NotifyByEmailOnBuddyAdd)
            {
                var currentMember = GetMemberFromAuthenticatedUser(true);
                //Kristof: sind bereits gefüllt > currentMember.Options = new MemberOption() { Language = options.Language };

                var memberToNotify =
                    DataRepository.Queryable<Member>()
                        .Include(m => m.Options)
                        .FirstOrDefault(m => m.Id == newBuddy.BuddyMemberId);

                CommunicationController.SendNewBuddyNotificationEmail(memberToNotify, currentMember);
            }
        }
        private void SendBuddyConfirmationNotificationEmail(Buddy existingBuddy)
        {
            var options = DataRepository.Queryable<Member>().Include(m => m.Options)
                .Where(m => m.Id == existingBuddy.MemberId)
                .Select(m => new { m.Options.NotifyByEmailOnBuddyConfirmation, m.Options.Language })
                .SingleOrDefault();

            if (options.NotifyByEmailOnBuddyConfirmation)
            {
                var confirmingBuddy = GetMemberFromAuthenticatedUser(true);
                var memberToNotify =
                    DataRepository.Queryable<Member>().FirstOrDefault(m => m.Id == existingBuddy.MemberId);

                if (memberToNotify != null)
                {
                    memberToNotify.Options = new MemberOption() { Language = options.Language };

                    CommunicationController.SendBuddyConfirmationNotificationEmail(memberToNotify, confirmingBuddy);
                }
            }
        }

        private Result<Buddy> ConfirmBuddy(object pocoJson)
        {
            var result = new Result<Buddy>();
            try
            {
                var input = JsonConvert.DeserializeObject<dynamic>(pocoJson.ToString());

                //kristof: ohne Details laden, sonst gibts eine Exception beim Update, weil in den Datails ebenfalls die Buddies geladen werden
                var currentMember = GetMemberFromAuthenticatedUser();

                var currentMemberId = currentMember.Id;
                int buddyId = input.BuddyMemberId;

                var existingBuddy =
                      DataRepository.Queryable<Buddy>().FirstOrDefault(
                      b => b.BuddyMemberId == currentMemberId && b.MemberId == buddyId);

                if (existingBuddy == null)
                {
                    result.Status.Code = Controller.StatusCode.NoData;
                    result.Status.Message = ResultMessages.NoMatchingData;
                    return result;
                }

                existingBuddy.IsMutual = true;
                DataRepository.Update(existingBuddy);

                var notification = new Notification
                {
                    IsUserAmember = true,
                    MemberId = buddyId,
                    Type = NotificationType.BuddyConfirmation,
                    UserAlias = currentMember.Alias,
                    UserAvatarLink = currentMember.Avatar100Url
                };
                DataRepository.Add(notification);

                result.Data = existingBuddy;

                NotificationHub.PushNotification(notification);
                SendBuddyConfirmationNotificationEmail(existingBuddy);

                return result;
            }
            catch (JsonReaderException ex)
            {
                Log.Info("Error on serializing:", ex);
                return CreateNotDeSerializableResult<Buddy>();
            }
            catch (Exception ex)
            {
                Log.Info("Error on save:", ex);
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;
                return result;
            }
        }
        private Result<T> AddPoco<T>(object pocoJson) where T : class
        {
            var result = new Result<T>();
            try
            {
                var poco = JsonConvert.DeserializeObject<T>(pocoJson.ToString());

                SetMember(poco);
                var errors = Valididate(poco);

                if (errors.Count > 0)
                {
                    result.Status.Code = Controller.StatusCode.NotValidEntity;
                    result.Status.Message = JsonConvert.SerializeObject(errors);
                    return result;
                }

                DataRepository.Add(poco);

                result.Data = poco;
                return result;
            }
            catch (JsonReaderException ex)
            {
                Log.Info("Error on serializing:", ex);
                return CreateNotDeSerializableResult<T>();
            }
            catch (Exception ex)
            {
                Log.Info("Error on save:", ex);
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;
                return result;
            }
        }

        private void SetMember(object poco)
        {
            var memberIdPropertyInfo = poco.GetType().GetProperty("MemberId");
            var memberPropertyInfo = poco.GetType().GetProperty("Member");

            if (memberIdPropertyInfo != null || memberPropertyInfo != null)
            {
                using (var controller = new AccountController(DataRepository, AuthRepository))
                {
                    controller.User = User;
                    var member = controller.GetMemberFromAuthenticatedUser();
                    if (memberIdPropertyInfo != null)
                    {
                        memberIdPropertyInfo.SetValue(poco, member.Id, null);
                    }

                    if (memberPropertyInfo != null)
                    {
                        memberPropertyInfo.SetValue(poco, member, null);
                    }
                }
            }
        }

        private static List<ValidationResult> Valididate<T>(T model, bool isModifiying = false) where T : class
        {
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model), result, true);

            if (isModifiying)
            {
                var poco = model as PocoBase;
                if (poco != null && poco.Id == 0)
                {
                    var idError = new ValidationResult("Id is not set", new[] { "Id" });
                    result.Add(idError);
                }
            }
            //TODO: ParentIds auf richtigen Member überprüfen!
            return result;
        }

        private Result<Photo> UploadPhoto(string filename)
        {
            var result = new Result<Photo>();
            try
            {
                var member = GetMemberWithStorageAccesses();
                var connector = CurrentConnector(member);

                if (connector == null)
                {
                    result.Status.Code = Controller.StatusCode.NoStorageAccessDefined;
                    result.Status.Message = ResultMessages.NotAuthorized;
                    return result;
                }

                if (Request.Content.IsMimeMultipartContent())
                {
                    var stream = GetImageStreamFromPost();
                    var photo = connector.UploadPhoto(stream, filename);
                    result.Data = photo;
                }
                if (result.Data.Id == Constants.NotSetId)
                {
                    DataRepository.Add(result.Data);
                }
                else
                {
                    DataRepository.Update(result.Data);
                }

                var data = new SynchProgress() { Index = 1, TotalFileCount = 1, Photo = result.Data };
                NotificationHub.PushNotification(new Notification() { Data = data, Type = NotificationType.PhotoSynch, Date = DateTime.Now, MemberId = member.Id });
            }
            catch (Exception ex)
            {
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;
                Log.Error(ex);
            }
            return result;
        }

        private Result<Photo> UpdatePhotoWithOriginalPhoto(string idValue)
        {
            var result = new Result<Photo>();
            IConnector connector = null;
            try
            {
                int id = int.Parse(idValue);
                if (!CanExecute<Photo>(id))
                {
                    SetNotAuthorizedResult(result);
                    return result;
                }

                var member = GetMemberWithStorageAccesses();
                connector = CurrentConnector(member);
                var stream = GetImageStreamFromPost();
                var photo =
                    DataRepository.Queryable<Photo>()
                        .Include(p => p.Topics)
                        .Include(p => p.Exif)
                        .Include(p => p.DirectLinks)
                        .FirstOrDefault(p => p.Id == id);

                connector.IsSynchInProgress = true;
                photo = connector.UpdateOrignalPhoto(stream, photo);
                
                DataRepository.Update(photo);

                photo.Member = null;
                var data = new SynchProgress() { Index = 1, TotalFileCount = 1, Photo = result.Data };
                NotificationHub.PushNotification(new Notification()
                {
                    Data = data,
                    Type = NotificationType.PhotoSynch,
                    Date = DateTime.Now,
                    MemberId = member.Id
                });

                result.Data = photo;
            }
            catch (Exception ex)
            {
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;
                Log.Error(ex);
            }
            finally
            {
                if (connector != null)
                    connector.IsSynchInProgress = false;
            }
            return result;
        } 

        private Result<Photo> UpdatePhotoWithNewImage(string idValue)
        {
            var result = new Result<Photo>();
            IConnector connector = null;
            try
            {
                int id = int.Parse(idValue);
                if (!CanExecute<Photo>(id))
                {
                    SetNotAuthorizedResult(result);
                    return result;
                }

                var member = GetMemberWithStorageAccesses();
                connector = CurrentConnector(member);
                var stream = GetImageStreamFromPost();
                var photo =
                    DataRepository.Queryable<Photo>()
                        .Include(p => p.Topics)
                        .Include(p => p.Exif)
                        .Include(p => p.DirectLinks)
                        .FirstOrDefault(p => p.Id == id);

                connector.IsSynchInProgress = true;
                photo = connector.UpdatePhoto(stream, photo);
                photo.Exif.PhotoId = photo.Id;
                //photo.StorageAccessType = connector.ProviderType;

                DataRepository.Update(photo);

                photo.Member = null;
                var data = new SynchProgress() { Index = 1, TotalFileCount = 1, Photo = result.Data };
                NotificationHub.PushNotification(new Notification()
                {
                    Data = data,
                    Type = NotificationType.PhotoSynch,
                    Date = DateTime.Now,
                    MemberId = member.Id
                });

                result.Data = photo;
            }
            catch (Exception ex)
            {
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;
                Log.Error(ex);
            }
            finally
            {
                if (connector != null)
                    connector.IsSynchInProgress = false;
            }
            return result;
        }
        #endregion

        #region Put-Methoden


        private Result<string> ColorReset(int photoId)
        {
            var result = new Result<string>();
            try
            {
                if (CanExecute<Photo>(photoId))
                {
                    var photo = DataRepository.Queryable<Photo>().Include(p => p.DirectLinks).First(p => p.Id == photoId);
                    var link = photo.DirectLinks.ToList()[1];

                    photo.Color = GetMainColor(link.Url);
                    result.Data = photo.Color;
                    DataRepository.Update(photo);

                    return result;
                }
                else
                {
                    return SetNotAuthorizedResult(result);
                }
            }
            catch (Exception ex)
            {
                return LogExceptionAndCreateEmptyResult(ex);
            }

        }

        private string GetMainColor(string imageLink)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(imageLink);
                var response = client.GetAsync(uri).Result;
                var stream = response.Content.ReadAsStreamAsync().Result;
                var imageStream = new MemoryStream();
                stream.CopyTo(imageStream);
                stream.Close();
                stream.Dispose();
                return Processing.GetDominantColor(imageStream);
            }
        }

        private Result<string> ColorReset(bool resetHeader)
        {
            var result = new Result<string>();
            try
            {
                var member = GetMemberFromAuthenticatedUser();
                var link = resetHeader ? member.Header640Url : member.Avatar100Url;
                var color = GetMainColor(link);
                result.Data = color;
                if (resetHeader)
                {
                    member.HeaderColor = color;
                }
                else
                {
                    member.AvatarColor = color;
                }
                DataRepository.Update(member);
            }
            catch (Exception ex)
            {
                return LogExceptionAndCreateEmptyResult(ex);
            }
            return result;
        }


        /// <summary>
        ///     Kapselt das Ändern von einem Objekt
        /// </summary>
        /// <param name="elementType">
        ///     Der Typ des Objects, dass geändert werden soll. Es muss eine textuelle Repräsentation von
        ///     <see cref="PostOrPutAction" /> sein
        /// </param>
        /// <param name="pocoJson">Das Object als Json-Format</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public object Put([FromUri] string elementType, [FromBody] dynamic pocoJson)
        {
            try
            {
                switch (elementType.ToLower())
                {
                    case PostOrPutAction.Brick:
                        {
                            var value = pocoJson["Type"].Value.ToLower();

                            if (value.Equals(BrickType.Text.ToString().ToLower()))
                            {
                                return ModifyPoco<TextBrick>(pocoJson);
                            }
                            if (value.Equals(BrickType.Map.ToString().ToLower()))
                            {
                                return ModifyPoco<MapBrick>(pocoJson);
                            }
                            if (value.Equals(BrickType.Photo.ToString().ToLower()))
                            {
                                Result<PhotoBrick> result = ModifyPoco<PhotoBrick>(pocoJson);
                                var storyId = DataRepository.GetStoryIdByPhotoBrickId(result.Data.Id);
                                DataRepository.UpdateCountsForStory(storyId);
                                return result;
                            }

                            return CreateNotDeSerializableResult<object>();
                        }
                    case PostOrPutAction.Member:
                        return ModifyPoco<Member>(pocoJson);
                    case PostOrPutAction.Chapter:
                        return ModifyPoco<Chapter>(pocoJson);
                    case PostOrPutAction.Event:
                        return ModifyPoco<Event>(pocoJson);
                    case PostOrPutAction.Exif:
                        return UpdateExif(pocoJson);
                    case PostOrPutAction.Ledge:
                        return ModifyPoco<Ledge>(pocoJson);
                    case PostOrPutAction.Location:
                        return ModifyPoco<Location>(pocoJson);
                    case PostOrPutAction.Photo:
                        return ModifyPoco<Photo>(pocoJson);
                    case PostOrPutAction.Story:
                        return ModifyPoco<Story>(pocoJson);
                    case PostOrPutAction.Topic:
                        return ModifyPoco<Topic>(pocoJson);
                    case PostOrPutAction.SocialMedia:
                        return ModifyPoco<SocialMedia>(pocoJson);
                    case PostOrPutAction.Thumbs:
                        return UpdateThumbNails(pocoJson);
                    case PostOrPutAction.ColorReset:
                        int id = Convert.ToInt32(pocoJson);
                        return ColorReset(id);
                    case PostOrPutAction.ColorResetheader:
                        return ColorReset(true);
                    case PostOrPutAction.ColorResetAvatar:
                        return ColorReset(false);
                    case PostOrPutAction.MultiUpdate:
                        return UpdateMultiplePhotos(pocoJson);
                    default:
                        return GetUnknownActionResult<object>();
                }
            }
            catch (Exception ex)
            {
                return LogExceptionAndCreateEmptyResult(ex);
            }
        }

        private Result<bool> UpdateMultiplePhotos(object pocoJson)
        {
            var model = JsonConvert.DeserializeObject<MultiUpdateModel>(pocoJson.ToString());

            var result = new Result<bool>();
            try
            {
                result.Data = true;
                if (model.PhotoIds == null || model.PhotoIds.Count == 0)
                    return result;

                if (!IsValidUpdateModel(model))
                    return CreateInvalidEntityResult(result);

                DataRepository.UpdateMultiplePhotos(model);
            }
            catch (Exception ex)
            {
                Log.Info("Error on save:", ex);
                result.Data = false;
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;

                LogExceptionAndCreateEmptyResult(ex);
                return result;
            }

            return result;
        }

        private bool IsValidUpdateModel(MultiUpdateModel model)
        {
            var currentMember = GetMemberFromAuthenticatedUser();
            var currentMemberId = currentMember.Id;

            var ids = model.PhotoIds.ToArray();

            //Gehören die Fotos dem aktuellen Benutzer?
            var idCount = DataRepository.Queryable<Photo>().Count(p => ids.Contains(p.Id) && p.MemberId == currentMemberId);

            if (idCount != model.PhotoIds.Count)
            {
                return false;
            }

            var isValidLocation = true;
            //Gehört der Ort dem aktuellen Benutzer?
            if (model.LocationId != Constants.NotSetId)
                isValidLocation = ElementBelongsToMember<Location>(currentMemberId, model.LocationId);

            if (!isValidLocation) return false;

            var isValidEvent = true;
            //Gehört das Ereigniss dem aktuellen Benutzer?
            if (model.EventId != Constants.NotSetId)
                isValidEvent = ElementBelongsToMember<Event>(currentMemberId, model.EventId);

            if (!isValidEvent) return false;

            //Gehören die Ereignisse dem aktuellen Benutzer?
            if (model.Topics != null && model.Topics.Count > 0)
            {
                ids = model.Topics.ToArray();
                idCount = DataRepository.Queryable<Topic>().Count(p => ids.Contains(p.Id) && p.MemberId == currentMemberId);
                if (idCount != model.Topics.Count)
                {
                    return false;
                }
            }

            return true;
        }

        private static Result<bool> CreateInvalidEntityResult(Result<bool> result)
        {
            result.Status.Code = Controller.StatusCode.NotValidEntity;
            result.Status.Message = ResultMessages.NotValidEntity;
            result.Data = false;
            return result;
        }

        private Result<ExifData> UpdateExif(object pocoJson)
        {
            var result = new Result<ExifData>();
            try
            {
                result = DeserializeAndValidate<ExifData>(pocoJson);
                if (result.Status.Code != Controller.StatusCode.Success)
                    return result;

                var poco = result.Data;

                if (!CanExecute<Photo>(poco.PhotoId))
                {
                    return SetNotAuthorizedResult(result);
                }
                DataRepository.Add(poco);
                result.Data = poco;
                return result;
            }
            catch (Exception ex)
            {
                Log.Info("Error on save:", ex);
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;
                return result;
            }
        }

        private Result<T> ModifyPoco<T>(object pocoJson) where T : PocoBase
        {
            var result = new Result<T>();
            try
            {
                result = DeserializeAndValidate<T>(pocoJson);
                if (result.Status.Code != Controller.StatusCode.Success)
                    return result;

                var poco = result.Data;

                var id = 0;
                var basePoco = poco as PocoBase;
                if (basePoco != null) id = basePoco.Id;
                if (!CanExecute<T>(id))
                {
                    return SetNotAuthorizedResult(result);
                }

                //kristof: da komm ich nie hin, weil ich 
                //         über Update feldweise speichere 
                //----------------------------------------
                if (poco is Photo)
                {
                    var photo = poco as Photo;
                    photo.IsNew = false;
                }

                DataRepository.Update(poco);
                result.Data = poco;
                return result;
            }
            catch (Exception ex)
            {
                Log.Info("Error on save:", ex);
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;
                return result;
            }
        }


        private Result<T> DeserializeAndValidate<T>(object pocoJson) where T : class
        {
            var result = new Result<T>();
            try
            {
                var poco = JsonConvert.DeserializeObject<T>(pocoJson.ToString());
                var errors = Valididate(poco, true);
                result.Data = poco;

                if (errors.Count > 0)
                {
                    result.Status.Code = Controller.StatusCode.NotValidEntity;
                    result.Status.Message = JsonConvert.SerializeObject(errors);
                    return result;
                }
            }
            catch (JsonReaderException ex)
            {
                Log.Info("Error on serializing:", ex);
                return CreateNotDeSerializableResult<T>();
            }
            return result;
        }


        private Result<Photo> UpdateThumbNails(object json)
        {
            var result = new Result<Photo>();
            try
            {
                var model = JsonConvert.DeserializeObject<ThumbModel>(json.ToString());
                if (!CanExecute<Photo>(model.PhotoId))
                {
                    return SetNotAuthorizedResult(result);
                }
                const int firstNotSquareSize = 480;
                var photo = DataRepository.Queryable<Photo>().Include(p => p.DirectLinks).First(p => p.Id == model.PhotoId);
                var links = photo.DirectLinks.Where(l => l.Size < firstNotSquareSize).ToArray();

                var client = new HttpClient();
                var fullSizelink = links.FirstOrDefault(l => l.Size == 0);
                if (fullSizelink != null)
                {
                    var uri = new Uri(fullSizelink.Url);
                    var response = client.GetAsync(uri).Result;
                    var stream = response.Content.ReadAsStreamAsync().Result;

                    var member = GetMemberWithStorageAccesses();
                    var connector = CurrentConnector(member);

                    var urls = connector.ChangeSquareImages(stream, photo.Folder, model.XPercentage, model.YPercentage,true);
                    var sizes = new[] { 400, 200, 100 };
                    for (var i = 0; i < sizes.Length; i++)
                    {
                        var link = links.First(l => l.Size == sizes[i]);
                        DataRepository.Delete(link);
                        link.Url = urls[i];                        
                        DataRepository.Add(link);
                    }
                    photo.Top = model.YPercentage;
                    photo.Left = model.XPercentage;
                    photo.IsNew = false;
                    DataRepository.Update(photo);
                }
                result.Data = photo;
                result.Data.Member = null;
                result.Data = photo;
            }
            catch (JsonReaderException ex)
            {
                Log.Info("Error on serializing:", ex);
                return CreateNotDeSerializableResult<Photo>();
            }
            catch (Exception ex)
            {
                Log.Info("Error on save:", ex);
                result.Status.Code = Controller.StatusCode.InternalException;
                result.Status.Message = ResultMessages.UnhandledException;
                return result;
            }

            return result;
        }

        #endregion

        #region Aktualisierungsmethoden

        #endregion
    }
}