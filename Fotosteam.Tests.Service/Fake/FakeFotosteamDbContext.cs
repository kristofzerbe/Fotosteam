using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Fotosteam.Service.Repository.Context;
using Fotosteam.Service.Repository.Poco;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fotosteam.Tests.Service.Fake
{
  
    public class FakeFotosteamDbContext : IFotosteamDbContext
    {
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<DirectLink> DirectLinks { get; set; }
        public IDbSet<Event> Events { get; set; }
        public IDbSet<Location> Locations { get; set; }
        public IDbSet<Topic> Topics { get; set; }
        public IDbSet<Member> Members { get; set; }
        public IDbSet<MemberStorageAccess> MemberStorageAccesses { get; set; }
        public IDbSet<SocialMedia> MemberSocialMedias { get; set; }
        public IDbSet<MemberOption> MemberOption { get; set; } 
        public IDbSet<Buddy> MemberBuddies { get; set; }
        public IDbSet<Photo> Photos { get; set; }
        public IDbSet<ExifData> Exifs { get; set; }
        public IDbSet<Rating> Ratings { get; set; }
        public IDbSet<StorageProvider> StorageProviders { get; set; }
        public IDbSet<Story> Stories { get; set; }
        public IDbSet<Chapter> Chapters { get; set; }
        public IDbSet<Ledge> Ledges { get; set; }
        public IDbSet<TextBrick> TextBricks { get; set; }
        public IDbSet<MapBrick> MapBricks { get; set; }
        public IDbSet<PhotoBrick> PhotoBricks { get; set; }
        public IDbSet<Notification> Notifications { get; set; }

        private static ILog Log = LogManager.GetLogger(typeof(FakeFotosteamDbContext));
        private static readonly Dictionary<string,object > DbSets = new Dictionary<string, object>();

        /// <summary>
        /// Die Methode wird genutzt, um sicherzustellen, dass immer die aktuellen Benutzer mit allen Daten gelanden sind
        /// </summary>
        internal static void ReloadMembers()
        {
            lock (Lock)
            {
                DbSets.Remove("members");
                CreateFakeSet<Member>("members");
            }
        }
        private static readonly object Lock = new object();
        public FakeFotosteamDbContext()        {
            lock (Lock)
            {
                Log.Debug("Setze die FakeDBSets");
                LoadContextDataFromFiles();             
            }
        }

        internal void LoadContextDataFromFiles()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            DbSets.Clear();
            DirectLinks = CreateFakeSet<DirectLink>("direktLinks");
            Events = CreateFakeSet<Event>("events");            
            Locations = CreateFakeSet<Location>("locations");
            Topics = CreateFakeSet<Topic>("topics");
            Members = CreateFakeSet<Member>("members");
            MemberStorageAccesses = CreateFakeSet<MemberStorageAccess>("memberStorageAccesses");
            MemberSocialMedias = CreateFakeSet<SocialMedia>("membersocialMedia");
            MemberBuddies = CreateFakeSet<Buddy>("memberBuddies");
            MemberOption = CreateFakeSet<MemberOption>("memberOptions");
            Photos = CreateFakeSet<Photo>("photos");
            Exifs= CreateFakeSet<ExifData>("exifs");
            Ratings = CreateFakeSet<Rating>("ratings");
            StorageProviders = CreateFakeSet<StorageProvider>("storageProviders");
            Stories = CreateFakeSet<Story>("stories");
            Chapters = CreateFakeSet<Chapter>("chapters");
            Ledges = CreateFakeSet<Ledge>("ledges");
            Comments = CreateFakeSet<Comment>("comments");
            TextBricks = CreateFakeSet<TextBrick>("textbricks");
            MapBricks = CreateFakeSet<MapBrick>("mapbricks");
            PhotoBricks = CreateFakeSet<PhotoBrick>("photobricks");
            Notifications = new FakeDbSet<Notification>();
            DbSets.Add("notifications.json", Notifications);

            AddMembers();
        }
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

        private static  FakeDbSet<T> CreateFakeSet<T>(string fileName) where T:class
        {
            lock (Lock)
            {
                if (DbSets.ContainsKey(fileName))
                {
                    Log.Debug(string.Format("FakeDBSets<{0}> aus Dictionary", fileName));
                    return (FakeDbSet<T>) DbSets[fileName];
                }
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    PreserveReferencesHandling = PreserveReferencesHandling.All
                };

                Log.Debug(string.Format("Erzeige FakeDBSets<{0}>", fileName));
                var dcr = new DefaultContractResolver();
                dcr.DefaultMembersSearchFlags |= BindingFlags.NonPublic;
                settings.ContractResolver = dcr;

                var json = File.ReadAllText(CurrentPath + @"\TestData\" + fileName + ".json");
                var list = JsonConvert.DeserializeObject<List<T>>(json, settings);
                var set = new FakeDbSet<T>();
                foreach (var item in list)
                {
                    set.Add(item);
                }

                DbSets.Add(fileName, set);
                return set;
            }
        }

        private  void AddMembers()
        {
            foreach (var photo in Photos )
            {
                photo.Member = Members.FirstOrDefault(m => m.Id == photo.MemberId);
            }
        }
        public int SaveChanges()
        {
            Log.Debug(string.Format("SaveChanges"));
            return 0;
        }

        public void Dispose()
        {
            lock(Lock)
            {
                DbSets.Clear();
            }
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            lock (Lock)
            {
                Log.Debug(string.Format("Ermittele Set<{0}>", typeof (TEntity)));
                foreach (var item in DbSets)
                {
                    if (item.Value is DbSet<TEntity>)
                    {
                        var set= (DbSet < TEntity>) item.Value;
                        Log.Debug(string.Format("Set<{0}> mit {1} Elementen gefunden", typeof(TEntity), set.Count()));
                        return set;
                    }
                }
                
                throw new Exception("Type collection not found");
            }
        }

        public DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public bool IsStateEqual<T>(T entity, EntityState state) where T : class
        {
            return true;
        }

        public Database Database { get; private set; }


        public void SetState<T>(T entity, EntityState state) where T:class
        {
            //wird nicht genutzt;
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            const int robertMemberId = 3;
            if (
                sql.Equals(
                    "SELECT M.ID FROM MEMBER M INNER JOIN AspNetUsers U on M.AspNetUserId=U.ID INNER JOIN AspNetUserLogins L ON U.ID=L.USERID WHERE LOWER(L.ProviderKey)={0} AND LOWER(U.UserName)={1}"))
                return robertMemberId;

            return 0;
        }

        public T SingleSqlQuery<T>(string sql, params object[] parameters)
        {
            object  storyId = 5;
            if (sql.Contains("select storyid"))
                return (T) storyId ;

            return default(T);
        }

        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            object storyId = 5;
            if (sql.Contains("select storyid"))
                return new List<T>{ (T)storyId};

            return new List<T>{default(T)};
        }
    }
}