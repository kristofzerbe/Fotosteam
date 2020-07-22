using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Moq;
using Moq.Language.Flow;
using Newtonsoft.Json;

namespace Fotosteam.Tests.Service
{
    internal class RepositoryHelper
    {
        private static Mock<IFotosteamRepository> _mockRepository;

        internal static IFotosteamRepository MockRepository
        {
            get
            {
                if (_mockRepository != null)
                    return _mockRepository.Object;

                SetupRepository();
                return _mockRepository.Object;
            }
        }

        private static void SetupRepository()
        {
            _mockRepository = new Mock<IFotosteamRepository>();
            AddComment();
            AddRating();
            SetUpData();
            SetupGetPhotos();
            SetUpGetAlias();
            SetGetPhotosByTopic();
            SetupGetPhotosByLocation();
            SetupGetPhotosByEvent();
            SetupGetPhotosByCategory();
            SetupQueryable();
            SetUpGetCategories();
        }

        private static int GetAlias(string alias)
        {
            return alias.ToLower() == "robert" ? 3 : 2;
        }

        private static void SetUpGetAlias()
        {
            _mockRepository.Setup(r => r.GetIdForAlias(It.IsAny<string>())).Callback<string>((alias) => GetAlias(alias));

        }

        private static void SetupQueryable()
        {
            _mockRepository.Setup(r => r.Queryable<Photo>()).Returns(Photos.AsQueryable());
            _mockRepository.Setup(r => r.Queryable<Member>()).Returns(Members.AsQueryable());
            _mockRepository.Setup(r => r.Queryable<Location>()).Returns(Locations.AsQueryable());
            _mockRepository.Setup(r => r.Queryable<Event>()).Returns(Events.AsQueryable());
            _mockRepository.Setup(r => r.Queryable<Topic>()).Returns( Topics.AsQueryable());
            _mockRepository.Setup(r => r.Queryable<Story>()).Returns(Stories.AsQueryable());

        }

        private static void SetUpGetCategories()
        {
            _mockRepository.Setup(r => r.GetCategories(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns<string,bool>((alias, includePrivate) =>
                {
                    return new List<Category>();
                }
                );
        }

        private static void SetGetPhotosByTopic()
        {
            _mockRepository.Setup(r => r.GetPhotosByTopic(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns<string, string, bool>(
                    (alias, criteria, includePrivate) =>
                    {
                        var memberId = GetAlias(alias);
                        var photos =
                            Photos.Where(
                                x => x.MemberId == memberId && x.Topics.Any(t => t.Name.ToLower() == criteria.ToLower()))
                                .ToList();

                        if (!includePrivate)
                            photos = photos.Where(p => p.IsPrivate == false).ToList();

                        return photos;
                    }
                );
        }

        private static void SetupGetPhotosByLocation()
        {
            _mockRepository.Setup(r => r.GetPhotosByLocation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns<string, string, bool>(
                    (alias, criteria, includePrivate) =>
                    {
                        var memberId = GetAlias(alias);
                        var photos =
                            Photos.Where(
                                x => x.MemberId == memberId && x.Location != null && x.Location.Name.ToLower() == criteria.ToLower())
                                .ToList();

                        if (!includePrivate)
                            photos = photos.Where(p => p.IsPrivate == false).ToList();

                        return photos;
                    }
                );
        }

        private static void SetupGetPhotosByEvent()
        {
            _mockRepository.Setup(r => r.GetPhotosByEvent(  It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns<string, string, bool>(
                    (alias, criteria, includePrivate) =>
                    {
                        var memberId = GetAlias(alias);
                        var photos =
                            Photos.Where(
                                x => x.MemberId == memberId && x.Event != null && x.Event.Name.ToLower() == criteria.ToLower())
                                .ToList();

                        if (!includePrivate)
                            photos = photos.Where(p => p.IsPrivate == false).ToList();

                        return photos;
                    }
                );
        }

        private static void SetupGetPhotosByCategory()
        {
            _mockRepository.Setup(r => r.GetPhotosByCategory( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns<string, string, bool>(
                    (alias, criteria, includePrivate) =>
                    {
                        var memberId = GetAlias(alias);
                        var photos =
                            Photos.Where(
                                x => x.MemberId == memberId && x.Categories != null && x.Categories.Any(c => c.Type.ToLower() == criteria.ToLower()))
                                .ToList();

                        if (!includePrivate)
                            photos = photos.Where(p => p.IsPrivate == false).ToList();

                        return photos;
                    }
                );
        }


        private static void SetupGetPhotos()
        {
            _mockRepository.Setup(r => r.GetPhotos(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns<string, int, int, bool>(
                    (alias, skip, take, includePrivate) =>
                    {
                        var memberId = GetAlias(alias);
                        var photos =
                            Photos.Where(x => x.MemberId == memberId)
                                .Skip(skip)
                                .Take(take)
                                .ToList();

                        if (!includePrivate)
                            photos = photos.Where(p => p.IsPrivate == false).ToList();

                        return photos;
                    }
                );
        }

        private static int _commentCount;

        private static int CommentCount
        {
            get
            {
                _commentCount += 1;
                return _commentCount;
            }
        }

        private static void AddComment()
        {

            _mockRepository.Setup(r => r.AddComment(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns<string, int, string>(
                (alias, id, comment) =>
                new Comment
                {
                    TotalCount = CommentCount,
                    CommentId = 1,
                    Date = DateTime.Now,
                    PhotoId = id,
                    Text = comment,
                    UserAlias = alias
                }
            );
        }

        private static void AddRating()
        {
            _mockRepository.Setup(r => r.AddRating(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns<string, int, int>(
                (alias, id, rate) =>
                    new Rating
                    {
                        UserAlias = alias,
                        AverageRating = GetAverage(rate),
                        PhotoId = rate,
                        Value = rate

                    }
                );
        }

        private static readonly List<int> Ratings = new List<int>();
        
        private static string _currentPath;
        internal static string CurrentPath
        {
            get
            {
                if (string.IsNullOrEmpty(_currentPath))
                    _currentPath = AppDomain.CurrentDomain.BaseDirectory;

                return _currentPath;
            }        
        }

        private static List<Photo> Photos { get; set; }
        private static List<Topic> Topics{ get; set; }
        private static List<Event> Events{ get; set; }
        private static List<Location> Locations{ get; set; }
        private static List<Story> Stories{ get; set; }
        private static List<Member> Members{ get; set; }
        
        private static void SetUpData()
        {
            Photos = LoadList<Photo>("photos.json");
            Topics = LoadList<Topic>("topics.json");
            Events = LoadList<Event>("events.json");
            Locations = LoadList<Location>("locations.json");
            Members = LoadList<Member>("members.json");
            Stories = LoadList<Story>("stories.json");                        
        }

        

        private static List<T> LoadList<T>(string fileName)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var json= System.IO.File.ReadAllText(CurrentPath + @"\TestData\"+fileName  );
            return JsonConvert.DeserializeObject<List<T>>(json,settings);
        }


        private static decimal GetAverage(int rate)
        {
            Ratings.Add(rate);

            var result = (decimal)Ratings.Sum() / Ratings.Count;
            return result;
        }
    }
}
