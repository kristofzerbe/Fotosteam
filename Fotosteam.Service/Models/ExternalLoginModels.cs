using System.ComponentModel.DataAnnotations;

namespace Fotosteam.Service.Models
{
    /// <summary>
    ///     Kapselt die Daten, die für eine externe Registrierung notwendig sind
    /// </summary>
    public class RegisterExternalBindingModel
    {
        /// <summary>
        ///     Der Benutzername
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        ///     Der Alias, der innerhalb der Anwendung verwendet werden soll
        /// </summary>
        [Required]
        public string Alias { get; set; }

        /// <summary>
        ///     Die Emailadresse des Benutzers
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        ///     Der Name des Authentifzierungsproviders
        ///     zuläassig sind Google, Microsoft, Twitter und Facebook
        /// </summary>
        [Required]
        public string Provider { get; set; }

        /// <summary>
        ///     Der Zugriffsschlüssel, der nach der Authentifizierung durch den externen Provider geliefert wurde
        /// </summary>
        public string ExternalAccessToken { get; set; }
         
        /// <summary>
        /// Sprache des Benutzers
        /// </summary>
        public MemberLanguage Language { get; set; }
    }

    /// <summary>
    ///     Parsed die Informationen, die im externen Zugriffsschlüssel enthalten sind
    /// </summary>
    public class ParsedExternalAccessToken
    {
        /// <summary>
        /// UserId vom Provider
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// AppId beim Provider
        /// </summary>
        public string AppId { get; set; }
    }
}