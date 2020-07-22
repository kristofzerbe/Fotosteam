using Fotosteam.Service.Models;
using Fotosteam.Service.Repository.Context;
using Fotosteam.Service.Repository.Poco;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LinqKit;

namespace Fotosteam.Service.Repository
{
    public class DataRepository : IDataRepository
    {
        private static Dictionary<string, int> _members;
        private static readonly Object Lock = new Object();
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IFotosteamDbContext _context;

        public DataRepository()
            : this(new FotosteamDbContext())
        {
        }

        public DataRepository(IFotosteamDbContext context)
        {
            _context = context;
        }

        public Rating AddRating(Rating newRating)
        {
            var rating = Queryable<Rating>()
                .Where(x => x.PhotoId == newRating.PhotoId)
                .GroupBy(x => x.PhotoId).Select(r => new
                {
                    Sum = r.Sum(x => x.Value),
                    Count = r.Count()
                }
            ).FirstOrDefault();

            var ratingValue = rating == null ? 0 : rating.Sum;
            var newRatingValue = ratingValue + newRating.Value;

            //Erst versuchen Rating zu speichern...                                
            Add(newRating);

            //... und wenn keine PRIMARY KEY-Exception kommt (doppelter Eintrag), dann in Foto eintragen
            Execute("Update Photo SET RatingSum ={0} WHERE Id={1}", newRatingValue, newRating.PhotoId);

            //Rückgabe der Summe für die GUI
            newRating.RatingSum = newRatingValue;

            return newRating;
        }

        public Comment AddComment(Comment newComment)
        {
            var photo = Queryable<Photo>().FirstOrDefault(p => p.Id == newComment.PhotoId);
            if (photo != null)
            {
                var commenCount = photo.CommentCount + 1;
                photo.CommentCount = commenCount;
                newComment.TotalCount = commenCount;

                Update(photo);
                Add(newComment);

                return newComment;
            }

            return null;
        }


        public int GetIdForAlias(string alias)
        {
            int id;
            lock (Lock)
            {
                if (_members == null)
                {
                    _members = new Dictionary<string, int>();
                }

                if (!_members.TryGetValue(alias, out id))
                {
                    var member = GetMemberByAlias(alias);
                    if (member == null)
                    {
                        return Constants.NotFound;
                    }
                    id = member.Id;

                    _members.Add(alias, id);
                }
            }

            return id;
        }

        private void AddBuddies(Member member)
        {
            if (member == null)
                return;

            var buddies = Queryable<Buddy>().Where(b => b.BuddyMemberId == member.Id).ToList();
            foreach (var buddy in buddies)
            {
                member.Buddies.Add(buddy);
            }

        }
        public Member GetMemberByAlias(string alias, bool includeOptions = false)
        {
            return GetMember(alias, Constants.NotSetId, includeOptions);
        }

        public Member GetMemberById(int id, bool includeOptions = false)
        {
            return GetMember(string.Empty, id, includeOptions);
        }

        private Member GetMember(string alias, int id, bool includeOptions = false)
        {
            alias = alias.ToLower();
            var query = Queryable<Member>()
                .Include(m => m.HomeLocation)
                .Include(m => m.Buddies)
                .Include(m => m.SocialMedias);
            if (includeOptions)
                query.Include(m => m.Options);

            Member member;

            if (!string.IsNullOrEmpty(alias))
                member = query.FirstOrDefault(x => x.Alias.ToLower() == alias);
            else
                member = query.FirstOrDefault(x => x.Id == id);

            AddBuddies(member);
            return member;
        }

        public List<Member> GetBuddiesByAlias(string alias)
        {
            var memberId =
                Queryable<Member>()
                    .Where(m => m.Alias.ToLower().Equals(alias.ToLower()))
                    .Select(m => m.Id)
                    .SingleOrDefault();

            return GetBuddiesById(memberId);
        }

        public List<int> GetBuddyIdListById(int id)
        {
            var isBuddyIds = Queryable<Buddy>().Where(b => b.MemberId == id).Select(b => b.BuddyMemberId).ToList();
            var hasBuddiesId = Queryable<Buddy>().Where(b => b.BuddyMemberId == id).Select(b => b.MemberId).ToList();

            var ids = new List<int>();

            if (isBuddyIds.Any())
                ids.AddRange(isBuddyIds);

            if (hasBuddiesId.Any())
            {
                foreach (var currentId in hasBuddiesId)
                {
                    if (!ids.Contains(currentId))
                        ids.Add(currentId);
                }
            }
            return ids;
        }

        public List<Member> GetBuddiesById(int id)
        {
            List<int> ids = GetBuddyIdListById(id);
            return GetBuddies(ids);
        }

