using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Fotosteam.Service.Controller;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository.Poco;
using log4net;
using Microsoft.AspNet.SignalR;

namespace Fotosteam.Service.Hub
{
    /// <summary>
    /// Der Hub stellt die Kommunikation mit Clients her.
    /// Es wird intern ein Dictionary verwaltet, um die Verbindung zu einzelnen Benutzer zuordnen können.
    /// Die Kommunikation soll nur für angemeldete und authorisierte Benutzer erfolgen
    /// </summary>
    public class NotificationHub : Microsoft.AspNet.SignalR.Hub
    {
        private static  Dictionary<int, List<string>> _connections = new Dictionary<int, List<string>>();
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        internal static List<string> GetConnectionId(int memberId)
        {
            List<string> ids;
            _connections.TryGetValue(memberId, out ids);
            return ids;
        }

        /// <summary>
        /// Reagiert auf die Herstellung der Verbindung vom Browser des Benutzers
        /// </summary>
        [Authorize]
        public override Task OnConnected()
        {
            var memberId = GetMemberId();
            List<string> connections;
            if(!_connections.TryGetValue(memberId, out connections))
            {
                connections = new List<string>();
                _connections.Add(memberId, connections);
            }
            if (!connections.Contains(Context.ConnectionId))
                connections.Add(Context.ConnectionId);

            return base.OnConnected();
        }

        private int GetMemberId()
        {
            int memberId;
            using (var accountController = new AccountController())
            {
                memberId = accountController.GetMemberFromAuthenticatedUser().Id;
            }
            return memberId;
        }
        /// <summary>
        /// Reagiert auf den Abbruch der Verbindung, damit das Dictionary mit den möglichen
        /// Verbindungen aktualisiert wird
        /// </summary>
        /// <param name="stopCalled">Kann verwendet werden, um über die Oberfläche die Verbindung zu kappen</param>
        [Authorize]
        public override Task OnDisconnected(bool stopCalled)
        {
            var memberId = GetMemberId();
            List<string> connections;
            if (_connections.TryGetValue(memberId, out connections))
            {
                if (connections.Contains(Context.ConnectionId))
                    connections.Remove(Context.ConnectionId );

                if (connections.Count == 0)
                    _connections.Remove(memberId);
            }
            
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Erlaubt das Senden einer Meldung vom angemeldeten Benutzer 
        /// an den ihn selber. Ist nur für Testzwecke sinnvoll
        /// </summary>
        /// <param name="notification">Objekt mit der Meldung, die angezeigt werden soll</param>
        public void SendNotification(Notification notification)
        {
            var memberId = GetMemberId();
            var connections =_connections[memberId];
            foreach (var id in connections)
            {
                Clients.Client(id).showNotification(notification);
            }
        }

        /// <summary>
        /// Testmethode, die es erlaubt an alle verbundenen Mitgliedern eine
        /// Nachricht zu vercshicken
        /// </summary>
        /// <param name="message">Die Nachricht, die gesendet werden soll</param>
        /// <param name="type">Die Art der Nachricht <see cref="NotificationType"/></param>
        public void SendDummyMessage(string message,NotificationType type)
        {
            object data;
            if (type == NotificationType.DropboxSynchronization )
            {
                data = new SynchProgress()
                {
                    Index = 1,
                    Photo = new Photo() { Name="neuesbild.jpg"}
                };
            }
            else
            {
                data = message;
            }
            var toSend = new Notification() { Data = data, Type = type,UserAlias ="Dummy" };

            Clients.All.showNotification(toSend);
        }

        internal static void PushNotification(Notification notification)
        {
            try
            {
                if (notification.Data is SynchProgress)
                {
                    var progress = (SynchProgress) notification.Data;
                    if (progress.Photo != null)
                    {
                        if (progress.Photo.Member != null && progress.Photo.Member.Photos != null)
                            progress.Photo.Member.Photos = null;
                    }
                }

                var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                
                var ids = GetConnectionId(notification.MemberId);
                if (ids != null)
                {
                    if (string.IsNullOrEmpty(notification.UserAlias))
                    {
                        notification.UserAlias = "Unknown";
                    }
                    foreach (var id in ids)
                    {
                        context.Clients.Client(id).showNotification(notification);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
