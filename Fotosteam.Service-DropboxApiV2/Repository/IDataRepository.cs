using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository.Poco;

namespace Fotosteam.Service.Repository
{
    public interface IDataRepository
    {
        Rating AddRating(Rating newRating);
        Comment AddComment(Comment newComment);
        int GetIdForAlias(string alias);
        Member GetMemberByAlias(string alias, bool includeOptions = false);
        Member GetMemberById(int id, bool includeOptions = false);
        List<Member> GetBuddiesByAlias(string alias);
        List<Member> GetBuddiesById(int id);
        List<int> GetBuddyIdListById(int id);
        List<Photo> GetPhotos(string alias, int skip, int take, bool includePrivate);
        List<Photo> GetPhotosByTopic(string alias, string topic, bool includePrivate, int skip, int take);
        List<Photo> GetRandomPhotos(string alias, int take, bool includePrivate);
        Photo GetPhotoById(string alias, int id, bool includePrivate);

        List<Photo> GetPhotosByLocation(string alias, string location, bool includePrivate, string type, int skip,
            int take);

        List<Category> GetCategories(string alias, bool includePrivate);
        List<Topic> GetTopics(string alias, bool includePrivate);
        List<Location> GetLocations(string alias, bool includePrivate);
        List<Event> GetEvents(string alias, bool includePrivate);
        List<Story> GetStories(string alias, bool includePrivate);
        List<Photo> GetPhotosByCategory(string alias, string category, bool includePrivate, int skip, int take);
        List<Photo> GetPhotosByEvent(string alias, string eventName, bool includePrivate, int skip, int take);
        List<Photo> GetNewPhotos(int memberId);
        List<NewPhoto> GetLatestPhotos(int skip, int take, int memberId = 0);
        List<NewPhoto> GetTopRatedPhotos(string alias, int skip, int take, int memberId = 0);
        List<FilteredPhoto> GetCC0Photos(string alias, int skip, int take, IEnumerable<KeyValuePair<string, string>> filter);
        void Add<T>(T entity) where T : class;
        void AddNewMember(Member newMember, MemberLanguage language);
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Detach<T>(T entity) where T : class;
        T Find<T>(params object[] keyvalues) where T : class;
        T Find<T>(Expression<Func<T, bool>> predicate) where T : class;
        IQueryable<T> Queryable<T>() where T : class;
        int Execute(string sql, params object[] parameters);
        int GetStoryIdByChapterId(int chapterId);
        int GetStoryIdByLedgeId(int ledgeId);
        int GetStoryIdByPhotoBrickId(int photoBrickId);
        void UpdateCountsForStory(int storyId);
        void DeleteMember(Member member);
        MinimalPhotoInfo GetMinimalPhotoInfo(int photoId);
        List<GroupedNotification> GetNotifications(int skip, int take, bool onlyNotRead, int memberId);
        bool UpdateAvatarInAllRelations(string alias, string avatarLink);
        bool UpdateLocationsInAllRelations(int newId, int oldId);
        bool UpdatePhotoTopic(int photoId, int topicId, bool isAdd);
        void DeletePhotoByFolder(string path);

        bool UpdateMultiplePhotos(MultiUpdateModel model);
    }
}