        public List<Photo> GetPhotos(string alias, int skip, int take, bool includePrivate)
        {
            var photos = GetBaseQueryForPhotos(alias, includePrivate);


            if (!includePrivate)
                photos = photos.Where(p => p.ShowInOverview);

            if (take == 0)
            {
                take = int.MaxValue;
            }

            var retVal = photos
                .OrderBy(p => p.Id)
                .Include(p => p.Exif)
                .Include(p => p.Location)
                .Include(p => p.Event)
                .Include(p => p.Topics)
                .Skip(skip)
                .Take(take)
                .ToList();

            var topics = GetTopics(alias, includePrivate);
            foreach (var photo in retVal)
            {
                foreach (var topic in photo.Topics)
                {
                    var topicForCount = topics.FirstOrDefault(t => t.Id == topic.Id);
                    if (topicForCount != null)
                        topic.PhotoCount = topicForCount.PhotoCount;
                }
            }
            AddStoriesToPhotos(retVal);
            return retVal;
        }

        private void AddStoriesToPhotos(List<Photo> photos)
        {
            if (photos == null || !photos.Any())
                return;

            var ids = photos.Select(p => p.Id).ToArray();
            var list = _context.SqlQuery<StoryForPhoto>("SELECT * FROM PhotosInStories WHERE PhotoId in (" + String.Join(",", ids) +
                                              ")").ToList();

            if (!list.Any())
                return;

            foreach (var photo in photos)
            {
                var stories = list.Where(s => s.PhotoId == photo.Id).ToList();
                if (stories.Any())
                {
                    photo.Stories = new List<Story>();
                    foreach (var story in stories)
                    {
                        photo.Stories.Add(StoryForPhotoToStory(story));
                    }
                }
            }

        }

        private Story StoryForPhotoToStory(StoryForPhoto storyForPhoto)
        {
            var story = new Story
            {
                ChapterCount = storyForPhoto.ChapterCount,
                Id = storyForPhoto.Id,
                MemberId = storyForPhoto.MemberId,
                HeaderPhoto = storyForPhoto.HeaderPhoto,
                Chapters = storyForPhoto.Chapters,
                IsPrivate = storyForPhoto.IsPrivate,
                Name = storyForPhoto.Name,
                PhotoCount = storyForPhoto.PhotoCount,
                HeaderPhotoId = storyForPhoto.HeaderPhotoId,
            };
            if (story.HeaderPhoto != null)
            {
                story.HeaderPhoto.Member = null;
            }
            return story;
        }

        public List<Photo> GetPhotosByTopic(string alias, string topic, bool includePrivate, int skip, int take)
        {
            var query = GetBaseQueryForPhotos(alias, includePrivate);

            if (!string.IsNullOrEmpty(topic))
                query = query.Include(p => p.Topics).Where(p => p.Topics.Any(t => t.Name.ToLower() == topic.ToLower()));

            query = AddSkipAndTakeTo(skip, take, query);

            var list = query.ToList();
            AddStoriesToPhotos(list);
            return list;
        }

        private static IQueryable<Photo> AddSkipAndTakeTo(int skip, int take, IQueryable<Photo> query)
        {
            if (skip > 0)
                query = query.Skip(skip);
            if (take > 0)
                query = query.Take(take);
            return query;
        }

        public List<Photo> GetRandomPhotos(string alias, int take, bool includePrivate)
        {
            var query = GetBaseQueryForPhotos(alias, includePrivate);
            if (!includePrivate)
                query = query.Where(p => p.ShowInOverview);

            query = query.OrderBy(m => Guid.NewGuid());


            var list = query.Take(take).ToList();
            AddStoriesToPhotos(list);
            return list;
        }

        public List<Photo> GetPhotosByLocation(string alias, string location, bool includePrivate, string type, int skip, int take)
        {
            var query = GetBaseQueryForPhotos(alias, includePrivate).Include(p => p.Location);

            if (!string.IsNullOrEmpty(location))
            {
                switch (type)
                {
                    case GetAction.ByLocation:
                        query =
                            query.Where(
                                p => p.Location != null && p.Location.Name.ToLower().Contains(location.ToLower()));
                        break;
                    case GetAction.ByCity:
                        query =
                            query.Where(
                                p =>
                                    p.Location != null && !string.IsNullOrEmpty(p.Location.City) &&
                                    p.Location.City.ToLower().Contains(location.ToLower()));
                        break;
                    case GetAction.ByCountry:
                        query =
                            query.Where(
                                p =>
                                    p.Location != null && !string.IsNullOrEmpty(p.Location.Country) &&
                                    p.Location.Country.ToLower().Contains(location.ToLower()));
                        break;
                }
            }

            query = AddSkipAndTakeTo(skip, take, query);

            if (query.Any())
            {
                var list = query.ToList();
                AddStoriesToPhotos(list);
                return list;
            }

            return null;
        }

