using System.Collections.Generic;
using System.Reflection;
using log4net;

namespace Fotosteam.Service.Controller
{
    /// <summary>
    /// Singleton, um die aktuelle ausgeführten Synchronisierungen aufgrund von Webhooks
    /// zu synchronisieren. Das Singleton muss über ThreadPool-Grenzen hinweg übergeben werden,
    /// da mit jedem Aufrauf von Dropbox ein neues ThreadPool aufgemacht wird
    /// </summary>
    public sealed class WebHookSession
    {
        // ReSharper disable once InconsistentNaming
        private static readonly WebHookSession _instance = new WebHookSession();
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private WebHookSession() { }

        private static readonly HashSet<string> CurrentSynchs = new HashSet<string>();
        private object _lock = new object();

        /// <summary>
        /// Erlaubt den Zugriff auf das Singleton
        /// </summary>
        public static WebHookSession Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Fügt einen Benutzer der Session hinzu
        /// </summary>
        /// <param name="userId">Id des Benutzers</param>
        public void AddUserToSession(string userId)
        {
            lock (_lock)
            {
                Log.Info(string.Format("Adding {0} to session. Count:{1}", userId, CurrentSynchs.Count));
                if (!CurrentSynchs.Contains(userId))
                    CurrentSynchs.Add(userId);
            }
        }

        /// <summary>
        /// Löscht einen Benutzer aus der Session 
        /// </summary>
        /// <param name="userId">Id des Benutzers</param>
        public void RemoveUserFromSession(string userId)
        {
            lock (_lock)
            {
                Log.Info(string.Format("Removing {0} from session. Count:{1}", userId, CurrentSynchs.Count));
                
                if (CurrentSynchs.Contains(userId))
                    CurrentSynchs.Remove(userId);
            }
        }

        /// <summary>
        /// Überprüft, ob der Benutzer in der Session ist
        /// </summary>
        /// <param name="userId">Id des Benutzers</param>
        /// <returns>true, wenn vorhanden</returns>
        public bool IsUserInSession(string userId)
        {
            lock (_lock)
            {
                Log.Info(string.Format("Looking for {0} in session. Count:{1}", userId, CurrentSynchs.Count));
                return CurrentSynchs.Contains(userId);
            }

        }
    }
}