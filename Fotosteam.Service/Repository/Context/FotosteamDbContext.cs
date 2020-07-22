using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Fotosteam.Service.Repository.Configuration;

namespace Fotosteam.Service.Repository.Context
{
    /// <summary>
    /// Definiert den Kontext für die Daten von Fotosteam über das Entity Framework
    /// </summary>
    public class FotosteamDbContext : DbContext, IFotosteamDbContext
    {
        static FotosteamDbContext()
        {
            Database.SetInitializer<FotosteamDbContext>(null);
        }

        public FotosteamDbContext()
            : base("Name=FotosteamDbContext")
        {}

        public FotosteamDbContext(string connectionString)
            : base(connectionString)
        {}

        public FotosteamDbContext(string connectionString, DbCompiledModel model)
            : base(connectionString, model)
        {}


        /// <summary>
        /// Ändert den Status einer Entität
        /// </summary>
        /// <typeparam name="T">Typ der Entität</typeparam>
        /// <param name="entity">Das Objekt das persistiert werden soll</param>
        /// <param name="state">Der neue Status des Objekts</param>
        /// <remarks>Das ist notwendig, da man bei Webprojekte keinen Status vorhält</remarks>
        public void SetState<T>(T entity, EntityState state) where T : class
        {
            Entry(entity).State = state;
        }

        /// <summary>
        /// Gibt an ob das Objekt einen entsprechende Status hat
        /// Das ist nur notwendig, da man DbEntityEntry nicht mocken kann
        /// </summary>
        /// <typeparam name="T">Typ der Entität</typeparam>
        /// <param name="entity">Das Objekt das persistiert werden soll</param>
        /// <param name="state">Der zu überprüfende Status des Objekts</param>
        /// <returns></returns>
        public bool IsStateEqual<T>(T entity, EntityState state) where T : class
        {
            return Equals(Entry(entity).State ==state);
        }

        /// <summary>
        /// FÜhrt ein SQL-Anweisung aus, die keine Rückgabewerte liefert
        /// </summary>
        /// <param name="sql">Das auszuführende SQL-Statement</param>
        /// <param name="parameters">Array mit den Werten für die Parameter, die im SQL String mit {[0]..[n]}, definiert werden müssen
        /// z.B. ExecuteSqlCommand("Update Photo SET TITLE = {0} WHERE Id ={1}", "Neuer Titel", 1)
        /// </param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sql, parameters);
        }

        /// <summary>
        /// Liefert ein Objekt aus einem SQL-Statement zurück
        /// </summary>
        /// <typeparam name="T">Das Objekt, das erzeugt werden soll</typeparam>
        /// <param name="sql">SQL-Statement</param>
        /// <param name="parameters">Werte der möglichen Parameter</param>
        /// <returns></returns>
        public T SingleSqlQuery<T>(string sql, params object[] parameters)
        {
            return Database.SqlQuery<T>(sql, parameters).FirstOrDefault();
        }

        /// <summary>
        /// Liefert ein Objekt aus einem SQL-Statement zurück
        /// </summary>
        /// <typeparam name="T">Das Objekt, das erzeugt werden soll</typeparam>
        /// <param name="sql">SQL-Statement</param>
        /// <param name="parameters">Werte der möglichen Parameter</param>
        /// <returns></returns>
        public IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters)
        {
            return Database.SqlQuery<T>(sql, parameters);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            AddConfigurations(modelBuilder);
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            AddConfigurations(modelBuilder, schema);
            return modelBuilder;
        }

        private static void AddConfigurations(DbModelBuilder modelBuilder, string schema = "dbo")
        {
            modelBuilder.Configurations.Add(new DirectLinkConfiguration(schema));
            modelBuilder.Configurations.Add(new EventConfiguration(schema));
            modelBuilder.Configurations.Add(new LocationConfiguration(schema));
            modelBuilder.Configurations.Add(new MemberConfiguration(schema));
            modelBuilder.Configurations.Add(new MemberStorageAccessConfiguration(schema));
            modelBuilder.Configurations.Add(new MemberSocialMediaConfiguration(schema));
            modelBuilder.Configurations.Add(new MemberOptionConfiguration(schema));
            modelBuilder.Configurations.Add(new BuddyConfiguration(schema));
            modelBuilder.Configurations.Add(new PhotoConfiguration(schema));
            modelBuilder.Configurations.Add(new RatingConfiguration(schema));
            modelBuilder.Configurations.Add(new StorageProviderConfiguration(schema));
            modelBuilder.Configurations.Add(new ExifDataConfiguration(schema));
            modelBuilder.Configurations.Add(new TopicConfiguration(schema));
            modelBuilder.Configurations.Add(new StoryConfiguration(schema));
            modelBuilder.Configurations.Add(new StoryForPhotoConfiguration());
            modelBuilder.Configurations.Add(new ChapterConfiguration(schema));
            modelBuilder.Configurations.Add(new LedgeConfiguration(schema));
            modelBuilder.Configurations.Add(new BrickConfiguration(schema));
            modelBuilder.Configurations.Add(new TextBrickConfiguration(schema));
            modelBuilder.Configurations.Add(new MapBrickConfiguration(schema));
            modelBuilder.Configurations.Add(new PhotoBrickConfiguration(schema));
            modelBuilder.Configurations.Add(new NotificationConfiguration(schema));
        }
    }
}