        /// <summary>
        ///     Liefert alle nicht bearbeiteten Bilder einer Benutzers zurück
        /// </summary>
        /// <param name="memberId">Die Id des Benutzers zu dem die Bilder gesucht werden</param>
        /// <returns>Liste aller nicht beaarbeiten Fotos</returns>
        public List<Photo> GetNewPhotos(int memberId)
        {
            return Queryable<Photo>().Include(p => p.DirectLinks).Where(p => p.MemberId == memberId && p.IsNew).ToList();
        }

        /// <summary>
        /// Liefert die neusten Fotos aller Benutzer zurück, die öffentlich sind     
        /// </summary>
        /// <param name="skip">Anzahl Datensätze, die übersprungen werden sollen</param>
        /// <param name="take">Anzahl Datensätze, die zurück geliefert werden sollen</param>
        /// <param name="memberId">Optional die Id des Benutzers</param>
        /// <returns></returns>
        public List<NewPhoto> GetLatestPhotos(int skip, int take, int memberId = 0)
        {
            var query =
                Queryable<Photo>()
                    .Include(p => p.Member)
                    .Include(p => p.DirectLinks)
                    .Include(p => p.Location)
                    .Where(p => p.IsPrivate == false && p.IsNew == false && p.IsForStoryOnly == false && p.ShowInOverview);

            if (memberId != Constants.NotSetId)
                query = query.Where(m => m.MemberId == memberId);

            var list = query.OrderByDescending(p => p.PublishDate)
            .Skip(skip)
            .Take(take)
                    .Select(n => new NewPhoto()
                    {
                        Id = n.Id,
                        Name = n.Name,
                        Title = n.Title,
                        PublishDate = n.PublishDate ?? DateTime.Now,
                        DirectLinks = n.DirectLinks,
                        Color = n.Color,
                        Location = (n.Location == null ? string.Empty : n.Location.Name),
                        RatingSum = n.RatingSum,
                        Alias = n.Member.Alias,
                        PlainName = n.Member.PlainName,
                        Avatar100Url = n.Member.Avatar100Url,
                        Avatar200Url = n.Member.Avatar200Url
                    })
                    .ToList();

            return list;
        }

        public List<NewPhoto> GetTopRatedPhotos(string alias, int skip, int take, int memberId = Constants.NotSetId)
        {
            var list =
                Queryable<Photo>()
                    .Include(p => p.Member)
                    .Include(p => p.DirectLinks)
                    .Include(p => p.Location);

            //Nur Bilder eines Nutzers ermitteln?
            if (!alias.ToLower().Equals(Constants.ForAll.ToLower()))
            {
                list = list.Where(p => p.Member.Alias.ToLower().Equals(alias.ToLower()));
            }

            //Bilder ermitteln, die von einem bestimmten Benutzer bewertet wruden?
            if (memberId != Constants.NotSetId)
                list = list.Include(p => p.Ratings).Where(p => p.Ratings.Any(r => r.MemberId == memberId));

            var result = list
                    .Where(p => p.IsPrivate == false)
                    .OrderByDescending(p => p.RatingSum)
                    .ThenByDescending(p => p.PublishDate)
                    .Skip(skip)
                    .Take(take)
                    .Select(n => new NewPhoto()
                    {
                        Id = n.Id,
                        Name = n.Name,
                        Title = n.Title,
                        PublishDate = n.PublishDate ?? DateTime.Now,
                        DirectLinks = n.DirectLinks,
                        Color = n.Color,
                        Location = (n.Location == null ? string.Empty : n.Location.Name),
                        RatingSum = n.RatingSum,
                        Alias = n.Member.Alias,
                        PlainName = n.Member.PlainName,
                        Avatar100Url = n.Member.Avatar100Url,
                        Avatar200Url = n.Member.Avatar200Url,
                    })
                    .ToList();

            return result;
        }

