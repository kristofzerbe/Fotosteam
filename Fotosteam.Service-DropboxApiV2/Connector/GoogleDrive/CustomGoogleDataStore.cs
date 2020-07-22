using System;
using System.Linq;
using System.Threading.Tasks;
using Fotosteam.Service.Models;
using Fotosteam.Service.Repository;
using Fotosteam.Service.Repository.Poco;
using Google.Apis.Json;
using Google.Apis.Util.Store;

namespace Fotosteam.Service.Connector.GoogleDrive
{
    /// <summary>
    /// Speichert den Refreshtoken für die Authorizierung des Google-Drives in MemberStoreageAccess.
    /// Die Klasse ist notwendig, damit sicher der Benutzer bei der Synchronisierung nicht erneut bei Google anmelden muss.
    /// </summary>
    public class CustomGoogleDataStore : IDataStore
    {
        private static IDataRepository _repository;

        internal CustomGoogleDataStore(IDataRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Speichert den Token für die Authorisierung in die Datenbank
        /// </summary>
        /// <typeparam name="T">Typ des Wertes der gespichert wird</typeparam>
        /// <param name="key">Code oder Token, je nach Schritt in der Authorisierung</param>
        /// <param name="value">Der Wert der gespeichert werden soll</param>
        /// <returns>Task ohne Rückgabewert</returns>
        public Task StoreAsync<T>(string key, T value)
        {
            var access = GetAccess(key);
            var token = NewtonsoftJsonSerializer.Instance.Serialize(value);
            if (access == null)
            {
                var memberId = GetMemberIdFromKey(key);
                access = new MemberStorageAccess
                {
                    MemberId = memberId,
                    Secret = "x",
                    Token = token,
                    Type = StorageProviderType.GoogleDrive
                };
                _repository.Add(access);
            }
            else
            {
                access.Token = token;
                _repository.Update(access);
            }

            return TaskEx.Delay(0);
        }

        /// <summary>
        /// Löscht die Googlezugriffsdaten aus der Datenbank
        /// </summary>
        /// <typeparam name="T">Typ des Schlüssels</typeparam>
        /// <param name="key">Der Token, der gelöscht werden soll</param>
        /// <returns>Task ohne Rückgabewert</returns>
        // ReSharper disable once UnusedTypeParameter: Der Typparameter, wird vom Interface verlangt, aber hier nicht benötigt
        public Task DeleteAsync<T>(string key)
        {
            var access = new MemberStorageAccess {MemberId = int.Parse(key)};
            _repository.Delete(access);
            return TaskEx.Delay(0);
        }

        /// <summary>
        /// Liefert die Googlezugriffsdaten aus der Datenbank
        /// </summary>
        /// <typeparam name="T">Typ des Schlüssels</typeparam>
        /// <param name="key">Der Token, der gelöscht werden soll</param>
        /// <returns>Task mtt Token-Objekt als Rückgabewert</returns>

        public Task<T> GetAsync<T>(string key)
        {
            var tcs = new TaskCompletionSource<T>();
            var result = GetAccess(key);
            try
            {
                if (result != null)
                {
                    tcs.SetResult(NewtonsoftJsonSerializer.Instance.Deserialize<T>(result.Token));
                }
                else
                {
                    tcs.SetResult(default(T));
                }
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }

        /// <summary>
        /// Macht in diesem Fall nichts.
        /// Könnte eventuell genutzt werden, wenn etwas im Speicher zwischengespeichert würde
        /// </summary>
        /// <returns>Task ohne Rückgabewert</returns>
        public Task ClearAsync()
        {
            return TaskEx.Delay(0);
        }
        /// <summary>
        /// Liefert die Id des Benutzers aus dem Key
        /// </summary>
        /// <param name="key">Der Schlüssel, der die Benutzerinformation enthält</param>
        /// <returns>Id des Benutzers</returns>
        /// <remarks>Die MemberId muss schon bei der Authorizierung mit übergeben worden sein</remarks>
        private int GetMemberIdFromKey(string key)
        {
            key = key.Replace("oauth_", "");
            var memberId = Int32.Parse(key);
            return memberId;
        }

        /// <summary>
        /// Liefert das Zugriffsobjekt zu einem Benutzer
        /// </summary>
        /// <param name="key">Der Schküssel, der die Benutzerinformation enthält</param>
        /// <returns>Das komplette Zugriffobjekt</returns>
        private MemberStorageAccess GetAccess(string key)
        {
            var memberId = GetMemberIdFromKey(key);
            return
                _repository.Queryable<MemberStorageAccess>()
                    .FirstOrDefault(
                        s => s.MemberId == memberId && s.Type == StorageProviderType.GoogleDrive);
        }
    }
}