using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Fotosteam.Service.Repository.Context
{
    /// <summary>
    /// Definiert die allmeinen Funktionen des Kontext,
    /// damit man das auch mocken kann, was für die Unittests wichtig ist
    /// </summary>
    public interface IFotosteamDbContext : IDisposable
    {
        /// <summary>
        /// Referenz zur Datenbank
        /// </summary>
        Database Database { get; }
        /// <summary>
        /// Set mit den Objekten
        /// </summary>
        /// <typeparam name="TEntity">Typ des Objkts das persistiert werden kann</typeparam>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        /// <summary>
        /// Ein DBEntityEntry-Objekt, das den Status zu einem EF-Objekt erlaubt
        /// </summary>
        /// <typeparam name="TEntity">Typ des Objkts das persistiert werden kann</typeparam>
        /// <param name="entity">Das Objekt das persistiert werden aknn</param>
        /// <returns></returns>
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// FÜhrt die Speicherung der Daten durch
        /// </summary>
        int SaveChanges();
        /// <summary>
        /// Ändert den Status einer Entität
        /// </summary>
        /// <typeparam name="T">Typ der Entität</typeparam>
        /// <param name="entity">Das Objekt das persistiert werden soll</param>
        /// <param name="state">Der neue Status des Objekts</param>
        /// <remarks>Das ist notwendig, da man bei Webprojekte keinen Status vorhält</remarks>
        void SetState<T>(T entity, EntityState state) where T : class;
        /// <summary>
        /// Gibt an ob das Objekt einen entsprechende Status hat
        /// Das ist nur notwendig, da man DbEntityEntry nicht mocken kann
        /// </summary>
        /// <typeparam name="T">Typ der Entität</typeparam>
        /// <param name="entity">Das Objekt das persistiert werden soll</param>
        /// <param name="state">Der zu überprüfende Status des Objekts</param>
        /// <returns></returns>
        bool IsStateEqual<T>(T entity, EntityState state) where T : class;

        /// <summary>
        /// FÜhrt ein SQL-Anweisung aus, die keine Rückgabewerte liefert
        /// </summary>
        /// <param name="sql">Das auszuführende SQL-Statement</param>
        /// <param name="parameters">Array mit den Werten für die Parameter, die im SQL String mit {[0]..[n]}, definiert werden müssen
        /// z.B. ExecuteSqlCommand("Update Photo SET TITLE = {0} WHERE Id ={1}", "Neuer Titel", 1)
        /// </param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);
        T SingleSqlQuery<T>(string sql, params object[] parameters);

        IEnumerable<T> SqlQuery<T>(string sql, params object[] parameters);
    }
}