        public List<FilteredPhoto> GetCC0Photos(string alias, int skip, int take, IEnumerable<KeyValuePair<string, string>> filter)
        {
            var list =
                Queryable<Photo>()
                    .Include(p => p.Member)
                    .Include(p => p.DirectLinks)
                    .Include(p => p.Location);


            if (filter != null)
            {
                var filterItems = filter.ToList();
                if (filterItems.Any())
                {
                    var predicate =
                        AddCategoryCriterias(filterItems.FirstOrDefault(x => x.Key.ToLower() == GetAction.ByCatergory));

                    var topics = filterItems.FirstOrDefault(x => x.Key.ToLower() == GetAction.ByTopic);
                    if (topics.Key != null)
                    {
                        list = list.Include(p => p.Topics);
                        if (predicate == null) predicate = PredicateBuilder.False<Photo>();

                        predicate =
                            predicate.Or(p => p.Topics.Any(t => t.Name.ToLower().Contains(topics.Value.ToLower())));
                    }

                    if (predicate != null)
                        list = list.Where(predicate);
                }
            }

            list = list.Where(p => p.IsPrivate == false && (int)p.License == (int)LicenseType.CcZero);
            if (!alias.ToLower().Equals(Constants.ForAll.ToLower()))
            {
                list = list.Where(p => p.Member.Alias.ToLower().Equals(alias.ToLower()));
            }

            list = list.OrderByDescending(p => p.PublishDate).ThenByDescending(p => p.RatingSum);

            list = list.Skip(skip);

            if (take > 0)
                list = list.Take(take);

            var photos = list.ToList();

            return photos.Select(photo => new FilteredPhoto()
            {
                Id = photo.Id,
                Name = photo.Name,
                Title = photo.Title,
                PublishDate = photo.PublishDate ?? DateTime.Now,
                DirectLinks = photo.DirectLinks,
                Color = photo.Color,
                Location = (photo.Location == null ? string.Empty : photo.Location.Name),
                RatingSum = photo.RatingSum,
                Category = photo.Category,
                Categories = photo.Categories,
                Topics = photo.Topics,
                Alias = photo.Member.Alias,
                PlainName = photo.Member.PlainName,
                Avatar100Url = photo.Member.Avatar100Url,
                Avatar200Url = photo.Member.Avatar200Url,
            }).ToList();
        }

