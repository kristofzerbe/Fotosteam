namespace Fotosteam.Service.Models
{
    /// <summary>
    /// Kapselt die Informaionen, die über Email an einen Benutzer geschickt werden sollen
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Der Betreff der Nachricht. 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Der Inhalt der Nachricht
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Die Emailadress des Absender der Nachricht
        /// </summary>
        public string SenderEmail { get; set; }
        /// <summary>
        /// Der Name des Absenders der Nachricht
        /// </summary>
        public string SenderName { get; set; }

    }
}