using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Fotosteam.Service.Repository.Context;
using Fotosteam.Service.Repository.Poco;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Fotosteam.Service.Repository
{
    /// <summary>
    ///     Definiert den Datenzugriff für die Authentifzierung
    /// </summary>
    public class AuthRepository : IDisposable, IAuthRepository
    {
        private readonly AuthContext _currentContext;
        private readonly UserManager<IdentityUser> _currentUserManager;

        /// <summary>
        ///     Standardkonstruktor, der den Datencontainer setzt und mögliches SQL-Logging definiert
        /// </summary>
        public AuthRepository()
        {
            _currentContext = new AuthContext();
            _currentUserManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_currentContext));
        }

        /// <summary>
        ///     Fügt den Refreshtoken dem aktuellen Authentifizierungskontext hinzu
        /// </summary>
        /// <param name="token">Der hinzuzufügende Token</param>
        /// <returns>Task mit Wahr/Falsch</returns>
        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var existingToken =
                _currentContext.RefreshTokens.SingleOrDefault(
                    r => r.Subject == token.Subject && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                await RemoveRefreshToken(existingToken);
            }

            _currentContext.RefreshTokens.Add(token);

            return await _currentContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        ///     Entfernt den Refreshtoken dem aktuellen Authentifizierungskontext
        /// </summary>
        /// <param name="refreshTokenId">Die ID des zu löschenden Token</param>
        /// <returns>Task mit Wahr/Falsch</returns>
        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _currentContext.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _currentContext.RefreshTokens.Remove(refreshToken);
                return await _currentContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        /// <summary>
        ///     Entfernt den Refreshtoken dem aktuellen Authentifizierungskontext
        /// </summary>
        /// <param name="refreshToken">Der zu löschende Token</param>
        /// <returns>Task mit Wahr/Falsch</returns>
        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _currentContext.RefreshTokens.Remove(refreshToken);
            return await _currentContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        ///     Ermittelt den Refreshtoken dem aktuellen Authentifizierungskontext
        /// </summary>
        /// <param name="refreshTokenId">Die ID des Token</param>
        /// <returns>Task mit Toke</returns>
        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _currentContext.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        /// <summary>
        ///     Liefert alle Refreshtokens zurück
        /// </summary>
        /// <returns>Liste aller Refreshtokens</returns>
        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _currentContext.RefreshTokens.ToList();
        }

        /// <summary>
        ///     Ermittelt eine internen Nutzer, der an einen Benutzer (Member) gekoppelt ist
        /// </summary>
        /// <param name="loginInfo">UserLoginInfo die zum Finden des Benutzers genutz werden soll</param>
        /// <returns>Task mit dem gefundenen Nutzer</returns>
        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            return await _currentUserManager.FindAsync(loginInfo);
        }

        /// <summary>
        ///     Ermittelt eine internen Nutzer, der an einen Benutzer (Member) gekoppelt ist
        /// </summary>
        /// <param name="loginInfo">UserLoginInfo die zum Finden des Benutzers genutz werden soll</param>
        /// <returns>Task mit dem gefundenen Nutzer</returns>
        public IdentityUser Find(UserLoginInfo loginInfo)
        {
            return _currentUserManager.Find(loginInfo);
        }

        public IdentityResult Delete(IdentityUser user)
        {
            return _currentUserManager.Delete(user);
        }

        /// <summary>
        ///     Ermittelt einen Nutzer anhand des Namens (ist gleich der Emailadresse)
        /// </summary>
        /// <param name="userName">Die Emailadresse des Benutzers</param>
        /// <returns>IdentityUser des Benutzers</returns>
        public IdentityUser FindByName(string userName)
        {
            return _currentUserManager.FindByName(userName);
        }

        /// <summary>
        ///     Ermittelt einen Nutzer anhand der Emailadresse)
        /// </summary>
        /// <param name="email">Die Emailadresse des Benutzers</param>
        /// <returns>IdentityUser des Benutzers</returns>
        public IdentityUser FindByEmail(string email)
        {
            return _currentUserManager.FindByEmail(email);
        }

        /// <summary>
        ///     Erzeugt einen neuen Benutzer
        /// </summary>
        /// <param name="user">IdentityUser für den anzulegenden Benutzer</param>
        /// <returns>Task mit dem Ergebnis des Anlegens</returns>
        public async Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            var result = await _currentUserManager.CreateAsync(user);

            return result;
        }

        /// <summary>
        ///     Erzeugt einen neuen Benutzer
        /// </summary>
        /// <param name="user">IdentityUser für den anzulegenden Benutzer</param>
        /// <param name="authenticationType">Die Art der Authentifizierung, z.B. Google, Microsoft</param>
        /// <returns>Task mit der ClaimsIdentity des Benutzers</returns>
        public async Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string authenticationType)
        {
            var result = await _currentUserManager.CreateIdentityAsync(user, authenticationType);
            return result;
        }

        /// <summary>
        ///     Erzeugt die Login-Informatioen für einen neuen Benutzer
        /// </summary>
        /// <param name="userId">Id des neuen Benutzer</param>
        /// <param name="login">UserLoginInfo des neuen Benutezrs</param>
        /// <returns>Task mit dem Ergebnis des Anlegens</returns>
        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _currentUserManager.AddLoginAsync(userId, login);

            return result;
        }

        /// <summary>
        ///     Löst vorhandene externe Referenzen auf
        /// </summary>
        public void Dispose()
        {
            _currentContext.Dispose();
            _currentUserManager.Dispose();
        }
    }
}