        private static Expression<Func<Photo, bool>> AddCategoryCriterias(KeyValuePair<string, string> categories)
        {
            if (categories.Key != null)
            {
                var predicate = PredicateBuilder.False<Photo>();
                var values = categories.Value.Split('|');
                if (values.Length > 0)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        var value = (CategoryType)Enum.Parse(typeof(CategoryType), values[i]);
                        predicate = predicate.Or(p => (p.Category & value) == value);
                    }
                    return predicate;
                }
                else
                {
                    var value = (CategoryType)Enum.Parse(typeof(CategoryType), categories.Value);
                    predicate = predicate.Or(p => (p.Category & value) == value);
                    return predicate;
                }
            }
            return null;
        }


        public Photo GetPhotoById(string alias, int id, bool includePrivate)
        {
            var memberId = GetIdForAlias(alias);
            var photo = Queryable<Photo>().FirstOrDefault(p => p.MemberId == memberId && p.Id == id);
            if (!includePrivate && photo != null && photo.IsPrivate)
                photo = null;

            return photo;
        }

        public List<Category> GetCategories(string alias, bool includePrivate)
        {
            var photos = GetBaseQueryForPhotos(alias, includePrivate);

            if (!photos.Any())
            {
                return null;
            }

            var distinctList = new Dictionary<string, int[]>();
            const int countColumn = 0;
            const int typeColumn = 1;

            var loadedPhotos = photos.ToList(); //Dies ist notwendig, da Categories auf der DB nicht vorhanden 
            foreach (var category in loadedPhotos.SelectMany(photo => photo.Categories))
            {
                if (distinctList.ContainsKey(category.Type))
                {
                    distinctList[category.Type][0] += 1;
                }
                else
                {
                    var values = new[] { 1, category.TypeValue };
                    distinctList.Add(category.Type, values);
                }
            }

            return distinctList.Select(d => new Category { Type = d.Key, PhotoCount = d.Value[countColumn], TypeValue = d.Value[typeColumn] }).ToList();
        }

        public List<Topic> GetTopics(string alias, bool includePrivate)
        {
            var photos = GetBaseQueryForPhotos(alias, includePrivate).Include(p => p.Topics);
            var result = new List<Topic>();

            if (photos != null && photos.Any())
            {
                var distinctList = new Dictionary<string, Topic>();
                var topics = photos.ToList().Where(p => p.Topics != null).SelectMany(p => p.Topics);
                foreach (var topic in topics)
                {
                    if (distinctList.ContainsKey(topic.Name))
                    {
                        distinctList[topic.Name].PhotoCount += 1;
                    }
                    else
                    {
                        topic.PhotoCount = 1;
                        topic.Member = null;
                        distinctList.Add(topic.Name, topic);
                    }
                }
                result = distinctList.Select(d => d.Value).ToList();
                if (!includePrivate)
                    return result.OrderByDescending(t => t.PhotoCount).ThenBy(t => t.Name).ToList();
            }

            var memberId = GetIdForAlias(alias);
            var allTopics = Queryable<Topic>().Where(t => t.MemberId == memberId);
            foreach (var currentTopic in allTopics)
            {
                if (result.All(t => t.Id != currentTopic.Id))
                {
                    result.Add(currentTopic);
                }

            }
            return result.OrderByDescending(t => t.PhotoCount).ThenBy(t => t.Name).ToList();
        }

        public List<Location> GetLocations(string alias, bool includePrivate)
        {
            var photos = GetBaseQueryForPhotos(alias, includePrivate).Include(p => p.Location).ToList();
            var locations = new List<Location>();

            if (photos.Any())
            {
                var groups = photos.GroupBy(p => p.LocationId).ToList();
                foreach (var group in groups)
                {
                    var location = @group.First().Location ?? new Location { Name = "NotSet" };

                    location.PhotoCount = group.Count();
                    locations.Add(location);
                }

                if (!includePrivate)
                    return locations.OrderByDescending(l => l.PhotoCount).ThenBy(l => l.Name).ToList();
            }

            var memberId = GetMemberByAlias(alias).Id;

            var allLocations = Queryable<Location>().Where(l => l.MemberId == memberId).ToList();

            foreach (var currentLocation in allLocations)
            {

                if (locations.All(l => l.Id != currentLocation.Id))
                {
                    locations.Add(currentLocation);
                }
            }
            return locations.OrderByDescending(l => l.PhotoCount).ThenBy(l => l.Name).ToList();
        }

        public List<Event> GetEvents(string alias, bool includePrivate)
        {
            var photos =
                GetBaseQueryForPhotos(alias, includePrivate).Include(p => p.Event).Include(p => p.Location).ToList();

            var events = new List<Event>();
            if (photos.Any())
            {
                foreach (var group in photos.GroupBy(p => p.EventId))
                {
                    var currentEvent = @group.First().Event ?? new Event { Name = "NotSet" };

                    currentEvent.PhotoCount = group.Count();
                    events.Add(currentEvent);
                }
                if (!includePrivate)
                    return events.OrderByDescending(e => e.PhotoCount).ThenBy(e => e.Name).ToList();
            }

            var memberId = GetMemberByAlias(alias).Id;
            var allEvents = Queryable<Event>().Where(e => e.MemberId == memberId);
            foreach (var currentEvent in allEvents)
            {
                if (events.All(e => e.Id != currentEvent.Id))
                {
                    events.Add(currentEvent);
                }
            }
            return events.OrderByDescending(e => e.PhotoCount).ThenBy(e => e.Name).ToList();
        }

        public List<Story> GetStories(string alias, bool includePrivate)
        {
            var memberId = GetIdForAlias(alias);
            var query =
                Queryable<Story>()
                    .Where(s => s.MemberId == memberId)
                    .Include(s => s.HeaderPhoto.DirectLinks)
                    .Include(s => s.Chapters.Select(c => c.Ledges.Select(l => l.Bricks)));

            if (!includePrivate)
                query = query.Where(s => !s.IsPrivate);

            var list = query.ToList();

            if (list.Count == 0)
                return list;

            var photoBricks =
                list.SelectMany(s => s.Chapters.SelectMany(c => c.Ledges.SelectMany(l => l.Bricks)))
                    .OfType<PhotoBrick>()
                    .OrderBy(b => b.Id)
                    .ToList();

            var photoCount = photoBricks.Count;

            if (photoCount == 0)
            {
                ClearPrivateElementsFromStories(list, includePrivate);
                return list;
            }

            var photoBricksWithPhotos = Queryable<Story>()
                .Where(s => s.MemberId == memberId)
                .SelectMany(s => s.Chapters.SelectMany(c => c.Ledges.SelectMany(l => l.Bricks))).OfType<PhotoBrick>()
                .Include(b => b.Photo.DirectLinks).OrderBy(b => b.Id).ToList();

            for (var i = 0; i < photoCount; i++)
            {

                photoBricks[i].Photo = photoBricksWithPhotos[i].Photo;

                if (photoBricks[i].Photo != null)
                    photoBricks[i].Photo.Member = null;
            }

            ClearPrivateElementsFromStories(list, includePrivate);
            foreach (var story in list)
            {
                story.PhotoCount = story.Chapters.SelectMany(c => c.Ledges.SelectMany(l => l.Bricks)).OfType<PhotoBrick>().Count();
                story.ChapterCount = story.Chapters.Count;
            }
            return list;
        }

        private void ClearPrivateElementsFromStories(List<Story> stories, bool includePrivate)
        {
            if (includePrivate)
            {
                return;
            }

            foreach (var story in stories)
            {
                if (story.HeaderPhoto != null)
                {
                    if (story.HeaderPhoto.IsPrivate)
                    {
                        story.HeaderPhoto = null;
                    }
                    else
                    {
                        story.HeaderPhoto.Member = null;
                        story.HeaderPhoto.Stories = null;
                    }
                }
                story.Member = null;
                if (story.Chapters != null)
                    story.Chapters = story.Chapters.Except(story.Chapters.Where(c => c.IsPrivate)).ToList();
            }

        }


        public List<Photo> GetPhotosByCategory(string alias, string category, bool includePrivate, int skip, int take)
        {
            var query = GetBaseQueryForPhotos(alias, includePrivate).Include(p => p.Location);

            CategoryType categoryType;
            if (Enum.TryParse(category, true, out categoryType))
            {

                if (!string.IsNullOrEmpty(category))
                    query = query.Where(p => (p.Category & categoryType) == categoryType);

                query = AddSkipAndTakeTo(skip, take, query);
            }
            else
            {
                query = query.Where(p => p.Id == Constants.NotSetId);
            }

            var list = query.ToList();
            AddStoriesToPhotos(list);
            return list;
        }

        public List<Photo> GetPhotosByEvent(string alias, string eventName, bool includePrivate, int skip, int take)
        {
            var query = GetBaseQueryForPhotos(alias, includePrivate).Include(p => p.Event).Include(p => p.Location);

            if (!string.IsNullOrEmpty(eventName))
                query = query.Where(p => p.Event != null && p.Event.Name.ToUpper().Contains(eventName.ToUpper()));

            query = AddSkipAndTakeTo(skip, take, query);

            var list = query.ToList();
            return list;
        }

        public int GetStoryIdByChapterId(int chapterId)
        {
            return SqlQuery<int>("select storyid from Chapter where id = {0}", chapterId);
        }

        public int GetStoryIdByLedgeId(int ledgeId)
        {
            return
                SqlQuery<int>("select storyid from Chapter c inner join Ledge l on l.ChapterId= c.Id where l.id = {0}",
                    ledgeId);
        }

        public int GetStoryIdByPhotoBrickId(int photoBrickId)
        {
            return
                SqlQuery<int>(
                    "select storyid from Chapter c inner join Ledge l on l.ChapterId= c.Id inner join Brick b on b.LedgeId = l.Id where b.id = {0}",
                    photoBrickId);
        }

        public void UpdateCountsForStory(int storyId)
        {
            var chapterSql =
                @"update story set ChapterCount = (select count(1) from Chapter where StoryId ={0}) where Id ={0}";

            Execute(chapterSql, storyId);

            var photoSql = @"update story set PhotoCount = (
select distinct count(photoid) from Brick where LedgeId  in 
( Select l.Id from Ledge l inner join Chapter c on c.Id = l.ChapterId
inner join Story s on s.Id = c.StoryId where s.Id ={0}) and type='photo')
where Id = {0}
";
            Execute(photoSql, storyId);
        }

        private List<Member> GetBuddies(IEnumerable<int> ids)
        {
            return Queryable<Member>().Include(m => m.Buddies).Where(m => ids.Contains(m.Id)).ToList();
        }

        private IQueryable<Photo> GetBaseQueryForPhotos(string alias, bool includePrivate)
        {
            var query = Queryable<Photo>().Include(p => p.DirectLinks);
            if (!string.IsNullOrEmpty(alias) && !alias.ToLower().Equals(Constants.ForAll.ToLower()))
            {
                var memberId = GetIdForAlias(alias);
                query = query.Where(p => p.MemberId == memberId);
            }

            if (!includePrivate)
                query = query.Where(p => !p.IsPrivate);

            return query;
        }

        public MinimalPhotoInfo GetMinimalPhotoInfo(int photoId)
        {

            var info = Queryable<Member>().Include(m => m.Options)
                .Join(Queryable<Photo>().Include(p => p.DirectLinks), m => m.Id, p => p.MemberId,
                (m, p) => new MinimalPhotoInfo
                {
                    Id = p.Id,
                    Name = p.Name,
                    Title = p.Title,
                    Url640 = p.DirectLinks.FirstOrDefault(d => d.Size == 640).Url,
                    Member = m
                }).FirstOrDefault(i => i.Id == photoId);

            if (info == null)
                return null;

            info.Member.Options = Queryable<MemberOption>().FirstOrDefault(m => m.MemberId == info.Member.Id);

            if (info.Member.Options == null)
            {
                info.Member.Options = new MemberOption { MemberId = info.Member.Id };
            }

            return info;
        }

        public List<GroupedNotification> GetNotifications(int skip, int take, bool onlyNotRead, int memberId)
        {
            var query = Queryable<Notification>()
                .Where(n => n.MemberId == memberId)
                .GroupJoin(Queryable<Photo>().Where(p => p.MemberId == memberId),
                    n => n.PhotoName,
                    p => p.Name,
                    (n, p) =>
                    new { Nofication = n, Photo = p.FirstOrDefault() }
                );

            if (onlyNotRead)
                query = query.Where(n => !n.Nofication.IsRead);
            var list = query.OrderByDescending(n => n.Nofication.Date).Take(take).Skip(skip)
                .GroupBy(n => new { n.Nofication.Type, n.Nofication.PhotoName })
                .Select(k => new GroupedNotification
                {
                    Type = k.Key.Type,
                    Notifications = k.Select(x => x.Nofication).ToList(),
                    PhotoUrl = k.Select(x =>
                        // ReSharper disable once PossibleNullReferenceException
                        (x.Photo == null ? string.Empty : x.Photo.DirectLinks.FirstOrDefault(d => d.Size == 100).Url)
                        ).FirstOrDefault(),
                    PhotoTitle = k.Select(x => x.Photo == null ? string.Empty : x.Photo.Title).FirstOrDefault()
                }).ToList();

            foreach (var group in list)
            {
                foreach (var item in group.Notifications)
                {
                    item.PhotoUrl = group.PhotoUrl;
                    item.PhotoTitle = group.PhotoTitle;
                }
            }
            return list;
        }

        public bool UpdateAvatarInAllRelations(string alias, string avatarLink)
        {
            Execute("UPDATE Comments SET UserAvatarLink = {0} WHERE UserAlias = {1}", avatarLink, alias);
            Execute("UPDATE Rating SET UserAvatarLink = {0} WHERE UserAlias = {1}", avatarLink, alias);
            return true;
        }

        public bool UpdateLocationsInAllRelations(int newId, int oldId)
        {
            Execute("Update Photo SET LocationId = {0} WHERE LocationId = {1} ", newId, oldId);
            Execute("Update Event SET LocationId = {0} WHERE LocationId = {1} ", newId, oldId);
            Execute("Update Member SET HomeLocation_Id = {0} WHERE HomeLocation_Id= {1}", newId, oldId);

            return true;
        }

        public bool UpdatePhotoTopic(int photoId, int topicId, bool isAdd)
        {
            if (isAdd)
            {
                if (!Queryable<Photo>().Include(p => p.Topics).Any(p => p.Id == photoId && p.Topics.Any(t => t.Id == topicId)))
                    Execute("INSERT INTO PhotoTopic (PhotoId,TopicId) VALUES ({0},{1})", photoId, topicId);
            }
            else
            {
                Execute("DELETE FROM PhotoTopic WHERE PhotoId ={0} AND TopicId={1}", photoId, topicId);
            }
            return true;
        }

        public bool UpdateMultiplePhotos(MultiUpdateModel model)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var photoIds = string.Join(",", model.PhotoIds.ToArray());

                    if (model.ToBeDeleted)
                    {
                        //Execute("DELETE FROM Photo WHERE ID IN (" + photoIds + ")");
                        //dbContextTransaction.Commit();
                        return true;
                    }

                    var fieldsAndValues = new Dictionary<string, object>();
                    var updateStatement = BuildUpdateStatement(model, ref fieldsAndValues);

                    if (!string.IsNullOrEmpty(updateStatement))
                        Execute(updateStatement, fieldsAndValues.Values.ToArray());

                    if ((model.Fields & MultiEditField.Topics) == MultiEditField.Topics)
                    {
                        if (model.ReplaceExistingValues)
                            Execute("DELETE FROM PhotoTopic WHERE PhotoId in (" + photoIds + ")");

                        foreach (var photoId in model.PhotoIds)
                        {
                            foreach (var topicId in model.Topics)
                            {
                                Execute("INSERT INTO PhotoTopic (PhotoId,TopicId) VALUES ({0},{1})", photoId, topicId);
                            }
                        }
                    }
                    if (!model.ReplaceExistingValues)
                    {
                        if (MultiEditField.License == (MultiEditField.License & model.Fields) && model.License != LicenseType.None)
                        {
                            Execute("UPDATE Photo SET License = License | {0} WHERE ID IN (" + model.PhotoIds + ")",
                                model.License);
                        }
                        if (MultiEditField.Category == (MultiEditField.Category & model.Fields) && model.Category != CategoryType.NotSet)
                        {
                            Execute("UPDATE Photo SET Category = Category | {0} WHERE ID IN (" + model.PhotoIds + ")",
                                model.Category);
                        }
                    }

                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        internal string BuildUpdateStatement(MultiUpdateModel model, ref Dictionary<string, object> fieldsAndValues)
        {
            var sql = string.Empty;
            var index = 0;
            var setStatement = new List<string>();

            foreach (MultiEditField value in Enum.GetValues(typeof(MultiEditField)))
            {
                if (value == (model.Fields & value))
                {
                    var fieldAndValue = AddToFieldsAndValues(value, model);
                    if (!string.IsNullOrEmpty(fieldAndValue.Key))
                    {
                        var currentValue = fieldAndValue.Value;

                        if (value == MultiEditField.EventId
                            || value == MultiEditField.LocationId
                            || value == MultiEditField.License)
                        {
                            if ((int)currentValue == 0)
                                currentValue = null;
                            else
                            {
                                currentValue = (int)currentValue;
                            }
                        }
                        fieldsAndValues.Add(fieldAndValue.Key, currentValue);
                        setStatement.Add(string.Format("{0}={{{1}}}", fieldAndValue.Key, index));
                        index++;
                    }
                }
            }


            if (fieldsAndValues.Count == 0)
                return sql;
            sql = string.Format("UPDATE PHOTO SET {0} WHERE ID IN ({1})", string.Join(",", setStatement), string.Join(",", model.PhotoIds));

            return sql;
        }

        private KeyValuePair<string, object> AddToFieldsAndValues(MultiEditField value, MultiUpdateModel model)
        {
            var fieldAndValue = new KeyValuePair<string, object>();
            if ((!model.ReplaceExistingValues & (value == MultiEditField.Category))
                || value == MultiEditField.Topics)
                return fieldAndValue;

            var fieldName = value.ToString();
            var field = model.GetType().GetProperty(fieldName);

            fieldAndValue = new KeyValuePair<string, object>(fieldName, field.GetValue(model, null));

            if (field.PropertyType == typeof(bool))
            {
                if ((bool)fieldAndValue.Value)
                {
                    fieldAndValue = new KeyValuePair<string, object>(fieldName, 1);
                }
                else
                {
                    fieldAndValue = new KeyValuePair<string, object>(fieldName, 0);
                }
            }

            return fieldAndValue;
        }

        #region Allgemeine Methoden

        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void AddNewMember(Member newMember, MemberLanguage language)
        {
            Add(newMember);
            Execute("INSERT INTO MemberOption (MemberId) VALUES ({0})", newMember.Id);
            Execute("UPDATE MemberOption SET Language = {0} WHERE MemberId ={1}", (int)language, newMember.Id);
        }

        public void Update<T>(T entity) where T : class
        {
            if (IsStateEqual(entity, EntityState.Detached))
                _context.Set<T>().Attach(entity);

            _context.SetState(entity, EntityState.Modified);
            _context.SaveChanges();
        }

        public void Delete<T>(T entity) where T : class
        {
            if (!IsStateEqual(entity, EntityState.Detached))
                _context.Set<T>().Attach(entity);

            if (entity is Event)
                SetEventToNull(entity as Event);

            if (entity is Location)
                SetLocationToNullForRelatedEntities(entity as Location);

            if (entity is Photo)
                SetPhotoToNullForRelatedEntities(entity as Photo);

            _context.Set<T>().Remove(entity);
            _context.SetState(entity, EntityState.Deleted);
            _context.SaveChanges();
        }

        private void SetEventToNull(Event tobeDeletedEvent)
        {
            Execute("UPDATE PHOTO SET EVENTID=NULL WHERE EVENTID={0}", tobeDeletedEvent.Id);
        }

        private void SetLocationToNullForRelatedEntities(Location tobeDeletedLocation)
        {
            if (tobeDeletedLocation == null) return;

            var id = tobeDeletedLocation.Id;
            Execute("UPDATE MEMBER SET HomeLocation_Id=NULL WHERE HomeLocation_Id={0}", id);
            Execute("UPDATE PHOTO SET LocationId=NULL WHERE LocationId={0}", id);
            Execute("UPDATE EVENT SET LocationId=NULL WHERE LocationId={0}", id);
        }
        private void SetPhotoToNullForRelatedEntities(Photo tobeDeletedPhoto)
        {
            if (tobeDeletedPhoto == null) return;
            var id = tobeDeletedPhoto.Id;
            Execute("UPDATE STORY SET HeaderPhotoId=NULL WHERE HeaderPhotoId={0}", id);
            Execute("UPDATE Brick SET PhotoId=NULL WHERE PhotoId={0}", id);
        }
        public bool IsStateEqual<T>(T entity, EntityState state) where T : class
        {
            return _context.IsStateEqual(entity, state);
        }

        public void DeletePhotoByFolder(string folder)
        {
            var photo = Queryable<Photo>().FirstOrDefault(p => p.Folder.ToLower().Equals(folder.ToLower()));
            if (photo != null)
                Delete(photo);

        }
        public void DeleteMember(Member member)
        {
            var alias = member.Alias;
            var buddies = Queryable<Buddy>().Where(b => b.BuddyMemberId == member.Id);
            foreach (var buddy in buddies)
            {
                Delete(buddy);
            }
            Delete(member);
            _members.Remove(alias);
        }

        public void Detach<T>(T entity) where T : class
        {
            _context.SetState(entity, EntityState.Detached);
        }

        public T Find<T>(params object[] keyvalues) where T : class
        {
            return _context.Set<T>().Find(keyvalues);
        }

        public T Find<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Queryable<T>().FirstOrDefault(predicate);
        }

        public IQueryable<T> Queryable<T>() where T : class
        {
            return _context.Set<T>().AsNoTracking<T>() ;
        }


        public int Execute(string sql, params object[] parameters)
        {
            return _context.ExecuteSqlCommand(sql, parameters);
        }

        public T SqlQuery<T>(string sql, params object[] parameters)
        {
            return _context.SingleSqlQuery<T>(sql, parameters);
        }

        #endregion
    }
}